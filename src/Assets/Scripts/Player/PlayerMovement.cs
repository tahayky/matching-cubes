using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
[RequireComponent(typeof(Player))]
public class PlayerMovement : MonoBehaviour
{
    #region Variables
    [HideInInspector]
    public float speed;
    [SerializeField]
    private float
    min_speed,
    jump_height,
    jump_duration,
    range,
    smoothness_speed;
    private Player player;
    private Transform armature;
    private Transform model;
    private PlayerInput player_input;
    #endregion
    private void Awake()
    {
        player = GetComponent<Player>();
        armature = player.armature.transform;
        model = player.player_physics.transform;
    }
    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Default);
        player_input = new PlayerInput();
        speed = min_speed;
    }
    private void Update()
    {
        if (!player.active)
            return;

        float move_aim = player_input.GetHorizontal(range);
        float smooth_position = Mathf.Lerp(model.transform.localPosition.x, move_aim, Time.deltaTime * smoothness_speed);
        Vector3 new_position = new Vector3(smooth_position, model.transform.localPosition.y) + Vector3.forward * speed * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, new_position.z));

        model.transform.localPosition =
            new Vector3(
                new_position.x
                , new_position.y);
    }
    #region Commands
    public void Translate(Vector3 new_position)
    {
        player.transform.position = new Vector3(0, 0, new_position.z);

        model.transform.localPosition =
            new Vector3(
                new_position.x
                , new_position.y);

    }
    public void SetSpeed(float? speed)
    {
        if(speed.HasValue)
        {
            this.speed = speed.Value;
        }
        else
        {
            this.speed = min_speed;
        }
    }
    public void Jump()
    {
        DOTween.Sequence()
            .AppendCallback(delegate { player.stacking_manager.DropTrail(); player.is_grounded = false; })
            .Append(model.transform.DOLocalMoveY(jump_height, jump_duration / 2).SetEase(Ease.OutCirc))
            .Append(model.transform.DOLocalMoveY(0, jump_duration / 2).SetEase(Ease.InCirc))
            .AppendCallback(delegate { player.stacking_manager.StartTrail(); player.is_grounded = true; });
    }
    #endregion
}
