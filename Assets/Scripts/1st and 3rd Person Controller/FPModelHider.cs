using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FPModelHider : MonoBehaviour
{
    public MeshRenderer[] renderers;

    public void HideMeshes()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.ShadowsOnly;
        }
    }

    public void ShowMeshes()
    {
        for (int i = 0; i < renderers.Length; i++)
        {
            renderers[i].shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
        }
    }
}
