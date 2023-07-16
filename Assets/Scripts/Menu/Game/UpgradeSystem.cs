using TMPro;
using UnityEngine;

public class UpgradeSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLives;
    [SerializeField] private TextMeshProUGUI textAttack;
    [SerializeField] private TextMeshProUGUI textSpeed;
    [SerializeField] private TextMeshProUGUI textKnockback;

    [SerializeField] private TextMeshProUGUI textUpgradePoints;
    [SerializeField] private TextMeshProUGUI textLevel;

    private int upgradePoints;
    private int level;

    private int knockback;
    private float lives;
    private float attack;
    private float speed;

    public void Awake()
    {
        upgradePoints = PlayerPrefs.GetInt("upgradePoints");
        level = PlayerPrefs.GetInt("levelRaptor");
        if (level== 0)
        {
            level = 1;
        }
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

        if (PlayerPrefs.GetInt("knockback") >= 10 && PlayerPrefs.GetInt("knockback") <= 1001)
        {
            knockback = PlayerPrefs.GetInt("knockback");
        }
        else
        {
            knockback = 10;
        }

        Save();
        ShowText();
    }

    public void ResetPoints()
    {
        while (lives > 50)
        {
            lives-=10;
            upgradePoints++;
        }

        while (speed > 8)
        {
            speed--;
            upgradePoints++;
        }

        while (attack > 5)
        {
            attack--;
            upgradePoints++;
        }

        while (knockback > 10)
        {
            knockback--;
            upgradePoints++;
        }
        Save();
        ShowText();
    }

    public void ShowText()
    {
        textLives.text = $"Lives: {lives}";
        textAttack.text = $"Attack: {attack}";
        textSpeed.text = $"Speed: {speed}";
        textKnockback.text = $"Knockback: {knockback}";

        textUpgradePoints.text = $"Upgrade Points: {upgradePoints}";
        textLevel.text = $"Level: {level}";
    }

    public void AddLives()
    {
        if(upgradePoints > 0 && lives <5000)
        {
            //lives = (int)(lives * 1.1f);
            lives += 10;
            upgradePoints--;
        }
        Save();
        ShowText();
    }

    public void AddAttack()
    {
        if (upgradePoints > 0 && attack < 1000)
        {
            //attack = (int)attack * 1.1f;
            attack += 1;
            upgradePoints--;
        }
        Save();
        ShowText();
    }

    public void AddSpeed()
    {
        if (upgradePoints > 0 && speed<=13)
        {
            //speed = (int)speed * 1.1f;
            speed += 1;
            upgradePoints--;
        }
        Save();
        ShowText();
    }

    public void AddKnockback()
    {
        if (upgradePoints > 0 && knockback <= 1000)
        {
            knockback += 1;
            upgradePoints--;
        }
        Save();
        ShowText();
    }

    public void Save()
    {
        PlayerPrefs.SetFloat("lives", lives);
        PlayerPrefs.SetFloat("attack", attack);
        PlayerPrefs.SetFloat("speed", speed);
        PlayerPrefs.SetInt("knockback", knockback);
        PlayerPrefs.SetInt("upgradePoints", upgradePoints);
    }
}
