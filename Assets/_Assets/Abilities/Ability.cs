using System;
using UnityEngine;

public abstract class Ability : ScriptableObject
{
    [field: SerializeField] public string mAbilityName { get; private set; }
    AbilityComponent mOwningAbilityComponent;
    internal void Init(AbilityComponent newAbility)
    {
        mOwningAbilityComponent = newAbility;
    }

    public virtual void ActivateAbility()
    {
        Debug.Log($"Activating ability");
    }
}
