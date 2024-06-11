using UnityEngine;

public class DieScreen : MonoBehaviour
{      
    [SerializeField] private Transform panel;  
    private GameObject raptor;
    private Camera cam;
    private float defaultFov;
    private bool raptorDied = false;

    private void Start()
    {
        raptor = GameObject.Find("Raptor");
        raptor.GetComponent<Raptor>().died += RaptorDied;
        cam = Camera.main;
        defaultFov = cam.orthographicSize;
    }

    private void Update()
    {
        if (raptorDied)
        {
            cam.orthographicSize = Mathf.MoveTowards(cam.orthographicSize, 3f, 0.2f * Time.deltaTime);
            panel.localScale= new(Mathf.MoveTowards(panel.localScale.x, 3, 0.2f * Time.deltaTime), Mathf.MoveTowards(panel.localScale.y,2, 0.2f * Time.deltaTime));
            Invoke("LoadMenu", 3); 
        }
    }

    public void RaptorDied()
    {
        raptorDied = true;
        panel.gameObject.SetActive(true);
    }

    public void LoadMenu()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }

    private void OnDestroy()
    {
        if (raptor == null) return;
        raptor.GetComponent<Raptor>().died -= RaptorDied;
    }
}
