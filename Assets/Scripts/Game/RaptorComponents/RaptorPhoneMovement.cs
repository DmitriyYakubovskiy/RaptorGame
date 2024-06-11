using System;
using UnityEngine;

public class RaptorPhoneMovement : MonoBehaviour
{
    private FixedJoystick fixedJoystick;
    private Animator raptorAnimator;
    private Raptor raptor;

    private States State
    {
        get { return (States)raptorAnimator.GetInteger("State"); }
        set { raptorAnimator.SetInteger("State", (int)value); }
    }

    private void Start()
    {
        raptorAnimator = GetComponentInChildren<Animator>();
        raptor=GetComponent<Raptor>();
        fixedJoystick = GameObject.Find("Fixed Joystick")?.GetComponent<FixedJoystick>();
    }

    private void Update()
    {
        if (fixedJoystick == null) return;
        if (Math.Abs(fixedJoystick.Horizontal) >= 0.1f)
        {
            raptor.MoveVector = new Vector2(fixedJoystick.Horizontal, 0);
            raptor.Move();
        }
    }

    private void FixedUpdate()
    {
        if (fixedJoystick == null) return;
        if (raptor.IsJumped) raptor.Jump();

        raptorAnimator.speed = 1;
        if (raptor.TimeBtwAttack > 0.2)
        {
            State = States.Attack;
        }
        else
        {
            if (raptor.IsGrounded)
            {
                if (Math.Abs(fixedJoystick.Horizontal) >= 0.1f)
                {
                    State = States.Run;
                    raptorAnimator.speed = Math.Abs(fixedJoystick.Horizontal);
                }
                else
                {
                    State = States.Idle;
                }
            }
            else
            {
                if (raptor.PreviousPosition.y + 0.09f < raptor.RaptorRigidbody.position.y) State = States.Jump;
                else State = States.Fall;
            }
        }
    }
}

