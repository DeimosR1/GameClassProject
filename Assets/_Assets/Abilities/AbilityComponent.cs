using System.Collections;
using System;
using UnityEngine;
using System.Collections.Generic;

public class AbilityComponent : MonoBehaviour
{
    [SerializeField] Ability[] mInitialAbilities;

    List<Ability> mAbilities = new List<Ability>();
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

    internal IEnumerable<Ability> GetAbilities()
    {
        return mAbilities;
    }
}
