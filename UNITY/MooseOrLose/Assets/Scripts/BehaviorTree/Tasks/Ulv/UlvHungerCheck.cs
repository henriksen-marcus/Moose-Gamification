using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;


public class UlvHungerCheck : Node
{
    Ulv mScript;
    public UlvHungerCheck(Ulv script)
    {
        mScript = script;
    }
    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") != null)
        {
            return NodeState.SUCCESS;
        }

        if (mScript != null)
        {
            if (mScript.hunger < 25)
            {
                return NodeState.SUCCESS;
            }
        }
        return NodeState.FAILURE;

    }
}
