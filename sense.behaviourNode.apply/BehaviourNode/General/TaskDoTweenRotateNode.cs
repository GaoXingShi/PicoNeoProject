using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskDoTweenRotateNode : BehaviourNode
    {
        public enum TweenRotateState
        {
            localRotation,
            localEulerAngle,
            eulerAngle
        }

        public Transform eulerTransform;
        public Vector3 mEurlerV3;
        public float tweenFinishTime = 1;
        public bool setRelative = false;
        public TweenRotateState rotateState = TweenRotateState.localRotation;

        private Sequence mSequence;
        private bool isFinish;

        private void Update()
        {
            if(State == NodeState.Running && isFinish)
            {
                State = NodeState.Succeed;
                isFinish = false;
            }
        }

        public override void Execute()
        {
            mSequence = DOTween.Sequence();
            mSequence.SetRelative(setRelative);
            switch (rotateState)
            {
                case TweenRotateState.localRotation:
                    mSequence.Append(DOTween.To(() => eulerTransform.localRotation, x => { eulerTransform.localRotation = x; }, mEurlerV3, tweenFinishTime));

                    break;

                case TweenRotateState.localEulerAngle:
                    mSequence.Append(DOTween.To(() => eulerTransform.localEulerAngles, x => { eulerTransform.localEulerAngles = x; }, mEurlerV3, tweenFinishTime));

                    break;

                case TweenRotateState.eulerAngle:
                    mSequence.Append(DOTween.To(() => eulerTransform.eulerAngles, x => { eulerTransform.eulerAngles = x; }, mEurlerV3, tweenFinishTime));

                    break;
            }


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
                if (setRelative)
                {
                    eulerTransform.localEulerAngles = mEurlerV3;
                }
                else
                {
                    eulerTransform.localEulerAngles = eulerTransform.localEulerAngles + mEurlerV3;
                }
            }
            base.Abort(_state);
        }
    }
}
