using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ARGameManager : MonoBehaviour
{
    public GameManager gm;
    public GameObject[] objectsToSpawn;
    //public float spawnRadius = 10.0f;
    //public int numberOfObjects = 10;
    public GameObject Oring;
    public GameObject emptyEnemy;
    public GameObject EnemyPrefab;

    private int num_enemies = 50;
    
    //머물수 있는 시간
    private float timer;
    //미세먼지 총 수
    private int numberOfFine;
    //보상
    private int rewardCoin;
    private int rewardDia;

    void Start()
    {
        //for(int i =0; i<numberOfObjects; i++)
        //{
        //    GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
        //    Vector2 spawnPositionV2 = Random.insideUnitCircle * spawnRadius;
        //    Vector3 spawnPosition = new Vector3(spawnPositionV2.x, 0.0f, spawnPositionV2.y);
        //    Vector3 transformOffsetSpawnPosition = transform.position + spawnPosition;

        //    Instantiate(objectToSpawn, transformOffsetSpawnPosition, Quaternion.identity);

        //}

        GameInit();
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

        //미세먼지 생성
        CreateFineDust();
    }

    private void CreateFineDust()
    {
        Vector3 enemyPos = emptyEnemy.transform.position;

        while (num_enemies > 0)
        {
            GameObject objectToSpawn = objectsToSpawn[Random.Range(0, objectsToSpawn.Length)];
            Instantiate(objectToSpawn, enemyPos, Oring.transform.rotation);
            num_enemies--;
            int angle = Random.Range(30, 90);
            emptyEnemy.transform.RotateAround(Oring.transform.position, Vector3.back, angle);
            enemyPos = emptyEnemy.transform.position;
        }
    }

    private void GameReward()
    {

    }
}
