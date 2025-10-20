using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameplayWidget : MonoBehaviour
{
    [SerializeField] Image mTransitionImage;
    [SerializeField] ChildSwitcher mChildSwitcher;
    [SerializeField] BattleWidget mBattleWidget;
    [SerializeField] GameObject mRoamingWidget;

    private void Awake()
    {
        mTransitionImage.gameObject.SetActive(false);
        mChildSwitcher = GetComponentInChildren<ChildSwitcher>();
    }

    public void DipToBlack(float dipInAndOutDuration, float dipStayDuration, Action dippedToBlackCallback)
    {
        StartCoroutine(StartDipToBlack(dipInAndOutDuration, dipStayDuration, dippedToBlackCallback));
    }

    public void SetFocusCharacterInBattle(BattleCharacter battleCharacter)
    {
        
    }


    IEnumerator StartDipToBlack(float dipInAndOutDuration, float dipStayDuration, Action dippedToBlackCallback)
    {
        float timeCounter = 0;
        mTransitionImage.gameObject.SetActive(true);
        Color transitionImageColor = Color.black;
        transitionImageColor.a = 0f;
        while(timeCounter < dipInAndOutDuration)
        {
            transitionImageColor.a = timeCounter / dipInAndOutDuration;
            mTransitionImage.color = transitionImageColor;
            timeCounter += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        transitionImageColor.a = 1;
        mTransitionImage.color = transitionImageColor;
        dippedToBlackCallback();

        //Wait for dipStayDuration
        //Dip out from black

        yield return new WaitForSeconds(dipStayDuration);

        timeCounter = 0;
        while (transitionImageColor.a > 0)
        {
            transitionImageColor.a -= Time.deltaTime;
            mTransitionImage.color = transitionImageColor;
            yield return new WaitForEndOfFrame();
        }

        mTransitionImage.gameObject.SetActive(false);


    }
    internal void SwitchToBattle()
    {
        mChildSwitcher.SetActiveChild(mBattleWidget.gameObject);
    }

    internal void SwitchToRoaming()
    {
        mChildSwitcher.SetActiveChild(mRoamingWidget);
    }
}
