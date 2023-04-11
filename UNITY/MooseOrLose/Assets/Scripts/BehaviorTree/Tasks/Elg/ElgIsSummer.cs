using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
public class ElgIsSummer : Node
{
    public override NodeState Evaluate()
    {
        if (!TimeManager.instance.IsWinter())
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }
}
