Shader "Cuphead/Sprites/MonochromeNoBlack" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_MonochromeBase ("Base Color", Vector) = (1,0,0,0)
		_Factor ("Factor", Range(0, 1)) = 1
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 35353
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
						vec4 _Color;
						vec4 unused_0_2[3];
					};
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_1_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_2_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_2_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD0;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
					    vs_COLOR0 = in_COLOR0 * _Color;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
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
						vec4 unused_0_0[3];
						vec4 _MonochromeBase;
						float _Factor;
						vec4 _MainTex_TexelSize;
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					float u_xlat3;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = (-_MainTex_TexelSize.xyxy) * vec4(1.0, -1.0, -1.0, 1.0) + vs_TEXCOORD0.xyxy;
					    u_xlat1 = texture(_MainTex, u_xlat0.xy);
					    u_xlat0 = texture(_MainTex, u_xlat0.zw);
					    u_xlat0 = max(u_xlat0, u_xlat1);
					    u_xlat1.xy = vs_TEXCOORD0.xy + (-_MainTex_TexelSize.xy);
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat0 = max(u_xlat0, u_xlat1);
					    u_xlat1.xy = vs_TEXCOORD0.xy + _MainTex_TexelSize.xy;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat0 = max(u_xlat0, u_xlat1);
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0 = max(u_xlat0, u_xlat1);
					    u_xlat3 = max(u_xlat0.z, u_xlat0.y);
					    u_xlat0.x = max(u_xlat3, u_xlat0.x);
					    u_xlat3 = u_xlat0.w + -0.5;
					    u_xlat3 = -abs(u_xlat3) * 2.0 + 1.0;
					    u_xlat0.x = max(u_xlat0.x, 0.5);
					    u_xlat6 = max(u_xlat1.z, u_xlat1.y);
					    u_xlat6 = max(u_xlat6, u_xlat1.x);
					    u_xlat0.x = u_xlat6 / u_xlat0.x;
					    u_xlat2 = (-_MonochromeBase) + vec4(1.0, 1.0, 1.0, 1.0);
					    u_xlat6 = min(u_xlat1.z, u_xlat1.y);
					    u_xlat6 = min(u_xlat6, u_xlat1.x);
					    u_xlat2 = vec4(u_xlat6) * u_xlat2 + _MonochromeBase;
					    u_xlat2 = (-u_xlat1) + u_xlat2;
					    u_xlat2 = vec4(_Factor) * u_xlat2 + u_xlat1;
					    u_xlat0.x = u_xlat0.x * u_xlat1.w + (-u_xlat2.w);
					    u_xlat2.w = _Factor * u_xlat0.x + u_xlat2.w;
					    u_xlat1 = u_xlat1 + (-u_xlat2);
					    u_xlat0 = vec4(u_xlat3) * u_xlat1 + u_xlat2;
					    SV_Target0 = u_xlat0 * vs_COLOR0;
					    return;
					}"
				}
			}
		}
	}
}