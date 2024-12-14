using UnityEngine;

namespace Barterta.StylizeShader
{
    public class ShaderApplier : MonoBehaviour
    {
        private void Start()
        {
            var renderers = GetComponentsInChildren<Renderer>();
            foreach (var renderer in renderers)
            {
                foreach (var material in renderer.materials)
                {
                    material.shader = Shader.Find("Quibli/Stylized Lit");
                }
            }
        }
    }
}