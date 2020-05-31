using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskParentNode : BehaviourNode
    {
        public Transform parent;
        public Transform ctrlTsm;
        public override void Execute()
        {
            if (ctrlTsm != null)
            {
                if (parent == null)
                {
                    ctrlTsm.parent = null;
                }
                else
                {
                    ctrlTsm.parent = parent;
                }

            }

            State = NodeState.Succeed;
        }

        public override void ResetNode()
        {
            base.ResetNode();
        }

        public override void Abort(NodeState _state)
        {
            base.Abort(_state);
        }
    }
}
