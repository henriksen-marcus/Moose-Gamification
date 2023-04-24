using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorTrees;

public class JegerFindTarget : Node
{
    float mTargetRange;
    Transform mTransform;
    int days;
    public JegerFindTarget(float range, Transform transform)
    {
        mTargetRange = range;
        mTransform = transform;
        TimeManager.Instance.OnNewDay += NewDay;
    }


    public override NodeState Evaluate()
    {
        if (parent.GetData("Target") != null)
        { 
            return NodeState.SUCCESS;
        }

        if (parent.GetData("Weekly Kills") == null)
        {
            parent.SetData("Weekly Kills", 0);
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
                        else if (!RuleManager.Instance.CanShootChild(mScript.mother.GetComponent<Elg>().number_of_children, (int)parent.GetData("Weekly Kills")))
                        {
                            continue;
                        }
                    }
                    if (mScript.gender == Gender.Male)
                    {
                        if (!RuleManager.Instance.CanShootMale(mScript.antler_tag_number, (int)parent.GetData("Weekly Kills")))
                        {
                            continue;
                        }
                        if (!JegerManager.instance.canShootMale())
                        {
                            continue;
                        }
                    }
                    else
                    {
                        if (!RuleManager.Instance.CanShootFemale(mScript.number_of_children, (int)parent.GetData("Weekly Kills")))
                        {
                            continue;
                        }
                        if (!JegerManager.instance.canShootFemale())
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
        days++;
        if (days > 6)
        {
            days = 0;
            parent.SetData("Weekly Kills", 0);
        }
        
    }

}
