using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResetProgressManager : MonoBehaviour
{
    public QuestionInfo ResetQuestion;

    private QuestionElement.AcceptCallback callback;

    public void TryResetProgress()
    {
        callback = () =>
        {
            ProgressManager.TryGetInstance().ResetSaveData();
            
            Debug.Log("reset progress and quit application");

            Application.Quit();
        };

        QuestionElement.TryGetInstance().DisplayQuestion(ResetQuestion, callback);
    }
}
