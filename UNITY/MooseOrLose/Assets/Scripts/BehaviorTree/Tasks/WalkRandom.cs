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
    float walkSpeed;
    float acceleration;
    float timeToWait;


    public WalkRandom(NavMeshAgent agent, Transform transform, float _walkDistance)
    {
        mAgent = agent;
        mTransform = transform;
        walkDistance = _walkDistance;
        walkSpeed = mAgent.speed;
        acceleration = mAgent.acceleration;
        timeToWait = 5f;
    }

    public override NodeState Evaluate()
    {
        timer += Time.deltaTime;
        float ratio = (TimeManager.instance.defaultPlaySpeed / TimeManager.instance.playSpeed);
        timeToWait = TimeManager.instance.playSpeed / 2;
        float speed = walkSpeed * ratio;
        mAgent.speed = speed;
        mAgent.acceleration = acceleration * ratio;
        if (mAgent.GetComponent<Elg>() != null)
        {
            if (mAgent.GetComponent<Elg>().age_years < 1)
            {
                return NodeState.FAILURE;
            }
            mAgent.GetComponent<Elg>().AIstate = ElgState.Walking;
            mAgent.speed *= ((100 + mAgent.GetComponent<Elg>().weight) / 500);
            mAgent.acceleration *= ((100 + mAgent.GetComponent<Elg>().weight) / 500);
            if (mAgent.GetComponent<Elg>().age_years < 1)
            {
                return NodeState.FAILURE;
            }
            
        }
        if (timer > timeToWait)
        {
            timer = 0f;
            NavMeshHit hit;
            int attempts = 0;
            do
            {
                attempts++;
                NavMesh.SamplePosition(mTransform.position + new Vector3(Random.Range(-walkDistance, walkDistance), 0, Random.Range(-walkDistance, walkDistance)), out hit, 200, 1);
                mAgent.SetDestination(hit.position);
            } while (!hit.hit && attempts < 10);
            
            return NodeState.RUNNING;
        }


        return NodeState.FAILURE;

    }
}
