using UnityEngine;
using UnityEngine.SceneManagement;

public class DieScreen : MonoBehaviour
{
    [SerializeField] private Transform m_panel;
    [SerializeField] private Camera cam;
    private float defaultFov;

    private void Awake()
    {
        defaultFov = cam.orthographicSize;
        m_panel=GetComponent<Transform>();
    }

    private void Update()
    {
        cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 3f, 0.2f * Time.deltaTime);
        m_panel.localScale= new(Mathf.MoveTowards(m_panel.localScale.x, 3, 0.2f * Time.deltaTime), Mathf.MoveTowards(m_panel.localScale.y,2, 0.2f * Time.deltaTime));
        Invoke("LoadMenu", 3);  
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene(0);
    }

    //private void OnDestroy()
    //{
    //   cam.orthographicSize = defaultFov;
    //}
}
