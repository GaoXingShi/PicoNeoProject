using System.Collections;
using System.Collections.Generic;
using Pvr_UnitySDKAPI;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneCtrl : MonoBehaviour
{
    public string sceneName;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyClick(0, Pvr_KeyCode.Y) && Pvr_UnitySDKAPI.Controller.UPvr_GetKeyClick(1, Pvr_KeyCode.B))
        {
            SceneManager.LoadScene(sceneName);
        }

        if (Pvr_UnitySDKAPI.Controller.UPvr_GetKeyClick(0, Pvr_KeyCode.APP) ||
            Pvr_UnitySDKAPI.Controller.UPvr_GetKeyClick(1, Pvr_KeyCode.APP))
        {
            Application.Quit();
        }
    }
}
