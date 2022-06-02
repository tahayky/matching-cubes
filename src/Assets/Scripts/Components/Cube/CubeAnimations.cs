using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
[RequireComponent(typeof(Cube))]
public class CubeAnimations : MonoBehaviour
{
    #region Variables
    private Cube cube;
    #endregion
    private void Start()
    {
        DOTween.Init(true,true,LogBehaviour.Default);

        cube = GetComponent<Cube>();
    }
    #region Commands
    public void CollectingAnimation()
    {
        //Küp elastik efekti
         DOTween.Sequence()
        .Append(transform.DOScale(transform.localScale + Vector3.one, 0.1f))
        .Append(transform.DOScale(transform.localScale, 0.1f));
    }

    public void Explode()
    {
        GameObject cube_go = this.gameObject;

        Material backup_mat = cube.cube_renderer.material;

        Material explosion_mat = (Material)Resources.Load("Explosion", typeof(Material));

        DOTween.Sequence()
       .Append(backup_mat.DOColor(explosion_mat.color, 0.1f))
       .Append(cube.transform.DOScale(0.1f, 0.1f))
       .AppendCallback(delegate {
           Destroy(cube_go);
       });
    }

    #endregion
}
