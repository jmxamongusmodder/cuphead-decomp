Shader "Sprites/KitchenBasementPlayer" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_ColorA ("ColorA", Vector) = (1,1,1,1)
		_ColorB ("ColorB", Vector) = (1,1,1,1)
		_AmountA ("AmountA", Float) = 0
		_AmountB ("AmountB", Float) = 0
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Vector) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		_Level ("Level", Float) = 0.3
		_MaskTex ("Mask Texture", 2D) = "white" {}
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 40909
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
						vec4 unused_0_2[4];
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
					layout(std140) uniform UnityPerDrawSprite {
						vec4 _RendererColor;
						vec4 unused_3_1;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    vs_TEXCOORD2.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat0 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    gl_Position = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat0;
					    u_xlat0 = in_COLOR0 * _Color;
					    vs_COLOR0 = u_xlat0 * _RendererColor;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "ETC1_EXTERNAL_ALPHA" }
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
						vec4 unused_0_2[4];
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
					layout(std140) uniform UnityPerDrawSprite {
						vec4 _RendererColor;
						vec4 unused_3_1;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    vs_TEXCOORD2.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat0 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    gl_Position = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat0;
					    u_xlat0 = in_COLOR0 * _Color;
					    vs_COLOR0 = u_xlat0 * _RendererColor;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "PIXELSNAP_ON" }
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
						vec4 unused_0_2[4];
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
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					layout(std140) uniform UnityPerDrawSprite {
						vec4 _RendererColor;
						vec4 unused_4_1;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    vs_TEXCOORD2.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat0 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    u_xlat0 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat0;
					    u_xlat0.xy = u_xlat0.xy / u_xlat0.ww;
					    u_xlat1.xy = _ScreenParams.xy * vec2(0.5, 0.5);
					    u_xlat0.xy = u_xlat0.xy * u_xlat1.xy;
					    u_xlat0.xy = roundEven(u_xlat0.xy);
					    u_xlat0.xy = u_xlat0.xy / u_xlat1.xy;
					    gl_Position.xy = u_xlat0.ww * u_xlat0.xy;
					    gl_Position.zw = u_xlat0.zw;
					    u_xlat0 = in_COLOR0 * _Color;
					    vs_COLOR0 = u_xlat0 * _RendererColor;
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "ETC1_EXTERNAL_ALPHA" "PIXELSNAP_ON" }
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
						vec4 unused_0_2[4];
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
						vec4 unused_3_0[17];
						mat4x4 unity_MatrixVP;
						vec4 unused_3_2[2];
					};
					layout(std140) uniform UnityPerDrawSprite {
						vec4 _RendererColor;
						vec4 unused_4_1;
					};
					in  vec4 in_POSITION0;
					in  vec4 in_COLOR0;
					in  vec2 in_TEXCOORD0;
					out vec4 vs_COLOR0;
					out vec2 vs_TEXCOORD0;
					out vec3 vs_TEXCOORD2;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = in_POSITION0.yyyy * unity_ObjectToWorld[1];
					    u_xlat0 = unity_ObjectToWorld[0] * in_POSITION0.xxxx + u_xlat0;
					    u_xlat0 = unity_ObjectToWorld[2] * in_POSITION0.zzzz + u_xlat0;
					    u_xlat1 = u_xlat0 + unity_ObjectToWorld[3];
					    vs_TEXCOORD2.xyz = unity_ObjectToWorld[3].xyz * in_POSITION0.www + u_xlat0.xyz;
					    u_xlat0 = u_xlat1.yyyy * unity_MatrixVP[1];
					    u_xlat0 = unity_MatrixVP[0] * u_xlat1.xxxx + u_xlat0;
					    u_xlat0 = unity_MatrixVP[2] * u_xlat1.zzzz + u_xlat0;
					    u_xlat0 = unity_MatrixVP[3] * u_xlat1.wwww + u_xlat0;
					    u_xlat0.xy = u_xlat0.xy / u_xlat0.ww;
					    u_xlat1.xy = _ScreenParams.xy * vec2(0.5, 0.5);
					    u_xlat0.xy = u_xlat0.xy * u_xlat1.xy;
					    u_xlat0.xy = roundEven(u_xlat0.xy);
					    u_xlat0.xy = u_xlat0.xy / u_xlat1.xy;
					    gl_Position.xy = u_xlat0.ww * u_xlat0.xy;
					    gl_Position.zw = u_xlat0.zw;
					    u_xlat0 = in_COLOR0 * _Color;
					    vs_COLOR0 = u_xlat0 * _RendererColor;
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
						vec4 unused_0_0[4];
						vec4 _ColorA;
						vec4 _ColorB;
						float _AmountA;
						float _AmountB;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _MaskTex;
					in  vec2 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD2.xy + vec2(-2370.0, 575.0);
					    u_xlat0.xy = u_xlat0.xy * vec2(0.000844999973, 0.000844999973);
					    u_xlat0 = texture(_MaskTex, u_xlat0.xy);
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat3.xyz = u_xlat1.www * u_xlat1.xyz;
					    SV_Target0.w = u_xlat1.w;
					    u_xlat1.xyz = u_xlat3.xyz * _ColorB.xyz + (-u_xlat3.xyz);
					    u_xlat1.xyz = vec3(vec3(_AmountB, _AmountB, _AmountB)) * u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat3.xyz * _ColorA.xyz + (-u_xlat3.xyz);
					    u_xlat3.xyz = vec3(_AmountA) * u_xlat2.xyz + u_xlat3.xyz;
					    u_xlat1.xyz = (-u_xlat3.xyz) + u_xlat1.xyz;
					    SV_Target0.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat3.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "ETC1_EXTERNAL_ALPHA" }
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
						vec4 _ColorA;
						vec4 _ColorB;
						float _AmountA;
						float _AmountB;
					};
					layout(std140) uniform UnityPerDrawSprite {
						vec4 unused_1_0;
						float _EnableExternalAlpha;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _AlphaTex;
					uniform  sampler2D _MaskTex;
					in  vec2 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD2.xy + vec2(-2370.0, 575.0);
					    u_xlat0.xy = u_xlat0.xy * vec2(0.000844999973, 0.000844999973);
					    u_xlat0 = texture(_MaskTex, u_xlat0.xy);
					    u_xlat1 = texture(_AlphaTex, vs_TEXCOORD0.xy);
					    u_xlat2 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat3.x = u_xlat1.x + (-u_xlat2.w);
					    u_xlat3.x = _EnableExternalAlpha * u_xlat3.x + u_xlat2.w;
					    u_xlat1.xyz = u_xlat3.xxx * u_xlat2.xyz;
					    SV_Target0.w = u_xlat3.x;
					    u_xlat3.xyz = u_xlat1.xyz * _ColorB.xyz + (-u_xlat1.xyz);
					    u_xlat3.xyz = vec3(vec3(_AmountB, _AmountB, _AmountB)) * u_xlat3.xyz + u_xlat1.xyz;
					    u_xlat2.xyz = u_xlat1.xyz * _ColorA.xyz + (-u_xlat1.xyz);
					    u_xlat1.xyz = vec3(_AmountA) * u_xlat2.xyz + u_xlat1.xyz;
					    u_xlat3.xyz = u_xlat3.xyz + (-u_xlat1.xyz);
					    SV_Target0.xyz = u_xlat0.xxx * u_xlat3.xyz + u_xlat1.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "PIXELSNAP_ON" }
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
						vec4 _ColorA;
						vec4 _ColorB;
						float _AmountA;
						float _AmountB;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _MaskTex;
					in  vec2 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec3 u_xlat2;
					vec3 u_xlat3;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD2.xy + vec2(-2370.0, 575.0);
					    u_xlat0.xy = u_xlat0.xy * vec2(0.000844999973, 0.000844999973);
					    u_xlat0 = texture(_MaskTex, u_xlat0.xy);
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat3.xyz = u_xlat1.www * u_xlat1.xyz;
					    SV_Target0.w = u_xlat1.w;
					    u_xlat1.xyz = u_xlat3.xyz * _ColorB.xyz + (-u_xlat3.xyz);
					    u_xlat1.xyz = vec3(vec3(_AmountB, _AmountB, _AmountB)) * u_xlat1.xyz + u_xlat3.xyz;
					    u_xlat2.xyz = u_xlat3.xyz * _ColorA.xyz + (-u_xlat3.xyz);
					    u_xlat3.xyz = vec3(_AmountA) * u_xlat2.xyz + u_xlat3.xyz;
					    u_xlat1.xyz = (-u_xlat3.xyz) + u_xlat1.xyz;
					    SV_Target0.xyz = u_xlat0.xxx * u_xlat1.xyz + u_xlat3.xyz;
					    return;
					}"
				}
				SubProgram "d3d11 " {
					Keywords { "ETC1_EXTERNAL_ALPHA" "PIXELSNAP_ON" }
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
						vec4 _ColorA;
						vec4 _ColorB;
						float _AmountA;
						float _AmountB;
					};
					layout(std140) uniform UnityPerDrawSprite {
						vec4 unused_1_0;
						float _EnableExternalAlpha;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _AlphaTex;
					uniform  sampler2D _MaskTex;
					in  vec2 vs_TEXCOORD0;
					in  vec3 vs_TEXCOORD2;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					void main()
					{
					    u_xlat0.xy = vs_TEXCOORD2.xy + vec2(-2370.0, 575.0);
					    u_xlat0.xy = u_xlat0.xy * vec2(0.000844999973, 0.000844999973);
					    u_xlat0 = texture(_MaskTex, u_xlat0.xy);
					    u_xlat1 = texture(_AlphaTex, vs_TEXCOORD0.xy);
					    u_xlat2 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat3.x = u_xlat1.x + (-u_xlat2.w);
					    u_xlat3.x = _EnableExternalAlpha * u_xlat3.x + u_xlat2.w;
					    u_xlat1.xyz = u_xlat3.xxx * u_xlat2.xyz;
					    SV_Target0.w = u_xlat3.x;
					    u_xlat3.xyz = u_xlat1.xyz * _ColorB.xyz + (-u_xlat1.xyz);
					    u_xlat3.xyz = vec3(vec3(_AmountB, _AmountB, _AmountB)) * u_xlat3.xyz + u_xlat1.xyz;
					    u_xlat2.xyz = u_xlat1.xyz * _ColorA.xyz + (-u_xlat1.xyz);
					    u_xlat1.xyz = vec3(_AmountA) * u_xlat2.xyz + u_xlat1.xyz;
					    u_xlat3.xyz = u_xlat3.xyz + (-u_xlat1.xyz);
					    SV_Target0.xyz = u_xlat0.xxx * u_xlat3.xyz + u_xlat1.xyz;
					    return;
					}"
				}
			}
		}
	}
}