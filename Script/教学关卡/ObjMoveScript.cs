using System.Collections;
using System.Collections.Generic;
using Sense.BehaviourTree;
using UnityEngine;
using UnityEngine.UI;

public class ObjMoveScript : TriggerBehaviour
{
    public Transform target;
    [Range(0, 90)]
    public float targetAngleMove = 30;

    public Text hintText;
    public int moveFinishTaskCount = 10, rotationFinishTaskCount = 10;
    private bool isEnterIenumerator;

    private int moveCount = 0, rotationCount = 0;
    // Use this for initialization
    void Start()
    {
        transform.position = target.forward + target.transform.position;
        Teleport.moveMethod += TeleportOnMoveMethod;
        Teleport.rotationMethod += TeleportOnRotationMethod;

        hintText.text =
            $"需移动{moveFinishTaskCount}次：已移动{moveCount}次。\n\n需旋转{rotationFinishTaskCount}次：已旋转{rotationCount}次。";
    }

    private void TeleportOnRotationMethod()
    {
        if (!running)
            return;

        rotationCount++;
        hintText.text =
            $"需移动{moveFinishTaskCount}次：已移动{moveCount}次。\n\n需旋转{rotationFinishTaskCount}次：已旋转{rotationCount}次。";
    }

    private void TeleportOnMoveMethod()
    {
        if (!running)
            return;
        moveCount++;
        hintText.text =
            $"需移动{moveFinishTaskCount}次：已移动{moveCount}次。\n\n需旋转{rotationFinishTaskCount}次：已旋转{rotationCount}次。";
    }

    private void CheckFinishTask()
    {
        if (moveCount >= moveFinishTaskCount)
        {
            if (rotationCount >= rotationFinishTaskCount)
            {
                Teleport.ClearAllEvent();
                DisableTrigger();
            }
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

    void Update()
    {
        if (target)
        {
            if (Quaternion.Angle(transform.rotation, target.rotation) >= targetAngleMove && !isEnterIenumerator)
            {
                isEnterIenumerator = true;
                StartCoroutine(AngleBegin());
            }
        }
    }
    IEnumerator AngleBegin()
    {
        while (running)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, target.rotation, 0.1f);
            Vector3 tmp = target.forward + target.transform.position;
            transform.position = Vector3.Lerp(transform.position, tmp, 0.05f);
            if (Mathf.RoundToInt(targetAngleMove) != 0 && Vector3.Distance(transform.position, tmp) <= 0.02f)
            {
                transform.position = tmp;
            }
            if (Quaternion.Angle(transform.rotation, target.rotation) <= 0.2f)
            {
                isEnterIenumerator = false;
                break;
            }
            yield return null;
        }
        yield return null;
    }
}