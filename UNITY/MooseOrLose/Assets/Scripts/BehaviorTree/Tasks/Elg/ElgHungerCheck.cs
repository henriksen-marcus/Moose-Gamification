using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;


public class ElgHungerCheck : Node
{
    Elg mScript;
    public ElgHungerCheck(Elg script)
    {
        mScript = script;
    }
    public override NodeState Evaluate()
    {
        if (mScript == null)
        {
            return NodeState.FAILURE;
        }

        if (mScript.hunger < 70)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}