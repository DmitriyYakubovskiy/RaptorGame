using TMPro;
using UnityEngine;

public class UpgradeManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLives;
    [SerializeField] private TextMeshProUGUI textAttack;
    [SerializeField] private TextMeshProUGUI textSpeed;
    [SerializeField] private TextMeshProUGUI textKnockback;
    [SerializeField] private TextMeshProUGUI textUpgradePoints;
    [SerializeField] private TextMeshProUGUI textLevel;

    public void Start()
    {
        ShowText();
    }

    public void ResetPoints()
    {
        while (SaveManager.Data.raptor.lives > 50)
        {
            SaveManager.Data.raptor.lives -= 10;
            SaveManager.Data.raptor.upgradePoints++;
        }

        while (SaveManager.Data.raptor.speed > 8)
        {
            SaveManager.Data.raptor.speed--;
            SaveManager.Data.raptor.upgradePoints++;
        }

        while (SaveManager.Data.raptor.attack > 5)
        {
            SaveManager.Data.raptor.attack--;
            SaveManager.Data.raptor.upgradePoints++;
        }

        while (SaveManager.Data.raptor.knockback > 15)
        {
            SaveManager.Data.raptor.knockback--;
            SaveManager.Data.raptor.upgradePoints++;
        }
        ShowText();
    }

    public void ShowText()
    {
        textLives.text = $"{SaveManager.Data.raptor.lives}";
        textAttack.text = $"{SaveManager.Data.raptor.attack}";
        textSpeed.text = $"{SaveManager.Data.raptor.speed}";
        textKnockback.text = $"{SaveManager.Data.raptor.knockback}";
        textUpgradePoints.text = $"{SaveManager.Data.raptor.upgradePoints}";
        textLevel.text = $"{SaveManager.Data.raptor.level}";
    }

    public void AddLives()
    {
        if (SaveManager.Data.raptor.upgradePoints > 0 && SaveManager.Data.raptor.lives < 5000)
        {
            SaveManager.Data.raptor.lives += 10;
            SaveManager.Data.raptor.upgradePoints--;
        }
        ShowText();
    }

    public void AddAttack()
    {
        if (SaveManager.Data.raptor.upgradePoints > 0 && SaveManager.Data.raptor.attack < 1000)
        {
            SaveManager.Data.raptor.attack++;
            SaveManager.Data.raptor.upgradePoints--;
        }
        ShowText();
    }

    public void AddSpeed()
    {
        if (SaveManager.Data.raptor.upgradePoints > 0 && SaveManager.Data.raptor.speed <= 13)
        {
            SaveManager.Data.raptor.speed++;
            SaveManager.Data.raptor.upgradePoints--;
        }
        ShowText();
    }

    public void AddKnockback()
    {
        if (SaveManager.Data.raptor.upgradePoints > 0 && SaveManager.Data.raptor.knockback <= 1000)
        {
            SaveManager.Data.raptor.knockback++;
            SaveManager.Data.raptor.upgradePoints--;
        }
        ShowText();
    }
}

