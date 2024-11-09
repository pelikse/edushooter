using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimAssistControls : MonoBehaviour
{
    public MeshRenderer aimAssist;

    public void ActivateAim()
    {
        aimAssist.enabled = true;
    }

    public void OnDestroy()
    {
        aimAssist.enabled = false;
    }
}
