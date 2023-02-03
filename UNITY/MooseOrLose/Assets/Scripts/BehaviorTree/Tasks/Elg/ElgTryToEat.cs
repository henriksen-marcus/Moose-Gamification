using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;


public class ElgTryToEat : Node
{
    Elg mScript;
    public ElgTryToEat(Elg script)
    {
        mScript = script;
    }
    public override NodeState Evaluate()
    {
        if ((Forest)parent.GetData("Forest") == null)
        {
            parent.ClearData("Forest");
            return NodeState.FAILURE;
        }
        float hunger = 70 - mScript.hunger;
        float n = 4;
        // n is how much the moose eat from each tree (in kg)
        for (float i = 0; i < hunger; i += n)
        {
            Forest forest = (Forest)parent.GetData("Forest");
            Forest.Trees tree = forest.GetOptimalTreeToEat();
            if (tree == null)
            {
                return NodeState.FAILURE;
            }
            mScript.hunger += n;
        }

        return NodeState.SUCCESS;
    }
}