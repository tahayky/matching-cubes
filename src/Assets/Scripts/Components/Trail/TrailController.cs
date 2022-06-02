using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailController : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public CubeColor color;
    [HideInInspector]
    public TrailRenderer trail_renderer;
    #endregion
    private void Awake()
    {
        trail_renderer = GetComponent<TrailRenderer>();
    }

    #region Commands
    public void Stop()
    {
        trail_renderer.time = Mathf.Infinity;
    }
    #endregion
}
