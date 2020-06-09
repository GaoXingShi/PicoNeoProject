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
        // false 还在原地 true代表掉落
        [HideInInspector]
        public bool isNextAllow;
        private float timer;
        private const float dropTime = 1;

        // Start is called before the first frame update
        private Sequence sequence;

        void Start()
        {
            DOTween.Init(true, false, LogBehaviour.ErrorsOnly);
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

    }
}