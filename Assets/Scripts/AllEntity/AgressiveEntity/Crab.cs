using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using UnityEngine;

public class Crab : AIEntity, ITrait<CanJump>, ITrait<CanMove>, ITrait<CanAgressiveLogics>, ITrait<CanAttackOneUnit>
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("CrabState"); }
        set { GetAnimator().SetInteger("CrabState", (int)value); }
    }

    private void Start()
    {
        System.Random rand = new System.Random();

        m_lives = 30;
        m_speed = rand.Next(200, 300) / 100f;
        m_jumpForce = 30;
        m_radiusCheckGround = 0.5f;
        m_rb.mass = 4;

        m_sizeCheckingWall = new Vector2(0.4f, 0.2f);
        m_animator = GetComponentInChildren<Animator>();
        m_startTimeBtwJump = 0.5f;
        m_radiusCheck = 20;
        IsJumped = false;

        m_damage = 7f;
        m_startTimeBtwAttack = 2f;
    }

    private void Update()
    {
        if (CheckUnit())
        {
            this.AttackOneUnit(this);
        }
        RechargeTimeAttack();
        CheckGround();
        SetMoveVector(this.AgressiveLogics(this), 0);
        this.Move(this);
        ExitFromTheCard();
    }

    private void FixedUpdate()
    {   
        if (IsJumped)
        {
            this.Jump(this);
            IsJumped = false;
        }

        if (m_timeBtwAttack > m_startTimeBtwAttack-0.5)
        {
            State = States.Attack;
        }
        else
        {
            if (GetSpeedReal() > 0+0.0001|| GetSpeedReal() < 0 - 0.0001)
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
