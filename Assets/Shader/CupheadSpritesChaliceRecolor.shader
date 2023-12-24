Shader "Cuphead/Sprites/ChaliceRecolor" {
	Properties {
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Vector) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		[HideInInspector] _RendererColor ("RendererColor", Vector) = (1,1,1,1)
		[HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
		[PerRendererData] _AlphaTex ("External Alpha", 2D) = "white" {}
		[PerRendererData] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
		_Threshold ("Recolor Threshold", Range(0.7, 1.01)) = 1
		_ThresholdShoes ("Recolor Threshold Shoes", Range(0.7, 1.01)) = 1
		_RecolorFactor ("Recolor Factor", Range(0, 1)) = 1
	}
	SubShader {
		Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
		Pass {
			Tags { "CanUseSpriteAtlas" = "true" "IGNOREPROJECTOR" = "true" "PreviewType" = "Plane" "QUEUE" = "Transparent" "RenderType" = "Transparent" }
			Blend One OneMinusSrcAlpha, One OneMinusSrcAlpha
			ZWrite Off
			Cull Off
			GpuProgramID 41378
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
						float _Threshold;
						float _ThresholdShoes;
						float _RecolorFactor;
					};
					uniform  sampler2D _MainTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					bool u_xlatb3;
					vec3 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat5;
					float u_xlat8;
					float u_xlat12;
					bool u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					void main()
					{
					    u_xlat0.z = float(-1.0);
					    u_xlat0.w = float(0.666666687);
					    u_xlat1.z = float(1.0);
					    u_xlat1.w = float(-1.0);
					    u_xlat2 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlatb3 = u_xlat2.y>=u_xlat2.z;
					    u_xlat3.x = u_xlatb3 ? 1.0 : float(0.0);
					    u_xlat0.xy = u_xlat2.zy;
					    u_xlat1.xy = (-u_xlat0.xy) + u_xlat2.yz;
					    u_xlat0 = u_xlat3.xxxx * u_xlat1.xywz + u_xlat0.xywz;
					    u_xlat1.z = u_xlat0.w;
					    u_xlatb3 = u_xlat2.x>=u_xlat0.x;
					    u_xlat3.x = u_xlatb3 ? 1.0 : float(0.0);
					    u_xlat0.w = u_xlat2.x;
					    u_xlat1.xyw = u_xlat0.wyx;
					    u_xlat1 = (-u_xlat0) + u_xlat1;
					    u_xlat0 = u_xlat3.xxxx * u_xlat1.yzxw + u_xlat0.yzxw;
					    u_xlat1.x = min(u_xlat0.x, u_xlat0.w);
					    u_xlat1.x = u_xlat0.z + (-u_xlat1.x);
					    u_xlat5.x = u_xlat1.x * 6.0 + 1.00000001e-10;
					    u_xlat12 = (-u_xlat0.x) + u_xlat0.w;
					    u_xlat12 = u_xlat12 / u_xlat5.x;
					    u_xlat12 = u_xlat12 + u_xlat0.y;
					    u_xlat0.x = abs(u_xlat12);
					    u_xlat5.xy = u_xlat0.zz + vec2(1.00000001e-10, 0.0117647052);
					    u_xlat0.y = u_xlat1.x / u_xlat5.x;
					    u_xlat1.xyz = u_xlat5.yyy * vec3(1.0, 0.823045373, 0.164609075);
					    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat0.xyw = vec3(u_xlat12) * u_xlat0.xyz;
					    u_xlat13 = dot(u_xlat0.xyw, vec3(0.136546776, 0.343266487, 0.929259479));
					    u_xlat0.x = dot(u_xlat0.xyw, vec3(0.496588767, 0.321275443, 0.806338429));
					    u_xlatb4 = _Threshold<u_xlat13;
					    u_xlatb12 = u_xlat13<u_xlat0.x;
					    u_xlatb0 = _ThresholdShoes<u_xlat0.x;
					    u_xlat12 = (u_xlatb12) ? 0.0117647052 : 0.0313725471;
					    u_xlat8 = u_xlat12 + u_xlat0.z;
					    u_xlat12 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat3.xyz = vec3(u_xlat12) * u_xlat2.xyz;
					    u_xlat12 = dot(u_xlat3.xyz, vec3(0.657744825, 0.615761101, 0.433831692));
					    u_xlat13 = dot(u_xlat3.xyz, vec3(0.434994638, 0.571292937, 0.695991397));
					    u_xlatb13 = _ThresholdShoes<u_xlat13;
					    u_xlatb0 = u_xlatb0 && u_xlatb13;
					    u_xlatb12 = _Threshold<u_xlat12;
					    u_xlatb4 = u_xlatb12 && u_xlatb4;
					    u_xlat4.xyz = (bool(u_xlatb4)) ? vec3(u_xlat8) : u_xlat2.xyz;
					    u_xlat0.xyz = (bool(u_xlatb0)) ? u_xlat1.xyz : u_xlat4.xyz;
					    u_xlat1.x = (-_RecolorFactor) + 1.0;
					    u_xlat1 = u_xlat2 * u_xlat1.xxxx;
					    u_xlat0.w = u_xlat2.w;
					    u_xlat0 = vec4(vec4(_RecolorFactor, _RecolorFactor, _RecolorFactor, _RecolorFactor)) * u_xlat0 + u_xlat1;
					    u_xlat0 = u_xlat0 * vs_COLOR0;
					    SV_Target0.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
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
						float _Threshold;
						float _ThresholdShoes;
						float _RecolorFactor;
					};
					layout(std140) uniform UnityPerDrawSprite {
						vec4 unused_1_0;
						float _EnableExternalAlpha;
					};
					uniform  sampler2D _MainTex;
					uniform  sampler2D _AlphaTex;
					in  vec4 vs_COLOR0;
					in  vec2 vs_TEXCOORD0;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					bool u_xlatb0;
					vec4 u_xlat1;
					vec4 u_xlat2;
					vec3 u_xlat3;
					bool u_xlatb3;
					vec3 u_xlat4;
					bool u_xlatb4;
					vec2 u_xlat5;
					float u_xlat8;
					float u_xlat12;
					bool u_xlatb12;
					float u_xlat13;
					bool u_xlatb13;
					void main()
					{
					    u_xlat0.z = float(-1.0);
					    u_xlat0.w = float(0.666666687);
					    u_xlat1.z = float(1.0);
					    u_xlat1.w = float(-1.0);
					    u_xlat2 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlatb3 = u_xlat2.y>=u_xlat2.z;
					    u_xlat3.x = u_xlatb3 ? 1.0 : float(0.0);
					    u_xlat0.xy = u_xlat2.zy;
					    u_xlat1.xy = (-u_xlat0.xy) + u_xlat2.yz;
					    u_xlat0 = u_xlat3.xxxx * u_xlat1.xywz + u_xlat0.xywz;
					    u_xlat1.z = u_xlat0.w;
					    u_xlatb3 = u_xlat2.x>=u_xlat0.x;
					    u_xlat3.x = u_xlatb3 ? 1.0 : float(0.0);
					    u_xlat0.w = u_xlat2.x;
					    u_xlat1.xyw = u_xlat0.wyx;
					    u_xlat1 = (-u_xlat0) + u_xlat1;
					    u_xlat0 = u_xlat3.xxxx * u_xlat1.yzxw + u_xlat0.yzxw;
					    u_xlat1.x = min(u_xlat0.x, u_xlat0.w);
					    u_xlat1.x = u_xlat0.z + (-u_xlat1.x);
					    u_xlat5.x = u_xlat1.x * 6.0 + 1.00000001e-10;
					    u_xlat12 = (-u_xlat0.x) + u_xlat0.w;
					    u_xlat12 = u_xlat12 / u_xlat5.x;
					    u_xlat12 = u_xlat12 + u_xlat0.y;
					    u_xlat0.x = abs(u_xlat12);
					    u_xlat5.xy = u_xlat0.zz + vec2(1.00000001e-10, 0.0117647052);
					    u_xlat0.y = u_xlat1.x / u_xlat5.x;
					    u_xlat1.xyz = u_xlat5.yyy * vec3(1.0, 0.823045373, 0.164609075);
					    u_xlat12 = dot(u_xlat0.xyz, u_xlat0.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat0.xyw = vec3(u_xlat12) * u_xlat0.xyz;
					    u_xlat13 = dot(u_xlat0.xyw, vec3(0.136546776, 0.343266487, 0.929259479));
					    u_xlat0.x = dot(u_xlat0.xyw, vec3(0.496588767, 0.321275443, 0.806338429));
					    u_xlatb4 = _Threshold<u_xlat13;
					    u_xlatb12 = u_xlat13<u_xlat0.x;
					    u_xlatb0 = _ThresholdShoes<u_xlat0.x;
					    u_xlat12 = (u_xlatb12) ? 0.0117647052 : 0.0313725471;
					    u_xlat8 = u_xlat12 + u_xlat0.z;
					    u_xlat12 = dot(u_xlat2.xyz, u_xlat2.xyz);
					    u_xlat12 = inversesqrt(u_xlat12);
					    u_xlat3.xyz = vec3(u_xlat12) * u_xlat2.xyz;
					    u_xlat12 = dot(u_xlat3.xyz, vec3(0.657744825, 0.615761101, 0.433831692));
					    u_xlat13 = dot(u_xlat3.xyz, vec3(0.434994638, 0.571292937, 0.695991397));
					    u_xlatb13 = _ThresholdShoes<u_xlat13;
					    u_xlatb0 = u_xlatb0 && u_xlatb13;
					    u_xlatb12 = _Threshold<u_xlat12;
					    u_xlatb4 = u_xlatb12 && u_xlatb4;
					    u_xlat4.xyz = (bool(u_xlatb4)) ? vec3(u_xlat8) : u_xlat2.xyz;
					    u_xlat0.xyz = (bool(u_xlatb0)) ? u_xlat1.xyz : u_xlat4.xyz;
					    u_xlat1 = texture(_AlphaTex, vs_TEXCOORD0.xy);
					    u_xlat1.x = (-u_xlat2.w) + u_xlat1.x;
					    u_xlat0.w = _EnableExternalAlpha * u_xlat1.x + u_xlat2.w;
					    u_xlat2.w = u_xlat0.w;
					    u_xlat1.x = (-_RecolorFactor) + 1.0;
					    u_xlat1 = u_xlat2 * u_xlat1.xxxx;
					    u_xlat0 = vec4(vec4(_RecolorFactor, _RecolorFactor, _RecolorFactor, _RecolorFactor)) * u_xlat0 + u_xlat1;
					    u_xlat0 = u_xlat0 * vs_COLOR0;
					    SV_Target0.xyz = u_xlat0.www * u_xlat0.xyz;
					    SV_Target0.w = u_xlat0.w;
					    return;
					}"
				}
			}
		}
	}
}