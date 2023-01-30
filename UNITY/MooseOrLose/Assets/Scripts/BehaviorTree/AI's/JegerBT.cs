using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;
using UnityEngine.AI;

public class JegerBT : BehaviorTrees.BehaviorTree
{
    public Vector3 homePosition;
    public float range;
    public float shotSpeed;
    protected override Node SetupTree()
    {
        Node root = new Selector(new List<Node>
        {
            // List of Nodes
            new Sequence(new List<Node> {
                new CheckForHuntingSeason(),
                new JegerFindTarget(range, transform),
                new JegerWalkToTarget(GetComponent<NavMeshAgent>()),
                new JegerShoot(transform, shotSpeed, GetComponent<NavMeshAgent>())
            }),
            new Sequence(new List<Node> {
                new CheckForHuntingSeason(),
                new JegerWalk(GetComponent<NavMeshAgent>())
            }),
            new Sequence(new List<Node> {
                new JegerNotHome(transform, homePosition),
                new JegerWalkHome(GetComponent<NavMeshAgent>())
            }),
            new Idle()

        });

        return root;
    }
}
