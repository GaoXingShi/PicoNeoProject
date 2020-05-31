using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskUIButtonEventNode : BehaviourNode
    {
        public Button listenButton;

        void Start()
        {
            listenButton.onClick.AddListener(ButtonEvent);
        }

        private void ButtonEvent()
        {
            if (State == NodeState.Running)
            {
                State = NodeState.Succeed;
            }
        }

        public override void ResetNode()
        {
            base.ResetNode();
        }

        public override void Execute()
        {
            base.Execute();
        }

        public override void Abort(NodeState _state)
        {
            base.Abort(_state);
        }

    }
}
