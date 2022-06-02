using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
public class PathBuilder : MonoBehaviour
{
    #region Variables
    [SerializeField]
    private GameObject path_prefab;
    [SerializeField]
    private GameObject finish_prefab;
    [SerializeField]
    private GameObject building_prefab;
    [SerializeField]
    private Ambiance ambiance;
    [SerializeField]
    private Part[] parts;
    #endregion
    private void Start()
    {
        ambiance.View();
    }
    private void OnValidate()
    {
        if (this.gameObject.activeInHierarchy)
        {
            Build();
        }
    }
    #region Commands
    private void Build()
    {
        //Eski yollarý siler
        for (int i = 0; i < this.transform.childCount; i++)
        {
            if (Application.isPlaying) Destroy(this.transform.GetChild(i).gameObject);
            else StartCoroutine(DestroyGO(this.transform.GetChild(i).gameObject));
        }

        //Yeni yollar oluþturur
        float current_pos = 0;
        bool gap = false;
        foreach (Part part in parts)
        {
            switch (part)
            {
                case Part.Path:
                    GameObject new_path_go = Instantiate(path_prefab, this.transform);
                    new_path_go.transform.localPosition = new Vector3(0, 0, current_pos);

                    //Gerek yoksa uçurumu siler
                    if (!gap)
                    {
                        if (Application.isPlaying) Destroy(new_path_go.transform.Find("LowerPartThin").gameObject);
                        else StartCoroutine(DestroyGO(new_path_go.transform.Find("LowerPartThin").gameObject));
                    }
                    else gap = false;
                    break;
                case Part.Gap:
                    gap = true;
                    break;
                case Part.Finish:
                    GameObject new_finish_go = Instantiate(finish_prefab, this.transform);
                    new_finish_go.transform.localPosition = new Vector3(0, 0, current_pos);

                    //Gerek yoksa uçurumu siler
                    if (!gap)
                    {
                        if (Application.isPlaying) Destroy(new_finish_go.transform.Find("LowerPartThin").gameObject);
                        else StartCoroutine(DestroyGO(new_finish_go.transform.Find("LowerPartThin").gameObject));
                    }
                    else gap = false;
                    break;
            }
            current_pos += Constants.path_prefab_lenght;
        }
    }
    #endregion
    #region IEnumerators
    IEnumerator DestroyGO(GameObject go)
    {
        yield return new WaitForSeconds(0);
        DestroyImmediate(go);
    }
    #endregion
}