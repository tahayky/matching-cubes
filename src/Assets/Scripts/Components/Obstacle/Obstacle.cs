using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Array2DEditor;
using DG.Tweening;
[RequireComponent(typeof(BoxCollider))]
public class Obstacle : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject obstacle_cube_prefab;
    [SerializeField]
    private Array2DBool matrix;
    private BoxCollider box_collider;
    #endregion
    private void Awake()
    {
        box_collider = GetComponent<BoxCollider>();
    }
    private void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Default);

        box_collider.size = new Vector3(Constants.cube_size*matrix.GridSize.x, box_collider.size.y, Constants.cube_size);
        box_collider.center = new Vector3(Constants.cube_size * matrix.GridSize.x/2, box_collider.center.y);
    }
    private void OnValidate()
    {
        if (this.gameObject.activeInHierarchy)
        {
            Build();
        }
    }
    #region IEnumerators
    IEnumerator DestroyGO(GameObject go)
    {
        yield return new WaitForSeconds(0);
        DestroyImmediate(go);
    }
    #endregion
    #region Methods
    public int GetRow(Vector3 position)
    {
        List<float> distances = new List<float>();
        for (int i=0;i<matrix.GridSize.x;i++)
        {
            Vector2 column_pos = new Vector2(Constants.cube_size * (i + .5f)+ transform.position.x, transform.position.z);
            Vector2 target_pos = new Vector2(position.x, position.z);
            float distance = Vector2.Distance(column_pos,target_pos);
            if (!matrix.GetCell(i, matrix.GridSize.y-1)) distance *= 1.5f;
            distances.Add(distance);
        }

        int column = distances.IndexOf(distances.Min());
        return matrix.GetColumnCount(column, item => item);
    }
    #endregion
    #region Commands
    public void Build()
    {
        //Eski küp engelleri siler
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (Application.isPlaying) Destroy(this.transform.GetChild(i).gameObject);
            else StartCoroutine(DestroyGO(this.transform.GetChild(i).gameObject));
        }

        for (int x = 0; x < matrix.GridSize.x; x++)
        {
            for (int y = 0; y < matrix.GridSize.y; y++)
            {
                bool cell_value = matrix.GetCell(x, y);
                if (cell_value)
                {
                    GameObject new_obstacle_cube = Instantiate(obstacle_cube_prefab, this.transform);
                    new_obstacle_cube.transform.localPosition = new Vector3(x * Constants.cube_size + Constants.cube_size / 2, -Constants.cube_size * y);
                }
            }
        }
    }
    public void Break()
    {
        for (int i=0;i<transform.childCount;i++)
        {
            Transform child_transform = transform.GetChild(i);
            Vector3 center = transform.position+ box_collider.center;
            Vector3 direction_vector = child_transform.position-center;
            direction_vector = direction_vector.normalized * 4;
            direction_vector += child_transform.position;

            DOTween.Sequence()
                .Append(child_transform.DOMove(direction_vector+(transform.up-transform.forward)*4, 1).SetEase(Ease.OutQuint))
                .Join(child_transform.DORotateQuaternion(Quaternion.Euler(90,90,90), 0.5f).SetLoops(2));
        }
    }
    #endregion

}
