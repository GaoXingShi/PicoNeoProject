using System.Collections;
using System.Collections.Generic;
using sense.networking.Apply;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    /// <summary>
    /// 播放音效的Task
    /// </summary>
    public class TaskAudioClipPlayNode : BehaviourNode
    {
        public AudioSource audioSource;
        public AudioClip audioClip;
        public bool nodeIsPlay = true;
        public bool playFinishSucceed = false;
        public bool isLoop = false;

        private bool updateRunning;
        public override void Execute()
        {
            audioSource.clip = audioClip;
            audioSource.Play(0);
            if (playFinishSucceed)
            {
                updateRunning = true;
                isLoop = false;
            }
            else
            {
                State = NodeState.Succeed;
                audioSource.loop = isLoop;
            }
        }

        private void Update()
        {
            if (!updateRunning)
            {
                return;
            }

            if (!audioSource.isPlaying)
            {
                updateRunning = true;
                State = NodeState.Succeed;
                if (isLoop)
                {
                    audioSource.loop = isLoop;
                    audioSource.Play(0);
                }
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

#if UNITY_EDITOR
    [CustomEditor(typeof(TaskAudioClipPlayNode))]
    public class ObserverTriggerEventNodeEditor : Editor
    {        // 序列化对象
        private TaskAudioClipPlayNode node;

        void OnEnable()
        {
            node = target as TaskAudioClipPlayNode;
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            node.audioSource = EditorGUILayout.ObjectField(new GUIContent("AudioSource"), node.audioSource, typeof(AudioSource), true) as AudioSource;
            node.nodeIsPlay = EditorGUILayout.Toggle(new GUIContent("NodeIsPlay"), node.nodeIsPlay);
            if (node.nodeIsPlay)
            {
                node.playFinishSucceed = EditorGUILayout.Toggle(new GUIContent("PlayFinishSucceed"), node.playFinishSucceed);

                node.isLoop = EditorGUILayout.Toggle(new GUIContent("IsLoop"), node.isLoop);
                node.audioClip = EditorGUILayout.ObjectField(new GUIContent("AudioClip"), node.audioClip, typeof(AudioClip), true) as AudioClip;
            }

            serializedObject.ApplyModifiedProperties();
        }
    }
#endif
}
