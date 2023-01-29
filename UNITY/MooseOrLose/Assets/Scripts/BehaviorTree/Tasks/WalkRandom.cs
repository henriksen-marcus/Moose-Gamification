using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTrees;
using UnityEngine.AI;

public class WalkRandom : Node
{

    NavMeshAgent mAgent;
    Transform mTransform;

    float timer;


    public WalkRandom(NavMeshAgent agent, Transform transform)
    {
        mAgent = agent;
        mTransform = transform;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;


        if (timer > 5f)
        {
            timer = 0f;
            mAgent.SetDestination(mTransform.position + new Vector3(Random.Range(-20, 20), 0, Random.Range(-20, 20)));
            return NodeState.RUNNING;
        }


        return NodeState.FAILURE;

    }
}
