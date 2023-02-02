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
    public UlvFollowPackLeader(NavMeshAgent agent)
    {
        mAgent = agent;
        walkSpeed = mAgent.speed;
        runSpeed = walkSpeed * (5f / 3f);
        mScript = mAgent.GetComponent<Ulv>();
    }

    public override NodeState Evaluate()
    {
        if (mScript.isLeader)
        {
            return NodeState.SUCCESS;
        }

        if (mScript.leader.GetComponent<Ulv>().hasTarget)
        {
            float speed = runSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
            mAgent.speed = speed;
        }
        else
        {
            float speed = walkSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
            mAgent.speed = speed;
        }
        mAgent.SetDestination(mScript.leader.position);

        return NodeState.FAILURE;
    }
}

