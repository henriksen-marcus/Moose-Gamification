using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class JegerNotHome : Node
{
    Transform transform;
    Vector3 home;
    public JegerNotHome(Transform t, Vector3 _home)
    {
        transform = t;
        home = _home;
    }
    public override NodeState Evaluate()
    {
        if (transform.position.x > home.x - 5f && transform.position.x < home.x + 5f)
        {
            if (transform.position.z > home.y - 5f && transform.position.z < home.y + 5f)
            {
                return NodeState.FAILURE;
            }
        }

        return NodeState.SUCCESS;
    }
}
