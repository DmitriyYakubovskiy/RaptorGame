using Assets.Scripts.AllEntity;
using UnityEngine;

public class Crab : AIEntity
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("State"); }
        set { GetAnimator().SetInteger("State", (int)value); }
    }

    private void Start()
    {
        SearchRaptor();

        experience = 20;

        Lives = 30;
        startLives = Lives;
        Speed = 2;
        JumpForce = 72;
        JumpForceStart = JumpForce;
        RadiusCheckGround = 0.18f;
        rigidbody.mass = 4;
        SmookeSize = 1.2f;

        sizeCheckingWall = new Vector2(0.4f, 0.09f);
        animator = GetComponentInChildren<Animator>();
        StartTimeBtwJump = 1.5f;
        endCheckPlayer = 7;
        beginCheckPlayer= 1f;
        checkPlayerY = 3;
        timeStartCheckPlayer = 0.6f;
        IsJumped = false;
        
        Knockback = 15;
        Damage = 7f;
        startTimeBtwAttack = 2f;
        
        CounterEntity.AddAgressiveEntity();
        CounterEntity.AddEntity();
    }

    private void Update()
    {
        if (entitysForDamage.Count>0)
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
        if (IsJumped) Jump();

        animator.speed = 1;
        if (timeBtwAttack > startTimeBtwAttack-0.5)
        {
            State = States.Attack;
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
