using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskSceneNode : BehaviourNode
    {
        public string sceneName;
        public override void Execute()
        {
            State = NodeState.Succeed;
            SceneManager.LoadScene(sceneName);
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
