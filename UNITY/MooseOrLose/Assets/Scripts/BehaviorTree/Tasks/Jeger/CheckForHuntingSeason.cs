using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class CheckForHuntingSeason : Node
{
    public CheckForHuntingSeason()
    {

    }
    public override NodeState Evaluate()
    {
        if (TimeManager.instance.HuntingSeason())
        {

            return NodeState.SUCCESS;
        }
        return NodeState.FAILURE;
    }
}
