using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupButton : MonoBehaviour
{
    public PopupInfo PopupInfo;
    
    public void TriggerPopup()
    {
        PopupElement.Current.DisplayPopup(PopupInfo);
    }

}
