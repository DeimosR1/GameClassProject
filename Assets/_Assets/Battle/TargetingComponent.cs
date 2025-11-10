using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingComponent : MonoBehaviour
{
    //Check Class Recording
    BattleInputActions mBattleInputActions;

    Vector2 mNavigationInput;

    ITargetService mTargetService;

    [SerializeField] List<BattleCharacter> mTargets = new List<BattleCharacter>();
    private bool mNavigationReset;

    int mCurrentSelectedTargetIndex = -1;

    public event Action<BattleCharacter> onTargetPicked;
    public event Action onTargetPick;

    public void SetTargetService(ITargetService targetService)
    {
        mTargetService = targetService;
    }


    public void StartTargetting(int PartyID, bool hostile)
    {
        mBattleInputActions.Enable();
        mTargets.Clear();
        mTargets = mTargetService.GetTargetsForTeam(PartyID, hostile);
        SetCurrentlySelectedTargetIndex(0);
    }

    private void Awake()
    {
        mBattleInputActions = new BattleInputActions();
        mBattleInputActions.Battle.Navigation.performed += HandleTargetNavigation;
        mBattleInputActions.Battle.Navigation.canceled += HandleTargetNavigation;
    }

    private void OnEnable()
    {
        mBattleInputActions.Enable();
    }
    private void OnDisable()
    {
        mBattleInputActions.Disable();
    }
    private void HandleTargetNavigation(InputAction.CallbackContext context)
    {
        mNavigationInput = context.ReadValue<Vector2>();
    }

    private void Update()
    {
        if(mNavigationInput.sqrMagnitude > 0.5 && mNavigationReset)
        {
            mNavigationReset = false;
            Debug.Log($"Navigating with input X: {mNavigationInput.x}");
            NavigateToNextTarget(mNavigationInput.x > 0 ? true : false);
        }

        if(mNavigationInput.sqrMagnitude < 0.25)
        {
            mNavigationReset = true;
        }
    }

    void NavigateToNextTarget(bool increment)
    {
        int newIndex = mCurrentSelectedTargetIndex + (increment ? 1 : -1);
        if (newIndex < 0)
        {
            newIndex = mTargets.Count - 1;
        }
        if (newIndex >= mTargets.Count)
        {
            newIndex = 0;
        }
        SetCurrentlySelectedTargetIndex(newIndex);
    }

    void SetCurrentlySelectedTargetIndex(int newIndex)
    {
        if(newIndex < 0 || newIndex >= mTargets.Count) { return; }

        if (mCurrentSelectedTargetIndex >= 0 && mCurrentSelectedTargetIndex < mTargets.Count)
        {
            mTargets[mCurrentSelectedTargetIndex].SetHighlighted(false);
        }

        mCurrentSelectedTargetIndex = newIndex;
        mTargets[mCurrentSelectedTargetIndex].SetHighlighted(true);
    }
}
