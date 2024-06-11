using UnityEngine;
using YG;

public static class YGAdsProvider
{
    public static void TryShowFullScreenAdWithChance(int chance)
    {
        var random = Random.Range(0, 101);

        if (chance < random) return;
        YandexGame.FullscreenShow();
    }

    public static void ShowRewardedAD(int id)
    {
        YandexGame.RewardVideoEvent(id);
    }
}

