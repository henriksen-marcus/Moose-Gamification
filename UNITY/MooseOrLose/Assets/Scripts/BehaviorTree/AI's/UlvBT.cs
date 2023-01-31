using System.Collections.Generic;
using UnityEngine;
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
            new Sequence(new List<Node>
            {
                new UlvFollowPackLeader(GetComponent<NavMeshAgent>()),
                new WalkRandom(GetComponent<NavMeshAgent>(), transform, 40)
            })
        });

        return root;
    }
}
