using MoreMountains.Tools;
using UnityEngine;
//using UnityEditor.Animations;

public class GameplayModelChanger : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [MMInformation("This script toggles the selected models on the player character and handles switching them. This is a version of the script to use on the Player prefab.", MMInformationAttribute.InformationType.None, false)]

    [Space]

    [SerializeField]
    [MMReadOnly]
    private ModelType SelectedHead;

    [SerializeField]
    [MMReadOnly]
    private ModelType SelectedBody;

    [SerializeField]
    [MMReadOnly]
    private WeaponType SelectedWeapon;

    [Space]
    [Header("Character Animators")]
    public Animator ModelAnimator;

    [Space]
    /*
    [SerializeField] private AnimatorController ARController;
    [SerializeField] private AnimatorController ShotgunController;
    [SerializeField] private AnimatorController SniperController;
    [SerializeField] private AnimatorController SmgController;
    [SerializeField] private AnimatorController GatlingController;
    */

    [Space, Space]

    [Header("Player Models")]
    [SerializeField]
    [MMReadOnly]
    private GameObject ActiveHead;

    [SerializeField]
    [MMReadOnly]
    private GameObject ActiveBody;

    [Space, Space]

    [Header("List of Models")]
    [SerializeField] private GameObject[] CharacterBody;
    [SerializeField] private GameObject[] CharacterHead;

    #region Resetters
    public void TurnOffSkin()
    {
        ResetHead();
        ResetBody();
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

    private void Start()
    {
        GetSkinDataFromSave();

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
        if (eventType.EventName == "CharacterSpawned")
        {
            GetSkinDataFromSave();
        }
    }
}
