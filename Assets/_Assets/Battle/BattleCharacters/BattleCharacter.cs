using System;
using Unity.VisualScripting;
using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [SerializeField] GameObject mTurnIndicator;

    public float CooldownDuration => 1f / Speed;

    public float CooldownTimeRemaining {  get; private set; }

    public event Action<BattleCharacter> onTurnStarted;

    public event Action OnTurnFinished;

    private void Awake()
    {
        CooldownTimeRemaining = CooldownDuration;
        mTurnIndicator.SetActive(false);
        GameMode.MainGameMode.BattleManager.IsInBattle = true;
    }

    public void TakeTurn()
    {
        Invoke("FinishTurn", 1);
        Debug.Log($"{this.gameObject} took its turn");
        mTurnIndicator.SetActive(true);
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
