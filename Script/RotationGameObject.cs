using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationGameObject : MonoBehaviour
{
    // 是否是顺时针
    public bool isSequence = true;
    private const float ANGLE = 0.5f;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(isSequence ? (Vector3.up) : -Vector3.up * ANGLE);
    }
}
