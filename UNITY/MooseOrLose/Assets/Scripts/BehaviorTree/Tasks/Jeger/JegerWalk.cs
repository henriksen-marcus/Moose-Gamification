using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class JegerWalk : Node
{
    NavMeshAgent mAgent;
    float walkSpeed;
    float timer;
    public JegerWalk(NavMeshAgent agent)
    {
        mAgent = agent;
        walkSpeed = agent.speed;
        timer = 11f;
    }
    public override NodeState Evaluate()
    {

        timer += Time.deltaTime;
        
        if (GetData("Destination") != null)
        {
            if (Vector3.Distance(mAgent.transform.position, (Vector3)GetData("Destination")) > 10)
            {
                mAgent.SetDestination((Vector3)GetData("Destination"));
                return NodeState.RUNNING;
            }
            else
            {
                ClearData("Destination");
            }
            
        }

        
        float speed = walkSpeed * (TimeManager.instance.startPlaySpeed / TimeManager.instance.playSpeed);
        mAgent.speed = speed;

        NavMeshHit hit;
        NavMesh.SamplePosition(new Vector3(Random.Range(-200, 200), 10, Random.Range(-200, 200)), out hit, 200, 1);
        mAgent.SetDestination(hit.position);
        SetData("Destination", hit.position);
        timer = 0f;
        return NodeState.RUNNING;
    }
}
