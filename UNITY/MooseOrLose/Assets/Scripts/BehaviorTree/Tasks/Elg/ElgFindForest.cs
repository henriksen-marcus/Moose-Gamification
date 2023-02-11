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

        float bestForest = float.MaxValue;

        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Forest")
            {
                float forestValue = 1;
                if (collider.GetComponent<Forest>().GetOptimalTreeToEat() != null)
                {
                    if (collider.GetComponent<Forest>().forestState_Type == ForestState_Type.forestType_Birch)
                    {
                        forestValue *= 0.5f;
                    }
                    if (collider.GetComponent<Forest>().GetOptimalTreeToEat().treeState_Health != TreeState_Health.treeHealth_Healthy)
                    {
                        forestValue *= 0.25f;
                    }
                    float distance = Vector3.Distance(collider.transform.position, mTransform.position);
                    float forest = distance / forestValue;
                    if (bestForest > forest)
                    {
                        bestForest = forest;
                        parent.SetData("Forest", collider.GetComponent<Forest>());

                    }
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