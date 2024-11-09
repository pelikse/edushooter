using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.TopDownEngine;
using MoreMountains.Tools;
using System.Drawing.Text;
using MoreMountains.Feedbacks;

public class CharacterAbilityTeleport : CharacterAbility
{
    public enum TeleportAbilityState
    {
        Idle,           // the ability is ready to use and has full charges
        Available,     // the ability is still usable even though the charges aren't full
        Cooldown        // the ability cannot be used because the charges are empty
    }

    public override string HelpBoxText() { return "Character Ability Teleport."; }

    [Header("Teleport")]
    // the distance of the teleports
    public float TeleportDistance = 5;

    // the origin of the teleport ray
    public Transform RaycastOrigin;

    // what can block teleports? uses raycast
    public LayerMask TeleportBlockers;
    // what can the player possibly teleport into?
    public LayerMask TeleportPassers;

    [Space]

    public Transform CharacterTransform;

    [Space]

    [Header("Cooldown and Charges")]
    
    public float CooldownDuration;

    [Space,Space]

    // the state of the ability currently
    [SerializeField]
    [MMReadOnly]
    public TeleportAbilityState AbilityState = TeleportAbilityState.Idle;

    public int MaxCharges = 2;

    [Space]
    [Header("Feedbacks")]
    public MMF_Player CooldownCompleteFeedback;

    [Space]

    [Header("Debugging")]
    [SerializeField]
    [MMReadOnly]
    private float CooldownTimer = 0f;

    [SerializeField]
    [MMReadOnly]
    public int ChargesRemaining;

    // private variables
    private bool _recharging = false;
    private float _collisionCheckRadius = 0.5f;
    

    protected override void Initialization()
    {
        base.Initialization();

        AbilityState = TeleportAbilityState.Idle;
        ChargesRemaining = MaxCharges;

        if (RaycastOrigin == null) RaycastOrigin = gameObject.transform;
        if (CharacterTransform == null) CharacterTransform = gameObject.transform;
    }

    public override void ProcessAbility()
    {
        base.ProcessAbility();

        if (_recharging)
        {
            CooldownTimer += Time.deltaTime;

            //if we finished cooldown then refill all charges and go into idle state
            if (CooldownTimer > CooldownDuration)
            {
                ChargesRemaining = MaxCharges;
                ChangeState(TeleportAbilityState.Idle);

                CooldownCompleteFeedback?.PlayFeedbacks();
            }
        }
    }

    protected override void HandleInput()
    {
        //detect something from the input manager to tell we should start the teleport
        //also make sure that we're not in cooldown
        if (_inputManager.TeleportAbilityButton.State.CurrentState == MMInput.ButtonStates.ButtonDown)
        {
            Teleport();
        }
    }

    protected virtual void Teleport()
    {
        // if we're in total cooldown then don't do anything
        if (AbilityState.Equals(TeleportAbilityState.Cooldown)) return;

        ChargesRemaining--;
        Debug.Log("dash charges is " +  ChargesRemaining);

        //if we run out of charges then enter full cooldown
        if (ChargesRemaining <= 0)
        {
            ChangeState(TeleportAbilityState.Cooldown);
        }
        //else we are still available
        else
        {
            ChangeState(TeleportAbilityState.Available);
        }

        float _teleportDistance = TeleportDistance;

        //fire a ray, make sure it doesnt pass through obstacles since we're using that for our level boundaries
        RaycastHit hit;

        if (Physics.Raycast(RaycastOrigin.position, RaycastOrigin.TransformDirection(Vector3.forward), out hit, TeleportDistance, TeleportBlockers))
        {
            _teleportDistance = hit.distance;
        }

        //move the player to that position forward
        Vector3 _initialPos = CharacterTransform.position;
        CharacterTransform.position += (_teleportDistance * RaycastOrigin.TransformDirection(Vector3.forward));

        //check for player position, if they are inside a collider then shunt them back towards their initial position
        if (PlayerIsInsideCollider(CharacterTransform.position))
        {
            ShuntToInitialPosition(_initialPos);
        }

        AbilityStopFeedbacks?.PlayFeedbacks();
    }

    private bool PlayerIsInsideCollider(Vector3 currentPos)
    {
        Collider[] colliders = Physics.OverlapSphere(currentPos, _collisionCheckRadius, TeleportPassers);
        return colliders.Length > 0;
    }

    private void ShuntToInitialPosition(Vector3 initialPosition)
    {
        Vector3 directionToInitial = (initialPosition - transform.position).normalized;
        float adjustmentStep = 0.1f; // Step size for adjustment

        while (PlayerIsInsideCollider(CharacterTransform.position))
        {
            transform.position += directionToInitial * adjustmentStep;

            // Prevent infinite loop (optional, adjust threshold as needed)
            if (Vector3.Distance(transform.position, initialPosition) <= adjustmentStep)
            {
                break;
            }
        }
    }

    // change ability states
    private void ChangeState(TeleportAbilityState state)
    {
        AbilityState = state;

        // if we're not idle, we have to track the cooldown AND reset the timer
        if (!AbilityState.Equals(TeleportAbilityState.Idle)) {
            CooldownTimer = 0f;
            _recharging = true;
        }
        // if we're going into idle, then we stop tracking cooldown
        else
        {
            CooldownTimer = 0f;
            _recharging = false;
        }
    }

    // set a new maximum charge
    public void SetMaxCharges(int maxCharges)
    {
        MaxCharges = maxCharges;

        if (AbilityState.Equals(TeleportAbilityState.Idle)) {
            ChargesRemaining = maxCharges;
        }
    }
}
