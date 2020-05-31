using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskActiveObjectNode : BehaviourNode
    {
        public bool objectsActive;
        public GameObject[] objects;
        public override void Execute()
        {
            foreach (var v in objects)
            {
                v.SetActive(objectsActive);
            }
     
            State = NodeState.Succeed;
        }

        public override void ResetNode()
        {
            base.ResetNode();
        }

        public override void Abort(NodeState _state)
        {
            if (State == NodeState.Running && _state == NodeState.Succeed)
            {
                foreach (var v in objects)
                {
                    v.SetActive(objectsActive);
                }
            }

            if(State == NodeState.Ready && _state == NodeState.Succeed)
            {
                foreach (var v in objects)
                {
                    v.SetActive(objectsActive);
                }
            }


            base.Abort(_state);
        }
    }
}