using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;
public class ElgMatingSeason : Node
{
    Elg mScript;
    public ElgMatingSeason(Elg script)
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


        if (TimeManager.instance.MatingSeason() && mScript.gender == Gender.Female)
        {
            if (!mScript.hasMated && mScript.GetAge() > 2f && mScript.GetAge() < 17f)
                return NodeState.SUCCESS;
        }
        else
        {
            mScript.hasMated = false;
        }
        return NodeState.FAILURE;
    }
}
