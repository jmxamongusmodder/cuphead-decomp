Shader "Hidden/FastBlur" {
	Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_Bloom ("Bloom (RGB)", 2D) = "black" {}
	}
	SubShader {
		Pass {
			ZWrite Off
			Cull Off
			GpuProgramID 50280
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
						vec4 _MainTex_TexelSize;
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
					in  vec2 in_TEXCOORD0;
					out vec2 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
					out vec2 vs_TEXCOORD2;
					out vec2 vs_TEXCOORD3;
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
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					in  vec2 vs_TEXCOORD2;
					in  vec2 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					void main()
					{
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD1.xy);
					    u_xlat0 = u_xlat0 + u_xlat1;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD2.xy);
					    u_xlat0 = u_xlat0 + u_xlat1;
					    u_xlat1 = texture(_MainTex, vs_TEXCOORD3.xy);
					    u_xlat0 = u_xlat0 + u_xlat1;
					    SV_Target0 = u_xlat0 * vec4(0.25, 0.25, 0.25, 0.25);
					    return;
					}"
				}
			}
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 105910
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
						vec4 _MainTex_TexelSize;
						vec4 _Parameter;
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
					in  vec2 in_TEXCOORD0;
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
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
					    u_xlat0.xw = _Parameter.xx;
					    u_xlat0.y = float(1.0);
					    u_xlat0.z = float(0.0);
					    u_xlat0.xy = u_xlat0.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat0.zw * u_xlat0.xy;
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
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					vec4 ImmCB_0_0_0[7];
					uniform  sampler2D _MainTex;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec2 u_xlat0;
					vec4 u_xlat1;
					int u_xlati2;
					vec4 u_xlat3;
					bool u_xlatb6;
					vec2 u_xlat8;
					void main()
					{
						ImmCB_0_0_0[0] = vec4(0.0205000006, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[1] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[2] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[3] = vec4(0.324000001, 0.0, 0.0, 1.0);
						ImmCB_0_0_0[4] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[5] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[6] = vec4(0.0205000006, 0.0, 0.0, 0.0);
					    u_xlat0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
					    u_xlat1.x = float(0.0);
					    u_xlat1.y = float(0.0);
					    u_xlat1.z = float(0.0);
					    u_xlat1.w = float(0.0);
					    u_xlat8.xy = u_xlat0.xy;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, u_xlat8.xy);
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1].xxxw + u_xlat1;
					        u_xlat8.xy = u_xlat8.xy + vs_TEXCOORD1.xy;
					    }
					    SV_Target0 = u_xlat1;
					    return;
					}"
				}
			}
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 191279
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
						vec4 _MainTex_TexelSize;
						vec4 _Parameter;
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
					in  vec2 in_TEXCOORD0;
					out vec4 vs_TEXCOORD0;
					out vec2 vs_TEXCOORD1;
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
					    u_xlat0.x = float(1.0);
					    u_xlat0.w = float(0.0);
					    u_xlat0.yz = _Parameter.xx;
					    u_xlat0.xy = u_xlat0.xy * _MainTex_TexelSize.xy;
					    vs_TEXCOORD1.xy = u_xlat0.zw * u_xlat0.xy;
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
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					vec4 ImmCB_0_0_0[7];
					uniform  sampler2D _MainTex;
					in  vec4 vs_TEXCOORD0;
					in  vec2 vs_TEXCOORD1;
					layout(location = 0) out vec4 SV_Target0;
					vec2 u_xlat0;
					vec4 u_xlat1;
					int u_xlati2;
					vec4 u_xlat3;
					bool u_xlatb6;
					vec2 u_xlat8;
					void main()
					{
						ImmCB_0_0_0[0] = vec4(0.0205000006, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[1] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[2] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[3] = vec4(0.324000001, 0.0, 0.0, 1.0);
						ImmCB_0_0_0[4] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[5] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[6] = vec4(0.0205000006, 0.0, 0.0, 0.0);
					    u_xlat0.xy = (-vs_TEXCOORD1.xy) * vec2(3.0, 3.0) + vs_TEXCOORD0.xy;
					    u_xlat1.x = float(0.0);
					    u_xlat1.y = float(0.0);
					    u_xlat1.z = float(0.0);
					    u_xlat1.w = float(0.0);
					    u_xlat8.xy = u_xlat0.xy;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<7 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, u_xlat8.xy);
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1].xxxw + u_xlat1;
					        u_xlat8.xy = u_xlat8.xy + vs_TEXCOORD1.xy;
					    }
					    SV_Target0 = u_xlat1;
					    return;
					}"
				}
			}
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 207766
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
						vec4 _MainTex_TexelSize;
						vec4 _Parameter;
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
					in  vec2 in_TEXCOORD0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
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
					    u_xlat0.x = _Parameter.x;
					    u_xlat0.y = 1.0;
					    u_xlat0.xy = u_xlat0.xy * _MainTex_TexelSize.xy;
					    u_xlat0.z = u_xlat0.y * _Parameter.x;
					    vs_TEXCOORD1 = u_xlat0.xzxz * vec4(-0.0, -3.0, 0.0, 3.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD2 = u_xlat0.xzxz * vec4(0.0, -2.0, -0.0, 2.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD3 = u_xlat0.xzxz * vec4(0.0, -1.0, -0.0, 1.0) + in_TEXCOORD0.xyxy;
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
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					vec4 ImmCB_0_0_0[7];
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					int u_xlati2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					bool u_xlatb7;
					vec4 phase0_Input0_2[3];
					void main()
					{
						ImmCB_0_0_0[0] = vec4(0.0205000006, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[1] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[2] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[3] = vec4(0.324000001, 0.0, 0.0, 1.0);
						ImmCB_0_0_0[4] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[5] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[6] = vec4(0.0205000006, 0.0, 0.0, 0.0);
					phase0_Input0_2[0] = vs_TEXCOORD1;
					phase0_Input0_2[1] = vs_TEXCOORD2;
					phase0_Input0_2[2] = vs_TEXCOORD3;
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat0 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0);
					    u_xlat1 = u_xlat0;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].xy);
					        u_xlat4 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].zw);
					        u_xlat3 = u_xlat3 + u_xlat4;
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1].xxxw + u_xlat1;
					    }
					    SV_Target0 = u_xlat1;
					    return;
					}"
				}
			}
		}
		Pass {
			ZTest Always
			ZWrite Off
			Cull Off
			GpuProgramID 323622
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
						vec4 _MainTex_TexelSize;
						vec4 _Parameter;
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
					in  vec2 in_TEXCOORD0;
					out vec2 vs_TEXCOORD0;
					out vec4 vs_TEXCOORD1;
					out vec4 vs_TEXCOORD2;
					out vec4 vs_TEXCOORD3;
					vec4 u_xlat0;
					vec4 u_xlat1;
					vec2 u_xlat2;
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
					    u_xlat2.x = 1.0;
					    u_xlat2.y = _Parameter.x;
					    u_xlat0.yz = u_xlat2.xy * _MainTex_TexelSize.xy;
					    u_xlat0.x = u_xlat0.y * _Parameter.x;
					    vs_TEXCOORD1 = u_xlat0.xzxz * vec4(-3.0, -0.0, 3.0, 0.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD2 = u_xlat0.xzxz * vec4(-2.0, 0.0, 2.0, -0.0) + in_TEXCOORD0.xyxy;
					    vs_TEXCOORD3 = u_xlat0.xzxz * vec4(-1.0, 0.0, 1.0, -0.0) + in_TEXCOORD0.xyxy;
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
					
					#define UNITY_SUPPORTS_UNIFORM_LOCATION 1
					#if UNITY_SUPPORTS_UNIFORM_LOCATION
					#define UNITY_LOCATION(x) layout(location = x)
					#define UNITY_BINDING(x) layout(binding = x, std140)
					#else
					#define UNITY_LOCATION(x)
					#define UNITY_BINDING(x) layout(std140)
					#endif
					vec4 ImmCB_0_0_0[7];
					uniform  sampler2D _MainTex;
					in  vec2 vs_TEXCOORD0;
					in  vec4 vs_TEXCOORD1;
					in  vec4 vs_TEXCOORD2;
					in  vec4 vs_TEXCOORD3;
					layout(location = 0) out vec4 SV_Target0;
					vec4 u_xlat0;
					vec4 u_xlat1;
					int u_xlati2;
					vec4 u_xlat3;
					vec4 u_xlat4;
					bool u_xlatb7;
					vec4 phase0_Input0_2[3];
					void main()
					{
						ImmCB_0_0_0[0] = vec4(0.0205000006, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[1] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[2] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[3] = vec4(0.324000001, 0.0, 0.0, 1.0);
						ImmCB_0_0_0[4] = vec4(0.231999993, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[5] = vec4(0.0855000019, 0.0, 0.0, 0.0);
						ImmCB_0_0_0[6] = vec4(0.0205000006, 0.0, 0.0, 0.0);
					phase0_Input0_2[0] = vs_TEXCOORD1;
					phase0_Input0_2[1] = vs_TEXCOORD2;
					phase0_Input0_2[2] = vs_TEXCOORD3;
					    u_xlat0 = texture(_MainTex, vs_TEXCOORD0.xy);
					    u_xlat0 = u_xlat0 * vec4(0.324000001, 0.324000001, 0.324000001, 1.0);
					    u_xlat1 = u_xlat0;
					    for(int u_xlati_loop_1 = 0 ; u_xlati_loop_1<3 ; u_xlati_loop_1++)
					    {
					        u_xlat3 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].xy);
					        u_xlat4 = texture(_MainTex, phase0_Input0_2[u_xlati_loop_1].zw);
					        u_xlat3 = u_xlat3 + u_xlat4;
					        u_xlat1 = u_xlat3 * ImmCB_0_0_0[u_xlati_loop_1].xxxw + u_xlat1;
					    }
					    SV_Target0 = u_xlat1;
					    return;
					}"
				}
			}
		}
	}
}