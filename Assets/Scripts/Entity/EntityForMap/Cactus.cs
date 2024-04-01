using UnityEngine;

public class Cactus :Entity
{
    private void Start()
    {
        Damage = 0.2f;
        startTimeBtwAttack = 0.5f;
    }

    private void Update()
    {
        CactusAttack();
    }

    private void CactusAttack()
    {
        if (entitysForDamage.Count>0)
        {
            AttackSplash();
        }
        RechargeTimeAttack();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.name == "Raptor") && collision.isTrigger == false)
        {
            entitysForDamage.Add(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if ((collision.gameObject.tag == "Enemy" || collision.gameObject.name == "Raptor") && collision.isTrigger == false)
        {
            entitysForDamage.Remove(collision);
        }
    }
}
