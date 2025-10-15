using UnityEngine;

public class BattleCharacter : MonoBehaviour
{
    [field: SerializeField] public float Speed { get; private set; } = 1;
    [SerializeField] GameObject mTurnIndicator;

    public float CooldownDuration => 1f / Speed;

    public float CooldownTimeRemaining {  get; private set; }

    private void Awake()
    {
        CooldownTimeRemaining = CooldownDuration;
        mTurnIndicator.SetActive(false);
    }

    public void TakeTurn()
    {
        Invoke("FinishTurn", 1);
        mTurnIndicator.SetActive(true);
    }

    public void FinishTurn()
    {
        mTurnIndicator.SetActive(false);
    }
}
