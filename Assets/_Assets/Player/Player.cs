using System;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour, IViewClient
{

    [SerializeField] CameraRig mCameraRigPrefab;
    [SerializeField] GameplayWidget mGameplayWidgetPrefab;

    GameplayWidget mGameplayWidget;

    private PlayerInputActions mPlayerInputActions; //Delete this from MovementController
    private MovementController mMovementController;
    CameraRig mCameraRig;

    private BattleState mBattleState;
    
    [SerializeField] private BattlePartyComponent mBattlePartyComponent;

    void Awake()
    {
        //Add All the player Input Actions here from the controller.
        mCameraRig = Instantiate(mCameraRigPrefab);
        mCameraRig.SetFollowTransform(transform);

        mMovementController = GetComponent<MovementController>();
        mPlayerInputActions = new PlayerInputActions();
        mPlayerInputActions.Gameplay.Jump.performed += mMovementController.PerformJump;
        mPlayerInputActions.Gameplay.Move.performed += mMovementController.HandleMoveInput;
        mPlayerInputActions.Gameplay.Move.canceled += mMovementController.HandleMoveInput;

        mPlayerInputActions.Gameplay.Look.performed += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());
        mPlayerInputActions.Gameplay.Look.canceled += (context) => mCameraRig.SetLookInput(context.ReadValue<Vector2>());

        mBattlePartyComponent = GetComponent<BattlePartyComponent>();
        mGameplayWidget = Instantiate(mGameplayWidgetPrefab);
    }

    private bool IsInBattle()
    {
        return mBattleState == BattleState.InBattle;
    }
    //Add OnEnable and Disable here!
    private void OnEnable()
    {
        mPlayerInputActions.Enable();
    }

    private void OnDisable()
    {
        mPlayerInputActions.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == gameObject)
        {
            return;
        }

        BattlePartyComponent otherBattlePartyComponent = other.GetComponent<BattlePartyComponent>();
        if(otherBattlePartyComponent && !IsInBattle())
        {
            GameMode.MainGameMode.BattleManager.StartBattle(mBattlePartyComponent, otherBattlePartyComponent);
            SwitchToBattleState(BattleState.InBattle);
        }
    }

    private void SwitchToBattleState(BattleState battleState)
    {
        switch (battleState)
        {
            case BattleState.InBattle:
                mPlayerInputActions.Disable();
                break;
            case BattleState.Roaming: 
                mPlayerInputActions.Enable(); 
                break;
            default:
                break;
        }

        mGameplayWidget.DipToBlack(1, 1, DippedToBlack);
    }

    void DippedToBlack()
    {
        Debug.Log($"Dipped To Black Called");
        mBattlePartyComponent.UpdateView();
    }


    public void SetViewTarget(Transform viewTarget)
    {
        mCameraRig.SetFollowTransform(viewTarget);
        mCameraRig.transform.rotation = viewTarget.transform.rotation;
    }

    public void ResetViewAngle()
    {
        mCameraRig.ResetViewAngle();
    }
}