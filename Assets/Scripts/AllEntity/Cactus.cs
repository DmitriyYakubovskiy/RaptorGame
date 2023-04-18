using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;

public class Cactus : Entity, ITrait<CanAttackSplash>
{
    protected override void Awake()
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
            if (CheckUnit())
            {
                this.AttackSplash(this);
            }
        }
        RechargeTimeAttack();
    }
}
