using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
[ImageEffectAllowedInSceneView]
public class ImageEffectBasic : MonoBehaviour
{
    public Material effectMaterial;

    //Gets called when the image of the scene is rendered
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {//destination is where the output of the shader goes
        //looks for _mainTex in the shader and automatically gives it the source render texture as its texture
        Graphics.Blit(source, destination, effectMaterial); //destination for normal shader is the render target
    }
}
