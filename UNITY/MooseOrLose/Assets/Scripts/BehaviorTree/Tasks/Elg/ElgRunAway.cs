using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class ElgRunAway : Node
{
    NavMeshAgent mAgent;
    Elg mScript;
    Transform mTransform;
    float runSpeed;
    float acceleration;
    public ElgRunAway(NavMeshAgent agent, Transform transform)
    {
        mAgent = agent;
        mScript = agent.GetComponent<Elg>();
        mTransform = transform;
        runSpeed = mAgent.speed * (4f / 3f);
        acceleration = mAgent.acceleration;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Danger") == null)
        {
            return NodeState.FAILURE;
        }
        float ratio = (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = runSpeed * ratio;
        mAgent.acceleration = acceleration * ratio; 
        Transform Danger = (Transform)parent.GetData("Danger");
        Vector3 direction = mTransform.position - Danger.position;
        direction.Normalize();
        mAgent.SetDestination(mTransform.position + (direction * 25));
        mScript.AIstate = ElgState.Running;
        return NodeState.SUCCESS;
    }
}
