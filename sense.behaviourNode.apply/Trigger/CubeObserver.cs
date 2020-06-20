using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace Sense.BehaviourTree
{
    public class CubeObserver : TriggerBehaviour
    {
        public CubeObserver backCubeObserver;

        public AudioSource audioSource;

        public CubeObserver[] jointCubeObserverArray;

        // false 还在原地 true代表掉落
        [HideInInspector]
        public bool isNextAllow;
        private float timer;
        private const float dropTime = 1;

        // Start is called before the first frame update
        [HideInInspector]
        public Sequence sequence;
        private Vector3 initPos;
        private CubeObserver cacheBackCubeObserver;

        private Material cubeMaterial;
        private Transform playerRef;
        private bool redPath;

        private const float DISTANCE_PLAYER_VALUE = 8, EMISSION_CHANGE_VALUE = 1.5f;
        void Start()
        {
            DOTween.Init(true, false, LogBehaviour.ErrorsOnly);
            initPos = transform.position;
            cacheBackCubeObserver = backCubeObserver;
            cubeMaterial = GetComponent<MeshRenderer>().material;

            if (jointCubeObserverArray.Length > 0)
            {
                cubeMaterial.SetColor("_EmissionColor", Color.red);
                playerRef = GameObject.FindGameObjectWithTag("Player").transform;
            }
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying && jointCubeObserverArray.Length > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawMesh(GetComponent<MeshFilter>().sharedMesh, transform.position, transform.rotation,transform.lossyScale);
                Gizmos.color = Color.green;
                foreach (var v in jointCubeObserverArray)
                {
                    Gizmos.DrawMesh(v.GetComponent<MeshFilter>().sharedMesh, v.transform.position, v.transform.rotation, v.transform.lossyScale);
                }
            }
        }


        void Update()
        {
            if (playerRef != null && !redPath)
            {
                if (Vector3.Distance(playerRef.position, transform.position) <= DISTANCE_PLAYER_VALUE)
                {
                    redPath = true;
                    sequence = DOTween.Sequence();
                    sequence.Append(
                        DOTween.To(() => cubeMaterial.GetColor("_EmissionColor"), x => cubeMaterial.SetColor("_EmissionColor", x), Color.black, EMISSION_CHANGE_VALUE));
                    sequence.AppendCallback(() =>
                    {
                        foreach (var v in jointCubeObserverArray)
                        {
                            v.EnableTrigger();
                        }
                        EnableTrigger();
                    });
                }
            }
        }

        // Update is called once per frame
        void FixedUpdate()
        {
            if ((backCubeObserver != null && backCubeObserver.isNextAllow))
            {
                backCubeObserver = null;
                EnableTrigger();
            }

            if (running)
            {
                backCubeObserver = null;

                timer += Time.deltaTime;
                if (timer >= dropTime)
                {
                    timer = 0;
                    DisableTrigger();
                    isNextAllow = true;
                    DropPlay();
                }
            }
        }

        void DropPlay()
        {
            audioSource.Play(0);
            sequence = DOTween.Sequence();
            sequence.SetRelative(true);
            sequence.Append(
                DOTween.To(() => transform.localPosition, x => transform.localPosition = x, Vector3.up * -600, 6));
            sequence.AppendCallback(() => { gameObject.SetActive(false); });
        }


        public override void EnableTrigger()
        {
            base.EnableTrigger();
        }

        public void ResetTrigger()
        {
            if (sequence != null && sequence.IsPlaying())
            {
                sequence.Kill(true);
            }

            transform.position = initPos;
            gameObject.SetActive(true);
            backCubeObserver = cacheBackCubeObserver;
            if (backCubeObserver != null)
                backCubeObserver.isNextAllow = false;
            isNextAllow = false;
            DisableTrigger();
            timer = 0;

            redPath = false;
            if (jointCubeObserverArray.Length > 0)
            {
                cubeMaterial.SetColor("_EmissionColor", Color.red);
            }
        }

        public void StopTrigger()
        {
            if (sequence != null && sequence.IsPlaying())
            {
                sequence.Kill(true);
            }

            if (backCubeObserver != null)
                backCubeObserver.isNextAllow = false;
            isNextAllow = false;
            DisableTrigger();
            timer = 0;

        }

    }
}