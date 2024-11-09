using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EdugymSelector : MonoBehaviour
{
    [SerializeField] private EdugymDescription SelectorGameInfo;
    [SerializeField] private EdugymSelectionManager SelectionManager;

    public void SelectGame()
    {
        SelectionManager.SetSelectedGame(SelectorGameInfo);
    }
}
