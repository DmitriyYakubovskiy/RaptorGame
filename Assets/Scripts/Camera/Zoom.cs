using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zoom : MonoBehaviour
{
    [SerializeField] private Camera cam;
    private float defaultFov;

    private void Awake()
    {
        defaultFov = cam.orthographicSize;
    }

    private void Start()
    {
        if (PlayerPrefs.GetInt("ZoomX15") == 1)
        {
            cam.orthographicSize = defaultFov - (defaultFov / 4);
        }
        else
        {
            cam.orthographicSize = defaultFov;
        }
    }

    public void ZoomX15()
    {
        if (PlayerPrefs.GetInt("ZoomX15") == 1)
        {
            cam.orthographicSize = defaultFov;
            PlayerPrefs.SetInt("ZoomX15", 0);
        }
        else
        {
            cam.orthographicSize = defaultFov - (defaultFov / 4);
            PlayerPrefs.SetInt("ZoomX15", 1);
        }
    }
}
