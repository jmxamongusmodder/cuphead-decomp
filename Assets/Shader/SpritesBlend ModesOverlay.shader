Shader "Sprites/Blend Modes/Overlay" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
		}
		Pass {
			Name "FORWARDBASE"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 46391
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTPROBE_SH" }
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "VERTEXLIGHT_ON" }
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "VERTEXLIGHT_ON" }
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTPROBE_SH" "VERTEXLIGHT_ON" }
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
					layout(std140) uniform UnityPerDraw {
						mat4x4 unity_ObjectToWorld;
						vec4 unused_0_1[6];
					};
					layout(std140) uniform UnityPerFrame {
						vec4 unused_1_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_1_2[2];
					};
					in  vec4 in_POSITION0;
					in  vec2 in_TEXCOORD0;
					in  vec4 in_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_COLOR0;
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
					    gl_Position = u_xlat0;
					    vs_TEXCOORD1 = u_xlat0;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    vs_COLOR0 = in_COLOR0;
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
						vec4 _MainTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _GrabTexture;
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat12;
					bool u_xlatb12;
					void main()
					{
					    u_xlat0.x = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat1.z = u_xlat0.x * u_xlat1.y;
					    u_xlat0.xy = u_xlat1.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat0 = texture(_GrabTexture, u_xlat0.xy);
					    u_xlatb12 = u_xlat0.x>=0.5;
					    u_xlat12 = u_xlatb12 ? 1.0 : float(0.0);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat2.xyz = u_xlat0.xyz * u_xlat1.xyz;
					    u_xlat2.xyz = vec3(u_xlat12) * u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat2.xyz + u_xlat2.xyz;
					    u_xlat3.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlatb0 = 0.5>=u_xlat0.x;
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) + vec3(1.0, 1.0, 1.0);
					    SV_Target0.w = u_xlat1.w * vs_COLOR0.w;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) * vec3(2.0, 2.0, 2.0) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz + (-u_xlat2.xyz);
					    u_xlat0.x = u_xlat12 * u_xlat0.x;
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
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
						vec4 _MainTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _GrabTexture;
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat12;
					bool u_xlatb12;
					void main()
					{
					    u_xlat0.x = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat1.z = u_xlat0.x * u_xlat1.y;
					    u_xlat0.xy = u_xlat1.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat0 = texture(_GrabTexture, u_xlat0.xy);
					    u_xlatb12 = u_xlat0.x>=0.5;
					    u_xlat12 = u_xlatb12 ? 1.0 : float(0.0);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat2.xyz = u_xlat0.xyz * u_xlat1.xyz;
					    u_xlat2.xyz = vec3(u_xlat12) * u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat2.xyz + u_xlat2.xyz;
					    u_xlat3.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlatb0 = 0.5>=u_xlat0.x;
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) + vec3(1.0, 1.0, 1.0);
					    SV_Target0.w = u_xlat1.w * vs_COLOR0.w;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) * vec3(2.0, 2.0, 2.0) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz + (-u_xlat2.xyz);
					    u_xlat0.x = u_xlat12 * u_xlat0.x;
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" }
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
						vec4 _MainTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _GrabTexture;
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat12;
					bool u_xlatb12;
					void main()
					{
					    u_xlat0.x = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat1.z = u_xlat0.x * u_xlat1.y;
					    u_xlat0.xy = u_xlat1.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat0 = texture(_GrabTexture, u_xlat0.xy);
					    u_xlatb12 = u_xlat0.x>=0.5;
					    u_xlat12 = u_xlatb12 ? 1.0 : float(0.0);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat2.xyz = u_xlat0.xyz * u_xlat1.xyz;
					    u_xlat2.xyz = vec3(u_xlat12) * u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat2.xyz + u_xlat2.xyz;
					    u_xlat3.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlatb0 = 0.5>=u_xlat0.x;
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) + vec3(1.0, 1.0, 1.0);
					    SV_Target0.w = u_xlat1.w * vs_COLOR0.w;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) * vec3(2.0, 2.0, 2.0) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz + (-u_xlat2.xyz);
					    u_xlat0.x = u_xlat12 * u_xlat0.x;
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "DIRECTIONAL" "SHADOWS_SCREEN" "LIGHTPROBE_SH" }
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
						vec4 _MainTex_ST;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _GrabTexture;
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					float u_xlat12;
					bool u_xlatb12;
					void main()
					{
					    u_xlat0.x = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat1.z = u_xlat0.x * u_xlat1.y;
					    u_xlat0.xy = u_xlat1.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat0 = texture(_GrabTexture, u_xlat0.xy);
					    u_xlatb12 = u_xlat0.x>=0.5;
					    u_xlat12 = u_xlatb12 ? 1.0 : float(0.0);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat2.xyz = u_xlat0.xyz * u_xlat1.xyz;
					    u_xlat2.xyz = vec3(u_xlat12) * u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat2.xyz + u_xlat2.xyz;
					    u_xlat3.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlatb0 = 0.5>=u_xlat0.x;
					    u_xlat0.x = u_xlatb0 ? 1.0 : float(0.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) + vec3(1.0, 1.0, 1.0);
					    SV_Target0.w = u_xlat1.w * vs_COLOR0.w;
					    u_xlat1.xyz = u_xlat1.xyz * u_xlat3.xyz;
					    u_xlat1.xyz = (-u_xlat1.xyz) * vec3(2.0, 2.0, 2.0) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz + (-u_xlat2.xyz);
					    u_xlat0.x = u_xlat12 * u_xlat0.x;
					    u_xlat0.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat2.xyz;
					    SV_Target0.xyz = u_xlat0.xyz * vs_COLOR0.xyz;
					    return;
					}"
				}
			}
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}