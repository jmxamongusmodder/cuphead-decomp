Shader "Cuphead/Sprites/Honey" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_Amount ("Amount", Range(0, 10)) = 5
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
			"_HoneyTexture"
		}
		Pass {
			Name "FORWARDBASE"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 36607
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _MainTex_ST;
						float _Amount;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _HoneyTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec4 u_xlat3;
					float u_xlat4;
					float u_xlat12;
					int u_xlati12;
					bool u_xlatb14;
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					    u_xlat0.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat12 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat0.z = u_xlat12 * u_xlat0.y;
					    u_xlat0.xy = u_xlat0.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat12 = u_xlat1.w * _Amount;
					    u_xlat4 = (-u_xlat12) * 3.0 + u_xlat0.y;
					    u_xlat2.x = float(0.0);
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat0.z = u_xlat4;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_HoneyTexture, u_xlat0.xz);
					        u_xlat2.xyz = u_xlat3.xyz * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat2.xyz;
					        u_xlat0.z = _Amount * u_xlat1.w + u_xlat0.z;
					    }
					    u_xlat0.xyz = u_xlat1.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xyz;
					    u_xlat0.xyz = clamp(u_xlat0.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat0.xyz * u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
					    u_xlat0.xyz = (-u_xlat2.xyz) * u_xlat0.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat0.xyz = u_xlat1.xyz * u_xlat0.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = (-u_xlat2.xyz) + u_xlat0.xyz;
					    SV_Target0.xyz = u_xlat1.www * u_xlat0.xyz + u_xlat2.xyz;
					    SV_Target0.w = u_xlat1.w;
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _MainTex_ST;
						float _Amount;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _HoneyTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec4 u_xlat3;
					float u_xlat4;
					float u_xlat12;
					int u_xlati12;
					bool u_xlatb14;
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					    u_xlat0.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat12 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat0.z = u_xlat12 * u_xlat0.y;
					    u_xlat0.xy = u_xlat0.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat12 = u_xlat1.w * _Amount;
					    u_xlat4 = (-u_xlat12) * 3.0 + u_xlat0.y;
					    u_xlat2.x = float(0.0);
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat0.z = u_xlat4;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_HoneyTexture, u_xlat0.xz);
					        u_xlat2.xyz = u_xlat3.xyz * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat2.xyz;
					        u_xlat0.z = _Amount * u_xlat1.w + u_xlat0.z;
					    }
					    u_xlat0.xyz = u_xlat1.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xyz;
					    u_xlat0.xyz = clamp(u_xlat0.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat0.xyz * u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
					    u_xlat0.xyz = (-u_xlat2.xyz) * u_xlat0.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat0.xyz = u_xlat1.xyz * u_xlat0.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = (-u_xlat2.xyz) + u_xlat0.xyz;
					    SV_Target0.xyz = u_xlat1.www * u_xlat0.xyz + u_xlat2.xyz;
					    SV_Target0.w = u_xlat1.w;
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _MainTex_ST;
						float _Amount;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _HoneyTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec4 u_xlat3;
					float u_xlat4;
					float u_xlat12;
					int u_xlati12;
					bool u_xlatb14;
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					    u_xlat0.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat12 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat0.z = u_xlat12 * u_xlat0.y;
					    u_xlat0.xy = u_xlat0.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat12 = u_xlat1.w * _Amount;
					    u_xlat4 = (-u_xlat12) * 3.0 + u_xlat0.y;
					    u_xlat2.x = float(0.0);
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat0.z = u_xlat4;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_HoneyTexture, u_xlat0.xz);
					        u_xlat2.xyz = u_xlat3.xyz * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat2.xyz;
					        u_xlat0.z = _Amount * u_xlat1.w + u_xlat0.z;
					    }
					    u_xlat0.xyz = u_xlat1.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xyz;
					    u_xlat0.xyz = clamp(u_xlat0.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat0.xyz * u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
					    u_xlat0.xyz = (-u_xlat2.xyz) * u_xlat0.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat0.xyz = u_xlat1.xyz * u_xlat0.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = (-u_xlat2.xyz) + u_xlat0.xyz;
					    SV_Target0.xyz = u_xlat1.www * u_xlat0.xyz + u_xlat2.xyz;
					    SV_Target0.w = u_xlat1.w;
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[2];
						vec4 _MainTex_ST;
						float _Amount;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _HoneyTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec3 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec4 u_xlat3;
					float u_xlat4;
					float u_xlat12;
					int u_xlati12;
					bool u_xlatb14;
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					    u_xlat0.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat12 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat0.z = u_xlat12 * u_xlat0.y;
					    u_xlat0.xy = u_xlat0.xz * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat1.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat1 = texture(_MainTex, u_xlat1.xy);
					    u_xlat12 = u_xlat1.w * _Amount;
					    u_xlat4 = (-u_xlat12) * 3.0 + u_xlat0.y;
					    u_xlat2.x = float(0.0);
					    u_xlat2.y = float(0.0);
					    u_xlat2.z = float(0.0);
					    u_xlat0.z = u_xlat4;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_HoneyTexture, u_xlat0.xz);
					        u_xlat2.xyz = u_xlat3.xyz * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat2.xyz;
					        u_xlat0.z = _Amount * u_xlat1.w + u_xlat0.z;
					    }
					    u_xlat0.xyz = u_xlat1.xyz + u_xlat1.xyz;
					    u_xlat0.xyz = u_xlat0.xyz;
					    u_xlat0.xyz = clamp(u_xlat0.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat0.xyz * u_xlat2.xyz;
					    u_xlat1.xyz = u_xlat1.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat1.xyz = clamp(u_xlat1.xyz, 0.0, 1.0);
					    u_xlat0.xyz = (-u_xlat2.xyz) * u_xlat0.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat0.xyz = u_xlat1.xyz * u_xlat0.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = (-u_xlat2.xyz) + u_xlat0.xyz;
					    SV_Target0.xyz = u_xlat1.www * u_xlat0.xyz + u_xlat2.xyz;
					    SV_Target0.w = u_xlat1.w;
					    return;
					}"
				}
			}
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}