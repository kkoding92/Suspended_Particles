using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CharacterState
{
    Idle,
    Idle2,
    Drink,
    Walk,
    Dust,
}

public enum Destination
{
    None,
    Bed,
    Wash,
    TV,
    Water,
    Plant,
}

public class PlayerFSM : MonoBehaviour
{
    public static PlayerFSM instance = null;

    public delegate void ArriveEvent();
    public static event ArriveEvent Arrive;

    public Transform bedPos;
    public Transform washPos;
    public Transform tvPos;
    public Transform waterPos;
    public Transform plantPos;

    public GameObject target;
    public GameObject character;
    public GameObject[] sleepChar;

    public CoolDownState curCoolDownState = CoolDownState.None;
    public CharacterState currentState = CharacterState.Idle;

    private NavMeshAgent agent;
    private PlayerAnim myAni;

    private bool isWalking;
    private bool isCheckWalking;

    void Start()
    {
        if (instance == null)
            instance = this;

        isCheckWalking = false;

        agent = gameObject.GetComponent<NavMeshAgent>();
        myAni = GetComponent<PlayerAnim>();

        ChangeState(CharacterState.Idle, PlayerAnim.Anim_Idle);
    }
    
    public void ChangeState(CharacterState newState, int aniNumber)
    {
        if (currentState == newState)
            return;
        myAni.ChangeAnim(aniNumber);
        currentState = newState;
    }

    void LateUpdate()
    {
        UpdateState();
    }

    void UpdateState()
    {
        switch (currentState)
        {
            case CharacterState.Idle:
                int ranVar = Random.Range(1, 10000);
                if(ranVar<10)
                    ChangeState(CharacterState.Idle2, PlayerAnim.Anim_Idle2);
                break;
            case CharacterState.Idle2:
                    ChangeState(CharacterState.Idle, PlayerAnim.Anim_Idle);
                break;
            case CharacterState.Drink:
                ChangeState(CharacterState.Idle, PlayerAnim.Anim_Idle);
                break;
            case CharacterState.Walk:
                if (!CheckArrived())
                {
                    if (curCoolDownState == CoolDownState.DrinkCoolTime)
                        ChangeState(CharacterState.Drink, PlayerAnim.Anim_Drink);
                    else
                        ChangeState(CharacterState.Idle, PlayerAnim.Anim_Idle);
                }
                break;
            case CharacterState.Dust:
                break;
        }
    }

    public bool CheckArrived()
    {
        if (!agent.pathPending)
        {
            if (agent.remainingDistance <= agent.stoppingDistance)
            {
                if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
                {
                    isCheckWalking = false;
                    Arrive();
                    return false;
                }
            }
        }
        return true;
    }

    public void SetDestination(CoolDownState receive)
    {
        isWalking = true;
        curCoolDownState = receive;

        switch (receive)
        {
            case CoolDownState.DrinkCoolTime:
                agent.SetDestination(waterPos.position);
                break;
            case CoolDownState.WashCoolTime:
                agent.SetDestination(washPos.position);
                break;
            case CoolDownState.WatchCoolTime:
                agent.SetDestination(tvPos.position);
                break;
            case CoolDownState.VentialationCoolTime:
                agent.SetDestination(plantPos.position);
                break;
            case CoolDownState.GrowTime:
                agent.SetDestination(plantPos.position);
                break;
            case CoolDownState.SleepCoolTime:
                agent.SetDestination(bedPos.position);
                break;
        }
        ChangeState(CharacterState.Walk, PlayerAnim.Anim_Walk);
        isCheckWalking = true;
    }

    public void TurnObj(Transform point=null)
    {
        if (point != null)
        {
            gameObject.transform.position = point.position;
            agent.ResetPath();
        }
            
        gameObject.transform.LookAt(target.transform);
    }
}
