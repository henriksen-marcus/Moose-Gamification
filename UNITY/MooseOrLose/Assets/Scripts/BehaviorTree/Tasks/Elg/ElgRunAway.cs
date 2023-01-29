using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class ElgRunAway : Node
{
    NavMeshAgent mAgent;
    Transform mTransform;
    public ElgRunAway(NavMeshAgent agent, Transform transform)
    {
        mAgent = agent;
        mTransform = transform;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") == null)
        {
            return NodeState.FAILURE;
        }
        Transform Danger = (Transform)parent.GetData("Danger");
        Vector3 direction = mTransform.position - Danger.position;
        mAgent.SetDestination(mTransform.position + (direction * 5));
        return NodeState.SUCCESS;
    }
}
