using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterUpgradeManager : MonoBehaviour
{
    public enum Stat
    {
        Health,
        Damage,
        Speed,
        Dash,
        Reload
    }

    [MMInformation("This manager upgrades the player's stats and determines upgrade costs.", MMInformationAttribute.InformationType.None, false)]

    public MMF_Player SuccessfulUpgradeFeedback;
    public MMF_Player FailedUpgradeFeedback;

    [Space]
    [TextArea(3,5)]
    public string UpgradeFailText;

    protected const string _UpgradeFailTitle = "Upgrade Failed!";

    #region Upgrade Encapsulation
    public void TryUpgradeHealth()
    {
        if (!TryUpgradeStat(Stat.Health))
        {
            PopupElement.TryGetInstance().DisplayPopup(_UpgradeFailTitle, UpgradeFailText);
            FailedUpgradeFeedback?.PlayFeedbacks();
        }
    }

    public void TryUpgradeDamage()
    {
        if (!TryUpgradeStat(Stat.Damage))
        {
            PopupElement.TryGetInstance().DisplayPopup(_UpgradeFailTitle, UpgradeFailText);
            FailedUpgradeFeedback?.PlayFeedbacks();
        }
    }

    public void TryUpgradeSpeed()
    {
        if (!TryUpgradeStat(Stat.Speed))
        {
            PopupElement.TryGetInstance().DisplayPopup(_UpgradeFailTitle, UpgradeFailText);
            FailedUpgradeFeedback?.PlayFeedbacks();
        }
    }

    public void TryUpgradeDash()
    {
        if (!TryUpgradeStat(Stat.Dash))
        {
            PopupElement.TryGetInstance().DisplayPopup(_UpgradeFailTitle, UpgradeFailText);
            FailedUpgradeFeedback?.PlayFeedbacks();
        }
    }

    public void TryUpgradeReload()
    {
        if (!TryUpgradeStat(Stat.Reload))
        {
            PopupElement.TryGetInstance().DisplayPopup(_UpgradeFailTitle, UpgradeFailText);
            FailedUpgradeFeedback?.PlayFeedbacks();
        }
    }

    #endregion

        //constants

    private const int STARTING_UPGRADE = 15;
    private const int COST_MULTIPLY = 5;

    private int GetNearestMultiplier(int value, int multiplier)
    {
        //if its already a multiply then return
        int remainder = value % multiplier;
        if (remainder == 0) return value;

        //else get the nearest largest multiplier
        return multiplier * (((value - remainder) / multiplier) + 1);
    }

    public string GetUpgradeCostString(int level)
    {
        if (level >= ProgressManager.TryGetInstance().LevelCap) return "MAX";

        return GetNearestMultiplier(Mathf.CeilToInt(STARTING_UPGRADE + 1.5f * (level * level)), COST_MULTIPLY).ToString();
    }

    public int GetUpgradeCost(int level)
    {
        return GetNearestMultiplier(Mathf.CeilToInt(STARTING_UPGRADE + 1.5f * (level * level)), COST_MULTIPLY);
    }

    public bool TryUpgradeStat(Stat stat)
    {
        ProgressManager data = ProgressManager.TryGetInstance();

        short _currentLevel = 0;
        int _upgradeCost;

        //get the level of the stat we're trying to upgrade
        //assignment doesnt work for some reason, so im using addition
        switch (stat)
        {
            case Stat.Health:
                _currentLevel += data.ExternalData.EdushooterStats.Health;
                break;
            case Stat.Damage:
                _currentLevel += data.ExternalData.EdushooterStats.Damage;
                break;
            case Stat.Speed:
                _currentLevel += data.ExternalData.EdushooterStats.Speed;
                break;
            case Stat.Dash:
                _currentLevel += data.ExternalData.EdushooterStats.Dash;
                break;
            case Stat.Reload:
                _currentLevel += data.ExternalData.EdushooterStats.Reload;
                break;
        }

        //if we are at the level cap, then we can't upgrade
        if (_currentLevel >= ProgressManager.TryGetInstance().LevelCap) return false;

        //if the upgrade cost array cant be mapped onto our level, then just take the highest cost
        _upgradeCost = GetUpgradeCost(_currentLevel);

        //check if we can afford the cost, stop the upgrade if we cant
        if (!data.TryReduceCoins(_upgradeCost)) return false;

        //upload the purchase to firestore
        FirebaseHandler.TryGetInstance().UploadUpgradePurchase(_upgradeCost, stat, _currentLevel+1);

        // passed all checks, upgrade is successful!
        UpgradeStat(stat);
        SuccessfulUpgradeFeedback?.PlayFeedbacks();

        //trigger an event so that display is updated and save it
        MMGameEvent.Trigger("CharacterStatChanged");
        MMGameEvent.Trigger("TriggerSave");

        //play the victory animation if available
        MMGameEvent.Trigger("CharacterAnimationTriggerVictory");

        return true;
    }

    private void UpgradeStat(Stat stat)
    {
        ProgressManager data = ProgressManager.TryGetInstance();

        switch (stat)
        {
            case Stat.Health:
                data.UpgradeHealth();
                break;
            case Stat.Damage:
                data.UpgradeDamage();
                break;
            case Stat.Speed:
                data.UpgradeSpeed();
                break;
            case Stat.Dash:
                data.UpgradeDash();
                break;
            case Stat.Reload:
                data.UpgradeReload();
                break;
        }
    }
}
