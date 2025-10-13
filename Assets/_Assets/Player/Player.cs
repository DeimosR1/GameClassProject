using System;
using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour
{

    [SerializeField] CameraRig mCameraRigPrefab;

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
        }
    }

}