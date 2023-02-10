using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class JegerFindTarget : Node
{
    float mTargetRange;
    Transform mTransform;
    public JegerFindTarget(float range, Transform transform)
    {
        mTargetRange = range;
        mTransform = transform;
        TimeManager.instance.OnNewDay += NewDay;
    }


    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") != null)
        { 
            return NodeState.SUCCESS;
        }

        if (parent.GetData("Daily Kills") == null)
        {
            parent.SetData("Daily Kills", 0);
        }
        Collider[] colliders = Physics.OverlapSphere(mTransform.position, mTargetRange);
        
        foreach (Collider collider in colliders)
        {
            
            if (collider.tag == "Elg")
            {
                Elg mScript = collider.GetComponent<Elg>();
                if (mScript != null)
                {
                    if (mScript.age_years < 1)
                    {
                        if (mScript.mother == null)
                        {

                        }
                        else if (RuleManager.Instance.CanShootChild(mScript.mother.GetComponent<Elg>().number_of_children, (int)parent.GetData("Daily Kills")))
                        {
                            continue;
                        }
                    }
                    if (mScript.gender == Gender.Male)
                    {
                        if (!RuleManager.Instance.CanShootMale(mScript.antler_tag_number, (int)parent.GetData("Daily Kills")))
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!RuleManager.Instance.CanShootFemale(mScript.number_of_children, (int)parent.GetData("Daily Kills")))
                        {
                            continue;
                        }
                    }

                    parent.SetData("Target", collider.gameObject.transform);
                }


            }
        }
        if (parent.GetData("Target") != null)
        {
            return NodeState.SUCCESS;
        }

        return NodeState.FAILURE;
    }

    void NewDay()
    {
        parent.SetData("Daily Kills", 0);
    }

}
