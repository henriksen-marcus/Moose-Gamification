using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace BehaviorTrees
{
    public abstract class BehaviorTree : MonoBehaviour
    {
        public Node Root = null;

        protected void Start()
        {
            Root = SetupTree();
        }

        private void Update()
        {

            if (Root != null)
            {
                Root.Evaluate();
            }
        }


        protected abstract Node SetupTree();



    }




}

