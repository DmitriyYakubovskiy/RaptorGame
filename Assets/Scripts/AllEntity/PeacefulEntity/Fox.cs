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

        System.Random rand = new System.Random();

        m_lives = 20;
        m_speed = rand.Next(400, 600) / 100f;
        m_jumpForce = 36;
        m_jumpForceStart = m_jumpForce;
        m_radiusCheckGround =0.3f;
        m_rb.mass = 2;

        m_sizeCheckingWall = new Vector2(0.4f, 0.09f);
        m_animator=GetComponentInChildren<Animator>();
        m_startTimeBtwJump = 1f;
        m_endCheckPlayer = 7;
        m_beginCheckPlayer = 0f;
        IsJumped = false;

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

        if (IsJumped)
        {
            State = States.Jump;
        }
        else
        {
            if (Math.Abs(m_moveVector.x) >= 0.11)
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
        CounterEntity.DeleteEntity();
    }
}
