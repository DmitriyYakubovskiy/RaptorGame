using System;
using UnityEngine;

namespace Assets.Scripts.Game.RaptorComponents
{
    public class RaptorMovement : MonoBehaviour
    {
        private Animator raptorAnimator;
        private Raptor raptor;

        private States State
        {
            get { return (States)raptorAnimator.GetInteger("State"); }
            set { raptorAnimator.SetInteger("State", (int)value); }
        }

        private void Start()
        {
            raptorAnimator = GetComponentInChildren<Animator>();
            raptor = GetComponent<Raptor>();
        }

        private void Update()
        {
            if (Input.GetAxis("Horizontal") != 0)
            {
                raptor.MoveVector = new Vector2(Input.GetAxis("Horizontal"), 0);
                raptor.Move();
            }
            if (Input.GetMouseButton(0))
            {
                raptor.ClickAttackButton();
            }
            if (Input.GetKeyDown(KeyCode.Space))
            {
                raptor.ClickJumpButton();
            }
        }

        private void FixedUpdate()
        {
            if (raptor.IsJumped) raptor.Jump();

            if (raptor.TimeBtwAttack > 0.2)
            {
                State = States.Attack;
            }
            else
            {
                if (raptor.IsGrounded)
                {
                    if (Input.GetAxis("Horizontal") != 0) State = States.Run;
                    else State = States.Idle;
                }
                else
                {
                    if (raptor.PreviousPosition.y + 0.09f < raptor.RaptorRigidbody.position.y) State = States.Jump;
                    else State = States.Fall;
                }
            }
        }
    }
}
