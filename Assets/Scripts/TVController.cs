using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TVController : ViewController
{
    public CoolDown coolDown;
    public GameObject TVPanel;
    public Sprite[] tvView;
    public Image tv;

    private string title;

    private string message;

    public delegate void RewardEvent();

    public static event RewardEvent WatchReward;

    private void Start()
    {
        BedController.Sleep += Sleep;
        BedController.SleepReward += SleepReward;
        AlertViewController.OnEvent += OnEvent;
        AlertViewController.OffEvent += OffEvent;
        PlayerFSM.Arrive += Arrive;

        coolDown.isAlertView = false;
        coolDown.isCoolTime = true;
        title = "알림";
        TVPanel.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (coolDown.isSleeping || coolDown.isAlertView)
            return;

        if (coolDown.isCoolTime)
        {
            message = "정말로 TV를 시청하시겠습니까?";

            AlertViewController.Show(title, message, new AlertViewOptions
            {
                //취소 버튼의 타이틀과 버튼을 눌렀을 때 실행되는 델리게이트를 설정한다.
                cancelButtonTitle = "아니요",
                cancelButtonDelegate = () =>
                {
                },

                //OK 버튼의 타이틀과 버튼을 눌렀을 때 실행되는 델리게이트를 설정한다.
                okButtonTitle = "네",
                okButtonDelegate = () =>
                {
                    PlayerFSM.instance.SetDestination(coolDown.coolDownState);
                },
            });
        }
        else
        {
            message = "지금은 방송 시간이 아닙니다.";
            AlertViewController.Show(title, message);
        }
    }

    public void Arrive()
    {
        if (coolDown.coolDownState == PlayerFSM.instance.curCoolDownState)
        {
            StartCoroutine(PlayTV());
            //콘텐츠 재생
            //TV를 다 보면 보상 및 CoolTime 체크
            PlayerFSM.instance.TurnObj();
            WatchReward();
            StartCoroutine(CheckCoolTime(coolDown.coolTime));
        }
    }

    private IEnumerator PlayTV()
    {
        TVPanel.SetActive(true);

        int ran = Random.Range(0, 4);
        if (ran == 0)
            tv.sprite = tvView[0];
        else if (ran == 1)
            tv.sprite = tvView[1];
        else if (ran == 2)
            tv.sprite = tvView[2];

        yield return new WaitForSeconds(5f);

        PlayerFSM.instance.TurnObj();
        WatchReward();
        TVPanel.SetActive(false);
        yield return StartCoroutine(CheckCoolTime(coolDown.coolTime));
    }

    //CoolTime 체크함수
    private IEnumerator CheckCoolTime(float time)
    {
        coolDown.isCoolTime = false;
        yield return new WaitForSeconds(time);
        coolDown.isCoolTime = true;
    }

    public void OnEvent()
    {
        coolDown.isAlertView = true;
    }

    public void OffEvent()
    {
        coolDown.isAlertView = false;
    }

    public void Sleep()
    {
        coolDown.isSleeping = true;
    }

    public void SleepReward()
    {
        coolDown.isSleeping = false;
    }
}