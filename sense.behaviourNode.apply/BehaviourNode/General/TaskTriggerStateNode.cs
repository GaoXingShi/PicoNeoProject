using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskTriggerStateNode : BehaviourNode
    {
        public TriggerBehaviour[] triggerBehaviourArray;
        public override void Execute()
        {
            foreach (var v in triggerBehaviourArray)
            {
                v.EnableTrigger();
            }
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

        private void FixedUpdate()
        {
            if (State != NodeState.Running) return;
            foreach (var v in triggerBehaviourArray)
            {
                if (v.IsRunning)
                {
                    return;
                }
            }

            State = NodeState.Succeed;
            foreach (var v in triggerBehaviourArray)
            {
                v.DisableTrigger();
            }
        }
    }
}
