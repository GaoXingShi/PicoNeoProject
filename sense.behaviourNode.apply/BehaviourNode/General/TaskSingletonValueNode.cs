﻿using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskSingletonValueNode : BehaviourNode
    {
        public enum SingletonValueType
        {
            IntensityByLight,ShaderValueByMaterial
        }

        public SingletonValueType type;
        public Light ctrlLight;
        public string shaderValueName;
        public Material ctrlMaterial;
        public float singletonValue;
        public float finishTime;

        private Sequence tweenSequence;

        public override void Execute()
        {
            TweenPlay();
            base.Execute();
        }

        private void Start()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        }
        private void TweenPlay()
        {
            tweenSequence = DOTween.Sequence();
            switch (type)
            {
                case SingletonValueType.IntensityByLight:
                    tweenSequence.Append(DOTween.To(() => ctrlLight.intensity, x => ctrlLight.intensity = x,
                        singletonValue, finishTime));
                    tweenSequence.AppendCallback(() => { State = NodeState.Succeed; });
                    break;
                case SingletonValueType.ShaderValueByMaterial:
                    tweenSequence.Append(DOTween.To(() => ctrlMaterial.GetFloat(shaderValueName), x => ctrlMaterial.SetFloat(shaderValueName,x),
                        singletonValue, finishTime));
                    tweenSequence.AppendCallback(() => { State = NodeState.Succeed; });
                    break;
            }
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

