using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class JegerWalkHome : Node
{
    NavMeshAgent mAgent;
    float walkSpeed;
    public JegerWalkHome(NavMeshAgent agent)
    {
        mAgent = agent;
        walkSpeed = agent.speed;
    }
    public override NodeState Evaluate()
    {

        float speed = walkSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = speed;

        if (GetData("Target Position") == null)
        {
            Vector3 pos = new Vector3(Random.Range(-120, -110), -10, Random.Range(10, 20));
            SetData("Target Position", pos);
        }

        if (GetData("Target Position") != null)
        {
            mAgent.SetDestination((Vector3)GetData("Target Position"));
        }

        return NodeState.SUCCESS;
    }
}
