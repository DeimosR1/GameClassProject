using System;
using TreeEditor;
using Unity.Behavior;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private float mSightDistance;
    [SerializeField] private float mViewAngle;
    [SerializeField] private float mEyeHeight;
    [SerializeField] private float mAlwaysAwareDistance = 1.5f;
 
    GameObject mTarget;
    GameObject Target
    {
        get { return mTarget; }
        set 
        {
            if(Target == value) return;

            if (value == null)
            {
                mBehaviorGraphAgent.BlackboardReference.SetVariableValue("HasLastSeenPosition", true);
                mBehaviorGraphAgent.BlackboardReference.SetVariableValue("TargetLastSeenPosition", mTarget.transform.position);
            }
            mTarget = value;
            mBehaviorGraphAgent.BlackboardReference.SetVariableValue("Target", Target);
        }
    }

    BehaviorGraphAgent mBehaviorGraphAgent;

    private void Awake()
    {
        mBehaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
    }
    // Update is called once per frame
    void Update()
    {
      UpdatePlayerPerception();  
    }

    private void UpdatePlayerPerception()
    {
        Player player = GameMode.MainGameMode.mPlayer;
        if (!player)
        {
            Target = null;
            return;
        }

        float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);

        if (distanceToPlayer <= mAlwaysAwareDistance)
        {
            Target = player.gameObject;
            return;
        }

        if(distanceToPlayer > mSightDistance)
        {
            Target = null;
            return;
        }

        Vector3 playerDir = (player.transform.position - transform.position).normalized;
        
        if(Vector3.Angle(playerDir, transform.forward) > mViewAngle)
        {
            Target = null;
            return;
        }

        Vector3 eyeViewPoint = transform.position + Vector3.up * mEyeHeight;
        if(Physics.Raycast(eyeViewPoint, playerDir, out RaycastHit hitInfo, mSightDistance))
        {
            if(hitInfo.collider.gameObject != player)
            {
                Target = null;
                return;
            }
        }

        Target = player.gameObject;
    }

    private void OnDrawGizmos()
    {
        Vector3 eyeViewPoint = transform.position + Vector3.up * mEyeHeight;
        Gizmos.DrawWireSphere(eyeViewPoint, mSightDistance);
        Gizmos.DrawWireSphere(eyeViewPoint, mAlwaysAwareDistance);

        Vector3 leftLineDir = Quaternion.AngleAxis(mViewAngle, Vector3.up) * transform.forward;
        Vector3 rightLineDir = Quaternion.AngleAxis(-mViewAngle, Vector3.up) * transform.forward;

        Gizmos.DrawLine(eyeViewPoint, eyeViewPoint + leftLineDir * mSightDistance);
        Gizmos.DrawLine(eyeViewPoint, eyeViewPoint + rightLineDir * mSightDistance);

        if (Target)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position, Target.transform.position);
            Gizmos.DrawWireSphere(Target.transform.position, 0.5f);
        }
    }
}
