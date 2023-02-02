using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class JegerWalkToTarget : Node
{
    NavMeshAgent mAgent;
    float walkSpeed;
    public JegerWalkToTarget(NavMeshAgent agent)
    {
        mAgent = agent;
        walkSpeed = agent.speed;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") == null)
        {

            return NodeState.FAILURE;
        }
        if (parent.GetData("Shooting") != null)
        {
            if ((bool)parent.GetData("Shooting"))
            {
                
                return NodeState.SUCCESS;
            }
        }


        float speed = walkSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = speed;


        if ((Transform)parent.GetData("Target") == null)
        {
            parent.ClearData("Target");
            return NodeState.FAILURE;
        }
        Transform transform = (Transform)parent.GetData("Target");

        if (Vector3.Distance(transform.position, mAgent.transform.position) < 10)
        {
            mAgent.isStopped = true;
            parent.SetData("Shooting", true);
            return NodeState.SUCCESS;
        }
        mAgent.SetDestination(transform.position);
        return NodeState.RUNNING;
    }
}

