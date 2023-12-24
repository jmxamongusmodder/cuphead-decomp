Shader "TMPro/Mobile/Distance Field" {
	Properties {
		_FaceColor ("Face Color", Vector) = (1,1,1,1)
		_FaceDilate ("Face Dilate", Range(-1, 1)) = 0
		_OutlineColor ("Outline Color", Vector) = (0,0,0,1)
		_OutlineWidth ("Outline Thickness", Range(0, 1)) = 0
		_OutlineSoftness ("Outline Softness", Range(0, 1)) = 0
		_UnderlayColor ("Border Color", Vector) = (0,0,0,0.5)
		_UnderlayOffsetX ("Border OffsetX", Range(-1, 1)) = 0
		_UnderlayOffsetY ("Border OffsetY", Range(-1, 1)) = 0
		_UnderlayDilate ("Border Dilate", Range(-1, 1)) = 0
		_UnderlaySoftness ("Border Softness", Range(0, 1)) = 0
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
		_MaskTex ("Mask Texture", 2D) = "white" {}
		_MaskCoord ("Mask Coordinates", Vector) = (0,0,100000,100000)
		_ClipRect ("Clip Rect", Vector) = (-100000,-100000,100000,100000)
		_MaskSoftnessX ("Mask SoftnessX", Float) = 0
		_MaskSoftnessY ("Mask SoftnessY", Float) = 0
		_StencilComp ("Stencil Comparison", Float) = 8
		_Stencil ("Stencil ID", Float) = 0
		_StencilOp ("Stencil Operation", Float) = 0
		_StencilWriteMask ("Stencil Write Mask", Float) = 255
		_StencilReadMask ("Stencil Read Mask", Float) = 255
		_ColorMask ("Color Mask", Float) = 15
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ColorMask 0 -1
			ZWrite Off
			Cull Off
			Stencil {
				ReadMask 0
				WriteMask 0
				Comp Disabled
				Pass Keep
				Fail Keep
				ZFail Keep
			}
			Fog {
				Mode Off
			}
			GpuProgramID 338
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
						vec4 unused_0_6[15];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_12[4];
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
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					float u_xlat5;
					float u_xlat8;
					float u_xlat12;
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
					    u_xlat2 = in_COLOR0 * _FaceColor;
					    u_xlat2.xyz = u_xlat2.www * u_xlat2.xyz;
					    vs_COLOR0 = u_xlat2;
					    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz;
					    u_xlat1.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat12 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat1.xyz = vec3(u_xlat12) * u_xlat1.xyz;
					    u_xlat0.x = dot(u_xlat1.xyz, u_xlat0.xyz);
					    u_xlat4.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat4.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat4.xy;
					    u_xlat4.xy = abs(u_xlat4.xy) * vec2(_ScaleX, _ScaleY);
					    u_xlat4.xy = u_xlat1.ww / u_xlat4.xy;
					    u_xlat12 = dot(u_xlat4.xy, u_xlat4.xy);
					    vs_TEXCOORD2.zw = vec2(0.5, 0.5) / u_xlat4.xy;
					    u_xlat4.x = inversesqrt(u_xlat12);
					    u_xlat8 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat4.x = u_xlat4.x * u_xlat8;
					    u_xlat8 = u_xlat4.x * 1.5;
					    u_xlat12 = (-_PerspectiveFilter) + 1.0;
					    u_xlat12 = u_xlat12 * u_xlat8;
					    u_xlat4.x = u_xlat4.x * 1.5 + (-u_xlat12);
					    u_xlat0.x = abs(u_xlat0.x) * u_xlat4.x + u_xlat12;
					    u_xlatb4 = glstate_matrix_projection[3].w==0.0;
					    u_xlat0.x = (u_xlatb4) ? u_xlat0.x : u_xlat8;
					    u_xlat4.xy = vec2(_FaceDilate, _OutlineSoftness) * vec2(vec2(_ScaleRatioA, _ScaleRatioA));
					    u_xlat8 = u_xlat4.y * u_xlat0.x + 1.0;
					    u_xlat0.x = u_xlat0.x / u_xlat8;
					    u_xlat8 = _OutlineWidth * _ScaleRatioA;
					    u_xlat8 = u_xlat8 * 0.5;
					    u_xlat1.x = u_xlat0.x * u_xlat8;
					    u_xlat1.x = u_xlat1.x + u_xlat1.x;
					    u_xlat1.x = min(u_xlat1.x, 1.0);
					    u_xlat1.x = sqrt(u_xlat1.x);
					    u_xlat3.w = in_COLOR0.w * _OutlineColor.w;
					    u_xlat3.xyz = u_xlat3.www * _OutlineColor.xyz;
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    vs_COLOR1 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlatb1 = 0.0>=in_TEXCOORD1.y;
					    u_xlat1.x = u_xlatb1 ? 1.0 : float(0.0);
					    u_xlat5 = (-_WeightNormal) + _WeightBold;
					    u_xlat1.x = u_xlat1.x * u_xlat5 + _WeightNormal;
					    u_xlat1.x = u_xlat1.x / _GradientScale;
					    u_xlat4.x = u_xlat4.x * 0.5 + u_xlat1.x;
					    u_xlat4.x = (-u_xlat4.x) + 0.5;
					    u_xlat0.w = u_xlat4.x * u_xlat0.x + -0.5;
					    vs_TEXCOORD1.y = (-u_xlat8) * u_xlat0.x + u_xlat0.w;
					    vs_TEXCOORD1.z = u_xlat8 * u_xlat0.x + u_xlat0.w;
					    vs_TEXCOORD1.xw = u_xlat0.xw;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "UNDERLAY_ON" }
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
						vec4 unused_0_6[11];
						vec4 _UnderlayColor;
						float _UnderlayOffsetX;
						float _UnderlayOffsetY;
						float _UnderlayDilate;
						float _UnderlaySoftness;
						vec4 unused_0_12[2];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _ScaleRatioC;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_19[3];
						float _TextureWidth;
						float _TextureHeight;
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
					out vec2 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD4;
					out vec2 vs_TEXCOORD5;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat5;
					float u_xlat8;
					float u_xlat12;
					bool u_xlatb12;
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
					    u_xlat2 = in_COLOR0 * _FaceColor;
					    u_xlat2.xyz = u_xlat2.www * u_xlat2.xyz;
					    vs_COLOR0 = u_xlat2;
					    u_xlat3.w = in_COLOR0.w * _OutlineColor.w;
					    u_xlat3.xyz = u_xlat3.www * _OutlineColor.xyz;
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz;
					    u_xlat1.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat12 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat1.xyz = vec3(u_xlat12) * u_xlat1.xyz;
					    u_xlat0.x = dot(u_xlat1.xyz, u_xlat0.xyz);
					    u_xlat4.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat4.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat4.xy;
					    u_xlat4.xy = abs(u_xlat4.xy) * vec2(_ScaleX, _ScaleY);
					    u_xlat4.xy = u_xlat1.ww / u_xlat4.xy;
					    u_xlat12 = dot(u_xlat4.xy, u_xlat4.xy);
					    vs_TEXCOORD2.zw = vec2(0.5, 0.5) / u_xlat4.xy;
					    u_xlat4.x = inversesqrt(u_xlat12);
					    u_xlat8 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat4.x = u_xlat4.x * u_xlat8;
					    u_xlat8 = u_xlat4.x * 1.5;
					    u_xlat12 = (-_PerspectiveFilter) + 1.0;
					    u_xlat12 = u_xlat12 * u_xlat8;
					    u_xlat4.x = u_xlat4.x * 1.5 + (-u_xlat12);
					    u_xlat0.x = abs(u_xlat0.x) * u_xlat4.x + u_xlat12;
					    u_xlatb4 = glstate_matrix_projection[3].w==0.0;
					    u_xlat0.x = (u_xlatb4) ? u_xlat0.x : u_xlat8;
					    u_xlat4.xy = vec2(_FaceDilate, _OutlineSoftness) * vec2(vec2(_ScaleRatioA, _ScaleRatioA));
					    u_xlat8 = u_xlat4.y * u_xlat0.x + 1.0;
					    u_xlat1.x = u_xlat0.x / u_xlat8;
					    u_xlat8 = _OutlineWidth * _ScaleRatioA;
					    u_xlat8 = u_xlat8 * 0.5;
					    u_xlat12 = u_xlat1.x * u_xlat8;
					    u_xlat12 = u_xlat12 + u_xlat12;
					    u_xlat12 = min(u_xlat12, 1.0);
					    u_xlat12 = sqrt(u_xlat12);
					    vs_COLOR1 = vec4(u_xlat12) * u_xlat3 + u_xlat2;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2 = vec4(_UnderlaySoftness, _UnderlayDilate, _UnderlayOffsetX, _UnderlayOffsetY) * vec4(vec4(_ScaleRatioC, _ScaleRatioC, _ScaleRatioC, _ScaleRatioC));
					    u_xlat5.xy = (-u_xlat2.zw) * vec2(_GradientScale);
					    u_xlat5.xy = u_xlat5.xy / vec2(_TextureWidth, _TextureHeight);
					    vs_TEXCOORD3.xy = u_xlat5.xy + in_TEXCOORD0.xy;
					    u_xlatb12 = 0.0>=in_TEXCOORD1.y;
					    u_xlat12 = u_xlatb12 ? 1.0 : float(0.0);
					    u_xlat5.x = (-_WeightNormal) + _WeightBold;
					    u_xlat12 = u_xlat12 * u_xlat5.x + _WeightNormal;
					    u_xlat12 = u_xlat12 / _GradientScale;
					    u_xlat4.x = u_xlat4.x * 0.5 + u_xlat12;
					    u_xlat4.x = (-u_xlat4.x) + 0.5;
					    u_xlat1.w = u_xlat4.x * u_xlat1.x + -0.5;
					    vs_TEXCOORD1.y = (-u_xlat8) * u_xlat1.x + u_xlat1.w;
					    vs_TEXCOORD1.z = u_xlat8 * u_xlat1.x + u_xlat1.w;
					    vs_TEXCOORD1.xw = u_xlat1.xw;
					    u_xlat8 = in_COLOR0.w * _UnderlayColor.w;
					    vs_TEXCOORD4.xyz = vec3(u_xlat8) * _UnderlayColor.xyz;
					    vs_TEXCOORD4.w = u_xlat8;
					    u_xlat8 = u_xlat2.x * u_xlat0.x + 1.0;
					    u_xlat0.x = u_xlat0.x / u_xlat8;
					    u_xlat8 = u_xlat2.y * 0.5;
					    u_xlat4.x = u_xlat4.x * u_xlat0.x + -0.5;
					    vs_TEXCOORD5.y = (-u_xlat8) * u_xlat0.x + u_xlat4.x;
					    vs_TEXCOORD5.x = u_xlat0.x;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "MASK_SOFT" }
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
						vec4 unused_0_6[15];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_12[4];
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
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					float u_xlat5;
					float u_xlat8;
					float u_xlat12;
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
					    u_xlat2 = in_COLOR0 * _FaceColor;
					    u_xlat2.xyz = u_xlat2.www * u_xlat2.xyz;
					    vs_COLOR0 = u_xlat2;
					    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz;
					    u_xlat1.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat12 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat1.xyz = vec3(u_xlat12) * u_xlat1.xyz;
					    u_xlat0.x = dot(u_xlat1.xyz, u_xlat0.xyz);
					    u_xlat4.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat4.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat4.xy;
					    u_xlat4.xy = abs(u_xlat4.xy) * vec2(_ScaleX, _ScaleY);
					    u_xlat4.xy = u_xlat1.ww / u_xlat4.xy;
					    u_xlat12 = dot(u_xlat4.xy, u_xlat4.xy);
					    vs_TEXCOORD2.zw = vec2(0.5, 0.5) / u_xlat4.xy;
					    u_xlat4.x = inversesqrt(u_xlat12);
					    u_xlat8 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat4.x = u_xlat4.x * u_xlat8;
					    u_xlat8 = u_xlat4.x * 1.5;
					    u_xlat12 = (-_PerspectiveFilter) + 1.0;
					    u_xlat12 = u_xlat12 * u_xlat8;
					    u_xlat4.x = u_xlat4.x * 1.5 + (-u_xlat12);
					    u_xlat0.x = abs(u_xlat0.x) * u_xlat4.x + u_xlat12;
					    u_xlatb4 = glstate_matrix_projection[3].w==0.0;
					    u_xlat0.x = (u_xlatb4) ? u_xlat0.x : u_xlat8;
					    u_xlat4.xy = vec2(_FaceDilate, _OutlineSoftness) * vec2(vec2(_ScaleRatioA, _ScaleRatioA));
					    u_xlat8 = u_xlat4.y * u_xlat0.x + 1.0;
					    u_xlat0.x = u_xlat0.x / u_xlat8;
					    u_xlat8 = _OutlineWidth * _ScaleRatioA;
					    u_xlat8 = u_xlat8 * 0.5;
					    u_xlat1.x = u_xlat0.x * u_xlat8;
					    u_xlat1.x = u_xlat1.x + u_xlat1.x;
					    u_xlat1.x = min(u_xlat1.x, 1.0);
					    u_xlat1.x = sqrt(u_xlat1.x);
					    u_xlat3.w = in_COLOR0.w * _OutlineColor.w;
					    u_xlat3.xyz = u_xlat3.www * _OutlineColor.xyz;
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    vs_COLOR1 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlatb1 = 0.0>=in_TEXCOORD1.y;
					    u_xlat1.x = u_xlatb1 ? 1.0 : float(0.0);
					    u_xlat5 = (-_WeightNormal) + _WeightBold;
					    u_xlat1.x = u_xlat1.x * u_xlat5 + _WeightNormal;
					    u_xlat1.x = u_xlat1.x / _GradientScale;
					    u_xlat4.x = u_xlat4.x * 0.5 + u_xlat1.x;
					    u_xlat4.x = (-u_xlat4.x) + 0.5;
					    u_xlat0.w = u_xlat4.x * u_xlat0.x + -0.5;
					    vs_TEXCOORD1.y = (-u_xlat8) * u_xlat0.x + u_xlat0.w;
					    vs_TEXCOORD1.z = u_xlat8 * u_xlat0.x + u_xlat0.w;
					    vs_TEXCOORD1.xw = u_xlat0.xw;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "UNDERLAY_ON" "MASK_SOFT" }
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
						vec4 unused_0_6[11];
						vec4 _UnderlayColor;
						float _UnderlayOffsetX;
						float _UnderlayOffsetY;
						float _UnderlayDilate;
						float _UnderlaySoftness;
						vec4 unused_0_12[2];
						float _WeightNormal;
						float _WeightBold;
						float _ScaleRatioA;
						float _ScaleRatioC;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_19[3];
						float _TextureWidth;
						float _TextureHeight;
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
					out vec2 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD3;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD4;
					out vec2 vs_TEXCOORD5;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec2 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat5;
					float u_xlat8;
					float u_xlat12;
					bool u_xlatb12;
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
					    u_xlat2 = in_COLOR0 * _FaceColor;
					    u_xlat2.xyz = u_xlat2.www * u_xlat2.xyz;
					    vs_COLOR0 = u_xlat2;
					    u_xlat3.w = in_COLOR0.w * _OutlineColor.w;
					    u_xlat3.xyz = u_xlat3.www * _OutlineColor.xyz;
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat0.xyz = vec3(u_xlat12) * u_xlat0.xyz;
					    u_xlat1.x = dot(in_NORMAL0.xyz, unity_WorldToObject[0].xyz);
					    u_xlat1.y = dot(in_NORMAL0.xyz, unity_WorldToObject[1].xyz);
					    u_xlat1.z = dot(in_NORMAL0.xyz, unity_WorldToObject[2].xyz);
					    u_xlat12 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat1.xyz = vec3(u_xlat12) * u_xlat1.xyz;
					    u_xlat0.x = dot(u_xlat1.xyz, u_xlat0.xyz);
					    u_xlat4.xy = _ScreenParams.yy * glstate_matrix_projection[1].xy;
					    u_xlat4.xy = glstate_matrix_projection[0].xy * _ScreenParams.xx + u_xlat4.xy;
					    u_xlat4.xy = abs(u_xlat4.xy) * vec2(_ScaleX, _ScaleY);
					    u_xlat4.xy = u_xlat1.ww / u_xlat4.xy;
					    u_xlat12 = dot(u_xlat4.xy, u_xlat4.xy);
					    vs_TEXCOORD2.zw = vec2(0.5, 0.5) / u_xlat4.xy;
					    u_xlat4.x = inversesqrt(u_xlat12);
					    u_xlat8 = abs(in_TEXCOORD1.y) * _GradientScale;
					    u_xlat4.x = u_xlat4.x * u_xlat8;
					    u_xlat8 = u_xlat4.x * 1.5;
					    u_xlat12 = (-_PerspectiveFilter) + 1.0;
					    u_xlat12 = u_xlat12 * u_xlat8;
					    u_xlat4.x = u_xlat4.x * 1.5 + (-u_xlat12);
					    u_xlat0.x = abs(u_xlat0.x) * u_xlat4.x + u_xlat12;
					    u_xlatb4 = glstate_matrix_projection[3].w==0.0;
					    u_xlat0.x = (u_xlatb4) ? u_xlat0.x : u_xlat8;
					    u_xlat4.xy = vec2(_FaceDilate, _OutlineSoftness) * vec2(vec2(_ScaleRatioA, _ScaleRatioA));
					    u_xlat8 = u_xlat4.y * u_xlat0.x + 1.0;
					    u_xlat1.x = u_xlat0.x / u_xlat8;
					    u_xlat8 = _OutlineWidth * _ScaleRatioA;
					    u_xlat8 = u_xlat8 * 0.5;
					    u_xlat12 = u_xlat1.x * u_xlat8;
					    u_xlat12 = u_xlat12 + u_xlat12;
					    u_xlat12 = min(u_xlat12, 1.0);
					    u_xlat12 = sqrt(u_xlat12);
					    vs_COLOR1 = vec4(u_xlat12) * u_xlat3 + u_xlat2;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat2 = vec4(_UnderlaySoftness, _UnderlayDilate, _UnderlayOffsetX, _UnderlayOffsetY) * vec4(vec4(_ScaleRatioC, _ScaleRatioC, _ScaleRatioC, _ScaleRatioC));
					    u_xlat5.xy = (-u_xlat2.zw) * vec2(_GradientScale);
					    u_xlat5.xy = u_xlat5.xy / vec2(_TextureWidth, _TextureHeight);
					    vs_TEXCOORD3.xy = u_xlat5.xy + in_TEXCOORD0.xy;
					    u_xlatb12 = 0.0>=in_TEXCOORD1.y;
					    u_xlat12 = u_xlatb12 ? 1.0 : float(0.0);
					    u_xlat5.x = (-_WeightNormal) + _WeightBold;
					    u_xlat12 = u_xlat12 * u_xlat5.x + _WeightNormal;
					    u_xlat12 = u_xlat12 / _GradientScale;
					    u_xlat4.x = u_xlat4.x * 0.5 + u_xlat12;
					    u_xlat4.x = (-u_xlat4.x) + 0.5;
					    u_xlat1.w = u_xlat4.x * u_xlat1.x + -0.5;
					    vs_TEXCOORD1.y = (-u_xlat8) * u_xlat1.x + u_xlat1.w;
					    vs_TEXCOORD1.z = u_xlat8 * u_xlat1.x + u_xlat1.w;
					    vs_TEXCOORD1.xw = u_xlat1.xw;
					    u_xlat8 = in_COLOR0.w * _UnderlayColor.w;
					    vs_TEXCOORD4.xyz = vec3(u_xlat8) * _UnderlayColor.xyz;
					    vs_TEXCOORD4.w = u_xlat8;
					    u_xlat8 = u_xlat2.x * u_xlat0.x + 1.0;
					    u_xlat0.x = u_xlat0.x / u_xlat8;
					    u_xlat8 = u_xlat2.y * 0.5;
					    u_xlat4.x = u_xlat4.x * u_xlat0.x + -0.5;
					    vs_TEXCOORD5.y = (-u_xlat8) * u_xlat0.x + u_xlat4.x;
					    vs_TEXCOORD5.x = u_xlat0.x;
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
						vec4 unused_0_0[26];
						vec4 _ClipRect;
						vec4 unused_0_2[2];
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec4 u_xlatb0;
					vec4 u_xlat1;
					float u_xlat2;
					void main()
					{
					    u_xlatb0.xy = greaterThanEqual(vs_TEXCOORD2.xyxx, _ClipRect.xyxx).xy;
					    u_xlatb0.zw = greaterThanEqual(_ClipRect.zzzw, vs_TEXCOORD2.xxxy).zw;
					    u_xlat0.x = u_xlatb0.x ? float(1.0) : 0.0;
					    u_xlat0.y = u_xlatb0.y ? float(1.0) : 0.0;
					    u_xlat0.z = u_xlatb0.z ? float(1.0) : 0.0;
					    u_xlat0.w = u_xlatb0.w ? float(1.0) : 0.0;
					;
					    u_xlat0.xy = u_xlat0.zw * u_xlat0.xy;
					    u_xlat0.x = u_xlat0.y * u_xlat0.x;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat2 = u_xlat1.w * vs_TEXCOORD1.x + (-vs_TEXCOORD1.w);
					    u_xlat2 = clamp(u_xlat2, 0.0, 1.0);
					    u_xlat1 = vec4(u_xlat2) * vs_COLOR0;
					    SV_Target0 = u_xlat0.xxxx * u_xlat1;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "UNDERLAY_ON" }
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
						vec4 unused_0_0[26];
						vec4 _ClipRect;
						vec4 unused_0_2[2];
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD4;
					in  vec2 vs_TEXCOORD5;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bvec4 u_xlatb1;
					vec4 u_xlat2;
					float u_xlat4;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat0.x = u_xlat0.w * vs_TEXCOORD5.x + (-vs_TEXCOORD5.y);
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat0 = u_xlat0.xxxx * vs_TEXCOORD4;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat1.xy = u_xlat1.ww * vs_TEXCOORD1.xx + (-vs_TEXCOORD1.wy);
					    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
					    u_xlat4 = (-u_xlat1.y) + 1.0;
					    u_xlat0 = u_xlat0 * vec4(u_xlat4);
					    u_xlat2 = u_xlat1.xxxx * vs_COLOR0;
					    u_xlat1.x = (-vs_COLOR0.w) * u_xlat1.x + 1.0;
					    u_xlat0 = u_xlat0 * u_xlat1.xxxx + u_xlat2;
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
				SubProgram "d3d11 " {
					Keywords { "MASK_SOFT" }
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
						vec4 unused_0_0[25];
						vec4 _MaskCoord;
						vec4 _ClipRect;
						float _MaskSoftnessX;
						float _MaskSoftnessY;
						vec4 unused_0_5;
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec4 u_xlatb0;
					vec4 u_xlat1;
					float u_xlat2;
					vec2 u_xlat5;
					void main()
					{
					    u_xlatb0.xy = greaterThanEqual(vs_TEXCOORD2.xyxx, _ClipRect.xyxx).xy;
					    u_xlatb0.zw = greaterThanEqual(_ClipRect.zzzw, vs_TEXCOORD2.xxxy).zw;
					    u_xlat0.x = u_xlatb0.x ? float(1.0) : 0.0;
					    u_xlat0.y = u_xlatb0.y ? float(1.0) : 0.0;
					    u_xlat0.z = u_xlatb0.z ? float(1.0) : 0.0;
					    u_xlat0.w = u_xlatb0.w ? float(1.0) : 0.0;
					;
					    u_xlat0.xy = u_xlat0.zw * u_xlat0.xy;
					    u_xlat0.x = u_xlat0.y * u_xlat0.x;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat2 = u_xlat1.w * vs_TEXCOORD1.x + (-vs_TEXCOORD1.w);
					    u_xlat2 = clamp(u_xlat2, 0.0, 1.0);
					    u_xlat1 = vec4(u_xlat2) * vs_COLOR0;
					    u_xlat0 = u_xlat0.xxxx * u_xlat1;
					    u_xlat1.xy = vs_TEXCOORD2.xy + (-_MaskCoord.xy);
					    u_xlat1.xy = abs(u_xlat1.xy) + (-_MaskCoord.zw);
					    u_xlat5.xy = vs_TEXCOORD2.zw * vec2(_MaskSoftnessX, _MaskSoftnessY);
					    u_xlat1.xy = u_xlat1.xy * vs_TEXCOORD2.zw + u_xlat5.xy;
					    u_xlat5.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vs_TEXCOORD2.zw + vec2(1.0, 1.0);
					    u_xlat1.xy = u_xlat1.xy / u_xlat5.xy;
					    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
					    u_xlat1.xy = (-u_xlat1.xy) + vec2(1.0, 1.0);
					    u_xlat1.xy = u_xlat1.xy * u_xlat1.xy;
					    u_xlat1.x = u_xlat1.y * u_xlat1.x;
					    SV_Target0 = u_xlat0 * u_xlat1.xxxx;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "UNDERLAY_ON" "MASK_SOFT" }
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
						vec4 unused_0_0[25];
						vec4 _MaskCoord;
						vec4 _ClipRect;
						float _MaskSoftnessX;
						float _MaskSoftnessY;
						vec4 unused_0_5;
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD3;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD4;
					in  vec2 vs_TEXCOORD5;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bvec4 u_xlatb1;
					vec4 u_xlat2;
					float u_xlat4;
					vec2 u_xlat7;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat0.x = u_xlat0.w * vs_TEXCOORD5.x + (-vs_TEXCOORD5.y);
					    u_xlat0.x = clamp(u_xlat0.x, 0.0, 1.0);
					    u_xlat0 = u_xlat0.xxxx * vs_TEXCOORD4;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat1.xy = u_xlat1.ww * vs_TEXCOORD1.xx + (-vs_TEXCOORD1.wy);
					    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
					    u_xlat4 = (-u_xlat1.y) + 1.0;
					    u_xlat0 = u_xlat0 * vec4(u_xlat4);
					    u_xlat2 = u_xlat1.xxxx * vs_COLOR0;
					    u_xlat1.x = (-vs_COLOR0.w) * u_xlat1.x + 1.0;
					    u_xlat0 = u_xlat0 * u_xlat1.xxxx + u_xlat2;
					    u_xlatb1.xy = greaterThanEqual(vs_TEXCOORD2.xyxx, _ClipRect.xyxx).xy;
					    u_xlatb1.zw = greaterThanEqual(_ClipRect.zzzw, vs_TEXCOORD2.xxxy).zw;
					    u_xlat1.x = u_xlatb1.x ? float(1.0) : 0.0;
					    u_xlat1.y = u_xlatb1.y ? float(1.0) : 0.0;
					    u_xlat1.z = u_xlatb1.z ? float(1.0) : 0.0;
					    u_xlat1.w = u_xlatb1.w ? float(1.0) : 0.0;
					;
					    u_xlat1.xy = u_xlat1.zw * u_xlat1.xy;
					    u_xlat1.x = u_xlat1.y * u_xlat1.x;
					    u_xlat0 = u_xlat0 * u_xlat1.xxxx;
					    u_xlat1.xy = vs_TEXCOORD2.xy + (-_MaskCoord.xy);
					    u_xlat1.xy = abs(u_xlat1.xy) + (-_MaskCoord.zw);
					    u_xlat7.xy = vs_TEXCOORD2.zw * vec2(_MaskSoftnessX, _MaskSoftnessY);
					    u_xlat1.xy = u_xlat1.xy * vs_TEXCOORD2.zw + u_xlat7.xy;
					    u_xlat7.xy = vec2(_MaskSoftnessX, _MaskSoftnessY) * vs_TEXCOORD2.zw + vec2(1.0, 1.0);
					    u_xlat1.xy = u_xlat1.xy / u_xlat7.xy;
					    u_xlat1.xy = clamp(u_xlat1.xy, 0.0, 1.0);
					    u_xlat1.xy = (-u_xlat1.xy) + vec2(1.0, 1.0);
					    u_xlat1.xy = u_xlat1.xy * u_xlat1.xy;
					    u_xlat1.x = u_xlat1.y * u_xlat1.x;
					    SV_Target0 = u_xlat0 * u_xlat1.xxxx;
					    return;
					}"
				}
			}
		}
	}
	CustomEditor "TMPro_SDFMaterialEditor"
}