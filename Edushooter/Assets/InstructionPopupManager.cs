using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InstructionPopupManager : MMSingleton<InstructionPopupManager>
{
    public GameObject InstructionGroup;

    [Space]

    public Image InstructionGraphic;
    public TextMeshProUGUI InstructionDescription;
    public TextMeshProUGUI InstructionTitle;

    [Space]

    public TextMeshProUGUI LeftBtnTxt;
    public TextMeshProUGUI RightBtnTxt;

    [Space]
    public MMF_Player PopupFeedback;


    //cache
    private InstructionPopup CurrentInstruction;
    private short InstructionIndex;


    const string DEFAULT_LEFT_TEXT = "PREV";
    const string DEFAULT_RIGHT_TEXT = "NEXT";
    const string DEFAULT_CLOSE_TEXT = "DONE";

    private void Start()
    {
        InstructionGroup.SetActive(false);
    }

    public void DisplayInstruction(InstructionPopup instruction)
    {
        //perform mandatory checks
        if (instruction == null || instruction.Panels.Length <= 0)
        {
            Debug.LogError("trying to display misformatted instructions!");
            return;
        }

        //pause the game
        MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 0f, 0f, false, 0f, true);
        SFXPlayer.TryGetInstance().PlayPopupSFX();

        //initialize
        CurrentInstruction = instruction;
        InstructionIndex = 0;
        InstructionTitle.text = instruction.Title;

        AssignInstructionDetails();

        //turn on the instruction panel
        InstructionGroup.SetActive(true);
    }

    public void NextInstructionPanel()
    {
        if (CurrentInstruction != null)
        {
            InstructionIndex++;
            
            //if we've exceeded the index
            if (InstructionIndex >= CurrentInstruction.Panels.Length)
            {
                //turn off instructions
                InstructionGroup.SetActive(false);

                //resume time
                MMTimeScaleEvent.Trigger(MMTimeScaleMethods.For, 1f, 0f, false, 0f, true);
                return;
            }

            AssignInstructionDetails();
        }
    }

    public void PreviousInstructionPanel()
    {
        if (CurrentInstruction != null)
        {
            InstructionIndex--;
            InstructionIndex = (short)Mathf.Clamp(InstructionIndex, 0, CurrentInstruction.Panels.Length - 1);

            AssignInstructionDetails();
        }
    }

    private void AssignInstructionDetails()
    {
        InstructionGraphic.sprite = CurrentInstruction.Panels[InstructionIndex].Graphic;
        InstructionDescription.text = CurrentInstruction.Panels[InstructionIndex].Description;

        LeftBtnTxt.text = DEFAULT_LEFT_TEXT;

        if (InstructionIndex == CurrentInstruction.Panels.Length - 1)
        {
            RightBtnTxt.text = DEFAULT_CLOSE_TEXT;
        }
        else
        {
            RightBtnTxt.text = DEFAULT_RIGHT_TEXT;
        }
    }
}
