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
            
            new Sequence(new List<Node>{ 
                new ElgCheckForUlv(transform),
                new ElgRunAway(GetComponent<NavMeshAgent>(), transform)
            }),
            new Selector(new List<Node>
            {
                new FollowMother(GetComponent<NavMeshAgent>(), transform, GetComponent<Elg>()),
                new WalkRandom(GetComponent<NavMeshAgent>(), transform, 15)
            })

        });

        return root;
    }
}
