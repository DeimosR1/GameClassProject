
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Abilities/BasicAttack")]
public class BasicAttack : Ability
{
    public override void ActivateAbility()
    {
        base.ActivateAbility();
        int partyID = OwningAbilityComponent.GetPartyID();
        List<BattleCharacter> targets = GameMode.MainGameMode.BattleManager.GetTargetsForTeam(partyID, true);
    }
}
