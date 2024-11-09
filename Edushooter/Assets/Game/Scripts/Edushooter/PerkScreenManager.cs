using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using UnityEngine;
using UnityEngine.EventSystems;

public class PerkScreenManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [Header("Possible Perks")]
    public PerkStorage PerkList;

    [Header("Dependencies")]
    [SerializeField]
    private CanvasGroup UpgradePanel;

    [Space]
    [SerializeField] private PerkSelection[] PerkCards;

    [Space,Space]

    [SerializeField] private Sprite CommonCard;
    [SerializeField] private Sprite ExoticCard;
    [SerializeField] private Sprite LegendaryCard;

    PerkScriptable _perkData;
    Sprite _cardSprite;

    [Space, Space]

    [Header("Feedbacks")]
    public MMF_Player OpenFeedback;

    [Space,Space]
    [SerializeField]
    [MMReadOnly]
    private bool ShuffleAvailable = true;

    //insert the data to the perks
    private void InsertPerkData()
    {
        foreach (PerkSelection perk in PerkCards)
        {
            _perkData = PerkList.GetPerk();

            if (_perkData.Rarity.Equals(PerkScriptable.PerkRarity.Common))
            {
                _cardSprite = CommonCard;
            }
            else if (_perkData.Rarity.Equals(PerkScriptable.PerkRarity.Exotic))
            {
                _cardSprite = ExoticCard;
            }
            else
            {
                _cardSprite = LegendaryCard;
            }

            perk._Initialize(_perkData, _cardSprite);
        }
    }


    private IEnumerator OpenPerkScreen()
    {
        yield return new WaitForSeconds(2f);

        //turn off navigation events
        EventSystem.current.sendNavigationEvents = false;

        //pause in-game time
        MMTimeManager.Current.SetTimeScaleTo(0f);

        //turn on the upgrade panel but set it invisible until we initialize all the perk cards
        UpgradePanel.gameObject.SetActive(true);
        UpgradePanel.alpha = 0f;

        //initialize perk cards from perkpools
        InsertPerkData();

        //show the perk selection screen
        UpgradePanel.alpha = 1f;

        OpenFeedback?.PlayFeedbacks();
        ShuffleAvailable = true;
    }

    private void Start()
    {
        UpgradePanel.gameObject.SetActive(false);
    }

    #region EventListener Implement
    private void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    private void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        if (eventType.EventName.Equals("IntermissionStart"))
        {
            StartCoroutine(OpenPerkScreen());
        }
        else if (eventType.EventName.Equals("UpgradeEnd"))
        {
            //turn on navigation events
            EventSystem.current.sendNavigationEvents = true;

            //resume in-game time
            MMTimeManager.Current.SetTimeScaleTo(1f);

            //heal the player
            // gets the first player in the list of players
            Character player = LevelManager.Current.Players[0];

            if (player.gameObject.TryGetComponent<Health>(out var _playerHealth))
            {
                //heal the player
                _playerHealth.ReceiveHealth(0.5f * (_playerHealth.MaximumHealth - _playerHealth.CurrentHealth), gameObject);
            }

            //turn off the upgrade panel
            UpgradePanel.gameObject.SetActive(false);
        }
        else if (eventType.EventName.Equals("ShufflePerks"))
        {
            if (ShuffleAvailable)
            {
                ShuffleAvailable = false;
                InsertPerkData();
            }
        }
    }
    #endregion
}
