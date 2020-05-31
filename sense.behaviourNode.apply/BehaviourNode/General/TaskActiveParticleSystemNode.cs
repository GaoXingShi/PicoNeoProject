using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskActiveParticleSystemNode : BehaviourNode
    {
        public bool objectsActive;
        public ParticleSystem[] objects;
        public override void Execute()
        {
            foreach (var v in objects)
            {
                if (objectsActive)
                {
                    v.Play(true);
                }
                else
                {
                    v.Stop(true);
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
            foreach (var v in objects)
            {
                if (objectsActive)
                {
                    v.Play(true);
                }
                else
                {
                    v.Stop(true);
                }
            }

            base.Abort(_state);
        }
    }

}