using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedController : ViewController
{
    public GameObject Mask;
    public CoolDown coolDown;
    public float sleepTime;
    private string title;
    private string message;

    public delegate void RewardEvent();
    public static event RewardEvent SleepReward;
    public static event RewardEvent Sleep;

    private void Start()
    {
        coolDown.isCoolTime = true;
        title = "알림";
    }

    private void OnMouseDown()
    {
        if (coolDown.isSleeping)
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
                    ShowAnim();
                },
            });
        }
        else
        {
            message = "지금은 졸리지 않습니다.";
            AlertViewController.Show(title, message);
        }
    }

    //침대까지 걷기 애니메이션 재생함수
    private void ShowAnim()
    {
        GoToSleep();
    }

    //잠자는 처리 함수
    private void GoToSleep()
    {
        Mask.SetActive(true);
        coolDown.isSleeping = true;
        Sleep();
        StartCoroutine(SleepTimeCheck(sleepTime));
    }

    private void WakeUp()
    {
        Mask.SetActive(false);
        //TV를 다 보면 보상 및 CoolTime 체크
        coolDown.isSleeping = false;
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
}
