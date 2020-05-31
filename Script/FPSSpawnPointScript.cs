using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Pvr_UnitySDKAPI;
using UnityEngine;
using UnityEngine.Assertions.Must;
using UnityStandardAssets.Characters.FirstPerson;

public class FPSSpawnPointScript : MonoBehaviour
{
    public GameObject fpsCtrl , spawnPoint;
    public MeshRenderer blurRenderer;

    private Material blurMaterial;

    private bool isStartBlur;

    private Sequence sequence;
    // Start is called before the first frame update
    void Start()
    {
        DOTween.Init(true,false, LogBehaviour.ErrorsOnly);
        blurMaterial = blurRenderer.material;

    }


    void OnTriggerEnter(Collider _collider)
    {
        Debug.Log(_collider);
        if (isStartBlur)
        {
            return;
        }

        if (_collider.tag.Equals("Player"))
        {
            isStartBlur = true;

            sequence = DOTween.Sequence();
            sequence.Append(DOTween.To(() => blurMaterial.color, x => blurMaterial.color = x, Color.black, 1));
            sequence.AppendInterval(1.5f);
            sequence.AppendCallback(() =>
            {
                fpsCtrl.GetComponent<VRPersonController>().enabled = false;
                fpsCtrl.transform.position = spawnPoint.transform.position;
            });

            sequence.Append(DOTween.To(() => blurMaterial.color, x => blurMaterial.color = x, Color.white, 1));
            sequence.AppendCallback(() =>
            {
                fpsCtrl.GetComponent<VRPersonController>().enabled = true;
                 
                isStartBlur = false;
            });


        }
    }
}
