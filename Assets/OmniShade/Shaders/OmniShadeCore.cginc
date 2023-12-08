//------------------------------------
//             OmniShade
//     Copyright© 2023 OmniShade
//------------------------------------

#include "OmniShadeURP.cginc"

//////////////////////////////////////////////////////////////////////////////////////////
// PROPERTY DECLARATIONS
//////////////////////////////////////////////////////////////////////////////////////////
CBUFFER_START(UnityPerMaterial)
half _TriplanarSharpness;

sampler2D _MainTex;
half4 _MainTex_ST;
half4 _Color;
half _Brightness;
half _Contrast;
half _Saturation;
half _IgnoreMainTexAlpha;

sampler2D _TopTex;
half4 _TopTex_ST;
half4 _TopColor;
half _TopBrightness;

half _DiffuseWrap;
half _DiffuseBrightness;
half _DiffuseContrast;

sampler2D _SpecularTex;
half4 _SpecularTex_ST;
half4 _SpecularColor;
half _SpecularBrightness;
half _SpecularSmoothness;

half4 _RimColor;
half _RimAmount;
half _RimContrast;
half _RimInverse;
half3 _RimDirection;

samplerCUBE _ReflectionTex;
half4 _ReflectionTex_HDR;
half _ReflectionAmount;
half4 _ReflectionColor;

sampler2D _NormalTex;
half4 _NormalTex_ST;
sampler2D _NormalTex2;
half4 _NormalTex2_ST;
half _NormalStrength;
sampler2D _NormalTopTex;
half4 _NormalTopTex_ST;

sampler2D _LightmapTex;
half4 _LightmapTex_ST;
half4 _LightmapTex_HDR;
half4 _LightmapColor;
half _LightmapBrightness;

half4 _Emissive;
sampler2D _EmissiveTex;
half4 _EmissiveTex_ST;

sampler2D _MatCapTex;
half4 _MatCapTex_ST;
half4 _MatCapColor;
half _MatCapBrightness;
half _MatCapContrast;
half3 _MatCapRot;

half _VertexColorsAmount;
half _VertexColorsContrast;

sampler2D _DetailTex;
half4 _DetailTex_ST;
half4 _DetailColor;
half _DetailBrightness;
half _DetailContrast;

sampler2D _Layer1Tex;
half4 _Layer1Tex_ST;
half4 _Layer1Color;
half _Layer1Brightness;
half _Layer1Alpha;
half _Layer1VertexColor;

sampler2D _Layer2Tex;
half4 _Layer2Tex_ST;
half4 _Layer2Color;
half _Layer2Brightness;
half _Layer2Alpha;
half _Layer2VertexColor;

sampler2D _Layer3Tex;
half4 _Layer3Tex_ST;
half4 _Layer3Color;
half _Layer3Brightness;
half _Layer3Alpha;
half _Layer3VertexColor;

sampler2D _TransparencyMaskTex;
half4 _TransparencyMaskTex_ST;
half _TransparencyMaskAmount;
half _TransparencyMaskContrast;

half4 _HeightColorsColor;
half _HeightColorsAlpha;
half _HeightColorsHeight;
half _HeightColorsEdgeThickness;
half _HeightColorsThickness;
half _HeightColorsSpace;
sampler2D _HeightColorsTex;
half4 _HeightColorsTex_ST;

sampler2D _ShadowOverlayTex;
half4 _ShadowOverlayTex_ST;
half _ShadowOverlayBrightness;
half _ShadowOverlaySpeedU;
half _ShadowOverlaySpeedV;
half _ShadowOverlaySwayAmount;

half _PlantSwayAmount;
half _PlantSwaySpeed;
half _PlantPhaseVariation;
half _PlantBaseHeight;

float _OutlineWidth;
half4 _OutlineColor;

half _AnimeThreshold1;
half4 _AnimeColor1;
half _AnimeThreshold2;
half4 _AnimeColor2;
half4 _AnimeColor3;
half _AnimeSoftness;

half _CameraFadeStart;
half _CameraFadeEnd;

half _AmbientBrightness;

half4 _ShadowColor;

half _ZOffset;
CBUFFER_END

//////////////////////////////////////////////////////////////////////////////////////////
// VERTEX STRUCTURE OUTPUT
//////////////////////////////////////////////////////////////////////////////////////////
#if !TRIPLANAR
	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		#if VERTEX_COLORS || LAYER1 || LAYER2 || LAYER3 || DETAIL_VERTEX_COLORS
			half4 color : COLOR;
		#endif
		float3 pos_world : TEXCOORD1;
		float3 nor_world : TEXCOORD2;
		#if SPECULAR && !(NORMAL_MAP || NORMAL_MAP2) && !DIRLIGHTMAP_COMBINED && !FLAT
			float3 specReflectDir : TEXCOORD3;
		#endif
		#if RIM || SPECULAR || REFLECTION
			float3 viewDir_world : TEXCOORD4;
		#endif
		#if MATCAP
			#if MATCAP_STATIC
				float3 nor_view : TEXCOORD5;
			#elif MATCAP_PERSPECTIVE
				float3 viewDir_view : TEXCOORD5;
			#endif
		#endif
		#if REFLECTION && !(NORMAL_MAP || NORMAL_MAP2) && !FLAT
			float3 viewReflectDir : TEXCOORD6;
		#endif
		#if (NORMAL_MAP || NORMAL_MAP2) && ((DIFFUSE && (!LIGHTMAP_ON || MIXED_LIGHTING)) || RIM || SPECULAR || MATCAP || DIRLIGHTMAP_COMBINED || REFLECTION) \
			|| SPECULAR_HAIR
			float4 tan_world : TEXCOORD7;
		#endif
		#if HEIGHT_COLORS
			half heightColorsHeight : TEXCOORD8;
		#endif
		#if SHADOW_OVERLAY
			float2 shadowOverlayUV : TEXCOORD9;
		#endif
		#if FOG
			#if FALLBACK_PASS
				float fogCoord : TEXCOORD7;
			#else
				float fogCoord : TEXCOORD10;
			#endif
		#endif
		#if DIFFUSE && !DIFFUSE_PER_PIXEL && !FLAT && (!LIGHTMAP_ON || MIXED_LIGHTING) && (VERTEXLIGHT_ON || _ADDITIONAL_LIGHTS)
			half3 col_diffuse_add : TEXCOORD11;
		#endif
		#if CAMERA_FADE
			half fade : TEXCOORD12;
		#endif
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING) && SHADOWS_ENABLED
			UNITY_SHADOW_COORDS(13)
		#endif
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};
#else  // TRIPLANAR
	struct v2f {
		float4 pos : SV_POSITION;
		float4 uv : TEXCOORD0;
		#if VERTEX_COLORS || LAYER1 || LAYER2 || LAYER3 || DETAIL_VERTEX_COLORS
			half4 color : COLOR;
		#endif
		float3 pos_world : TEXCOORD1;
		float3 nor_world : TEXCOORD2;
		#if SPECULAR && !(NORMAL_MAP || NORMAL_MAP_TOP) && !DIRLIGHTMAP_COMBINED && !FLAT
			float3 specReflectDir : TEXCOORD3;
		#endif
		#if RIM || SPECULAR || REFLECTION
			float3 viewDir_world : TEXCOORD4;
		#endif
		#if MATCAP
			#if MATCAP_STATIC
				float3 nor_view : TEXCOORD5;
			#elif MATCAP_PERSPECTIVE
				float3 viewDir_view : TEXCOORD5;
			#endif
		#endif
		#if REFLECTION && !(NORMAL_MAP || NORMAL_MAP_TOP) && !FLAT
			float3 viewReflectDir : TEXCOORD6;
		#endif
		#if HEIGHT_COLORS
			half heightColorsHeight : TEXCOORD7;
		#endif
		#if SHADOW_OVERLAY
			float2 shadowOverlayUV : TEXCOORD8;
		#endif
		#if FOG
			#if FALLBACK_PASS
				float fogCoord : TEXCOORD7;
			#else
				float fogCoord : TEXCOORD9;
			#endif
		#endif
		#if DIFFUSE && !DIFFUSE_PER_PIXEL && !FLAT && (!LIGHTMAP_ON || MIXED_LIGHTING) && (VERTEXLIGHT_ON || _ADDITIONAL_LIGHTS)
			half3 col_diffuse_add : TEXCOORD10;
		#endif
		#if SPECULAR_HAIR
			float3 tan_world : TEXCOORD11;
		#endif
		#if CAMERA_FADE
			half fade : TEXCOORD12;
		#endif
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING) && SHADOWS_ENABLED
			UNITY_SHADOW_COORDS(13)
		#endif
		UNITY_VERTEX_INPUT_INSTANCE_ID
		UNITY_VERTEX_OUTPUT_STEREO
	};
#endif

//////////////////////////////////////////////////////////////////////////////////////////
// FUNCTIONS
//////////////////////////////////////////////////////////////////////////////////////////
float3 PlantSway(appdata_full v) {
	float3 pos = v.vertex.xyz;
	#if PLANT_SWAY
		// Semi-random phase based on world position of this mesh
		float3 worldPosCenter = TransformObjectToWorld(float3(0, 0, 0)).xyz;
		half posBasedRandom = dot(worldPosCenter, 1);
		half phase = _PlantPhaseVariation * posBasedRandom;

		// Base sway amount
		#if _PLANTTYPE_LEAF					// Constant for leaves
			half swayAmount = 0.15;
		#elif _PLANTTYPE_PLANT				// Based on height for plants
			half swayAmount = max(0, v.vertex.y - _PlantBaseHeight);
		#elif _PLANTTYPE_VERTEX_COLOR_ALPHA	// Based on alpha
			half swayAmount = 0.15 * v.color.a;
		#endif
		swayAmount *= _PlantSwayAmount;

		// Sway XZ positions
		// Sway Y at double-speed and sync phase to match XY
		half swayXZ = sin(_PlantSwaySpeed * _Time.y + phase) * swayAmount;
		pos.xz += swayXZ;
		pos.y += -(sin(_PlantSwaySpeed * _Time.z + phase*2 - UNITY_PI/2) * 0.5 + 0.5) * swayAmount * 0.5;
	#endif
	return pos;
}

half4 ColorBrightness(half4 base, half brightness, half4 color) {
	base.rgb *= brightness;
	return base * color;
}

float3 TangentSpaceNormalToWorldSpaceNormal(float3 nor_world, float4 tan_world, float3 nor_tan) {
	float3 bin_world = cross(nor_world, tan_world.xyz) * tan_world.w;
	float3 ts0 = float3(tan_world.x, bin_world.x, nor_world.x);
	float3 ts1 = float3(tan_world.y, bin_world.y, nor_world.y);
	float3 ts2 = float3(tan_world.z, bin_world.z, nor_world.z);
	return normalize(float3(dot(ts0, nor_tan), dot(ts1, nor_tan), dot(ts2, nor_tan)));
}

bool IsLitMainLight(float3 unityLightData, uint meshRenderingLayers, bool isSpecular) {
	// Specular is on for baked directional lighting
	#if DIRLIGHTMAP_COMBINED
		if (isSpecular)
			return true;
	#endif

	// Fix for URP mixed lighting
	#if URP && LIGHTMAP_ON
		return false;
	#endif

	bool cullingMask = unityLightData.z == 1;
	#if URP && (_LIGHT_LAYERS && UNITY_VERSION >= 202102)
		bool layerMask = IsMatchingLightLayer(_MainLightLayerMask, meshRenderingLayers);
		return cullingMask && layerMask;
	#else
		return cullingMask;
	#endif
}

// BLENDING FUNCTIONS ////////////////////////////////////////////////////////////////////////
half3 BlendDetail(half3 col, half3 col_blend, float blendAlpha) {
	#if _DETAILBLEND_ALPHA_BLEND
		col = lerp(col, col_blend, blendAlpha);
	#else
		half3 col_blend_alphaed = col_blend * blendAlpha;
		#if _DETAILBLEND_ADDITIVE
			col += col_blend_alphaed;
		#elif _DETAILBLEND_MULTIPLY
			col *= col_blend_alphaed;
		#elif _DETAILBLEND_MULTIPLY_LIGHTEN
			col *= 1 + col_blend_alphaed;
		#endif
	#endif
	return col;
}

half3 BlendMatCap(half3 col, half3 col_blend) {
	#if _MATCAPBLEND_MULTIPLY
		col *= col_blend;
	#elif _MATCAPBLEND_MULTIPLY_LIGHTEN
		col *= 1 + col_blend;
	#endif
	return col;
}

half3 BlendRim(half3 col, half3 col_blend, float blendAlpha) {
	#if _RIMBLEND_ALPHA_BLEND
		col = lerp(col, col_blend, blendAlpha);
	#else
		half3 col_blend_alphaed = col_blend * blendAlpha;
		#if _RIMBLEND_ADDITIVE
			col += col_blend_alphaed;
		#elif _RIMBLEND_MULTIPLY
			col *= col_blend_alphaed;
		#elif _RIMBLEND_MULTIPLY_LIGHTEN
			col *= 1 + col_blend_alphaed;
		#endif
	#endif
	return col;
}

half4 BlendLayer1(half4 col, half4 col_blend, float blendAlpha) {
	#if _LAYER1BLEND_ALPHA_BLEND
		col = lerp(col, col_blend, blendAlpha);
	#else
		half3 col_blend_alphaed = col_blend.rgb * blendAlpha;
		#if _LAYER1BLEND_ADDITIVE
			col.rgb += col_blend_alphaed;
		#elif _LAYER1BLEND_MULTIPLY
			col.rgb *= col_blend_alphaed;
		#elif _LAYER1BLEND_MULTIPLY_LIGHTEN
			col.rgb *= 1 + col_blend_alphaed;
		#endif
	#endif
	return col;
}

half4 BlendLayer2(half4 col, half4 col_blend, float blendAlpha) {
	#if _LAYER2BLEND_ALPHA_BLEND
		col = lerp(col, col_blend, blendAlpha);
	#else
		half3 col_blend_alphaed = col_blend.rgb * blendAlpha;
		#if _LAYER2BLEND_ADDITIVE
			col.rgb += col_blend_alphaed;
		#elif _LAYER2BLEND_MULTIPLY
			col.rgb *= col_blend_alphaed;
		#elif _LAYER2BLEND_MULTIPLY_LIGHTEN
			col.rgb *= 1 + col_blend_alphaed;
		#endif
	#endif
	return col;
}

half4 BlendLayer3(half4 col, half4 col_blend, float blendAlpha) {
	#if _LAYER3BLEND_ALPHA_BLEND
		col = lerp(col, col_blend, blendAlpha);
	#else
		half3 col_blend_alphaed = col_blend.rgb * blendAlpha;
		#if _LAYER3BLEND_ADDITIVE
			col.rgb += col_blend_alphaed;
		#elif _LAYER3BLEND_MULTIPLY
			col.rgb *= col_blend_alphaed;
		#elif _LAYER3BLEND_MULTIPLY_LIGHTEN
			col.rgb *= 1 + col_blend_alphaed;
		#endif
	#endif
	return col;
}

half4 BlendHeightColors(half4 col, half4 col_blend, float blendAlpha) {
	#if _HEIGHTCOLORSBLEND_ALPHA_BLEND || _HEIGHTCOLORSBLEND_LIT
		col = lerp(col, col_blend, blendAlpha);
	#else
		half3 col_blend_alphaed = col_blend.rgb * blendAlpha;
		#if _HEIGHTCOLORSBLEND_ADDITIVE
			col.rgb += col_blend_alphaed;
		#endif
	#endif
	return col;
}

// LAYER FUNCTIONS ////////////////////////////////////////////////////////////////////////
#if LAYER1
	half4 Layer1(half4 col, float2 uv, half4 vertexColor, float2 uvX, float2 uvY, float2 uvZ, half4 blend) {
		#if !TRIPLANAR
			half4 layer = tex2D(_Layer1Tex, TRANSFORM_TEX(uv, _Layer1Tex));
		#else
			half4 layerX = tex2D(_Layer1Tex, TRANSFORM_TEX(uvX, _Layer1Tex));
			half4 layerY = tex2D(_Layer1Tex, TRANSFORM_TEX(uvY, _Layer1Tex));
			half4 layerZ = tex2D(_Layer1Tex, TRANSFORM_TEX(uvZ, _Layer1Tex));
			half4 layer = layerX * blend.x + layerY * (blend.y + blend.w) + layerZ * blend.z;
		#endif
		layer = ColorBrightness(layer, _Layer1Brightness, _Layer1Color);
		half mask = layer.a;
		mask *= _Layer1VertexColor == 0 ? 1 : vertexColor.r;
		mask = min(1, mask * _Layer1Alpha);
		return BlendLayer1(col, layer, mask);
	}
#endif

#if LAYER2
	half4 Layer2(half4 col, float2 uv, half4 vertexColor, float2 uvX, float2 uvY, float2 uvZ, half4 blend) {
		#if !TRIPLANAR
			half4 layer = tex2D(_Layer2Tex, TRANSFORM_TEX(uv, _Layer2Tex));
		#else
			half4 layerX = tex2D(_Layer2Tex, TRANSFORM_TEX(uvX, _Layer2Tex));
			half4 layerY = tex2D(_Layer2Tex, TRANSFORM_TEX(uvY, _Layer2Tex));
			half4 layerZ = tex2D(_Layer2Tex, TRANSFORM_TEX(uvZ, _Layer2Tex));
			half4 layer = layerX * blend.x + layerY * (blend.y + blend.w) + layerZ * blend.z;
		#endif
		layer = ColorBrightness(layer, _Layer2Brightness, _Layer2Color);
		half mask = layer.a;
		mask *= _Layer2VertexColor == 0 ? 1 : vertexColor.g;
		mask = min(1, mask * _Layer2Alpha);
		return BlendLayer2(col, layer, mask);
	}
#endif

#if LAYER3
	half4 Layer3(half4 col, float2 uv, half4 vertexColor, float2 uvX, float2 uvY, float2 uvZ, half4 blend) {
		#if !TRIPLANAR
			half4 layer = tex2D(_Layer3Tex, TRANSFORM_TEX(uv, _Layer3Tex));
		#else
			half4 layerX = tex2D(_Layer3Tex, TRANSFORM_TEX(uvX, _Layer3Tex));
			half4 layerY = tex2D(_Layer3Tex, TRANSFORM_TEX(uvY, _Layer3Tex));
			half4 layerZ = tex2D(_Layer3Tex, TRANSFORM_TEX(uvZ, _Layer3Tex));
			half4 layer = layerX * blend.x + layerY * (blend.y + blend.w) + layerZ * blend.z;
		#endif
		layer = ColorBrightness(layer, _Layer3Brightness, _Layer3Color);
		half mask = layer.a;
		mask *= _Layer3VertexColor == 0 ? 1 : vertexColor.b;
		mask = min(1, mask * _Layer3Alpha);
		return BlendLayer3(col, layer, mask);
	}
#endif

#if HEIGHT_COLORS
	half4 HeightColors(half4 col, half waterHeight, float2 uv) {
		half4 heightCol = 1;
		#if HEIGHT_COLORS_TEX
			heightCol = tex2D(_HeightColorsTex, TRANSFORM_TEX(uv, _HeightColorsTex));
		#endif
		heightCol *= _HeightColorsColor;
		half mask = 1 - saturate((abs(waterHeight) - _HeightColorsThickness) / _HeightColorsEdgeThickness);
		mask = min(1, mask * _HeightColorsAlpha);
		return BlendHeightColors(col, heightCol, mask);
	}
#endif

//////////////////////////////////////////////////////////////////////////////////////////
// VERTEX SHADER
//////////////////////////////////////////////////////////////////////////////////////////
v2f vert (appdata_full v) {
	v2f o;
	UNITY_INITIALIZE_OUTPUT(v2f, o)
	UNITY_SETUP_INSTANCE_ID(v);
	UNITY_TRANSFER_INSTANCE_ID(v, o);
	UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

	// Vertex manipulations
	#if OUTLINE_PASS
		#if OUTLINE
			v.vertex.xyz += _OutlineWidth * normalize(v.normal);
		#else
			o.pos = 0;
			return o;
		#endif
	#endif
	#if !TRIPLANAR
		v.vertex.xyz = PlantSway(v);
		o.pos_world = TransformObjectToWorld(v.vertex.xyz);
	#else  // TRIPLANAR Sway after pos_world and nor_world are set to avoid shifting texture
		o.pos_world = TransformObjectToWorld(v.vertex.xyz);
		v.vertex.xyz = PlantSway(v);
	#endif

	// Clip pos
	#if ZOFFSET || CAMERA_FADE
		float3 pos_view = UnityObjectToViewPos(v.vertex.xyz);
	#endif
	#if ZOFFSET
		pos_view.z += _ZOffset;
		o.pos = mul(UNITY_MATRIX_P, float4(pos_view, 1));
	#else
		o.pos = UnityObjectToClipPos(v.vertex.xyz);
	#endif
	#if CAMERA_FADE
		o.fade = saturate((-pos_view.z - _CameraFadeStart) / (_CameraFadeEnd - _CameraFadeStart));
	#endif

	// Fragment data
	o.uv.xy = v.texcoord.xy;
	o.uv.zw = v.texcoord1.xy;
	#if VERTEX_COLORS || LAYER1 || LAYER2 || LAYER3 || DETAIL_VERTEX_COLORS
		o.color = v.color;
	#endif
	o.nor_world = UnityObjectToWorldNormal(v.normal);
	#if (!TRIPLANAR && \
			(NORMAL_MAP || NORMAL_MAP2) && ((DIFFUSE && (!LIGHTMAP_ON || MIXED_LIGHTING)) || RIM || SPECULAR || MATCAP || DIRLIGHTMAP_COMBINED || REFLECTION)) \
		|| SPECULAR_HAIR
			o.tan_world = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w);
	#endif
	#if DIFFUSE && !DIFFUSE_PER_PIXEL && !FLAT && (!LIGHTMAP_ON || MIXED_LIGHTING) && (VERTEXLIGHT_ON || _ADDITIONAL_LIGHTS)
		uint meshRenderingLayers = GetMeshRenderingLightLayerCustom();
		o.col_diffuse_add = AdditionalLights(o.pos_world, o.nor_world, _DiffuseWrap, _DiffuseBrightness, _DiffuseContrast, meshRenderingLayers);
	#endif
	#if SPECULAR && !(NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) && !DIRLIGHTMAP_COMBINED && !FLAT && !SPECULAR_HAIR
		o.specReflectDir = normalize(reflect(-_WorldSpaceLightPos0.rgb, o.nor_world));
	#endif
	#if RIM || SPECULAR || REFLECTION
		o.viewDir_world = WorldSpaceViewDir(v.vertex);
	#endif
	#if MATCAP
		#if MATCAP_STATIC
			float3 worldNorm = UnityObjectToWorldNormal(v.normal);
			float3x3 viewMat = ((float3x3)UNITY_MATRIX_V);
			// Remove view rotation
			viewMat[0] = float3(1, 0, 0);
			viewMat[1] = float3(0, 1, 0);
			viewMat[2] = float3(0, 0, 1);
			// Add custom direction rotation
			float3 cosRot = cos(_MatCapRot);
			float3 sinRot = sin(_MatCapRot);
			float3x3 rotX = float3x3(1, 0, 0, 0, cosRot.x, -sinRot.x, 0, sinRot.x, cosRot.x);
			float3x3 rotY = float3x3(cosRot.y, 0, sinRot.y,	0, 1, 0, -sinRot.y, 0, cosRot.y);
			float3x3 rotZ = float3x3(cosRot.z, -sinRot.z, 0, sinRot.z, cosRot.z, 0, 0, 0, 1);
			worldNorm = mul(rotX, mul(rotY, mul(rotZ, worldNorm)));
			o.nor_view = mul(viewMat, worldNorm);
		#elif MATCAP_PERSPECTIVE
			o.viewDir_view = UnityObjectToViewPos(v.vertex.xyz);
		#endif
	#endif
	#if REFLECTION && !(NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) && !FLAT
		o.viewReflectDir = reflect(-o.viewDir_world, o.nor_world);
	#endif
	#if HEIGHT_COLORS  // World or local space height
		o.heightColorsHeight = _HeightColorsSpace == 0 ? o.pos_world.y : v.vertex.y;
		o.heightColorsHeight -= _HeightColorsHeight;
	#endif
	#if SHADOW_OVERLAY
		o.shadowOverlayUV = float2(o.pos_world.x, o.pos_world.z) / 80;
		float2 shadowAnim = (_Time.x % 60) * float2(_ShadowOverlaySpeedU, _ShadowOverlaySpeedV);
		#if _SHADOWOVERLAYANIMATION_SWAY
			shadowAnim = sin(shadowAnim * 500) * _ShadowOverlaySwayAmount;
		#endif
		o.shadowOverlayUV += shadowAnim;
		o.shadowOverlayUV *= 2;
	#endif

	// Unity system
	#if FOG
		UNITY_TRANSFER_FOG(o, o.pos);
	#endif
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING) && SHADOWS_ENABLED
		UNITY_TRANSFER_SHADOW(o, o.uv.zw);
	#endif
	#if SHADOW_CASTER
		TRANSFER_SHADOW_CAST(o, v)
	#endif
	#if META
		o.pos = UnityMetaVertexPosition(v.vertex, v.texcoord1.xy, v.texcoord2.xy, unity_LightmapST, unity_DynamicLightmapST);
	#endif

	return o;
}

//////////////////////////////////////////////////////////////////////////////////////////
// FRAGMENT SHADER
//////////////////////////////////////////////////////////////////////////////////////////
half4 frag (v2f i) : COLOR {
	float2 uv = i.uv.xy;
	float2 uv2 = i.uv.zw;
	UNITY_SETUP_INSTANCE_ID(i);
	UNITY_LIGHTDATA
	#if META
		UnityMetaInput meta;
		UNITY_INITIALIZE_OUTPUT(UnityMetaInput, meta);
	#endif

	#if TRIPLANAR  // Scale UVs to more reasonable size for world texturing
		float TEX_SCALE = 32;
		#if !TRIPLANAR_BASE_UV
			_MainTex_ST /= TEX_SCALE;
			#if NORMAL_MAP
				_NormalTex_ST /= TEX_SCALE;
			#endif
			#if DETAIL
				_DetailTex_ST /= TEX_SCALE;
			#endif
			#if LAYER1
				_Layer1Tex_ST /= TEX_SCALE;
			#endif
			#if LAYER2
				_Layer2Tex_ST /= TEX_SCALE;
			#endif
			#if LAYER3
				_Layer3Tex_ST /= TEX_SCALE;
			#endif
		#endif
		#if TOP_TEX
			_TopTex_ST /= TEX_SCALE;
		#endif
		#if NORMAL_MAP_TOP
			_NormalTopTex_ST /= TEX_SCALE;
		#endif
	#endif

	// Normalize vectors
	#if FLAT
		i.nor_world = cross(ddy(i.pos_world), ddx(i.pos_world)) * -_ProjectionParams.x;;
	#endif
	i.nor_world = normalize(i.nor_world);
	#if (!TRIPLANAR && \
			(NORMAL_MAP || NORMAL_MAP2) && ((DIFFUSE && (!LIGHTMAP_ON || MIXED_LIGHTING)) || RIM || SPECULAR || MATCAP || DIRLIGHTMAP_COMBINED || REFLECTION)) \
		|| SPECULAR_HAIR
			i.tan_world.xyz = normalize(i.tan_world.xyz);
	#endif
	#if SPECULAR && !(NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) && !DIRLIGHTMAP_COMBINED && !FLAT && !SPECULAR_HAIR
		i.specReflectDir = normalize(i.specReflectDir);
	#endif
	#if RIM || SPECULAR || (REFLECTION && ((NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) || FLAT))
		i.viewDir_world = normalize(i.viewDir_world);
	#endif
	#if MATCAP && (!MATCAP_STATIC && MATCAP_PERSPECTIVE)
		i.viewDir_view = normalize(i.viewDir_view);
	#endif
	#if REFLECTION && !(NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) && !FLAT
		i.viewReflectDir = normalize(i.viewReflectDir);
	#endif

	#if TRIPLANAR  // Calculate blend weights
		#if TRIPLANAR_BASE_UV
			float2 uvX = uv;
			float2 uvZ = uv;
		#else
			float2 uvX = i.pos_world.zy;
			float2 uvZ = i.pos_world.xy;
		#endif
		float2 uvY = i.pos_world.xz;
		float4 blend;
		blend.xyz = i.nor_world;
		blend.xz = abs(i.nor_world.xz);
		blend.y = max(0, i.nor_world.y);
		blend.w = -min(0, i.nor_world.y);
		#if TRIPLANAR_SHARPNESS
			blend = pow(blend, _TriplanarSharpness);
		#endif
		blend /= (blend.x + blend.y + blend.z + blend.w);
	#elif LAYER1 || LAYER2 || LAYER3
		float2 uvX = 0, uvY = 0, uvZ = 0;
		half4 blend = 0;
	#endif

	// Calculate intermediate vectors
	uint meshRenderingLayers = GetMeshRenderingLightLayerCustom();
	#if !TRIPLANAR
		#if (NORMAL_MAP || NORMAL_MAP2)	// Calculate tangent-space normal
			float2 normalUV = uv;
			#if _NORMALUV_UV2
				normalUV = uv2;
			#endif
			float3 nor_tan = 0;
			#if NORMAL_MAP
				nor_tan = UnpackNormal(tex2D(_NormalTex, TRANSFORM_TEX(normalUV, _NormalTex)));
			#endif
			#if NORMAL_MAP2
				nor_tan += UnpackNormal(tex2D(_NormalTex2, TRANSFORM_TEX(normalUV, _NormalTex2)));
			#endif
			nor_tan.xy *= _NormalStrength;
			nor_tan = normalize(nor_tan);
		#else
			float3 nor_tan = float3(0, 0, 1);
		#endif
		#if (NORMAL_MAP || NORMAL_MAP2) && ((DIFFUSE && (!LIGHTMAP_ON || MIXED_LIGHTING)) || RIM || SPECULAR || MATCAP || DIRLIGHTMAP_COMBINED || REFLECTION)
			float3 bumpNor_world = TangentSpaceNormalToWorldSpaceNormal(i.nor_world, i.tan_world, nor_tan);
		#endif
	#else
		#if (NORMAL_MAP || NORMAL_MAP_TOP)
			#if NORMAL_MAP
				float3 norX = UnpackNormal(tex2D(_NormalTex, TRANSFORM_TEX(uvX, _NormalTex))) * _NormalStrength;
				float3 norYbottom = UnpackNormal(tex2D(_NormalTex, TRANSFORM_TEX(uvY, _NormalTex))) * _NormalStrength;
				float3 norZ = UnpackNormal(tex2D(_NormalTex, TRANSFORM_TEX(uvZ, _NormalTex))) * _NormalStrength;
			#else  // Not using normal map - just use vector pointing outwards
				float3 norX = float3(0, 0, 1);
				float3 norYbottom = float3(0, 0, 1);
				float3 norZ = float3(0, 0, 1);
			#endif
			norX = float3(norX.xy + i.nor_world.zy, i.nor_world.x);
			norYbottom = float3(norYbottom.xy + i.nor_world.xz, i.nor_world.y);
			norZ = float3(norZ.xy + i.nor_world.xy, i.nor_world.z);
			#if NORMAL_MAP_TOP
				float3 norYtop = UnpackNormal(tex2D(_NormalTopTex, TRANSFORM_TEX(uvY, _NormalTopTex))) * _NormalStrength;
				norYtop = float3(norYtop.xy + i.nor_world.xz, i.nor_world.y);
			#else
				float3 norYtop = norYbottom;
			#endif
			#if (DIFFUSE && (!LIGHTMAP_ON || MIXED_LIGHTING)) || RIM || SPECULAR || MATCAP || DIRLIGHTMAP_COMBINED || REFLECTION
				float3 bumpNor_world = normalize(norX.zyx * blend.x + norYtop.xzy * blend.y + norZ.xyz * blend.z + norYbottom * blend.w);
			#endif
		#endif
	#endif
	#if (DIFFUSE && (!LIGHTMAP_ON || MIXED_LIGHTING)) || RIM || SPECULAR || MATCAP || REFLECTION
		#if (NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP)
			float3 adjNor_world = bumpNor_world;
		#else  // Not using normal map - just use nor_world from vertex shader
			float3 adjNor_world = i.nor_world;
		#endif
	#endif
	#if LIGHT_MAP  // Lightmap UV
		float2 lightmapUV = uv;
		#if _LIGHTMAPUV_UV2
			lightmapUV = uv2;
		#endif
		lightmapUV = TRANSFORM_TEX(lightmapUV, _LightmapTex);
	#endif
	#if LIGHTMAP_ON || DIRLIGHTMAP_COMBINED
		float2 lightmapUnityUV = uv2 * unity_LightmapST.xy + unity_LightmapST.zw;
	#endif
	#if ((NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) || SPECULAR) && DIRLIGHTMAP_COMBINED
		half4 lightmapDir = UNITY_SAMPLE_TEX2D_SAMPLER(unity_LightmapInd, unity_Lightmap, lightmapUnityUV);
	#endif
	#if SPECULAR_MAP && (SPECULAR || REFLECTION_SPECULAR)
		float2 specUV = uv;
		#if _SPECULARUV_UV2
			specUV = uv2;
		#endif
		half4 specTex = tex2D(_SpecularTex, TRANSFORM_TEX(specUV, _SpecularTex));
	#endif

	// BASE
	#if !TRIPLANAR
		float4 col_base = tex2D(_MainTex, TRANSFORM_TEX(uv, _MainTex));
		col_base.a = _IgnoreMainTexAlpha == 0 ? saturate(col_base.a) : 1;
		#if BASE_CONTRAST
			col_base.rgb = pow(col_base.rgb, _Contrast);
		#endif
		col_base = ColorBrightness(col_base, _Brightness, _Color);
	#else  // TRIPLANAR
		half4 colX = tex2D(_MainTex, TRANSFORM_TEX(uvX, _MainTex));
		#if TOP_TEX || TRIPLANAR_TOP
			half4 colYtop = tex2D(_TopTex, TRANSFORM_TEX(uvY, _TopTex));
		#else
			half4 colYtop = tex2D(_MainTex, TRANSFORM_TEX(uvY, _MainTex));
		#endif
		#if !(TOP_TEX || TRIPLANAR_TOP)  // Same as top
			half4 colYbottom = colYtop;
		#else
			half4 colYbottom = tex2D(_MainTex, TRANSFORM_TEX(uvY, _MainTex));
		#endif
		half4 colZ = tex2D(_MainTex, TRANSFORM_TEX(uvZ, _MainTex));
		float4 col_base = colX * blend.x + colZ * blend.z + colYbottom * blend.w;
		col_base = ColorBrightness(col_base, _Brightness, _Color);
		// BASE TOP
		half4 col_base_top = colYtop * blend.y;
		#if TRIPLANAR_TOP || TOP_TEX
			col_base_top = ColorBrightness(col_base_top, _TopBrightness, _TopColor);
		#else
			col_base_top = ColorBrightness(col_base_top, _Brightness, _Color);
		#endif
		col_base += col_base_top;
		col_base.a = _IgnoreMainTexAlpha == 0 ? saturate(col_base.a) : 1;
		#if BASE_CONTRAST  // Triplanar apply contrast after top added
			col_base.rgb = pow(col_base.rgb, _Contrast);
		#endif
	#endif
	#if VERTEX_COLORS
		#if VERTEX_COLORS_CONTRAST
			i.color = pow(i.color, _VertexColorsContrast);
		#endif
		col_base = lerp(col_base, col_base * i.color, _VertexColorsAmount);
	#endif
	#if DETAIL
		#if !TRIPLANAR
			float2 detailUV = uv;
			#if _DETAILUV_UV2
				detailUV = uv2;
			#endif
			half4 col_detail = tex2D(_DetailTex, TRANSFORM_TEX(detailUV, _DetailTex));
		#else
			half4 col_detailX = tex2D(_DetailTex, TRANSFORM_TEX(uvX, _DetailTex));
			half4 col_detailY = tex2D(_DetailTex, TRANSFORM_TEX(uvY, _DetailTex));
			half4 col_detailZ = tex2D(_DetailTex, TRANSFORM_TEX(uvZ, _DetailTex));
			half4 col_detail = col_detailX * blend.x + col_detailY * (blend.y + blend.w) + col_detailZ * blend.z;
		#endif
		#if DETAIL_CONTRAST
			col_detail.rgb = pow(col_detail.rgb, _DetailContrast);
		#endif
		col_detail = ColorBrightness(col_detail, _DetailBrightness, _DetailColor);
		#if DETAIL_VERTEX_COLORS
			col_detail.a *= i.color.a;
		#endif
		#if !DETAIL_LIGHTING
			col_base.rgb = BlendDetail(col_base.rgb, col_detail.rgb, col_detail.a);
		#endif
	#endif
	#if LAYER1
		col_base = Layer1(col_base, uv, i.color, uvX, uvY, uvZ, blend);
	#endif
	#if LAYER2
		col_base = Layer2(col_base, uv, i.color, uvX, uvY, uvZ, blend);
	#endif
	#if LAYER3
		col_base = Layer3(col_base, uv, i.color, uvX, uvY, uvZ, blend);
	#endif
	#if !TRIPLANAR
		#if TRANSPARENCY_MASK
			half4 maskColor = tex2D(_TransparencyMaskTex, TRANSFORM_TEX(uv, _TransparencyMaskTex));
			half mask = min(maskColor.r, maskColor.a);  // Supports either RGB or RGBA mask
			#if TRANSPARENCY_MASK_CONTRAST
				mask = pow(mask, _TransparencyMaskContrast);
			#endif
			half alphaMask = min(1, mask * _TransparencyMaskAmount);
			col_base.a *= alphaMask;
		#endif
	#endif
	#if HEIGHT_COLORS && _HEIGHTCOLORSBLEND_LIT
		col_base = HeightColors(col_base, i.heightColorsHeight, uv);
	#endif
	#if RIM
		half4 col_rim = _RimColor;
		half rim = max(0, dot(i.viewDir_world.xyz, adjNor_world));
		rim = _RimInverse == 0 ? 1 - rim : rim;
		rim = min(1, pow(rim, _RimContrast) * _RimAmount) * col_rim.a;
		#if RIM_DIRECTION
			rim *= max(0, dot(adjNor_world, normalize(_RimDirection)));
		#endif
		#if !REFLECTION_RIM && _RIMBLEND_TRANSPARENCY
			col_base.a *= 1 - rim;
		#endif
	#endif
	#if REFLECTION
		#if (NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) || FLAT
			float3 viewReflectDir = reflect(-i.viewDir_world, adjNor_world);
		#else
			float3 viewReflectDir = i.viewReflectDir;
		#endif
		#if REFLECTION_TEX
			half3 col_reflect = DecodeHDR(texCUBE(_ReflectionTex, viewReflectDir), _ReflectionTex_HDR);
		#else
			half3 col_reflect = DecodeHDR(unity_SpecCube0.Sample(samplerunity_SpecCube0, viewReflectDir), unity_SpecCube0_HDR);
		#endif
		col_reflect *= _ReflectionColor;
		half reflectAmount = _ReflectionAmount;
		#if REFLECTION_RIM && RIM  // Mask reflection with rim
			col_reflect *= col_rim.rgb;
			reflectAmount *= rim;
		#endif
		#if REFLECTION_SPECULAR && SPECULAR_MAP
			reflectAmount *= specTex.r;
		#endif
		col_base.rgb = lerp(col_base.rgb, col_reflect.rgb, reflectAmount);
	#endif
	#if CAMERA_FADE
		col_base.a *= i.fade;
	#endif

	// LIGHTING
	#if !ANIME
		half3 col = col_base.rgb;
	#else  // Anime lighting slightly less accurate, mostly multiplicative lighting
		half3 col = 1;
	#endif

	#if DIFFUSE && (!LIGHTMAP_ON || MIXED_LIGHTING)
		half3 col_diffuse = 0;
		if (IsLitMainLight(unity_LightData, meshRenderingLayers, false)) {
			float ndotl = max(0, dot(_WorldSpaceLightPos0.xyz, adjNor_world));
			ndotl = max(0, lerp(ndotl, 1, _DiffuseWrap));
			ndotl = pow(ndotl, _DiffuseContrast) * _DiffuseBrightness;
			col_diffuse = ndotl * _LightColor0.rgb;
		}
		#if !DIFFUSE_PER_PIXEL && !FLAT && (VERTEXLIGHT_ON || _ADDITIONAL_LIGHTS)
			col_diffuse += i.col_diffuse_add;
		#else
			col_diffuse += AdditionalLights(i.pos_world, adjNor_world, _DiffuseWrap, _DiffuseBrightness, _DiffuseContrast, meshRenderingLayers);
		#endif
		#if DETAIL && DETAIL_LIGHTING
			col_diffuse = BlendDetail(col_diffuse, col_detail.rgb, col_detail.a);
		#endif
		col *= col_diffuse;
	#endif
	#if LIGHT_MAP
		half3 col_light = tex2D(_LightmapTex, lightmapUV).rgb;
		col_light = max(0, 1 - _LightmapBrightness) + col_light * _LightmapBrightness;
		col_light *= _LightmapColor.rgb;
		col *= col_light;
	#endif
	#if LIGHTMAP_ON || DIRLIGHTMAP_COMBINED
		float3 col_light2 = SampleUnityLightmap(lightmapUnityUV);
		col_light2 = max(0, 1 - _LightmapBrightness) + col_light2 * _LightmapBrightness;
		col_light2 *= _LightmapColor.rgb;
		#if DIRLIGHTMAP_COMBINED && (NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP)
			col_light2 = DecodeDirectionalLightmap(col_light2, lightmapDir, bumpNor_world);
		#endif
		col *= col_light2;
	#endif
	#if MATCAP  // Calculate view-space normal
		#if MATCAP_STATIC  // Use simplified matcap calculation without normal map
			float2 nor_view = i.nor_view.xy;
		#else
			float3 nor_view = normalize(mul((float3x3)UNITY_MATRIX_V, adjNor_world));
			#if MATCAP_PERSPECTIVE
				float3 viewCross = cross(i.viewDir_view, nor_view);
				nor_view = float3(-viewCross.y, viewCross.x, 0.0);
			#endif
		#endif
		float2 matCapUV = nor_view.xy * 0.5 + 0.5;
		half3 col_matcap = tex2D(_MatCapTex, TRANSFORM_TEX(matCapUV, _MatCapTex)).rgb;
		#if MATCAP_CONTRAST
			col_matcap = pow(col_matcap, _MatCapContrast);
		#endif
		col_matcap = col_matcap * _MatCapColor.rgb * _MatCapBrightness;
		#if DETAIL && DETAIL_LIGHTING
			col_matcap = BlendDetail(col_matcap, col_detail.rgb, col_detail.a);
		#endif
		col = BlendMatCap(col.rgb, col_matcap);
	#endif
	#if SPECULAR
		if (IsLitMainLight(unity_LightData, meshRenderingLayers, true)) {
			#if !DIRLIGHTMAP_COMBINED
				float3 specLightDir = _WorldSpaceLightPos0.xyz;
			#else
				float3 specLightDir = (lightmapDir.xyz - 0.5) * 2;
				#if DIRLIGHTMAP_COMBINED
					specLightDir /= max(0.000001, lightmapDir.w);
				#endif
			#endif
			#if !TRIPLANAR && SPECULAR_MAP
				half specBrightness = _SpecularBrightness * specTex.r;
				half specSmoothness = max(1, _SpecularSmoothness * specTex.a);
				half shiftTex = specTex.r - 0.5;
			#else
				half specBrightness = _SpecularBrightness;
				half specSmoothness = _SpecularSmoothness;
				half shiftTex = 0;
			#endif
			#if !SPECULAR_HAIR
				#if (NORMAL_MAP || NORMAL_MAP2 || NORMAL_MAP_TOP) || DIRLIGHTMAP_COMBINED || FLAT
					float3 specReflectDir = reflect(-specLightDir, adjNor_world);
				#else
					float3 specReflectDir = i.specReflectDir;
				#endif
				half glare = max(0, dot(i.viewDir_world, specReflectDir));
			#else
				float3 tanShift = normalize(i.tan_world.xyz + adjNor_world * shiftTex);
				float3 halfVec = normalize(specLightDir + i.viewDir_world);
				float dotTH = dot(tanShift, halfVec);
				half glare = sqrt(1 - dotTH * dotTH);
			#endif
			glare = pow(glare, specSmoothness);
			#if SPECULAR_HAIR
				glare *= smoothstep(-1, 0, dot(specLightDir, adjNor_world));
			#endif
			#if LIGHTMAP_ON
				half3 specColor = col_light2;
			#else
				half3 specColor = _LightColor0.rgb;
			#endif
			if (dot(_SpecularColor.rgb, 1) != 3)  // Specular Color override
				specColor = _SpecularColor.rgb;
			half3 col_specular = glare * specBrightness * specColor;
			col += col_specular;
		}
	#endif
	#if RIM && !REFLECTION_RIM && !_RIMBLEND_TRANSPARENCY
		col = BlendRim(col, col_rim.rgb, rim);
	#endif

	// Shadows
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING || \
		_MAIN_LIGHT_SHADOWS || _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS_SCREEN) && SHADOWS_ENABLED
		if (IsLitMainLight(unity_LightData, meshRenderingLayers, false)) {
			UNITY_LIGHT_ATTENUATION(atten, i, i.pos_world)
			half3 col_shadow = lerp(_ShadowColor.rgb, 1, atten);
			col *= col_shadow;
		}
	#endif
	#if SHADOW_OVERLAY
		half4 col_shadowOverlay = tex2D(_ShadowOverlayTex, TRANSFORM_TEX(i.shadowOverlayUV, _ShadowOverlayTex));
		half shadowMask = min(col_shadowOverlay.r, col_shadowOverlay.a);
		shadowMask = saturate(shadowMask + (_ShadowOverlayBrightness - 1));
		half topFactor = max(0, dot(float3(0, 1, 0), i.nor_world));
		col *= lerp(1, shadowMask, topFactor);
	#endif

	#if ANIME
		half3 animeColor;
		half lum = Luminance(col);
		#if ANIME_SOFT
			half thresLower1 = _AnimeThreshold1 - _AnimeSoftness;
			half thresUpper1 = _AnimeThreshold1 + _AnimeSoftness;
			half thresLower2 = _AnimeThreshold2 - _AnimeSoftness;
			half thresUpper2 = _AnimeThreshold2 + _AnimeSoftness;
		#else
			half thresLower1 = _AnimeThreshold1;
			half thresLower2 = _AnimeThreshold2;
		#endif
		if (lum < thresLower1)
			animeColor = _AnimeColor1.rgb;
		#if ANIME_SOFT
			else if (lum < thresUpper1)
				animeColor = lerp(_AnimeColor1.rgb, _AnimeColor2.rgb, (lum - thresLower1) / (_AnimeSoftness * 2));
		#endif
		else if (lum < thresLower2)
			animeColor = _AnimeColor2.rgb;
		#if ANIME_SOFT
			else if (lum < thresUpper2)
				animeColor = lerp(_AnimeColor2.rgb, _AnimeColor3.rgb, (lum - thresLower2) / (_AnimeSoftness * 2));
		#endif
		else
			animeColor = _AnimeColor3.rgb;
		if (dot(col, 1) > 0.000001)
			animeColor *= normalize(col);
		else
			animeColor = 0;
		// Blend light to base
		col = animeColor * col_base.rgb;
	#endif

	// Additives
	half3 col_emissive = _Emissive.rgb;
	#if EMISSIVE_MAP
		col_emissive *= tex2D(_EmissiveTex, TRANSFORM_TEX(uv, _EmissiveTex)).rgb;
	#endif
	#if META
		meta.Emission = col_emissive;
	#else
		col += col_emissive;
	#endif
	#if AMBIENT && !LIGHTMAP_ON
		half3 col_ambient = ShadeSH9(float4(i.nor_world, 1.0)).rgb * _AmbientBrightness;
		col += col_base.rgb * col_ambient;
	#endif

	// Overlays
	#if HEIGHT_COLORS && !_HEIGHTCOLORSBLEND_LIT
		half4 heightCol = HeightColors(half4(col, col_base.a), i.heightColorsHeight, uv);
		col = heightCol.rgb;
		col_base.a = heightCol.a;
	#endif
	#if FOG
		UNITY_APPLY_FOG(i.fogCoord, col);
	#endif
	#if BASE_SATURATION
		half3 col_desat = Luminance(col);
		col = lerp(col_desat, col, _Saturation);
	#endif

	// Special-case return values
	#if CUTOUT
		if (col_base.a < 0.5)
			discard;
		else
			col_base.a = 1;
	#endif
	#if SHADOW_CASTER || DEPTH
		return 0;
	#endif
	#if OUTLINE_PASS
		#if OUTLINE
			return _OutlineColor;
		#else
			return 0;
		#endif
	#endif

	// Optimization Flags error check
	#if (SHADOWS_SCREEN || SHADOWS_SHADOWMASK || LIGHTMAP_SHADOW_MIXING || \
		_MAIN_LIGHT_SHADOWS || _MAIN_LIGHT_SHADOWS_CASCADE || _MAIN_LIGHT_SHADOWS_SCREEN) && SHADOWS_ENABLED
		#if _OPTSHADOW_DISABLED
			return half4(0, 0, 1, 1);		// Blue - Fog error
		#endif
	#endif
	#if (VERTEXLIGHT_ON || _ADDITIONAL_LIGHTS) && DIFFUSE
		#if _OPTPOINTLIGHTS_DISABLED
			return half4(1, 1, 0, 1);		// Yellow - Point Lights error
		#endif
	#endif
	#if (FOG_LINEAR || FOG_EXP || FOG_EXP2) && FOG
		#if _OPTFOG_DISABLED
			return half4(0.5, 0, 0.5, 1);	// Purple - Fog error
		#endif
	#endif
	#if (LIGHTMAP_ON || DIRLIGHTMAP_COMBINED)
		#if _OPTLIGHTMAPPING_DISABLED
			return half4(1, 0, 0, 1);		// Red - Lightmap error
		#endif
	#endif
	#if FALLBACK_PASS
		#if _OPTFALLBACK_DISABLED
			return half4(0.5, 0.5, 0.5, 1);	// Gray - Fallback error
		#endif
	#endif
	// End error check

	#if META
		meta.Albedo = col;
		return UnityMetaFragment(meta);
	#else
		return half4(col, col_base.a);
	#endif
}
