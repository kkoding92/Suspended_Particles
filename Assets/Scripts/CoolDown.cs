using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CoolDownState
{
    SleepCoolTime = 0,
    WatchCoolTime,
    VentialationCoolTime,
    WashCoolTime,
    DrinkCoolTime,
}

[System.Serializable]
public class CoolDown
{
    public CoolDownState coolDownState;
    public float coolTime;
    public bool isCoolTime;
    [HideInInspector]
    public bool isSleeping;
}

