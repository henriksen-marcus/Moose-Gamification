using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class ElgIsWinter : Node
{
    public override NodeState Evaluate()
    {
        if (TimeManager.instance.IsWinter())
        {
            Debug.Log("isWinter");
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
