
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform m_redLine;
    [SerializeField] private GameObject m_HealthBar;
    private float m_maxHealth;
    private float m_fill;

    public float SetMaxHealth(float maxHealth)
    {
        return m_maxHealth = maxHealth;
    }

    public float GetMaxHealth()
    {
        return m_maxHealth;
    }

    public void Awake()
    {
        m_redLine = GetComponent<RectTransform>();
        m_HealthBar = transform.parent.gameObject;
        m_fill = 1f;
    }

    public void ShowHealth(Raptor raptor)
    {
        if (raptor.GetLives() >= 0 && raptor != null)
        {
            m_fill = raptor.GetLives() / m_maxHealth;
        }

        m_redLine.GetComponent<Image>().fillAmount = m_fill;
    }

    public void DeleteHealthBar()
    {
        Destroy(m_HealthBar);
    }
}
