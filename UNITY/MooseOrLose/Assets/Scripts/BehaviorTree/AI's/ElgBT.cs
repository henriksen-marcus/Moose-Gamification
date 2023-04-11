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
                new ElgMatingSeason(GetComponent<Elg>()),
                new ElgFindPartner(transform, 35),
                new ElgGoToPartner(GetComponent<NavMeshAgent>(), GetComponent<Elg>())
            }),
            new Sequence(new List<Node>{ 
                new ElgCheckForUlv(transform),
                new ElgRunAway(GetComponent<NavMeshAgent>(), transform)
            }),
            new Sequence(new List<Node>
            {
                new ElgIsWinter(),
                new ElgGoToWinterLocation(GetComponent<NavMeshAgent>(), transform)
            }),
            new Sequence(new List<Node>
            {
                new ElgIsSummer(),
                new ElgGoToSummerLocation(GetComponent<NavMeshAgent>(), transform)
            }),
            new Sequence(new List<Node>{
                new ElgHungerCheck(GetComponent<Elg>()),
                new ElgFindForest(transform),
                new ElgGoToForest(GetComponent<NavMeshAgent>()),
                new ElgTryToEat(GetComponent<Elg>())
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
