using UnityEngine;
using System;
using System.Linq;

namespace Assets.Scripts.AllEntity
{
    public abstract class AIEntity : Entity
    {        
        [SerializeField] private LayerMask m_MaskWall;
        [SerializeField] private LayerMask m_MaskEndMap;

        protected GameObject m_raptor;
        
        protected Animator animator;
        protected Vector2 sizeCheckingWall;
        protected Vector2 distance =new(0,0);

        protected float timePatrul = 0;

        protected float timeCheckPlayer = 0;
        protected float timeStartCheckPlayer = 0;

        protected float beginCheckPlayer = 0;
        protected float endCheckPlayer = 0;
        protected float checkPlayerY = 0;

        protected float experience;
        protected System.Random m_rand = new System.Random();

        public float JumpForceStart { get; set; }

        public Animator GetAnimator()
        {
            return animator;
        }

        //public Vector2 GetSizeCheckingWall()
        //{
        //    return sizeCheckingWall;
        //}

        public void SearchRaptor()
        {
            if(GameObject.Find("Raptor")!=null)
            {
                m_raptor = GameObject.Find("Raptor");
            }
        }

        public void DistanceToPlayer()
        {
            timeCheckPlayer-= Time.deltaTime;
            if (m_raptor != null)
            {
                if (timeCheckPlayer <= 0)
                {
                    distance = new(Math.Abs(m_raptor.transform.position.x - rigidbody.position.x), Math.Abs(m_raptor.transform.position.y - rigidbody.position.y));
                    timeCheckPlayer = timeStartCheckPlayer;
                }
            }
            else
            {
                if (timeCheckPlayer <= 0)
                {
                    distance = new(100, 100);
                }
            }
        }

        public void SpeedCalculation()
        {
            var k = Math.Abs(rigidbody.position.x - previousPosition.x) / (Speed * Time.deltaTime);
            animator.speed = k;
        }

        public virtual bool CheckTheWall()
        {
            Collider2D[] Wall = Physics2D.OverlapBoxAll(new Vector2(rigidbody.position.x + (IsFlip == true ? -sizeCheckingWall.x : sizeCheckingWall.x), rigidbody.position.y + sizeCheckingWall.y), new Vector2(sizeCheckingWall.x, sizeCheckingWall.y), 0f, m_MaskWall);

            int size = Wall.Length;

            for(int i= 0; i < Wall.Length; i++)
            {
                if (Wall[i].isTrigger == true)
                {
                    size--;
                }
            }

            //if (Wall?.Length > 2)
            if (size>2)
            {
                return true;
            }
            return false;
        }

        public virtual bool СheckTheMapEnd()
        {
            Collider2D[] End = Physics2D.OverlapBoxAll(new Vector2(rigidbody.position.x + (IsFlip == true ? -sizeCheckingWall.x : sizeCheckingWall.x), rigidbody.position.y + sizeCheckingWall.y), new Vector2(sizeCheckingWall.x, sizeCheckingWall.y), 0f, m_MaskEndMap);

            if (End?.Length > 0)
            {
                timePatrul = 2;

                if (IsFlip == true)
                {
                    moveVector.x = 1;
                }
                else
                {
                    moveVector.x = -1;
                }
                return true;
            }
            return false;
        }

        public virtual bool CheckThePlayer(int isAgressive)
        {
            DistanceToPlayer();
            float moveVector;
            if (m_raptor == null)
            {
                moveVector = 0;
            }
            else
            {
                moveVector = -Math.Abs(m_raptor.transform.position.x - rigidbody.position.x) / (m_raptor.transform.position.x - rigidbody.position.x);
            }

            if (distance.y < checkPlayerY && distance.x < endCheckPlayer && distance.x >= beginCheckPlayer)
            {
                base.moveVector = new Vector2(isAgressive * moveVector, 0);
                return true;
            }
            else if(distance.y < checkPlayerY && distance.x < endCheckPlayer && distance.x < beginCheckPlayer && distance.y > 0.5 && TimeBtwJump <= 0 && beginCheckPlayer > 0)
            {
                if (!IsFlip)
                {
                    base.moveVector = new Vector2(1, 0);
                }
                else
                {
                    base.moveVector = new Vector2(-1, 0);
                }
                return true;
            }
            else if (distance.x < beginCheckPlayer && beginCheckPlayer>0)
            {
                base.moveVector = new Vector2(0, 0);

                if (moveVector == 1)
                {
                    SetFlip(true);
                }
                else
                {
                    SetFlip(false);
                }

                return true;
            }
            return false;
        }

        protected void AgressiveLogics()
        {
            timePatrul = timePatrul - Time.deltaTime;

            if (RechargeTimeJump())
            {
                if (CheckTheWall())
                {
                    if (IsGrounded)
                    {
                        IsJumped = true;
                        TimeBtwJump=StartTimeBtwJump;
                    }
                }
            }

            СheckTheMapEnd();
            if (timePatrul > 2)
            {
                if (CheckThePlayer(-1) == true) { }
            }
            if (CheckThePlayer(-1) == false)
            {
                if (timePatrul <= 0)
                {
                    RandomizeMove();
                }
            }
        }

       protected void PeacefulLogics()
        {
            timePatrul=timePatrul - Time.deltaTime;

            if (RechargeTimeJump())
            {
                if (CheckTheWall())
                {
                    if (IsGrounded)
                    {
                        IsJumped = true;
                        TimeBtwJump=StartTimeBtwJump;
                    }
                }
            }

            СheckTheMapEnd();
            if (timePatrul > 2f)
            {
                CheckThePlayer(1);
            }
            if (CheckThePlayer(1) == false)
            {
                if (timePatrul <= 0)
                {
                    RandomizeMove();
                }
            }
        }

        public virtual void RandomizeMove()
        {
            //System.Random rand = new System.Random();
            timePatrul= m_rand.Next(1, 5);
            moveVector.x= m_rand.Next(-1, 2);
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Raptor" && collision.isTrigger==false )
            {
                entitysForDamage.Add(collision);
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.name == "Raptor" && collision.isTrigger == false)
            {
                entitysForDamage.Remove(collision);
            }
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(rigidbody.position.x + (IsFlip == true ? -sizeCheckingWall.x : sizeCheckingWall.x), rigidbody.position.y + + sizeCheckingWall.y, 1), new Vector3(sizeCheckingWall.x, sizeCheckingWall.y, 1));

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(new Vector2(rigidbody.position.x, rigidbody.position.y + RadiusCheckGround / 2), RadiusCheckGround + 0.2f);
        }
    }
}
