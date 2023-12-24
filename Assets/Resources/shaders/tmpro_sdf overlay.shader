Shader "TMPro/Distance Field Overlay" {
	Properties {
		_FaceTex ("Face Texture", 2D) = "white" {}
		_FaceUVSpeedX ("Face UV Speed X", Range(-5, 5)) = 0
		_FaceUVSpeedY ("Face UV Speed Y", Range(-5, 5)) = 0
		_FaceColor ("Face Color", Vector) = (1,1,1,1)
		_FaceDilate ("Face Dilate", Range(-1, 1)) = 0
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_OutlineTex ("Outline Texture", 2D) = "white" {}
		_OutlineUVSpeedX ("Outline UV Speed X", Range(-5, 5)) = 0
		_OutlineUVSpeedY ("Outline UV Speed Y", Range(-5, 5)) = 0
		_OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
		_OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
		_Bevel ("Bevel", Range(0, 1)) = 0.5
		_BevelOffset ("Bevel Offset", Range(-0.5, 0.5)) = 0
		_BevelWidth ("Bevel Width", Range(-0.5, 0.5)) = 0
		_BevelClamp ("Bevel Clamp", Range(0, 1)) = 0
		_BevelRoundness ("Bevel Roundness", Range(0, 1)) = 0
		_LightAngle ("Light Angle", Range(0, 6.283185)) = 3.1416
		_SpecularColor ("Specular", Vector) = (1,1,1,1)
		_SpecularPower ("Specular", Range(0, 4)) = 2
		_Reflectivity ("Reflectivity", Range(5, 15)) = 10
		_Diffuse ("Diffuse", Range(0, 1)) = 0.5
		_Ambient ("Ambient", Range(1, 0)) = 0.5
		_BumpMap ("Normal map", 2D) = "bump" {}
		_BumpOutline ("Bump Outline", Range(0, 1)) = 0
		_BumpFace ("Bump Face", Range(0, 1)) = 0
		_ReflectFaceColor ("Reflection Color", Vector) = (0,0,0,1)
		_ReflectOutlineColor ("Reflection Color", Vector) = (0,0,0,1)
		_Cube ("Reflection Cubemap", Cube) = "black" {}
		_EnvMatrixRotation ("Texture Rotation", Vector) = (0,0,0,0)
		_UnderlayColor ("Border Color", Vector) = (0,0,0,0.5)
		_UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
		_UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
		_UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
		_UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
		_GlowColor ("Color", Vector) = (0,1,0,0.5)
		_GlowOffset ("Offset", Range(-1, 1)) = 0
		_GlowInner ("Inner", Range(0, 1)) = 0.05
		_GlowOuter ("Outer", Range(0, 1)) = 0.05
		_GlowPower ("Falloff", Range(1, 0)) = 0.75
		_WeightNormal ("Weight Normal", Float) = 0
		_WeightBold ("Weight Bold", Float) = 0.5
		_ShaderFlags ("Flags", Float) = 0
		_ScaleRatioA ("Scale RatioA", Float) = 1
		_ScaleRatioB ("Scale RatioB", Float) = 1
		_ScaleRatioC ("Scale RatioC", Float) = 1
		_MainTex ("Font Atlas", 2D) = "white" {}
		_TextureWidth ("Texture Width", Float) = 512
		_TextureHeight ("Texture Height", Float) = 512
		_GradientScale ("Gradient Scale", Float) = 5
		_ScaleX ("Scale X", Float) = 1
		_ScaleY ("Scale Y", Float) = 1
		_PerspectiveFilter ("Perspective Correction", Range(0, 1)) = 0.875
		_VertexOffsetX ("Vertex OffsetX", Float) = 0
		_VertexOffsetY ("Vertex OffsetY", Float) = 0
		_MaskCoord ("Mask Coordinates", Vector) = (0,0,100000,100000)
		_ClipRect ("Clip Rect", Vector) = (-100000,-100000,100000,100000)
		_MaskSoftnessX ("Mask SoftnessX", Float) = 0
		_MaskSoftnessY ("Mask SoftnessY", Float) = 0
		_ColorMask ("Color Mask", Float) = 15
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Overlay" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Overlay" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ColorMask 0 -1
			ZTest Always
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 30524
			Program "vp" {
				SubProgram "d3d11 " {
					"vs_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform VGlobals {
						vec4 unused_0_0[3];
						vec4 _FaceColor;
						float _FaceDilate;
						float _OutlineSoftness;
						vec4 _OutlineColor;
						float _OutlineWidth;
						vec4 unused_0_6[4];
						mat4x4 _EnvMatrix;
						vec4 unused_0_8[7];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_14[4];
						float _GradientScale;
						float _ScaleX;
						float _ScaleY;
						float _PerspectiveFilter;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[4];
						vec3 _WorldSpaceCameraPos;
						vec4 unused_1_2;
						vec4 _ScreenParams;
						vec4 unused_1_4[2];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_2_2[2];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[5];
						mat4x4 glstate_matrix_projection;
						vec4 unused_3_2[8];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_4[2];
					};
					in  vec4 in_POSITION0;
					in  vec3 in_NORMAL0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					in  vec2 in_TEXCOORD1;
					out vec4 vs_COLOR0;
					out vec4 vs_COLOR1;
					out vec4 vs_COLOR2;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec3 vs_TEXCOORD3;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec3 u_xlat4;
					float u_xlat7;
					vec2 u_xlat8;
					float u_xlat9;
					bool u_xlatb9;
					void main()
					{
					    u_xlat0.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat1 = u_xlat0.yyyy * unity_ObjectToWorld[1];
					    u_xlat1 = unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    vs_TEXCOORD2.xy = u_xlat0.xy;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat0.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0.xyz = (-u_xlat0.xyz) + _WorldSpaceCameraPos.xyz;
					    u_xlat2 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat2 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat2;
					    u_xlat2 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat2;
					    u_xlat1 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat2;
					    gl_Position = u_xlat1;
					    vs_COLOR0 = in_COLOR0;
					    vs_COLOR1 = in_COLOR0 * _FaceColor;
					    vs_COLOR2.w = in_COLOR0.w * _OutlineColor.w;
					    vs_COLOR2.xyz = _OutlineColor.xyz;
					    u_xlat9 = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat8.x = floor(u_xlat9);
					    u_xlat8.y = (-u_xlat8.x) * 4096.0 + in_TEXCOORD1.x;
					    vs_TEXCOORD0.zw = u_xlat8.xy * vec2(0.001953125, 0.001953125);
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat1.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat9 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat9 = inversesqrt(u_xlat9);
					    u_xlat1.xyz = vec3(u_xlat9) * u_xlat1.xyz;
					    u_xlat9 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat9 = inversesqrt(u_xlat9);
					    u_xlat2.xyz = vec3(u_xlat9) * u_xlat0.xyz;
					    u_xlat9 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlat1.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat1.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat1.xy;
					    u_xlat1.xy = abs(u_xlat1.xy) * vec2(_ScaleX, _ScaleY);
					    u_xlat1.xy = u_xlat1.ww / u_xlat1.xy;
					    u_xlat7 = dot(u_xlat1.xy, u_xlat1.xy);
					    vs_TEXCOORD2.zw = vec2(0.5, 0.5) / u_xlat1.xy;
					    u_xlat1.x = inversesqrt(u_xlat7);
					    u_xlat1.x = u_xlat1.x * abs(in_TEXCOORD1.y);
					    u_xlat1.x = u_xlat1.x * _GradientScale;
					    u_xlat4.x = u_xlat1.x * 1.5;
					    u_xlat7 = (-_PerspectiveFilter) + 1.0;
					    u_xlat7 = u_xlat7 * u_xlat4.x;
					    u_xlat1.x = u_xlat1.x * 1.5 + (-u_xlat7);
					    u_xlat9 = abs(u_xlat9) * u_xlat1.x + u_xlat7;
					    u_xlatb1 = glstate_matrix_projection[3].w==0.0;
					    u_xlat4.x = (u_xlatb1) ? u_xlat9 : u_xlat4.x;
					    u_xlatb9 = 0.0>=in_TEXCOORD1.y;
					    u_xlat9 = u_xlatb9 ? 1.0 : float(0.0);
					    u_xlat1.x = (-_WeightNormal) + _WeightBold;
					    u_xlat9 = u_xlat9 * u_xlat1.x + _WeightNormal;
					    u_xlat9 = u_xlat9 / _GradientScale;
					    u_xlat1.x = _FaceDilate * _ScaleRatioA;
					    u_xlat4.z = u_xlat1.x * 0.5 + u_xlat9;
					    vs_TEXCOORD1.yw = u_xlat4.xz;
					    u_xlat9 = 0.5 / u_xlat4.x;
					    u_xlat1.x = (-_OutlineWidth) * _ScaleRatioA + 1.0;
					    u_xlat1.x = (-_OutlineSoftness) * _ScaleRatioA + u_xlat1.x;
					    u_xlat1.x = u_xlat1.x * 0.5 + (-u_xlat9);
					    vs_TEXCOORD1.x = (-u_xlat4.z) + u_xlat1.x;
					    u_xlat1.x = (-u_xlat4.z) + 0.5;
					    vs_TEXCOORD1.z = u_xlat9 + u_xlat1.x;
					    u_xlat1.xyz = u_xlat0.yyy * _EnvMatrix[1].xyz;
					    u_xlat0.xyw = _EnvMatrix[0].xyz * u_xlat0.xxx + u_xlat1.xyz;
					    vs_TEXCOORD3.xyz = _EnvMatrix[2].xyz * u_xlat0.zzz + u_xlat0.xyw;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define HLSLCC_ENABLE_UNIFORM_BUFFERS 1
					#if HLSLCC_ENABLE_UNIFORM_BUFFERS
					#define UNITY_UNIFORM
					#else
					#define UNITY_UNIFORM uniform
					#endif
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						float _FaceUVSpeedX;
						float _FaceUVSpeedY;
						vec4 unused_0_3;
						float _OutlineSoftness;
						float _OutlineUVSpeedX;
						float _OutlineUVSpeedY;
						vec4 unused_0_7;
						float _OutlineWidth;
						vec4 unused_0_9[15];
						float _ScaleRatioA;
						vec4 unused_0_11[3];
						vec4 _ClipRect;
						vec4 unused_0_13[2];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _FaceTex;
					uniform  sampler2D _OutlineTex;
					in  vec4 vs_COLOR1;
					in  vec4 vs_COLOR2;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					bvec4 u_xlatb1;
					vec4 u_xlat2;
					float u_xlat3;
					vec2 u_xlat6;
					float u_xlat9;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0.x = u_xlat0.w + (-vs_TEXCOORD1.x);
					    u_xlat3 = (-u_xlat0.w) + vs_TEXCOORD1.z;
					    u_xlatb0 = u_xlat0.x<0.0;
					    if(((int(u_xlatb0) * int(0xffffffffu)))!=0){discard;}
					    u_xlat0.x = _OutlineWidth * _ScaleRatioA;
					    u_xlat0.x = u_xlat0.x * vs_TEXCOORD1.y;
					    u_xlat6.x = min(u_xlat0.x, 1.0);
					    u_xlat0.x = u_xlat0.x * 0.5;
					    u_xlat6.x = sqrt(u_xlat6.x);
					    u_xlat9 = u_xlat3 * vs_TEXCOORD1.y + u_xlat0.x;
					    u_xlat9 = clamp(u_xlat9, 0.0, 1.0);
					    u_xlat0.x = u_xlat3 * vs_TEXCOORD1.y + (-u_xlat0.x);
					    u_xlat3 = u_xlat6.x * u_xlat9;
					    u_xlat6.xy = vec2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + vs_TEXCOORD0.zw;
					    u_xlat1 = texture(_OutlineTex, u_xlat6.xy);
					    u_xlat1 = u_xlat1 * vs_COLOR2;
					    u_xlat1.xyz = u_xlat1.www * u_xlat1.xyz;
					    u_xlat6.xy = vec2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + vs_TEXCOORD0.zw;
					    u_xlat2 = texture(_FaceTex, u_xlat6.xy);
					    u_xlat2 = u_xlat2 * vs_COLOR1;
					    u_xlat2.xyz = u_xlat2.www * u_xlat2.xyz;
					    u_xlat1 = u_xlat1 + (-u_xlat2);
					    u_xlat1 = vec4(u_xlat3) * u_xlat1 + u_xlat2;
					    u_xlat3 = _OutlineSoftness * _ScaleRatioA;
					    u_xlat6.x = u_xlat3 * vs_TEXCOORD1.y;
					    u_xlat3 = u_xlat3 * vs_TEXCOORD1.y + 1.0;
					    u_xlat0.x = u_xlat6.x * 0.5 + u_xlat0.x;
					    u_xlat0.x = u_xlat0.x / u_xlat3;
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat0.x = (-u_xlat0.x) + 1.0;
					    u_xlat0 = u_xlat0.xxxx * u_xlat1;
					    u_xlatb1.xy = greaterThanEqual(vs_TEXCOORD2.xyxx, _ClipRect.xyxx).xy;
					    u_xlatb1.zw = greaterThanEqual(_ClipRect.zzzw, vs_TEXCOORD2.xxxy).zw;
					    u_xlat1.x = u_xlatb1.x ? float(1.0) : 0.0;
					    u_xlat1.y = u_xlatb1.y ? float(1.0) : 0.0;
					    u_xlat1.z = u_xlatb1.z ? float(1.0) : 0.0;
					    u_xlat1.w = u_xlatb1.w ? float(1.0) : 0.0;
					;
					    u_xlat1.xy = u_xlat1.zw * u_xlat1.xy;
					    u_xlat1.x = u_xlat1.y * u_xlat1.x;
					    SV_Target0 = u_xlat0 * u_xlat1.xxxx;
					    return;
					}"
				}
			}
		}
	}
	Fallback "TMPro/Mobile/Distance Field"
	CustomEditor "TMPro_SDFMaterialEditor"
}