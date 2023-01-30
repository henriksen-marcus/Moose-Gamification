using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class JegerFindTarget : Node
{
    float mTargetRange;
    Transform mTransform;
    public JegerFindTarget(float range, Transform transform)
    {
        mTargetRange = range;
        mTransform = transform;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") != null)
        {

            return NodeState.SUCCESS;
        }



        Collider[] colliders = Physics.OverlapSphere(mTransform.position, mTargetRange);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Elg")
            {
                // Code Choosing of Elg Here

                if (parent.GetData("Target") == null)
                    parent.SetData("Target", collider.gameObject.transform);
                return NodeState.SUCCESS;
            }
        }

        return NodeState.FAILURE;
    }
}
