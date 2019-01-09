using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowController : ViewController
{
    public CoolDown coolDown;
    public Animation ani;

    private string title;
    private string message;

    public delegate void RewardEvent();
    public static event RewardEvent VentialationReward;

    private void Start()
    {
        BedController.Sleep += Sleep;
        BedController.SleepReward += SleepReward;
        AlertViewController.OnEvent += OnEvent;
        AlertViewController.OffEvent += OffEvent;
        PlayerFSM.Arrive += Arrive;

        ani.Stop();
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
            message = "환기를 하시겠습니까?";

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
            message = "산소가 충분합니다.";
            AlertViewController.Show(title, message);
        }
    }

    public void Arrive()
    {
        if (coolDown.coolDownState == PlayerFSM.instance.curCoolDownState)
        {
            //콘텐츠 재생
            StartCoroutine(OpenWindow());
        }
    }
    
    IEnumerator OpenWindow()
    {
        ani.Play();
        PlayerFSM.instance.TurnObj();
        yield return new WaitForSeconds(9f);
        ani.Stop();
        VentialationReward();
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
