using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class TargetingComponent : MonoBehaviour
{
    BattleInputActions mBattleInputActions;

    Vector2 mNavigationInput;

    ITargetService mTargetService;

    List<BattleCharacter> mTargets = new List<BattleCharacter>();
    private bool mNavigationReset;

    public void SetTargetService(ITargetService targetService)
    {
        mTargetService = targetService;
    }


    public void StartTargetting(int PartyID, bool hostile)
    {
        mBattleInputActions.Enable();
        mTargets.Clear();
        mTargets = mTargetService.GetTargetsForTeam(PartyID, hostile);
        mTargets[0].SetHighlighted(true);
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
        }

        if(mNavigationInput.sqrMagnitude < 0.25)
        {
            mNavigationReset = true;
        }
    }
}
