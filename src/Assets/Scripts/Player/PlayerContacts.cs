using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MEC;
[RequireComponent(typeof(Player))]
public class PlayerContacts : MonoBehaviour
{
    #region Variables
    private Player player;
    private StackingManager stacking_controller;
    private PlayerPhysics player_physics;
    private PathFollower path_follower;
    #endregion
    private void Awake()
    {
        player = GetComponent<Player>();
        stacking_controller = GetComponent<StackingManager>();
        player_physics = player.player_physics;
        path_follower = player.path_follower;
    }
    #region Commands
    public void CubeContact(Transform cube_transform)
    {
        stacking_controller.CollectCube(cube_transform);
    }
    public void GateContact(Gate gate)
    {
        List<CubeColor> _colors = stacking_controller.SortBy(gate.Pass());
        StartCoroutine(stacking_controller.RefreshColors(_colors));
    }
    public void RampContact()
    {
        player.Jump();
    }
    public void FinishContact()
    {
        player.Deactivate();
        GameManager.Instance.LevelCompleted();
        stacking_controller.StopTrail();
    }
    public void ObstacleContact(GameObject obstacle_go)
    {
        Obstacle obstacle = obstacle_go.GetComponent<Obstacle>();

        int obstacle_row_count = obstacle.GetRow(player_physics.transform.position);

        int cube_count = stacking_controller.GetCubeCount();
        if (obstacle_row_count != 0&&player.turbo)
        {
            obstacle.Break();
            return;
        }

        if (cube_count >= obstacle_row_count)
            stacking_controller.DropCubes(obstacle.GetRow(player_physics.transform.position));
        else
            player.Fall();
        
    }
    public void TurboContact()
    {
        player.Turbo();
    }
    public void OverpassContact(OverPass overpass)
    {
        path_follower.Follow(overpass);
    }
    public void HoleContact()
    {
        player.EndTurbo();
        int cube_count = stacking_controller.GetCubeCount();

        if (cube_count > 0)
            stacking_controller.LoseCube();
        else
            player.Fall();
    }
    #endregion
}
