using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ProJect1
{
    public class OutlineHighlighter : MonoBehaviour
    {
        [Header("적용할 Outline 머티리얼")]
        public Material outlineMaterial;

        private SkinnedMeshRenderer[] renderers;
        private Material[][] originalMaterials;

        void Awake()
        {
            renderers = GetComponentsInChildren<SkinnedMeshRenderer>();
            originalMaterials = new Material[renderers.Length][];
        }

        public void ApplyOutline()
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                var renderer = renderers[i];
                if (renderer == null) continue;

                // 기존 머티리얼 저장
                originalMaterials[i] = renderer.materials;

                // 기존 머티리얼 + outline 머티리얼 적용
                Material[] newMats = new Material[renderer.materials.Length + 1];
                renderer.materials.CopyTo(newMats, 0);
                newMats[newMats.Length - 1] = outlineMaterial;
                renderer.materials = newMats;
            }
        }

        public void ClearOutline()
        {
            for (int i = 0; i < renderers.Length; i++)
            {
                if (renderers[i] != null && originalMaterials[i] != null)
                {
                    renderers[i].materials = originalMaterials[i];
                }
            }
        }
    }
}
