using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MainScene
{
    public class ToolEntity : MonoBehaviour
    {
        protected bool selection;
        public virtual void Selection()
        {
            if (selection)
            {
                return;
            }

            selection = true;
            gameObject.SetActive(selection);
        }
        public virtual void UnSelection()
        {
            if (!selection)
            {
                return;
            }

            selection = false;
            gameObject.SetActive(selection);
        }

    }

}

