using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class MovementController : MonoBehaviour
{
    [SerializeField] float mJumpSpeed = 5f;
    [SerializeField] float mMaxFallSpeed = 50f;
    [SerializeField] float mMaxMoveSpeed = 5f;
    [SerializeField] float mGroundMoveSpeedAcceleration = 40f;
    [SerializeField] float TurnLerpRate;
    [SerializeField] float mAirMoveSpeedAcceleration = 5f;

    [SerializeField] bool mWasGrounded;
    [SerializeField] private bool mShouldTryJump;

    [SerializeField] float mAirCheckRadius = 0.5f;
    [SerializeField] LayerMask mAirCheckLayerMask = 1;

    [SerializeField] private bool mIsInAir;

    private CharacterController mCharacterController;
    [SerializeField] private Vector3 mVerticalVelocity;
    private Vector3 mHorizontalVelocity;
    private Vector2 mMoveInput;

    [SerializeField] Animator mAnimator;

    private void Awake()
    {
        mCharacterController = GetComponent<CharacterController>();
        mAnimator = GetComponent<Animator>();
    }

    public void HandleMoveInput(InputAction.CallbackContext context)
    {
        mMoveInput = context.ReadValue<Vector2>();
        //Debug.Log($"Move Input is: {mMoveInput}");
    }
    
    public void PerformJump(InputAction.CallbackContext context)
    {
        if (!mIsInAir)
        {
            mShouldTryJump = true;
        }

    }

    bool IsInAir()
    {
        if (mCharacterController.isGrounded)
        {
            return false;
        }

        Collider[] airCheckColliders = Physics.OverlapSphere(transform.position, mAirCheckRadius, mAirCheckLayerMask);

        foreach (Collider collider in airCheckColliders)
        {
            if(collider.gameObject != gameObject)
            {
                return false;
            }
        }
        return true;
    }

    private void Update()
    {
        mIsInAir = IsInAir();
        UpdateVerticalVelocity();
        UpdateHorizontalVelocity();
        UpdateTransform();
        UpdateAnim();
        //Debug.Log($"Is Grounded: { mCharacterController.isGrounded}");
    }

    void UpdateHorizontalVelocity()
    {
        Vector3 moveDir = PlayerInputToWorldDir(mMoveInput);

        float acceleration = mCharacterController.isGrounded ? mGroundMoveSpeedAcceleration : mAirMoveSpeedAcceleration;

        if(moveDir.sqrMagnitude > 0)
        {
            mHorizontalVelocity += (moveDir * acceleration * Time.deltaTime);
            mHorizontalVelocity = Vector3.ClampMagnitude(mHorizontalVelocity, mMaxMoveSpeed);
        }
        else
        {
            if(mHorizontalVelocity.sqrMagnitude > 0)
            {
                mHorizontalVelocity -= mHorizontalVelocity.normalized * acceleration * Time.deltaTime;
                if(mHorizontalVelocity.sqrMagnitude < 0.1f)
                {
                    mHorizontalVelocity = Vector3.zero;
                }
            }
        }
    }

    void UpdateVerticalVelocity()
    {
        if (mShouldTryJump && !mIsInAir)
        {
            mAnimator.SetTrigger("Jump");
            mVerticalVelocity.y = mJumpSpeed;
            mShouldTryJump = false;
            return;
        }

        if (mCharacterController.isGrounded)
        {
            mAnimator.ResetTrigger("Jump");
            mVerticalVelocity.y = -1f;
            return;
        }

        //Free Falling
        if (mVerticalVelocity.y > -mMaxFallSpeed)
        {
            mVerticalVelocity.y += Physics.gravity.y * Time.deltaTime;
        }
    }

    void UpdateTransform()
    {
        mCharacterController.Move((mVerticalVelocity + mHorizontalVelocity) * Time.deltaTime);

        if (mHorizontalVelocity.sqrMagnitude > 0)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(mHorizontalVelocity.normalized, Vector3.up), Time.deltaTime * TurnLerpRate);
        }

        //if (mWasGrounded && !mCharacterController.isGrounded)
        //{
        //    mVerticalVelocity.y = 0f;
        //}

        //mWasGrounded = mCharacterController.isGrounded;
    }

    void UpdateAnim()
    {
        mAnimator.SetFloat("Speed", mHorizontalVelocity.magnitude);
        mAnimator.SetBool("Landed", !mIsInAir);
    }
    Vector3 PlayerInputToWorldDir(Vector2 inputVal)
    {
        Vector3 rightDir = Camera.main.transform.right;
        Vector3 fwdDir = Vector3.Cross(rightDir, Vector3.up);

        return rightDir * inputVal.x + fwdDir * inputVal.y;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = mIsInAir ? Color.red : Color.green;
        Gizmos.DrawSphere(transform.position, mAirCheckRadius);
    }
}
