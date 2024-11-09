using JetBrains.Annotations;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownStartManager : MonoBehaviour, MMEventListener<MMGameEvent>
{
    [MMInformation("This manager manages the overlay that shows the countdown when a game starts, ends, and the associated sound effects.", MMInformationAttribute.InformationType.None, false)]

    [Header("Countdown Audio")]
    [SerializeField] protected MMF_Player CountdownFeedback; // the tone for normal countdown
    [SerializeField] protected MMF_Player GameStartFeedback; // the tone for the last countdown
    [SerializeField] protected MMF_Player GameEndFeedback; // the tone for the end of the session

    [Space, Space]

    [Header("Countdown Attributes")]
    [SerializeField] protected float CountdownDelay = 1f; // how long should each countdown be?
    [SerializeField] protected int CountdownNumber = 3; // the number of counts

    [Space]
    [SerializeField] protected Image Dimmer;
    [SerializeField] protected TextMeshProUGUI CountdownTextOverlay; // a text object to display countdown
    [SerializeField] protected VictoryOverlay VictoryOverlay; //a text object to display the victory screen


#region EventListenerImplement
    //implement event listener
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
        switch (eventType.EventName)
        {
            case "SessionFinished":
                EndSession();
                break;

            case "StartSessionCountdown":
                StartCoroutine(StartCountdown());
                break;
        }
    }
#endregion


    private void EndSession()
    {
        ToggleDimmer(true);
        PlaySound(GameEndFeedback);
        VictoryOverlay.gameObject.SetActive(true);

        //play victory animation
        MMGameEvent.Trigger("CharacterAnimationLoopVictory");
        MMGameEvent.Trigger("CharacterAnimationTriggerVictory");
    }

    private void PlaySound(MMF_Player player)
    {
        player.PlayFeedbacks();
    }

    private IEnumerator StartCountdown()
    {
        //prepare variable for countdown
        int count = CountdownNumber;

        //turn off player UI and controls, then turn on the countdown UI
        ToggleDimmer(true);
        CountdownTextOverlay.gameObject.SetActive(true);
        TogglePlayerUI(false);

        //display the countdown and play the tone
        while (count > 0)
        {
            CountdownTextOverlay.text = count--.ToString();

            PlaySound(CountdownFeedback);

            yield return new WaitForSeconds(CountdownDelay);
        }

        //turn on the player UI and end the countdown, play starting tone
        TogglePlayerUI(true);

        PlaySound(GameStartFeedback);

        CountdownTextOverlay.gameObject.SetActive(false); //turn off the counter after we're done with it
        ToggleDimmer(false); //turn off the dimmer

        //start the gameplay session
        MMGameEvent.Trigger("StartSessionGameplay");
    }

    // this toggles the player UI visibility, setting it to inactive breaks the game for some reason
    // FIX LATER
    private void TogglePlayerUI(bool state)
    {
        GUIManager _GUIManager = GUIManager.Instance;
        if (_GUIManager != null)
        {
            if (state)
            {
                _GUIManager.HUD.GetComponent<CanvasGroup>().alpha = 1;
            }
            else if (!state)
            {
                _GUIManager.HUD.GetComponent<CanvasGroup>().alpha = 0;
            }
        }
    }

    private void ToggleDimmer(bool state)
    {
        Dimmer.gameObject.SetActive(state);
    }


    private void Start()
    {
        // make sure overlay is invisible at the start of the game
        Dimmer.gameObject.SetActive(false);
        CountdownTextOverlay.gameObject.SetActive(false);
        VictoryOverlay.gameObject.SetActive(false);
    }
}
