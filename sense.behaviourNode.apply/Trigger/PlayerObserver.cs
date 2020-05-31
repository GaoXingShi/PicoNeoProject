using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sense.BehaviourTree
{
    public class PlayerObserver : TriggerBehaviour
    {
        [HideInInspector]
        public bool isEnter = false;
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
            gameObject.SetActive(false);
        }
    }

}

