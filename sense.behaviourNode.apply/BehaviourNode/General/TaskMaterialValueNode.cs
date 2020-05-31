using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskMaterialValueNode : BehaviourNode
    {
        public Material ctrlMaterial;
        public string shaderValueName;
        public Color color;
        public override void Execute()
        {
            ctrlMaterial.SetColor(shaderValueName, color);
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