using System.Collections;
using UnityEngine;

public class WaterController : ViewController
{
    public CoolDown coolDown;
    public GameObject cupObj;
    public GameObject camObj;
    private string title;
    private string message;

    public delegate void RewardEvent();
    public static event RewardEvent DrinkReward;

    private void Start()
    {
        BedController.Sleep += Sleep;
        BedController.SleepReward += SleepReward;
        AlertViewController.OnEvent += OnEvent;
        AlertViewController.OffEvent += OffEvent;
        PlayerFSM.Arrive += Arrive;

        camObj.SetActive(false);
        cupObj.SetActive(false);
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
            message = "물을 먹이겠습니까?";

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
            message = "지금은 배가 부릅니다.";
            AlertViewController.Show(title, message);
        }
    }

    public void Arrive()
    {
        if (coolDown.coolDownState == PlayerFSM.instance.curCoolDownState)
        {
            //콘텐츠 재생
            StartCoroutine(ShowDrinkWater());
        }
    }

    IEnumerator ShowDrinkWater()
    {
        cupObj.SetActive(true);
        camObj.SetActive(true);
        yield return new WaitForSeconds(6f);
        cupObj.SetActive(false);
        PlayerFSM.instance.TurnObj();
        camObj.SetActive(false);
        DrinkReward();
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
