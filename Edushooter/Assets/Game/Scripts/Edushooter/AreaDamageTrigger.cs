using MoreMountains.Tools;
using UnityEngine;
using UnityEngine.Serialization;

public class AreaDamageTrigger : MonoBehaviour
{
    [MMInformation("This script can turn on a DamageOnTouch component's collider for a certain duration. Make sure a collider is referenced.", MMInformationAttribute.InformationType.Warning, true)]
    [FormerlySerializedAs("Info")]
    [SerializeField]
    [MMReadOnly]
    private bool _info;

    [Space]

    [SerializeField] protected GameObject Hurtbox;
    [SerializeField] protected float DamageDuration;

    //cache
    //private int i = 0;
    private bool _active = false;
    private float _lastActive;

    private void Start()
    {
        Hurtbox.SetActive(false);
    }

    private void Update()
    {
        if (_active)
        {
            //if we've exceeded the duration
            if (Time.time - _lastActive >= DamageDuration)
            {
                //turn off
                _active = false;

                Hurtbox.SetActive(false);
            }
        }
    }

    public void DisableHurtbox()
    {
        if (_active)
        {
            _active = false;
            Hurtbox.SetActive(false);
        }
    }

    public void ActivateHurtbox()
    {
        //track duration
        _lastActive = Time.time;

        //turn on the Hurtbox
        Hurtbox.SetActive(true);

        //start timer
        _active = true;
    }
}
