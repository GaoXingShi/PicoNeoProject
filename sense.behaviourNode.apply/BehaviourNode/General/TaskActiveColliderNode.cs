using UnityEngine;
using System.Collections;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskActiveColliderNode : BehaviourNode
    {
        public bool objectsActive;
        public GameObject[] objects;
        public override void Execute()
        {
            foreach (var t in objects)
            {
                if(t.GetComponent<Collider>())
                {
                    foreach (var v in t.GetComponents<Collider>())
                    {
                        v.enabled = objectsActive;
                    }
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
            for (int i = 0; i < objects.Length; i++)
            {
                if (objects[i].GetComponent<Collider>())
                {
                    foreach (var v in objects[i].GetComponents<Collider>())
                    {
                        v.enabled = objectsActive;
                    }
                }
            }
            base.Abort(_state);
        }
    }
}
