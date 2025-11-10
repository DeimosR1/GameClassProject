using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] mInitialAbilities;

    List<Ability> mAbilities = new List<Ability>();

    IViewClient mOwnerViewClient;

    [SerializeField] Transform mTargettingFollowTransform;

    public int GetPartyID()
    {
        return GetComponent<BattleCharacter>().PartyID;
    }
    private void Start()
    {
        foreach(Ability initialAbility in mInitialAbilities)
        {
            GiveAbility(initialAbility);
        }
    }

    private void GiveAbility(Ability abilityDefaultObject)
    {
        Ability newAbility = Instantiate(abilityDefaultObject);
        newAbility.Init(this);
        mAbilities.Add(newAbility);
    }

    public void StartTargeting(bool hostile)
    {
        if(mOwnerViewClient is not null)
        {
            mOwnerViewClient.PushViewTarget(mTargettingFollowTransform);
        }
        GameMode.MainGameMode.BattleManager.GetTargetingComponent().StartTargetting(GetPartyID(), hostile);
    }

    internal IEnumerable<Ability> GetAbilities()
    {
        return mAbilities;
    }
    internal void SetViewClient(IViewClient viewClient)
    {
        mOwnerViewClient = viewClient;
    }
}
