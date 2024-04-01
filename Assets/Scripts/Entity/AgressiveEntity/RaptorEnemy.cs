using Assets.Scripts.AllEntity;
using UnityEngine;

public class RaptorEnemy : AIEntity
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("RaptorState"); }
        set { GetAnimator().SetInteger("RaptorState", (int)value); }
    }

    private void Start()
    {
        SearchRaptor();

        experience = 200;

        Lives = 150;
        startLives = Lives;
        Speed = 7;
        JumpForce = 100;
        JumpForceStart = JumpForce;
        RadiusCheckGround = 0.27f;
        rigidbody.mass = 5;
        SmookeSize = 1.8f;
        Knockback = 30;

        sizeCheckingWall = new Vector2(0.4f, 0.09f);
        animator = GetComponentInChildren<Animator>();
        StartTimeBtwJump = 0.5f;
        endCheckPlayer = 20;
        beginCheckPlayer = 1.5f;
        checkPlayerY = 8f;
        timeStartCheckPlayer = 0.1f;
        IsJumped = false;

        Damage = 10f;
        startTimeBtwAttack = 1f;

        CounterEntity.AddAgressiveEntity();
        CounterEntity.AddEntity();
    }

    private void Update()
    {
        if (entitysForDamage.Count > 0)
        {
            AttackOneUnit();
        }
        RechargeTimeAttack();
        CheckGround();
        AgressiveLogics();
        Move();
        ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        if (IsJumped)
        {
            Jump();
            IsJumped = false;
        }

        animator.speed = 1;
        if (timeBtwAttack > 0.7)
        {
            State = States.Attack;
        }
        else
        {
            if (IsGrounded)
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
            if (!IsGrounded)
            {
                if (previousPosition.y + 0.1f < rigidbody.position.y)
                {
                    State = States.Jump;
                }
                else
                {
                    State = States.Fall;
                }
            }
        }
        previousPosition = rigidbody.position;
    }

    private void OnDestroy()
    {
        if (Lives <= 0)
        {
            m_raptor.GetComponent<Raptor>().AddExperience(experience);
        }
        CounterEntity.DeleteAgressiveEntity();
        CounterEntity.DeleteEntity();
    }
}