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
    public UlvFollowPackLeader(NavMeshAgent agent)
    {
        mAgent = agent;
        walkSpeed = mAgent.speed ;
        mScript = mAgent.GetComponent<Ulv>();
    }

    public override NodeState Evaluate()
    {
        if (mScript.isLeader)
        {
            return NodeState.SUCCESS;
        }

        
        float speed = walkSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = speed;

        mAgent.SetDestination(mScript.leader.position);

        return NodeState.FAILURE;
    }
}

