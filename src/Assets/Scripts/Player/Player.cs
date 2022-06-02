using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using MEC;
[RequireComponent(typeof(PathFollower))]
public class Player : MonoBehaviour
{
    #region Variables
    public GameManager game_manager;
    public PlayerPhysics player_physics;
    public StackingController stacking_controller;
    [HideInInspector]
    public PathFollower path_follower;
    [HideInInspector]
    public float speed;
    [SerializeField]
    private float min_speed,
        turbo_speed,
        turbo_duration,
        jump_height,
        jump_duration,
        range,
        sensivity,
        smoothness_speed;
    [HideInInspector]
    public bool turbo = false,
        is_grounded = true;
    [SerializeField]
    private GameObject armature;
    [SerializeField]
    private Cinemachine.CinemachineVirtualCamera cam;
    private Animator animator;
    private bool active = false;
    private float move_aim=0;    //hedeflenen X pozisyonu
    private int order = 0;
    #endregion
    private void Awake()
    {
        animator = armature.GetComponent<Animator>();
        path_follower = GetComponent<PathFollower>();
    }
    void Start()
    {
        DOTween.Init(true, true, LogBehaviour.Default);

        player_physics.ReSizeCollider(Constants.player_height);

        speed = min_speed;
    }

    void Update()
    {
        if (!active)
            return;

        //Hedef x pozisyonunu saða sola kaydýrma
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float currentPosition = touch.deltaPosition.x / Screen.width;

                move_aim += sensivity * currentPosition;

                move_aim=Mathf.Clamp(move_aim, -range / 2f, range/ 2f);
            }
        }

        float smooth_position = Mathf.Lerp(player_physics.transform.localPosition.x, move_aim, Time.deltaTime * smoothness_speed);
        Vector3 new_position = new Vector3(smooth_position, player_physics.transform.localPosition.y) + Vector3.forward * speed * Time.deltaTime;
        transform.Translate(new Vector3(0, 0, new_position.z));

        player_physics.transform.localPosition =
            new Vector3(
                new_position.x
                , new_position.y);


    }
    #region IEnumerators
    public IEnumerator<float> TurboTimer()
    {
        yield return Timing.WaitForSeconds(turbo_duration);
        DOTween.To(() => Constants.turbo_fow, x => cam.m_Lens.FieldOfView = x, Constants.default_fow, 1f);
        speed = min_speed;
        turbo = false;
    }
    #endregion

    #region Commands
    public void Turbo()
    {
        DOTween.To(()=>Constants.default_fow, x=> cam.m_Lens.FieldOfView=x, Constants.turbo_fow, 1f);
        speed = turbo_speed;
        turbo = true;
        Timing.RunCoroutine(TurboTimer(),"turbo");
    }
    public void EndTurbo()
    {
        speed = min_speed;
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
        stacking_controller.RemoveAll();
        transform.position = Vector3.zero;
        Deactivate();
    }
    public void SetPosition(Vector3 new_position)
    {
        transform.position = new Vector3(0, 0, new_position.z);

        player_physics.transform.localPosition =
            new Vector3(
                new_position.x
                , new_position.y);

    }

    public void Fall()
    {
        animator.ResetTrigger("run");
        animator.ResetTrigger("reset");
        Pause();
        animator.SetTrigger("fall");
        game_manager.LevelFailed();
        stacking_controller.StopTrail();
    }
    public void Pause()
    {
        active = false;
    }
    public void Resume()
    {
        active = true;
    }
    public void Jump()
    {
        DOTween.Sequence()
            .AppendCallback(delegate { stacking_controller.DropTrail(); is_grounded = false; })
            .Append(player_physics.transform.DOLocalMoveY(jump_height, jump_duration/2).SetEase(Ease.OutCirc))
            .Append(player_physics.transform.DOLocalMoveY(0, jump_duration / 2).SetEase(Ease.InCirc))
            .AppendCallback(delegate { stacking_controller.StartTrail(); is_grounded = true; });
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
