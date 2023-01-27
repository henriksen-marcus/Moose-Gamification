using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using BehaviorTrees;
using UnityEngine.AI;

public class WalkRandom : Node
{

    NavMeshAgent mAgent;
    float timer;
    Transform mTransform;

    public WalkRandom(NavMeshAgent agent, Transform transform)
    {
        mAgent = agent;
        mTransform = transform;
        timer = 0f;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (timer > 5f)
        {
            timer = 0f;
            mAgent.SetDestination(mTransform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
            return NodeState.RUNNING;
        }


        return NodeState.FAILURE;

    }
}
