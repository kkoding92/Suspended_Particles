using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Item
{
    Cleansing=0,
    SkillBook,
    Stuckyi,
    PalmTree,
    AirCleaner,
    KF80,
    KF94,
    KF99
}

[System.Serializable]
public class StoreItem
{
    public Item item;
    public int price;
    public string title;
}

public class StoreController : MonoBehaviour
{
    public StoreItem storeItem;
    public GameManager gm;
    public GameObject UICam;
    public GameObject previewObj;
    public GameObject skillUiObj;

    private string message;
    private bool isEvent;
    public delegate void StoreRewardEvent(Item item, int price);
    public static event StoreRewardEvent StoreReward;

    private void Start()
    {
        isEvent = false;

        StoreAlertViewController.OnEvent += OnEvent;
        StoreAlertViewController.OffEvent += OffEvent;

        UICam.SetActive(false);
    }
    private void OnMouseDown()
    {
        if (isEvent || skillUiObj.activeSelf)
            return;

        SetMessage();
    }

    private void SetMessage()
    { 
        switch (storeItem.item)
        {
            case Item.AirCleaner:
                if (gm.checkAirCleaner)
                {
                    message = "이미 보유했습니다.";
                    StoreAlertViewController.Show(storeItem.title, message);
                }
                else
                {
                    message = "미세먼지와 기타 유해물질을 걸러줍니다. 구매 하시겠습니까?";
                    CallStoreAlertView();
                }
                break;
            case Item.Cleansing:
                if (gm.checkCleansing)
                {
                    message = "이미 보유했습니다.";
                    StoreAlertViewController.Show(storeItem.title, message);
                }
                else
                {
                    message = "미세먼지를 효과적으로 씻어줍니다. 구매 하시겠습니까?";
                    CallStoreAlertView();
                }
                break;
            case Item.Stuckyi:
                if (gm.checkStuckyi)
                {
                    message = "이미 보유했습니다.";
                    StoreAlertViewController.Show(storeItem.title, message);
                }
                else
                {
                    message = "미세먼지를 걸러주고 키우는 재미가 있는 식물입니다. 구매 하시겠습니까?";
                    CallStoreAlertView();
                }
                break;
            case Item.PalmTree:
                if (gm.checkPalmTree)
                {
                    message = "이미 보유했습니다.";
                    StoreAlertViewController.Show(storeItem.title, message);
                }
                else
                {
                    message = "미세먼지를 걸러주고 키우는 재미가 있는 식물입니다. 구매 하시겠습니까?";
                    CallStoreAlertView();
                }
                break;
            case Item.SkillBook:
                message = "밖에서 쓸수 있는 스킬이 담겨있습니다. 구경하시겠습니까?";
                StoreAlertViewController.Show(storeItem.title, message, new StoreAlertViewOptions
                {
                    cancelButtonTitle = "아니요",
                    cancelButtonDelegate = () =>
                    {
                    },

                    okButtonTitle = "네",
                    okButtonDelegate = () =>
                    {
                        skillUiObj.SetActive(true);
                    },
                });
                break;
            case Item.KF80:
                if (gm.maskState == MaskState.basic)
                {
                    message = "미세먼지를 막아줘 밖에서 더 오래 있을 수 있습니다. 업그레이드 하겠습니까?";
                    CallStoreAlertView();
                }
                else
                {
                    message = "이미 보유했습니다.";
                    StoreAlertViewController.Show(storeItem.title, message);
                }
                break;
            case Item.KF94:
                if (gm.maskState == MaskState.KF80)
                {
                    message = "미세먼지를 막아줘 밖에서 더 오래 있을 수 있습니다. 업그레이드 하겠습니까?";
                    CallStoreAlertView();
                }
                else
                {
                    message = "이미 보유했습니다.";
                    StoreAlertViewController.Show(storeItem.title, message);
                }
                break;
            case Item.KF99:
                if (gm.maskState == MaskState.KF94)
                {
                    message = "미세먼지를 막아줘 밖에서 더 오래 있을 수 있습니다. 업그레이드 하겠습니까?";
                    CallStoreAlertView();
                }
                else
                {
                    message = "이미 보유했습니다.";
                    StoreAlertViewController.Show(storeItem.title, message);
                }
                break;
        }
    }

    public void OnEvent()
    {
        isEvent = true;
        UICam.SetActive(true);
        previewObj.SetActive(true);
    }

    public void OffEvent()
    {
        isEvent = false;
        UICam.SetActive(false);
        previewObj.SetActive(false);
    }

    public void OnClickXBtn()
    {
        skillUiObj.SetActive(false);
    }

    private void CallStoreAlertView()
    {
        StoreAlertViewController.Show(storeItem.title, message, new StoreAlertViewOptions
        {
            cancelButtonTitle = "아니요",
            cancelButtonDelegate = () =>
            {
            },

            okButtonTitle = "네",
            okButtonDelegate = () =>
            {
                if (gm.Coin > storeItem.price)
                {
                    StoreReward(storeItem.item, storeItem.price);
                }
                else
                {
                    string title = "알림";
                    message = "잔액이 부족합니다.";
                    StoreAlertViewController.Show(title, message);
                }
            },
        });
    }
}
