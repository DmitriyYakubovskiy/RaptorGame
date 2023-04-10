using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using UnityEngine;

public class Fox : AIEntity,ITrait<CanJump>,ITrait<CanMove>,ITrait<CanPeacefulLogics>
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("FoxState"); }
        set { GetAnimator().SetInteger("FoxState", (int)value); }
    }

    private void Start()
    {
        System.Random rand = new System.Random();

        m_lives = 20;
        m_speed = rand.Next(400, 600) / 100f;
        m_jumpForce = 20;
        m_radiusCheckGround=0.5f;
        m_rb.mass = 1;

        m_sizeCheckingWall = new Vector2(0.3f, 0.09f);
        m_animator=GetComponentInChildren<Animator>();
        m_startTimeBtwJump = 0.5f;
        m_radiusCheck = 10;
        IsJumped = false;
    }

    private void Update()
    {
        CheckGround();
        SetMoveVector(this.PeacefulLogics(this),0);
        this.Move(this);
        ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        if (IsJumped)
        {
            this.Jump(this);
            IsJumped=false;
        }

        if (IsJumped)
        {
            State = States.Jump;
        }
        else
        {
            if (GetSpeedReal() > 0 + 0.0001 || GetSpeedReal() < 0 - 0.0001)
            {
                State = States.Run;
            }
            else
            {
                State = States.Idle;
            }
        }
        SpeedCalculation();
    }
}
