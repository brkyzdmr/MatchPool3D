//------------------------------------
//             OmniShade
//     Copyright© 2023 OmniShade     
//------------------------------------

// Mapping from Built-In to URP //////////////////////////////////////////////////////////
#if URP

// Vertex input structure
struct appdata_full {
	float4 vertex : POSITION;
	float4 tangent : TANGENT;
	float3 normal : NORMAL;
	float4 texcoord : TEXCOORD0;
	float4 texcoord1 : TEXCOORD1;
	float4 texcoord2 : TEXCOORD2;
	float4 texcoord3 : TEXCOORD3;
	half4 color : COLOR;
	UNITY_VERTEX_INPUT_INSTANCE_ID
};

// Function renames
#define _LightColor0 _MainLightColor
#define _WorldSpaceLightPos0 _MainLightPosition
#define UnityObjectToClipPos TransformObjectToHClip
#define UnityObjectToWorldNormal TransformObjectToWorldNormal
#define UnityObjectToWorldDir TransformObjectToWorldDir
#define ShadeSH9(x) SampleSH(x.rgb)
#define UnityMetaInput MetaInput
#define UnityMetaVertexPosition MetaVertexPosition
#define UnityMetaFragment MetaFragment
#define unity_LightData unity_LightData.rgb

// Macro definitions
#define UNITY_PI 3.14159265359f
#define UNITY_INITIALIZE_OUTPUT ZERO_INITIALIZE
#define UNITY_SAMPLE_TEX2D_SAMPLER(tex,samplertex,coord) tex.Sample (sampler##samplertex,coord)
#define UNITY_TRANSFER_FOG(o,outpos) o.fogCoord = ComputeFogFactor(outpos.z);
#define UNITY_APPLY_FOG(coord, col) col = MixFog(col, coord);
#define UNITY_LIGHT_ATTENUATION(atten, i, pos_world) \
	VertexPositionInputs vi = (VertexPositionInputs)0; \
	vi.positionWS = pos_world; \
	float4 shadowCoord = GetShadowCoord(vi); \
	half atten = MainLightRealtimeShadow(shadowCoord);
#define TRANSFER_SHADOW_CAST(o, v) o.pos = GetShadowPositionClip(v.vertex.xyz, v.normal);
#define UNITY_LIGHTDATA

// Functions missing from built-in
float3 UnityObjectToViewPos(float3 pos ) {
    return TransformWorldToView(TransformObjectToWorld(pos));
}

float3 WorldSpaceViewDir(float4 vertex) {
	float3 worldPos = mul(unity_ObjectToWorld, vertex).xyz;
	float3 camPos = float3(
		unity_CameraToWorld[0][3],
		unity_CameraToWorld[1][3],
		unity_CameraToWorld[2][3]
	);
	return camPos.xyz - worldPos;
}

half3 DecodeHDR (half4 data, half4 decodeInstructions) {
	half alpha = decodeInstructions.w * (data.a - 1.0) + 1.0;
	#if defined(UNITY_COLORSPACE_GAMMA)
		return (decodeInstructions.x * alpha) * data.rgb;
	#else
		#if defined(UNITY_USE_NATIVE_HDR)
			return decodeInstructions.x * data.rgb;
		#else
			return (decodeInstructions.x * pow(alpha, decodeInstructions.y)) * data.rgb;
		#endif
	#endif
}

half3 DecodeDirectionalLightmap (half3 color, half4 dirTex, half3 normalWorld) {
	half halfLambert = dot(normalWorld, dirTex.xyz - 0.5) + 0.5;
	return color * halfLambert / max(1e-4h, dirTex.w);
}

#if LIGHTMAP_ON || DIRLIGHTMAP_COMBINED
half3 SampleUnityLightmap(float2 uv) {
	return SampleSingleLightmap(
		TEXTURE2D_ARGS(unity_Lightmap, samplerunity_Lightmap), 
		uv,
		float4(1.0, 1.0, 0.0, 0.0),
		#if defined(UNITY_LIGHTMAP_FULL_HDR)
			false,
		#else
			true,
		#endif
		float4(LIGHTMAP_HDR_MULTIPLIER, LIGHTMAP_HDR_EXPONENT, 0.0, 0.0)
	);
}
#endif

#if SHADOW_CASTER
float4 GetShadowPositionClip(float3 vertex, float3 normal) {
	float3 positionWS = TransformObjectToWorld(vertex);
	float3 normalWS = TransformObjectToWorldNormal(normal);
	#if _CASTING_PUNCTUAL_LIGHT_SHADOW
		float3 lightDirectionWS = normalize(_LightPosition - positionWS);
	#else
		float3 lightDirectionWS = _LightDirection;
	#endif
	float4 positionCS = TransformWorldToHClip(ApplyShadowBias(positionWS, normalWS, lightDirectionWS));
	#if UNITY_REVERSED_Z
		positionCS.z = min(positionCS.z, UNITY_NEAR_CLIP_VALUE);
	#else
		positionCS.z = max(positionCS.z, UNITY_NEAR_CLIP_VALUE);
	#endif
	return positionCS;
}
#endif

half3 AdditionalLights(half3 pos_world, half3 nor_world, half _DiffuseWrap, half _DiffuseBrightness, half _DiffuseContrast, uint meshRenderingLayers) {
	half3 col_diffuse = 0;
	#if _ADDITIONAL_LIGHTS
		uint lightsCount = GetAdditionalLightsCount();
		for (uint lightIndex = 0u; lightIndex < lightsCount; ++lightIndex) {
			Light light = GetAdditionalLight(lightIndex, pos_world);
			#if (_LIGHT_LAYERS && UNITY_VERSION >= 202102)
				if (IsMatchingLightLayer(light.layerMask, meshRenderingLayers)) 
			#endif
			{
				half ndotl = max(0, dot(light.direction, nor_world));
				ndotl = max(0, lerp(ndotl, 1, _DiffuseWrap));			
				ndotl = pow(ndotl, _DiffuseContrast) * _DiffuseBrightness;
				half diff = ndotl * light.distanceAttenuation;
				col_diffuse += light.color * diff;
			}
		}
	#endif
	return col_diffuse;
}

uint GetMeshRenderingLightLayerCustom() {
    #if (_LIGHT_LAYERS && UNITY_VERSION >= 202102)
		return (asuint(unity_RenderingLayer.x) & RENDERING_LIGHT_LAYERS_MASK) >> RENDERING_LIGHT_LAYERS_MASK_SHIFT;
    #else
		return 0xFF;
    #endif
}

#else // Not URP - Built-in //////////////////////////////////////////////////////////////

#define TransformObjectToWorld(pos) mul(unity_ObjectToWorld, float4(pos, 1))
#define UNITY_LIGHTDATA half3 unity_LightData = half3(0, 0, dot(_WorldSpaceLightPos0, 1) != 0);
#define TRANSFER_SHADOW_CAST(o, v) o.pos = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal); o.pos = UnityApplyLinearShadowBias(o.pos);

half3 SampleUnityLightmap(float2 uv) {
	half4 color = UNITY_SAMPLE_TEX2D(unity_Lightmap, uv);
	return DecodeLightmap(color);
}

half3 AdditionalLights(half3 pos_world, half3 nor_world, half _DiffuseWrap, half _DiffuseBrightness, half _DiffuseContrast, uint meshRenderingLayers) {
	half3 col_diffuse = 0;
	#if VERTEXLIGHT_ON
		float4 lightPosX = unity_4LightPosX0;
		float4 lightPosY = unity_4LightPosY0;
		float4 lightPosZ = unity_4LightPosZ0;
		float4 lightAtten = unity_4LightAtten0;
		float4 toLightX = lightPosX - pos_world.x;
		float4 toLightY = lightPosY - pos_world.y;
		float4 toLightZ = lightPosZ - pos_world.z;
		float4 lengthSq = toLightX * toLightX;
		lengthSq += toLightY * toLightY;
		lengthSq += toLightZ * toLightZ;
		lengthSq = max(lengthSq, 0.000001);
		half4 ndotl = toLightX * nor_world.x;
		ndotl += toLightY * nor_world.y;
		ndotl += toLightZ * nor_world.z;
		ndotl = max(0, ndotl * rsqrt(lengthSq));
		ndotl = max(0, lerp(ndotl, 1, _DiffuseWrap));
		ndotl = pow(ndotl, _DiffuseContrast) * _DiffuseBrightness;
		float4 pointAtten = 1.0 / (1.0 + lengthSq * lightAtten);
		half4 diff = ndotl * pointAtten;
		col_diffuse += unity_LightColor[0].rgb * diff.x;
		col_diffuse += unity_LightColor[1].rgb * diff.y;
		col_diffuse += unity_LightColor[2].rgb * diff.z;
		col_diffuse += unity_LightColor[3].rgb * diff.w;
	#endif
	return col_diffuse;
}

uint GetMeshRenderingLightLayerCustom() {
	return 0xFF;
}

#endif
