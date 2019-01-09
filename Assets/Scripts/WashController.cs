using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WashController : ViewController
{
    public CoolDown coolDown;
    public GameObject video;
    public Transform resetPoint;

    private string title;
    private string message;

    public delegate void RewardEvent();
    public static event RewardEvent WashReward;

    private void Start()
    {
        BedController.Sleep += Sleep;
        BedController.SleepReward += SleepReward;
        AlertViewController.OnEvent += OnEvent;
        AlertViewController.OffEvent += OffEvent;
        PlayerFSM.Arrive += Arrive;

        video.SetActive(false);
        coolDown.isAlertView = false;
        coolDown.isCoolTime = true;
        title = "알림";
    }

    private void OnMouseDown()
    {
        if (coolDown.isSleeping || coolDown.isAlertView)
            return;

        if (coolDown.isCoolTime)
        {
            message = "샤워를 시키겠습니까?";

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
            message = "아직 몸에서 향기가 난다";
            AlertViewController.Show(title, message);
        }
    }

    public void Arrive()
    {
        if (coolDown.coolDownState == PlayerFSM.instance.curCoolDownState)
        {
            //콘텐츠 재생
            StartCoroutine(WashVideoPlay());
        }
    }

    IEnumerator WashVideoPlay()
    {
        video.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        PlayerFSM.instance.TurnObj(resetPoint);
        yield return new WaitForSeconds(4f);
        video.SetActive(false);
        WashReward();
        yield return StartCoroutine(CheckCoolTime(coolDown.coolTime));
    }

    //CoolTime 체크함수
    IEnumerator CheckCoolTime(float time)
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
