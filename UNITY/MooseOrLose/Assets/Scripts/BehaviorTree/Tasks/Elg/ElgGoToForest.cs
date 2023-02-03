using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class ElgGoToForest : Node
{
    NavMeshAgent mAgent;
    float mSpeed;
    public ElgGoToForest(NavMeshAgent agent)
    {
        mAgent = agent;
        mSpeed = agent.speed;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Forest") == null)
        {
            return NodeState.FAILURE;
        }
        float speed = mSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = speed;
        if ((Forest)parent.GetData("Forest") == null)
        {
            parent.ClearData("Forest");
            return NodeState.FAILURE;
        }
        Forest forest = (Forest)parent.GetData("Forest");
        mAgent.SetDestination(forest.transform.position + new Vector3(Random.Range(-10,10),0f,Random.Range(-10,10)));

        if (Vector3.Distance(forest.transform.position, mAgent.transform.position) < 2)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}