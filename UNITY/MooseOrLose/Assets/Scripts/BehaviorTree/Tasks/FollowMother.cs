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

    float speed;
    float acceleration;

    public FollowMother(NavMeshAgent agent, Transform transform, Elg script)
    {
        mAgent = agent;
        mTransform = transform;
        mScript = script;
        timer = 0;
        speed = mAgent.speed;
        acceleration = mAgent.acceleration;
    }
    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;

        if (mScript.mother != null)
        {
            if (mScript.age_months < 10)
            {
                if (timer > TimeManager.instance.playSpeed / 2)
                {
                    timer = 0f;
                    mAgent.speed = speed * (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
                    mAgent.acceleration = acceleration * (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
                    mAgent.SetDestination(mScript.mother.position);
                    return NodeState.RUNNING;
                }
            }
        }
        return NodeState.FAILURE;

    }
}
