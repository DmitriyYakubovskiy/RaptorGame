using UnityEngine;

namespace Assets.Scripts.AllEntity
{
    public abstract class AIEntity :Entity
    {
        protected float m_timeBtwJump = 0;
        protected float m_startTimeBtwJump = 0;

        protected float m_timePatrul = 0;
        protected float m_moveVectorPatrul = 0;

        protected float m_radiusCheck = 0;

        protected Vector2 m_sizeCheckingWall;
        protected Animator m_animator;

        [SerializeField] private LayerMask m_MaskWall;

        public Entity SetMoveVectorPatrul(float vector)
        {
            m_moveVectorPatrul = vector;   
            return this;
        }

        public Entity SetTimePatrul(float time)
        {
            m_timePatrul= time;
            return this;
        }

        public AIEntity SetStartTimeBtwJump(float time)
        {
            m_startTimeBtwJump = time;
            return this;
        }

        public Entity SetTimeBtwJump(float time)
        {
            m_timeBtwJump = time;
            return this;
        }

        public AIEntity SetAnimator(Animator animator)
        {
            m_animator = animator;
            return this;
        }

        public AIEntity SetSizeCheckingWall(Vector2 sizeCheckingWall)
        {
            m_sizeCheckingWall = sizeCheckingWall;
            return this;
        }

        public AIEntity SetSizeCheckingWall(float x, float y)
        {
            m_sizeCheckingWall.x = x;
            m_sizeCheckingWall.y = y;
            return this;
        }

        public float GetRadiusCheck()
        {
            return m_radiusCheck;
        }

        public float GetMoveVectorPatrul()
        {
            return m_moveVectorPatrul;
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

        public virtual bool СheckTheWall()
        {
            Collider2D[] Wall = Physics2D.OverlapBoxAll(new Vector2(GetRb().position.x + (IsFlip == true ? -GetSizeCheckingWall().x : GetSizeCheckingWall().x), GetRb().position.y + GetSizeCheckingWall().y), new Vector2(GetSizeCheckingWall().x, GetSizeCheckingWall().y), 0f, m_MaskWall);
            
            if (Wall?.Length > 1)
            {
                return true;
            }
            return false;
        }

        public virtual bool RechargeTimeJump()
        {
            if (m_timeBtwJump > 0)
            {
                m_timeBtwJump -= Time.deltaTime;
                return false;
            }
            return true;
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
