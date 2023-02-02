using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;


public class UlvEatElg : Node
{
    Ulv mScript;
    public UlvEatElg(Ulv script)
    {
        mScript = script;
    }
    public override NodeState Evaluate()
    {
        Transform transform = (Transform)parent.GetData("Target");

        // Delete Elg
        transform.GetComponent<Elg>().Die();
        parent.ClearData("Target");


        // Fill Hunger
        for (int i = 0; i < mScript.pack.Count; i++)
        {
            mScript.pack[i].GetComponent<Ulv>().hunger = 100;
        }
        return NodeState.SUCCESS;
    }
}
