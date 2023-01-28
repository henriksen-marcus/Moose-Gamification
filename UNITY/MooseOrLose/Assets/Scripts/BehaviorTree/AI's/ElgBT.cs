using System.Collections.Generic;
using BehaviorTrees;
using UnityEngine.AI;

public class ElgBT : BehaviorTrees.BehaviorTree
{

    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            // List of Nodes
            new WalkRandom(GetComponent<NavMeshAgent>(), transform, GetComponent<Elg>())
        });

        return root;
    }
}
