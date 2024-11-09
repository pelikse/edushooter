using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ReloadAlertListener : MonoBehaviour, MMEventListener<MMGameEvent>
{
    public CanvasGroup AlertText;
    public MMF_Player AlertStartFeedback;

    [Space]

    public float FadeDuration;
    public float FadeDelay;

    [SerializeField]
    [MMReadOnly]
    private bool _alerting = false;
    private float _fadeTimer = 0f;

    //turn off the alert text
    private void Start()
    {
        AlertText.alpha = 0;
    }

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
        if (eventType.EventName.Equals("ReloadAlertTrigger"))
        {
            //if not alerting then start the alerting
            if (!_alerting)
            {
                AlertText.alpha = 1;
                _alerting = true;

                //play the alert feedback
                AlertStartFeedback.PlayFeedbacks();
                StartCoroutine(FadeAlert());
            }
        }
    }

    private IEnumerator FadeAlert()
    {
        _fadeTimer = FadeDuration;

        //wait for delay to end
        yield return new WaitForSeconds(FadeDelay);

        //start fading
        while (AlertText.alpha > 0f || _fadeTimer > 0f) {
            _fadeTimer = Mathf.Clamp(_fadeTimer - Time.deltaTime, 0f, FadeDuration);

            AlertText.alpha = _fadeTimer / FadeDuration;

            yield return null;
        }

        //alert has stopped, can be restarted
        _alerting = false;
        StopAllCoroutines();
    }
}
