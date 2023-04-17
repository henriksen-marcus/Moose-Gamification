using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;


public class UlvHungerCheck : Node
{
    Ulv mScript;
    public UlvHungerCheck(Ulv script)
    {
        mScript = script;
    }
    public override NodeState Evaluate()
    {
        if (TimeManager.instance.gamePaused)
        {
            if (mScript.GetComponent<NavMeshAgent>().isOnNavMesh)
                mScript.GetComponent<NavMeshAgent>().isStopped = true;
        }
        else
        {
            if (mScript.GetComponent<NavMeshAgent>().isOnNavMesh)
                mScript.GetComponent<NavMeshAgent>().isStopped = false;
        }

        if (parent.GetData("Target") != null)
        {
            return NodeState.SUCCESS;
        }

        if (mScript != null)
        {
            if (mScript.hunger < 25)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;

    }
}
