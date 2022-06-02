using UnityEngine;

public class CityGenerator : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private int count=20;
    [SerializeField]
    private float height_range=40;
    [SerializeField]
    private float deflection_range=70;
    [SerializeField]
    private float interval=20;
    [SerializeField]
    private GameObject building_prefab;
    #endregion
    private void Start()
    {
        for (int i=0;i<count;i++)
        {
            GameObject new_building = Instantiate(building_prefab, this.transform);
            float random_height = Random.Range(-height_range / 2, height_range / 2);
            float deflection = Random.Range(-deflection_range / 2, deflection_range / 2);
            float size = Random.Range(1, 2);
            new_building.transform.localPosition = new Vector3(deflection, random_height, i*interval);
            new_building.transform.localScale *= size;
        }
    }

}
