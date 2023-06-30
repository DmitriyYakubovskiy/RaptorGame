using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using UnityEngine;

public class RaptorEnemy : AIEntity, ITrait<CanJump>, ITrait<CanMove>, ITrait<CanAgressiveLogics>, ITrait<CanAttackOneUnit>
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("RaptorState"); }
        set { GetAnimator().SetInteger("RaptorState", (int)value); }
    }

    private void Start()
    {
        SearchRaptor();

        System.Random rand = new System.Random();

        experience = 150;

        m_lives = 150;
        m_startLives = m_lives;
        m_speed = 7;
        m_jumpForce = 100;
        m_jumpForceStart = m_jumpForce;
        m_radiusCheckGround = 0.27f;
        m_rb.mass = 5;
        m_smookeSize = 1.8f;

        m_sizeCheckingWall = new Vector2(0.4f, 0.09f);
        m_animator = GetComponentInChildren<Animator>();
        m_startTimeBtwJump = 0.5f;
        m_endCheckPlayer = 20;
        m_beginCheckPlayer = 1.5f;
        m_YCheckPlayer = 8f;
        m_timeStartCheckPlayer = 0.1f;
        IsJumped = false;

        m_damage = 10f;
        m_startTimeBtwAttack = 1f;

        CounterEntity.AddAgressiveEntity();
        CounterEntity.AddEntity();
    }

    private void Update()
    {
        if ((this as IEntityAttack).CheckUnit(m_attackPosition,m_attackRange,m_enemies))
        {
            this.AttackOneUnit(this);
        }
         (this as IEntityAttack).RechargeTimeAttack(m_timeBtwAttack);
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
            m_timeBtwJump = m_startTimeBtwJump;
        }

        m_animator.speed = 1;
        if (m_timeBtwAttack > 0.7)
        {
            State = States.Attack;
        }
        else
        {
            if (IsGrounded)
            {
                if (m_moveVector.x!=0)
                {
                    Speed—alculation();
                    State = States.Run;
                }
                else
                {
                    State = States.Idle;
                }
            }
            if (!IsGrounded)
            {
                if (m_previousPosition.y + 0.1f < m_rb.position.y)
                {
                    State = States.Jump;
                }
                else
                {
                    State = States.Fall;
                }
            }
        }
        m_previousPosition = m_rb.position;
    }

    private void OnDestroy()
    {
        CounterEntity.DeleteAgressiveEntity();
        CounterEntity.DeleteEntity();

        m_raptor.GetComponent<Raptor>().AddExperience(experience);
    }
}