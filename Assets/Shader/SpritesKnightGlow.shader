Shader "Sprites/KnightGlow" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (0,0,0,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Vector) = (0,0,0,1)
		[HideInInspector] _Flip ("Flip", Vector) = (0,0,0,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		_OutlineColor ("Outline color", Vector) = (0.5,0.5,0.5,1)
		_DimFactor ("Dim factor", Float) = 0
		_OutlineIncreaseFactor ("Outline increase factor", Float) = 0.3
		_LightingVector ("Lighting vector", Vector) = (2,0.4,0,0)
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 22832
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
					layout(std140) uniform UnityPerDrawSprite {
						vec4 _RendererColor;
						vec4 unused_3_1;
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
					layout(std140) uniform UnityPerDrawSprite {
						vec4 _RendererColor;
						vec4 unused_3_1;
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
						vec4 unused_0_2[3];
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
						vec4 unused_0_2[3];
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
						vec4 unused_0_0[3];
						vec4 _OutlineColor;
						vec4 _MainTex_TexelSize;
						float _DimFactor;
						float _OutlineIncreaseFactor;
						vec2 _LightingVector;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec3 u_xlat5;
					bool u_xlatb6;
					float u_xlat10;
					float u_xlat13;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlatb1 = 0.100000001<u_xlat0.w;
					    if(u_xlatb1){
					        u_xlat1.yz = _MainTex_TexelSize.xy * vec2(_LightingVector.x, _LightingVector.y);
					        u_xlat13 = vs_TEXCOORD0.x * 4.0 + -2.0;
					        u_xlat13 = max(u_xlat13, -0.5);
					        u_xlat13 = min(u_xlat13, 0.5);
					        u_xlat1.x = u_xlat13 * u_xlat1.y;
					        u_xlat5.xz = u_xlat1.xz + vs_TEXCOORD0.xy;
					        u_xlat2 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb2 = u_xlat2.w<0.100000001;
					        u_xlat2.x = u_xlatb2 ? _OutlineIncreaseFactor : float(0.0);
					        u_xlat5.xz = u_xlat1.xz + u_xlat5.xz;
					        u_xlat3 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb6 = u_xlat3.w<0.100000001;
					        u_xlat10 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb6) ? u_xlat10 : u_xlat2.x;
					        u_xlat5.xz = u_xlat1.xz + u_xlat5.xz;
					        u_xlat3 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb6 = u_xlat3.w<0.100000001;
					        u_xlat10 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb6) ? u_xlat10 : u_xlat2.x;
					        u_xlat5.xz = u_xlat1.xz + u_xlat5.xz;
					        u_xlat3 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb6 = u_xlat3.w<0.100000001;
					        u_xlat10 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb6) ? u_xlat10 : u_xlat2.x;
					        u_xlat1.xy = u_xlat1.xz + u_xlat5.xz;
					        u_xlat1 = texture(_MainTex, u_xlat1.xy);
					        u_xlatb1 = u_xlat1.w<0.100000001;
					        u_xlat5.x = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat1.x = (u_xlatb1) ? u_xlat5.x : u_xlat2.x;
					        u_xlat1.x = max(u_xlat1.x, 0.0);
					        u_xlat1.x = min(u_xlat1.x, _OutlineColor.w);
					        u_xlat1.x = (-u_xlat1.x) + 1.0;
					        u_xlat2.xyz = _OutlineColor.xyz;
					        u_xlat2.w = 1.0;
					        u_xlat3.xyz = (-u_xlat2.xyz);
					        u_xlat3.w = _DimFactor + -1.0;
					        SV_Target0 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					        return;
					    } else {
					        SV_Target0 = u_xlat0;
					        return;
					    }
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
						vec4 unused_0_0[3];
						vec4 _OutlineColor;
						vec4 _MainTex_TexelSize;
						float _DimFactor;
						float _OutlineIncreaseFactor;
						vec2 _LightingVector;
					};
					layout(std140) uniform UnityPerDrawSprite {
						vec4 unused_1_0;
						float _EnableExternalAlpha;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _AlphaTex;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec3 u_xlat6;
					float u_xlat7;
					bool u_xlatb7;
					float u_xlat12;
					float u_xlat16;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat1 = texture(_AlphaTex, vs_TEXCOORD0.xy);
					    u_xlat1.x = (-u_xlat0.w) + u_xlat1.x;
					    u_xlat0.w = _EnableExternalAlpha * u_xlat1.x + u_xlat0.w;
					    u_xlatb1 = 0.100000001<u_xlat0.w;
					    if(u_xlatb1){
					        u_xlat1.yz = _MainTex_TexelSize.xy * vec2(_LightingVector.x, _LightingVector.y);
					        u_xlat16 = vs_TEXCOORD0.x * 4.0 + -2.0;
					        u_xlat16 = max(u_xlat16, -0.5);
					        u_xlat16 = min(u_xlat16, 0.5);
					        u_xlat1.x = u_xlat16 * u_xlat1.y;
					        u_xlat6.xz = u_xlat1.xz + vs_TEXCOORD0.xy;
					        u_xlat2 = texture(_MainTex, u_xlat6.xz);
					        u_xlat3 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat2.x = (-u_xlat2.w) + u_xlat3.x;
					        u_xlat2.x = _EnableExternalAlpha * u_xlat2.x + u_xlat2.w;
					        u_xlatb2 = u_xlat2.x<0.100000001;
					        u_xlat2.x = u_xlatb2 ? _OutlineIncreaseFactor : float(0.0);
					        u_xlat6.xz = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat6.xz);
					        u_xlat4 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat7 = (-u_xlat3.w) + u_xlat4.x;
					        u_xlat7 = _EnableExternalAlpha * u_xlat7 + u_xlat3.w;
					        u_xlatb7 = u_xlat7<0.100000001;
					        u_xlat12 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb7) ? u_xlat12 : u_xlat2.x;
					        u_xlat6.xz = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat6.xz);
					        u_xlat4 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat7 = (-u_xlat3.w) + u_xlat4.x;
					        u_xlat7 = _EnableExternalAlpha * u_xlat7 + u_xlat3.w;
					        u_xlatb7 = u_xlat7<0.100000001;
					        u_xlat12 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb7) ? u_xlat12 : u_xlat2.x;
					        u_xlat6.xz = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat6.xz);
					        u_xlat4 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat7 = (-u_xlat3.w) + u_xlat4.x;
					        u_xlat7 = _EnableExternalAlpha * u_xlat7 + u_xlat3.w;
					        u_xlatb7 = u_xlat7<0.100000001;
					        u_xlat12 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb7) ? u_xlat12 : u_xlat2.x;
					        u_xlat1.xy = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat1.xy);
					        u_xlat1 = texture(_AlphaTex, u_xlat1.xy);
					        u_xlat1.x = (-u_xlat3.w) + u_xlat1.x;
					        u_xlat1.x = _EnableExternalAlpha * u_xlat1.x + u_xlat3.w;
					        u_xlatb1 = u_xlat1.x<0.100000001;
					        u_xlat6.x = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat1.x = (u_xlatb1) ? u_xlat6.x : u_xlat2.x;
					        u_xlat1.x = max(u_xlat1.x, 0.0);
					        u_xlat1.x = min(u_xlat1.x, _OutlineColor.w);
					        u_xlat1.x = (-u_xlat1.x) + 1.0;
					        u_xlat2.xyz = _OutlineColor.xyz;
					        u_xlat2.w = 1.0;
					        u_xlat3.xyz = (-u_xlat2.xyz);
					        u_xlat3.w = _DimFactor + -1.0;
					        SV_Target0 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					        return;
					    } else {
					        SV_Target0 = u_xlat0;
					        return;
					    }
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
						vec4 unused_0_0[3];
						vec4 _OutlineColor;
						vec4 _MainTex_TexelSize;
						float _DimFactor;
						float _OutlineIncreaseFactor;
						vec2 _LightingVector;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec3 u_xlat5;
					bool u_xlatb6;
					float u_xlat10;
					float u_xlat13;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlatb1 = 0.100000001<u_xlat0.w;
					    if(u_xlatb1){
					        u_xlat1.yz = _MainTex_TexelSize.xy * vec2(_LightingVector.x, _LightingVector.y);
					        u_xlat13 = vs_TEXCOORD0.x * 4.0 + -2.0;
					        u_xlat13 = max(u_xlat13, -0.5);
					        u_xlat13 = min(u_xlat13, 0.5);
					        u_xlat1.x = u_xlat13 * u_xlat1.y;
					        u_xlat5.xz = u_xlat1.xz + vs_TEXCOORD0.xy;
					        u_xlat2 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb2 = u_xlat2.w<0.100000001;
					        u_xlat2.x = u_xlatb2 ? _OutlineIncreaseFactor : float(0.0);
					        u_xlat5.xz = u_xlat1.xz + u_xlat5.xz;
					        u_xlat3 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb6 = u_xlat3.w<0.100000001;
					        u_xlat10 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb6) ? u_xlat10 : u_xlat2.x;
					        u_xlat5.xz = u_xlat1.xz + u_xlat5.xz;
					        u_xlat3 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb6 = u_xlat3.w<0.100000001;
					        u_xlat10 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb6) ? u_xlat10 : u_xlat2.x;
					        u_xlat5.xz = u_xlat1.xz + u_xlat5.xz;
					        u_xlat3 = texture(_MainTex, u_xlat5.xz);
					        u_xlatb6 = u_xlat3.w<0.100000001;
					        u_xlat10 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb6) ? u_xlat10 : u_xlat2.x;
					        u_xlat1.xy = u_xlat1.xz + u_xlat5.xz;
					        u_xlat1 = texture(_MainTex, u_xlat1.xy);
					        u_xlatb1 = u_xlat1.w<0.100000001;
					        u_xlat5.x = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat1.x = (u_xlatb1) ? u_xlat5.x : u_xlat2.x;
					        u_xlat1.x = max(u_xlat1.x, 0.0);
					        u_xlat1.x = min(u_xlat1.x, _OutlineColor.w);
					        u_xlat1.x = (-u_xlat1.x) + 1.0;
					        u_xlat2.xyz = _OutlineColor.xyz;
					        u_xlat2.w = 1.0;
					        u_xlat3.xyz = (-u_xlat2.xyz);
					        u_xlat3.w = _DimFactor + -1.0;
					        SV_Target0 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					        return;
					    } else {
					        SV_Target0 = u_xlat0;
					        return;
					    }
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
						vec4 unused_0_0[3];
						vec4 _OutlineColor;
						vec4 _MainTex_TexelSize;
						float _DimFactor;
						float _OutlineIncreaseFactor;
						vec2 _LightingVector;
					};
					layout(std140) uniform UnityPerDrawSprite {
						vec4 unused_1_0;
						float _EnableExternalAlpha;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _AlphaTex;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					bool u_xlatb1;
					vec4 u_xlat2;
					bool u_xlatb2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					vec3 u_xlat6;
					float u_xlat7;
					bool u_xlatb7;
					float u_xlat12;
					float u_xlat16;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat1 = texture(_AlphaTex, vs_TEXCOORD0.xy);
					    u_xlat1.x = (-u_xlat0.w) + u_xlat1.x;
					    u_xlat0.w = _EnableExternalAlpha * u_xlat1.x + u_xlat0.w;
					    u_xlatb1 = 0.100000001<u_xlat0.w;
					    if(u_xlatb1){
					        u_xlat1.yz = _MainTex_TexelSize.xy * vec2(_LightingVector.x, _LightingVector.y);
					        u_xlat16 = vs_TEXCOORD0.x * 4.0 + -2.0;
					        u_xlat16 = max(u_xlat16, -0.5);
					        u_xlat16 = min(u_xlat16, 0.5);
					        u_xlat1.x = u_xlat16 * u_xlat1.y;
					        u_xlat6.xz = u_xlat1.xz + vs_TEXCOORD0.xy;
					        u_xlat2 = texture(_MainTex, u_xlat6.xz);
					        u_xlat3 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat2.x = (-u_xlat2.w) + u_xlat3.x;
					        u_xlat2.x = _EnableExternalAlpha * u_xlat2.x + u_xlat2.w;
					        u_xlatb2 = u_xlat2.x<0.100000001;
					        u_xlat2.x = u_xlatb2 ? _OutlineIncreaseFactor : float(0.0);
					        u_xlat6.xz = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat6.xz);
					        u_xlat4 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat7 = (-u_xlat3.w) + u_xlat4.x;
					        u_xlat7 = _EnableExternalAlpha * u_xlat7 + u_xlat3.w;
					        u_xlatb7 = u_xlat7<0.100000001;
					        u_xlat12 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb7) ? u_xlat12 : u_xlat2.x;
					        u_xlat6.xz = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat6.xz);
					        u_xlat4 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat7 = (-u_xlat3.w) + u_xlat4.x;
					        u_xlat7 = _EnableExternalAlpha * u_xlat7 + u_xlat3.w;
					        u_xlatb7 = u_xlat7<0.100000001;
					        u_xlat12 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb7) ? u_xlat12 : u_xlat2.x;
					        u_xlat6.xz = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat6.xz);
					        u_xlat4 = texture(_AlphaTex, u_xlat6.xz);
					        u_xlat7 = (-u_xlat3.w) + u_xlat4.x;
					        u_xlat7 = _EnableExternalAlpha * u_xlat7 + u_xlat3.w;
					        u_xlatb7 = u_xlat7<0.100000001;
					        u_xlat12 = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat2.x = (u_xlatb7) ? u_xlat12 : u_xlat2.x;
					        u_xlat1.xy = u_xlat1.xz + u_xlat6.xz;
					        u_xlat3 = texture(_MainTex, u_xlat1.xy);
					        u_xlat1 = texture(_AlphaTex, u_xlat1.xy);
					        u_xlat1.x = (-u_xlat3.w) + u_xlat1.x;
					        u_xlat1.x = _EnableExternalAlpha * u_xlat1.x + u_xlat3.w;
					        u_xlatb1 = u_xlat1.x<0.100000001;
					        u_xlat6.x = u_xlat2.x + _OutlineIncreaseFactor;
					        u_xlat1.x = (u_xlatb1) ? u_xlat6.x : u_xlat2.x;
					        u_xlat1.x = max(u_xlat1.x, 0.0);
					        u_xlat1.x = min(u_xlat1.x, _OutlineColor.w);
					        u_xlat1.x = (-u_xlat1.x) + 1.0;
					        u_xlat2.xyz = _OutlineColor.xyz;
					        u_xlat2.w = 1.0;
					        u_xlat3.xyz = (-u_xlat2.xyz);
					        u_xlat3.w = _DimFactor + -1.0;
					        SV_Target0 = u_xlat1.xxxx * u_xlat3 + u_xlat2;
					        return;
					    } else {
					        SV_Target0 = u_xlat0;
					        return;
					    }
					    return;
					}"
				}
			}
		}
	}
}