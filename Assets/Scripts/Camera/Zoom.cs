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
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;
        if (data.isZoomed == true) cam.orthographicSize = defaultFov - (defaultFov / 4);
        else cam.orthographicSize = defaultFov;
    }

    public void ZoomX15()
    {
        SettingsFileManager manager = new SettingsFileManager();
        SettingsData data = manager.LoadData() as SettingsData;
        if (data.isZoomed == true)
        {
            cam.orthographicSize = defaultFov;
            data.isZoomed = false;
            manager.SaveData(data);
        }
        else
        {
            cam.orthographicSize = defaultFov - (defaultFov / 4);
            data.isZoomed = true;
            manager.SaveData(data);
        }
    }
}
