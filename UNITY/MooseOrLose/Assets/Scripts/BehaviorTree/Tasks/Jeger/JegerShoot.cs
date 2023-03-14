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
            shootSpeed = TimeManager.instance.playSpeed;
            shootTimer += Time.deltaTime;

            if (shootTimer > shootSpeed)
            {
                shootTimer = 0f;
                Transform target = (Transform)parent.GetData("Target");
                if (target == null)
                {
                    parent.ClearData("Target");
                    parent.SetData("Shooting", false);
                    mAgent.isStopped = false;
                    return NodeState.FAILURE;
                }
                if (target.GetComponent<Elg>().gender == Gender.Male)
                {
                    JegerManager.instance.ShotMale();
                }
                else
                {
                    JegerManager.instance.ShotFemale();
                }
                parent.ClearData("Target");
                parent.SetData("Shooting", false);
                mAgent.isStopped = false;
                target.GetComponent<Elg>().Die();
                JegerManager.instance.shotThisMonth++;
                if (parent.GetData("Weekly Kills") == null)
                {                   
                    parent.SetData("Weekly Kills", 1);
                }
                else
                {
                    int num = (int)parent.GetData("Weekly Kills");
                    num++;
                    parent.SetData("Weekly Kills", num);
                }
                
            }
            return NodeState.SUCCESS;

        }
        return NodeState.FAILURE;
    }
}

