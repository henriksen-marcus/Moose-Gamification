using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class UlvFollowPackLeader : Node
{
    NavMeshAgent mAgent;
    Ulv mScript;
    float walkSpeed;
    float runSpeed;
    float acceleration;
    public UlvFollowPackLeader(NavMeshAgent agent)
    {
        mAgent = agent;
        walkSpeed = mAgent.speed;
        runSpeed = walkSpeed * (5f / 3f);
        mScript = mAgent.GetComponent<Ulv>();
        acceleration = mAgent.acceleration;
    }

    public override NodeState Evaluate()
    {
        if (mScript.isLeader)
        {
            return NodeState.SUCCESS;
        }

        if (mScript.leader.GetComponent<Ulv>().hasTarget)
        {
            float speed = runSpeed * (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
            mAgent.speed = speed;
            mAgent.acceleration = acceleration * (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
        }
        else
        {
            float speed = walkSpeed * (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
            mAgent.speed = speed;
            mAgent.acceleration = acceleration * (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
        }
        mAgent.SetDestination(mScript.leader.position + new Vector3(Random.Range(-2,2),0,Random.Range(-2,2)));

        return NodeState.FAILURE;
    }
}

