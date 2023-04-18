using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using UnityEngine;
using System;

public class Crab : AIEntity, ITrait<CanJump>, ITrait<CanMove>, ITrait<CanAgressiveLogics>, ITrait<CanAttackOneUnit>
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("CrabState"); }
        set { GetAnimator().SetInteger("CrabState", (int)value); }
    }

    private void Start()
    {
        SearchRaptor();

        System.Random rand = new System.Random();

        m_lives = 30;
        m_speed = rand.Next(150, 250) / 100f;
        m_jumpForce = 68;
        m_jumpForceStart = m_jumpForce;
        m_radiusCheckGround = 0.3f;
        m_rb.mass = 4;

        m_sizeCheckingWall = new Vector2(0.4f, 0.09f);
        m_animator = GetComponentInChildren<Animator>();
        m_startTimeBtwJump = 1.5f;
        m_endCheckPlayer = 7;
        m_beginCheckPlayer= 0.8f;
        IsJumped = false;

        m_damage = 7f;
        m_startTimeBtwAttack = 2f;

        CounterEntity.AddAgressiveEntity();
        CounterEntity.AddEntity();
    }

    private void Update()
    {
        if (CheckUnit())
        {
            this.AttackOneUnit(this);
        }
        RechargeTimeAttack();
        CheckGround();
        this.AgressiveLogics(this);
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
            if (Math.Abs(m_moveVector.x) >= 0.1)
            {
                State = States.Run;
            }
            else
            {
                State = States.Idle;
            }
        }
    }

    private void OnDestroy()
    {
        CounterEntity.DeleteAgressiveEntity();
        CounterEntity.DeleteEntity();
    }
}
