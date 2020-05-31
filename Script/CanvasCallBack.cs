using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasCallBack : MonoBehaviour
{
    public Dropdown mDropDown;

    public MeshRenderer mian, xian;

    public Material planA_Normal,planA_Emission, planB_Normal, planB_Emission, planC_Normal, planC_Emission;
    void Update()
    {
        if (mDropDown.value == 0)
        {
            mian.material = planA_Normal;
            xian.material = planA_Emission;
        }
        else if (mDropDown.value == 1)
        {
            mian.material = planB_Normal;
            xian.material = planB_Emission;
        }
        else if (mDropDown.value == 2)
        {
            mian.material = planC_Normal;
            xian.material = planC_Emission;
        }
    }


}
