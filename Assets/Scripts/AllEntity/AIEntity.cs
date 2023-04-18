using UnityEngine;
using System;

namespace Assets.Scripts.AllEntity
{
    public abstract class AIEntity :Entity
    {
        protected GameObject m_raptor;

        protected float m_timePatrul = 0;
        protected float m_jumpForceStart;

        protected float m_beginCheckPlayer = 0;
        protected float m_endCheckPlayer = 0;

        protected Vector2 m_sizeCheckingWall;
        protected Animator m_animator;

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

        public virtual bool СheckTheWall()
        {
            Collider2D[] Wall = Physics2D.OverlapBoxAll(new Vector2(GetRb().position.x + (IsFlip == true ? -GetSizeCheckingWall().x : GetSizeCheckingWall().x), GetRb().position.y + GetSizeCheckingWall().y), new Vector2(GetSizeCheckingWall().x, GetSizeCheckingWall().y), 0f, m_MaskWall);
            
            if (Wall?.Length > 2)
            {
                return true;
            }
            return false;
        }

        public virtual bool СheckTheMapEnd()
        {
            Collider2D[] End = Physics2D.OverlapBoxAll(new Vector2(GetRb().position.x + (IsFlip == true ? -GetSizeCheckingWall().x : GetSizeCheckingWall().x), GetRb().position.y + GetSizeCheckingWall().y), new Vector2(GetSizeCheckingWall().x, GetSizeCheckingWall().y), 0f, m_MaskEndMap);

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
            Vector2 distance = new(Math.Abs(m_raptor.transform.position.x - m_rb.position.x), Math.Abs(m_raptor.transform.position.y - m_rb.position.y));

            if (distance.y < 3 && distance.x < m_endCheckPlayer && distance.x >= m_beginCheckPlayer)
            {
                m_moveVector=new Vector2(isAgressive * -Math.Abs(m_raptor.transform.position.x - m_rb.position.x) / (m_raptor.transform.position.x - m_rb.position.x), 0);
                return true;
            }
            else if (distance.x < m_beginCheckPlayer)
            {
                m_moveVector = new Vector2(0, 0);
                return true;
            }
            return false;
        }

        public virtual void RandomMove()
        {
            System.Random rand = new System.Random();
            m_timePatrul=rand.Next(1, 5);
            m_moveVector.x=rand.Next(-1, 2);
        }

        void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(new Vector3(GetRb().position.x + (IsFlip == true ? -GetSizeCheckingWall().x : GetSizeCheckingWall().x), GetRb().position.y + +GetSizeCheckingWall().y, 1), new Vector3(GetSizeCheckingWall().x, GetSizeCheckingWall().y, 1));

            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(m_attackPosition.position, m_attackRange);

            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(new Vector2(GetRb().position.x, GetRb().position.y + GetRadiusCheckGround() / 2), GetRadiusCheckGround() + 0.2f);
        }
    }
}
