using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AccountLinkPanels : MonoBehaviour
{
    public GameObject Container;

    // Start is called before the first frame update
    void Start()
    {
        ClosePanel();
    }

    public void ActivatePanel()
    {
        Container.SetActive(true);
    }

    public void ClosePanel()
    {
        Container.SetActive(false);
    }
}
