using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using UnityEngine;
using System;

public class Fox : AIEntity,ITrait<CanJump>,ITrait<CanMove>,ITrait<CanPeacefulLogics>
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("FoxState"); }
        set { GetAnimator().SetInteger("FoxState", (int)value); }
    }

    private void Start()
    {
        SearchRaptor();

        m_lives = 20;
        m_startLives = m_lives;
        m_speed = m_rand.Next(400, 600) / 100f;
        m_jumpForce = 36;
        m_jumpForceStart = m_jumpForce;
        m_radiusCheckGround =0.06f;
        m_rb.mass = 2;
        m_smookeSize = 1;

        m_sizeCheckingWall = new Vector2(0.4f, 0.09f);
        m_animator=GetComponentInChildren<Animator>();
        m_startTimeBtwJump = 1f;
        m_endCheckPlayer = 7;
        m_beginCheckPlayer = 0f;
        m_YCheckPlayer = 3f;
        IsJumped = false;

        m_timeStartCheckPlayer = 0.3f;

        m_nameBonus = "Heal";

        CounterEntity.AddEntity();
    }

    private void Update()
    {
        CheckGround();
        this.PeacefulLogics(this);
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

        m_animator.speed = 1;
        if (m_previousPosition.y < m_rb.position.y && !IsGrounded)
        {
            State = States.Jump;
        }
        else
        {
            if (m_moveVector.x != 0)
            {
                SpeedÑalculation();
                State = States.Run;
            }
            else
            {
                State = States.Idle;
            }
        }
        m_previousPosition=m_rb.position;
    }

    private void OnDestroy()
    {
        CounterEntity.DeleteEntity();
    }
}
