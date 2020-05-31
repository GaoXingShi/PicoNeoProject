using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskDoTweenPositionNode : BehaviourNode
    {
        public Vector3 mPosV3;
        public float tweenFinishTime = 1;
        public bool setRelative = false;
        public Transform posTransform;
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
            mSequence.SetRelative(setRelative);
            mSequence.Append(DOTween.To(() => posTransform.localPosition, x => { posTransform.localPosition = x; }, mPosV3, tweenFinishTime));
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
                if(setRelative)
                {
                    posTransform.localPosition = mPosV3;
                }
                else
                {
                    posTransform.localPosition = posTransform.localPosition + mPosV3;
                }
            }
            base.Abort(_state);
        }
    }
}
