using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Transform m_redLine;
    [SerializeField] private GameObject m_HealthBar;
    private Vector2 m_size;
    private Vector2 m_position;
    private float m_maxHealth;
    private float m_lenLine;

    public float SetMaxHealth(float maxHealth)
    {
        return m_maxHealth = maxHealth;
    }

    public void Awake()
    {
        m_redLine = GetComponent<Transform>();
        m_HealthBar = transform.parent.gameObject;
        m_size = m_redLine.localScale;
        m_position = m_redLine.localPosition;
    }

    public void ShowHealth(Raptor raptor)
    {
        if (raptor.GetLives() != 0 && raptor != null)
        {
            m_lenLine = m_maxHealth / (raptor.GetLives());
            m_redLine.localScale = new Vector3((m_size.x / m_lenLine), m_size.y, 0);
            m_redLine.localPosition = new Vector3(m_position.x, m_position.y, 0) - new Vector3((m_size.x - (m_size.x / m_lenLine)) / 2, 0, 0);
        }
    }

    public void DeleteHealthBar()
    {
        Destroy(m_HealthBar);
    }
}
