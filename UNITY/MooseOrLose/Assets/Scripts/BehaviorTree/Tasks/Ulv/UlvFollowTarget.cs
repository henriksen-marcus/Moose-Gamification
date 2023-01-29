using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class UlvFollowTarget : Node
{
    NavMeshAgent mAgent;
    public UlvFollowTarget(NavMeshAgent agent)
    {
        mAgent = agent;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") == null)
        {
            return NodeState.FAILURE;
        }

        Transform transform = (Transform)parent.GetData("Target");
        mAgent.SetDestination(transform.position);
        
        if (Vector3.Distance(transform.position, mAgent.transform.position) < 3)
        {
            Debug.Log("Eat");
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
