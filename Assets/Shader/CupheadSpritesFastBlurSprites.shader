Shader "Cuphead/Sprites/FastBlurSprites" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Vector) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		_BlurAmount ("Blur Amount", Range(0, 5)) = 0
		_BlurLerp ("Blur Lerp", Range(0, 5)) = 1
		_DullBlacksCutoff ("Dull Blacks Cutoff", Range(0, 1)) = 0
		_DullBlacksFactor ("Dull Blacks Factor", Range(0, 1)) = 0
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 10434
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
						vec4 _MainTex_TexelSize;
						vec4 unused_0_3;
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
					out vec2 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					out vec2 vs_TEXCOORD2;
					out vec2 vs_TEXCOORD3;
					out vec2 vs_NORMAL0;
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
					    vs_TEXCOORD0.xy = in_TEXCOORD0.xy + _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = _MainTex_TexelSize.xy * vec2(-0.5, -0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD2.xy = _MainTex_TexelSize.xy * vec2(0.5, -0.5) + in_TEXCOORD0.xy;
					    vs_TEXCOORD3.xy = _MainTex_TexelSize.xy * vec2(-0.5, 0.5) + in_TEXCOORD0.xy;
					    vs_NORMAL0.xy = in_TEXCOORD0.xy;
					    u_xlat0 = in_COLOR0 * _Color;
					    vs_COLOR0 = u_xlat0 * _RendererColor;
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
						float _BlurAmount;
						float _BlurLerp;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec2 vs_TEXCOORD2;
					in  vec2 vs_TEXCOORD3;
					in  vec2 vs_NORMAL0;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					float u_xlat2;
					bvec2 u_xlatb5;
					float u_xlat8;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat0 = u_xlat0 * vs_COLOR0;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat1 * vs_COLOR0 + u_xlat0;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat0 = u_xlat1 * vs_COLOR0 + u_xlat0;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat0 = u_xlat1 * vs_COLOR0 + u_xlat0;
					    u_xlat1 = texture(_MainTex, vs_NORMAL0.xy);
					    u_xlat0 = u_xlat0 * vec4(0.25, 0.25, 0.25, 0.25) + (-u_xlat1);
					    u_xlat2 = _BlurAmount / _BlurLerp;
					    u_xlat2 = clamp(u_xlat2, 0.0, 1.0);
					    u_xlatb5.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), vec4(_BlurLerp, _BlurAmount, _BlurLerp, _BlurLerp)).xy;
					    u_xlat8 = u_xlatb5.y ? 1.0 : float(0.0);
					    u_xlat2 = (u_xlatb5.x) ? u_xlat2 : u_xlat8;
					    u_xlat0 = vec4(u_xlat2) * u_xlat0 + u_xlat1;
					    SV_Target0.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 90749
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
						vec4 _MainTex_TexelSize;
						float _BlurAmount;
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
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
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
					    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
					    u_xlat0.xy = _MainTex_TexelSize.xy * vec2(0.0, 1.0);
					    vs_TEXCOORD1.xy = u_xlat0.xy * vec2(_BlurAmount);
					    vs_COLOR0 = in_COLOR0;
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[4];
						float _BlurAmount;
						float _BlurLerp;
						float _DullBlacksCutoff;
						float _DullBlacksFactor;
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec3 u_xlatb0;
					vec4 u_xlat1;
					float u_xlat2;
					int u_xlati2;
					vec4 u_xlat3;
					bvec2 u_xlatb6;
					vec2 u_xlat8;
					float u_xlat10;
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					    u_xlat0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
					    u_xlat8.xy = u_xlat0.xy;
					    u_xlat1.x = float(0.0);
					    u_xlat1.y = float(0.0);
					    u_xlat1.z = float(0.0);
					    u_xlat1.w = float(0.0);
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, u_xlat8.xy);
					        u_xlat3 = u_xlat3 * vs_COLOR0;
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat1;
					        u_xlat8.xy = u_xlat8.xy + vs_TEXCOORD1.xy;
					    }
					    u_xlatb0.xyz = greaterThanEqual(vec4(vec4(_DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff)), u_xlat1.xyzx).xyz;
					    u_xlat0.x = u_xlatb0.x ? float(1.0) : 0.0;
					    u_xlat0.y = u_xlatb0.y ? float(1.0) : 0.0;
					    u_xlat0.z = u_xlatb0.z ? float(1.0) : 0.0;
					;
					    u_xlat0.x = u_xlat0.x * _DullBlacksFactor;
					    u_xlat0.x = u_xlat0.y * u_xlat0.x;
					    u_xlat0.x = (-u_xlat0.x) * u_xlat0.z + 1.0;
					    u_xlat0.w = u_xlat0.x * u_xlat1.w;
					    u_xlat2 = _BlurAmount / _BlurLerp;
					    u_xlat2 = clamp(u_xlat2, 0.0, 1.0);
					    u_xlatb6.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), vec4(_BlurLerp, _BlurAmount, _BlurLerp, _BlurLerp)).xy;
					    u_xlat10 = u_xlatb6.y ? 1.0 : float(0.0);
					    u_xlat2 = (u_xlatb6.x) ? u_xlat2 : u_xlat10;
					    u_xlat1.w = 0.0;
					    u_xlat0.xyz = u_xlat1.xyz;
					    u_xlat0 = u_xlat0 + (-u_xlat1);
					    u_xlat0 = vec4(u_xlat2) * u_xlat0 + u_xlat1;
					    SV_Target0.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 180016
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
						vec4 _MainTex_TexelSize;
						float _BlurAmount;
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
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
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
					    vs_TEXCOORD0.zw = vec2(1.0, 1.0);
					    u_xlat0.xy = _MainTex_TexelSize.xy * vec2(1.0, 0.0);
					    vs_TEXCOORD1.xy = u_xlat0.xy * vec2(_BlurAmount);
					    vs_COLOR0 = in_COLOR0;
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[4];
						float _BlurAmount;
						float _BlurLerp;
						float _DullBlacksCutoff;
						float _DullBlacksFactor;
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec3 u_xlatb0;
					vec4 u_xlat1;
					float u_xlat2;
					int u_xlati2;
					vec4 u_xlat3;
					bvec2 u_xlatb6;
					vec2 u_xlat8;
					float u_xlat10;
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					    u_xlat0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
					    u_xlat8.xy = u_xlat0.xy;
					    u_xlat1.x = float(0.0);
					    u_xlat1.y = float(0.0);
					    u_xlat1.z = float(0.0);
					    u_xlat1.w = float(0.0);
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, u_xlat8.xy);
					        u_xlat3 = u_xlat3 * vs_COLOR0;
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat1;
					        u_xlat8.xy = u_xlat8.xy + vs_TEXCOORD1.xy;
					    }
					    u_xlatb0.xyz = greaterThanEqual(vec4(vec4(_DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff)), u_xlat1.xyzx).xyz;
					    u_xlat0.x = u_xlatb0.x ? float(1.0) : 0.0;
					    u_xlat0.y = u_xlatb0.y ? float(1.0) : 0.0;
					    u_xlat0.z = u_xlatb0.z ? float(1.0) : 0.0;
					;
					    u_xlat0.x = u_xlat0.x * _DullBlacksFactor;
					    u_xlat0.x = u_xlat0.y * u_xlat0.x;
					    u_xlat0.x = (-u_xlat0.x) * u_xlat0.z + 1.0;
					    u_xlat0.w = u_xlat0.x * u_xlat1.w;
					    u_xlat2 = _BlurAmount / _BlurLerp;
					    u_xlat2 = clamp(u_xlat2, 0.0, 1.0);
					    u_xlatb6.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), vec4(_BlurLerp, _BlurAmount, _BlurLerp, _BlurLerp)).xy;
					    u_xlat10 = u_xlatb6.y ? 1.0 : float(0.0);
					    u_xlat2 = (u_xlatb6.x) ? u_xlat2 : u_xlat10;
					    u_xlat1.w = 0.0;
					    u_xlat0.xyz = u_xlat1.xyz;
					    u_xlat0 = u_xlat0 + (-u_xlat1);
					    u_xlat0 = vec4(u_xlat2) * u_xlat0 + u_xlat1;
					    SV_Target0.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 249982
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
						vec4 _MainTex_TexelSize;
						float _BlurAmount;
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
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
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
					    u_xlat0.yw = _MainTex_TexelSize.yy * vec2(_BlurAmount);
					    u_xlat0.xz = vec2(_BlurAmount);
					    vs_TEXCOORD1 = u_xlat0.zwzw * vec4(-0.0, -3.0, 0.0, 3.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD2 = u_xlat0.zwzw * vec4(0.0, -2.0, -0.0, 2.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD3 = u_xlat0 * vec4(0.0, -1.0, -0.0, 1.0) + in_TEXCOORD0.xyxy;
					    vs_COLOR0 = in_COLOR0;
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[4];
						float _BlurAmount;
						float _BlurLerp;
						float _DullBlacksCutoff;
						float _DullBlacksFactor;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec3 u_xlatb0;
					vec4 u_xlat1;
					float u_xlat2;
					int u_xlati2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					bvec2 u_xlatb7;
					float u_xlat12;
					vec4 phase0_Input0_2[3];
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					phase0_Input0_2[0] = vs_TEXCOORD1;
					phase0_Input0_2[1] = vs_TEXCOORD2;
					phase0_Input0_2[2] = vs_TEXCOORD3;
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat0 * vs_COLOR0;
					    u_xlat0 = u_xlat0 * vec4(0.324000001, 0.324000001, 0.324000001, 0.324000001);
					    u_xlat1 = u_xlat0;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].xy);
					        u_xlat4 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].zw);
					        u_xlat4 = u_xlat4 * vs_COLOR0;
					        u_xlat3 = u_xlat3 * vs_COLOR0 + u_xlat4;
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat1;
					    }
					    u_xlatb0.xyz = greaterThanEqual(vec4(vec4(_DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff)), u_xlat1.xyzx).xyz;
					    u_xlat0.x = u_xlatb0.x ? float(1.0) : 0.0;
					    u_xlat0.y = u_xlatb0.y ? float(1.0) : 0.0;
					    u_xlat0.z = u_xlatb0.z ? float(1.0) : 0.0;
					;
					    u_xlat0.x = u_xlat0.x * _DullBlacksFactor;
					    u_xlat0.x = u_xlat0.y * u_xlat0.x;
					    u_xlat0.x = (-u_xlat0.x) * u_xlat0.z + 1.0;
					    u_xlat0.w = u_xlat0.x * u_xlat1.w;
					    u_xlat2 = _BlurAmount / _BlurLerp;
					    u_xlat2 = clamp(u_xlat2, 0.0, 1.0);
					    u_xlatb7.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), vec4(_BlurLerp, _BlurAmount, _BlurLerp, _BlurLerp)).xy;
					    u_xlat12 = u_xlatb7.y ? 1.0 : float(0.0);
					    u_xlat2 = (u_xlatb7.x) ? u_xlat2 : u_xlat12;
					    u_xlat1.w = 0.0;
					    u_xlat0.xyz = u_xlat1.xyz;
					    u_xlat0 = u_xlat0 + (-u_xlat1);
					    u_xlat0 = vec4(u_xlat2) * u_xlat0 + u_xlat1;
					    SV_Target0.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 288355
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
						vec4 _MainTex_TexelSize;
						float _BlurAmount;
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
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
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
					    u_xlat0.xz = _MainTex_TexelSize.xx * vec2(_BlurAmount);
					    u_xlat0.yw = vec2(_BlurAmount);
					    vs_TEXCOORD1 = u_xlat0.zwzw * vec4(-3.0, -0.0, 3.0, 0.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD2 = u_xlat0.zwzw * vec4(-2.0, 0.0, 2.0, -0.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD3 = u_xlat0 * vec4(-1.0, 0.0, 1.0, -0.0) + in_TEXCOORD0.xyxy;
					    vs_COLOR0 = in_COLOR0;
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
					float ImmCB_0_0_0[7];
					layout(std140) uniform PGlobals {
						vec4 unused_0_0[4];
						float _BlurAmount;
						float _BlurLerp;
						float _DullBlacksCutoff;
						float _DullBlacksFactor;
					};
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					in  vec4 vs_COLOR0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bvec3 u_xlatb0;
					vec4 u_xlat1;
					float u_xlat2;
					int u_xlati2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					bvec2 u_xlatb7;
					float u_xlat12;
					vec4 phase0_Input0_2[3];
					void main()
					{
						ImmCB_0_0_0[0] = 0.0205000006;
						ImmCB_0_0_0[1] = 0.0855000019;
						ImmCB_0_0_0[2] = 0.231999993;
						ImmCB_0_0_0[3] = 0.324000001;
						ImmCB_0_0_0[4] = 0.231999993;
						ImmCB_0_0_0[5] = 0.0855000019;
						ImmCB_0_0_0[6] = 0.0205000006;
					phase0_Input0_2[0] = vs_TEXCOORD1;
					phase0_Input0_2[1] = vs_TEXCOORD2;
					phase0_Input0_2[2] = vs_TEXCOORD3;
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat0 * vs_COLOR0;
					    u_xlat0 = u_xlat0 * vec4(0.324000001, 0.324000001, 0.324000001, 0.324000001);
					    u_xlat1 = u_xlat0;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].xy);
					        u_xlat4 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].zw);
					        u_xlat4 = u_xlat4 * vs_COLOR0;
					        u_xlat3 = u_xlat3 * vs_COLOR0 + u_xlat4;
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1] + u_xlat1;
					    }
					    u_xlatb0.xyz = greaterThanEqual(vec4(vec4(_DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff, _DullBlacksCutoff)), u_xlat1.xyzx).xyz;
					    u_xlat0.x = u_xlatb0.x ? float(1.0) : 0.0;
					    u_xlat0.y = u_xlatb0.y ? float(1.0) : 0.0;
					    u_xlat0.z = u_xlatb0.z ? float(1.0) : 0.0;
					;
					    u_xlat0.x = u_xlat0.x * _DullBlacksFactor;
					    u_xlat0.x = u_xlat0.y * u_xlat0.x;
					    u_xlat0.x = (-u_xlat0.x) * u_xlat0.z + 1.0;
					    u_xlat0.w = u_xlat0.x * u_xlat1.w;
					    u_xlat2 = _BlurAmount / _BlurLerp;
					    u_xlat2 = clamp(u_xlat2, 0.0, 1.0);
					    u_xlatb7.xy = lessThan(vec4(0.0, 0.0, 0.0, 0.0), vec4(_BlurLerp, _BlurAmount, _BlurLerp, _BlurLerp)).xy;
					    u_xlat12 = u_xlatb7.y ? 1.0 : float(0.0);
					    u_xlat2 = (u_xlatb7.x) ? u_xlat2 : u_xlat12;
					    u_xlat1.w = 0.0;
					    u_xlat0.xyz = u_xlat1.xyz;
					    u_xlat0 = u_xlat0 + (-u_xlat1);
					    u_xlat0 = vec4(u_xlat2) * u_xlat0 + u_xlat1;
					    SV_Target0.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
	}
}