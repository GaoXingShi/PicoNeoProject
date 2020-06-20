using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree
{
    public class PlayerObserver : TriggerBehaviour
    {
        [HideInInspector]
        public bool isEnter = false;
        [Header("是出局线吗?")]
        public bool isOutLine;
        void OnTriggerEnter(Collider _other)
        {
            if (!running)
            {
                return;
            }
            if (_other.tag.Equals("Player"))
            {
                isEnter = true;
                DisableTrigger();
            }
        }

        public override void DisableTrigger()
        {
            base.DisableTrigger();
            if (!isOutLine)
                gameObject.SetActive(false);
        }
    }

}

