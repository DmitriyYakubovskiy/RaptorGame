using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExperienceBar : MonoBehaviour
{
    [SerializeField] private RectTransform m_greenLine;
    [SerializeField] private GameObject m_ExperienceBar;
    [SerializeField] private TextMeshProUGUI text;
    private float m_maxExperience;
    private float m_fill;
    private float experience;
    private int upgradePoints;

    public float SetMaxExperience(float maxExp)
    {
        return m_maxExperience = maxExp;
    }

    public float GetMaxExperience()
    {
        return m_maxExperience;
    }

    public void Awake()
    {
        upgradePoints =PlayerPrefs.GetInt("upgradePoints");
        experience = PlayerPrefs.GetFloat("experience");
        m_fill = 0f;
    }

    public void FixedUpdate()
    {
        if (m_greenLine.GetComponent<Image>().fillAmount >= 1)
        {
            m_greenLine.GetComponent<Image>().fillAmount = 0;
        }
        if (m_greenLine.GetComponent<Image>().fillAmount != m_fill)
        {
            Invoke("SmoothAddExperience", 0.1f);
        }
    }

    public void SmoothAddExperience()
    {
        if(m_greenLine.GetComponent<Image>().fillAmount <= m_fill + 0.01f && m_greenLine.GetComponent<Image>().fillAmount >= m_fill - 0.01f)
        {
            m_greenLine.GetComponent<Image>().fillAmount = m_fill;
        }
        else 
        {
            m_greenLine.GetComponent<Image>().fillAmount += 0.01f;
        }
    }

    public void ShowExperience(Raptor raptor)
    {
        if (raptor != null)
        {
            m_fill = raptor.GetExperience() / m_maxExperience;
        }
    }

    public void ShowUpdatePoints(int up)
    {
        text.text = $"{up}";
    }

    public void SaveExperience(int up,float exp)
    {
        PlayerPrefs.SetInt("upgradePoints", up);
        PlayerPrefs.SetFloat("experience", exp);
    }

    public void UploadDataToRaptor(Raptor raptor)
    {
        raptor.SetExperience(experience);
        raptor.SetUpdatePoints(upgradePoints);
    }
}
