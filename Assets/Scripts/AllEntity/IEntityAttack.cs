using UnityEngine;

public interface IEntityAttack
{
    public Transform GetAttackPosition();
    public float GetAttackRange();
    public LayerMask GetEnemies();
    public float GetDamage();
    public float GetTimeBtwAttack();
    public float GetStartTimeBtwAttack();
    public void SetTimeBtwAttack(float time);
    public void RechargeTimeAttack(float timeBtwAttack)
    {
        if (timeBtwAttack > 0)
        {
            SetTimeBtwAttack(timeBtwAttack -= Time.deltaTime);
        }
    }

    public bool CheckUnit(Transform attackPosition,float attackRange,LayerMask enemies)
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);
        if (enemiesToDamage.Length > 0)
        {
            return true;
        }
        return false;
    }
}
