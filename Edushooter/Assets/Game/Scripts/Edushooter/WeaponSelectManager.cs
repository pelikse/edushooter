using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponSelectManager : MonoBehaviour, MMEventListener<MMGameEvent>
{


    [Header("Select Button References")]

    public ModifiableButtons SelectButton;
    public TextMeshProUGUI SelectionText;

    [Space, Space]
    [Header("Button States")]
    [TextArea(3,2)]
    [SerializeField] private string PurchaseText;
    [SerializeField] private Sprite PurchaseSprite;

    [Space]
    [TextArea(3, 2)]
    [SerializeField] private string EquipText;
    [SerializeField] private Sprite EquipSprite;

    [Space]
    [TextArea(3, 2)]
    [SerializeField] private string NeutralText;
    [SerializeField] private Sprite NeutralSprite;

    [Space]
    [TextArea(3, 2)]
    [SerializeField] private string EquippedText;
    [SerializeField] private Sprite EquippedSprite;

    [Space, Space]
    [Header("Weapon Purchase Cost")]
    public int ShotgunCost = 0;
    public int SniperCost = 0;
    public int SmgCost = 0;
    public int GatlingCost = 0;
    public int RocketCost = 0;
    public int FlameCost = 0;

    [Space,Space]
    [Header("Debugging")]
    [SerializeField]
    [MMReadOnly]
    private WeaponType _selectedWeapon;

    private bool _playerHasSelected;


    private int GetWeaponCost(WeaponType weapon)
    {
        switch(weapon)
        {
            case WeaponType.Shotgun:
                return ShotgunCost;

            case WeaponType.Smg:
                return SmgCost;

            case WeaponType.Sniper:
                return SniperCost;

            case WeaponType.Gatling:
                return GatlingCost;

            case WeaponType.Rpg:
                return RocketCost;

            case WeaponType.Flamethrower:
                return FlameCost;

            default:
                return 100;
        }
    }

    public void SelectWeapon(WeaponType weapon)
    {
        _selectedWeapon = weapon;
        _playerHasSelected = true;

        //if the weapon is already unlocked
        if (ProgressManager.TryGetInstance().WeaponIsUnlocked(weapon))
        {
            //if the weapon is equipped
            if (ProgressManager.TryGetInstance().LocalCache.EquippedWeapon == weapon)
            {
                SelectButton.ChangeInitialSprite(EquippedSprite);
                SelectionText.text = EquippedText;
            }
            else
            {
                SelectButton.ChangeInitialSprite(EquipSprite);
                SelectionText.text = EquipText;
            }
        }
        //if it's locked then prompt them to buy
        else
        {
            SelectButton.ChangeInitialSprite(PurchaseSprite);
            SelectionText.text = string.Format(PurchaseText, GetWeaponCost(weapon));
        }
    }

    public void EquipSelectedWeapon()
    {
        PlayerEdushooterData data = ProgressManager.TryGetInstance().ExternalData.EdushooterStats;

        //if the player hasnt selected anything then dont equip
        if (!_playerHasSelected)
        {
            PopupElement.TryGetInstance().DisplayPopup("NO WEAPON SELECTED","Please select a weapon to equip/purchase before proceeding!");
            return;
        }

        //if the weapon is already unlocked
        if (ProgressManager.TryGetInstance().WeaponIsUnlocked(_selectedWeapon))
        {
            //try equipping the weapon, if it fails then we should display a popup
            if (!ProgressManager.TryGetInstance().SetEquippedWeapon(_selectedWeapon))
            {
                PopupElement.TryGetInstance().DisplayPopup("WEAPON LOCKED", "Please purchase the weapon before equipping!");
                return;
            }

            SelectButton.ChangeInitialSprite(EquippedSprite);
            SelectionText.text = EquippedText;
            //save equipped weapon
            MMGameEvent.Trigger("TriggerSave");
            MMGameEvent.Trigger("UpdatedCharacterModel");
        }
        //if the weapon isn't unlocked
        else
        {
            //try purchasing the weapon and display a popup according to the results
            if (!ProgressManager.TryGetInstance().UnlockEquippedWeapon(_selectedWeapon, GetWeaponCost(_selectedWeapon)))
            {
                PopupElement.TryGetInstance().DisplayPopup("NOT ENOUGH COINS", "You do not have enough coins to purchase this weapon!");
                SFXPlayer.TryGetInstance()?.PlayCancelButtonPress();
                return;
            }
            else
            {
                //unlock the weapon and signify the equip
                MMGameEvent.Trigger("TriggerSave");

                SFXPlayer.TryGetInstance().PlayPurchaseSFX();

                SelectButton.ChangeInitialSprite(EquipSprite);
                SelectionText.text = EquipText;
            }
        }
    }



    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public virtual void OnMMEvent(MMGameEvent gameEvent)
    {
        switch (gameEvent.EventName)
        {
            case "SwitchedPanel":
                //reset the weapon selection
                _playerHasSelected = false;

                SelectButton.ChangeInitialSprite(NeutralSprite);
                SelectionText.text = NeutralText;
                break;
        }
    }
}
