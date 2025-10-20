using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviour
{

    List<BattleSite> mBattleSites;

    [SerializeField] List<BattleCharacter> mBattleCharacter = new List<BattleCharacter>();

    Queue<BattleCharacter> mFirstRoundBattleCharacters = new Queue<BattleCharacter>();

    int mRoundNumber = 1;
    int mFirstTurnNextIndex;

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
        UpdateTurnOrder();
        mFirstRoundBattleCharacters = new Queue<BattleCharacter>(mBattleCharacter);
        ProcessFirstRound();
        Debug.Log("Started Turnes");
    }

    private void ProcessFirstRound()
    {
        if(mFirstRoundBattleCharacters.TryDequeue(out BattleCharacter nextBattleCharacter))
        {
            if (!mBattleCharacter.Contains(nextBattleCharacter)) { ProcessFirstRound(); return; }
            nextBattleCharacter.TakeTurn();
            return;
        }

        foreach(BattleCharacter battleCharacter in mBattleCharacter)
        {
            battleCharacter.OnTurnFinished -= ProcessFirstRound;
            battleCharacter.OnTurnFinished += NextTurn;
        }

        NextTurn();
    }

    void NextTurn()
    {
        UpdateTurnOrder();
        float globalCooldown = mBattleCharacter[0].CooldownDuration;

        foreach (BattleCharacter battleCharacter in mBattleCharacter)
        {
            battleCharacter.AdvanceCooldown(globalCooldown);
        }
        BattleCharacter nextInTurn = mBattleCharacter[0];
        nextInTurn.TakeTurn();
        mBattleCharacter.Remove(mBattleCharacter[0]);
        mBattleCharacter.Add(nextInTurn);

    }

    private void UpdateTurnOrder()
    {
        Debug.Log("Started next Turn");
        mBattleCharacter = mBattleCharacter.OrderBy((battleCharacter) => { return battleCharacter.CooldownTimeRemaining; }).ToList();
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
            partyBattleCharacter.OnTurnFinished += ProcessFirstRound;
            mBattleCharacter.Add(partyBattleCharacter);
            i++;
        }
    }
}
