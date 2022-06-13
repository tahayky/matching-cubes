using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
[RequireComponent(typeof(PathFollower))]
[RequireComponent(typeof(StackingManager))]
public class Player : Singleton<Player>
{
    #region Variables
    public GameObject armature;
    public PlayerPhysics player_physics;
    [HideInInspector]
    public StackingManager stacking_manager;
    [HideInInspector]
    public PathFollower path_follower;
    [HideInInspector]
    public float speed;
    [SerializeField]
    private float
        turbo_speed,
        turbo_duration;
    [HideInInspector]
    public bool turbo = false;
    [HideInInspector]
    public bool is_grounded = false;
    [HideInInspector]
    public bool active = false;
    private PlayerMovement player_movement;
    private Animator animator;
    private PlayerInput player_input;
    private int order = 0;
    #endregion
    private void Awake()
    {
        stacking_manager = GetComponent<StackingManager>();
        animator = armature.GetComponent<Animator>();
        path_follower = GetComponent<PathFollower>();
        player_movement = GetComponent<PlayerMovement>();
    }
    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Default);

        player_physics.ReSizeCollider(Constants.player_height);
    }
    #region IEnumerators
    public IEnumerator<float> TurboTimer()
    {
        yield return Timing.WaitForSeconds(turbo_duration);
        DOTween.To(() => Constants.turbo_fow, x => GameManager.Instance.cam.m_Lens.FieldOfView = x, Constants.default_fow, 1f);
        player_movement.SetSpeed(null);
        turbo = false;
    }
    #endregion

    #region Commands
    public void Turbo()
    {
        DOTween.To(()=>Constants.default_fow, x=> GameManager.Instance.cam.m_Lens.FieldOfView=x, Constants.turbo_fow, 1f);
        player_movement.SetSpeed(turbo_speed);
        turbo = true;
        Timing.RunCoroutine(TurboTimer(),"turbo");
    }
    public void EndTurbo()
    {
        player_movement.SetSpeed(null);
        turbo = false;
        Timing.KillCoroutines("turbo");
    }
    public void Activate()
    {
        animator.ResetTrigger("reset");
        animator.ResetTrigger("fall");
        Resume();
        animator.SetTrigger("run");
    }
    public void Deactivate()
    {
        animator.ResetTrigger("run");
        animator.ResetTrigger("fall");
        Pause();
        animator.SetTrigger("reset");
    }
    public void GoStart()
    {
        stacking_manager.RemoveAll();
        transform.position = Vector3.zero;
        Deactivate();
    }

    public void Fall()
    {
        animator.ResetTrigger("run");
        animator.ResetTrigger("reset");
        Pause();
        animator.SetTrigger("fall");
        GameManager.Instance.LevelFailed();
        stacking_manager.StopTrail();
    }
    public void Jump()
    {
        player_movement.Jump();
    }
    public void Pause()
    {
        active = false;
    }
    public void Resume()
    {
        active = true;
    }
    public void Leap(int _cubes_count,float _cube_height)
    {
        if (_cubes_count < order)
        {
            armature.transform.DOLocalMoveY(_cubes_count * _cube_height, 0.1f).SetEase(Ease.InSine);
        }
        else if (_cubes_count > order)
        {
            DOTween.Sequence()
            .Append(armature.transform.DOLocalMoveY(_cubes_count*_cube_height + 2, 0.1f).SetEase(Ease.OutSine))
            .Append(armature.transform.DOLocalMoveY(_cubes_count*_cube_height, 0.1f).SetEase(Ease.InSine));
        }

        animator.SetInteger("order", order = _cubes_count);

        player_physics.ReSizeCollider(_cubes_count * _cube_height + Constants.player_height);

    }
    #endregion
}
