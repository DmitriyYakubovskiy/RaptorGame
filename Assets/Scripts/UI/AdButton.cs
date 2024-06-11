using UnityEngine;
using YG;

public class AdButton : MonoBehaviour
{
    [SerializeField] private UpgradeManager upgradeManager;
    private const int bonusId = 1;

    private void AddPoints(int id)
    {
        if (id != bonusId) return;

        SaveManager.Data.raptor.upgradePoints += 5;
        SaveManager.Data.raptor.level += 5;
        upgradeManager.ShowText();
    }

    public void ButtonClick()
    {
        YGAdsProvider.ShowRewardedAD(bonusId);
    }

    private void OnEnable()
    {
        YandexGame.RewardVideoEvent += AddPoints;
    }

    private void OnDisable()
    {
        YandexGame.RewardVideoEvent -= AddPoints;
    }
}
