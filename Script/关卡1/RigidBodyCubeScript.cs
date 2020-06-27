using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class RigidBodyCubeScript : MonoBehaviour
{
    public Vector3[] pathLocalPosArray;
    public float timer;
    private Sequence mSequence;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        mSequence = DOTween.Sequence();
        for (int i = 1; i < pathLocalPosArray.Length; i++)
        {
            Vector3 value = pathLocalPosArray[i];
            mSequence.Append(DOTween.To(() => transform.localPosition, x => transform.localPosition = x, value, timer));
        }

        mSequence.SetLoops(-1,LoopType.Yoyo);
    }

}
