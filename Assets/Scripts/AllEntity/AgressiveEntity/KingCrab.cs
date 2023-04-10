using Assets.Scripts.AllEntity;
using UnityEngine;
using System;
using Assets.Scripts.AllEntity.Traits;

public class KingCrab : AIEntity, ITrait<CanClimb>,ITrait<CanMove>,ITrait<CanAgressiveLogics>,ITrait<CanAttackSplash>
{

    private States State
    {
        get { return (States)GetAnimator().GetInteger("KingCrabState"); }
        set { GetAnimator().SetInteger("KingCrabState", (int)value); }
    }

    public void Logica()
    {
        Vector2 distance = new(Math.Abs(Raptor.GetInstance().GetRb().position.x - GetRb().position.x), Math.Abs(Raptor.GetInstance().GetRb().position.y - GetRb().position.y));

        if (ÑheckTheWall())
        {
            if (IsGrounded)
            {
                IsJumped = true;
            }
        }

        if (distance.x < 100 && distance.x >= m_attackRange)
        {
            SetMoveVector(new Vector2(Math.Abs(Raptor.GetInstance().GetRb().position.x - GetRb().position.x) / (Raptor.GetInstance().GetRb().position.x - GetRb().position.x), 0));
        }
        else if (distance.x <= m_attackRange)
        {
            SetMoveVector(0, 0);
        }
        else
        {
            if (m_timePatrul <= 0)
            {
                System.Random rand = new System.Random();
                m_timePatrul = rand.Next(1, 4);
                m_moveVectorPatrul = rand.Next(-1, 2);
            }
            else
            {
                m_timePatrul -= Time.deltaTime;
                SetMoveVector(m_moveVectorPatrul, 0);
            }
        }
        this.Move(this);
    }



    private void Start()
    {
        System.Random rand = new System.Random();

        m_lives = 300;
        m_speed = 4;
        m_jumpForce = 28000;
        m_radiusCheckGround = 3.1f;
        m_rb.mass = 200;

        m_sizeCheckingWall = new Vector2(2.0f, 0.5f);
        m_animator = GetComponentInChildren<Animator>();
        m_radiusCheck = 50;
        m_startTimeBtwJump = 0.5f;
        IsJumped = false;

        m_damage = 50f;
        m_startTimeBtwAttack = 10f;
    }

    private void Update()
    {
        if (CheckUnit())
        {
            this.AttackSplash(this);
        }
        RechargeTimeAttack();
        CheckGround();        
        Logica();
        ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        if (IsJumped)
        {
            this.Climb(this);
            IsJumped = false;
        }
        
        if (m_timeBtwAttack > m_startTimeBtwAttack - 1.2)
        {
            State = States.Attack;
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
