using UnityEngine;

public class Zoom : MonoBehaviour
{
    private Camera cam;
    private float defaultFov;

    private void Awake()
    {
        cam = Camera.main;
        defaultFov = cam.orthographicSize;
    }

    private void Start()
    {
        if (SaveManager.Data.settings.isZoomed == true) cam.orthographicSize = defaultFov - (defaultFov / 4);
        else cam.orthographicSize = defaultFov;
    }

    public void ZoomX15()
    {
        if (SaveManager.Data.settings.isZoomed == true)
        {
            cam.orthographicSize = defaultFov;
            SaveManager.Data.settings.isZoomed = false;
        }
        else
        {
            cam.orthographicSize = defaultFov - (defaultFov / 4);
            SaveManager.Data.settings.isZoomed = true;
        }
    }
}
