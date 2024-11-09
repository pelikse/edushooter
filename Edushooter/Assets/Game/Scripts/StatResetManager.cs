using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatResetManager : MonoBehaviour
{
    public CharacterUpgradeManager CharacterUpgradeManager;
    public MMF_Player RefundFeedback;

    private const float REFUND_RATIO = 0.9f;
    private const string REFUND_DESC = "Are you sure you want to reset all of your upgraded levels? This will refund <color=#ffd700>{0} coins</color>!\n\n<color=red>This cannot be undone.</color>";
    

    private bool ValidUpgradeRefund(int statLevel, int comparison)
    {
        return statLevel > comparison;
    }

    public void TryResetStats()
    {
        Debug.Log("resetting player stats");

        PlayerEdushooterData data = ProgressManager.TryGetInstance().ExternalData.EdushooterStats;

        //fetch the player's levels
        int _HPLevel = data.Health;
        int _dmgLevel = data.Damage;
        int _spdLevel = data.Speed;
        int _dashLevel = data.Dash;
        int _reloadLevel = data.Reload;

        //calculate the cost of all upgrades
        int _totalUpgradeCost = 0;
        int[] statLevels = { _HPLevel, _dmgLevel, _spdLevel, _dashLevel, _reloadLevel };


        //iterate from level 0 to MAX-1 level
        for (int i = 0; i < ProgressManager.TryGetInstance().LevelCap; i++)
        {
            // get the current level's upgrade cost
            int _currentLevelCost = CharacterUpgradeManager.GetUpgradeCost(i);
            // check if any of the stats are upgradable
            bool _anyStatsUpgradable = false;

            // iterate over each stat level
            foreach (int level in statLevels)
            {
                if (ValidUpgradeRefund(level, i))
                {
                    _totalUpgradeCost += _currentLevelCost;
                    _anyStatsUpgradable = true;
                }
            }

            // if none of the stats are upgradable, then we can stop the refund calculation
            if (!_anyStatsUpgradable)
            {
                break;
            }
        }

        // if there's no money to refund...
        if (_totalUpgradeCost <= 0)
        {
            PopupElement.TryGetInstance().DisplayPopup("Refund Failure", "You haven't upgraded anything, there's nothing to refund!");
            return;
        }

        // reduce the refund amount
        _totalUpgradeCost = (int)Mathf.Ceil(_totalUpgradeCost * REFUND_RATIO);
        // round it to the lowest multiple of 5
        _totalUpgradeCost = MiscellaneousMethods.ClosestLowerMultipleOfFive(_totalUpgradeCost);


        QuestionElement.TryGetInstance().DisplayQuestion(string.Format(REFUND_DESC, _totalUpgradeCost), () => {
            //if they say yes, then reset and add refunded coins
            Debug.Log("refunding coins and resetting stats");

            ProgressManager.TryGetInstance().ResetAllStats();
            ProgressManager.TryGetInstance().AddCoins(_totalUpgradeCost);

            //trigger an event so that display is updated and save it
            MMGameEvent.Trigger("TriggerSave");
            MMGameEvent.Trigger("CharacterStatChanged");

            RefundFeedback?.PlayFeedbacks();
            PopupElement.TryGetInstance().DisplayPopup("Refund Successful", "All stat levels reset and coins spent refunded!");

            return;
        });


    }
}
