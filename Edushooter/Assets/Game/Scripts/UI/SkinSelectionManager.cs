using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkinSelectionManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [System.Serializable]
    public class SkinButtonState
    {
        public Sprite btnSprite;
        public string btnText;
    }

    [System.Serializable]
    public enum ButtonState
    {
        NoSelection,
        Purchasable,
        Equipabble,
        Equipped
    }

    public TextMeshProUGUI HeadDesc;
    public TextMeshProUGUI BodyDesc;

    [Space,Space]

    [Header("Button States")]
    [SerializeField] private SkinButtonState NoSelectionState;
    [SerializeField] private SkinButtonState PurchasableState;
    [SerializeField] private SkinButtonState EquippableState;
    [SerializeField] private SkinButtonState EquippedState;

    [Space, Space]

    public PlayerModelChanger PreviewModel;

    [Space]

    public SkinPriceFetcher SkinPrices;
    public QuestionInfo PurchaseQuestion;
    public PopupInfo FailedPurchasePopup;
    public MMF_Player PurchaseFeedback;

    [Space]

    public ModifiableButtons BodySelectButton;
    public TextMeshProUGUI BodySelectText;

    [Space]

    public ModifiableButtons HeadSelectButton;
    public TextMeshProUGUI HeadSelectText;

    [Space]

    [SerializeField]
    [MMReadOnly]
    private ButtonState CurrentBtnState;
    public ButtonState InitialBtnState;


    private SkinSelector _selectedSkin;

    public void SelectSkin(SkinSelector selected)
    {
        if (selected == null)
        {
            Debug.LogError("No Skin Selected!");
            return;
        }

        _selectedSkin = selected;

        // set the text to show selected skin
        HeadDesc.text = selected.modelType.ToString().ToUpper() + " HEAD";
        BodyDesc.text = selected.modelType.ToString().ToUpper() + " BODY";

        // preview the model and change button states
        // if we're previewing a head skin

        bool isHead = selected.skinType.Equals(SkinSelector.SkinType.Head);

        if (isHead)
        {
            PreviewModel.ChangeHead(selected.modelType);
            // if the head is already unlocked
            if (ProgressManager.TryGetInstance().HeadIsUnlocked(selected.modelType))
            {
                // if the head is equipped then dont prompt anything
                if (ProgressManager.TryGetInstance().LocalCache.HeadModel == selected.modelType)
                {
                    SwitchButtonStates(ButtonState.Equipped, isHead);
                }
                else
                {
                    // if the ehad is unequipped then prompt to equip
                    SwitchButtonStates(ButtonState.Equipabble, isHead);
                }
            }
            // the head is locked, then prompt to purchase
            else
            {
                SwitchButtonStates(ButtonState.Purchasable, isHead);
            }
        }
        else
        {
            PreviewModel.ChangeBody(selected.modelType);
            // if the body is already unlocked
            if (ProgressManager.TryGetInstance().BodyIsUnlocked(selected.modelType))
            {
                // if the body is equipped then dont prompt anything
                if (ProgressManager.TryGetInstance().LocalCache.BodyModel == selected.modelType)
                {
                    SwitchButtonStates(ButtonState.Equipped, isHead);
                }
                else
                {
                    // if the body is unequipped then prompt to equip
                    SwitchButtonStates(ButtonState.Equipabble, isHead);
                }

            }
            // the body is locked, then prompt to purchase
            else
            {
                SwitchButtonStates(ButtonState.Purchasable, isHead);
            }
        }
    }

    private void ChangeButton(bool isHeadBtn, Sprite btnSprite, string btnText)
    {
        if (isHeadBtn)
        {
            HeadSelectButton.ChangeInitialSprite(btnSprite);
            HeadSelectText.text = string.Format(btnText, SkinPrices.GetPrice().ToString());
        }
        else
        {
            BodySelectButton.ChangeInitialSprite(btnSprite);
            BodySelectText.text = string.Format(btnText, SkinPrices.GetPrice().ToString());
        }
    }

    public void SwitchButtonStates(ButtonState state, bool isHead)
    {
        CurrentBtnState = state;

        switch (CurrentBtnState)
        {
            case ButtonState.NoSelection:
                ChangeButton(isHead, NoSelectionState.btnSprite, NoSelectionState.btnText);
                break;

            case ButtonState.Purchasable:
                ChangeButton(isHead, PurchasableState.btnSprite, PurchasableState.btnText);
                break;

            case ButtonState.Equipabble:
                ChangeButton(isHead, EquippableState.btnSprite, EquippableState.btnText);
                break;

            case ButtonState.Equipped:
                ChangeButton(isHead, EquippedState.btnSprite, EquippedState.btnText);
                break;

            default:
                ChangeButton(isHead, NoSelectionState.btnSprite, NoSelectionState.btnText);
                break;
        }
    }

    public void TrySelectSkin()
    {
        switch (CurrentBtnState)
        {
            case ButtonState.Purchasable:
                //the callback for when the player accepts a purchase
                QuestionElement.AcceptCallback _acceptCallback = () =>
                {
                    Debug.Log("purchasing skin with gems callback");
                    //if the player can afford to purchase the gems
                    if (ProgressManager.TryGetInstance().TryReduceGems(SkinPrices.GetPrice(_selectedSkin.modelType)))
                    {
                        ProgressManager.Instance.UnlockSkin(_selectedSkin.modelType, _selectedSkin.skinType);
                        SwitchButtonStates(ButtonState.Equipabble, _selectedSkin.skinType.Equals(SkinSelector.SkinType.Head));

                        FirebaseHandler.TryGetInstance().UploadSkinPurchase(SkinPrices.GetPrice(_selectedSkin.modelType), _selectedSkin.modelType, _selectedSkin.skinType);

                        PurchaseFeedback.PlayFeedbacks();
                        MMGameEvent.Trigger("TriggerSave");
                    }
                    else
                    {
                        PopupElement.TryGetInstance().DisplayPopup(FailedPurchasePopup);
                    }
                };  

                QuestionElement.TryGetInstance().DisplayQuestion(PurchaseQuestion, _acceptCallback);
                break;

            case ButtonState.Equipabble:
                //change equipped skin
                bool _ishead = _selectedSkin.skinType.Equals(SkinSelector.SkinType.Head);

                if (_ishead)
                {
                    ProgressManager.TryGetInstance().SetEquippedHead(_selectedSkin.modelType);
                }
                else
                {
                    ProgressManager.TryGetInstance().SetEquippedBody(_selectedSkin.modelType);
                }

                SwitchButtonStates(ButtonState.Equipped, _ishead);
                MMGameEvent.Trigger("TriggerSave");
                break;

            default:
                return;
        }
    }

    #region EventListener Implement
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
        if (eventType.EventName.Equals("SwitchInventoryPanel"))
        {
            CurrentBtnState = InitialBtnState;
            SwitchButtonStates(CurrentBtnState, true);
        }
    }
    #endregion
}
