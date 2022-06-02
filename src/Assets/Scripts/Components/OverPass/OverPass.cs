using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;
public class OverPass : MonoBehaviour
{
    #region Variables
    public PathCreator path_creator;
    #endregion

    private void Start()
    {

        PathCreation.Examples.RoadMeshCreator road = path_creator.gameObject.GetComponent<PathCreation.Examples.RoadMeshCreator>();
        road.TriggerUpdate();
    }
}
