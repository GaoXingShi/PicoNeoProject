using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    /// <summary>
    /// 更改物体的Material
    /// </summary>
    public class TaskChangeMaterialNode : BehaviourNode
    {
        public Transform[] changeMaterial0Transforms;
        public Material changeMaterial;
        public override void Execute()
        {
            foreach (var v in changeMaterial0Transforms.Where(v => v.GetComponent<Renderer>()))
            {
                v.GetComponent<Renderer>().material = changeMaterial;
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
                foreach (var v in changeMaterial0Transforms.Where(v => v.GetComponent<Renderer>()))
                {
                    v.GetComponent<Renderer>().material = changeMaterial;
                }
            }

            if (State == NodeState.Ready && _state == NodeState.Succeed)
            {
                foreach (var v in changeMaterial0Transforms.Where(v => v.GetComponent<Renderer>()))
                {
                    v.GetComponent<Renderer>().material = changeMaterial;
                }
            }
            base.Abort(_state);
        }
    }


}
