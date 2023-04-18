using Assets.Scripts.AllEntity;
using UnityEngine;

public class JumpBuster : MonoBehaviour
{
    [SerializeField] private LayerMask m_AIEntityMask;
    [SerializeField] private Transform m_transform;
    [SerializeField] private float m_boost;

    private Collider2D[] m_colider;
    private float m_timeStart=1.5f;

    private void Start()
    {
        m_transform= GetComponent<Transform>();
    }

    private void Update()
    {
        if (m_timeStart <= 0)
        {
            SearchAIEntityForBoost();
            Invoke("SearchAIEntityForAntiBoost", 1);
            m_timeStart = 1.5f;
        }
        m_timeStart-=Time.deltaTime;
    }

    private void SearchAIEntityForBoost()
    {
        Collider2D[] entity = Physics2D.OverlapCircleAll(m_transform.position, 0.5f, m_AIEntityMask);
        m_colider = entity;
        if (entity.Length > 0)
        {
            for (int i=0;i<entity.Length;i++)
            {
                entity[i].GetComponent<AIEntity>().SetJumpForce(entity[i].GetComponent<AIEntity>().GetJumpForce()*m_boost);
            }
        }
    }

    public void SearchAIEntityForAntiBoost()
    {
        if (m_colider.Length > 0)
        {
            for (int i = 0; i < m_colider.Length; i++)
            {
                m_colider[i].GetComponent<AIEntity>().SetJumpForce(m_colider[i].GetComponent<AIEntity>().GetJumpForceStart());
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(m_transform.position, 0.5f);
    }
}
