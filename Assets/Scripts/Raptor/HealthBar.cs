using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform m_redLine;
    [SerializeField] private GameObject m_HealthBar;
    private float maxHealth;
    private float m_fill;

    public float SetMaxHealth(float maxHealth)
    {
        return this.maxHealth = maxHealth;
    }

    public float GetMaxHealth()
    {
        return maxHealth;
    }

    public void Awake()
    {
        m_redLine = GetComponent<RectTransform>();
        m_HealthBar = transform.parent.gameObject;
        m_fill = 1f;
    }

    public void FixedUpdate()
    {
        if (m_redLine.GetComponent<Image>().fillAmount != m_fill)
        {
            Invoke("SmoothAddLives", 0.1f);
        }    
    }

    public void SmoothAddLives()
    {
        if (m_redLine.GetComponent<Image>().fillAmount > m_fill + 0.01f)
        {
            m_redLine.GetComponent<Image>().fillAmount -= 0.01f;
        }
        else if (m_redLine.GetComponent<Image>().fillAmount < m_fill - 0.01f)
        {
            m_redLine.GetComponent<Image>().fillAmount += 0.01f;
        }
        else
        {
            m_redLine.GetComponent<Image>().fillAmount = m_fill;
        }
    }

    public void ShowHealth(Raptor raptor)
    {
        if (raptor.Lives >= 0 && raptor != null)
        {
            m_fill = raptor.Lives / maxHealth;
        }
    }

    public void DeleteHealthBar()
    {
        Destroy(m_HealthBar);
    }
}
