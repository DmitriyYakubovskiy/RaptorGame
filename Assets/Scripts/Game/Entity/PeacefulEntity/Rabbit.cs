using Assets.Scripts.AllEntity;
using UnityEngine;

public class Rabbit : AIEntity
{
    private States State
    {
        get { return (States)GetAnimator().GetInteger("State"); }
        set { GetAnimator().SetInteger("State", (int)value); }
    }

    private void Start()
    {
        SearchRaptor();

        System.Random rand = new System.Random();

        experience = 2;

        Lives = 10;
        startLives = Lives;
        Speed = rand.Next(300, 350) / 100f;
        JumpForce = 16;
        JumpForceStart = JumpForce;
        RadiusCheckGround = 0.06f;
        rigidbody.mass = 2;
        SmookeSize = 1;

        sizeCheckingWall = new Vector2(0.3f, 0.09f);
        animator = GetComponentInChildren<Animator>();
        StartTimeBtwJump = 1f;
        endCheckPlayer = 5;
        beginCheckPlayer = 0f;
        checkPlayerY = 3;
        IsJumped = false;

        timeStartCheckPlayer = 0.4f;

        NameBonus = "Heal";

        CounterEntity.AddEntity();
    }

    private void Update()
    {
        CheckGround();
        PeacefulLogics();
        Move();
        //ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        if (IsJumped)
        {
            Jump();
            IsJumped = false;
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
        previousPosition = rigidbody.position;
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
