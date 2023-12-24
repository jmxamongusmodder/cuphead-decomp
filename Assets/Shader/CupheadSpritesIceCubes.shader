Shader "Cuphead/Sprites/IceCubes" {
	Properties {
		_MainTex ("MainTex", 2D) = "white" {}
		_Amount ("Amount", Range(0, 10)) = 5
		_CutoffMax ("Alpha cutoff max", Range(0, 1)) = 0.867
		_CutoffMin ("Alpha cutoff min", Range(0, 1)) = 0.2
		_CutoffGreen ("Green cutoff", Range(0, 1)) = 0.1
		_CutoffBlue ("Blue cutoff", Range(0, 1)) = 0.1
		_BlendValue ("Blend value", Range(0, 1)) = 0.5
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		GrabPass {
			"_IceTexture"
		}
		Pass {
			Name "FORWARDBASE"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			Fog {
				Mode Off
			}
			GpuProgramID 42929
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
						float _Amount;
						float _CutoffMax;
						float _CutoffMin;
						float _CutoffGreen;
						float _CutoffBlue;
						float _BlendValue;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _IceTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					bool u_xlatb6;
					bool u_xlatb11;
					float u_xlat16;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1 = _CutoffMax<u_xlat0.w;
					    u_xlatb6 = u_xlat0.y<_CutoffGreen;
					    u_xlatb11 = u_xlat0.z<_CutoffBlue;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb11 = 0.0<u_xlat0.w;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb1 = u_xlatb6 || u_xlatb1;
					    if(u_xlatb1){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1 = u_xlat0.w<_CutoffMin;
					    if(u_xlatb1){
					        SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
					        return;
					    }
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat16 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.z = u_xlat16 * u_xlat1.y;
					    u_xlat1.xy = u_xlat1.zx * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat2.x = u_xlat0.w * _Amount;
					    u_xlat1.z = (-u_xlat2.x) * 3.0 + u_xlat1.x;
					    u_xlat2 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019);
					    u_xlat2.xyz = u_xlat2.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat3.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.324000001, 0.324000001, 0.324000001) + u_xlat2.xyz;
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat1 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat0.xyz + u_xlat0.xyz;
					    u_xlat2.xyz = u_xlat2.xyz;
					    u_xlat2.xyz = clamp(u_xlat2.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat1.xyz * u_xlat2.xyz;
					    u_xlat4.xyz = u_xlat0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat4.xyz = clamp(u_xlat4.xyz, 0.0, 1.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat1.xyz = u_xlat4.xyz * u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    SV_Target0.xyz = vec3(vec3(_BlendValue, _BlendValue, _BlendValue)) * u_xlat0.xyz + u_xlat1.xyz;
					    SV_Target0.w = 1.0;
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
						float _Amount;
						float _CutoffMax;
						float _CutoffMin;
						float _CutoffGreen;
						float _CutoffBlue;
						float _BlendValue;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _IceTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					bool u_xlatb6;
					bool u_xlatb11;
					float u_xlat16;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1 = _CutoffMax<u_xlat0.w;
					    u_xlatb6 = u_xlat0.y<_CutoffGreen;
					    u_xlatb11 = u_xlat0.z<_CutoffBlue;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb11 = 0.0<u_xlat0.w;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb1 = u_xlatb6 || u_xlatb1;
					    if(u_xlatb1){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1 = u_xlat0.w<_CutoffMin;
					    if(u_xlatb1){
					        SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
					        return;
					    }
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat16 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.z = u_xlat16 * u_xlat1.y;
					    u_xlat1.xy = u_xlat1.zx * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat2.x = u_xlat0.w * _Amount;
					    u_xlat1.z = (-u_xlat2.x) * 3.0 + u_xlat1.x;
					    u_xlat2 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019);
					    u_xlat2.xyz = u_xlat2.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat3.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.324000001, 0.324000001, 0.324000001) + u_xlat2.xyz;
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat1 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat0.xyz + u_xlat0.xyz;
					    u_xlat2.xyz = u_xlat2.xyz;
					    u_xlat2.xyz = clamp(u_xlat2.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat1.xyz * u_xlat2.xyz;
					    u_xlat4.xyz = u_xlat0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat4.xyz = clamp(u_xlat4.xyz, 0.0, 1.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat1.xyz = u_xlat4.xyz * u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    SV_Target0.xyz = vec3(vec3(_BlendValue, _BlendValue, _BlendValue)) * u_xlat0.xyz + u_xlat1.xyz;
					    SV_Target0.w = 1.0;
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
						float _Amount;
						float _CutoffMax;
						float _CutoffMin;
						float _CutoffGreen;
						float _CutoffBlue;
						float _BlendValue;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _IceTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					bool u_xlatb6;
					bool u_xlatb11;
					float u_xlat16;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1 = _CutoffMax<u_xlat0.w;
					    u_xlatb6 = u_xlat0.y<_CutoffGreen;
					    u_xlatb11 = u_xlat0.z<_CutoffBlue;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb11 = 0.0<u_xlat0.w;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb1 = u_xlatb6 || u_xlatb1;
					    if(u_xlatb1){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1 = u_xlat0.w<_CutoffMin;
					    if(u_xlatb1){
					        SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
					        return;
					    }
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat16 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.z = u_xlat16 * u_xlat1.y;
					    u_xlat1.xy = u_xlat1.zx * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat2.x = u_xlat0.w * _Amount;
					    u_xlat1.z = (-u_xlat2.x) * 3.0 + u_xlat1.x;
					    u_xlat2 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019);
					    u_xlat2.xyz = u_xlat2.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat3.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.324000001, 0.324000001, 0.324000001) + u_xlat2.xyz;
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat1 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat0.xyz + u_xlat0.xyz;
					    u_xlat2.xyz = u_xlat2.xyz;
					    u_xlat2.xyz = clamp(u_xlat2.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat1.xyz * u_xlat2.xyz;
					    u_xlat4.xyz = u_xlat0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat4.xyz = clamp(u_xlat4.xyz, 0.0, 1.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat1.xyz = u_xlat4.xyz * u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    SV_Target0.xyz = vec3(vec3(_BlendValue, _BlendValue, _BlendValue)) * u_xlat0.xyz + u_xlat1.xyz;
					    SV_Target0.w = 1.0;
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
						float _Amount;
						float _CutoffMax;
						float _CutoffMin;
						float _CutoffGreen;
						float _CutoffBlue;
						float _BlendValue;
					};
					layout(std140) uniform UnityPerCamera {
						vec4 unused_1_0[5];
						vec4 _ProjectionParams;
						vec4 unused_1_2[3];
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _IceTexture;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					vec4 u_xlat3;
					vec3 u_xlat4;
					bool u_xlatb6;
					bool u_xlatb11;
					float u_xlat16;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1 = _CutoffMax<u_xlat0.w;
					    u_xlatb6 = u_xlat0.y<_CutoffGreen;
					    u_xlatb11 = u_xlat0.z<_CutoffBlue;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb11 = 0.0<u_xlat0.w;
					    u_xlatb6 = u_xlatb11 && u_xlatb6;
					    u_xlatb1 = u_xlatb6 || u_xlatb1;
					    if(u_xlatb1){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1 = u_xlat0.w<_CutoffMin;
					    if(u_xlatb1){
					        SV_Target0 = vec4(0.0, 0.0, 0.0, 0.0);
					        return;
					    }
					    u_xlat1.xy = vs_TEXCOORD1.xy / vs_TEXCOORD1.ww;
					    u_xlat16 = _ProjectionParams.x * (-_ProjectionParams.x);
					    u_xlat1.z = u_xlat16 * u_xlat1.y;
					    u_xlat1.xy = u_xlat1.zx * vec2(0.5, 0.5) + vec2(0.5, 0.5);
					    u_xlat2.x = u_xlat0.w * _Amount;
					    u_xlat1.z = (-u_xlat2.x) * 3.0 + u_xlat1.x;
					    u_xlat2 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat3.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019);
					    u_xlat2.xyz = u_xlat2.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat3.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.324000001, 0.324000001, 0.324000001) + u_xlat2.xyz;
					    u_xlat1.w = _Amount * u_xlat0.w + u_xlat1.z;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yw);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.231999993, 0.231999993, 0.231999993) + u_xlat2.xyz;
					    u_xlat1.x = _Amount * u_xlat0.w + u_xlat1.w;
					    u_xlat3 = texture(_IceTexture, u_xlat1.yx);
					    u_xlat2.xyz = u_xlat3.xyz * vec3(0.0855000019, 0.0855000019, 0.0855000019) + u_xlat2.xyz;
					    u_xlat1.z = _Amount * u_xlat0.w + u_xlat1.x;
					    u_xlat1 = texture(_IceTexture, u_xlat1.yz);
					    u_xlat1.xyz = u_xlat1.xyz * vec3(0.0205000006, 0.0205000006, 0.0205000006) + u_xlat2.xyz;
					    u_xlat2.xyz = u_xlat0.xyz + u_xlat0.xyz;
					    u_xlat2.xyz = u_xlat2.xyz;
					    u_xlat2.xyz = clamp(u_xlat2.xyz, 0.0, 1.0);
					    u_xlat3.xyz = u_xlat1.xyz * u_xlat2.xyz;
					    u_xlat4.xyz = u_xlat0.xyz * vec3(2.0, 2.0, 2.0) + vec3(-1.0, -1.0, -1.0);
					    u_xlat4.xyz = clamp(u_xlat4.xyz, 0.0, 1.0);
					    u_xlat1.xyz = (-u_xlat1.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat1.xyz = u_xlat4.xyz * u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat0.xyz = u_xlat0.xyz + (-u_xlat1.xyz);
					    SV_Target0.xyz = vec3(vec3(_BlendValue, _BlendValue, _BlendValue)) * u_xlat0.xyz + u_xlat1.xyz;
					    SV_Target0.w = 1.0;
					    return;
					}"
				}
			}
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}