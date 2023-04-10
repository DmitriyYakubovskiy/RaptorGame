using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;

public class Cactus : Entity, ITrait<CanAttackSplash>
{
    public override void Awake()
    {
        m_damage = 2f;
        m_startTimeBtwAttack = 0.5f;
    }

    void Update()
    {
        if (CheckUnit())
        {
            this.AttackSplash(this);
        }
        RechargeTimeAttack();
    }
}
