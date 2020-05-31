﻿using UnityEngine;

public class FirstView : MonoBehaviour
{
    //方向灵敏度
    public float sensitivityX = 10F;
    public float sensitivityY = 10F;

    //上下最大视角(Y视角)
    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationY = 0F;
    private float YValue;

    void Start()
    {
        YValue = transform.position.y;
    }
    void LateUpdate()
    {
        if (Input.GetMouseButton(1))
        {
            //根据鼠标移动的快慢(增量), 获得相机左右旋转的角度(处理X)
            float rotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * sensitivityX;

            //根据鼠标移动的快慢(增量), 获得相机上下旋转的角度(处理Y)
            rotationY += Input.GetAxis("Mouse Y") * sensitivityY;
            //角度限制. rotationY小于min,返回min. 大于max,返回max. 否则返回value 
            rotationY = Mathf.Clamp(rotationY, minimumY, maximumY);

            //总体设置一下相机角度
            transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0);
        }

        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        bool isShift = Input.GetKey(KeyCode.LeftShift);
        int multiple = isShift ? 5 : 1;

        transform.Translate(Vector3.forward * vertical * Time.deltaTime * multiple);
        transform.Translate(Vector3.right * horizontal * Time.deltaTime * multiple);


        Vector3 temp = transform.position;
        temp.y = YValue;
        transform.position = temp;

    }

}