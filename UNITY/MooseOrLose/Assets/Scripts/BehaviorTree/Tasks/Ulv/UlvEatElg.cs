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
        Object.Destroy(transform.gameObject);
        parent.ClearData("Target");
        ElgManager.instance.DecreasePopulation();


        // Fill Hunger
        mScript.hunger = 100;
        return NodeState.SUCCESS;
    }
}
