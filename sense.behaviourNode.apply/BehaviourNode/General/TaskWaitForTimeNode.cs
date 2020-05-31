using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskWaitForTimeNode : BehaviourNode
    {
        public float second;
        private float timer;

        // Update is called once per frame
        void Update()
        {
            if (State != NodeState.Running)
            {
                return;
            }
            timer += Time.deltaTime;
            if (timer>=second)
            {
                NodeToDisabledTrigger();
                State = NodeState.Succeed;
            }
        }
        public override void Execute()
        {
            NodeToEnableTrigger();

            base.Execute();
        }

        public override void ResetNode()
        {
            NodeToDisabledTrigger();
            base.ResetNode();
        }

        public override void Abort(NodeState _state)
        {
            NodeToDisabledTrigger();
            base.Abort(_state);
        }

        private void NodeToDisabledTrigger()
        {
            

        }

        private void NodeToEnableTrigger()
        {
            timer = 0;
        }
    }
}