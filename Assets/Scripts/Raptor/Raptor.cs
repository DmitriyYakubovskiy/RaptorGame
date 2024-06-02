using System;
using UnityEngine;

public class Raptor : Entity
{
    [SerializeField] private HealthBar m_healthBar;
    [SerializeField] private FixedJoystick m_fixedJoystick;
    [SerializeField] private Transform m_diePanel;
    private RaptorFileManager fileManager;
    private RaptorData data;
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
        fileManager = new RaptorFileManager();
        data= fileManager.LoadData() as RaptorData;
        RaptorAnimator = GetComponentInChildren<Animator>();

        level = data.level;
        Lives = data.lives;
        Speed = data.speed;
        Knockback = data.knockback;
        Damage = data.attack;

        maxExperience = 100;
        JumpForce = 90;
        RadiusCheckGround = 0.4f;
        rigidbody.mass = 3.75f;
        StartTimeBtwJump = 0.2f;
        startLives = Lives;
        SmookeSize = 1.8f;

        timeBtwAttack = 0;
        startTimeBtwAttack = 0.5f;

        m_healthBar.SetMaxHealth(Lives);

        experienceBar.SetMaxExperience(maxExperience);
        experienceBar.UploadDataToRaptor(this);
        experienceBar.ShowExperience(this,0);
        experienceBar.ShowUpdatePoints(updatePoints);

        m_matHeal = Resources.Load("Material/HealBlink", typeof(Material)) as Material;
    }

    private void Update()
    {
        if (Math.Abs(m_fixedJoystick.Horizontal) >= 0.1f)
        {
            moveVector=new Vector2(m_fixedJoystick.Horizontal, 0);
            Move();
        }
        if(Input.GetAxis("Horizontal") != 0)
        {
            moveVector = new Vector2(Input.GetAxis("Horizontal"),0);
            Move();
        }
        if (Input.GetMouseButton(0))
        {
            ClickAttackButton();
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ClickJumpButton();
        }
        RechargeTimeJump();
        RechargeTimeAttack();
        ExitFromTheCard();
    }

    private void FixedUpdate()
    {
        if (IsJumped)
        {
            Jump();
            IsJumped = false;
            TimeBtwJump = StartTimeBtwJump;
        }

        RaptorAnimator.speed = 1;
        if (timeBtwAttack > 0.2)
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
                else if (Math.Abs(rigidbody.position.x-previousPosition.x)>=0.01f)
                {
                    State = States.Run;
                    RaptorAnimator.speed = 1;
                }
                else
                {
                    State = States.Idle;
                }
            }
            else
            {
                if (previousPosition.y + 0.09f < rigidbody.position.y)
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
        m_healthBar.ShowHealth(this);
    }

    public void AddHeatPoint(float hp)
    {
        if (Lives + hp > m_healthBar.GetMaxHealth()) Lives=m_healthBar.GetMaxHealth();
        else Lives+=hp;

        m_healthBar.ShowHealth(this);
        spriteRenderer.material = m_matHeal;
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
            data.level = level;
            fileManager.SaveData(data);
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
        PlaySound(2, volume, isDestroyed: true);
        Lives = 0;
        m_healthBar.ShowHealth(this);
        base.Die();
        m_diePanel.gameObject.SetActive(true);
    }

    protected override void AttackOneUnit()
    {
        if (entitysForDamage.Count == 0 && timeBtwAttack <= 0) PlaySound(1, volume);
        base.AttackOneUnit();
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
