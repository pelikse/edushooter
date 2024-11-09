using MoreMountains.Tools;
using MoreMountains.TopDownEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[AddComponentMenu("TopDown Engine/Character/AI/Decisions/AIDecisionChargeReady")]
public class AIDecisionChargeReady : AIDecision
{
    protected CharacterDash3D _characterDash3D;

    /// <summary>
    /// On Init we store our CharacterHandleWeapon
    /// </summary>
    public override void Initialization()
    {
        base.Initialization();
        _characterDash3D = this.gameObject.GetComponentInParent<Character>()?.FindAbility<CharacterDash3D>();
    }

    /// <summary>
    /// On Decide we return true if a reload is needed
    /// </summary>
    /// <returns></returns>
    public override bool Decide()
    {
        if (_characterDash3D == null)
        {
            return false;
        }

        return _characterDash3D.Cooldown.CooldownState.Equals(MMCooldown.CooldownStates.Idle);
    }
}
