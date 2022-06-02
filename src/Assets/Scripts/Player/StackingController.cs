using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using MEC;
using DG.Tweening;
public class StackingController : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private Player player;
    [SerializeField]
    private GameObject trail_prefab;
    private List<Cube> cubes;
    private TrailController current_trail;
    private GameManager game_manager;
    #endregion
    private void Awake()
    {
        game_manager = player.game_manager;
        cubes = new List<Cube>();
    }
    private void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
    }

    #region IEnumerators
    public IEnumerator<float> Explode()
    {
        Cube[] matchings = Matchings();
        Refresh();
        yield return Timing.WaitForSeconds(.2f);
        //if (similars.Length > 0)
        if (matchings!=null)
        {
            ExplosionAnimation(matchings);
            yield return Timing.WaitForSeconds(.2f);
            CheckMatchings();
        }
    }
    public IEnumerator RefreshColors(List<CubeColor> _colors)
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            Sequence sequence = DOTween.Sequence()
            .Append(cubes[i].transform.DOScale(cubes[i].transform.localScale + Vector3.one / 2, .025f))
            .Append(cubes[i].transform.DOScale(cubes[i].transform.localScale, .025f))
            .Join(cubes[i].transform.DORotateQuaternion(Quaternion.Euler(0, cubes[i].transform.localRotation.eulerAngles.y + 180, 0), .05f))
            .AppendCallback(delegate {
                cubes[i].SetColor(_colors[i]);
                cubes[i].UpdateColor();
            });
            yield return sequence.WaitForCompletion();
        }
        CheckMatchings();
    }
    #endregion
    #region Methods
    private Cube[] Matchings()
    {
        List<Cube> result = new List<Cube>();
        for (int i = 0; i < cubes.Count; i++)
        {
            if (result.Count == 0)
            {
                result.Add(cubes[i]);
                continue;
            }

            if (!cubes[i].color.Equals(result.Last().color))
            {
                result.Clear();
            }

            result.Add(cubes[i]);

            if (result.Count == 3)
            {
                return result.ToArray();
            }
        }
        return null;
    }
    private Cube[] Similars()
    {
        List<Cube> result = new List<Cube>();
        List<Cube> triple = new List<Cube>();
        for (int i = 0; i < cubes.Count; i++)
        {
            if (triple.Count == 0)
            {
                triple.Add(cubes[i]);
                continue;
            }

            if (!cubes[i].color.Equals(triple.Last().color))
            {
                if (triple.Count == 3)
                {
                    result.AddRange(triple);
                    return result.ToArray();
                }
                triple.Clear();
            }

            triple.Add(cubes[i]);

            if (i == cubes.Count - 1)
            {
                if (triple.Count == 3)
                {
                    result.AddRange(triple);
                    return result.ToArray();
                }
            }
        }
        return result.ToArray();
    }
    public int GetCubeCount()
    {
        return cubes.Count;
    }
    #endregion
    #region Commands
    public void Refresh()
    {
        player.Leap(cubes.Count, Constants.cube_size);

        if (cubes.Count == 0)
        {
            DropTrail();

            return;
        }

        for (int i = 0; i < cubes.Count; i++)
        {
            if((cubes.Count - i) < cubes[i].order)
            {
                cubes[i].transform.DOLocalMoveY((cubes.Count - i - 1) * Constants.cube_size, 0.1f).SetEase(Ease.InSine);
            }
            else if ((cubes.Count - i) > cubes[i].order)
            {
                float multiply = 1f - Mathf.Sqrt(1f - Mathf.Pow((float)(cubes.Count - i) / (float)cubes.Count, 2));
                DOTween.Sequence()
                .Append(cubes[i].transform.DOLocalMoveY((cubes.Count - i - 1) * Constants.cube_size + multiply, 0.1f).SetEase(Ease.OutSine))
                .Append(cubes[i].transform.DOLocalMoveY((cubes.Count - i - 1) * Constants.cube_size, 0.1f).SetEase(Ease.InSine));
            }
            cubes[i].order = cubes.Count - i;
        }

        if(player.is_grounded) SetTrail(cubes[cubes.Count-1].color);
    }
    public List<CubeColor> SortBy(SortingType _type)
    {
        List<CubeColor> _colors = cubes.Select<Cube, CubeColor>(item => item.color).ToList();

        switch (_type)
        {
            case SortingType.Color:
                _colors.Sort();
                break;
            case SortingType.Random:
                _colors.Shuffle();
                break;
        }
        return _colors;
    }
    public void CheckMatchings()//Eşleşmeleri kontrol eden coroutine'i başlatır
    {
        Timing.KillCoroutines("ex");
        Timing.RunCoroutine(Explode(),"ex");
    }
    public void CollectCube(Transform cube)
    {
        cube.SetParent(this.transform);
        cube.GetComponent<Collider>().enabled = false;
        cube.transform.localPosition = Vector3.zero;
        
        cubes.Add(cube.GetComponent<Cube>());

        cube.GetComponent<CubeAnimations>().CollectingAnimation();

        Refresh();

        if(cubes.Count>2)
            CheckMatchings();
    }
    private void ExplosionAnimation(Cube[] matchings)
    {
        for(int i=0;i<matchings.Length;i++)
        {
            cubes.Remove(matchings[i]);

            matchings[i].GetComponent<CubeAnimations>().Explode();
        }
    }
    public void RemoveAll()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            cubes[i].transform.parent = game_manager.current_level_go.transform;
        }
        cubes.Clear();

        Refresh();
    }
    public void DropCubes(int count)
    {
        for (int i=0;i<count;i++)
        {
            cubes[cubes.Count - 1].transform.parent = game_manager.current_level_go.transform;
            cubes.Remove(cubes[cubes.Count - 1]);
        }

        Refresh();
    }
    public void LoseCube()
    {
        DropTrail();
        Cube last_cube = cubes[cubes.Count - 1];
        last_cube.transform.parent = game_manager.current_level_go.transform;
        cubes.Remove(last_cube);
        last_cube.transform.DOMove(last_cube.transform.position + player.transform.forward*5 + Vector3.down*2, 0.1f).SetEase(Ease.Linear);
        Refresh();
    }
    public void StopTrail()
    {
        if (current_trail != null)
        {
            current_trail.Stop();
        }
    }
    public void DropTrail()
    {
        if (current_trail != null)
        {
            current_trail.transform.parent = game_manager.current_level_go.transform;
            current_trail = null;
        }
    }
    public void StartTrail()
    {
        SetTrail(cubes[cubes.Count - 1].color);
    }

    private void SetTrail(CubeColor _cube_color)
    {
        if (current_trail != null) 
        {
            if (current_trail.color == _cube_color) return;
            else DropTrail();
        }
        

        GameObject trail_go = Instantiate(trail_prefab.gameObject, this.transform);
        TrailController trail = trail_go.GetComponent<TrailController>();
        trail.color = _cube_color;
        current_trail = trail;

        switch (_cube_color)
        {
            case CubeColor.Blue:
                trail.trail_renderer.sharedMaterial = (Material)Resources.Load("TrailBlue", typeof(Material));
                break;
            case CubeColor.Orange:
                trail.trail_renderer.sharedMaterial = (Material)Resources.Load("TrailOrange", typeof(Material));
                break;
            case CubeColor.Purple:
                trail.trail_renderer.sharedMaterial = (Material)Resources.Load("TrailPurple", typeof(Material));
                break;
        }
        
    }
    #endregion
}
