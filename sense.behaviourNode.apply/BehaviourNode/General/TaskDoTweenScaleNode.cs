using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskDoTweenScaleNode : BehaviourNode
    {
        public Transform scaleTransform;
        public Vector3 mScale;
        public float tweenFinishTime = 1;
        private Sequence mSequence;
        private bool isFinish;

        private void Update()
        {
            if (State == NodeState.Running && isFinish)
            {
                State = NodeState.Succeed;
                isFinish = false;
            }
        }

        public override void Execute()
        {
            mSequence = DOTween.Sequence();
            mSequence.Append(DOTween.To(() => scaleTransform.localScale, x => { scaleTransform.localScale = x; }, mScale, tweenFinishTime));
            mSequence.AppendCallback(() =>
            {
                isFinish = true;
            });
            base.Execute();
        }

        public override void ResetNode()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            base.ResetNode();
        }

        public override void Abort(NodeState _state)
        {
            if (State == NodeState.Ready && _state == NodeState.Succeed)
            {
                scaleTransform.localPosition = mScale;
            }
            base.Abort(_state);
        }
        
    }
}
