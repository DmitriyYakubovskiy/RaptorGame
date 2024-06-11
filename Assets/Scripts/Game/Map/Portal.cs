using System;
using UnityEngine;

public class Portal : MonoBehaviour
{
    [SerializeField] private Transform greenLine;
    [SerializeField] private GameObject processBar;
    private float time;
    private float startTime;
    private bool isInside;
    private float fill;

    public event Action teleport;

    private void Start()
    {
        isInside = false;
        startTime = 2;
        time = startTime;
        fill = 0f;
    }

    private void Update()
    {
        if (isInside) time -= Time.deltaTime;
        Teleport();
        ShowProcess();
    }

    public void ShowProcess()
    {
        var k = startTime - time;
        fill = k / startTime;
        greenLine.localScale =new(fill,greenLine.localScale.y,1);
    }

    private void Teleport()
    {
        if (time <= 0)
        {
            teleport?.Invoke();
            processBar.SetActive(false);
            SaveManager.Data.levels.openLevels[UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex+1] = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Raptor" && collision.isTrigger == false)
        {
            if (CounterEntity.GetCountAgressiveEntity() <= 0)
            {
                isInside = true;
                processBar.SetActive(true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Raptor" && collision.isTrigger == false)
        {
            time = startTime;
            isInside = false;
            fill = 0;
            greenLine.localScale = new(fill, greenLine.localScale.y, 1);
            processBar.SetActive(false);
        }
    }
}
