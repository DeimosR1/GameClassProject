using UnityEngine;

public class BattleSite : MonoBehaviour
{
    [SerializeField] float mSiteRadius;
    [SerializeField, Range(0,5)] int mSiteCapacity;

    [SerializeField] bool mIsPlayerSite = false;

    public bool IsPlayerSite => mIsPlayerSite;
    //zero indexed, first unity should have an index of 0
    public Vector3 GetPositionForUnit(int index)
    {
        if (mSiteCapacity <= 1)
        {
            return transform.position;
        }

        float gap = (mSiteRadius * 2)/(mSiteCapacity - 1);
        Vector3 startingPoint = transform.position - transform.right * mSiteRadius;

        return startingPoint + index * gap * transform.right;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = IsPlayerSite ? Color.green : Color.red;
        Gizmos.DrawWireSphere(transform.position, mSiteRadius);
        for(int i = 0; i < mSiteCapacity; i++)
        {
            Gizmos.DrawSphere(GetPositionForUnit(i), 0.5f);
        }
    }
}
