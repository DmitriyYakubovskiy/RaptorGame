using UnityEngine;

public abstract class Entity : MonoBehaviour, IEntityAttack
{
    [SerializeField] protected float m_lives;
    [SerializeField] protected float m_speed;
    [SerializeField] protected float m_jumpForce;
    [SerializeField] protected float m_attackRange;
    [SerializeField] protected float m_damage;

    [SerializeField] protected float m_timeBtwAttack;
    [SerializeField] protected float m_startTimeBtwAttack;

    protected float m_startLives;
    protected string m_nameBonus;

    protected float m_timeBtwJump = 0;
    protected float m_startTimeBtwJump = 0;

    protected float m_smookeSize;

    [SerializeField] protected Transform m_attackPosition;

    protected Vector2 m_moveVector;
    protected Vector2 m_previousPosition;
    protected float m_radiusCheckGround;

    protected Transform m_transform;
    protected Rigidbody2D m_rb;

    protected SpriteRenderer m_spriteRenderer;
    protected Material m_matBlink;
    protected Material m_matDefault;

    private UnityEngine.Object m_explosion;

    [SerializeField] protected LayerMask m_enemies;
    public bool IsGrounded { get; set; }
    public bool IsFlip { get; set; }
    public bool IsJumped { get; set; }

    public Entity SetLives(float lives)
    {
        m_lives= lives;
        return this;
    }

    public Entity SetFlip(bool flip)
    {
        IsFlip = flip;
        if (flip == true)
        {
            m_transform.eulerAngles = new Vector2(0, 180);
        }
        else
        {
            m_transform.eulerAngles = new Vector2(0, 0);
        }
        return this;
    }

    public Entity SetJumpForce(float jumpForce)
    {
        m_jumpForce= jumpForce;
        return this;
    }

    public void SetTimeBtwAttack(float time)
    {
        m_timeBtwAttack = time;
    }

    public Entity SetMoveVector(Vector2 moveVector)
    {
        m_moveVector = moveVector;
        return this;
    }
    public Entity SetMoveVector(float x, float y)
    {
        m_moveVector.x = x;
        m_moveVector.y = y;
        return this;
    }

    public LayerMask GetEnemies()
    {
        return m_enemies;
    }

    public float GetTimeBtwAttack()
    {
        return m_timeBtwAttack;
    }

    public float GetAttackRange()
    {
        return m_attackRange;
    }
    public float GetDamage()
    {
        return m_damage;
    }

    public Transform GetAttackPosition()
    {
        return m_attackPosition;
    }

    public float GetStartTimeBtwAttack()
    {
        return m_startTimeBtwAttack;
    }

    public float GetRadiusCheckGround()
    {
        return m_radiusCheckGround;
    }

    public SpriteRenderer GetSpriteRenderer()
    {
        return m_spriteRenderer;
    }

    public Material GetMatBlink()
    {
        return m_matBlink;
    }

    public Vector2 GetMoveVector()
    {
        return m_moveVector;
    }

    public float GetLives()
    {
        return m_lives;
    }

    public Rigidbody2D GetRigidbody()
    {
        return m_rb;
    }

    public float GetJumpForce()
    {
        return m_jumpForce;
    }

    public float GetSpeed()
    {
        return m_speed;
    }

    protected virtual void Awake()
    {
        IsGrounded = true;

        m_rb = GetComponent<Rigidbody2D>();
        m_transform = GetComponent<Transform>();

        foreach (Transform child in m_transform.GetComponentsInChildren<Transform>())
        {
            if (child.gameObject.tag == "Sprite")
            {
                m_spriteRenderer = child.GetComponent<SpriteRenderer>();
            }
        }

        IsFlip = false;

        m_matBlink = Resources.Load("Material/DamageBlink", typeof(Material)) as Material;
        m_matDefault = m_spriteRenderer.material;

        m_explosion = Resources.Load("Prefabs/Entity/Smoke");
    }

    protected virtual void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(new Vector2(m_rb.position.x, m_rb.position.y + m_radiusCheckGround / 2), m_radiusCheckGround + 0.2f);
        IsGrounded = collider.Length > 2;
    }

    public virtual void DealDamage(float damage)
    {
        m_lives -= damage;

        m_spriteRenderer.material = m_matBlink;

        if (m_lives <= 0)
        {
            Die();
        }
        else
        {
            Invoke("ResetDamageMaterial", 0.2f);
        }
    }

    public virtual bool RechargeTimeJump()
    {
        if (m_timeBtwJump > 0)
        {
            m_timeBtwJump -= Time.deltaTime;
            return false;
        }
        return true;
    }

    protected virtual void ExitFromTheCard()
    {
        if (m_rb.position.y < -20)
        {
            Die();
        }
    }

    protected void ResetDamageMaterial()
    {
        m_spriteRenderer.material = m_matDefault;
    }

    public virtual void Die()
    {
        GameObject explosionRef = (GameObject)Instantiate(m_explosion);
        explosionRef.GetComponent<Transform>().localScale = new(m_smookeSize, m_smookeSize, 0);
        explosionRef.transform.position = new Vector3(m_spriteRenderer.GetComponent<Transform>().position.x, m_spriteRenderer.GetComponent<Transform>().position.y, 0);
        Destroy(explosionRef, 1f);

        if (m_nameBonus == "Heal")
        {
            var buf = Resources.Load("Prefabs/Entity/"+ m_nameBonus+"/"+ m_nameBonus);
            GameObject heal = (GameObject)Instantiate(buf);
            heal.GetComponent<Heal>().SetHeatPoint(m_startLives);
            heal.transform.position = new Vector3(m_spriteRenderer.GetComponent<Transform>().position.x, m_spriteRenderer.GetComponent<Transform>().position.y, 0);
        }

        Destroy(this.gameObject);
    }

}
