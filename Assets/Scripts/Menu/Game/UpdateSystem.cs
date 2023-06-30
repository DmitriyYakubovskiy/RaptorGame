using TMPro;
using UnityEngine;

public class UpdateSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLives;
    [SerializeField] private TextMeshProUGUI textAttack;
    [SerializeField] private TextMeshProUGUI textSpeed;
    [SerializeField] private TextMeshProUGUI textUpdatePoints;
    [SerializeField] private TextMeshProUGUI textLevel;

    private int updatePoints;
    private int level;
    
    private float lives;
    private float attack;
    private float speed;

    public void Awake()
    {
        updatePoints = PlayerPrefs.GetInt("updatePoints");
        level = PlayerPrefs.GetInt("levelRaptor");

        if (PlayerPrefs.GetFloat("lives") >= 50 && PlayerPrefs.GetFloat("lives") <= 6000)
        {
            lives = PlayerPrefs.GetFloat("lives");
        }
        else
        {
            lives = 50;
        }        
        
        if (PlayerPrefs.GetFloat("speed") >= 8 && PlayerPrefs.GetFloat("speed") <= 14)
        {
            speed = PlayerPrefs.GetFloat("speed");
        }
        else
        {
            speed = 8;
        }

        if (PlayerPrefs.GetFloat("attack") >= 5 && PlayerPrefs.GetFloat("attack") <= 1001)
        {
            attack = PlayerPrefs.GetFloat("attack");
        }
        else
        {
            attack = 5;
        }

        Save();
        ShowText();
    }

    public void ShowText()
    {
        textLives.text = $"Lives: {lives}";
        textAttack.text = $"Attack: {attack}";
        textSpeed.text = $"Speed: {speed}";
        textUpdatePoints.text = $"Update Points: {updatePoints}";
        textLevel.text = $"Level: {level}";
    }

    public void AddLives()
    {
        if(updatePoints > 0 && lives <5000)
        {
            //lives = (int)(lives * 1.1f);
            lives += 10;
            updatePoints--;
        }
        Save();
        ShowText();
    }

    public void AddAttack()
    {
        if (updatePoints > 0 && attack < 1000)
        {
            //attack = (int)attack * 1.1f;
            attack += 1;
            updatePoints--;
        }
        Save();
        ShowText();
    }

    public void AddSpeed()
    {
        if (updatePoints > 0 && speed<=13)
        {
            //speed = (int)speed * 1.1f;
            speed += 1;
            updatePoints--;
        }
        Save();
        ShowText();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("lives", lives);
        PlayerPrefs.SetFloat("attack", attack);
        PlayerPrefs.SetFloat("speed", speed);
        PlayerPrefs.SetInt("updatePoints", updatePoints);
    }
}
