using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private RectTransform m_greenLine;
    [SerializeField] private TextMeshProUGUI text;
    private GameObject raptor;
    private float m_maxExperience;
    private float m_fill;

    private int cnt = 0;

    public float SetMaxExperience(float maxExp)
    {
        return m_maxExperience = maxExp;
    }

    public float GetMaxExperience()
    {
        return m_maxExperience;
    }

    private void Start()
    {
        raptor = GameObject.Find("Raptor");
        raptor.GetComponent<Raptor>().experienceChanged += ShowExperience;
        raptor.GetComponent<Raptor>().upgradePointChanged += ShowUpdatePoints;
        m_maxExperience = Raptor.maxExperience;
        m_fill = 0f;
    }

   private void FixedUpdate()
    {
        if (m_greenLine.GetComponent<Image>().fillAmount >= 1) m_greenLine.GetComponent<Image>().fillAmount = 0;
        if (m_greenLine.GetComponent<Image>().fillAmount != m_fill) Invoke("SmoothAddExperience", 0.1f);
    }

    private void SmoothAddExperience()
    {
        if(m_greenLine.GetComponent<Image>().fillAmount <= m_fill + 0.01f && m_greenLine.GetComponent<Image>().fillAmount >= m_fill - 0.01f)
        {
            m_greenLine.GetComponent<Image>().fillAmount = m_fill;
        }
        else 
        {
            m_greenLine.GetComponent<Image>().fillAmount += 0.01f;
            cnt += 1;
        }
        if (cnt >= 100)
        {
            m_fill -= 1;
            cnt= 0;
        }
    }

    public void ShowExperience(float experience,int cycles)
    {
        if (raptor != null) m_fill = experience / m_maxExperience;
        m_fill = cycles>=2?m_fill+cycles:m_fill;
        cnt = 0;
    }

    public void ShowUpdatePoints(int up)
    {
        text.text = $"{up}";
    }

    private void OnDestroy()
    {
        if (raptor == null) return;
        raptor.GetComponent<Raptor>().experienceChanged -= ShowExperience;
        raptor.GetComponent<Raptor>().upgradePointChanged -= ShowUpdatePoints;
    }
}
