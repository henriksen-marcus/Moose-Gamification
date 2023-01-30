using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class JegerShoot : Node
{
    Transform transform;
    NavMeshAgent mAgent;
    float shootTimer;
    float shootSpeed;
    public JegerShoot(Transform t, float _shootSpeed, NavMeshAgent agent)
    {
        shootTimer = 0f;
        transform = t;
        shootSpeed = _shootSpeed;
        mAgent = agent;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") != null)
        {
            shootTimer += Time.deltaTime;

            if (shootTimer > shootSpeed)
            {
                shootTimer = 0f;
                Transform target = (Transform)parent.GetData("Target");
                parent.ClearData("Target");
                parent.SetData("Shooting", false);
                mAgent.isStopped = false;
                target.GetComponent<Elg>().Die();

            }
            return NodeState.SUCCESS;

        }
        return NodeState.FAILURE;
    }
}

