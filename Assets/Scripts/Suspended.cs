using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SuspendedParticle
{
    Small = 0,
    Medium,
    Large,
}

public class Suspended
{
    private int Life { get; set; }
    private int Reward { get; set; }
    private SuspendedParticle suspendedParticle { get; set; }

    public Suspended(SuspendedParticle set)
    {
        if(set == SuspendedParticle.Small)
        {
            Life = 1;
            Reward = 10;
        }
        else if(set == SuspendedParticle.Medium)
        {
            Life = 2;
            Reward = 20;
        }
        else if (set == SuspendedParticle.Large)
        {
            Life = 3;
            Reward = 30;
        }

        suspendedParticle = set;
    }

    public void Hit()
    {
        Life--;

        if(Life == 0)
        {
            Debug.Log("Get "+Reward+"Reward");
        }
    }
}
