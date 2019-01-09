using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public const int Anim_Idle = 0;
    public const int Anim_Idle2 = 1;
    public const int Anim_Dust = 2;
    public const int Anim_Drink = 3;
    public const int Anim_Walk = 4;

    Animator anim;

    void Start()
    {
        anim = GetComponent<Animator>();
    }

    public void ChangeAnim(int aniNumber)
    {
        anim.SetInteger("aniName", aniNumber);
    }
}
