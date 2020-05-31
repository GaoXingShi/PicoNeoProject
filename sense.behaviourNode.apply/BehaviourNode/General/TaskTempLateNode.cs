using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskTempLateNode : BehaviourNode
    {
        public override void Execute()
        {
            base.Execute();
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
