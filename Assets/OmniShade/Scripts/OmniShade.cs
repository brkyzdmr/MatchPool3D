//------------------------------------
//             OmniShade
//     Copyright© 2023 OmniShade     
//------------------------------------
using UnityEngine;

/**
 * This class contains shader constants and utility functions.
 **/
public static class OmniShade {
	public const string NAME = "OmniShade";
    public const string STANDARD_SHADER = "OmniShade/Standard";
    public const string STANDARD_URP_SHADER = "OmniShade/Standard URP";
    public const string TRIPLANAR_SHADER = "OmniShade/Triplanar";
    public const string TRIPLANAR_URP_SHADER = "OmniShade/Triplanar URP";

	public const string DOCS_URL = "https://www.omnishade.io/documentation/features";
	public const string PRO_URL = "https://assetstore.unity.com/packages/vfx/shaders/omnishade-mobile-optimized-shader-213594";

	public const int TRIPLANAR_UV_SCALE = 32;
    
    // PRO ONLY begin
    public const int NORMAL_LOD = 200;
    public const int FALLBACK_LOD = 100;

    public static void SetNormalShader() {
        OmniShade.SetShaderLOD(OmniShade.NORMAL_LOD);
    }

    public static void SetFallbackShader() {
        OmniShade.SetShaderLOD(OmniShade.FALLBACK_LOD);
    }

    public static bool IsFallbackShader() {
        var shader = Shader.Find(OmniShade.STANDARD_SHADER);
        if (shader == null) {
            shader = Shader.Find(OmniShade.STANDARD_URP_SHADER);
            if (shader == null) {
                Debug.LogWarning(OmniShade.NAME + ": Cannot find " + OmniShade.STANDARD_SHADER);
                return false;
            }
        }

        int lod = shader.maximumLOD;
        return 0 < lod && lod < OmniShade.NORMAL_LOD;
    }
    
    static void SetShaderLOD(int lod) {
        string[] shaderNames = new string[] { 
            OmniShade.STANDARD_SHADER, 
            OmniShade.STANDARD_URP_SHADER, 
            OmniShade.TRIPLANAR_SHADER, 
            OmniShade.TRIPLANAR_URP_SHADER
        };

        foreach (var shaderName in shaderNames) {
            var shader = Shader.Find(shaderName);
            if (shader != null)
                shader.maximumLOD = lod;
        }
    }
    // PRO ONLY end
}
