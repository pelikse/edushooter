using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TutorialTrigger : MonoBehaviour
{
    public TutorialPopup BoundPopup;

    [Space, Space]

    public UnityEvent OnTriggered;

    [Space]

    public bool TurnOffOnTrigger;

    private bool _canTrigger = true;

    private void Start()
    {
        _canTrigger = true;
    }

    private void TriggerTutorial(Collider other)
    {
        if (!_canTrigger) return;

        //if player enters the tutorial trigger
        if (other.transform.tag == "Player")
        {
            OnTriggered?.Invoke();

            //trigger popup
            TutorialPopupManager.TryGetInstance().TriggerTutorialPopup(BoundPopup);
            if (TurnOffOnTrigger)
            {
                _canTrigger = false;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        TriggerTutorial(other);
    }

    private void OnTriggerStay(Collider other)
    {
        TriggerTutorial(other);   
    }

    private void OnTriggerExit(Collider other)
    {
        TriggerTutorial(other);
    }
}
