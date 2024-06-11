using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private RectTransform m_redLine;
    private GameObject entity;
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

    public void Start()
    {
        entity = GameObject.Find("Raptor");
        entity.GetComponent<Raptor>().healthPointChanged += ShowHealth;
        maxHealth = entity.GetComponent<Raptor>().MaxHealth;
        m_fill = 1f;
    }

    public void FixedUpdate()
    {
        if (m_redLine.GetComponent<Image>().fillAmount != m_fill) Invoke("SmoothAddLives", 0.1f);
    }

    public void SmoothAddLives()
    {
        if (m_redLine.GetComponent<Image>().fillAmount > m_fill + 0.01f) m_redLine.GetComponent<Image>().fillAmount -= 0.01f;
        else if (m_redLine.GetComponent<Image>().fillAmount < m_fill - 0.01f) m_redLine.GetComponent<Image>().fillAmount += 0.01f;
        else m_redLine.GetComponent<Image>().fillAmount = m_fill;
    }

    public void ShowHealth(float lives)
    {
        if (lives >= 0) m_fill = lives / maxHealth;
    }

    private void OnDestroy()
    {
        if (entity == null) return;
        entity.GetComponent<Raptor>().healthPointChanged -= ShowHealth;
    }
}
