using Assets.Scripts.AllEntity;
using Assets.Scripts.AllEntity.Traits;
using System;
using Unity.VisualScripting;
using UnityEngine;

public class Raptor : Entity, ITrait<CanMove>, ITrait<CanJump>, ITrait<CanAttackOneUnit>
{
    [SerializeField] HealthBar m_healthBar;
    [SerializeField] PolygonCollider2D[] m_collider;
    private static Raptor m_Instance;
    private Animator RaptorAnimator { get; set; }

    private States State
    {
        get { return (States)RaptorAnimator.GetInteger("State"); }
        set { RaptorAnimator.SetInteger("State", (int)value); }
    }

    private Raptor()
    {
        m_Instance = this;
    }

    public static Raptor GetInstance()
    {
        return m_Instance == null ? new Raptor() : m_Instance;
    }

    public void Start()
    {
        RaptorAnimator = GetComponentInChildren<Animator>();

        m_lives = 200;
        m_speed = 10;
        m_jumpForce = 120;
        m_radiusCheckGround = 0.5f;
        m_rb.mass = 5;

        m_damage = 5;
        m_timeBtwAttack = 0.5f;

        m_healthBar.SetMaxHealth(m_lives);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            this.AttackOneUnit(this);
        }
        if (Input.GetButton("Horizontal"))
        {
            SetMoveVector(Input.GetAxis("Horizontal"),0);
            this.Move(this);
        }
        if (IsGrounded && Input.GetButtonDown("Jump"))
        {
            IsJumped = true;
        }
        RechargeTimeAttack();
    }

    private void FixedUpdate()
    { 
        if (IsJumped)
        {
            this.Jump(this);
            IsJumped = false;
        }

        if (m_timeBtwAttack > 0.2)
        {
            State = States.Attack;
            ChangeCollider((int)States.Attack);
        }
        else
        {
            if (IsGrounded)
            {
                if (Input.GetButton("Horizontal"))
                {
                    State = States.Run;
                    ChangeCollider((int)States.Run);
                }
                else
                {
                    State = States.Idle;
                    ChangeCollider((int)States.Idle);
                }
            }
            if (!IsGrounded)
            {
                if (GetPreviousPosition().y + 0.1f < GetRb().position.y)
                {
                    State = States.Jump;
                    ChangeCollider((int)States.Jump);
                }
                else
                {
                    State = States.Fall;
                    ChangeCollider((int)States.Fall);
                }
            }
        }
        ExitFromTheCard();
        CheckGround();
        m_previousPosition=new Vector2(m_rb.position.x,m_rb.position.y);
    }

    public void ChangeCollider(int value)
    {
        m_collider[value].enabled = true;

        for (int i = 0; i < Enum.GetNames(typeof(States)).Length; i++)
        {
            if (i != value)
            {
                m_collider[i].enabled = false;
            }
        }
    }

    public override void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(new Vector2(GetRb().position.x, GetRb().position.y - 0.1f), 0.3f);
        IsGrounded = collider.Length > 1;
    }

    public override void GetDamage(float damage)
    {
        m_lives=m_lives - damage;

        GetSpriteRenderer().material = GetMatBlink();

        Debug.Log($"{GetLives()}");

        if (GetLives()<= 0)
        {
            m_healthBar.DeleteHealthBar();
            Die();
        }
        else
        {
            Invoke("ResetMaterial", 0.2f);
        }

        m_healthBar.ShowHealth(this);
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(m_attackPosition.position, m_attackRange);

        Gizmos.color = Color.gray;
        Gizmos.DrawWireSphere(new Vector2(GetRb().position.x, GetRb().position.y - 0.1f), 0.3f);
    }
}
