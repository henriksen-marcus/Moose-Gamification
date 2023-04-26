using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class ElgGoToSummerLocation : Node
{
    NavMeshAgent mAgent;
    Transform mTransform;
    Elg elg;
    float walkSpeed;
    float acceleration;
    public ElgGoToSummerLocation(NavMeshAgent agent, Transform transform)
    {
        mAgent = agent;
        mTransform = transform;
        walkSpeed = mAgent.speed;
        acceleration = mAgent.acceleration;
        elg = transform.GetComponent<Elg>();

    }
    public override NodeState Evaluate()
    {
        if (Vector3.Distance(mTransform.position, elg.GetSummerLocation()) < 20)
        {
            return NodeState.FAILURE;
        }
        float ratio = (TimeManager.Instance.defaultPlaySpeed / TimeManager.Instance.playSpeed);
        mAgent.speed = walkSpeed * ratio;
        mAgent.acceleration = acceleration * ratio;

        elg.AIstate = ElgState.Walking;
        NavMeshHit hit;
        NavMesh.SamplePosition(elg.GetSummerLocation(), out hit, 200, 1);
        if (!hit.hit) return NodeState.FAILURE;

        mAgent.SetDestination(hit.position);

        if (Vector3.Distance(mTransform.position, elg.GetSummerLocation()) > 20)
        {
            return NodeState.RUNNING;
        }

        // Finish this sequence whenever we get "home". Failure because its in a sequence under a selector
        return NodeState.FAILURE;
    }
}
