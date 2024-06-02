using System;
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

    private int cnt = 0;

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
        RaptorFileManager fileManager = new RaptorFileManager();
        RaptorData data = fileManager.LoadData() as RaptorData;
        upgradePoints =data.upgradePoints;
        experience = data.experience;
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
            cnt += 1;
        }

        if (cnt >= 100)
        {
            m_fill -= 1;
            cnt= 0;
        }
    }

    public void ShowExperience(Raptor raptor,int cycles)
    {
        if (raptor != null)
        {
            m_fill = raptor.GetExperience() / m_maxExperience;
        }

        m_fill = cycles>=2?m_fill+cycles:m_fill;
        cnt = 0;
    }

    public void ShowUpdatePoints(int up)
    {
        text.text = $"{up}";
    }

    public void SaveExperience(int up,float exp)
    {
        RaptorFileManager fileManager = new RaptorFileManager();
        RaptorData data = fileManager.LoadData() as RaptorData;
        data.upgradePoints=up;
        data.experience= exp;
        fileManager.SaveData(data);
    }

    public void UploadDataToRaptor(Raptor raptor)
    {
        raptor.SetExperience(experience);
        raptor.SetUpdatePoints(upgradePoints);
    }
}
