using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

[RequireComponent(typeof(Player))]
public class PathFollower : MonoBehaviour
{
    #region Variables
    private PathCreator pathCreator;
    private Player player;
    private float speed, dstTravelled;
    private bool active = false;
    #endregion
    private void Awake()
    {
        player = GetComponent<Player>();
    }
    private void Update()
    {
        if (active)
        {
            dstTravelled += speed * Time.deltaTime;
            Vector3 point_at_distance = pathCreator.path.GetPointAtDistance(dstTravelled, EndOfPathInstruction.Stop);
            player.SetPosition(point_at_distance+Vector3.up/3);

            if(dstTravelled>= pathCreator.path.length)
            {
                active = false;
                player.Resume();
            }
        }
    }
    #region Commands
    public void Follow(OverPass overpass)
    {
        speed = player.speed;
        dstTravelled = 0;
        pathCreator = overpass.path_creator;
        player.Pause();
        active = true;
    }
    #endregion
}
