using System.Collections.Generic;
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

    }
}