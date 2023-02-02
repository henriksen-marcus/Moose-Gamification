using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class UlvFollowTarget : Node
{
    NavMeshAgent mAgent;
    Ulv mScript;
    float runSpeed;
    public UlvFollowTarget(NavMeshAgent agent)
    {
        mAgent = agent;
        runSpeed = mAgent.speed * (5f / 3f);
        mScript = agent.GetComponent<Ulv>();
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") == null)
        {
            mScript.hasTarget = false;
            return NodeState.FAILURE;
        }
        mScript.hasTarget = true;
        float speed = runSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = speed;
        if ((Transform)parent.GetData("Target") == null)
        {
            parent.ClearData("Target");
            return NodeState.FAILURE;
        }
        Transform transform = (Transform)parent.GetData("Target");
        mAgent.SetDestination(transform.position);
        
        if (Vector3.Distance(transform.position, mAgent.transform.position) < 3)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
