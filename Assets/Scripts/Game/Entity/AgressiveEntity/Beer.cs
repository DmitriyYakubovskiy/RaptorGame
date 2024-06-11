using Assets.Scripts.AllEntity;
using UnityEngine;

namespace Assets.Scripts.Game.Entity.AgressiveEntity
{
    public class Beer : AIEntity
    {
        private States State
        {
            get { return (States)GetAnimator().GetInteger("State"); }
            set { GetAnimator().SetInteger("State", (int)value); }
        }

        private void Start()
        {
            SearchRaptor();

            experience = 210;

            Lives = 230;
            startLives = Lives;
            Speed = 5;
            JumpForce = 140;
            JumpForceStart = JumpForce;
            RadiusCheckGround = 0.7f;
            SmookeSize = 3.2f;

            sizeCheckingWall = new Vector2(0.7f, -0.1f);
            animator = GetComponentInChildren<Animator>();
            StartTimeBtwJump = 2f;
            endCheckPlayer = 15;
            beginCheckPlayer = 1.5f;
            checkPlayerY = 5;
            timeStartCheckPlayer = 0.6f;
            IsJumped = false;

            Knockback = 25;
            Damage = 30f;
            startTimeBtwAttack = 2f;

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
            if (IsJumped) Jump();

            animator.speed = 1;
            if (timeBtwAttack > startTimeBtwAttack - 0.5)
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
        }
    }
}
