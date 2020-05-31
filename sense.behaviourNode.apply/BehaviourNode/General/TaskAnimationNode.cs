using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskAnimationNode : BehaviourNode
    {
        public Animator anim;
        public string animName;
        public bool loop;

        public override void Execute()
        {
            anim.enabled = true;
            anim.Play(animName, 0, 0);
            if (loop)
            {
                State = NodeState.Succeed;
            }
            else
            {
                base.Execute();
            }
        }

        public override void ResetNode()
        {
            base.ResetNode();
        }

        public override void Abort(NodeState _state)
        {
            anim.Play(animName, 0, 0);
            base.Abort(_state);
        }

        void Update()
        {

            if (State != NodeState.Running)
            {
                return;
            }

            if (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1.0f && anim.GetCurrentAnimatorStateInfo(0).IsName(animName))
            {
                State = NodeState.Succeed;
                if (!loop)
                {
                    anim.enabled = false;
                }

            }
        }
    }
}
