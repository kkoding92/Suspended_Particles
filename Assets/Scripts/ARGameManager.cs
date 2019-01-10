using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGameManager : MonoBehaviour
{
    public GameManager gm;

    //머물수 있는 시간
    private float timer;
    //미세먼지 총 수
    private int numberOfFine;
    //보상
    private int rewardCoin;
    private int rewardDia;

    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void GameInit()
    {
        //방안 미세먼지 증가를 막기위해 게임중인지 체크
        gm.isGamming = true;
        //timer 설정
        timer = gm.StayTime;

        //미세먼지 갯수 설정
        if(gm.gameLevel == GameLevel.Default || gm.gameLevel == GameLevel.Easy)
            numberOfFine = 100;
        else if (gm.gameLevel == GameLevel.Normal)
            numberOfFine = 120;
        else if (gm.gameLevel == GameLevel.Hard)
            numberOfFine = 150;

        //스킬 활성화
        //
    }

    private void GameReward()
    {

    }
}
