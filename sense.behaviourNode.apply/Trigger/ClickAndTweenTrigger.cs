using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using MainScene;
using Pvr_UnitySDKAPI;
using Sense.BehaviourTree;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using VRTK;
using Image = UnityEngine.UI.Image;

namespace sense.networking.Apply
{
    public enum HandBridgeType
    {
        Hand,
        ToolEntity
    }

    public enum ResultOptionType
    {
        Postion,
        EurlerAngle,
        PostionAndEulerAngle,
        UIAndAudioSource,
        WaitOtherExecute,
        MeshRendererSwitch,
        Time,
        None,
    }

    [RequireComponent(typeof(HighlightingSystem.Highlighter))]
    public class ClickAndTweenTrigger : TriggerBehaviour
    {
        public HandBridgeType bridgeType;
        public ToolEntity handTool;
        public ResultOptionType optionType;

        public bool isHighlighter = true;

        // PostionAndEulerAngle
        public bool isRelative = true;
        public Vector3 incrementPos, incrementEur;

        public ClickAndTweenTrigger[] waitExecute;

        // UIAndAudioSource
        public CanvasGroup showUIGroup;
        public AudioSource audioSource;
        public AudioClip audioClip;

        // MeshRenderer
        public bool isChildHideActive;
        public MeshRenderer rendererComponent;
        public Material absoluteClearMaterial, changeMaterial;

        // Time
        public CanvasGroup canvasGroup;
        public Image sliderImage;
        public float timeValue;
        [HideInInspector] public bool isTimeFinish;
        public bool GetLoopHeadValue => loopHead;
        public bool GetLoopHeadValueEnd;
        private Vector3 initPos, initEur;
        private bool isWaitHandShank, loopHead;
        private ControllerHand leftCtrl, rightCtrl;
        private HighlightingSystem.Highlighter high;
        private Sequence DGSequence;

        void Start()
        {
            DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
            //VRTK
            //leftCtrl = VRTK_SDKManager.instance.scriptAliasLeftController.GetComponent<VRTK_ControllerEvents>();
            //rightCtrl = VRTK_SDKManager.instance.scriptAliasRightController.GetComponent<VRTK_ControllerEvents>();
            leftCtrl = Pvr_ControllerManager.controllerlink.Controller0;
            rightCtrl = Pvr_ControllerManager.controllerlink.Controller1;
            high = GetComponent<HighlightingSystem.Highlighter>();

            if (isRelative)
            {
                initPos = -incrementPos;
                initEur = -incrementEur;
            }
            else
            {
                initPos = transform.localPosition;
                initEur = transform.localEulerAngles;
            }

            DGSequence = DOTween.Sequence();

            if (rendererComponent == null)
            {
                rendererComponent = GetComponent<MeshRenderer>();
            }

            if (optionType == ResultOptionType.MeshRendererSwitch)
            {
                rendererComponent.material = absoluteClearMaterial;
                if (isChildHideActive)
                {
                    foreach (var v in rendererComponent.GetComponentsInChildren<Transform>().Where(v => v != transform))
                    {
                        v.gameObject.SetActive(false);
                    }
                }
            }

            if (GetComponent<Collider>())
            {
                GetComponent<Collider>().isTrigger = true;
            }

            EnableTrigger();
        }

        // Update is called once per frame
        void Update()
        {
            if (!running)
            {
                return;
            }
            if (isWaitHandShank && optionType != ResultOptionType.WaitOtherExecute)
            {
                if (isHighlighter)
                {
                    high.constant = true;
                    high.constantColor = Color.cyan;
                }
                if (leftCtrl.Trigger.Click || rightCtrl.Trigger.Click)
                {
                    if (!DGSequence.IsPlaying())
                        TriggerClickEvent();
                }
            }
            else
            {
                if (isHighlighter)
                    high.constant = false;
            }
        }

        public void SyncData(bool _loopHead)
        {
            loopHead = _loopHead;
            DoTweenPlay();
        }

        public void OnWaitOtherExecute()
        {
            Debug.Log(optionType, gameObject);
            if (optionType == ResultOptionType.WaitOtherExecute)
            {
                loopHead = !loopHead;
                DoTweenPlay();
            }
        }

        public override void EnableTrigger()
        {
            base.EnableTrigger();
        }

        public override void DisableTrigger()
        {
            base.DisableTrigger();
        }

        public void TriggerClickEvent()
        {
            loopHead = !loopHead;
            isWaitHandShank = false;
            DoTweenPlay();
            if (isHighlighter)
                high.constant = false;
        }


        void DoTweenPlay()
        {
            if (optionType == ResultOptionType.MeshRendererSwitch)
            {
                if (loopHead)
                {
                    rendererComponent.material = changeMaterial;
                    if (isChildHideActive)
                    {
                        foreach (var v in rendererComponent.GetComponentsInChildren<Transform>().Where(v => v != transform))
                        {
                            v.gameObject.SetActive(true);
                        }
                    }
                }
                else
                {
                    rendererComponent.material = absoluteClearMaterial;
                    foreach (var v in rendererComponent.GetComponentsInChildren<Transform>().Where(v => v != transform))
                    {
                        v.gameObject.SetActive(false);
                    }
                }

                return;
            }
            else if (optionType == ResultOptionType.None)
            {
                return;
            }

            DGSequence.Kill();
            DGSequence = DOTween.Sequence();

            if (optionType == ResultOptionType.UIAndAudioSource)
            {
                DGSequence.Append(DOTween.To(() => showUIGroup.alpha, x => showUIGroup.alpha = x,
                    1,
                    1));
                DGSequence.AppendCallback(() =>
                {
                    audioSource.clip = audioClip;
                    audioSource.loop = true;
                    audioSource.Play();
                    GetLoopHeadValueEnd = loopHead;
                });
            }
            else if (optionType == ResultOptionType.Postion || optionType == ResultOptionType.PostionAndEulerAngle ||
                optionType == ResultOptionType.EurlerAngle || optionType == ResultOptionType.WaitOtherExecute)
            {
                DGSequence.SetRelative(isRelative);

                if (loopHead)
                {

                    // Pos Tween
                    if (optionType == ResultOptionType.Postion ||
                        optionType == ResultOptionType.PostionAndEulerAngle ||
                        optionType == ResultOptionType.WaitOtherExecute)
                    {
                        DGSequence.Append(DOTween.To(() => transform.localPosition, x =>
                            {
                                transform.localPosition = x;
                            },
                            incrementPos,
                            2));
                    }

                    if (optionType == ResultOptionType.EurlerAngle ||
                        optionType == ResultOptionType.PostionAndEulerAngle ||
                        optionType == ResultOptionType.WaitOtherExecute)
                    {
                        if (optionType == ResultOptionType.EurlerAngle)
                        {
                            DGSequence.Append(DOTween.To(() => transform.localEulerAngles,
                                x =>
                                {
                                    transform.localEulerAngles = x;
                                },
                                incrementEur,
                                2));
                        }
                        else
                        {
                            DGSequence.Join(DOTween.To(() => transform.localEulerAngles,
                                x =>
                                {
                                    transform.localEulerAngles = x;
                                },
                                incrementEur,
                                2));
                        }
                    }

                    DGSequence.AppendCallback(() =>
                    {
                        GetLoopHeadValueEnd = loopHead;
                    });
                }
                else
                {
                    // euler Tween
                    if (optionType == ResultOptionType.Postion ||
                        optionType == ResultOptionType.PostionAndEulerAngle ||
                        optionType == ResultOptionType.WaitOtherExecute)
                    {
                        DGSequence.Append(DOTween.To(() => transform.localPosition, x => transform.localPosition = x,
                            initPos,
                            2));
                    }

                    if (optionType == ResultOptionType.EurlerAngle ||
                        optionType == ResultOptionType.PostionAndEulerAngle ||
                        optionType == ResultOptionType.WaitOtherExecute)
                    {
                        if (optionType == ResultOptionType.EurlerAngle)
                        {
                            DGSequence.Append(DOTween.To(() => transform.localRotation,
                                x => transform.localRotation = x,
                                initEur,
                                2));
                        }
                        else
                        {
                            DGSequence.Join(DOTween.To(() => transform.localRotation,
                                x => transform.localRotation = x,
                                initEur,
                                2));
                        }


                        DGSequence.AppendCallback(() =>
                        {
                            GetLoopHeadValueEnd = loopHead;
                        });
                    }
                }

                if (waitExecute.Length != 0)
                {
                    DGSequence.AppendCallback(() =>
                    {
                        for (int i = 0; i < waitExecute.Length; i++)
                        {
                            ClickAndTweenTrigger temp = waitExecute[i];
                            temp.OnWaitOtherExecute();
                        }

                    });
                }
            }
            else if (optionType == ResultOptionType.Time)
            {
                DisableTrigger();
                DGSequence.Append(DOTween.To(() => sliderImage.fillAmount,
                    x =>
                    {
                        sliderImage.fillAmount = x;
                        canvasGroup.alpha = 1;
                    },
                    1,
                    timeValue));
                DGSequence.AppendCallback(() =>
                {
                    canvasGroup.alpha = 0;
                    sliderImage.fillAmount = 0;
                    isTimeFinish = true;
                    GetLoopHeadValueEnd = loopHead;
                });
            }
        }

        void OnTriggerEnter(Collider _collider)
        {
            if (DGSequence.IsPlaying() || optionType == ResultOptionType.WaitOtherExecute)
            {
                return;
            }

            if (bridgeType == HandBridgeType.Hand)
            {
                if (_collider.tag.Equals("Hand"))
                {
                    isWaitHandShank = true;
                }
            }
            else if (bridgeType == HandBridgeType.ToolEntity)
            {
                if (handTool == null)
                {
                    Debug.LogError("HandTool Null");
                    return;
                }

                if (_collider.GetComponent<ToolEntity>() != null && _collider.GetComponent<ToolEntity>().GetInstanceID() == handTool.GetInstanceID())
                {
                    isWaitHandShank = true;
                }
            }
        }

        void OnTriggerExit(Collider _collider)
        {
            if (optionType == ResultOptionType.WaitOtherExecute)
            {
                return;
            }

            if (isWaitHandShank)
            {
                if (bridgeType == HandBridgeType.Hand)
                {
                    if (_collider.tag.Equals("Hand"))
                    {
                        isWaitHandShank = false;
                    }
                }
                else if (bridgeType == HandBridgeType.ToolEntity)
                {
                    if (_collider.GetComponent<ToolEntity>() != null && _collider.GetComponent<ToolEntity>().GetInstanceID() == handTool.GetInstanceID())
                    {
                        isWaitHandShank = false;
                    }
                }
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(ClickAndTweenTrigger))]
    public class ClickAndTweenTriggerEditor : Editor
    {
        // 序列化对象
        private SerializedObject test;
        private ClickAndTweenTrigger currentTrigger;

        // 暂存值
        private int waitExecuteLengthValue;

        void OnEnable()
        {
            test = serializedObject;
            currentTrigger = target as ClickAndTweenTrigger;
            if (currentTrigger.waitExecute != null)
                waitExecuteLengthValue = currentTrigger.waitExecute.Length;
        }
        public override void OnInspectorGUI()
        {
            // 从物体上抓取最新的信息
            test.Update();
            currentTrigger.bridgeType = (HandBridgeType)EditorGUILayout.EnumPopup(new GUIContent("HandBridgeType :"), currentTrigger.bridgeType);

            if (currentTrigger.bridgeType == HandBridgeType.Hand)
            {
            }
            else if (currentTrigger.bridgeType == HandBridgeType.ToolEntity)
            {
                currentTrigger.handTool = EditorGUILayout.ObjectField(new GUIContent("ToolEntity:"), currentTrigger.handTool, typeof(ToolEntity)) as ToolEntity;
            }

            currentTrigger.isHighlighter = EditorGUILayout.Toggle("IsHighlighter", currentTrigger.isHighlighter);

            currentTrigger.optionType = (ResultOptionType)EditorGUILayout.EnumPopup(new GUIContent("Vector3OptionType :"), currentTrigger.optionType);
            if (currentTrigger.optionType == ResultOptionType.Postion ||
                currentTrigger.optionType == ResultOptionType.PostionAndEulerAngle ||
                currentTrigger.optionType == ResultOptionType.EurlerAngle || currentTrigger.optionType == ResultOptionType.WaitOtherExecute)
            {
                currentTrigger.isRelative = EditorGUILayout.Toggle("isRelative", currentTrigger.isRelative);

                if (currentTrigger.optionType == ResultOptionType.Postion || currentTrigger.optionType == ResultOptionType.PostionAndEulerAngle || currentTrigger.optionType == ResultOptionType.WaitOtherExecute)
                {
                    currentTrigger.incrementPos = EditorGUILayout.Vector3Field("IncrementPos", currentTrigger.incrementPos);
                }

                if (currentTrigger.optionType == ResultOptionType.EurlerAngle || currentTrigger.optionType == ResultOptionType.PostionAndEulerAngle || currentTrigger.optionType == ResultOptionType.WaitOtherExecute)
                {
                    currentTrigger.incrementEur = EditorGUILayout.Vector3Field("IncrementEur", currentTrigger.incrementEur);
                }

                if (currentTrigger.optionType != ResultOptionType.WaitOtherExecute)
                {
                    if (currentTrigger.waitExecute != null)
                    {
                        if (waitExecuteLengthValue != currentTrigger.waitExecute.Length)
                        {
                            currentTrigger.waitExecute = new ClickAndTweenTrigger[waitExecuteLengthValue];
                        }
                    }
                    else
                    {
                        currentTrigger.waitExecute = new ClickAndTweenTrigger[waitExecuteLengthValue];
                    }

                    waitExecuteLengthValue =
                        EditorGUILayout.IntField("waitExecuteLengthValue:", waitExecuteLengthValue);
                    for (int i = 0; i < currentTrigger.waitExecute.Length; i++)
                    {
                        currentTrigger.waitExecute[i] =
                            EditorGUILayout.ObjectField(new GUIContent($"WaitExecute[{i}]:"),
                                currentTrigger.waitExecute[i], typeof(ClickAndTweenTrigger)) as ClickAndTweenTrigger;
                    }
                }
            }


            if (currentTrigger.optionType == ResultOptionType.UIAndAudioSource)
            {
                currentTrigger.showUIGroup = EditorGUILayout.ObjectField(new GUIContent("ShowUIGroup:"), currentTrigger.showUIGroup, typeof(CanvasGroup)) as CanvasGroup;
                currentTrigger.audioSource = EditorGUILayout.ObjectField(new GUIContent("AudioSource:"), currentTrigger.audioSource, typeof(AudioSource)) as AudioSource;
                currentTrigger.audioClip = EditorGUILayout.ObjectField(new GUIContent("AudioClip:"), currentTrigger.audioClip, typeof(AudioClip)) as AudioClip;
            }

            if (currentTrigger.optionType == ResultOptionType.MeshRendererSwitch)
            {
                currentTrigger.isChildHideActive = EditorGUILayout.Toggle("IsChildHideActive", currentTrigger.isChildHideActive);
                currentTrigger.rendererComponent = EditorGUILayout.ObjectField(new GUIContent("RendererComponent:"), currentTrigger.rendererComponent, typeof(MeshRenderer)) as MeshRenderer;
                currentTrigger.absoluteClearMaterial = EditorGUILayout.ObjectField(new GUIContent("AbsoluteClearMaterial:"), currentTrigger.absoluteClearMaterial, typeof(Material)) as Material;
                currentTrigger.changeMaterial = EditorGUILayout.ObjectField(new GUIContent("ChangeMaterial:"), currentTrigger.changeMaterial, typeof(Material)) as Material;
            }

            if (currentTrigger.optionType == ResultOptionType.Time)
            {
                currentTrigger.canvasGroup = EditorGUILayout.ObjectField(new GUIContent("CanvasGroup:"), currentTrigger.canvasGroup, typeof(CanvasGroup)) as CanvasGroup;
                currentTrigger.sliderImage = EditorGUILayout.ObjectField(new GUIContent("SliderImage:"), currentTrigger.sliderImage, typeof(Image)) as Image;
                currentTrigger.timeValue =
                    EditorGUILayout.FloatField(new GUIContent("TimeValue:"), currentTrigger.timeValue);
            }

            if (EditorApplication.isPlaying)
            {
                if (GUILayout.Button("模拟点击"))
                {
                    currentTrigger.TriggerClickEvent();
                }
            }

            test.ApplyModifiedProperties();
        }
    }
#endif

}

