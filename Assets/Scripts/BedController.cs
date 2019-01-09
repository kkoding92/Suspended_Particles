using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : ViewController
{
    public CoolDown coolDown;
    public float sleepTime;
    private string title;
    private string message;

    public delegate void RewardEvent();
    public static event RewardEvent SleepReward;
    public static event RewardEvent Sleep;

    private void Start()
    {
        AlertViewController.OnEvent += OnEvent;
        AlertViewController.OffEvent += OffEvent;
        PlayerFSM.Arrive += Arrive;

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
            message = "정말로 재우겠습니까?";

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
                    //ok시 보여줄 콘텐츠 재생
                    PlayerFSM.instance.SetDestination(coolDown.coolDownState);
                },
            });
        }
        else
        {
            message = "지금은 졸리지 않습니다.";
            AlertViewController.Show(title, message);
        }
    }

    public void Arrive()
    {
        if (coolDown.coolDownState == PlayerFSM.instance.curCoolDownState)
        {
            //잠자는 로직 처리
            coolDown.isSleeping = true;
            Sleep();
            PlayerFSM.instance.character.SetActive(false);
            PlayerFSM.instance.sleepChar[0].SetActive(false);
            PlayerFSM.instance.sleepChar[1].SetActive(true);
            StartCoroutine(SleepTimeCheck(sleepTime));
        }
    }

    //일어났을 때 처리
    private void WakeUp()
    {
        PlayerFSM.instance.TurnObj();
        coolDown.isSleeping = false;
        PlayerFSM.instance.character.SetActive(true);
        PlayerFSM.instance.sleepChar[0].SetActive(true);
        PlayerFSM.instance.sleepChar[1].SetActive(false);
        SleepReward();
        StartCoroutine(CheckCoolTime(coolDown.coolTime));
    }

    //CoolTime 체크 코루틴
    IEnumerator CheckCoolTime(float time)
    {
        coolDown.isCoolTime = false;
        yield return new WaitForSeconds(time);
        coolDown.isCoolTime = true;
    }

    //SleepTime 체크 코루틴
    IEnumerator SleepTimeCheck(float time)
    {
        yield return new WaitForSeconds(time);
        WakeUp();
    }

    public void OnEvent()
    {
        coolDown.isAlertView = true;
    }

    public void OffEvent()
    {
        coolDown.isAlertView = false;
    }
}
