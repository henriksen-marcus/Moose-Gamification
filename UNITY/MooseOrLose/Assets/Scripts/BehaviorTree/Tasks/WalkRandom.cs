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
    float walkDistance;


    public WalkRandom(NavMeshAgent agent, Transform transform, float _walkDistance)
    {
        mAgent = agent;
        mTransform = transform;
        walkDistance = _walkDistance;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;


        if (timer > 5f)
        {
            timer = 0f;
            mAgent.SetDestination(mTransform.position + new Vector3(Random.Range(-walkDistance, walkDistance), 0, Random.Range(-walkDistance, walkDistance)));
            return NodeState.RUNNING;
        }


        return NodeState.FAILURE;

    }
}
