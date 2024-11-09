using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerModelChanger : MonoBehaviour, MMEventListener<MMGameEvent>
{
    //[System.Serializable]
    //public class EquippedWeapon
    //{
    //    public PlayerEdushooterData.WeaponType Type;
    //    public GameObject WeaponModel;
    //}

    [System.Serializable]
    public class WeaponModelSet
    {
        [MMInformation("Weapon Type that the entry signifies. WeaponModel is the model that is present on the model changed. WeaponAnimator is the proper animator for the model.", MMInformationAttribute.InformationType.Info, false)]
        public WeaponType Type;
        public GameObject WeaponModel;
        public RuntimeAnimatorController WeaponAnimator;
    }

    [MMInformation("This script toggles the selected models on the player character and handles switching them.", MMInformationAttribute.InformationType.None, false)]
    public bool ChangeWeaponModel = true;

    [Space]

    public WeaponType SelectedWeapon;
    public ModelType SelectedHead;
    public ModelType SelectedBody;

    [Space]

    public Animator ModelAnimator;

    [Space]

    [SerializeField]
    [MMReadOnly]
    private GameObject ActiveHead;

    [SerializeField]
    [MMReadOnly]
    private GameObject ActiveBody;

    [SerializeField]
    [MMReadOnly]
    private GameObject ActiveWeapon;

    [Space,Space]

    [Header("List of Models")]
    [SerializeField] private GameObject[] CharacterBody;
    [SerializeField] private GameObject[] CharacterHead;

    [MMInformation("Make sure that there is an entry for each type of weapon!", MMInformationAttribute.InformationType.Warning, false)]
    [SerializeField]
    [MMReadOnly]
    private bool _info;

    [SerializeField] private WeaponModelSet[] Arsenal;

#region Resetters
    public void TurnOffSkin()
    {
        ResetWeapon();
        ResetHead();
        ResetBody();
    }

    private void ResetWeapon()
    {
        if (!ChangeWeaponModel) return;

        // turn off every single weapon
        foreach (var weapon in Arsenal)
        {
            weapon.WeaponModel.SetActive(false);
        }
    }

    private void ResetHead()
    {
        //toggles off all heads
        foreach (var head in CharacterHead)
        {
            head.SetActive(false);
        }
    }

    private void ResetBody()
    {
        //toggles off all bodies
        foreach (var body in CharacterBody)
        {
            body.SetActive(false);
        }
    }
#endregion

    public void ChangeHead(ModelType head)
    {
        //if the head to change is the same as the head that we already have, then don't do anything
        if (ActiveHead == CharacterHead[(int)head]) return;

        ResetHead();
        ActiveHead = CharacterHead[(int)head];

        ActiveHead.SetActive(true);
    }

    public void ChangeBody(ModelType body)
    {
        //if the body to change is the same as the body that we already have, then don't do anything
        if (ActiveBody == CharacterBody[(int)body]) return;

        ResetBody();
        ActiveBody = CharacterBody[(int)body];

        ActiveBody.SetActive(true);
    }

    public void ChangeWeapon(WeaponType weapon)
    {
        //if the weapon to change is the same as the weapon that we already have or we don't enable change weapon model, then don't do anything
        if (ActiveWeapon == Arsenal[(int)weapon].WeaponModel || !ChangeWeaponModel)
        {
            Debug.Log("trying to change weapon but cancelled\ncurrent = " + SelectedWeapon + " and changed is " + weapon);
            return;
        }

        ResetWeapon();

        WeaponModelSet _setWeapon = Arsenal[0];
        bool _foundWeapon = false;

        //try and get reference to the correct weapon, if not then just return the first one
        foreach (WeaponModelSet wep in Arsenal)
        {
            if (wep.Type == weapon)
            {
                _setWeapon = wep;
                _foundWeapon = true;
            }
        }

        //if there's no weapon inside the arsenal, then just use the first weapon.
        if (!_foundWeapon)
        {
            Debug.LogWarning("arsenal lacks a weapon type, fix this!");
        }

        Debug.Log(_setWeapon.ToString());
        //change animator
        if (_setWeapon.WeaponAnimator != null)
        {
            ModelAnimator.runtimeAnimatorController = _setWeapon.WeaponAnimator;
        }
        else
        {
            Debug.LogWarning("No controller present in arsenal!");
        }

        //turn on the weapon model
        ActiveWeapon = _setWeapon.WeaponModel;

        ActiveWeapon.SetActive(true);
    }

    private void Start()
    {
        GetSkinDataFromSave();

        ChangeWeapon(SelectedWeapon);
        ChangeBody(SelectedBody);
        ChangeHead(SelectedHead);
    }

    public void GetSkinDataFromSave()
    {
        //gets the relevant data about equipped player skins and assigns it
        LocalEdushooterStorage data = ProgressManager.TryGetInstance().LocalCache;

        SelectedBody = data.BodyModel;
        SelectedHead = data.HeadModel;

        SelectedWeapon = data.EquippedWeapon;
    }

    public void PlayVictoryAnimation()
    {
        ModelAnimator.SetTrigger("Celebrate");
    }

    public void SetModels()
    {
        GetSkinDataFromSave();

        ChangeWeapon(SelectedWeapon);
        ChangeBody(SelectedBody);
        ChangeHead(SelectedHead);
    }


    #region EventListeners
    protected virtual void OnEnable()
    {
        this.MMEventStartListening<MMGameEvent>();
    }

    protected virtual void OnDisable()
    {
        this.MMEventStopListening<MMGameEvent>();
    }

    public void OnMMEvent(MMGameEvent eventType)
    {
        switch (eventType.EventName)
        {
            case "SwitchedPanel":
                SetModels();
                break;
                
            case "UpdatedCharacterModel":
                SetModels();
                break;

            case "SwitchedEquipmentTab":
                SetModels();
                break;

            case "CharacterAnimationTriggerVictory":
                ModelAnimator.SetTrigger("Celebrate");
                break;

            case "CharacterAnimationLoopVictory":
                ModelAnimator.SetBool("Celebrating", true);
                break;

            case "CharacterAnimationTriggerFailure":
                Debug.Log("playing failure character animation");
                ModelAnimator.SetTrigger("Failed");
                break;

            case "CharacterAnimationTriggerPerfect":
                ModelAnimator.SetTrigger("Perfect");
                break;
        } 

    }
    #endregion
}
