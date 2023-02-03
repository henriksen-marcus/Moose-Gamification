using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;


public class ElgFindForest : Node
{
    Transform mTransform;
    public ElgFindForest(Transform transform)
    {
        mTransform = transform;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Forest") != null)
        {
            return NodeState.SUCCESS;
        }


        Collider[] colliders = Physics.OverlapSphere(mTransform.position, 20);
        float closest = float.MaxValue;


        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Forest")
            {
                float distance = Vector3.Distance(collider.transform.position, mTransform.position);
                if (closest > distance)
                {
                    closest = distance;
                    parent.SetData("Forest", collider.GetComponent<Forest>());

                }
            }
        }
        if (parent.GetData("Forest") != null)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}