using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TVController : ViewController
{
    public CoolDown coolDown;
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
                    PlayAnimation();
                },
            });
        }
        else
        {
            message = "지금은 방송 시간이 아닙니다.";
            AlertViewController.Show(title, message);
        }
    }

    private void PlayAnimation()
    {
        //ok시 보여줄 콘텐츠 재생
        ShowContents();
    }

    //TV에서 보여줄 콘텐츠 재생함수
    private void ShowContents()
    {
        //TV를 다 보면 보상 및 CoolTime 체크
        WatchReward();
        StartCoroutine(CheckCoolTime(coolDown.coolTime));
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
