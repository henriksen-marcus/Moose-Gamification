using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
public class ElgFindPartner : Node
{
    Transform mTransform;
    float mViewRange;
    public ElgFindPartner(Transform transform, float range)
    {
        mTransform = transform;
        mViewRange = range;
    }

    public override NodeState Evaluate()
    {

        if (parent.GetData("Partner") != null)
        {
            return NodeState.SUCCESS;
        }


        Collider[] colliders = Physics.OverlapSphere(mTransform.position, mViewRange);


        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Elg")
            {
                Elg script = collider.GetComponent<Elg>();
                if (script.gender == Gender.Male && script.GetAge() > 3.0f)
                {
                    if (!script.hasPartner)
                    {
                        script.hasPartner = true;
                        parent.SetData("Partner", collider.gameObject.transform);
                        return NodeState.SUCCESS;
                    }

                }
            }
        }

        return NodeState.FAILURE;

    }  

}
