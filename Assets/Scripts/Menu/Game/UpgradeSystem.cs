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
        RaptorFileManager raptor=new RaptorFileManager();
        RaptorData data = raptor.LoadData() as RaptorData;

        upgradePoints = data.upgradePoints;
        level = data.level;
        lives = data.lives;      
        speed = data.speed;
        attack = data.attack;
        knockback = data.knockback;

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

        while (knockback > 15)
        {
            knockback--;
            upgradePoints++;
        }
        Save();
        ShowText();
    }

    public void ShowText()
    {
        textLives.text = $"{lives}";
        textAttack.text = $"{attack}";
        textSpeed.text = $"{speed}";
        textKnockback.text = $"{knockback}";
        textUpgradePoints.text = $"{upgradePoints}";
        textLevel.text = $"{level}";
    }

    public void AddLives()
    {
        if(upgradePoints > 0 && lives < 5000)
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
        RaptorFileManager raptor = new RaptorFileManager();
        RaptorData data = new RaptorData();

        data.upgradePoints = upgradePoints;
        data.level = level;
        data.lives = lives;
        data.speed = speed;
        data.attack = attack;
        data.knockback = knockback;

        raptor.SaveData(data);
    }
}
