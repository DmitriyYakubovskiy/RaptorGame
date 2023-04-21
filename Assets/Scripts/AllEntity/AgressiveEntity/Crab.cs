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
        m_startLives = m_lives;
        m_speed = 2;
        m_jumpForce = 68;
        m_jumpForceStart = m_jumpForce;
        m_radiusCheckGround = 0.3f;
        m_rb.mass = 4;
        m_smookeSize = 1.2f;

        m_sizeCheckingWall = new Vector2(0.4f, 0.09f);
        m_animator = GetComponentInChildren<Animator>();
        m_startTimeBtwJump = 1.5f;
        m_endCheckPlayer = 7;
        m_beginCheckPlayer= 1f;
        m_timeStartCheckPlayer = 0.6f;
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

        m_animator.speed = 1;
        if (m_timeBtwAttack > m_startTimeBtwAttack-0.5)
        {
            State = States.Attack;
        }
        else
        {
            if (m_moveVector.x != 0)
            {
                Speed—alculation();
                State = States.Run;
            }
            else
            {
                State = States.Idle;
            }
        }
        m_previousPosition = m_rb.position;
    }

    private void OnDestroy()
    {
        CounterEntity.DeleteAgressiveEntity();
        CounterEntity.DeleteEntity();
    }
}
