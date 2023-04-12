using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class ElgGoToForest : Node
{
    NavMeshAgent mAgent;
    Elg mScript;
    float mSpeed;
    float acceleration;
    public ElgGoToForest(NavMeshAgent agent)
    {
        mAgent = agent;
        mSpeed = agent.speed;
        mScript = agent.GetComponent<Elg>();
        acceleration = agent.acceleration;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Forest") == null)
        {
            return NodeState.FAILURE;
        }
        float ratio = (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = mSpeed * ratio;
        mAgent.acceleration = acceleration * ratio;
        if ((Forest)parent.GetData("Forest") == null)
        {
            parent.ClearData("Forest");
            return NodeState.FAILURE;
        }
        Forest forest = (Forest)parent.GetData("Forest");
        
        if (Vector3.Distance(mAgent.destination, mAgent.transform.position) < 2)
        {
            return NodeState.SUCCESS;
        }
        mAgent.SetDestination(forest.transform.position + new Vector3(Random.Range(-10, 10), 0f, Random.Range(-10, 10)));
        mScript.AIstate = ElgState.Eating;
        return NodeState.RUNNING;
    }
}