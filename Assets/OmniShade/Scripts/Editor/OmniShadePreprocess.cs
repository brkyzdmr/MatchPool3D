//------------------------------------
//             OmniShade
//     Copyright© 2023 OmniShade     
//------------------------------------

using UnityEditor.Build;
using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.Rendering;
using System.Collections.Generic;

/**
 * This class strips shader variants at build time to reduce the shader size and memory.
 * Note that you should adjust the project-wide Shader Stripping settings in Project Settings->Graphics first.
 * It is completely optional to toggle the optimization flags in the material.
 **/
public class OmniShadePreprocess : IPreprocessShaders {
    // PROJECT-WIDE SETTINGS
    // Set these values to always strip certain variants
    const bool ALWAYS_STRIP_SHADOWS = false;
    const bool ALWAYS_STRIP_POINT_LIGHTS = false;
    const bool ALWAYS_STRIP_FALLBACK = false;

    // Shadows
    ShaderKeyword SHADOWS_SHADOWMASK = new ShaderKeyword("SHADOWS_SHADOWMASK");
    ShaderKeyword SHADOWS_SCREEN = new ShaderKeyword("SHADOWS_SCREEN");
    ShaderKeyword _SHADOWS_SOFT = new ShaderKeyword("_SHADOWS_SOFT");
    ShaderKeyword _MAIN_LIGHT_SHADOWS = new ShaderKeyword("_MAIN_LIGHT_SHADOWS");
    ShaderKeyword _MAIN_LIGHT_SHADOWS_CASCADE = new ShaderKeyword("_MAIN_LIGHT_SHADOWS_CASCADE");
    ShaderKeyword SHADOWS_ENABLED = new ShaderKeyword("SHADOWS_ENABLED");
    ShaderKeyword _OPTSHADOW_DISABLED = new ShaderKeyword("_OPTSHADOW_DISABLED");
    ShaderKeyword _OPTSHADOW_ENABLED_ONLY = new ShaderKeyword("_OPTSHADOW_ENABLED_ONLY");

    // Point light
    ShaderKeyword VERTEXLIGHT_ON = new ShaderKeyword("VERTEXLIGHT_ON");
    ShaderKeyword _ADDITIONAL_LIGHTS = new ShaderKeyword("_ADDITIONAL_LIGHTS");
    ShaderKeyword DIFFUSE = new ShaderKeyword("DIFFUSE");
    ShaderKeyword _OPTPOINTLIGHTS_DISABLED = new ShaderKeyword("_OPTPOINTLIGHTS_DISABLED");

    // Fog
    ShaderKeyword FOG_LINEAR = new ShaderKeyword("FOG_LINEAR");
    ShaderKeyword FOG_EXP = new ShaderKeyword("FOG_EXP");
    ShaderKeyword FOG_EXP2 = new ShaderKeyword("FOG_EXP2");
    ShaderKeyword FOG = new ShaderKeyword("FOG");
    ShaderKeyword _OPTFOG_DISABLED = new ShaderKeyword("_OPTFOG_DISABLED");
    ShaderKeyword _OPTFOG_ENABLED_ONLY = new ShaderKeyword("_OPTFOG_ENABLED_ONLY");

    // Lightmapping
    ShaderKeyword LIGHTMAP_ON = new ShaderKeyword("LIGHTMAP_ON");
    ShaderKeyword DIRLIGHTMAP_COMBINED = new ShaderKeyword("DIRLIGHTMAP_COMBINED");
    ShaderKeyword _OPTLIGHTMAPPING_DISABLED = new ShaderKeyword("_OPTLIGHTMAPPING_DISABLED");
    ShaderKeyword _OPTLIGHTMAPPING_ENABLED_ONLY = new ShaderKeyword("_OPTLIGHTMAPPING_ENABLED_ONLY");

    // Fallback
    ShaderKeyword _OPTFALLBACK_DISABLED = new ShaderKeyword("_OPTFALLBACK_DISABLED");
    const string FALLBACK_PASS_NAME = "Fallback";

    // Outline
    ShaderKeyword OUTLINE = new ShaderKeyword("OUTLINE");
    ShaderKeyword OUTLINE_PASS_DISABLED = new ShaderKeyword("OUTLINE_PASS_DISABLED");
    const string OUTLINE_PASS_NAME = "Outline";

    // Depth
    const string DEPTH_PASS_NAME = "DepthOnly";
    
    public void OnProcessShader(Shader shader, ShaderSnippetData snippet, IList<ShaderCompilerData> data) {
		// Skip if not OmniShade, or if OmniShade PBR
		if (!shader.name.Contains(OmniShade.NAME) || shader.name.Contains("PBR"))
            return;

        for (int i = data.Count - 1; i >= 0; --i) {
            var keys = data[i].shaderKeywordSet;

            // Strip shadows
            bool isShadowVariant = keys.IsEnabled(SHADOWS_SHADOWMASK) || keys.IsEnabled(SHADOWS_SCREEN) || keys.IsEnabled(_SHADOWS_SOFT) ||
                keys.IsEnabled(_MAIN_LIGHT_SHADOWS) || keys.IsEnabled(_MAIN_LIGHT_SHADOWS_CASCADE);
            bool isShadowDisabled = !keys.IsEnabled(SHADOWS_ENABLED) || keys.IsEnabled(_OPTSHADOW_DISABLED) || ALWAYS_STRIP_SHADOWS;
            bool isShadowEnabledOnly = keys.IsEnabled(SHADOWS_ENABLED) && keys.IsEnabled(_OPTSHADOW_ENABLED_ONLY);
            if ((isShadowVariant && isShadowDisabled) || (!isShadowVariant && isShadowEnabledOnly)) {
                data.RemoveAt(i);
                continue;
            }

            // Strip shadow caster pass
            bool isShadowCasterPass = snippet.passType == PassType.ShadowCaster;
            bool isShadowCasterDisabled = keys.IsEnabled(_OPTSHADOW_DISABLED) || ALWAYS_STRIP_SHADOWS;
            bool isShadowCasterEnabledOnly = keys.IsEnabled(_OPTSHADOW_ENABLED_ONLY);
            if ((isShadowCasterPass && isShadowCasterDisabled) || (!isShadowCasterPass && isShadowCasterEnabledOnly)) {
                data.RemoveAt(i);
                continue;
            }

            // Strip point lights
            bool isPointLightVariant = keys.IsEnabled(VERTEXLIGHT_ON) || keys.IsEnabled(_ADDITIONAL_LIGHTS);
            bool isPointLightDisabled = !keys.IsEnabled(DIFFUSE) || keys.IsEnabled(_OPTPOINTLIGHTS_DISABLED) || ALWAYS_STRIP_POINT_LIGHTS;
            if (isPointLightVariant && isPointLightDisabled) {
                data.RemoveAt(i);
                continue;
            }

            // Strip fog
            bool isFogVariant = keys.IsEnabled(FOG_LINEAR) || keys.IsEnabled(FOG_EXP) || keys.IsEnabled(FOG_EXP2);
            bool isFogDisabled = !keys.IsEnabled(FOG) || keys.IsEnabled(_OPTFOG_DISABLED);
            bool isFogEnabledOnly = keys.IsEnabled(FOG) && keys.IsEnabled(_OPTFOG_ENABLED_ONLY);
            if ((isFogVariant && isFogDisabled) || (!isFogVariant && isFogEnabledOnly)) {
                data.RemoveAt(i);
                continue;
            }

            // Strip lightmapping
            bool isLightmappingVariant = keys.IsEnabled(LIGHTMAP_ON) || keys.IsEnabled(DIRLIGHTMAP_COMBINED);
            bool isLightmappingDisabled = keys.IsEnabled(_OPTLIGHTMAPPING_DISABLED);
            bool isLightmappingEnableOnly = keys.IsEnabled(_OPTLIGHTMAPPING_ENABLED_ONLY);
            if ((isLightmappingVariant && isLightmappingDisabled) || (!isLightmappingVariant && isLightmappingEnableOnly)) {
                data.RemoveAt(i);
                continue;
            }

            // Strip fallback
            bool isFallbackPass = snippet.passName == FALLBACK_PASS_NAME;
            bool isFallbackDisabled = keys.IsEnabled(_OPTFALLBACK_DISABLED) || ALWAYS_STRIP_FALLBACK;
            if (isFallbackPass && isFallbackDisabled) {
                data.RemoveAt(i);
                continue;
            }

            // Strip outline
            bool isOutlinePass = snippet.passName == OUTLINE_PASS_NAME;
            bool isOutlineDisabled = !keys.IsEnabled(OUTLINE) && keys.IsEnabled(OUTLINE_PASS_DISABLED);
            if (isOutlinePass && isOutlineDisabled) {
                data.RemoveAt(i);
                continue;
            }
        }
    }

    public int callbackOrder { get { return 0; } }
}
