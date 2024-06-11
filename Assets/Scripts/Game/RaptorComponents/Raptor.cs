using System;
using UnityEngine;

public class Raptor : Entity
{
    private Material m_matHeal;
    private float experience=0;
    private float maxHealth;
    private int upgradePoints=1;
    private int level = 1;

    public event Action died;
    public event Action<float> healthPointChanged;
    public event Action<float, int> experienceChanged;
    public event Action<int> upgradePointChanged;
    public const float maxExperience = 100;

    public Vector2 MoveVector { get => moveVector; set => moveVector = value; }

    public Rigidbody2D RaptorRigidbody => rigidbody;
    public Vector2 PreviousPosition => previousPosition;
    public float MaxHealth => maxHealth;
    public float TimeBtwAttack => timeBtwAttack;

    public float GetExperience()
    {
        return experience;
    }

    public void SetExperience(float exp)
    {
        experience= exp;
    }
    
    public void SetUpdatePoints(int points)
    {
        upgradePoints = points;
    }

    private void Start()
    {
        level = SaveManager.Data.raptor.level;
        Lives = SaveManager.Data.raptor.lives;
        Speed = SaveManager.Data.raptor.speed;
        Knockback = SaveManager.Data.raptor.knockback;
        Damage = SaveManager.Data.raptor.attack;
        upgradePoints = SaveManager.Data.raptor.upgradePoints;
        experience = SaveManager.Data.raptor.experience;
        maxHealth = SaveManager.Data.raptor.lives;

        JumpForce = 90;
        RadiusCheckGround = 0.4f;
        rigidbody.mass = 3.75f;
        StartTimeBtwJump = 0.2f;
        startLives = Lives;
        SmookeSize = 1.8f;

        timeBtwAttack = 0;
        startTimeBtwAttack = 0.5f;

        healthPointChanged?.Invoke(Lives);
        experienceChanged?.Invoke(experience, 0);
        upgradePointChanged?.Invoke(upgradePoints);

        m_matHeal = Resources.Load("Material/HealBlink", typeof(Material)) as Material;
    }

    private void Update()
    {
        RechargeTimeJump();
        RechargeTimeAttack();
        ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        CheckGround();
        previousPosition = new Vector2(rigidbody.position.x, rigidbody.position.y);
    }

    protected override void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(new Vector2(rigidbody.position.x, rigidbody.position.y + 0.025f), RadiusCheckGround, ground);
        IsGrounded = collider.Length > 2;
    }

    public override void DealDamage(float damage)
    {
        Lives = Lives - damage;
        spriteRenderer.material = matBlink;

        if (Lives <= 0)
        {
            Die();
        }
        else
        {
            Invoke("ResetDamageMaterial", 0.2f);
            PlaySound(0, volume);
        }
        healthPointChanged?.Invoke(Lives);
    }

    public void AddHeatPoint(float hp)
    {
        if (Lives + hp > MaxHealth) Lives=MaxHealth;
        else Lives+=hp;

        healthPointChanged?.Invoke(Lives);
        spriteRenderer.material = m_matHeal;
        Invoke("ResetHealMaterial", 0.2f);
    }    
    
    public void AddExperience(float exp)
    {            
        int buf=(int)(experience + exp) / (int)maxExperience;
        if (experience + exp >= maxExperience)
        {
            upgradePoints += buf;
            experience = (int)(experience + exp) % (int)maxExperience;
            level+= 1 + (int)(experience + exp) / (int)maxExperience;
        }
        else 
        {
            experience += exp;
        }

        experienceChanged?.Invoke(experience, buf);
        upgradePointChanged?.Invoke(upgradePoints);
    }

    public void ResetHealMaterial()
    {
        spriteRenderer.material = matDefault;
    }

    public void ClickJumpButton()
    {
        if (IsGrounded == true && RechargeTimeJump() == true) IsJumped = true;
    }

    public void ClickAttackButton()
    {
        AttackOneUnit();
    }

    public override void Die()
    {
        Lives = 0;
        healthPointChanged?.Invoke(Lives);
        died?.Invoke();
        PlaySound(2, volume, isDestroyed: true);
        base.Die();
    }

    protected override void AttackOneUnit()
    {
        if (entitysForDamage.Count == 0 && timeBtwAttack <= 0) PlaySound(1, volume);
        base.AttackOneUnit();
    }

    private void Save()
    {
        SaveManager.Data.raptor.level = level;
        SaveManager.Data.raptor.upgradePoints = upgradePoints;
        SaveManager.Data.raptor.experience = experience;
    }

    private void OnDisable()
    {
        Save();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && collision.isTrigger == false) entitysForDamage.Add(collision);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag== "Enemy" && collision.isTrigger == false) entitysForDamage.Remove(collision);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(new Vector2(rigidbody.position.x, rigidbody.position.y + 0.25f), RadiusCheckGround);
    }
}
