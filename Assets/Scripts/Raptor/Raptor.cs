using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using System;
using UnityEngine;

public class Raptor : Entity, ITrait<CanMove>, ITrait<CanJump>, ITrait<CanAttackOneUnit>
{
    [SerializeField] private HealthBar m_healthBar;
    [SerializeField] private FixedJoystick m_fixedJoystick;
    [SerializeField] private Transform m_diePanel;
    private Material m_matHeal;
    private float experience=0;
    private float maxExperience;
    private int updatePoints=1;
    private int level = 1;

    public ExperienceBar experienceBar;

    private Animator RaptorAnimator { get; set; }

    public HealthBar GetHealthBar()
    {
        return m_healthBar;
    }

    public ExperienceBar GetExperienceBar()
    {
        return experienceBar;
    }

    public float GetExperience()
    {
        return experience;
    }

    public int GetUpdatePoints()
    {
        return updatePoints;
    }

    public void SetExperience(float exp)
    {
        experience= exp;
    }
    
    public void SetUpdatePoints(int points)
    {
        updatePoints = points;
    }

    private States State
    {
        get { return (States)RaptorAnimator.GetInteger("State"); }
        set { RaptorAnimator.SetInteger("State", (int)value); }
    }

    private void Start()
    {
        RaptorAnimator = GetComponentInChildren<Animator>();

        maxExperience = 100;
        m_lives = PlayerPrefs.GetFloat("lives");
        m_speed = PlayerPrefs.GetFloat("speed"); ;
        m_jumpForce = 120;
        m_radiusCheckGround = 0.4f;
        m_rb.mass = 5;
        m_startTimeBtwJump = 0.2f;
        m_startLives = m_lives;
        m_smookeSize = 1.8f;

        knockback = PlayerPrefs.GetInt("knockback");
        m_damage = PlayerPrefs.GetFloat("attack"); ;
        m_timeBtwAttack = 0;
        m_startTimeBtwAttack = 0.5f;

        m_healthBar.SetMaxHealth(m_lives);

        experienceBar.SetMaxExperience(maxExperience);
        experienceBar.UploadDataToRaptor(this);
        experienceBar.ShowExperience(this,0);
        experienceBar.ShowUpdatePoints(updatePoints);

        level = PlayerPrefs.GetInt("levelRaptor");
        if (level == 0)
        {
            level = 1;
        }
        m_matHeal = Resources.Load("Material/HealBlink", typeof(Material)) as Material;
    }

    private void Update()
    {
        if (Math.Abs(m_fixedJoystick.Horizontal) >= 0.1f)
        {
            SetMoveVector(m_fixedJoystick.Horizontal, 0);
            this.Move(this);
        }

        RechargeTimeJump();
        (this as IEntityAttack).RechargeTimeAttack(m_timeBtwAttack);
        ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        if (IsJumped)
        {
            this.Jump(this);
            IsJumped = false;
            m_timeBtwJump = m_startTimeBtwJump;
        }

        RaptorAnimator.speed = 1;
        if (m_timeBtwAttack > 0.2)
        {
            State = States.Attack;
        }
        else
        {
            if (IsGrounded)
            {
                if (Math.Abs(m_fixedJoystick.Horizontal) >= 0.1f)
                {
                    State = States.Run;
                    RaptorAnimator.speed = Math.Abs(m_fixedJoystick.Horizontal);
                }
                else
                {
                    State = States.Idle;
                }
            }
            else
            {
                if (m_previousPosition.y + 0.09f < m_rb.position.y)
                {
                    State = States.Jump;
                }
                else
                {
                    State = States.Fall;
                }
            }
        }
        CheckGround();
        m_previousPosition = new Vector2(m_rb.position.x, m_rb.position.y);
    }

    protected override void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(new Vector2(m_rb.position.x, m_rb.position.y + 0.025f), m_radiusCheckGround, ground);
        IsGrounded = collider.Length > 2;
    }

    public override void DealDamage(float damage)
    {
        m_lives = m_lives - damage;

        GetSpriteRenderer().material = GetMatBlink();

        if (m_lives <= 0)
        {
            Die();
        }
        else
        {
            Invoke("ResetDamageMaterial", 0.2f);    
        }
        
        m_healthBar.ShowHealth(this);
    }

    public void AddHeatPoint(float hp)
    {
        if (m_lives + hp > m_healthBar.GetMaxHealth())
        {
            m_lives=m_healthBar.GetMaxHealth();
        }
        else
        {
            m_lives+=hp;
        }
        m_healthBar.ShowHealth(this);
        m_spriteRenderer.material = m_matHeal;
        Invoke("ResetHealMaterial", 0.2f);
    }    
    
    public void AddExperience(float exp)
    {            
        int buf=(int)(experience + exp) / (int)maxExperience;
        if (experience + exp >= maxExperience)
        {
            updatePoints += buf;
            experience = (int)(experience + exp) % (int)maxExperience;
            experienceBar.ShowUpdatePoints(updatePoints);
            level+= 1 + (int)(experience + exp) / (int)maxExperience;
            PlayerPrefs.SetInt("levelRaptor", level);
        }
        else 
        {
            experience += exp;
        }
        experienceBar.ShowExperience(this, buf);
        experienceBar.SaveExperience(updatePoints, experience);
    }

    public void ResetHealMaterial()
    {
        m_spriteRenderer.material = m_matDefault;
    }

    public void Jump()
    {
        if (IsGrounded == true && RechargeTimeJump() == true)
        {
            IsJumped = true;
        }
    }
    public void Attack()
    {
        this.AttackOneUnit(this,IsFlip==true?-1:1, knockback);
    }

    public override void Die()
    {
        m_lives = 0;
        m_healthBar.ShowHealth(this);
        base.Die();
        m_diePanel.gameObject.SetActive(true);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_attackPosition.position, m_attackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(new Vector2(m_rb.position.x, m_rb.position.y + 0.25f), m_radiusCheckGround);
    }
}
