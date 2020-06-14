using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sense.BehaviourTree;
using UnityEditor;
using UnityEngine;

namespace gaoxingshiEditor
{
    public class GaoxingshiEditor : Editor
    {
        [MenuItem("Tools/Gaoxingshi/临时")]
        public static void OnEditorMethod01()
        {
            GameObject[] allSelection = Selection.objects.OfType<GameObject>().ToArray();
            for (int i = 0; i < allSelection.Length; i += 3)
            {
                if (i > 0)
                {
                    CubeObserver temp = allSelection[i - 3].GetComponent<CubeObserver>();
                    allSelection[i].GetComponent<CubeObserver>().backCubeObserver = temp;
                    if (i + 1 < allSelection.Length)
                        allSelection[i + 1].GetComponent<CubeObserver>().backCubeObserver = temp;
                    if (i + 2 < allSelection.Length)
                        allSelection[i + 2].GetComponent<CubeObserver>().backCubeObserver = temp;
                    
                }
                Debug.Log(allSelection[i].name, allSelection[i]);
            }
        }
    }

}

