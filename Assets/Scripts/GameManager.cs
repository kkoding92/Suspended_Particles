using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MaskState { basic=0, KF80, KF94, KF99 }
public enum PalmState { None=0, Level1, Level2, Level3 }
public enum StuckyiState { None=0, Level1, Level2 }

public class GameManager : MonoBehaviour
{
    //보유한 코인 수
    private int coin;
    //보유한 다이아몬드 수
    private int diamond;
    //체내 미세먼지 수
    private float fineDustLevel;
    //이산화탄소 수
    private float co2Level;
    //최대 체내 미세먼지 
    private int maxFineDustLevel;
    //밖에서 머물 수 있는 시간
    private int stayTime = 60;
    //특정 미세먼지 증가시간 
    [SerializeField] private float fineDustCheckTime = 10;
    //특정 코인 증가시간 
    [SerializeField] private float coinCheckTime = 15;

    //야자수 상태
    [HideInInspector] public PalmState palmState = PalmState.None;
    //야자수 자라는 조건
    [HideInInspector] public int palmGrowCount=0;
    //스투키 상태
    [HideInInspector] public StuckyiState stuckyiState = StuckyiState.None;
    //스투키 자라는 조건
    [HideInInspector] public int stuckyiGrowCount=0;
    //차량2부제 스킬 갯수
    [HideInInspector] public int carSkillCount;
    //바람 스킬 갯수
    [HideInInspector] public int windSkillCount;
    //비 스킬 갯수
    [HideInInspector] public int rainSkillCount;
    //클렌징 보유 여부
    [HideInInspector] public bool checkCleansing = false;
    //공기청정기 유무
    [HideInInspector] public bool checkAirCleaner = false;
    //야자수 보유 유무
    [HideInInspector] public bool checkPalmTree = false;
    //Stuckyi 보유 유무
    [HideInInspector] public bool checkStuckyi = false;
    //마스크 보유 현환
    [HideInInspector] public MaskState maskState = MaskState.basic;
    //방충망 필터 보유 여부
    private bool checkFilter = false;
    //방안 내 산소 유무
    private bool checkOxygen = true;
    //창문이 열렸는지 여부
    private bool isOpen = true;
    //잠들어 있는지 여부
    private bool isSleeping = false;

    public int Coin { get => coin; set => coin = value; }

    void Start()
    {
        //이벤트 등록
        TVController.WatchReward += WatchReward;
        BedController.SleepReward += SleepReward;
        BedController.Sleep += Sleep;
        WaterController.DrinkReward += DrinkReward;
        WashController.WashReward += WashReward;
        WindowController.VentialationReward += VentialationReward;
        StoreController.StoreReward += StoreReward;
        SkillController.SkillReward += SkillReward;
        PlantController.GrowReward += GrowReward;

        StartCoroutine(IncreseFineDustLevel());
        StartCoroutine(IncreseCoin());
    }

    void Update()
    {
        
    }

    private void SetMask(MaskState newMaskState)
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

    public void VentialationReward()
    {
        Debug.Log("Ventialation Reward!");
        co2Level = 0;
        checkOxygen = true;
        fineDustLevel += 20;
        Coin += 100;
        StartCoroutine(IncreaseCo2());
    }

    public void WashReward()
    {
        Debug.Log("Washed Reward!");
        if(checkCleansing)
        {
            Coin += 120;
            fineDustLevel -= 30;
        }
        else
        {
            Coin += 100;
            fineDustLevel -= 10;
        }

        if (fineDustLevel < 0)
            fineDustLevel = 0;
    }

    public void DrinkReward()
    {
        Debug.Log("Drinked Reward!");
        Coin += 100;
    }

    public void WatchReward()
    {
        Debug.Log("Watched Reward!");
        Coin += 100;
    }

    public void Sleep()
    {
        isSleeping = true;
    }

    public void SleepReward()
    {
        Debug.Log("Slept Reward!");
        isSleeping = false;
        Coin += 500;
    }

    public void StoreReward(Item item, int price)
    {
        switch (item)
        {
            case Item.Cleansing:
                checkCleansing = true;
                break;
            case Item.Stuckyi:
                checkStuckyi = true;
                break;
            case Item.PalmTree:
                checkPalmTree = true;
                break;
            case Item.AirCleaner:
                checkAirCleaner = true;
                break;
            case Item.KF80:
                SetMask(MaskState.KF80);
                break;
            case Item.KF94:
                SetMask(MaskState.KF94);
                break;
            case Item.KF99:
                SetMask(MaskState.KF99);
                break;
        }
        Coin -= price;
    }

    public void SkillReward(Skill_Item item, int price)
    {
        switch (item)
        {
            case Skill_Item.car:
                carSkillCount++;
                break;
            case Skill_Item.rain:
                rainSkillCount++;
                break;
            case Skill_Item.wind:
                windSkillCount++;
                break;
        }
        coin -= price;
    }

    public void GrowReward(Plant plant)
    {
        if(plant == Plant.PalmTree)
        {
            if(palmState == PalmState.Level1)
            {
                coin += 50;
                palmGrowCount = 0;
                palmState = PalmState.Level2;
            }
            else if(palmState == PalmState.Level2)
            {
                coin += 50;
                palmGrowCount = 0;
                palmState = PalmState.Level3;
            }
        }
        else if(plant == Plant.Stuckyi)
        {
            if (stuckyiState == StuckyiState.Level1)
            {
                coin += 50;
                stuckyiGrowCount = 0;
                stuckyiState = StuckyiState.Level2;
            }
        }
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
                Coin += 5;
            else
                Coin++;

            Debug.Log("coin : " + Coin + " FineDustLevel : " + fineDustLevel);
            yield return new WaitForSeconds(coinCheckTime);
        }
    }

    IEnumerator IncreaseCo2()
    {
        while (true)
        {
            co2Level += Time.deltaTime;
            if(co2Level>100)
            {
                Debug.Log("co2 Max");
                checkOxygen = false;
                break;
            }
            yield return null;
        }
    }
}
