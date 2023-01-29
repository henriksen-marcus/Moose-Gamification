using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class FollowMother : Node
{

    NavMeshAgent mAgent;
    float timer;
    Transform mTransform;
    Elg mScript;

    public FollowMother(NavMeshAgent agent, Transform transform, Elg script)
    {
        mAgent = agent;
        mTransform = transform;
        mScript = script;
        timer = 0;
    }
    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (mScript.mother != null)
        {
            if (mScript.age_years < 1)
            {
                if (timer > 3f)
                {
                    timer = 0f;
                    mAgent.SetDestination(mScript.mother.position);
                    return NodeState.RUNNING;
                }
            }
        }
        return NodeState.FAILURE;

    }
}
