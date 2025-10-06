using UnityEngine;

[RequireComponent(typeof(MovementController))]
public class Player : MonoBehaviour
{

    [SerializeField] CameraRig mCameraRigPrefab;

    private PlayerInputActions mPlayerInputActions; //Delete this from MovementController
    private MovementController mMovementController;
    CameraRig mCameraRig;


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
}