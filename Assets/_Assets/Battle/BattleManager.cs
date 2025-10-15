using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager
{

    List<BattleSite> mBattleSites;
    public void StartBattle(BattlePartyComponent playerParty, BattlePartyComponent enemyParty)
    {
        if (mBattleSites == null)
        {
            mBattleSites = new List<BattleSite>();
            mBattleSites.AddRange(GameObject.FindObjectsByType<BattleSite>(FindObjectsSortMode.None));
        }

        PrepParty(playerParty);
        PrepParty(enemyParty);
    }

    private IEnumerator StartTurns()
    {
        //TODO: Refacto to not hard code the delay
        yield return new WaitForSeconds(2);
        NextTurn();
    }

    void NextTurn()

    private void PrepParty(BattlePartyComponent party)
    {
        BattleSite partyBattleSite = mBattleSites.Find((battleSite)=> { return !battleSite.IsPlayerSite; });
        if (party.gameObject.CompareTag("Player"))
        {
            partyBattleSite = mBattleSites.Find((battleSite) => { return battleSite.IsPlayerSite; });
        }

        int i = 0;
        foreach(BattleCharacter partyBattleCharacter in party.GetBattleCharacters())
        {
            partyBattleCharacter.transform.position = partyBattleSite.GetPositionForUnit(i);
            partyBattleCharacter.transform.rotation = partyBattleSite.transform.rotation;
            i++;
        }
    }
}
