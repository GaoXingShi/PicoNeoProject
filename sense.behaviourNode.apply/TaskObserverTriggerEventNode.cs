using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sense.BehaviourTree;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using VRTK;

namespace sense.networking.Apply
{

    public class TaskObserverTriggerEventNode : BehaviourNode
    {
        public ClickAndTweenTrigger[] allCheckTweenTrigger;
        public bool[] allCheckTweenLoopValue;
        private bool isStartEvent, isWaitHandShank;


        private HighlightingSystem.Highlighter high;
        // Start is called before the first frame update
        void Start()
        {
            high = GetComponent<HighlightingSystem.Highlighter>();
        }

        // Update is called once per frame
        void Update()
        {
            if (State != NodeState.Running)
            {
                return;
            }

            for (int i = 0; i < allCheckTweenTrigger.Length; i++)
            {
                if (allCheckTweenTrigger[i].optionType == ResultOptionType.Time)
                {
                    if (allCheckTweenTrigger[i].isTimeFinish != allCheckTweenLoopValue[i])
                        return;
                }
                else
                {
                    if (allCheckTweenTrigger[i].GetLoopHeadValue != allCheckTweenLoopValue[i])
                        return;
                }
            }

            OnFinishNode();
            State = NodeState.Succeed;

        }

        public override void ResetNode()
        {
            base.ResetNode();
        }


        public override void Execute()
        {
            base.Execute();
        }

        public override void Abort(NodeState _state)
        {
            base.Abort(_state);
        }
        private void OnFinishNode()
        {
            foreach (var v in allCheckTweenTrigger)
            {
                v.DisableTrigger();
            }
        }
    }

#if UNITY_EDITOR
    [CustomEditor(typeof(TaskObserverTriggerEventNode))]
    public class ObserverTriggerEventNodeEditor : Editor
    {
        private SerializedProperty triggerProperty, boolProperty;

        private void OnEnable()
        {
            triggerProperty = serializedObject.FindProperty("allCheckTweenTrigger");
            boolProperty = serializedObject.FindProperty("allCheckTweenLoopValue");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            SerializedProperty arraySizeProperty = triggerProperty.FindPropertyRelative("Array.size");
            EditorGUILayout.PropertyField(arraySizeProperty);
            boolProperty.FindPropertyRelative("Array.size").intValue = arraySizeProperty.intValue;
            EditorGUI.indentLevel++;
            for (int i = 0; i < arraySizeProperty.intValue; i++)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(triggerProperty.GetArrayElementAtIndex(i), new GUIContent($"Trigger[{i}]:"));
                EditorGUILayout.PropertyField(boolProperty.GetArrayElementAtIndex(i), new GUIContent($"BoolValue[{i}]:"));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();

        }
    }
#endif
}
