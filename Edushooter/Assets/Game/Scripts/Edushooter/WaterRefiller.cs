using MoreMountains.Feedbacks;
using MoreMountains.InventoryEngine;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// inherits from pickable item
public class WaterRefiller : PickableItem
{
    [Space]
    [Space]
    // attributes special to the water refiller script
    [Header("Water Refiller")]
    public MMF_Player Feedback;
    public float Cooldown = 30f; // cooldown for the refiller
    public InventoryItem WaterAmmo;
    public SpriteFillBar CooldownBar;
    
    private float _lastUseTimestamp; // a tracker for cooldown
    private bool _cooldown = false; // the state that the refiller is in right now

    // for debugging
    [SerializeField]
    [MMReadOnly]
    private float CurrentTimer;

    // when the player enters the refill zone, refill water to max
    protected override void Pick(GameObject picker)
    {
        // if its not in cooldown, then give water and turn on cooldown
        if (!_cooldown )
        {
            Feedback.PlayFeedbacks();
            _cooldown = true;
            _lastUseTimestamp = Time.time;

            WeaponAmmo currentAmmo = picker.GetComponent<CharacterHandleWeapon>().CurrentWeapon.GetComponent<WeaponAmmo>();
            if (currentAmmo != null)
            {
                currentAmmo.AmmoInventory.AddItem(WaterAmmo, (currentAmmo.MaxAmmo - currentAmmo.CurrentAmmoAvailable));
                currentAmmo.FillWeaponWithAmmo();
            }
        }
    }

    // check for cooldown every frame
    private void Update()
    {
        // if its in cooldown, then check if time has passed enough to remove cooldown
        if (_cooldown)
        {
            CurrentTimer = Time.time - _lastUseTimestamp;
            CooldownBar.UpdateFill(CurrentTimer, Cooldown);

            if (CurrentTimer >= Cooldown)
            {
                CooldownBar.UpdateFill(Cooldown, Cooldown);
                _cooldown = false;
            }
        }
    }

}
