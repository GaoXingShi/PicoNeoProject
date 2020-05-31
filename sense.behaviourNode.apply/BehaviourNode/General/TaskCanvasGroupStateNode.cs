using System.Collections;
using System.Collections.Generic;
using Sense.BehaviourTree;
using UnityEngine;

namespace sense.networking.Apply
{
    public class TaskCanvasGroupStateNode : BehaviourNode
    {
        public bool isOpen;
        public CanvasGroup ctrlCanvasGroup;

        public override void ResetNode()
        {
            base.ResetNode();
        }

        public override void Execute()
        {
            ctrlCanvasGroup.alpha = isOpen ? 1 : 0;
            ctrlCanvasGroup.blocksRaycasts = isOpen;
            ctrlCanvasGroup.interactable = isOpen;

            State = NodeState.Succeed;
        }

        
    }
}
