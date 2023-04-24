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
        float ratio = (TimeManager.Instance.defaultPlaySpeed / TimeManager.Instance.playSpeed);
        mAgent.speed = mSpeed * ratio;
        mAgent.acceleration = acceleration * ratio;
        if ((Forest)parent.GetData("Forest") == null)
        {
            parent.ClearData("Forest");
            return NodeState.FAILURE;
        }
        Forest forest = (Forest)parent.GetData("Forest");
        

        if (parent.GetData("Destination") == null)
        {
            
            parent.SetData("Destination", forest.transform.position + new Vector3(Random.Range(-5, 5), 0f, Random.Range(-5, 5)));
            mAgent.SetDestination((Vector3)parent.GetData("Destination"));
        }

        if (Vector3.Distance(mAgent.destination, mAgent.transform.position) < 1f)
        {
            return NodeState.SUCCESS;
        }

        mScript.AIstate = ElgState.Eating;
        return NodeState.RUNNING;
    }
}