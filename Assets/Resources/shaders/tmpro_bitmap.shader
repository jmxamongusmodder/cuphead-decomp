Shader "TMPro/Bitmap" {
	Properties {
		_MainTex ("Font Atlas", 2D) = "white" {}
		_FaceTex ("Font Texture", 2D) = "white" {}
		_FaceColor ("Text Color", Vector) = (1,1,1,1)
		_VertexOffsetX ("Vertex OffsetX", Float) = 0
		_VertexOffsetY ("Vertex OffsetY", Float) = 0
		_MaskSoftnessX ("Mask SoftnessX", Float) = 0
		_MaskSoftnessY ("Mask SoftnessY", Float) = 0
		_ClipRect ("Clip Rect", Vector) = (-100000,-100000,100000,100000)
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
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
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
			GpuProgramID 37803
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
						vec4 unused_0_0[2];
						vec4 _FaceColor;
						float _VertexOffsetX;
						float _VertexOffsetY;
						vec4 unused_0_4[2];
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[6];
						vec4 _ScreenParams;
						vec4 unused_1_2[2];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_2_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_3_0[5];
						mat4x4 glstate_matrix_projection;
						vec4 unused_3_2[8];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_4[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					in  vec2 in_TEXCOORD1;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec2 u_xlat5;
					void main()
					{
					    u_xlat0.xy = in_POSITION0.xy + vec2(_VertexOffsetX, _VertexOffsetY);
					    u_xlat1 = u_xlat0.yyyy * unity_ObjectToWorld[1];
					    u_xlat1 = unity_ObjectToWorld[0] * u_xlat0.xxxx + u_xlat1;
					    vs_TEXCOORD2.xy = u_xlat0.xy;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat1;
					    u_xlat0 = u_xlat0 + unity_ObjectToWorld[3];
					    u_xlat1 = u_xlat0.yyyy * unity_MatrixVP[1];
					    u_xlat1 = unity_MatrixVP[0] * u_xlat0.xxxx + u_xlat1;
					    u_xlat1 = unity_MatrixVP[2] * u_xlat0.zzzz + u_xlat1;
					    u_xlat0 = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    u_xlat0.xy = u_xlat0.xy / u_xlat0.ww;
					    u_xlat1.xy = _ScreenParams.xy * vec2(0.5, 0.5);
					    u_xlat0.xy = u_xlat0.xy * u_xlat1.xy;
					    u_xlat0.xy = roundEven(u_xlat0.xy);
					    u_xlat0.xy = u_xlat0.xy / u_xlat1.xy;
					    gl_Position.xy = u_xlat0.ww * u_xlat0.xy;
					    gl_Position.zw = u_xlat0.zw;
					    vs_COLOR0 = in_COLOR0 * _FaceColor;
					    u_xlat0.x = floor(in_TEXCOORD1.x);
					    vs_TEXCOORD1.x = u_xlat0.x * 0.0009765625;
					    u_xlat0.x = fract(in_TEXCOORD1.x);
					    vs_TEXCOORD1.y = u_xlat0.x * 4.0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    u_xlat5.x = _ScreenParams.x * glstate_matrix_projection[0].x;
					    u_xlat5.y = _ScreenParams.y * glstate_matrix_projection[1].y;
					    u_xlat0.xy = u_xlat0.ww / abs(u_xlat5.xy);
					    vs_TEXCOORD2.zw = vec2(0.5, 0.5) / u_xlat0.xy;
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
						vec4 unused_0_0[4];
						vec4 _ClipRect;
						vec4 unused_0_2;
					};
					uniform  sampler2D _FaceTex;
					uniform  sampler2D _MainTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec4 u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
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
					    u_xlat2 = texture(_FaceTex, vs_TEXCOORD1.xy);
					    u_xlat2 = u_xlat2 * vs_COLOR0;
					    u_xlat2.w = u_xlat1.w * u_xlat2.w;
					    SV_Target0 = u_xlat0.xxxx * u_xlat2;
					    return;
					}"
				}
			}
		}
	}
}