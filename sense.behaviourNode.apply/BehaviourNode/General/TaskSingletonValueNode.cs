 using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    public class TaskSingletonValueNode : BehaviourNode
    {
        public enum SingletonValueType
        {
            IntensityByLight,               // 调整灯光亮度
            ShaderValueByMaterial,          // 调整Material的Shader值
            ConditionNodeIndex,             // ConditionNode 选择节点的值
            GameObjectMaterialShaderValue,  // GameObject的Material值 
        }

        public SingletonValueType type;
        public Light ctrlLight;
        public string shaderValueName;
        public Material ctrlMaterial;
        public float singletonValue;
        public float finishTime;
        public GameObject[] ctrlGameobjectArray;
        public ConditionNode conditionNode;
        public int conditionNodeIndex;

        private Sequence tweenSequence;
        private Material[] ctrlGameobjectToMaterialArray;
        public override void Execute()
        {
            base.Execute();
            TweenPlay();
        }

        private void Start()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            ctrlGameobjectToMaterialArray = new Material[ctrlGameobjectArray.Length];
            for (int i = 0; i < ctrlGameobjectToMaterialArray.Length; i++)
            {
                ctrlGameobjectToMaterialArray[i] = ctrlGameobjectArray[i].GetComponent<MeshRenderer>().material;
            }
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
                case SingletonValueType.ConditionNodeIndex:
                    conditionNode.executeNodeIndex = conditionNodeIndex;
                    State = NodeState.Succeed;
                    break;
                case SingletonValueType.GameObjectMaterialShaderValue:
                    if (ctrlGameobjectToMaterialArray.Length == 0)
                    {
                        State = NodeState.Succeed;
                        return;
                    }

                    Material m1 = ctrlGameobjectToMaterialArray[0];
                    tweenSequence.Append(DOTween.To(() => m1.GetFloat(shaderValueName), x => m1.SetFloat(shaderValueName, x),
                        singletonValue, finishTime));
                    for (int i = 1; i < ctrlGameobjectToMaterialArray.Length; i++)
                    {
                        Material m = ctrlGameobjectToMaterialArray[i];
                        tweenSequence.Join(DOTween.To(() => m.GetFloat(shaderValueName), x => m.SetFloat(shaderValueName, x),
                            singletonValue, finishTime));
                    }
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

