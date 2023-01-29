using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;


public class ElgCheckForUlv : Node
{
    Transform mTransform;

    public ElgCheckForUlv(Transform transform)
    {
        mTransform = transform;
    }
    public override NodeState Evaluate()
    {
        Collider[] colliders = Physics.OverlapSphere(mTransform.position, 20);
        bool found = false;
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Ulv")
            {
                found = true;

                parent.SetData("Danger", collider.gameObject.transform);
                return NodeState.SUCCESS;
            }
        }

        if (!found)
        {
            parent.ClearData("Danger");
        }
        
        if (parent.GetData("Danger") != null)
        {
            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
