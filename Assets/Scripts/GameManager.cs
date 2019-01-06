using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaskState { basic=0, KF80, KF94, KF99 }

public class GameManager : MonoBehaviour
{
    //보유한 코인 수
    private ulong coin = 0;
    //보유한 다이아몬드 수
    private int diamond;
    //체내 미세먼지 수
    private float fineDustLevel;
    //최대 체내 미세먼지 
    private int maxFineDustLevel;
    //밖에서 머물 수 있는 시간
    private int stayTime = 60;
    //마스크 보유 현환
    private MaskState maskState = MaskState.basic;
    //특정 미세먼지 증가시간 
    [SerializeField] private float fineDustCheckTime = 10;
    //특정 코인 증가시간 
    [SerializeField] private float coinCheckTime = 15;

    //클렌징 보유 여부
    private bool checkCleansing = false;
    //방충망 필터 보유 여부
    private bool checkFilter = false;
    //방안 내 산소 유무
    private bool checkOxygen = true;
    //공기청정기 유무
    private bool checkAirCleaner = false;
    //창문이 열렸는지 여부
    private bool isOpen = true;
    //잠들어 있는지 여부
    private bool isSleeping = false;

    void Start()
    {
        //이벤트 등록
        TVController.WatchReward += WatchReward;
        BedController.SleepReward += SleepReward;
        BedController.Sleep += Sleep;

        StartCoroutine(IncreseFineDustLevel());
        StartCoroutine(IncreseCoin());
    }

    void Update()
    {
        
    }

    public void Ventialation()
    {
    }

    public void Wash()
    {
    }

    public void CheckMask(MaskState newMaskState)
    {
        maskState = newMaskState;

        switch (newMaskState)
        {
            case MaskState.basic:
                stayTime = 60;
                break;
            case MaskState.KF80:
                stayTime = 70;
                break;
            case MaskState.KF94:
                stayTime = 80;
                break;
            case MaskState.KF99:
                stayTime = 90;
                break;
        }
    }

    public void Drink()
    {
    }

    public void WatchReward()
    {
        Debug.Log("Watching Reward!");
        coin += 100;
    }

    public void Sleep()
    {
        isSleeping = true;
    }

    public void SleepReward()
    {
        isSleeping = false;
        coin += 500;
    }

    //미세먼지 증가루틴 
    IEnumerator IncreseFineDustLevel()
    {
        while (true) {
            if (!isSleeping)
            {
                if (isOpen)
                {
                    if (checkFilter)
                        fineDustLevel += 2;
                    else
                        fineDustLevel += 5;
                }
                else
                    fineDustLevel++;

                if (checkOxygen)
                    yield return new WaitForSeconds(fineDustCheckTime);
                else
                    yield return new WaitForSeconds(fineDustCheckTime * 0.5f);
            }
            yield return null;
        }
    }

    IEnumerator IncreseCoin()
    {
        while (true)
        {
            if (checkAirCleaner)
                coin += 5;
            else
                coin++;

            Debug.Log("coin : " + coin + " FineDustLevel : " + fineDustLevel);
            yield return new WaitForSeconds(coinCheckTime);
        }
    }
}
