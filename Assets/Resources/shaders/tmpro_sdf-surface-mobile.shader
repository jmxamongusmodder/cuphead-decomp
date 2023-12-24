Shader "TMPro/Mobile/Distance Field (Surface)" {
	Properties {
		_FaceTex ("Fill Texture", 2D) = "white" {}
		_FaceColor ("Fill Color", Vector) = (1,1,1,1)
		_FaceDilate ("Face Dilate", Range(-1, 1)) = 0
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_OutlineTex ("Outline Texture", 2D) = "white" {}
		_OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
		_OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
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
	}
	SubShader {
		LOD 300
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			LOD 300
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ColorMask RGB -1
			ZWrite Off
			Cull Off
			GpuProgramID 39874
			Program "vp" {
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
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
						vec4 unused_0_0[6];
						float _FaceDilate;
						vec4 unused_0_2[6];
						mat4x4 _EnvMatrix;
						vec4 unused_0_4[7];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_10[4];
						float _GradientScale;
						float _ScaleX;
						float _ScaleY;
						float _PerspectiveFilter;
						vec4 unused_0_15;
						vec4 _MainTex_ST;
						vec4 _FaceTex_ST;
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
						vec4 unused_2_2;
						vec4 unity_WorldTransformParams;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[5];
						mat4x4 glstate_matrix_projection;
						vec4 unused_3_2[8];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_4[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_TEXCOORD1;
					in  vec4 in_COLOR0;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD4;
					out vec3 vs_TEXCOORD5;
					vec3 u_xlat0;
					int u_xlati0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					float u_xlat5;
					int u_xlati5;
					vec2 u_xlat13;
					float u_xlat15;
					float u_xlat16;
					void main()
					{
					    u_xlat0.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat1 = u_xlat0.yyyy * unity_ObjectToWorld[1];
					    u_xlat1 = unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat2 = u_xlat1 + unity_ObjectToWorld[3];
					    u_xlat1.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat1.xyz;
					    u_xlat3 = u_xlat2.yyyy * unity_MatrixVP[1];
					    u_xlat3 = unity_MatrixVP[0] * u_xlat2.xxxx + u_xlat3;
					    u_xlat3 = unity_MatrixVP[2] * u_xlat2.zzzz + u_xlat3;
					    gl_Position = unity_MatrixVP[3] * u_xlat2.wwww + u_xlat3;
					    u_xlat15 = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat13.x = floor(u_xlat15);
					    u_xlat13.y = (-u_xlat13.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat3.xy = u_xlat13.xy * _FaceTex_ST.xy;
					    vs_TEXCOORD0.zw = u_xlat3.xy * vec2(0.001953125, 0.001953125) + _FaceTex_ST.zw;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3.xyz = _WorldSpaceCameraPos.yyy * unity_WorldToObject[1].xyz;
					    u_xlat3.xyz = unity_WorldToObject[0].xyz * _WorldSpaceCameraPos.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_WorldToObject[2].xyz * _WorldSpaceCameraPos.zzz + u_xlat3.xyz;
					    u_xlat3.xyz = u_xlat3.xyz + unity_WorldToObject[3].xyz;
					    u_xlat0.z = in_POSITION0.z;
					    u_xlat0.xyz = (-u_xlat0.xyz) + u_xlat3.xyz;
					    u_xlat0.x = dot(in_NORMAL0.xyz, u_xlat0.xyz);
					    u_xlati5 = int((0.0<u_xlat0.x) ? 0xFFFFFFFFu : uint(0));
					    u_xlati0 = int((u_xlat0.x<0.0) ? 0xFFFFFFFFu : uint(0));
					    u_xlati0 = (-u_xlati5) + u_xlati0;
					    u_xlat0.x = float(u_xlati0);
					    u_xlat0.xyz = u_xlat0.xxx * in_NORMAL0.xyz;
					    u_xlat3.y = dot(u_xlat0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat3.z = dot(u_xlat0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat3.x = dot(u_xlat0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat3.xyz;
					    vs_TEXCOORD1.z = u_xlat0.y;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat15 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat15 = inversesqrt(u_xlat15);
					    u_xlat3.xyz = vec3(u_xlat15) * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat0.xyz * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat0.zxy * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat15 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = vec3(u_xlat15) * u_xlat4.xyz;
					    vs_TEXCOORD1.y = u_xlat4.x;
					    vs_TEXCOORD1.w = u_xlat1.x;
					    vs_TEXCOORD1.x = u_xlat3.z;
					    vs_TEXCOORD2.z = u_xlat0.z;
					    vs_TEXCOORD2.x = u_xlat3.x;
					    vs_TEXCOORD3.x = u_xlat3.y;
					    vs_TEXCOORD2.y = u_xlat4.y;
					    vs_TEXCOORD3.y = u_xlat4.z;
					    vs_TEXCOORD2.w = u_xlat1.y;
					    vs_TEXCOORD3.z = u_xlat0.x;
					    vs_TEXCOORD3.w = u_xlat1.z;
					    u_xlat1.xyz = (-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz;
					    vs_COLOR0 = in_COLOR0;
					    u_xlat15 = u_xlat2.y * unity_MatrixVP[1].w;
					    u_xlat15 = unity_MatrixVP[0].w * u_xlat2.x + u_xlat15;
					    u_xlat15 = unity_MatrixVP[2].w * u_xlat2.z + u_xlat15;
					    u_xlat15 = unity_MatrixVP[3].w * u_xlat2.w + u_xlat15;
					    u_xlat2.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat2.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat2.xy;
					    u_xlat2.xy = u_xlat2.xy * vec2(_ScaleX, _ScaleY);
					    u_xlat2.xy = vec2(u_xlat15) / u_xlat2.xy;
					    u_xlat15 = dot(u_xlat2.xy, u_xlat2.xy);
					    u_xlat15 = inversesqrt(u_xlat15);
					    u_xlat16 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat15 = u_xlat15 * u_xlat16;
					    u_xlat16 = u_xlat15 * 1.5;
					    u_xlat2.x = (-_PerspectiveFilter) + 1.0;
					    u_xlat16 = u_xlat16 * u_xlat2.x;
					    u_xlat15 = u_xlat15 * 1.5 + (-u_xlat16);
					    u_xlat2.x = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat2.x = inversesqrt(u_xlat2.x);
					    u_xlat2.xyz = u_xlat1.xyz * u_xlat2.xxx;
					    u_xlat0.x = dot(u_xlat0.yzx, u_xlat2.xyz);
					    vs_TEXCOORD4.y = abs(u_xlat0.x) * u_xlat15 + u_xlat16;
					    u_xlatb0 = 0.0>=in_TEXCOORD1.y;
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat5 = (-_WeightNormal) + _WeightBold;
					    u_xlat0.x = u_xlat0.x * u_xlat5 + _WeightNormal;
					    u_xlat0.x = u_xlat0.x / _GradientScale;
					    u_xlat5 = _FaceDilate * _ScaleRatioA;
					    vs_TEXCOORD4.x = u_xlat5 * 0.5 + u_xlat0.x;
					    u_xlat0.xyz = u_xlat1.yyy * _EnvMatrix[1].xyz;
					    u_xlat0.xyz = _EnvMatrix[0].xyz * u_xlat1.xxx + u_xlat0.xyz;
					    vs_TEXCOORD5.xyz = _EnvMatrix[2].xyz * u_xlat1.zzz + u_xlat0.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
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
						vec4 unused_0_0[6];
						float _FaceDilate;
						vec4 unused_0_2[6];
						mat4x4 _EnvMatrix;
						vec4 unused_0_4[7];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_10[4];
						float _GradientScale;
						float _ScaleX;
						float _ScaleY;
						float _PerspectiveFilter;
						vec4 unused_0_15;
						vec4 _MainTex_ST;
						vec4 _FaceTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[4];
						vec3 _WorldSpaceCameraPos;
						vec4 unused_1_2;
						vec4 _ScreenParams;
						vec4 unused_1_4[2];
					};
					layout(std140) uniform UnityLighting {
						vec4 unused_2_0[42];
						vec4 unity_SHBr;
						vec4 unity_SHBg;
						vec4 unity_SHBb;
						vec4 unity_SHC;
						vec4 unused_2_5[2];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_3_2;
						vec4 unity_WorldTransformParams;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_4_0[5];
						mat4x4 glstate_matrix_projection;
						vec4 unused_4_2[8];
						mat4x4 unity_MatrixVP;
						vec4 unused_4_4[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_TEXCOORD1;
					in  vec4 in_COLOR0;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD4;
					out vec3 vs_TEXCOORD5;
					out vec3 vs_TEXCOORD6;
					vec4 u_xlat0;
					int u_xlati0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					int u_xlati5;
					vec3 u_xlat7;
					vec2 u_xlat13;
					float u_xlat15;
					float u_xlat16;
					bool u_xlatb16;
					void main()
					{
					    u_xlat0.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat1 = u_xlat0.yyyy * unity_ObjectToWorld[1];
					    u_xlat1 = unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat2 = u_xlat1 + unity_ObjectToWorld[3];
					    u_xlat1.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat1.xyz;
					    u_xlat3 = u_xlat2.yyyy * unity_MatrixVP[1];
					    u_xlat3 = unity_MatrixVP[0] * u_xlat2.xxxx + u_xlat3;
					    u_xlat3 = unity_MatrixVP[2] * u_xlat2.zzzz + u_xlat3;
					    gl_Position = unity_MatrixVP[3] * u_xlat2.wwww + u_xlat3;
					    u_xlat15 = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat13.x = floor(u_xlat15);
					    u_xlat13.y = (-u_xlat13.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat3.xy = u_xlat13.xy * _FaceTex_ST.xy;
					    vs_TEXCOORD0.zw = u_xlat3.xy * vec2(0.001953125, 0.001953125) + _FaceTex_ST.zw;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3.xyz = _WorldSpaceCameraPos.yyy * unity_WorldToObject[1].xyz;
					    u_xlat3.xyz = unity_WorldToObject[0].xyz * _WorldSpaceCameraPos.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_WorldToObject[2].xyz * _WorldSpaceCameraPos.zzz + u_xlat3.xyz;
					    u_xlat3.xyz = u_xlat3.xyz + unity_WorldToObject[3].xyz;
					    u_xlat0.z = in_POSITION0.z;
					    u_xlat0.xyz = (-u_xlat0.xyz) + u_xlat3.xyz;
					    u_xlat0.x = dot(in_NORMAL0.xyz, u_xlat0.xyz);
					    u_xlati5 = int((0.0<u_xlat0.x) ? 0xFFFFFFFFu : uint(0));
					    u_xlati0 = int((u_xlat0.x<0.0) ? 0xFFFFFFFFu : uint(0));
					    u_xlati0 = (-u_xlati5) + u_xlati0;
					    u_xlat0.x = float(u_xlati0);
					    u_xlat0.xyz = u_xlat0.xxx * in_NORMAL0.xyz;
					    u_xlat3.x = dot(u_xlat0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat3.y = dot(u_xlat0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat3.z = dot(u_xlat0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0 = u_xlat0.xxxx * u_xlat3.xyzz;
					    vs_TEXCOORD1.z = u_xlat0.x;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat16 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat16 = inversesqrt(u_xlat16);
					    u_xlat3.xyz = vec3(u_xlat16) * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat0.wxy * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat0.ywx * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat16 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = vec3(u_xlat16) * u_xlat4.xyz;
					    vs_TEXCOORD1.y = u_xlat4.x;
					    vs_TEXCOORD1.w = u_xlat1.x;
					    vs_TEXCOORD1.x = u_xlat3.z;
					    vs_TEXCOORD2.x = u_xlat3.x;
					    vs_TEXCOORD3.x = u_xlat3.y;
					    vs_TEXCOORD2.z = u_xlat0.y;
					    vs_TEXCOORD2.w = u_xlat1.y;
					    vs_TEXCOORD2.y = u_xlat4.y;
					    vs_TEXCOORD3.y = u_xlat4.z;
					    vs_TEXCOORD3.w = u_xlat1.z;
					    u_xlat1.xyz = (-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz;
					    vs_TEXCOORD3.z = u_xlat0.w;
					    vs_COLOR0 = in_COLOR0;
					    u_xlat16 = u_xlat2.y * unity_MatrixVP[1].w;
					    u_xlat16 = unity_MatrixVP[0].w * u_xlat2.x + u_xlat16;
					    u_xlat16 = unity_MatrixVP[2].w * u_xlat2.z + u_xlat16;
					    u_xlat16 = unity_MatrixVP[3].w * u_xlat2.w + u_xlat16;
					    u_xlat2.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat2.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat2.xy;
					    u_xlat2.xy = u_xlat2.xy * vec2(_ScaleX, _ScaleY);
					    u_xlat2.xy = vec2(u_xlat16) / u_xlat2.xy;
					    u_xlat16 = dot(u_xlat2.xy, u_xlat2.xy);
					    u_xlat16 = inversesqrt(u_xlat16);
					    u_xlat2.x = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat16 = u_xlat16 * u_xlat2.x;
					    u_xlat2.x = u_xlat16 * 1.5;
					    u_xlat7.x = (-_PerspectiveFilter) + 1.0;
					    u_xlat2.x = u_xlat7.x * u_xlat2.x;
					    u_xlat16 = u_xlat16 * 1.5 + (-u_xlat2.x);
					    u_xlat7.x = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat7.x = inversesqrt(u_xlat7.x);
					    u_xlat7.xyz = u_xlat1.xyz * u_xlat7.xxx;
					    u_xlat7.x = dot(u_xlat0.xyw, u_xlat7.xyz);
					    vs_TEXCOORD4.y = abs(u_xlat7.x) * u_xlat16 + u_xlat2.x;
					    u_xlatb16 = 0.0>=in_TEXCOORD1.y;
					    u_xlat16 = u_xlatb16 ? 1.0 : float(0.0);
					    u_xlat2.x = (-_WeightNormal) + _WeightBold;
					    u_xlat16 = u_xlat16 * u_xlat2.x + _WeightNormal;
					    u_xlat16 = u_xlat16 / _GradientScale;
					    u_xlat2.x = _FaceDilate * _ScaleRatioA;
					    vs_TEXCOORD4.x = u_xlat2.x * 0.5 + u_xlat16;
					    u_xlat2.xyz = u_xlat1.yyy * _EnvMatrix[1].xyz;
					    u_xlat1.xyw = _EnvMatrix[0].xyz * u_xlat1.xxx + u_xlat2.xyz;
					    vs_TEXCOORD5.xyz = _EnvMatrix[2].xyz * u_xlat1.zzz + u_xlat1.xyw;
					    u_xlat1.x = u_xlat0.y * u_xlat0.y;
					    u_xlat1.x = u_xlat0.x * u_xlat0.x + (-u_xlat1.x);
					    u_xlat0 = u_xlat0.ywzx * u_xlat0;
					    u_xlat2.x = dot(unity_SHBr, u_xlat0);
					    u_xlat2.y = dot(unity_SHBg, u_xlat0);
					    u_xlat2.z = dot(unity_SHBb, u_xlat0);
					    vs_TEXCOORD6.xyz = unity_SHC.xyz * u_xlat1.xxx + u_xlat2.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" }
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
						vec4 unused_0_0[6];
						float _FaceDilate;
						vec4 unused_0_2[6];
						mat4x4 _EnvMatrix;
						vec4 unused_0_4[7];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_10[4];
						float _GradientScale;
						float _ScaleX;
						float _ScaleY;
						float _PerspectiveFilter;
						vec4 unused_0_15;
						vec4 _MainTex_ST;
						vec4 _FaceTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[4];
						vec3 _WorldSpaceCameraPos;
						vec4 unused_1_2;
						vec4 _ScreenParams;
						vec4 unused_1_4[2];
					};
					layout(std140) uniform UnityLighting {
						vec4 unused_2_0[3];
						vec4 unity_4LightPosX0;
						vec4 unity_4LightPosY0;
						vec4 unity_4LightPosZ0;
						vec4 unity_4LightAtten0;
						vec4 unity_LightColor[8];
						vec4 unused_2_6[34];
						vec4 unity_SHBr;
						vec4 unity_SHBg;
						vec4 unity_SHBb;
						vec4 unity_SHC;
						vec4 unused_2_11[2];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						mat4x4 unity_WorldToObject;
						vec4 unused_3_2;
						vec4 unity_WorldTransformParams;
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_4_0[5];
						mat4x4 glstate_matrix_projection;
						vec4 unused_4_2[8];
						mat4x4 unity_MatrixVP;
						vec4 unused_4_4[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TANGENT0;
					in  vec3 in_NORMAL0;
					in  vec4 in_TEXCOORD0;
					in  vec4 in_TEXCOORD1;
					in  vec4 in_COLOR0;
					out vec4 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD4;
					out vec3 vs_TEXCOORD5;
					out vec3 vs_TEXCOORD6;
					vec4 u_xlat0;
					int u_xlati0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					int u_xlati5;
					vec3 u_xlat7;
					vec2 u_xlat13;
					float u_xlat15;
					float u_xlat16;
					bool u_xlatb16;
					void main()
					{
					    u_xlat0.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat1 = u_xlat0.yyyy * unity_ObjectToWorld[1];
					    u_xlat1 = unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat2 = u_xlat1 + unity_ObjectToWorld[3];
					    u_xlat1.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat1.xyz;
					    u_xlat3 = u_xlat2.yyyy * unity_MatrixVP[1];
					    u_xlat3 = unity_MatrixVP[0] * u_xlat2.xxxx + u_xlat3;
					    u_xlat3 = unity_MatrixVP[2] * u_xlat2.zzzz + u_xlat3;
					    gl_Position = unity_MatrixVP[3] * u_xlat2.wwww + u_xlat3;
					    u_xlat15 = in_TEXCOORD1.x * 0.000244140625;
					    u_xlat13.x = floor(u_xlat15);
					    u_xlat13.y = (-u_xlat13.x) * 4096.0 + in_TEXCOORD1.x;
					    u_xlat3.xy = u_xlat13.xy * _FaceTex_ST.xy;
					    vs_TEXCOORD0.zw = u_xlat3.xy * vec2(0.001953125, 0.001953125) + _FaceTex_ST.zw;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat3.xyz = _WorldSpaceCameraPos.yyy * unity_WorldToObject[1].xyz;
					    u_xlat3.xyz = unity_WorldToObject[0].xyz * _WorldSpaceCameraPos.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_WorldToObject[2].xyz * _WorldSpaceCameraPos.zzz + u_xlat3.xyz;
					    u_xlat3.xyz = u_xlat3.xyz + unity_WorldToObject[3].xyz;
					    u_xlat0.z = in_POSITION0.z;
					    u_xlat0.xyz = (-u_xlat0.xyz) + u_xlat3.xyz;
					    u_xlat0.x = dot(in_NORMAL0.xyz, u_xlat0.xyz);
					    u_xlati5 = int((0.0<u_xlat0.x) ? 0xFFFFFFFFu : uint(0));
					    u_xlati0 = int((u_xlat0.x<0.0) ? 0xFFFFFFFFu : uint(0));
					    u_xlati0 = (-u_xlati5) + u_xlati0;
					    u_xlat0.x = float(u_xlati0);
					    u_xlat0.xyz = u_xlat0.xxx * in_NORMAL0.xyz;
					    u_xlat3.x = dot(u_xlat0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat3.y = dot(u_xlat0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat3.z = dot(u_xlat0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat0.x = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat0.x = inversesqrt(u_xlat0.x);
					    u_xlat0 = u_xlat0.xxxx * u_xlat3.xyzz;
					    u_xlat3.xyz = in_TANGENT0.yyy * unity_ObjectToWorld[1].yzx;
					    u_xlat3.xyz = unity_ObjectToWorld[0].yzx * in_TANGENT0.xxx + u_xlat3.xyz;
					    u_xlat3.xyz = unity_ObjectToWorld[2].yzx * in_TANGENT0.zzz + u_xlat3.xyz;
					    u_xlat16 = dot(u_xlat3.xyz, u_xlat3.xyz);
					    u_xlat16 = inversesqrt(u_xlat16);
					    u_xlat3.xyz = vec3(u_xlat16) * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat0.wxy * u_xlat3.xyz;
					    u_xlat4.xyz = u_xlat0.ywx * u_xlat3.yzx + (-u_xlat4.xyz);
					    u_xlat16 = in_TANGENT0.w * unity_WorldTransformParams.w;
					    u_xlat4.xyz = vec3(u_xlat16) * u_xlat4.xyz;
					    vs_TEXCOORD1.y = u_xlat4.x;
					    vs_TEXCOORD1.z = u_xlat0.x;
					    vs_TEXCOORD1.x = u_xlat3.z;
					    vs_TEXCOORD1.w = u_xlat1.x;
					    vs_TEXCOORD2.x = u_xlat3.x;
					    vs_TEXCOORD3.x = u_xlat3.y;
					    vs_TEXCOORD2.z = u_xlat0.y;
					    vs_TEXCOORD2.y = u_xlat4.y;
					    vs_TEXCOORD3.y = u_xlat4.z;
					    vs_TEXCOORD2.w = u_xlat1.y;
					    vs_TEXCOORD3.z = u_xlat0.w;
					    vs_TEXCOORD3.w = u_xlat1.z;
					    vs_COLOR0 = in_COLOR0;
					    u_xlatb16 = 0.0>=in_TEXCOORD1.y;
					    u_xlat16 = u_xlatb16 ? 1.0 : float(0.0);
					    u_xlat3.x = (-_WeightNormal) + _WeightBold;
					    u_xlat16 = u_xlat16 * u_xlat3.x + _WeightNormal;
					    u_xlat16 = u_xlat16 / _GradientScale;
					    u_xlat3.x = _FaceDilate * _ScaleRatioA;
					    vs_TEXCOORD4.x = u_xlat3.x * 0.5 + u_xlat16;
					    u_xlat16 = u_xlat2.y * unity_MatrixVP[1].w;
					    u_xlat16 = unity_MatrixVP[0].w * u_xlat2.x + u_xlat16;
					    u_xlat16 = unity_MatrixVP[2].w * u_xlat2.z + u_xlat16;
					    u_xlat16 = unity_MatrixVP[3].w * u_xlat2.w + u_xlat16;
					    u_xlat2.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat2.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat2.xy;
					    u_xlat2.xy = u_xlat2.xy * vec2(_ScaleX, _ScaleY);
					    u_xlat2.xy = vec2(u_xlat16) / u_xlat2.xy;
					    u_xlat16 = dot(u_xlat2.xy, u_xlat2.xy);
					    u_xlat16 = inversesqrt(u_xlat16);
					    u_xlat2.x = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat16 = u_xlat16 * u_xlat2.x;
					    u_xlat2.x = u_xlat16 * 1.5;
					    u_xlat7.x = (-_PerspectiveFilter) + 1.0;
					    u_xlat2.x = u_xlat7.x * u_xlat2.x;
					    u_xlat16 = u_xlat16 * 1.5 + (-u_xlat2.x);
					    u_xlat7.xyz = (-u_xlat1.xyz) + _WorldSpaceCameraPos.xyz;
					    u_xlat3.x = dot(u_xlat7.xyz, u_xlat7.xyz);
					    u_xlat3.x = inversesqrt(u_xlat3.x);
					    u_xlat3.xyz = u_xlat7.xyz * u_xlat3.xxx;
					    u_xlat3.x = dot(u_xlat0.xyw, u_xlat3.xyz);
					    vs_TEXCOORD4.y = abs(u_xlat3.x) * u_xlat16 + u_xlat2.x;
					    u_xlat3.xyz = u_xlat7.yyy * _EnvMatrix[1].xyz;
					    u_xlat2.xyz = _EnvMatrix[0].xyz * u_xlat7.xxx + u_xlat3.xyz;
					    vs_TEXCOORD5.xyz = _EnvMatrix[2].xyz * u_xlat7.zzz + u_xlat2.xyz;
					    u_xlat2 = (-u_xlat1.yyyy) + unity_4LightPosY0;
					    u_xlat3 = u_xlat0.yyyy * u_xlat2;
					    u_xlat2 = u_xlat2 * u_xlat2;
					    u_xlat4 = (-u_xlat1.xxxx) + unity_4LightPosX0;
					    u_xlat1 = (-u_xlat1.zzzz) + unity_4LightPosZ0;
					    u_xlat3 = u_xlat4 * u_xlat0.xxxx + u_xlat3;
					    u_xlat2 = u_xlat4 * u_xlat4 + u_xlat2;
					    u_xlat2 = u_xlat1 * u_xlat1 + u_xlat2;
					    u_xlat1 = u_xlat1 * u_xlat0.wwzw + u_xlat3;
					    u_xlat2 = max(u_xlat2, vec4(9.99999997e-07, 9.99999997e-07, 9.99999997e-07, 9.99999997e-07));
					    u_xlat3 = inversesqrt(u_xlat2);
					    u_xlat2 = u_xlat2 * unity_4LightAtten0 + vec4(1.0, 1.0, 1.0, 1.0);
					    u_xlat2 = vec4(1.0, 1.0, 1.0, 1.0) / u_xlat2;
					    u_xlat1 = u_xlat1 * u_xlat3;
					    u_xlat1 = max(u_xlat1, vec4(0.0, 0.0, 0.0, 0.0));
					    u_xlat1 = u_xlat2 * u_xlat1;
					    u_xlat2.xyz = u_xlat1.yyy * unity_LightColor[1].xyz;
					    u_xlat2.xyz = unity_LightColor[0].xyz * u_xlat1.xxx + u_xlat2.xyz;
					    u_xlat1.xyz = unity_LightColor[2].xyz * u_xlat1.zzz + u_xlat2.xyz;
					    u_xlat1.xyz = unity_LightColor[3].xyz * u_xlat1.www + u_xlat1.xyz;
					    u_xlat2.xyz = u_xlat1.xyz * vec3(0.305306017, 0.305306017, 0.305306017) + vec3(0.682171106, 0.682171106, 0.682171106);
					    u_xlat2.xyz = u_xlat1.xyz * u_xlat2.xyz + vec3(0.0125228781, 0.0125228781, 0.0125228781);
					    u_xlat16 = u_xlat0.y * u_xlat0.y;
					    u_xlat16 = u_xlat0.x * u_xlat0.x + (-u_xlat16);
					    u_xlat0 = u_xlat0.ywzx * u_xlat0;
					    u_xlat3.x = dot(unity_SHBr, u_xlat0);
					    u_xlat3.y = dot(unity_SHBg, u_xlat0);
					    u_xlat3.z = dot(unity_SHBb, u_xlat0);
					    u_xlat0.xyz = unity_SHC.xyz * vec3(u_xlat16) + u_xlat3.xyz;
					    vs_TEXCOORD6.xyz = u_xlat1.xyz * u_xlat2.xyz + u_xlat0.xyz;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" }
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
						vec4 _LightColor0;
						vec4 unused_0_2;
						float _FaceUVSpeedX;
						float _FaceUVSpeedY;
						vec4 _FaceColor;
						float _OutlineSoftness;
						float _OutlineUVSpeedX;
						float _OutlineUVSpeedY;
						vec4 _OutlineColor;
						float _OutlineWidth;
						vec4 unused_0_11[15];
						float _ScaleRatioA;
						vec4 unused_0_13[9];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_2_1[45];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_2_3;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _FaceTex;
					uniform  sampler2D _OutlineTex;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD4;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					vec3 u_xlat5;
					vec3 u_xlat6;
					float u_xlat8;
					float u_xlat12;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0.x = (-u_xlat0.w) + 0.5;
					    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD4.x);
					    u_xlat0.x = u_xlat0.x * vs_TEXCOORD4.y + 0.5;
					    u_xlat4.x = _OutlineWidth * _ScaleRatioA;
					    u_xlat4.y = _OutlineSoftness * _ScaleRatioA;
					    u_xlat4.xz = u_xlat4.xy * vs_TEXCOORD4.yy;
					    u_xlat1 = vs_COLOR0 * _FaceColor;
					    u_xlat2.x = vs_COLOR0.w * _OutlineColor.w;
					    u_xlat6.xy = vec2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + vs_TEXCOORD0.zw;
					    u_xlat3 = texture(_FaceTex, u_xlat6.xy);
					    u_xlat1 = u_xlat1 * u_xlat3;
					    u_xlat6.xy = vec2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + vs_TEXCOORD0.zw;
					    u_xlat3 = texture(_OutlineTex, u_xlat6.xy);
					    u_xlat6.xyz = u_xlat3.xyz * _OutlineColor.xyz;
					    u_xlat3.w = u_xlat2.x * u_xlat3.w;
					    u_xlat2.x = (-u_xlat4.x) * 0.5 + u_xlat0.x;
					    u_xlat12 = u_xlat4.z * 0.5 + u_xlat2.x;
					    u_xlat8 = u_xlat4.y * vs_TEXCOORD4.y + 1.0;
					    u_xlat8 = u_xlat12 / u_xlat8;
					    u_xlat8 = clamp(u_xlat8, 0.0, 1.0);
					    u_xlat8 = (-u_xlat8) + 1.0;
					    u_xlat0.x = u_xlat4.x * 0.5 + u_xlat0.x;
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat4.x = min(u_xlat4.x, 1.0);
					    u_xlat4.x = sqrt(u_xlat4.x);
					    u_xlat0.x = u_xlat4.x * u_xlat0.x;
					    u_xlat1.xyz = u_xlat1.www * u_xlat1.xyz;
					    u_xlat3.xyz = u_xlat6.xyz * u_xlat3.www;
					    u_xlat2 = (-u_xlat1) + u_xlat3;
					    u_xlat1 = u_xlat0.xxxx * u_xlat2 + u_xlat1;
					    u_xlat0 = vec4(u_xlat8) * u_xlat1;
					    u_xlat1.x = max(u_xlat0.w, 9.99999975e-05);
					    u_xlat0.xyz = u_xlat0.xyz / u_xlat1.xxx;
					    u_xlatb1 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb1){
					        u_xlatb1 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat5.xyz = vs_TEXCOORD2.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD1.www + u_xlat5.xyz;
					        u_xlat5.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.www + u_xlat5.xyz;
					        u_xlat5.xyz = u_xlat5.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat6.x = vs_TEXCOORD1.w;
					        u_xlat6.y = vs_TEXCOORD2.w;
					        u_xlat6.z = vs_TEXCOORD3.w;
					        u_xlat1.xyz = (bool(u_xlatb1)) ? u_xlat5.xyz : u_xlat6.xyz;
					        u_xlat1.xyz = u_xlat1.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat1.yzw = u_xlat1.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat5.x = u_xlat1.y * 0.25 + 0.75;
					        u_xlat2.x = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat1.x = max(u_xlat5.x, u_xlat2.x);
					        u_xlat1 = texture(unity_ProbeVolumeSH, u_xlat1.xzw);
					    } else {
					        u_xlat1.x = float(1.0);
					        u_xlat1.y = float(1.0);
					        u_xlat1.z = float(1.0);
					        u_xlat1.w = float(1.0);
					    }
					    u_xlat1.x = dot(u_xlat1, unity_OcclusionMaskSelector);
					    u_xlat1.x = clamp(u_xlat1.x, 0.0, 1.0);
					    u_xlat2.x = vs_TEXCOORD1.z;
					    u_xlat2.y = vs_TEXCOORD2.z;
					    u_xlat2.z = vs_TEXCOORD3.z;
					    u_xlat5.x = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat5.x = inversesqrt(u_xlat5.x);
					    u_xlat5.xyz = u_xlat5.xxx * u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat1.xxx * _LightColor0.xyz;
					    u_xlat1.x = dot(u_xlat5.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat1.x = max(u_xlat1.x, 0.0);
					    u_xlat0.xyz = u_xlat0.xyz * u_xlat2.xyz;
					    SV_Target0.xyz = u_xlat1.xxx * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "LIGHTPROBE_SH" }
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
						vec4 _LightColor0;
						vec4 unused_0_2;
						float _FaceUVSpeedX;
						float _FaceUVSpeedY;
						vec4 _FaceColor;
						float _OutlineSoftness;
						float _OutlineUVSpeedX;
						float _OutlineUVSpeedY;
						vec4 _OutlineColor;
						float _OutlineWidth;
						vec4 unused_0_11[15];
						float _ScaleRatioA;
						vec4 unused_0_13[9];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 _Time;
						vec4 unused_1_1[8];
					};
					layout(std140) uniform UnityLighting {
						vec4 _WorldSpaceLightPos0;
						vec4 unused_2_1[38];
						vec4 unity_SHAr;
						vec4 unity_SHAg;
						vec4 unity_SHAb;
						vec4 unused_2_5[4];
						vec4 unity_OcclusionMaskSelector;
						vec4 unused_2_7;
					};
					layout(std140) uniform UnityProbeVolume {
						vec4 unity_ProbeVolumeParams;
						mat4x4 unity_ProbeVolumeWorldToObject;
						vec3 unity_ProbeVolumeSizeInv;
						vec3 unity_ProbeVolumeMin;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _FaceTex;
					uniform  sampler2D _OutlineTex;
					uniform  sampler3D unity_ProbeVolumeSH;
					in  vec4 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD4;
					in  vec3 vs_TEXCOORD6;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec4 u_xlat5;
					vec3 u_xlat6;
					vec3 u_xlat7;
					bool u_xlatb7;
					vec3 u_xlat8;
					vec3 u_xlat9;
					vec3 u_xlat10;
					float u_xlat12;
					float u_xlat13;
					float u_xlat18;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0.x = (-u_xlat0.w) + 0.5;
					    u_xlat0.x = u_xlat0.x + (-vs_TEXCOORD4.x);
					    u_xlat0.x = u_xlat0.x * vs_TEXCOORD4.y + 0.5;
					    u_xlat6.x = _OutlineWidth * _ScaleRatioA;
					    u_xlat6.y = _OutlineSoftness * _ScaleRatioA;
					    u_xlat6.xz = u_xlat6.xy * vs_TEXCOORD4.yy;
					    u_xlat1 = vs_COLOR0 * _FaceColor;
					    u_xlat2.x = vs_COLOR0.w * _OutlineColor.w;
					    u_xlat8.xy = vec2(_FaceUVSpeedX, _FaceUVSpeedY) * _Time.yy + vs_TEXCOORD0.zw;
					    u_xlat3 = texture(_FaceTex, u_xlat8.xy);
					    u_xlat1 = u_xlat1 * u_xlat3;
					    u_xlat8.xy = vec2(_OutlineUVSpeedX, _OutlineUVSpeedY) * _Time.yy + vs_TEXCOORD0.zw;
					    u_xlat3 = texture(_OutlineTex, u_xlat8.xy);
					    u_xlat8.xyz = u_xlat3.xyz * _OutlineColor.xyz;
					    u_xlat3.w = u_xlat2.x * u_xlat3.w;
					    u_xlat2.x = (-u_xlat6.x) * 0.5 + u_xlat0.x;
					    u_xlat18 = u_xlat6.z * 0.5 + u_xlat2.x;
					    u_xlat12 = u_xlat6.y * vs_TEXCOORD4.y + 1.0;
					    u_xlat12 = u_xlat18 / u_xlat12;
					    u_xlat12 = clamp(u_xlat12, 0.0, 1.0);
					    u_xlat12 = (-u_xlat12) + 1.0;
					    u_xlat0.x = u_xlat6.x * 0.5 + u_xlat0.x;
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat6.x = min(u_xlat6.x, 1.0);
					    u_xlat6.x = sqrt(u_xlat6.x);
					    u_xlat0.x = u_xlat6.x * u_xlat0.x;
					    u_xlat1.xyz = u_xlat1.www * u_xlat1.xyz;
					    u_xlat3.xyz = u_xlat8.xyz * u_xlat3.www;
					    u_xlat2 = (-u_xlat1) + u_xlat3;
					    u_xlat1 = u_xlat0.xxxx * u_xlat2 + u_xlat1;
					    u_xlat0 = vec4(u_xlat12) * u_xlat1;
					    u_xlat1.x = max(u_xlat0.w, 9.99999975e-05);
					    u_xlat0.xyz = u_xlat0.xyz / u_xlat1.xxx;
					    u_xlatb1 = unity_ProbeVolumeParams.x==1.0;
					    if(u_xlatb1){
					        u_xlatb7 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat2.xyz = vs_TEXCOORD2.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat2.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD1.www + u_xlat2.xyz;
					        u_xlat2.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.www + u_xlat2.xyz;
					        u_xlat2.xyz = u_xlat2.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat9.x = vs_TEXCOORD1.w;
					        u_xlat9.y = vs_TEXCOORD2.w;
					        u_xlat9.z = vs_TEXCOORD3.w;
					        u_xlat7.xyz = (bool(u_xlatb7)) ? u_xlat2.xyz : u_xlat9.xyz;
					        u_xlat7.xyz = u_xlat7.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat2.yzw = u_xlat7.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat7.x = u_xlat2.y * 0.25 + 0.75;
					        u_xlat13 = unity_ProbeVolumeParams.z * 0.5 + 0.75;
					        u_xlat2.x = max(u_xlat13, u_xlat7.x);
					        u_xlat2 = texture(unity_ProbeVolumeSH, u_xlat2.xzw);
					    } else {
					        u_xlat2.x = float(1.0);
					        u_xlat2.y = float(1.0);
					        u_xlat2.z = float(1.0);
					        u_xlat2.w = float(1.0);
					    }
					    u_xlat7.x = dot(u_xlat2, unity_OcclusionMaskSelector);
					    u_xlat7.x = clamp(u_xlat7.x, 0.0, 1.0);
					    u_xlat2.x = vs_TEXCOORD1.z;
					    u_xlat2.y = vs_TEXCOORD2.z;
					    u_xlat2.z = vs_TEXCOORD3.z;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat7.xyz = u_xlat7.xxx * _LightColor0.xyz;
					    if(u_xlatb1){
					        u_xlatb1 = unity_ProbeVolumeParams.y==1.0;
					        u_xlat3.xyz = vs_TEXCOORD2.www * unity_ProbeVolumeWorldToObject[1].xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[0].xyz * vs_TEXCOORD1.www + u_xlat3.xyz;
					        u_xlat3.xyz = unity_ProbeVolumeWorldToObject[2].xyz * vs_TEXCOORD3.www + u_xlat3.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + unity_ProbeVolumeWorldToObject[3].xyz;
					        u_xlat10.x = vs_TEXCOORD1.w;
					        u_xlat10.y = vs_TEXCOORD2.w;
					        u_xlat10.z = vs_TEXCOORD3.w;
					        u_xlat3.xyz = (bool(u_xlatb1)) ? u_xlat3.xyz : u_xlat10.xyz;
					        u_xlat3.xyz = u_xlat3.xyz + (-unity_ProbeVolumeMin.xyz);
					        u_xlat3.yzw = u_xlat3.xyz * unity_ProbeVolumeSizeInv.xyz;
					        u_xlat1.x = u_xlat3.y * 0.25;
					        u_xlat9.x = unity_ProbeVolumeParams.z * 0.5;
					        u_xlat4.x = (-unity_ProbeVolumeParams.z) * 0.5 + 0.25;
					        u_xlat1.x = max(u_xlat1.x, u_xlat9.x);
					        u_xlat3.x = min(u_xlat4.x, u_xlat1.x);
					        u_xlat4 = texture(unity_ProbeVolumeSH, u_xlat3.xzw);
					        u_xlat5.xyz = u_xlat3.xzw + vec3(0.25, 0.0, 0.0);
					        u_xlat5 = texture(unity_ProbeVolumeSH, u_xlat5.xyz);
					        u_xlat3.xyz = u_xlat3.xzw + vec3(0.5, 0.0, 0.0);
					        u_xlat3 = texture(unity_ProbeVolumeSH, u_xlat3.xyz);
					        u_xlat2.w = 1.0;
					        u_xlat4.x = dot(u_xlat4, u_xlat2);
					        u_xlat4.y = dot(u_xlat5, u_xlat2);
					        u_xlat4.z = dot(u_xlat3, u_xlat2);
					    } else {
					        u_xlat2.w = 1.0;
					        u_xlat4.x = dot(unity_SHAr, u_xlat2);
					        u_xlat4.y = dot(unity_SHAg, u_xlat2);
					        u_xlat4.z = dot(unity_SHAb, u_xlat2);
					    }
					    u_xlat3.xyz = u_xlat4.xyz + vs_TEXCOORD6.xyz;
					    u_xlat3.xyz = max(u_xlat3.xyz, vec3(0.0, 0.0, 0.0));
					    u_xlat3.xyz = log2(u_xlat3.xyz);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(0.416666657, 0.416666657, 0.416666657);
					    u_xlat3.xyz = exp2(u_xlat3.xyz);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(1.05499995, 1.05499995, 1.05499995) + vec3(-0.0549999997, -0.0549999997, -0.0549999997);
					    u_xlat3.xyz = max(u_xlat3.xyz, vec3(0.0, 0.0, 0.0));
					    u_xlat1.x = dot(u_xlat2.xyz, _WorldSpaceLightPos0.xyz);
					    u_xlat1.x = max(u_xlat1.x, 0.0);
					    u_xlat7.xyz = u_xlat0.xyz * u_xlat7.xyz;
					    u_xlat0.xyz = u_xlat0.xyz * u_xlat3.xyz;
					    SV_Target0.xyz = u_xlat7.xyz * u_xlat1.xxx + u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
		Pass {
			Name "CASTER"
			LOD 300
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "SHADOWCASTER" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			ColorMask RGB -1
			Cull Off
			Offset 1, 1
			Fog {
				Mode Off
			}
			GpuProgramID 72001
			Program "vp" {
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_DEPTH" }
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
						vec4 unused_0_0[2];
						vec4 _MainTex_ST;
						float _OutlineWidth;
						float _FaceDilate;
						float _ScaleRatioA;
					};
					layout(std140) uniform UnityShadows {
						vec4 unused_1_0[5];
						vec4 unity_LightShadowBias;
						vec4 unused_1_2[20];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					out float vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat4;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    u_xlat1.x = unity_LightShadowBias.x / u_xlat0.w;
					    u_xlat1.x = min(u_xlat1.x, 0.0);
					    u_xlat1.x = max(u_xlat1.x, -1.0);
					    u_xlat4 = u_xlat0.z + u_xlat1.x;
					    u_xlat1.x = min(u_xlat0.w, u_xlat4);
					    gl_Position.xyw = u_xlat0.xyw;
					    u_xlat0.x = (-u_xlat4) + u_xlat1.x;
					    gl_Position.z = unity_LightShadowBias.y * u_xlat0.x + u_xlat4;
					    u_xlat0.x = (-_OutlineWidth) * _ScaleRatioA + 1.0;
					    u_xlat0.x = (-_FaceDilate) * _ScaleRatioA + u_xlat0.x;
					    vs_TEXCOORD2 = u_xlat0.x * 0.5;
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_CUBE" }
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
						vec4 unused_0_0[2];
						vec4 _MainTex_ST;
						float _OutlineWidth;
						float _FaceDilate;
						float _ScaleRatioA;
					};
					layout(std140) uniform UnityShadows {
						vec4 unused_1_0[5];
						vec4 unity_LightShadowBias;
						vec4 unused_1_2[20];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					out float vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    u_xlat1.x = min(u_xlat0.w, u_xlat0.z);
					    u_xlat1.x = (-u_xlat0.z) + u_xlat1.x;
					    gl_Position.z = unity_LightShadowBias.y * u_xlat1.x + u_xlat0.z;
					    gl_Position.xyw = u_xlat0.xyw;
					    u_xlat0.x = (-_OutlineWidth) * _ScaleRatioA + 1.0;
					    u_xlat0.x = (-_FaceDilate) * _ScaleRatioA + u_xlat0.x;
					    vs_TEXCOORD2 = u_xlat0.x * 0.5;
					    vs_TEXCOORD1.xy = in_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    return;
					}"
				}
			}
			Program "fp" {
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_DEPTH" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD1;
					in  float vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat0.x = u_xlat0.w + (-vs_TEXCOORD2);
					    u_xlatb0 = u_xlat0.x<0.0;
					    if(((int(u_xlatb0) * int(0xffffffffu)))!=0){discard;}
					    SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "SHADOWS_CUBE" }
					"ps_4_0
					
					#version 330
					#extension GL_ARB_explicit_attrib_location : require
					#extension GL_ARB_explicit_uniform_location : require
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD1;
					in  float vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat0.x = u_xlat0.w + (-vs_TEXCOORD2);
					    u_xlatb0 = u_xlat0.x<0.0;
					    if(((int(u_xlatb0) * int(0xffffffffu)))!=0){discard;}
					    SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
					    return;
					}"
				}
			}
		}
	}
	CustomEditor "TMPro_SDFMaterialEditor"
}