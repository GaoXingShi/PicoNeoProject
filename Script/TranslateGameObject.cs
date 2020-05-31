using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateGameObject : MonoBehaviour
{
    public int count = 0;
    private const float SPEED = 0.01f;
    private Vector3 currentDirect = -Vector3.forward;
    
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.position = transform.position + currentDirect * SPEED;
        //transform.Translate(currentDirect * SPEED);
        count++;
        if (count == 180)
        {
            count = 0;
            currentDirect = -currentDirect;
        }
    }
}
