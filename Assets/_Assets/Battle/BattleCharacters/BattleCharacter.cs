using System;
using Unity.VisualScripting;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [field: SerializeField] public string Name { get; private set; } = "BattleCharacter";
    [SerializeField] GameObject mTurnIndicator;

    public float CooldownDuration => 1f / Speed;

    public float CooldownTimeRemaining {  get; private set; }

    public event Action<BattleCharacter> onTurnStarted;

    public event Action OnTurnFinished;

    AbilityComponent mAbilityComponent;

    public int PartyID { get; private set; } 

    public void Init(int partyID, IViewClient viewClient)
    {
        PartyID = partyID;
        if (mAbilityComponent == null)
        {
            mAbilityComponent = GetComponent<AbilityComponent>();
            mAbilityComponent.SetViewClient(viewClient);
        }
    }

    public AbilityComponent GetAbilityComponent()
    {
        return mAbilityComponent;
    }
    private void Awake()
    {
        CooldownTimeRemaining = CooldownDuration;
        mTurnIndicator.SetActive(false);
        GameMode.MainGameMode.BattleManager.IsInBattle = true;

        mAbilityComponent = GetComponent<AbilityComponent>();
        
    }

    public void SetHighlighted(bool highlighted)
    {
        mTurnIndicator.SetActive(highlighted);
    }

    public void TakeTurn()
    {
        //Invoke("FinishTurn", 1);
        SetHighlighted(true);
        onTurnStarted?.Invoke(this);
    }

    public void FinishTurn()
    {
        mTurnIndicator.SetActive(false);
        CooldownTimeRemaining = CooldownDuration;
        OnTurnFinished?.Invoke();
    }

    public void AdvanceCooldown(float amount)
    {
        CooldownTimeRemaining -= amount;
    }
}
