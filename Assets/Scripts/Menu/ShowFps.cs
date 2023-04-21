using UnityEngine;
using TMPro;
using Unity.VisualScripting;

public class ShowFps : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_fpsText;
    private float m_time;
    private float pollingTime = 1f;
    private int m_frameCount;

    private void Awake()
    {
        if (PlayerPrefs.GetInt("ShowFps") == 1)
        {
            m_fpsText.gameObject.SetActive(true);
        }
        else
        {
            m_fpsText.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        Fps();
    }

    private void Fps()
    {
        if (PlayerPrefs.GetInt("ShowFps") == 1)
        {
            m_time += Time.deltaTime;
            m_frameCount++;

            if (m_time >= pollingTime)
            {
                m_fpsText.text = $"FPS: {(int)(m_frameCount / m_time)}";
                m_time = 0f;
                m_frameCount = 0;
            }
        }
    }
}
