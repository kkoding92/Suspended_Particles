using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalController : MonoBehaviour
{
    public CoolDown coolDown;

    public GameObject[] shopObj;
    public GameObject[] earthObj;
    public GameObject[] gameUI;

    public Animator anim;

    private string title;
    private string message;

    private void Start()
    {
        anim.enabled = false;
        BedController.Sleep += Sleep;
        BedController.SleepReward += SleepReward;
        AlertViewController.OnEvent += OnEvent;
        AlertViewController.OffEvent += OffEvent;

        shopObj[0].SetActive(false);
        earthObj[0].SetActive(false);
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
            message = "포탈을 통해 밖으로 미세먼지와 싸우거나 상점을 갈 수 있습니다. 포탈을 작동 시키겠습니까?";

            AlertViewController.Show(title, message, new AlertViewOptions
            {
                //취소 버튼의 타이틀과 버튼을 눌렀을 때 실행되는 델리게이트를 설정한다.
                cancelButtonTitle = "취소",
                cancelButtonDelegate = () =>
                {
                },

                //OK 버튼의 타이틀과 버튼을 눌렀을 때 실행되는 델리게이트를 설정한다.
                okButtonTitle = "출발",
                okButtonDelegate = () =>
                {
                    StartCoroutine(CheckDestination());
                },
            });
        }
        else
        {
            message = "포탈을 열기 위해서는 에너지가 더 필요합니다.";
            AlertViewController.Show(title, message);
        }
    }

    IEnumerator CheckDestination()
    {
        yield return new WaitForSeconds(.1f);
        message = "이동하고자 하는 장소를 입력하세요";
        AlertViewController.Show(title, message, new AlertViewOptions
        {
            //취소 버튼의 타이틀과 버튼을 눌렀을 때 실행되는 델리게이트를 설정한다.
            cancelButtonTitle = "지구",
            cancelButtonDelegate = () =>
            {
                gameUI[0].SetActive(false);
                gameUI[1].SetActive(false);
                shopObj[0].SetActive(false);
                earthObj[0].SetActive(true);
                earthObj[2].SetActive(true);
                earthObj[4].SetActive(true);
                StartCoroutine(StartARGame());

                anim.enabled = true;
            },

            //OK 버튼의 타이틀과 버튼을 눌렀을 때 실행되는 델리게이트를 설정한다.
            okButtonTitle = "상점",
            okButtonDelegate = () =>
            {
                gameUI[0].SetActive(false);
                gameUI[1].SetActive(false);
                shopObj[0].SetActive(true);
                shopObj[1].SetActive(true);
                shopObj[2].SetActive(true);
                earthObj[0].SetActive(false);
                earthObj[1].SetActive(false);

                anim.enabled = true;
            },
        });
    }

    IEnumerator StartARGame()
    {
        yield return new WaitForSeconds(10f);
        earthObj[1].SetActive(true);
        earthObj[3].SetActive(true);
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
