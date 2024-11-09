using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialSeries : MonoBehaviour
{
    public InstructionPopup InitialPopup;
    public float InitialDelay;

    [Space]

    public bool PlayInitialPopup = true;

    // Start is called before the first frame update
    void Start()
    {
        if (PlayInitialPopup)
        {
            StartCoroutine(SummonInitialPopup());
        }
    }

    private IEnumerator SummonInitialPopup()
    {
        yield return new WaitForSeconds(InitialDelay);

        InstructionPopupManager.TryGetInstance().DisplayInstruction(InitialPopup);
    }
}
