using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UserSelectionPanel : MonoBehaviour
{
    [SerializeField] private GameObject SelectionContainer;


    private void Start()
    {
        SelectionContainer.SetActive(false);
    }

    public void PromptSelection()
    {
        SelectionContainer.SetActive(true);
    }

    public void CloseSelection()
    {
        SelectionContainer?.SetActive(false);
    }
}
