using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;
public class ElgGoToPartner : Node
{
    NavMeshAgent mAgent;
    Elg mScript;
    float walkSpeed;
    float acceleration;
    public ElgGoToPartner(NavMeshAgent agent, Elg script)
    {
        mAgent = agent;
        walkSpeed = agent.speed;
        acceleration = agent.acceleration;
        mScript = script;
    }
    public override NodeState Evaluate()
    {
        // Failure Check 1
        if (parent.GetData("Partner") == null)
        {
            return NodeState.FAILURE;
        }
        float ratio = (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = walkSpeed * ratio;
        mAgent.acceleration = acceleration * ratio;

        // Failure Check 2
        if ((Transform)parent.GetData("Partner") == null)
        {
            parent.ClearData("Partner");
            return NodeState.FAILURE;
        }


        Transform transform = (Transform)parent.GetData("Partner");
        mAgent.SetDestination(transform.position);

        if (Vector3.Distance(transform.position, mAgent.transform.position) < 2f)
        {
            transform.GetComponent<Elg>().hasPartner = false;
            mScript.pregnate(transform.GetComponent<Elg>());
            parent.ClearData("Partner");
            return NodeState.SUCCESS;
        }

        return NodeState.RUNNING;
    }
}
