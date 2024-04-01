using System.Collections.Generic;
using UnityEngine;

public class Spike : Entity
{
    [SerializeField] private float DamageInput;

    private void Start()
    {
        Damage = DamageInput;
        startTimeBtwAttack =0.5f;
    }

    private void Update()
    {
        SpikeAttack();
    }

    private void SpikeAttack()
    {
        if (entitysForDamage.Count > 0)
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
