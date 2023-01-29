using System.Collections.Generic;
using BehaviorTrees;
using UnityEngine.AI;

public class UlvBT : BehaviorTrees.BehaviorTree
{
    public float range = 10;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            new Sequence(new List<Node>
            {
                new UlvHungerCheck(GetComponent<Ulv>()),
                new UlvFindTarget(range, transform),
                new UlvFollowTarget(GetComponent<NavMeshAgent>()),
                new UlvEatElg(GetComponent<Ulv>())
            }),
            // List of Nodes
            new WalkRandom(GetComponent<NavMeshAgent>(), transform)

        });

        return root;
    }
}
