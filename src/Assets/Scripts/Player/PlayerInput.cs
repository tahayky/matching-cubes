using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput
{
    #region Variables
    private float move_aim=0;
    #endregion
    public float GetHorizontal(float range)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Moved)
            {
                float currentPosition = touch.deltaPosition.x / Screen.width;

                move_aim += Constants.touch_movement_sensivity * currentPosition;

                move_aim = Mathf.Clamp(move_aim, -range / 2f, range / 2f);
            }
        }
        return move_aim;
    }
}
