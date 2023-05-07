// Upgrade NOTE: upgraded instancing buffer 'NatureManufactureShadersIceShaderMetalicLakeTranslucency' to new syntax.

Shader "NatureManufacture Shaders/Ice Shader Metalic Lake Translucency"
{
	Properties
	{
		_RefractionPower("Refraction Power", Range( 0 , 1)) = 0.155
		[Header(Translucency)]
		_Translucency("Strength", Range( 0 , 50)) = 1
		_TransNormalDistortion("Normal Distortion", Range( 0 , 1)) = 0.1
		_TransScattering("Scaterring Falloff", Range( 1 , 50)) = 2
		_TransDirect("Direct", Range( 0 , 1)) = 1
		_TransAmbient("Ambient", Range( 0 , 1)) = 0.2
		_IceTexture("Ice Texture", 2D) = "white" {}
		_TransShadow("Shadow", Range( 0 , 1)) = 0.9
		_IceColorTop("Ice Color Top", Color) = (1,1,1,1)
		_IceColorBackground("Ice Color Background", Color) = (1,1,1,1)
		_BackGroundIceBlend("BackGround Ice Blend", Range( 0 , 1)) = 0.6
		_WaterDepth("Water Depth", Range( 0 , 5)) = 0.7
		_ShalowColor("Shalow Color", Color) = (1,1,1,0)
		_Smoothness("Smoothness", Range( 0 , 2)) = 0.85
		_MetalicPower("Metalic Power", Range( 0 , 1)) = 0.3
		_IceNormal("Ice Normal", 2D) = "bump" {}
		_IceNormalScale("Ice Normal Scale", Range( 0 , 2)) = 1
		_IceNoise("Ice Noise", 2D) = "black" {}
		_NoisePower("Noise Power", Range( 0 , 2)) = 1
		_IceDepth("Ice Depth", Range( 0 , 1)) = 0.057
		_IceAmountOpacity("Ice Amount (Opacity)", Range( 0 , 1)) = 1
		_WaterFalloff("Water Falloff", Range( 0 , 3)) = 0.8
		_FogColorMultiply("Fog Color Multiply", Color) = (1,1,1,1)
		_FogColorHardness("Fog Color Hardness", Range( 0.01 , 10)) = 0.15
		_FogColorDistance("Fog Color Distance", Range( 0.01 , 100000)) = 1000
		_ShadowPower("Shadow Power", Range( 0 , 5)) = 2
		_RefractionColor("Refraction Color", Color) = (0.1029411,0.1121703,0.117647,1)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Transparent+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		GrabPass{ "_GrabScreen1" }
		CGINCLUDE
		#include "UnityStandardUtils.cginc"
		#include "UnityCG.cginc"
		#include "UnityShaderVariables.cginc"
		#include "UnityPBSLighting.cginc"
		#include "Lighting.cginc"
		#pragma target 2.0
		#pragma multi_compile_instancing
		#ifdef UNITY_PASS_SHADOWCASTER
			#undef INTERNAL_DATA
			#undef WorldReflectionVector
			#undef WorldNormalVector
			#define INTERNAL_DATA half3 internalSurfaceTtoW0; half3 internalSurfaceTtoW1; half3 internalSurfaceTtoW2;
			#define WorldReflectionVector(data,normal) reflect (data.worldRefl, half3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal)))
			#define WorldNormalVector(data,normal) fixed3(dot(data.internalSurfaceTtoW0,normal), dot(data.internalSurfaceTtoW1,normal), dot(data.internalSurfaceTtoW2,normal))
		#endif
		struct Input
		{
			float2 uv_texcoord;
			fixed3 viewDir;
			INTERNAL_DATA
			fixed4 screenPos;
			float3 worldPos;
		};

		struct SurfaceOutputStandardCustom
		{
			fixed3 Albedo;
			fixed3 Normal;
			half3 Emission;
			half Metallic;
			half Smoothness;
			half Occlusion;
			fixed Alpha;
			fixed3 Translucency;
		};

		uniform fixed _IceNormalScale;
		uniform sampler2D _IceNormal;
		uniform float4 _IceNormal_ST;
		uniform fixed4 _ShalowColor;
		uniform sampler2D _IceTexture;
		uniform fixed _IceDepth;
		uniform float4 _IceTexture_ST;
		uniform fixed4 _IceColorBackground;
		uniform fixed _BackGroundIceBlend;
		uniform fixed4 _IceColorTop;
		uniform sampler2D _IceNoise;
		uniform float4 _IceNoise_ST;
		uniform fixed _NoisePower;
		uniform sampler2D _CameraDepthTexture;
		uniform fixed _WaterDepth;
		uniform fixed _WaterFalloff;
		uniform fixed4 _FogColorMultiply;
		uniform fixed _FogColorDistance;
		uniform fixed _FogColorHardness;
		uniform fixed _ShadowPower;
		uniform sampler2D _GrabScreen1;
		uniform fixed _RefractionPower;
		uniform half _Translucency;
		uniform half _TransNormalDistortion;
		uniform half _TransScattering;
		uniform half _TransDirect;
		uniform half _TransAmbient;
		uniform half _TransShadow;

		UNITY_INSTANCING_BUFFER_START(NatureManufactureShadersIceShaderMetalicLakeTranslucency)
			UNITY_DEFINE_INSTANCED_PROP(fixed4, _RefractionColor)
#define _RefractionColor_arr NatureManufactureShadersIceShaderMetalicLakeTranslucency
			UNITY_DEFINE_INSTANCED_PROP(fixed, _MetalicPower)
#define _MetalicPower_arr NatureManufactureShadersIceShaderMetalicLakeTranslucency
			UNITY_DEFINE_INSTANCED_PROP(fixed, _Smoothness)
#define _Smoothness_arr NatureManufactureShadersIceShaderMetalicLakeTranslucency
			UNITY_DEFINE_INSTANCED_PROP(fixed, _IceAmountOpacity)
#define _IceAmountOpacity_arr NatureManufactureShadersIceShaderMetalicLakeTranslucency
		UNITY_INSTANCING_BUFFER_END(NatureManufactureShadersIceShaderMetalicLakeTranslucency)


		float4 CalculateContrast( float contrastValue, float4 colorTarget )
		{
			float t = 0.5 * ( 1.0 - contrastValue );
			return mul( float4x4( contrastValue,0,0,t, 0,contrastValue,0,t, 0,0,contrastValue,t, 0,0,0,1 ), colorTarget );
		}

		inline half4 LightingStandardCustom(SurfaceOutputStandardCustom s, half3 viewDir, UnityGI gi )
		{
			#if !DIRECTIONAL
			float3 lightAtten = gi.light.color;
			#else
			float3 lightAtten = lerp( _LightColor0.rgb, gi.light.color, _TransShadow );
			#endif
			half3 lightDir = gi.light.dir + s.Normal * _TransNormalDistortion;
			half transVdotL = pow( saturate( dot( viewDir, -lightDir ) ), _TransScattering );
			half3 translucency = lightAtten * (transVdotL * _TransDirect + gi.indirect.diffuse * _TransAmbient) * s.Translucency;
			half4 c = half4( s.Albedo * translucency * _Translucency, 0 );

			SurfaceOutputStandard r;
			r.Albedo = s.Albedo;
			r.Normal = s.Normal;
			r.Emission = s.Emission;
			r.Metallic = s.Metallic;
			r.Smoothness = s.Smoothness;
			r.Occlusion = s.Occlusion;
			r.Alpha = s.Alpha;
			return LightingStandard (r, viewDir, gi) + c;
		}

		inline void LightingStandardCustom_GI(SurfaceOutputStandardCustom s, UnityGIInput data, inout UnityGI gi )
		{
			UNITY_GI(gi, s, data);
		}

		void surf( Input i , inout SurfaceOutputStandardCustom o )
		{
			float2 uv_IceNormal = i.uv_texcoord * _IceNormal_ST.xy + _IceNormal_ST.zw;
			fixed3 tex2DNode17 = UnpackScaleNormal( tex2D( _IceNormal, uv_IceNormal ) ,_IceNormalScale );
			o.Normal = tex2DNode17;
			float temp_output_242_0 = ( _IceDepth * 0.5 );
			fixed2 temp_cast_0 = (( temp_output_242_0 * 2.0 )).xx;
			float cos247 = cos( 0.6 );
			float sin247 = sin( 0.6 );
			float2 rotator247 = mul( temp_cast_0 - float2( 0.5,0.5 ) , float2x2( cos247 , -sin247 , sin247 , cos247 )) + float2( 0.5,0.5 );
			fixed4 tex2DNode234 = tex2D( _IceTexture, ( rotator247 + float2( 0.7,0.3 ) ) );
			float2 uv_IceTexture = i.uv_texcoord * _IceTexture_ST.xy + _IceTexture_ST.zw;
			float2 Offset238 = ( ( 0.0 - 1 ) * i.viewDir.xy * temp_output_242_0 ) + uv_IceTexture;
			float cos244 = cos( 0.6 );
			float sin244 = sin( 0.6 );
			float2 rotator244 = mul( ( Offset238 * float2( 2,2 ) ) - float2( 0.5,0.5 ) , float2x2( cos244 , -sin244 , sin244 , cos244 )) + float2( 0.5,0.5 );
			float4 lerpResult235 = lerp( tex2DNode234 , ( tex2D( _IceTexture, rotator244 ) * _IceColorBackground ) , _BackGroundIceBlend);
			fixed4 blendOpSrc236 = tex2DNode234;
			fixed4 blendOpDest236 = lerpResult235;
			float4 temp_output_268_0 = ( tex2D( _IceTexture, uv_IceTexture ) * _IceColorTop );
			float2 Offset201 = ( ( 0.0 - 1 ) * i.viewDir.xy * _IceDepth ) + uv_IceTexture;
			float4 lerpResult228 = lerp( temp_output_268_0 , ( tex2D( _IceTexture, Offset201 ) * _IceColorBackground ) , _BackGroundIceBlend);
			fixed4 blendOpSrc230 = temp_output_268_0;
			fixed4 blendOpDest230 = lerpResult228;
			float4 temp_output_230_0 = ( saturate( 	max( blendOpSrc230, blendOpDest230 ) ));
			float2 uv_IceNoise = i.uv_texcoord * _IceNoise_ST.xy + _IceNoise_ST.zw;
			fixed4 tex2DNode251 = tex2D( _IceNoise, uv_IceNoise );
			float3 appendResult260 = (fixed3(tex2DNode251.r , tex2DNode251.g , tex2DNode251.b));
			float4 clampResult261 = clamp( CalculateContrast(0.0,fixed4( appendResult260 , 0.0 )) , float4( 0,0,0,0 ) , float4( 1,1,1,0 ) );
			float4 lerpResult249 = lerp( ( saturate( 	max( blendOpSrc236, blendOpDest236 ) )) , temp_output_230_0 , clampResult261.r);
			float4 lerpResult252 = lerp( float4( 0,0,0,0 ) , lerpResult249 , _NoisePower);
			fixed4 blendOpSrc243 = lerpResult252;
			fixed4 blendOpDest243 = temp_output_230_0;
			float4 clampResult229 = clamp( ( saturate( 	max( blendOpSrc243, blendOpDest243 ) )) , float4( 0,0,0,0 ) , float4( 1,1,1,1 ) );
			float4 ase_screenPos = float4( i.screenPos.xyz , i.screenPos.w + 0.00000000001 );
			float eyeDepth1 = LinearEyeDepth(UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture,UNITY_PROJ_COORD(ase_screenPos))));
			float temp_output_89_0 = abs( ( eyeDepth1 - ase_screenPos.w ) );
			float temp_output_94_0 = saturate( pow( ( temp_output_89_0 * _WaterDepth ) , _WaterFalloff ) );
			float4 lerpResult13 = lerp( _ShalowColor , clampResult229 , temp_output_94_0);
			float4 lerpResult93 = lerp( lerpResult13 , clampResult229 , temp_output_94_0);
			float3 ase_worldPos = i.worldPos;
			float clampResult276 = clamp( pow( ( distance( ase_worldPos , _WorldSpaceCameraPos ) / _FogColorDistance ) , _FogColorHardness ) , 0.0 , 0.9999 );
			float4 lerpResult280 = lerp( lerpResult93 , ( unity_FogColor * _FogColorMultiply ) , clampResult276);
			fixed4 _RefractionColor_Instance = UNITY_ACCESS_INSTANCED_PROP(_RefractionColor_arr, _RefractionColor);
			float4 ase_screenPosNorm = ase_screenPos / ase_screenPos.w;
			ase_screenPosNorm.z = ( UNITY_NEAR_CLIP_VALUE >= 0 ) ? ase_screenPosNorm.z : ase_screenPosNorm.z * 0.5 + 0.5;
			fixed4 screenColor294 = tex2D( _GrabScreen1, ( (ase_screenPosNorm).xy + (( tex2DNode17 * _RefractionPower )).xy ) );
			float decodeFloatRGBA292 = DecodeFloatRGBA( ( 1.0 - screenColor294 ) );
			fixed4 temp_cast_4 = (pow( ( decodeFloatRGBA292 - 0.1 ) , 4.0 )).xxxx;
			float decodeFloatRGBA284 = DecodeFloatRGBA( temp_cast_4 );
			float lerpResult287 = lerp( 0.0 , _ShadowPower , decodeFloatRGBA284);
			float clampResult288 = clamp( lerpResult287 , 0.0 , 1.0 );
			float4 lerpResult283 = lerp( lerpResult280 , _RefractionColor_Instance , clampResult288);
			o.Emission = lerpResult283.rgb;
			fixed _MetalicPower_Instance = UNITY_ACCESS_INSTANCED_PROP(_MetalicPower_arr, _MetalicPower);
			o.Metallic = _MetalicPower_Instance;
			fixed _Smoothness_Instance = UNITY_ACCESS_INSTANCED_PROP(_Smoothness_arr, _Smoothness);
			o.Smoothness = _Smoothness_Instance;
			o.Translucency = lerpResult280.rgb;
			float lerpResult264 = lerp( 0.0 , 1.0 , temp_output_94_0);
			fixed _IceAmountOpacity_Instance = UNITY_ACCESS_INSTANCED_PROP(_IceAmountOpacity_arr, _IceAmountOpacity);
			o.Alpha = ( lerpResult264 * _IceAmountOpacity_Instance );
		}

		ENDCG
		CGPROGRAM
		#pragma surface surf StandardCustom alpha:fade keepalpha fullforwardshadows exclude_path:deferred 

		ENDCG
		Pass
		{
			Name "ShadowCaster"
			Tags{ "LightMode" = "ShadowCaster" }
			ZWrite On
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 2.0
			#pragma multi_compile_shadowcaster
			#pragma multi_compile UNITY_PASS_SHADOWCASTER
			#pragma skip_variants FOG_LINEAR FOG_EXP FOG_EXP2
			# include "HLSLSupport.cginc"
			#if ( SHADER_API_D3D11 || SHADER_API_GLCORE || SHADER_API_GLES3 || SHADER_API_METAL || SHADER_API_VULKAN )
				#define CAN_SKIP_VPOS
			#endif
			#include "UnityCG.cginc"
			#include "Lighting.cginc"
			#include "UnityPBSLighting.cginc"
			sampler3D _DitherMaskLOD;
			struct v2f
			{
				V2F_SHADOW_CASTER;
				float4 screenPos : TEXCOORD7;
				float4 tSpace0 : TEXCOORD1;
				float4 tSpace1 : TEXCOORD2;
				float4 tSpace2 : TEXCOORD3;
				float4 texcoords01 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};
			v2f vert( appdata_full v )
			{
				v2f o;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_INITIALIZE_OUTPUT( v2f, o );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				float3 worldPos = mul( unity_ObjectToWorld, v.vertex ).xyz;
				fixed3 worldNormal = UnityObjectToWorldNormal( v.normal );
				fixed3 worldTangent = UnityObjectToWorldDir( v.tangent.xyz );
				fixed tangentSign = v.tangent.w * unity_WorldTransformParams.w;
				fixed3 worldBinormal = cross( worldNormal, worldTangent ) * tangentSign;
				o.tSpace0 = float4( worldTangent.x, worldBinormal.x, worldNormal.x, worldPos.x );
				o.tSpace1 = float4( worldTangent.y, worldBinormal.y, worldNormal.y, worldPos.y );
				o.tSpace2 = float4( worldTangent.z, worldBinormal.z, worldNormal.z, worldPos.z );
				o.texcoords01 = float4( v.texcoord.xy, v.texcoord1.xy );
				TRANSFER_SHADOW_CASTER_NORMALOFFSET( o )
				o.screenPos = ComputeScreenPos( o.pos );
				return o;
			}
			fixed4 frag( v2f IN
			#if !defined( CAN_SKIP_VPOS )
			, UNITY_VPOS_TYPE vpos : VPOS
			#endif
			) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				Input surfIN;
				UNITY_INITIALIZE_OUTPUT( Input, surfIN );
				surfIN.uv_texcoord.xy = IN.texcoords01.xy;
				float3 worldPos = float3( IN.tSpace0.w, IN.tSpace1.w, IN.tSpace2.w );
				fixed3 worldViewDir = normalize( UnityWorldSpaceViewDir( worldPos ) );
				surfIN.viewDir = IN.tSpace0.xyz * worldViewDir.x + IN.tSpace1.xyz * worldViewDir.y + IN.tSpace2.xyz * worldViewDir.z;
				surfIN.worldPos = worldPos;
				surfIN.internalSurfaceTtoW0 = IN.tSpace0.xyz;
				surfIN.internalSurfaceTtoW1 = IN.tSpace1.xyz;
				surfIN.internalSurfaceTtoW2 = IN.tSpace2.xyz;
				surfIN.screenPos = IN.screenPos;
				SurfaceOutputStandardCustom o;
				UNITY_INITIALIZE_OUTPUT( SurfaceOutputStandardCustom, o )
				surf( surfIN, o );
				#if defined( CAN_SKIP_VPOS )
				float2 vpos = IN.pos;
				#endif
				half alphaRef = tex3D( _DitherMaskLOD, float3( vpos.xy * 0.25, o.Alpha * 0.9375 ) ).a;
				clip( alphaRef - 0.01 );
				SHADOW_CASTER_FRAGMENT( IN )
			}
			ENDCG
		}
	}
	Fallback "Diffuse"
}