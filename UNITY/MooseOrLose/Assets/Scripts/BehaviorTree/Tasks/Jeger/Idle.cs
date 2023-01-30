using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class Idle : Node
{
    public Idle()
    {

    }
    public override NodeState Evaluate()
    {
        return NodeState.SUCCESS;
    }
}
