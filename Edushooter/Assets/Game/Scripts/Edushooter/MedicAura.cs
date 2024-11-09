using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using MoreMountains.Tools;

public class MedicAura : MonoBehaviour
{
    public float DamageReduction = 0.6f;
    public float HealingAmount = 20f;
    
    [Space]

    public float CheckFrequency = 0.5f;
    public float AuraRange = 7f;
    public int OverlapMaximum = 5;

    public GameObject Owner;

    [Space,Space]

    public Collider EnemyCollider;
    public Vector3 DetectionOriginOffset = new Vector3(0, 0, 0);

    [Space]

    public LayerMask TargetLayerMask;

    [Space]

    public bool IgnoreOwner = true;

    protected Vector3 _raycastOrigin;
    protected List<Character> _medicTarget;
    protected float _lastCheck;

    [MMReadOnly]
    protected Collider[] _hits;

    protected TimedResistance _resistance;
    protected EnemyHealth _health;

    private void Update()
    {
        // we check if there's a need to detect a new target
        if (Time.time - _lastCheck > CheckFrequency)
        {
            AuraCheck();
        }
    }

    private void AuraCheck()
    {
        _lastCheck = Time.time;

        //calculate any colliders caught (only cares about enemy colliders)
        _raycastOrigin = EnemyCollider.bounds.center + DetectionOriginOffset / 2;
        _hits = Physics.OverlapSphere(_raycastOrigin, AuraRange, TargetLayerMask, QueryTriggerInteraction.Ignore);
        int numberOfCollidersFound = _hits.Length;

        // we go through each collider found
        int min = Mathf.Min(OverlapMaximum, numberOfCollidersFound);
        for (int i = 0; i < min; i++)
        {
            //if its null then skip
            if (_hits[i] == null)
            {
                continue;
            }

            if (IgnoreOwner)
            {
                //if its the owner or a child of the object then skip
                if (_hits[i].gameObject == Owner || (_hits[i].transform.IsChildOf(transform)))
                {
                    continue;
                }
            }

            // try and get the resistance, give it a new resistance value and duration
            if (_hits[i].TryGetComponent<TimedResistance>(out _resistance))
            {
                // if we want to ignore resistance then dont do anything
                if (_resistance.IgnoreResistance) continue;

                _resistance.AddResistance(CheckFrequency, DamageReduction);
            }

            // try and get enemy health and heal them
            if (_hits[i].TryGetComponent<EnemyHealth>(out _health))
            {
                _health.ReceiveHealth(HealingAmount, gameObject);
            }

        }
    }
}
