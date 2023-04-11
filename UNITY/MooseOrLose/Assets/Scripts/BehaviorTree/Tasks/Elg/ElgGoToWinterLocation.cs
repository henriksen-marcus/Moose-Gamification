using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class ElgGoToWinterLocation : Node
{
    NavMeshAgent mAgent;
    Transform mTransform;
    Elg elg;
    float walkSpeed;
    float acceleration;
    public ElgGoToWinterLocation(NavMeshAgent agent, Transform transform)
    {
        mAgent = agent;
        mTransform = transform;
        walkSpeed = mAgent.speed;
        acceleration = mAgent.acceleration;
        elg = transform.GetComponent<Elg>();

    }
    public override NodeState Evaluate()
    {
        if (Vector3.Distance(mTransform.position, elg.GetWinterLocation()) < 10 && mTransform.position.y !>= -1f)
        {
            
            return NodeState.FAILURE;
        }
        float ratio = (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = walkSpeed * ratio;
        mAgent.acceleration = acceleration * ratio;

        elg.AIstate = ElgState.Walking;
        NavMeshHit hit;
        NavMesh.SamplePosition(elg.GetWinterLocation(), out hit, 200, 1);
        if (!hit.hit) return NodeState.FAILURE;

        mAgent.SetDestination(hit.position);

        if (Vector3.Distance(mTransform.position, elg.GetWinterLocation()) > 1)
        {
            
            return NodeState.RUNNING;
        }

        // Finish this sequence whenever we get "home". Failure because its in a sequence under a selector
        
        return NodeState.FAILURE;
    }
}
