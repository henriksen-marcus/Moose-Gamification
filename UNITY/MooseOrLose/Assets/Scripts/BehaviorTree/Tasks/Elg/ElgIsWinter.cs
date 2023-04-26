using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class ElgIsWinter : Node
{
    public override NodeState Evaluate()
    {
        if (TimeManager.Instance.IsWinter())
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
