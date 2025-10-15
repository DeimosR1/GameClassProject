using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    List<BattleSite> mBattleSites;

    [SerializeField] List<BattleCharacter> mBattleCharacter = new List<BattleCharacter>();

    public bool IsInBattle;
    public void StartBattle(BattlePartyComponent playerParty, BattlePartyComponent enemyParty)
    {
        mBattleCharacter.Clear();
        if (mBattleSites == null)
        {
            mBattleSites = new List<BattleSite>();
            mBattleSites.AddRange(GameObject.FindObjectsByType<BattleSite>(FindObjectsSortMode.None));
        }

        PrepParty(playerParty);
        PrepParty(enemyParty);
        StartCoroutine(StartTurns());
    }

    private IEnumerator StartTurns()
    {
        //TODO: Refacto to not hard code the delay
        yield return new WaitForSeconds(2);
        Debug.Log("Started Turnes");
        NextTurn();
    }

    void NextTurn()
    {
        Debug.Log("Started next Turn");
        mBattleCharacter = mBattleCharacter.OrderBy((battleCharacter) => { return battleCharacter.CooldownDuration; }).ToList();
        float globalCooldown = mBattleCharacter[0].CooldownDuration;
        BattleCharacter battleCharacter1 = mBattleCharacter[0];

        foreach (BattleCharacter battleCharacter in mBattleCharacter)
        {
            battleCharacter.AdvanceCooldown(globalCooldown);
            if (battleCharacter.CooldownTimeRemaining <= 0)
            {
                battleCharacter.TakeTurn();
            }
            globalCooldown = battleCharacter.CooldownDuration;
        }
        mBattleCharacter.Remove(mBattleCharacter[0]);
        mBattleCharacter.Add(battleCharacter1);

    }

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
            partyBattleCharacter.OnTurnFinished += NextTurn;
            mBattleCharacter.Add(partyBattleCharacter);
            i++;
        }
    }
}
