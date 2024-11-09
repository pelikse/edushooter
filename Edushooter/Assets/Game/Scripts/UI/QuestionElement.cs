using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestionElement : MMSingleton<QuestionElement>
{
    [SerializeField] private GameObject QuestionGroup;
    [SerializeField] private MMF_Player PopupSoundPlayer;
    [Space]
    [SerializeField] private TextMeshProUGUI QuestionDescription;
    
    public delegate void AcceptCallback();

    private AcceptCallback _callbackOnAccept;

    public void DisplayQuestion(QuestionInfo info, AcceptCallback accept)
    {
        QuestionGroup.SetActive(true);  
        PopupSoundPlayer.PlayFeedbacks();

        QuestionDescription.text = info.QuestionDescription;

        _callbackOnAccept = accept;
    }

    public void DisplayQuestion(string question, AcceptCallback accept)
    {
        QuestionGroup.SetActive(true);
        PopupSoundPlayer.PlayFeedbacks();

        QuestionDescription.text = question;

        _callbackOnAccept = accept;
    }

    public void AcceptQuestion()
    {
        _callbackOnAccept.Invoke();

        //close the popup;
        CloseQuestion();
    }

    public void CloseQuestion()
    {
        _callbackOnAccept = null;

        QuestionGroup?.SetActive(false);
    }

    private void Start()
    {
        QuestionGroup?.SetActive(false);
    }
}
