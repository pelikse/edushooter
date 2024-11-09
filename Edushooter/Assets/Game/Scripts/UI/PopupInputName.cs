using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PopupInputName : MMSingleton<PopupInputName>, MMEventListener<MMGameEvent>
{
    [SerializeField] private TextMeshProUGUI NameText;
    [SerializeField] private GameObject PopupInput;
    [SerializeField] private GameObject CloseBtn;

    public bool EnableCloseBtn = false;

    //private int MAX_NAME_LENGTH = 12;

    private void Start()
    {
        ClosePopup();
    }

    public void SavePlayerName()
    {
        string newName = NameText.text;

        //turn off the popup
        ClosePopup();

        //save the name into progress manager, only the first 100
        ProgressManager.TryGetInstance().ExternalData.DisplayName = newName;
        MMGameEvent.Trigger("TriggerSave");

        //assign to firebase
        FirebaseHandler.TryGetInstance().SetCharacterName(newName);

        //go to the tutorial level
        MMGameEvent.Trigger("GoToTutorial");
    }

    public void ActivatePopup()
    {
        PopupInput.SetActive(true);
        CloseBtn.SetActive(EnableCloseBtn);
    }

    public void ClosePopup()
    {
        PopupInput.SetActive(false);
        CloseBtn.SetActive(false);
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
        if (eventType.EventName == "SummonNamePopup")
        {
            ActivatePopup();
        }
    }
}
