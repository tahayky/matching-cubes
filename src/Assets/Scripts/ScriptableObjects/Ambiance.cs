using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
[CreateAssetMenu(fileName ="Ambiance",menuName = "Custom/Ambiance")]
public class Ambiance : ScriptableObject
{
    #region Variables
    [SerializeField]
    private Material skybox;
    [SerializeField]
    private Material building_material;
    [SerializeField]
    private Material lower_path_material;
    [SerializeField]
    private Color building_color;
    [SerializeField]
    private Color path_lower_color;
    [SerializeField]
    private Color sky_top_color;
    [SerializeField]
    private Color sky_bottom_color;
    #endregion
    #region Commands
    public void View()
    {
        building_material.SetColor("_Color", building_color);
        skybox.SetColor("_Top", sky_top_color);
        skybox.SetColor("_Bottom", sky_bottom_color);
        lower_path_material.SetColor("_Color", path_lower_color);
    }
    #endregion

}
#if UNITY_EDITOR
[CustomEditor(typeof(Ambiance))]
public class ObjectBuilderEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        Ambiance ambiance = (Ambiance)target;
        if (GUILayout.Button("View"))
        {
            ambiance.View();
        }
    }
}
#endif