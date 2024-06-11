using Assets.Scripts.AllEntity;
using UnityEngine;
using System;

public class Fox : AIEntity
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("State"); }
        set { GetAnimator().SetInteger("State", (int)value); }
    }

    private void Start()
    {
        SearchRaptor();

        experience = 4;

        Lives = 20;
        startLives = Lives;
        Speed = m_rand.Next(400, 600) / 100f;
        JumpForce = 36;
        JumpForceStart = JumpForce;
        RadiusCheckGround =0.06f;
        rigidbody.mass = 2.5f;
        SmookeSize = 1;

        sizeCheckingWall = new Vector2(0.4f, 0.09f);
        animator=GetComponentInChildren<Animator>();
        StartTimeBtwJump = 1f;
        endCheckPlayer = 7;
        beginCheckPlayer = 0f;
        checkPlayerY = 3f;
        IsJumped = false;

        timeStartCheckPlayer = 0.3f;

        NameBonus = "Heal";

        CounterEntity.AddEntity();
    }

    private void Update()
    {
        CheckGround();
        PeacefulLogics();
        Move();
       // ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        if (IsJumped)
        {
            Jump();
            IsJumped=false;
        }

        animator.speed = 1;
        if (previousPosition.y < rigidbody.position.y && !IsGrounded)
        {
            State = States.Jump;
        }
        else
        {
            if (moveVector.x != 0)
            {
                SpeedCalculation();
                State = States.Run;
            }
            else
            {
                State = States.Idle;
            }
        }
        previousPosition=rigidbody.position;
    }

    private void OnDestroy()
    {
        if (Lives <= 0)
        {
            m_raptor.GetComponent<Raptor>().AddExperience(experience);
        }
        CounterEntity.DeleteEntity();
    }
}
