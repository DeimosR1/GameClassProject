using System.Collections.Generic;
using UnityEngine;

public interface ITargetService
{
    public List<BattleCharacter> GetTargetsForTeam(int teamId, bool hostileTargets);

    public TargetingComponent GetTargetingComponent();
}
