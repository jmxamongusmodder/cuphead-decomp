Shader "Cuphead/SpriteBlendDynamic/ScreenIgnoreColor" {
	Properties {
		_MainTex ("Main Tex", 2D) = "white" {}
		_Amount ("Amount", Range(0, 1)) = 0.5
		[HideInInspector] _Cutoff ("Alpha cutoff", Range(0, 1)) = 0.5
		_ReferenceColor1 ("Reference Color 1", Vector) = (1,1,1,1)
		_Threshold ("Recolor Threshold 1", Range(0.9, 1.01)) = 1
		_ReferenceColor2 ("Reference Color 2", Vector) = (1,1,1,1)
		_Threshold2 ("Recolor Threshold 2", Range(0.9, 1.01)) = 1
		_ReferenceColor3 ("Reference Color 3", Vector) = (1,1,1,1)
		_Threshold3 ("Recolor Threshold 3", Range(0.9, 1.01)) = 1
		[Toggle] _IncludeBlacks ("Include Blacks", Float) = 0
		_BlackThreshold ("Black Threshold", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "IGNOREPROJECTOR" = "true" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Name "FORWARD"
			Tags { "IGNOREPROJECTOR" = "true" "LIGHTMODE" = "FORWARDBASE" "QUEUE" = "Transparent" "RenderType" = "Transparent" "SHADOWSUPPORT" = "true" }
			Blend SrcAlpha OneMinusSrcAlpha, SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 12498
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
					    gl_Position = unity_MatrixVP[3] * u_xlat0.wwww + u_xlat1;
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
						vec4 _ReferenceColor1;
						vec4 _ReferenceColor2;
						vec4 _ReferenceColor3;
						float _Threshold;
						float _Threshold2;
						float _Threshold3;
						float _IncludeBlacks;
						float _BlackThreshold;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bvec3 u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec3 u_xlat5;
					float u_xlat6;
					float u_xlat13;
					bool u_xlatb13;
					float u_xlat14;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1.x = u_xlat0.y>=u_xlat0.z;
					    u_xlat1.x = u_xlatb1.x ? 1.0 : float(0.0);
					    u_xlat2.xy = u_xlat0.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = u_xlat0.yz + (-u_xlat2.xy);
					    u_xlat3.z = float(1.0);
					    u_xlat3.w = float(-1.0);
					    u_xlat1 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					    u_xlatb2 = u_xlat0.x>=u_xlat1.x;
					    u_xlat2.x = u_xlatb2 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat1.xyw;
					    u_xlat3.w = u_xlat0.x;
					    u_xlat1.xyw = u_xlat3.wyx;
					    u_xlat1 = (-u_xlat3) + u_xlat1;
					    u_xlat1 = u_xlat2.xxxx * u_xlat1.yzxw + u_xlat3.yzxw;
					    u_xlat2.x = min(u_xlat1.x, u_xlat1.w);
					    u_xlat2.x = u_xlat1.z + (-u_xlat2.x);
					    u_xlat13 = (-u_xlat1.x) + u_xlat1.w;
					    u_xlat6 = u_xlat2.x * 6.0 + 1.00000001e-10;
					    u_xlat13 = u_xlat13 / u_xlat6;
					    u_xlat13 = u_xlat13 + u_xlat1.y;
					    u_xlat1.x = abs(u_xlat13);
					    u_xlat13 = u_xlat1.z + 1.00000001e-10;
					    u_xlat1.y = u_xlat2.x / u_xlat13;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat1.xyz = vec3(u_xlat13) * u_xlat1.xyz;
					    u_xlatb13 = _ReferenceColor1.y>=_ReferenceColor1.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor1.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor1.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor1.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor1.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor2.y>=_ReferenceColor2.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor2.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor2.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor2.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor2.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold2<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor3.y>=_ReferenceColor3.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor3.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor3.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor3.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor3.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat1.x = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb1.x = _Threshold3<u_xlat1.x;
					    if(u_xlatb1.x){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1.x = vec4(0.0, 0.0, 0.0, 0.0)!=vec4(_IncludeBlacks);
					    if(u_xlatb1.x){
					        u_xlatb1.xyz = lessThan(u_xlat0.xyzx, vec4(_BlackThreshold)).xyz;
					        u_xlatb1.x = u_xlatb1.y && u_xlatb1.x;
					        u_xlatb1.x = u_xlatb1.z && u_xlatb1.x;
					        if(u_xlatb1.x){
					            SV_Target0 = u_xlat0;
					            return;
					        }
					    }
					    u_xlat1.x = (-_Amount) + 1.0;
					    u_xlat5.xyz = (-vs_COLOR0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = (-u_xlat5.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = u_xlat5.xyz * vec3(_Amount);
					    SV_Target0.xyz = u_xlat1.xxx * u_xlat0.xyz + u_xlat5.xyz;
					    SV_Target0.w = u_xlat0.w * vs_COLOR0.w;
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
						vec4 _ReferenceColor1;
						vec4 _ReferenceColor2;
						vec4 _ReferenceColor3;
						float _Threshold;
						float _Threshold2;
						float _Threshold3;
						float _IncludeBlacks;
						float _BlackThreshold;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bvec3 u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec3 u_xlat5;
					float u_xlat6;
					float u_xlat13;
					bool u_xlatb13;
					float u_xlat14;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1.x = u_xlat0.y>=u_xlat0.z;
					    u_xlat1.x = u_xlatb1.x ? 1.0 : float(0.0);
					    u_xlat2.xy = u_xlat0.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = u_xlat0.yz + (-u_xlat2.xy);
					    u_xlat3.z = float(1.0);
					    u_xlat3.w = float(-1.0);
					    u_xlat1 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					    u_xlatb2 = u_xlat0.x>=u_xlat1.x;
					    u_xlat2.x = u_xlatb2 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat1.xyw;
					    u_xlat3.w = u_xlat0.x;
					    u_xlat1.xyw = u_xlat3.wyx;
					    u_xlat1 = (-u_xlat3) + u_xlat1;
					    u_xlat1 = u_xlat2.xxxx * u_xlat1.yzxw + u_xlat3.yzxw;
					    u_xlat2.x = min(u_xlat1.x, u_xlat1.w);
					    u_xlat2.x = u_xlat1.z + (-u_xlat2.x);
					    u_xlat13 = (-u_xlat1.x) + u_xlat1.w;
					    u_xlat6 = u_xlat2.x * 6.0 + 1.00000001e-10;
					    u_xlat13 = u_xlat13 / u_xlat6;
					    u_xlat13 = u_xlat13 + u_xlat1.y;
					    u_xlat1.x = abs(u_xlat13);
					    u_xlat13 = u_xlat1.z + 1.00000001e-10;
					    u_xlat1.y = u_xlat2.x / u_xlat13;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat1.xyz = vec3(u_xlat13) * u_xlat1.xyz;
					    u_xlatb13 = _ReferenceColor1.y>=_ReferenceColor1.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor1.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor1.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor1.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor1.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor2.y>=_ReferenceColor2.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor2.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor2.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor2.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor2.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold2<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor3.y>=_ReferenceColor3.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor3.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor3.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor3.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor3.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat1.x = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb1.x = _Threshold3<u_xlat1.x;
					    if(u_xlatb1.x){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1.x = vec4(0.0, 0.0, 0.0, 0.0)!=vec4(_IncludeBlacks);
					    if(u_xlatb1.x){
					        u_xlatb1.xyz = lessThan(u_xlat0.xyzx, vec4(_BlackThreshold)).xyz;
					        u_xlatb1.x = u_xlatb1.y && u_xlatb1.x;
					        u_xlatb1.x = u_xlatb1.z && u_xlatb1.x;
					        if(u_xlatb1.x){
					            SV_Target0 = u_xlat0;
					            return;
					        }
					    }
					    u_xlat1.x = (-_Amount) + 1.0;
					    u_xlat5.xyz = (-vs_COLOR0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = (-u_xlat5.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = u_xlat5.xyz * vec3(_Amount);
					    SV_Target0.xyz = u_xlat1.xxx * u_xlat0.xyz + u_xlat5.xyz;
					    SV_Target0.w = u_xlat0.w * vs_COLOR0.w;
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
						vec4 _ReferenceColor1;
						vec4 _ReferenceColor2;
						vec4 _ReferenceColor3;
						float _Threshold;
						float _Threshold2;
						float _Threshold3;
						float _IncludeBlacks;
						float _BlackThreshold;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bvec3 u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec3 u_xlat5;
					float u_xlat6;
					float u_xlat13;
					bool u_xlatb13;
					float u_xlat14;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1.x = u_xlat0.y>=u_xlat0.z;
					    u_xlat1.x = u_xlatb1.x ? 1.0 : float(0.0);
					    u_xlat2.xy = u_xlat0.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = u_xlat0.yz + (-u_xlat2.xy);
					    u_xlat3.z = float(1.0);
					    u_xlat3.w = float(-1.0);
					    u_xlat1 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					    u_xlatb2 = u_xlat0.x>=u_xlat1.x;
					    u_xlat2.x = u_xlatb2 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat1.xyw;
					    u_xlat3.w = u_xlat0.x;
					    u_xlat1.xyw = u_xlat3.wyx;
					    u_xlat1 = (-u_xlat3) + u_xlat1;
					    u_xlat1 = u_xlat2.xxxx * u_xlat1.yzxw + u_xlat3.yzxw;
					    u_xlat2.x = min(u_xlat1.x, u_xlat1.w);
					    u_xlat2.x = u_xlat1.z + (-u_xlat2.x);
					    u_xlat13 = (-u_xlat1.x) + u_xlat1.w;
					    u_xlat6 = u_xlat2.x * 6.0 + 1.00000001e-10;
					    u_xlat13 = u_xlat13 / u_xlat6;
					    u_xlat13 = u_xlat13 + u_xlat1.y;
					    u_xlat1.x = abs(u_xlat13);
					    u_xlat13 = u_xlat1.z + 1.00000001e-10;
					    u_xlat1.y = u_xlat2.x / u_xlat13;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat1.xyz = vec3(u_xlat13) * u_xlat1.xyz;
					    u_xlatb13 = _ReferenceColor1.y>=_ReferenceColor1.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor1.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor1.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor1.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor1.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor2.y>=_ReferenceColor2.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor2.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor2.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor2.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor2.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold2<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor3.y>=_ReferenceColor3.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor3.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor3.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor3.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor3.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat1.x = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb1.x = _Threshold3<u_xlat1.x;
					    if(u_xlatb1.x){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1.x = vec4(0.0, 0.0, 0.0, 0.0)!=vec4(_IncludeBlacks);
					    if(u_xlatb1.x){
					        u_xlatb1.xyz = lessThan(u_xlat0.xyzx, vec4(_BlackThreshold)).xyz;
					        u_xlatb1.x = u_xlatb1.y && u_xlatb1.x;
					        u_xlatb1.x = u_xlatb1.z && u_xlatb1.x;
					        if(u_xlatb1.x){
					            SV_Target0 = u_xlat0;
					            return;
					        }
					    }
					    u_xlat1.x = (-_Amount) + 1.0;
					    u_xlat5.xyz = (-vs_COLOR0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = (-u_xlat5.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = u_xlat5.xyz * vec3(_Amount);
					    SV_Target0.xyz = u_xlat1.xxx * u_xlat0.xyz + u_xlat5.xyz;
					    SV_Target0.w = u_xlat0.w * vs_COLOR0.w;
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
						vec4 _ReferenceColor1;
						vec4 _ReferenceColor2;
						vec4 _ReferenceColor3;
						float _Threshold;
						float _Threshold2;
						float _Threshold3;
						float _IncludeBlacks;
						float _BlackThreshold;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bvec3 u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec3 u_xlat5;
					float u_xlat6;
					float u_xlat13;
					bool u_xlatb13;
					float u_xlat14;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD0.xy * _MainTex_ST.xy + _MainTex_ST.zw;
					    u_xlat0 = texture(_MainTex, u_xlat0.xy);
					    u_xlatb1.x = u_xlat0.y>=u_xlat0.z;
					    u_xlat1.x = u_xlatb1.x ? 1.0 : float(0.0);
					    u_xlat2.xy = u_xlat0.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = u_xlat0.yz + (-u_xlat2.xy);
					    u_xlat3.z = float(1.0);
					    u_xlat3.w = float(-1.0);
					    u_xlat1 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					    u_xlatb2 = u_xlat0.x>=u_xlat1.x;
					    u_xlat2.x = u_xlatb2 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat1.xyw;
					    u_xlat3.w = u_xlat0.x;
					    u_xlat1.xyw = u_xlat3.wyx;
					    u_xlat1 = (-u_xlat3) + u_xlat1;
					    u_xlat1 = u_xlat2.xxxx * u_xlat1.yzxw + u_xlat3.yzxw;
					    u_xlat2.x = min(u_xlat1.x, u_xlat1.w);
					    u_xlat2.x = u_xlat1.z + (-u_xlat2.x);
					    u_xlat13 = (-u_xlat1.x) + u_xlat1.w;
					    u_xlat6 = u_xlat2.x * 6.0 + 1.00000001e-10;
					    u_xlat13 = u_xlat13 / u_xlat6;
					    u_xlat13 = u_xlat13 + u_xlat1.y;
					    u_xlat1.x = abs(u_xlat13);
					    u_xlat13 = u_xlat1.z + 1.00000001e-10;
					    u_xlat1.y = u_xlat2.x / u_xlat13;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat1.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat1.xyz = vec3(u_xlat13) * u_xlat1.xyz;
					    u_xlatb13 = _ReferenceColor1.y>=_ReferenceColor1.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor1.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor1.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor1.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor1.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor2.y>=_ReferenceColor2.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor2.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor2.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor2.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor2.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat13 = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb13 = _Threshold2<u_xlat13;
					    if(u_xlatb13){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb13 = _ReferenceColor3.y>=_ReferenceColor3.z;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat2.xy = _ReferenceColor3.zy;
					    u_xlat2.z = float(-1.0);
					    u_xlat2.w = float(0.666666687);
					    u_xlat3.xy = _ReferenceColor3.yz;
					    u_xlat3.z = float(0.0);
					    u_xlat3.w = float(-0.333333343);
					    u_xlat3 = (-u_xlat2) + u_xlat3;
					    u_xlat2 = vec4(u_xlat13) * u_xlat3 + u_xlat2;
					    u_xlatb13 = _ReferenceColor3.x>=u_xlat2.x;
					    u_xlat13 = u_xlatb13 ? 1.0 : float(0.0);
					    u_xlat3.xyz = u_xlat2.xyw;
					    u_xlat3.w = _ReferenceColor3.x;
					    u_xlat2.xyw = u_xlat3.wyx;
					    u_xlat2 = (-u_xlat3) + u_xlat2;
					    u_xlat2 = vec4(u_xlat13) * u_xlat2.yzxw + u_xlat3.yzxw;
					    u_xlat13 = min(u_xlat2.x, u_xlat2.w);
					    u_xlat13 = (-u_xlat13) + u_xlat2.z;
					    u_xlat14 = (-u_xlat2.x) + u_xlat2.w;
					    u_xlat3.x = u_xlat13 * 6.0 + 1.00000001e-10;
					    u_xlat14 = u_xlat14 / u_xlat3.x;
					    u_xlat14 = u_xlat14 + u_xlat2.y;
					    u_xlat2.x = abs(u_xlat14);
					    u_xlat14 = u_xlat2.z + 1.00000001e-10;
					    u_xlat2.y = u_xlat13 / u_xlat14;
					    u_xlat13 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat13 = inversesqrt(u_xlat13);
					    u_xlat2.xyz = vec3(u_xlat13) * u_xlat2.xyz;
					    u_xlat1.x = dot(u_xlat1.xyz, u_xlat2.xyz);
					    u_xlatb1.x = _Threshold3<u_xlat1.x;
					    if(u_xlatb1.x){
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    u_xlatb1.x = vec4(0.0, 0.0, 0.0, 0.0)!=vec4(_IncludeBlacks);
					    if(u_xlatb1.x){
					        u_xlatb1.xyz = lessThan(u_xlat0.xyzx, vec4(_BlackThreshold)).xyz;
					        u_xlatb1.x = u_xlatb1.y && u_xlatb1.x;
					        u_xlatb1.x = u_xlatb1.z && u_xlatb1.x;
					        if(u_xlatb1.x){
					            SV_Target0 = u_xlat0;
					            return;
					        }
					    }
					    u_xlat1.x = (-_Amount) + 1.0;
					    u_xlat5.xyz = (-vs_COLOR0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat2.xyz = (-u_xlat0.xyz) + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = (-u_xlat5.xyz) * u_xlat2.xyz + vec3(1.0, 1.0, 1.0);
					    u_xlat5.xyz = u_xlat5.xyz * vec3(_Amount);
					    SV_Target0.xyz = u_xlat1.xxx * u_xlat0.xyz + u_xlat5.xyz;
					    SV_Target0.w = u_xlat0.w * vs_COLOR0.w;
					    return;
					}"
				}
			}
		}
	}
	Fallback "Diffuse"
	CustomEditor "ShaderForgeMaterialInspector"
}