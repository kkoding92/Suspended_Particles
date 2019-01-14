using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ARGameManager : MonoBehaviour
{
    public GameManager gm;
    public GameObject[] objectsToSpawn;
    public Transform[] Oring;
    public Button[] skillBtn;
    public GameObject[] skillEffect;
    public Text timerTxt;
    public GameObject ARCanvas;

    private int num_enemies = 20;

    //머물수 있는 시간
    private float timer;

    //미세먼지 총 수
    private int numberOfFine;

    //보상
    private int rewardCoin;

    private GameObject[] spawnObjs;

    private int rewardDia;

    private Transform tempGO;

    private void Start()
    {
        ARCanvas.SetActive(true);
        gm.carSkillCount = 1;
        gm.treeSkillCount = 1;
        spawnObjs = new GameObject[num_enemies];
        skillEffect[0].SetActive(false);
        skillEffect[1].SetActive(false);

        GameInit();
    }

    private void Update()
    {
    }

    private void GameInit()
    {
        //방안 미세먼지 증가를 막기위해 게임중인지 체크
        gm.isGamming = true;
        //timer 설정
        timer = gm.StayTime;

        //미세먼지 갯수 설정
        if (gm.gameLevel == GameLevel.Default || gm.gameLevel == GameLevel.Easy)
            numberOfFine = 100;
        else if (gm.gameLevel == GameLevel.Normal)
            numberOfFine = 120;
        else if (gm.gameLevel == GameLevel.Hard)
            numberOfFine = 150;

        //스킬 활성화
        if (gm.carSkillCount == 1)
            skillBtn[0].interactable = true;
        else
            skillBtn[0].interactable = false;

        if (gm.treeSkillCount == 1)
            skillBtn[1].interactable = true;
        else
            skillBtn[1].interactable = false;

        //미세먼지 생성
        CreateFineDust();
    }

    private void CreateFineDust()
    {
        Shuffle();

        for (int i = 0; i < num_enemies; i++)
        {
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            GameObject obj = Instantiate(objectToSpawn, Oring[i].position, Oring[i].rotation);
            spawnObjs[i] = obj;
        }
    }

    public void Shuffle()
    {
        for (int i = 0; i < Oring.Length; i++)
        {
            int rnd = Random.Range(0, Oring.Length);
            tempGO = Oring[rnd];
            Oring[rnd] = Oring[i];
            Oring[i] = tempGO;
        }
    }

    public void UseCarSkill()
    {
        skillEffect[0].SetActive(true);
        skillBtn[0].interactable = false;
    }

    public void UseTreeSkill()
    {
        skillEffect[1].SetActive(true);
        skillBtn[1].interactable = false;
    }

    private void GameReward()
    {
    }
}