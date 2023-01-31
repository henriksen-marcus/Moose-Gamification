using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;


public class UlvFindTarget : Node
{
    float mTargetRange;
    Transform mTransform;
    public UlvFindTarget(float range, Transform transform)
    {
        mTargetRange = range;
        mTransform = transform;
    }
    public override NodeState Evaluate()
    {
        if (!mTransform.GetComponent<Ulv>().isLeader)
        {
            return NodeState.FAILURE;
        }

        if (parent.GetData("Target") != null)
        {
            return NodeState.SUCCESS;
        }


        Collider[] colliders = Physics.OverlapSphere(mTransform.position, mTargetRange);
        float smallest = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Elg")
            {
                float weight = collider.gameObject.GetComponent<Elg>().weight;
                if (smallest > weight)
                { 
                    smallest = weight;
                    parent.SetData("Target", collider.gameObject.transform);
                    return NodeState.SUCCESS;
                }
            }
        }

        return NodeState.FAILURE;

    }
}
