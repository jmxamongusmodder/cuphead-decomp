Shader "Cuphead/Sprites/LightRay" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_ShadowTex ("Shadow Texture", 2D) = "defaulttexture" {}
		_SkyGlowTex ("Sky Glow Texture", 2D) = "defaulttexture" {}
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 31648
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
						vec4 unused_0_2;
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
						float t;
						float textureWidth;
						float textureSpeed;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _ShadowTex;
					uniform  sampler2D _SkyGlowTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					float u_xlat3;
					bool u_xlatb3;
					float u_xlat6;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlatb3 = 0.0>=u_xlat0.x;
					    if(((int(u_xlatb3) * int(0xffffffffu)))!=0){discard;}
					    u_xlat3 = vs_TEXCOORD0.x / textureWidth;
					    u_xlat6 = t * textureSpeed;
					    u_xlat1.x = (-textureSpeed) * t + u_xlat3;
					    u_xlat1.yw = vs_TEXCOORD0.yy;
					    u_xlat2 = texture(_ShadowTex, u_xlat1.xy);
					    u_xlat2.xyz = u_xlat2.www * u_xlat2.xyz;
					    u_xlat1.z = (-u_xlat6) * -0.25 + u_xlat3;
					    u_xlat1 = texture(_SkyGlowTex, u_xlat1.zw);
					    u_xlat1.xyz = u_xlat1.www * u_xlat1.xyz;
					    u_xlat3 = (-u_xlat1.w) + 1.0;
					    u_xlat1 = u_xlat2 * vec4(u_xlat3) + u_xlat1;
					    u_xlat0 = u_xlat0.xxxx * u_xlat1;
					    u_xlat0 = u_xlat0 * vs_COLOR0;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.www;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
	}
}