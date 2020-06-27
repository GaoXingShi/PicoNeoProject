using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using PicoMainNameSpace;
using UnityEngine;

namespace Sense.BehaviourTree.VRTKExtend
{
    [System.Serializable]
    public struct NodeAndNames
    {
        public List<BehaviourNode> behaviourNode;
        public string name;
    }
    public class TreeNodeController : MonoBehaviour
    {

        [SerializeField]

        public List<BehaviourNode> rootNodes = new List<BehaviourNode>();

        public GameObject spriteCube;
        public PicoTeleportRigibody picoTeleport;
        public Teleport teleport;
        public CubeObserver[] turn1Array, turn2Array;
        public Vector3 turn1Pos, turn2Pos;
        public AudioSource backageAudioSource;
        public AudioClip backageClip;
        private void OnEnable()
        {
            InitChildNodes();
        }

        private void Start()
        {
            ExecuteCompositeNode();
        }


        private void InitChildNodes()
        {
            if (rootNodes.Count != 0)
            {
                return;
            }

            for (int i = 0; i < transform.childCount; i++)
            {
                BehaviourNode temp = transform.GetChild(i).GetComponent<BehaviourNode>();
                if (temp)
                {
                    rootNodes.Add(temp);
                }
            }

        }
        [ContextMenu("执行第1个根部复合节点")]
        public void ExecuteCompositeNode()
        {
            if (rootNodes.Count != 0)
            {
                rootNodes[0].ResetNode();
                rootNodes[0].Execute();
            }
        }


        public IEnumerator ResetTurn(int value)
        {
            spriteCube.SetActive(true);
            yield return StartCoroutine(teleport.ForceMove(picoTeleport.transform, value == 1 ? turn1Pos : turn2Pos));
            //picoTeleport.GetComponent<Rigidbody>().isKinematic = true;
            Time.timeScale = 0;
            backageAudioSource.clip = backageClip;
            backageAudioSource.Play(0);
            if (value == 1)
            {
                CubeObserver[] temp = turn1Array.Where(x =>
                        x.isNextAllow || x.IsRunning || x.isNextAllow || (x.sequence != null && x.sequence.IsPlaying()))
                    .ToArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].ResetTrigger();
                    yield return null;
                }
            }
            else if (value == 2)
            {
                CubeObserver[] temp = turn2Array.Where(x =>
                        x.isNextAllow || x.IsRunning || x.isNextAllow || (x.sequence != null && x.sequence.IsPlaying()))
                    .ToArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].ResetTrigger();
                    yield return null;
                }
            }

            Time.timeScale = 1;
            yield return new WaitForSeconds(3);
            spriteCube.SetActive(false);
            //picoTeleport.GetComponent<Rigidbody>().isKinematic = false;
            ExecuteCompositeNode();
        }

        public void StopAllCubeObserver(int value)
        {
            if (value == 1)
            {
                CubeObserver[] temp = turn1Array.Where(x =>
                        x.isNextAllow || x.IsRunning || x.isNextAllow || (x.sequence != null && x.sequence.IsPlaying()))
                    .ToArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].StopTrigger();
                }
            }
            else if (value == 2)
            {
                CubeObserver[] temp = turn2Array.Where(x =>
                        x.isNextAllow || x.IsRunning || x.isNextAllow || (x.sequence != null && x.sequence.IsPlaying()))
                    .ToArray();
                for (int i = 0; i < temp.Length; i++)
                {
                    temp[i].StopTrigger();
                }

            }
        }

    }
}