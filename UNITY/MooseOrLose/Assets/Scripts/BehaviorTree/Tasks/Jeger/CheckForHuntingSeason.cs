using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class CheckForHuntingSeason : Node
{
    NavMeshAgent mAgent;
    public CheckForHuntingSeason(NavMeshAgent agent)
    {
        mAgent = agent;
    }
    public override NodeState Evaluate()
    {
        if (RuleManager.Instance.HuntingSeason())
        {

            return NodeState.SUCCESS;
        }
        parent.SetData("Shooting", false);
        parent.ClearData("Target");
        mAgent.isStopped = false;
        return NodeState.FAILURE;
    }
}
