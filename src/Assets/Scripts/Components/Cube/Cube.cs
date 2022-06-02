using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    #region Variables
    public CubeColor color = CubeColor.Blue;
    [HideInInspector]
    public int order=-1;
    [HideInInspector]
    public Renderer cube_renderer;
    #endregion

    private void Awake()
    {
        cube_renderer = GetComponent<Renderer>();
    }

    void Start()
    {
        UpdateColor();
    }

    private void OnValidate()
    {
        if(cube_renderer == null) cube_renderer = GetComponent<Renderer>();
        UpdateColor();
    }
    #region Commands
    public void UpdateColor()
    {
        switch (color)
        {
            case CubeColor.Blue:
                cube_renderer.sharedMaterial = (Material)Resources.Load("CubeBlue", typeof(Material));
                break;
            case CubeColor.Purple:
                cube_renderer.sharedMaterial = (Material)Resources.Load("CubePurple", typeof(Material));
                break;
            case CubeColor.Orange:
                cube_renderer.sharedMaterial = (Material)Resources.Load("CubeOrange", typeof(Material));
                break;

        }
    }
    public void SetColor(CubeColor _color)
    {
        color = _color;
    }
    #endregion
}
