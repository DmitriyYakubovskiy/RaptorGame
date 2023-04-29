using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using UnityEngine;

public class Cactus :MonoBehaviour, IEntityAttack, ITrait<CanAttackSplash>
{
    [SerializeField]private Transform m_attackPosition;
    [SerializeField] private float m_attackRange;
    [SerializeField] private LayerMask m_enemies;
    private float m_damage;
    private float m_startTimeBtwAttack;
    private float m_timeBtwAttack;

    private void Awake()
    {
        m_damage = 0.2f;
        m_startTimeBtwAttack = 0.5f;
    }

    private void Update()
    {
        CactusAttack();
    }

    private void CactusAttack()
    {
        if (m_timeBtwAttack <= 0)
        {
            if ((this as IEntityAttack).CheckUnit(m_attackPosition,m_attackRange,m_enemies))
            {
                this.AttackSplash(this);
            }
        }
        (this as IEntityAttack).RechargeTimeAttack(m_timeBtwAttack);
    }

    public Transform GetAttackPosition()
    {
        return m_attackPosition;
    }

    public float GetAttackRange()
    {
        return m_attackRange;
    }

    public LayerMask GetEnemies()
    {
        return m_enemies;
    }

    public float GetDamage()
    {
        return m_damage;
    }

    public float GetTimeBtwAttack()
    {
        return m_timeBtwAttack;
    }

    public float GetStartTimeBtwAttack()
    {
        return m_startTimeBtwAttack;
    }

    public void SetTimeBtwAttack(float time)
    {
        m_timeBtwAttack= time;
    }
}
