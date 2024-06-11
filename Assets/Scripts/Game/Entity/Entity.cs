using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public abstract class Entity : Sound
{   
    [SerializeField] protected LayerMask enemies;
    [SerializeField] protected LayerMask ground;

    protected Vector2 moveVector;
    protected Vector2 previousPosition;

    protected Transform transform;
    protected Rigidbody2D rigidbody;

    protected SpriteRenderer spriteRenderer;
    protected Material matBlink;
    protected Material matDefault;
    private UnityEngine.Object explosion;

    protected float startLives;
    protected string NameBonus;

    protected float timeBtwAttack=0;
    protected float startTimeBtwAttack;

    protected float TimeBtwJump=0;
    protected float StartTimeBtwJump;

    protected List<Collider2D> entitysForDamage = new List<Collider2D>();

    public float RadiusCheckGround { get; set; }
    public float Lives { get; set; }
    public float Speed { get; set; }
    public float JumpForce { get; set; }

    public float Damage { get; set; }
    public int Knockback { get; set; }
    public float SmookeSize { get; set; }

    public bool IsGrounded { get; set; }
    public bool IsFlip { get; set; }
    public bool IsJumped { get; set; }
    public bool isMoved { get; set; }

    public Entity SetFlip(bool flip)
    {
        IsFlip = flip;
        if (flip == true)
        {
            transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            transform.eulerAngles = new Vector2(0, 0);
        }
        return this;
    }

    protected virtual void Awake()
    {
        IsGrounded = true;
        isMoved= true;

        rigidbody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();

        spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        IsFlip = false;

        matBlink = Resources.Load("Material/DamageBlink", typeof(Material)) as Material;
        matDefault = spriteRenderer.material;

        explosion = Resources.Load("Prefabs/Environment/Smoke");
    }

    protected virtual void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(new Vector2(rigidbody.position.x, rigidbody.position.y + RadiusCheckGround / 2), RadiusCheckGround + 0.2f,ground);
        IsGrounded = collider.Length > 2;
    }

    public virtual void DealDamage(float damage)
    {
        Lives -= damage;

        spriteRenderer.material = matBlink;

        if (Lives <= 0)
        {
            Die();
        }
        else
        {
            PlaySound(0, volume);
            Invoke("ResetDamageMaterial", 0.2f);
        }
    }

    public virtual bool RechargeTimeJump()
    {
        if (TimeBtwJump > 0)
        {
            TimeBtwJump -= Time.deltaTime;
            return false;
        }
        return true;
    }

    protected virtual void ExitFromTheCard()
    {
        if (rigidbody.position.y < -50)
        {
            DealDamage(Lives + 1);
        }
    }

    protected void ResetDamageMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    public virtual void StopMove(float time)
    {
        isMoved= false;
        Invoke("ReturnMove", time);
    }

    protected virtual void ReturnMove()
    {
        isMoved = true;
    }

    protected virtual void KnockBack()
    {

    }


    public void Move()
    {
        if (isMoved)
        {
            if (moveVector.x < 0)
            {
                SetFlip(true);
            }
            else if (moveVector.x != 0)
            {
                SetFlip(false);
            }

            rigidbody.velocity = new Vector2(moveVector.x * Speed, rigidbody.velocity.y);
        }
    }

    public void Jump()
    {
        rigidbody.AddForce(new Vector2(0, JumpForce), ForceMode2D.Impulse);
        IsJumped = false;
        TimeBtwJump = StartTimeBtwJump;
    }

    //protected void AttackOneUnit()
    //{
    //    int vector = (IsFlip == true ? -1 : 1);
    //    if (timeBtwAttack <= 0)
    //    {
    //        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);
    //        if (enemiesToDamage.Length > 0)
    //        {
    //            enemiesToDamage[0].GetComponent<Entity>().DealDamage(Damage);
    //            enemiesToDamage[0].GetComponent<Entity>().StopMove(Knockback / 100 + 0.2f);
    //            enemiesToDamage[0].GetComponent<Entity>().rigidbody.AddForce(new Vector2(Knockback * vector, Knockback), ForceMode2D.Impulse);
    //        }
    //        timeBtwAttack=startTimeBtwAttack;
    //    }
    //}

    protected virtual void AttackOneUnit()
    {
        if (entitysForDamage.Count==0)
        {
            if (timeBtwAttack <= 0) timeBtwAttack = startTimeBtwAttack;
        }
        else
        {
            int vector = (IsFlip == true ? -1 : 1);
            var entity = entitysForDamage[0].GetComponent<Entity>();

            if (timeBtwAttack <= 0)
            {
                entity.DealDamage(Damage);
                entity.GetComponent<Entity>().StopMove(Knockback / 100 + 0.2f);
                entity.GetComponent<Entity>().rigidbody.AddForce(new Vector2(Knockback * vector, Knockback), ForceMode2D.Impulse);
                timeBtwAttack = startTimeBtwAttack;
                PlaySound(1, volume);
            }
        }
    }

    //protected void AttackSplash()
    //{
    //    if (timeBtwAttack <= 0)
    //    {
    //        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);
    //        if (enemiesToDamage.Length > 0)
    //        {
    //            for (int i = 0; i < enemiesToDamage.Length; i++)
    //            {
    //                if (enemiesToDamage[i].isTrigger==false) enemiesToDamage[i].GetComponent<Entity>().DealDamage(Damage);
    //            }
    //        }
    //        timeBtwAttack=startTimeBtwAttack;
    //    }
    //}

    protected void AttackSplash()
    {
        if (timeBtwAttack <= 0)
        {
            for (int i = 0; i < entitysForDamage.Count; i++)
            {
                if (entitysForDamage[i].isTrigger == false) entitysForDamage[i].GetComponent<Entity>().DealDamage(Damage);
            }

            timeBtwAttack = startTimeBtwAttack;
        }
    }

    public void RechargeTimeAttack()
    {
        if (timeBtwAttack > 0)
        {
            timeBtwAttack-=Time.deltaTime;
        }
    }

    //public bool CheckUnit()
    //{
    //    Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPosition.position, attackRange, enemies);
    //    if (enemiesToDamage.Length > 0)
    //    {
    //        return true;
    //    }
    //    return false;
    //}

    public virtual void Die()
    {
        PlaySound(2, volume, isDestroyed: true);
        GameObject explosionRef = (GameObject)Instantiate(explosion);
        explosionRef.GetComponent<Transform>().localScale = new(SmookeSize, SmookeSize, 0);
        explosionRef.transform.position = new Vector3(spriteRenderer.GetComponent<Transform>().position.x, spriteRenderer.GetComponent<Transform>().position.y, 0);
        Destroy(explosionRef, 1f);

        if (NameBonus == "Heal")
        {
            var buf = Resources.Load("Prefabs/Environment/" + NameBonus);
            GameObject heal = (GameObject)Instantiate(buf);
            heal.GetComponent<Heal>().SetHeatPoint(startLives);
            heal.transform.position = new Vector3(spriteRenderer.GetComponent<Transform>().position.x, spriteRenderer.GetComponent<Transform>().position.y, 0);
        }

        Destroy(this.gameObject);
    }

}
