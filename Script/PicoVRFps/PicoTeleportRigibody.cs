using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;


namespace PicoMainNameSpace
{
    [RequireComponent(typeof(Rigidbody))]
    public class PicoTeleportRigibody : MonoBehaviour
    {
        public Transform fixedPoint;

        private Rigidbody mRigidbody;
        private FixedJoint mFixedJoint;
        public static PicoTeleportRigibody singleton;

        private void Awake()
        {

            if (singleton == null)
            {
                singleton = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void OnDestroy()
        {
            singleton = null;
        }

        private void Start()
        {
            TryGetComponent(out mRigidbody);
            //TryGetComponent(out mFixedJoint);

        }

        public void SetTargetLandingLocation(Transform _parentObj,Vector3 _pos)
        {
            if (_parentObj != null)
            {
                // 是否运动
                mRigidbody.isKinematic = false;
                fixedPoint.parent = _parentObj;
                fixedPoint.position = _pos;
            }
        }

        public void CancelLandingLocation()
        {
            mRigidbody.isKinematic = true;
        }

        public Rigidbody GetMineRigidbody()
        {
            return mRigidbody;
        }
    }
}

