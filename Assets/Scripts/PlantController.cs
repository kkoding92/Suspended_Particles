using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Plant { Stuckyi, PalmTree}

public class PlantController : MonoBehaviour
{
    public CoolDown coolDown;
    public Plant plant;
    public GameManager gm;
    private string title;
    private string message;

    public delegate void RewardEvent(Plant plant);
    public static event RewardEvent GrowReward;

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
            message = "흙이 말라있습니다. 물을 주시겠습니까?";

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
                    CheckGrowCount();
                },
            });
        }
        else
        {
            message = "흙이 아직 축축하다";
            AlertViewController.Show(title, message);
        }
    }

    private void CheckGrowCount()
    {
        if(plant == Plant.PalmTree)
        {
            if (gm.palmGrowCount == 2)
                GrowReward(plant);
        }
        else if (plant == Plant.Stuckyi)
        {
            if (gm.stuckyiGrowCount == 2)
                GrowReward(plant);
        }
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
