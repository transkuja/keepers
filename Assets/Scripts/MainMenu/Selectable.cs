using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace Keepers
{
    public class Selectable : MonoBehaviour
    {

        [SerializeField]
        public bool selected = false;

        public SkinnedMeshRenderer meshToHighlight;

        public GameObject associatedSprite;

        public bool Selected
        {
            get { return selected; }
            set
            {
                selected = value;
                ToggleHighlightOnMesh(selected);
            }
        }

        private void ToggleHighlightOnMesh(bool isSelected)
        {
            if (meshToHighlight != null)
            {
                if (isSelected)
                {
                    meshToHighlight.material.shader = Shader.Find("Outlined/Silhouetted Diffuse");
                    meshToHighlight.material.SetColor("_OutlineColor", Color.blue);
                }
                else
                {
                    meshToHighlight.material.shader = Shader.Find("Diffuse");
                }
            }
        }
    }
}