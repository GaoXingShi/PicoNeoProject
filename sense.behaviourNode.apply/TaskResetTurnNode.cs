using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskResetTurnNode : BehaviourNode
    {
        public bool isReset = true;
        public bool isStopTrigger = false;
        public int value;
        public TreeNodeController treeNodeCtrl;
        public override void Execute()
        {
            if (isReset)
                StartCoroutine(treeNodeCtrl.ResetTurn(value));
            if (isStopTrigger)
            {
                treeNodeCtrl.StopAllCubeObserver(value);
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

