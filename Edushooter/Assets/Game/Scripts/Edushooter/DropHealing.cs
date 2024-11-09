using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropHealing : MonoBehaviour
{
    [MMInformation("This script tells the healing orb manager to try and spawn a healing orb when an enemy dies. This should be placed ON THE MODEL since it is disabled BEFORE the enemy is disabled.", MMInformationAttribute.InformationType.Warning, true)]
    [MMReadOnly]
    public bool Info = true;

    public void TryDrop()
    {
        if (!Application.isPlaying)
        {
            return;
        }
        HealingOrbManager.Instance.TryDropHealing(transform.position);
    }

    private void OnDisable()
    {
        TryDrop();
    }
}
