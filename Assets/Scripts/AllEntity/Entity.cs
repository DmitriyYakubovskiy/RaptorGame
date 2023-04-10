using System;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [SerializeField] protected float m_lives;
    [SerializeField] protected float m_speed;
    [SerializeField] protected float m_jumpForce;
    [SerializeField] protected float m_attackRange;
    [SerializeField] protected float m_damage;

    [SerializeField] protected float m_timeBtwAttack;
    [SerializeField] protected float m_startTimeBtwAttack;

    [SerializeField] protected Transform m_attackPosition;

    protected Vector2 m_moveVector;
    protected Vector2 m_previousPosition;
    protected float m_speedReal;
    protected float m_radiusCheckGround;

    protected Transform m_transform;
    protected Rigidbody2D m_rb;

    private SpriteRenderer m_spriteRenderer;
    private Material m_matBlink;
    private Material m_matDefault;

    private UnityEngine.Object m_explosion;

    [SerializeField] protected LayerMask m_enemies;
    public bool IsGrounded { get; set; }
    public bool IsFlip { get; set; }
    public bool IsJumped { get; set; }
    
    public LayerMask GetEnemies()
    {
        return m_enemies;
    }

    public Entity SetTimeBtwAttack(float time)
    {
        m_timeBtwAttack = time;
        return this;
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

    public Vector2 GetPreviousPosition()
    {
        return m_previousPosition;
    }
    public Vector2 GetMoveVector()
    {
        return m_moveVector;
    }

    public float GetLives()
    {
        return m_lives;
    }

    public Rigidbody2D GetRb()
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

    public float GetSpeedReal()
    {
        return m_speedReal;
    }

    public virtual void Awake()
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

        m_matBlink = Resources.Load("DamageBlink", typeof(Material)) as Material;
        m_matDefault = m_spriteRenderer.material;

        m_explosion = Resources.Load("Smoke");
    }

    public virtual void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(new Vector2(m_rb.position.x, m_rb.position.y + m_radiusCheckGround / 2), m_radiusCheckGround + 0.2f);
        IsGrounded = collider.Length > 1;
    }

    public void RechargeTimeAttack()
    {
        if (m_timeBtwAttack > 0)
        {
            m_timeBtwAttack -= Time.deltaTime;
        }
    }

    public bool CheckUnit()
    {
        Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(m_attackPosition.position, m_attackRange, m_enemies);
        if (enemiesToDamage.Length > 0)
        {
            return true;
        }
        return false;
    }

    public virtual void GetDamage(float damage)
    {
        m_lives -= damage;

        m_spriteRenderer.material = m_matBlink;

        if (m_lives <= 0)
        {
            Die();
        }
        else
        {
            Invoke("ResetMaterial", 0.2f);
        }
    }

    public void SpeedCalculation()
    {
        m_speedReal = (Math.Abs(m_rb.position.x - m_previousPosition.x)) / Time.fixedTime;
        m_previousPosition.x = m_rb.position.x;
    }

    public void ExitFromTheCard()
    {
        if (m_rb.position.y < -6)
        {
            Die();
        }
    }

    public void ResetMaterial()
    {
        m_spriteRenderer.material = m_matDefault;
    }

    public virtual void Die()
    {
        GameObject explosionRef = (GameObject)Instantiate(m_explosion);
        explosionRef.transform.position = new Vector3(m_spriteRenderer.gameObject.GetComponent<Transform>().position.x, m_spriteRenderer.gameObject.GetComponent<Transform>().position.y, -1);
        Destroy(explosionRef, 1f);

        Destroy(this.gameObject);
    }

}
