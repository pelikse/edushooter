using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InstructionPopup", menuName = "Popups/Instruction")]
public class InstructionPopup : ScriptableObject
{
    [System.Serializable]
    public class InstructionPanels
    {
        [TextArea(3,3)]
        public string Description;
        public Sprite Graphic;
    }

    public string Title;
    public InstructionPanels[] Panels;
}
