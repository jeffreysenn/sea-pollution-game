using UnityEditor;

[InitializeOnLoad]
public class WebGLEditorScript
{
    static WebGLEditorScript()
    {
        PlayerSettings.SetPropertyBool("useEmbeddedResources", true, BuildTargetGroup.WebGL);
    }
}