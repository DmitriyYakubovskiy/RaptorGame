using UnityEngine;
using TMPro;

public class FpsController : MonoBehaviour
{
    private TextMeshProUGUI fpsText;
    private float time;
    private float pollingTime = 1f;
    private int frameCount;

    private void Start()
    {
        fpsText=GetComponent<TextMeshProUGUI>();
        if (SaveManager.Data.settings.fpsIsOn==true) fpsText.gameObject.SetActive(true);
        else fpsText.gameObject.SetActive(false);
    }

    private void Update()
    {
        ShowFPS();
    }

    private void ShowFPS()
    {
        if (SaveManager.Data.settings.fpsIsOn == true)
        {
            time += Time.deltaTime;
            frameCount++;

            if (time >= pollingTime)
            {
                fpsText.text = $"FPS: {(int)(frameCount / time)}";
                time = 0f;
                frameCount = 0;
            }
        }
    }
}
