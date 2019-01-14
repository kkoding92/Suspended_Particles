using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Skill_Item { car = 0, tree, rain };

[System.Serializable]
public class Skill
{
    public Skill_Item skill_item;
    public int price;
    public string title;
}

public class SkillController : MonoBehaviour
{
    public Skill skill;
    public GameManager gm;

    public delegate void SkillRewardEvent(Skill_Item item, int price);

    public static event SkillRewardEvent SkillReward;

    private string message;

    public void OnClickBuySkill()
    {
        switch (skill.skill_item)
        {
            case Skill_Item.car:
                if (gm.carSkillCount == 0)
                {
                    message = "전체 미세먼지의 20%를 감소시켜 줍니다. 구매 하시겠습니까?";
                    CallAlertView();
                }
                else
                {
                    message = "이미 보유한 스킬입니다";
                    AlertViewController.Show(skill.title, message);
                }
                break;

            case Skill_Item.rain:
                if (gm.rainSkillCount == 0)
                {
                    message = "전체 미세먼지의 20%를 감소시켜 줍니다. 구매 하시겠습니까?";
                    CallAlertView();
                }
                else
                {
                    message = "이미 보유한 스킬입니다";
                    AlertViewController.Show(skill.title, message);
                }
                break;

            case Skill_Item.tree:
                if (gm.treeSkillCount == 0)
                {
                    message = "전체 미세먼지의 20%를 감소시켜 줍니다. 구매 하시겠습니까?";
                    CallAlertView();
                }
                else
                {
                    message = "이미 보유한 스킬입니다";
                    AlertViewController.Show(skill.title, message);
                }
                break;
        }
    }

    private void CallAlertView()
    {
        AlertViewController.Show(skill.title, message, new AlertViewOptions
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
                if (gm.Coin > skill.price)
                    SkillReward(skill.skill_item, skill.price);
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