using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(TargetingComponent))]
public class BattleManager : MonoBehaviour, ITargetService
{

    List<BattleSite> mBattleSites;

    [SerializeField] List<BattleCharacter> mBattleCharacters = new List<BattleCharacter>();

    Queue<BattleCharacter> mFirstRoundBattleCharacters = new Queue<BattleCharacter>();

    //int mRoundNumber = 1;
    //int mFirstTurnNextIndex;

    public bool IsInBattle;

    IViewClient mOwnerViewClient;
    TargetingComponent mTargetingComponent;

    private void Awake()
    {
        mTargetingComponent = GetComponent<TargetingComponent>();
        mTargetingComponent.SetTargetService(this);
    }
    public void StartBattle(BattlePartyComponent playerParty, BattlePartyComponent enemyParty)
    {
        mOwnerViewClient = GameObject.FindGameObjectWithTag("Player").GetComponent<IViewClient>();
        mBattleCharacters.Clear();
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
        mFirstRoundBattleCharacters = new Queue<BattleCharacter>(mBattleCharacters);
        ProcessFirstRound();
        Debug.Log("Started Turnes");
    }

    private void ProcessFirstRound()
    {
        if(mFirstRoundBattleCharacters.TryDequeue(out BattleCharacter nextBattleCharacter))
        {
            if (mBattleCharacters.Contains(nextBattleCharacter))
            {
                nextBattleCharacter.TakeTurn();
                return;
            }
            else
            {
                ProcessFirstRound();
            }
            nextBattleCharacter.TakeTurn();
            return;
        }

        foreach(BattleCharacter battleCharacter in mBattleCharacters)
        {
            battleCharacter.OnTurnFinished -= ProcessFirstRound;
            battleCharacter.OnTurnFinished += NextTurn;
        }

        NextTurn();
    }

    void NextTurn()
    {
        UpdateTurnOrder();
        float globalCooldown = mBattleCharacters[0].CooldownDuration;

        foreach (BattleCharacter battleCharacter in mBattleCharacters)
        {
            battleCharacter.AdvanceCooldown(globalCooldown);
        }
        BattleCharacter nextInTurn = mBattleCharacters[0];
        nextInTurn.TakeTurn();
        mBattleCharacters.Remove(mBattleCharacters[0]);
        mBattleCharacters.Add(nextInTurn);

    }

    private void UpdateTurnOrder()
    {
        Debug.Log("Started next Turn");
        mBattleCharacters = mBattleCharacters.OrderBy((battleCharacter) => { return battleCharacter.CooldownTimeRemaining; }).ThenBy((battleCharacter) => {return 1/battleCharacter.Speed; }).ToList();
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
            mBattleCharacters.Add(partyBattleCharacter);
            i++;
        }
    }

    public List<BattleCharacter> GetTargetsForTeam(int teamId, bool hostileTargets)
    {
        List<BattleCharacter> targets = new List<BattleCharacter>();
        foreach(BattleCharacter battleCharacter in mBattleCharacters)
        {
            if(battleCharacter.PartyID == teamId && !hostileTargets)
            {
                targets.Add(battleCharacter);
            }
            if(battleCharacter.PartyID != teamId && hostileTargets)
            {
                targets.Add(battleCharacter);
            }
        }
        return targets;
    }

    public TargetingComponent GetTargetingComponent()
    {
        return mTargetingComponent;
    }
}
