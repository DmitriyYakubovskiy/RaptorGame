using UnityEngine;
using System;

namespace Assets.Scripts.AllEntity
{
    public abstract class AIEntity :Entity
    {
        protected GameObject m_raptor;

        protected float m_timePatrul = 0;
        protected float m_jumpForceStart;

        protected float m_timeCheckPlayer = 0;
        protected float m_timeStartCheckPlayer = 0;

        protected float m_beginCheckPlayer = 0;
        protected float m_endCheckPlayer = 0;
        protected float m_YCheckPlayer = 0;
        protected Vector2 m_distance =new(0,0);

        protected Vector2 m_sizeCheckingWall;
        protected Animator m_animator;

        protected System.Random m_rand = new System.Random();

        [SerializeField] private LayerMask m_MaskWall;
        [SerializeField] private LayerMask m_MaskEndMap;

        public Entity SetTimePatrul(float time)
        {
            m_timePatrul= time;
            return this;
        }

        public Entity SetTimeBtwJump(float time)
        {
            m_timeBtwJump = time;
            return this;
        }

        public GameObject GetRaptor()
        {
            return m_raptor;
        }

        public float GetTimePatrul()
        {
            return m_timePatrul;
        }

        public float GetStartTimeBtwJump()
        {
            return m_startTimeBtwJump;
        }

        public Animator GetAnimator()
        {
            return m_animator;
        }

        public Vector2 GetSizeCheckingWall()
        {
            return m_sizeCheckingWall;
        }

        public float GetJumpForceStart()
        {
            return m_jumpForceStart;
        }

        public void SearchRaptor()
        {
            if(GameObject.Find("Raptor")!=null)
            {
                m_raptor = GameObject.Find("Raptor");
            }
            //else
            //{
            //    m_raptor = new(0f, 10f, 0f);
            //}
        }

        public void DistanceToPlayer()
        {
            m_timeCheckPlayer-= Time.deltaTime;
            if (m_raptor != null)
            {
                if (m_timeCheckPlayer <= 0)
                {
                    m_distance = new(Math.Abs(m_raptor.transform.position.x - m_rb.position.x), Math.Abs(m_raptor.transform.position.y - m_rb.position.y));
                    m_timeCheckPlayer = m_timeStartCheckPlayer;
                }
            }
            else
            {
                if (m_timeCheckPlayer <= 0)
                {
                    m_distance = new(100, 100);
                }
            }
        }

        public void SpeedСalculation()
        {
            var k = Math.Abs(m_rb.position.x - m_previousPosition.x) / (m_speed * Time.deltaTime);
            m_animator.speed = k;
        }

        public virtual bool СheckTheWall()
        {
            Collider2D[] Wall = Physics2D.OverlapBoxAll(new Vector2(m_rb.position.x + (IsFlip == true ? -GetSizeCheckingWall().x : GetSizeCheckingWall().x), m_rb.position.y + GetSizeCheckingWall().y), new Vector2(GetSizeCheckingWall().x, GetSizeCheckingWall().y), 0f, m_MaskWall);
            
            if (Wall?.Length > 2)
            {
                return true;
            }
            return false;
        }

        public virtual bool СheckTheMapEnd()
        {
            Collider2D[] End = Physics2D.OverlapBoxAll(new Vector2(m_rb.position.x + (IsFlip == true ? -GetSizeCheckingWall().x : GetSizeCheckingWall().x), m_rb.position.y + GetSizeCheckingWall().y), new Vector2(GetSizeCheckingWall().x, GetSizeCheckingWall().y), 0f, m_MaskEndMap);

            if (End?.Length > 0)
            {
                m_timePatrul = 2;

                if (IsFlip == true)
                {
                    m_moveVector.x = 1;
                }
                else
                {
                    m_moveVector.x = -1;
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
                moveVector = -Math.Abs(m_raptor.transform.position.x - m_rb.position.x) / (m_raptor.transform.position.x - m_rb.position.x);
            }

            if (m_distance.y < m_YCheckPlayer && m_distance.x < m_endCheckPlayer && m_distance.x >= m_beginCheckPlayer)
            {
                m_moveVector = new Vector2(isAgressive * moveVector, 0);
                return true;
            }
            else if(m_distance.y < m_YCheckPlayer && m_distance.x < m_endCheckPlayer && m_distance.x < m_beginCheckPlayer && m_distance.y > 0.5 && m_timeBtwJump <= 0 && m_beginCheckPlayer > 0)
            {
                if (!IsFlip)
                {
                    m_moveVector = new Vector2(1, 0);
                }
                else
                {
                    m_moveVector = new Vector2(-1, 0);
                }
                return true;
            }
            else if (m_distance.x < m_beginCheckPlayer && m_beginCheckPlayer>0)
            {
                m_moveVector = new Vector2(0, 0);

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

        public virtual void RandomMove()
        {
            //System.Random rand = new System.Random();
            m_timePatrul= m_rand.Next(1, 5);
            m_moveVector.x= m_rand.Next(-1, 2);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(m_rb.position.x + (IsFlip == true ? -GetSizeCheckingWall().x : GetSizeCheckingWall().x), m_rb.position.y + +GetSizeCheckingWall().y, 1), new Vector3(GetSizeCheckingWall().x, GetSizeCheckingWall().y, 1));

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_attackPosition.position, m_attackRange);

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(new Vector2(m_rb.position.x, m_rb.position.y + GetRadiusCheckGround() / 2), GetRadiusCheckGround() + 0.2f);
        }
    }
}
