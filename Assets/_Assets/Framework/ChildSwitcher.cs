using System;
using System.Collections.Generic;
using UnityEngine;

public class ChildSwitcher : MonoBehaviour
{
   List<GameObject> mChildGameObjects = new List<GameObject>();
    private int mCurrentActiveChildIndex;

    private void Awake()
    {
        foreach (RectTransform childTransform in transform)
        {
            mChildGameObjects.Add(childTransform.gameObject);
        }

        SetActiveChildByIndex(mCurrentActiveChildIndex);
    }

    public void SetActiveChild(GameObject childToSwitchTo)
    {
        int childIndex = mChildGameObjects.FindIndex((x) => { return x.gameObject == x; });
        SetActiveChildByIndex(childIndex);
    }
    private void SetActiveChildByIndex(int newActiveChildIndex)
    {
        if (newActiveChildIndex < 0 || newActiveChildIndex >= mChildGameObjects.Count)
        {
            return;
        }

        foreach (GameObject childGameObject in mChildGameObjects)
        {
            childGameObject.SetActive(false);
        }

        mCurrentActiveChildIndex = newActiveChildIndex;
        mChildGameObjects[mCurrentActiveChildIndex].SetActive(true);
    }

    public List<GameObject> GiveChildren()
    {
        return mChildGameObjects;
    }

    public int GiveChildIndex()
    {
        return mCurrentActiveChildIndex;
    }
}
