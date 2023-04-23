using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform m_greenLine;
    [SerializeField] private GameObject m_processBar;
    [SerializeField] private RectTransform m_panel;
    private float m_time;
    private float m_startTime;
    private bool m_isInside;
    private float m_fill;

    private void Awake()
    {
        m_isInside = false;
        m_startTime = 2;
        m_time = m_startTime;
        m_fill = 0f;
    }

    private void Update()
    {
        if (m_isInside)
        {
            m_time -= Time.deltaTime;
        }
        Teleport();
        ShowProcess();
    }

    public void ShowProcess()
    {
        var k = m_startTime - m_time;
        m_fill = k / m_startTime;

        m_greenLine.localScale =new(m_fill,m_greenLine.localScale.y,1);
    }

    private void Teleport()
    {
        if (m_time <= 0)
        {
            m_panel.gameObject.SetActive(true);
            //SceneManager.LoadScene(0);
            m_processBar.SetActive(false);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Raptor")
        {
            if (CounterEntity.GetCountAgressiveEntity() == 0)
            {
                m_isInside = true;
                m_processBar.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Raptor")
        {
            m_time = m_startTime;
            m_isInside = false;
            m_fill = 0;
            m_greenLine.localScale = new(m_fill, m_greenLine.localScale.y, 1);
            m_processBar.SetActive(false);
        }
    }
}
