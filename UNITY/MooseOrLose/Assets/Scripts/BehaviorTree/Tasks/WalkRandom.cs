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
    Elg mScript;

    public WalkRandom(NavMeshAgent agent, Transform transform, Elg script)
    {
        mAgent = agent;
        mTransform = transform;
        timer = 0f;
        mScript = script;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (mScript.hasMother)
        {
            if (mScript.age_years < 2)
            {
                if (timer > 3f)
                {
                    timer = 0f;
                    Debug.Log("Im a kid, trying to reach mommy");
                    mAgent.SetDestination(mScript.mother.position);
                    return NodeState.RUNNING;
                }
            }
        }

        if (timer > 5f)
        {
            timer = 0f;
            mAgent.SetDestination(mTransform.position + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
            return NodeState.RUNNING;
        }


        return NodeState.FAILURE;

    }
}
