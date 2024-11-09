using MoreMountains.TopDownEngine;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using System.Collections;

public class TriggerExplosion : MonoBehaviour
{
    [MMInformation(
        "This component is meant to be used for any sort of explosion and area damage. It turns on a hurtbox temporarily. You can also attach a feedback for the explosion.",
        MMInformationAttribute.InformationType.Info, false)]
    [Header("Explosion Hurtbox")]
    public DamageOnTouch Hurtbox;
    public float ExplosionDuration = 0.1f;
    [Space]
    public Health SelfHealth;
    public AIBrain SelfBrain;
    public MMF_Player ExplosionFeedback;
    [Space]
    public bool DestroyOnExplosion = false;


    //make sure that the hurtbox is disabled on start
    public void DisableExplosion()
    {
        Hurtbox.gameObject.SetActive(false);
    }

    public void Explode()
    {
        // play the explosion feedback
        ExplosionFeedback?.PlayFeedbacks();

        StartCoroutine(TurnOnHurtbox());

        // after explosion, kill self
        if (DestroyOnExplosion)
        {
            if (SelfHealth != null)
            {
                SelfHealth?.Kill();
            }
            if (SelfBrain != null)
            {
                SelfBrain.BrainActive = false;
            }
        }
    }

    private IEnumerator TurnOnHurtbox()
    {
        Hurtbox.gameObject.SetActive(true);

        yield return new WaitForSeconds(ExplosionDuration);

        Hurtbox.gameObject.SetActive(false);
    }
}
