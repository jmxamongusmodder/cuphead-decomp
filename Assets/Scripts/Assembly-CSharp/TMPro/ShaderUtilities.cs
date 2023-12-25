using System;
using System.Linq;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000CB1 RID: 3249
	public static class ShaderUtilities
	{
		// Token: 0x0600519B RID: 20891 RVA: 0x0029A188 File Offset: 0x00298588
		public static void GetShaderPropertyIDs()
		{
			if (!ShaderUtilities.isInitialized)
			{
				ShaderUtilities.isInitialized = true;
				ShaderUtilities.ID_MainTex = Shader.PropertyToID("_MainTex");
				ShaderUtilities.ID_FaceTex = Shader.PropertyToID("_FaceTex");
				ShaderUtilities.ID_FaceColor = Shader.PropertyToID("_FaceColor");
				ShaderUtilities.ID_FaceDilate = Shader.PropertyToID("_FaceDilate");
				ShaderUtilities.ID_Shininess = Shader.PropertyToID("_FaceShininess");
				ShaderUtilities.ID_UnderlayColor = Shader.PropertyToID("_UnderlayColor");
				ShaderUtilities.ID_UnderlayOffsetX = Shader.PropertyToID("_UnderlayOffsetX");
				ShaderUtilities.ID_UnderlayOffsetY = Shader.PropertyToID("_UnderlayOffsetY");
				ShaderUtilities.ID_UnderlayDilate = Shader.PropertyToID("_UnderlayDilate");
				ShaderUtilities.ID_UnderlaySoftness = Shader.PropertyToID("_UnderlaySoftness");
				ShaderUtilities.ID_WeightNormal = Shader.PropertyToID("_WeightNormal");
				ShaderUtilities.ID_WeightBold = Shader.PropertyToID("_WeightBold");
				ShaderUtilities.ID_OutlineTex = Shader.PropertyToID("_OutlineTex");
				ShaderUtilities.ID_OutlineWidth = Shader.PropertyToID("_OutlineWidth");
				ShaderUtilities.ID_OutlineSoftness = Shader.PropertyToID("_OutlineSoftness");
				ShaderUtilities.ID_OutlineColor = Shader.PropertyToID("_OutlineColor");
				ShaderUtilities.ID_GradientScale = Shader.PropertyToID("_GradientScale");
				ShaderUtilities.ID_ScaleX = Shader.PropertyToID("_ScaleX");
				ShaderUtilities.ID_ScaleY = Shader.PropertyToID("_ScaleY");
				ShaderUtilities.ID_PerspectiveFilter = Shader.PropertyToID("_PerspectiveFilter");
				ShaderUtilities.ID_TextureWidth = Shader.PropertyToID("_TextureWidth");
				ShaderUtilities.ID_TextureHeight = Shader.PropertyToID("_TextureHeight");
				ShaderUtilities.ID_BevelAmount = Shader.PropertyToID("_Bevel");
				ShaderUtilities.ID_LightAngle = Shader.PropertyToID("_LightAngle");
				ShaderUtilities.ID_EnvMap = Shader.PropertyToID("_Cube");
				ShaderUtilities.ID_EnvMatrix = Shader.PropertyToID("_EnvMatrix");
				ShaderUtilities.ID_EnvMatrixRotation = Shader.PropertyToID("_EnvMatrixRotation");
				ShaderUtilities.ID_GlowColor = Shader.PropertyToID("_GlowColor");
				ShaderUtilities.ID_GlowOffset = Shader.PropertyToID("_GlowOffset");
				ShaderUtilities.ID_GlowPower = Shader.PropertyToID("_GlowPower");
				ShaderUtilities.ID_GlowOuter = Shader.PropertyToID("_GlowOuter");
				ShaderUtilities.ID_MaskCoord = Shader.PropertyToID("_MaskCoord");
				ShaderUtilities.ID_ClipRect = Shader.PropertyToID("_ClipRect");
				ShaderUtilities.ID_UseClipRect = Shader.PropertyToID("_UseClipRect");
				ShaderUtilities.ID_MaskSoftnessX = Shader.PropertyToID("_MaskSoftnessX");
				ShaderUtilities.ID_MaskSoftnessY = Shader.PropertyToID("_MaskSoftnessY");
				ShaderUtilities.ID_VertexOffsetX = Shader.PropertyToID("_VertexOffsetX");
				ShaderUtilities.ID_VertexOffsetY = Shader.PropertyToID("_VertexOffsetY");
				ShaderUtilities.ID_StencilID = Shader.PropertyToID("_Stencil");
				ShaderUtilities.ID_StencilOp = Shader.PropertyToID("_StencilOp");
				ShaderUtilities.ID_StencilComp = Shader.PropertyToID("_StencilComp");
				ShaderUtilities.ID_StencilReadMask = Shader.PropertyToID("_StencilReadMask");
				ShaderUtilities.ID_StencilWriteMask = Shader.PropertyToID("_StencilWriteMask");
				ShaderUtilities.ID_ShaderFlags = Shader.PropertyToID("_ShaderFlags");
				ShaderUtilities.ID_ScaleRatio_A = Shader.PropertyToID("_ScaleRatioA");
				ShaderUtilities.ID_ScaleRatio_B = Shader.PropertyToID("_ScaleRatioB");
				ShaderUtilities.ID_ScaleRatio_C = Shader.PropertyToID("_ScaleRatioC");
			}
		}

		// Token: 0x0600519C RID: 20892 RVA: 0x0029A468 File Offset: 0x00298868
		public static void UpdateShaderRatios(Material mat, bool isBold)
		{
			bool flag = !mat.shaderKeywords.Contains(ShaderUtilities.Keyword_Ratios);
			float @float = mat.GetFloat(ShaderUtilities.ID_GradientScale);
			float float2 = mat.GetFloat(ShaderUtilities.ID_FaceDilate);
			float float3 = mat.GetFloat(ShaderUtilities.ID_OutlineWidth);
			float float4 = mat.GetFloat(ShaderUtilities.ID_OutlineSoftness);
			float num = isBold ? (mat.GetFloat(ShaderUtilities.ID_WeightBold) * 2f / @float) : (mat.GetFloat(ShaderUtilities.ID_WeightNormal) * 2f / @float);
			float num2 = Mathf.Max(1f, num + float2 + float3 + float4);
			float value = (!flag) ? 1f : ((@float - ShaderUtilities.m_clamp) / (@float * num2));
			mat.SetFloat(ShaderUtilities.ID_ScaleRatio_A, value);
			if (mat.HasProperty(ShaderUtilities.ID_GlowOffset))
			{
				float float5 = mat.GetFloat(ShaderUtilities.ID_GlowOffset);
				float float6 = mat.GetFloat(ShaderUtilities.ID_GlowOuter);
				float num3 = (num + float2) * (@float - ShaderUtilities.m_clamp);
				num2 = Mathf.Max(1f, float5 + float6);
				float value2 = (!flag) ? 1f : (Mathf.Max(0f, @float - ShaderUtilities.m_clamp - num3) / (@float * num2));
				mat.SetFloat(ShaderUtilities.ID_ScaleRatio_B, value2);
			}
			if (mat.HasProperty(ShaderUtilities.ID_UnderlayOffsetX))
			{
				float float7 = mat.GetFloat(ShaderUtilities.ID_UnderlayOffsetX);
				float float8 = mat.GetFloat(ShaderUtilities.ID_UnderlayOffsetY);
				float float9 = mat.GetFloat(ShaderUtilities.ID_UnderlayDilate);
				float float10 = mat.GetFloat(ShaderUtilities.ID_UnderlaySoftness);
				float num4 = (num + float2) * (@float - ShaderUtilities.m_clamp);
				num2 = Mathf.Max(1f, Mathf.Max(Mathf.Abs(float7), Mathf.Abs(float8)) + float9 + float10);
				float value3 = (!flag) ? 1f : (Mathf.Max(0f, @float - ShaderUtilities.m_clamp - num4) / (@float * num2));
				mat.SetFloat(ShaderUtilities.ID_ScaleRatio_C, value3);
			}
		}

		// Token: 0x0600519D RID: 20893 RVA: 0x0029A67A File Offset: 0x00298A7A
		public static Vector4 GetFontExtent(Material material)
		{
			return Vector4.zero;
		}

		// Token: 0x0600519E RID: 20894 RVA: 0x0029A684 File Offset: 0x00298A84
		public static bool IsMaskingEnabled(Material material)
		{
			return !(material == null) && material.HasProperty(ShaderUtilities.ID_ClipRect) && (material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_SOFT) || material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_HARD) || material.shaderKeywords.Contains(ShaderUtilities.Keyword_MASK_TEX));
		}

		// Token: 0x0600519F RID: 20895 RVA: 0x0029A6F4 File Offset: 0x00298AF4
		public static float GetPadding(Material material, bool enableExtraPadding, bool isBold)
		{
			if (!ShaderUtilities.isInitialized)
			{
				ShaderUtilities.GetShaderPropertyIDs();
			}
			if (material == null)
			{
				return 0f;
			}
			int num = (!enableExtraPadding) ? 0 : 4;
			if (!material.HasProperty(ShaderUtilities.ID_GradientScale))
			{
				return (float)num;
			}
			Vector4 a = Vector4.zero;
			Vector4 zero = Vector4.zero;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			ShaderUtilities.UpdateShaderRatios(material, isBold);
			string[] shaderKeywords = material.shaderKeywords;
			if (material.HasProperty(ShaderUtilities.ID_ScaleRatio_A))
			{
				num5 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_A);
			}
			if (material.HasProperty(ShaderUtilities.ID_FaceDilate))
			{
				num2 = material.GetFloat(ShaderUtilities.ID_FaceDilate) * num5;
			}
			if (material.HasProperty(ShaderUtilities.ID_OutlineSoftness))
			{
				num3 = material.GetFloat(ShaderUtilities.ID_OutlineSoftness) * num5;
			}
			if (material.HasProperty(ShaderUtilities.ID_OutlineWidth))
			{
				num4 = material.GetFloat(ShaderUtilities.ID_OutlineWidth) * num5;
			}
			float num10 = num4 + num3 + num2;
			if (material.HasProperty(ShaderUtilities.ID_GlowOffset) && shaderKeywords.Contains(ShaderUtilities.Keyword_Glow))
			{
				if (material.HasProperty(ShaderUtilities.ID_ScaleRatio_B))
				{
					num6 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_B);
				}
				num8 = material.GetFloat(ShaderUtilities.ID_GlowOffset) * num6;
				num9 = material.GetFloat(ShaderUtilities.ID_GlowOuter) * num6;
			}
			num10 = Mathf.Max(num10, num2 + num8 + num9);
			if (material.HasProperty(ShaderUtilities.ID_UnderlaySoftness) && shaderKeywords.Contains(ShaderUtilities.Keyword_Underlay))
			{
				if (material.HasProperty(ShaderUtilities.ID_ScaleRatio_C))
				{
					num7 = material.GetFloat(ShaderUtilities.ID_ScaleRatio_C);
				}
				float num11 = material.GetFloat(ShaderUtilities.ID_UnderlayOffsetX) * num7;
				float num12 = material.GetFloat(ShaderUtilities.ID_UnderlayOffsetY) * num7;
				float num13 = material.GetFloat(ShaderUtilities.ID_UnderlayDilate) * num7;
				float num14 = material.GetFloat(ShaderUtilities.ID_UnderlaySoftness) * num7;
				a.x = Mathf.Max(a.x, num2 + num13 + num14 - num11);
				a.y = Mathf.Max(a.y, num2 + num13 + num14 - num12);
				a.z = Mathf.Max(a.z, num2 + num13 + num14 + num11);
				a.w = Mathf.Max(a.w, num2 + num13 + num14 + num12);
			}
			a.x = Mathf.Max(a.x, num10);
			a.y = Mathf.Max(a.y, num10);
			a.z = Mathf.Max(a.z, num10);
			a.w = Mathf.Max(a.w, num10);
			a.x += (float)num;
			a.y += (float)num;
			a.z += (float)num;
			a.w += (float)num;
			a.x = Mathf.Min(a.x, 1f);
			a.y = Mathf.Min(a.y, 1f);
			a.z = Mathf.Min(a.z, 1f);
			a.w = Mathf.Min(a.w, 1f);
			zero.x = ((zero.x >= a.x) ? zero.x : a.x);
			zero.y = ((zero.y >= a.y) ? zero.y : a.y);
			zero.z = ((zero.z >= a.z) ? zero.z : a.z);
			zero.w = ((zero.w >= a.w) ? zero.w : a.w);
			float @float = material.GetFloat(ShaderUtilities.ID_GradientScale);
			a *= @float;
			num10 = Mathf.Max(a.x, a.y);
			num10 = Mathf.Max(a.z, num10);
			num10 = Mathf.Max(a.w, num10);
			return num10 + 0.5f;
		}

		// Token: 0x060051A0 RID: 20896 RVA: 0x0029AB7C File Offset: 0x00298F7C
		public static float GetPadding(Material[] materials, bool enableExtraPadding, bool isBold)
		{
			if (!ShaderUtilities.isInitialized)
			{
				ShaderUtilities.GetShaderPropertyIDs();
			}
			if (materials == null)
			{
				return 0f;
			}
			int num = (!enableExtraPadding) ? 0 : 4;
			if (!materials[0].HasProperty(ShaderUtilities.ID_GradientScale))
			{
				return (float)num;
			}
			Vector4 a = Vector4.zero;
			Vector4 zero = Vector4.zero;
			float num2 = 0f;
			float num3 = 0f;
			float num4 = 0f;
			float num5 = 0f;
			float num6 = 0f;
			float num7 = 0f;
			float num8 = 0f;
			float num9 = 0f;
			float num10;
			for (int i = 0; i < materials.Length; i++)
			{
				ShaderUtilities.UpdateShaderRatios(materials[i], isBold);
				string[] shaderKeywords = materials[i].shaderKeywords;
				if (materials[i].HasProperty(ShaderUtilities.ID_ScaleRatio_A))
				{
					num5 = materials[i].GetFloat(ShaderUtilities.ID_ScaleRatio_A);
				}
				if (materials[i].HasProperty(ShaderUtilities.ID_FaceDilate))
				{
					num2 = materials[i].GetFloat(ShaderUtilities.ID_FaceDilate) * num5;
				}
				if (materials[i].HasProperty(ShaderUtilities.ID_OutlineSoftness))
				{
					num3 = materials[i].GetFloat(ShaderUtilities.ID_OutlineSoftness) * num5;
				}
				if (materials[i].HasProperty(ShaderUtilities.ID_OutlineWidth))
				{
					num4 = materials[i].GetFloat(ShaderUtilities.ID_OutlineWidth) * num5;
				}
				num10 = num4 + num3 + num2;
				if (materials[i].HasProperty(ShaderUtilities.ID_GlowOffset) && shaderKeywords.Contains(ShaderUtilities.Keyword_Glow))
				{
					if (materials[i].HasProperty(ShaderUtilities.ID_ScaleRatio_B))
					{
						num6 = materials[i].GetFloat(ShaderUtilities.ID_ScaleRatio_B);
					}
					num8 = materials[i].GetFloat(ShaderUtilities.ID_GlowOffset) * num6;
					num9 = materials[i].GetFloat(ShaderUtilities.ID_GlowOuter) * num6;
				}
				num10 = Mathf.Max(num10, num2 + num8 + num9);
				if (materials[i].HasProperty(ShaderUtilities.ID_UnderlaySoftness) && shaderKeywords.Contains(ShaderUtilities.Keyword_Underlay))
				{
					if (materials[i].HasProperty(ShaderUtilities.ID_ScaleRatio_C))
					{
						num7 = materials[i].GetFloat(ShaderUtilities.ID_ScaleRatio_C);
					}
					float num11 = materials[i].GetFloat(ShaderUtilities.ID_UnderlayOffsetX) * num7;
					float num12 = materials[i].GetFloat(ShaderUtilities.ID_UnderlayOffsetY) * num7;
					float num13 = materials[i].GetFloat(ShaderUtilities.ID_UnderlayDilate) * num7;
					float num14 = materials[i].GetFloat(ShaderUtilities.ID_UnderlaySoftness) * num7;
					a.x = Mathf.Max(a.x, num2 + num13 + num14 - num11);
					a.y = Mathf.Max(a.y, num2 + num13 + num14 - num12);
					a.z = Mathf.Max(a.z, num2 + num13 + num14 + num11);
					a.w = Mathf.Max(a.w, num2 + num13 + num14 + num12);
				}
				a.x = Mathf.Max(a.x, num10);
				a.y = Mathf.Max(a.y, num10);
				a.z = Mathf.Max(a.z, num10);
				a.w = Mathf.Max(a.w, num10);
				a.x += (float)num;
				a.y += (float)num;
				a.z += (float)num;
				a.w += (float)num;
				a.x = Mathf.Min(a.x, 1f);
				a.y = Mathf.Min(a.y, 1f);
				a.z = Mathf.Min(a.z, 1f);
				a.w = Mathf.Min(a.w, 1f);
				zero.x = ((zero.x >= a.x) ? zero.x : a.x);
				zero.y = ((zero.y >= a.y) ? zero.y : a.y);
				zero.z = ((zero.z >= a.z) ? zero.z : a.z);
				zero.w = ((zero.w >= a.w) ? zero.w : a.w);
			}
			float @float = materials[0].GetFloat(ShaderUtilities.ID_GradientScale);
			a *= @float;
			num10 = Mathf.Max(a.x, a.y);
			num10 = Mathf.Max(a.z, num10);
			num10 = Mathf.Max(a.w, num10);
			return num10 + 0.25f;
		}

		// Token: 0x040054D6 RID: 21718
		public static int ID_MainTex;

		// Token: 0x040054D7 RID: 21719
		public static int ID_FaceTex;

		// Token: 0x040054D8 RID: 21720
		public static int ID_FaceColor;

		// Token: 0x040054D9 RID: 21721
		public static int ID_FaceDilate;

		// Token: 0x040054DA RID: 21722
		public static int ID_Shininess;

		// Token: 0x040054DB RID: 21723
		public static int ID_UnderlayColor;

		// Token: 0x040054DC RID: 21724
		public static int ID_UnderlayOffsetX;

		// Token: 0x040054DD RID: 21725
		public static int ID_UnderlayOffsetY;

		// Token: 0x040054DE RID: 21726
		public static int ID_UnderlayDilate;

		// Token: 0x040054DF RID: 21727
		public static int ID_UnderlaySoftness;

		// Token: 0x040054E0 RID: 21728
		public static int ID_WeightNormal;

		// Token: 0x040054E1 RID: 21729
		public static int ID_WeightBold;

		// Token: 0x040054E2 RID: 21730
		public static int ID_OutlineTex;

		// Token: 0x040054E3 RID: 21731
		public static int ID_OutlineWidth;

		// Token: 0x040054E4 RID: 21732
		public static int ID_OutlineSoftness;

		// Token: 0x040054E5 RID: 21733
		public static int ID_OutlineColor;

		// Token: 0x040054E6 RID: 21734
		public static int ID_GradientScale;

		// Token: 0x040054E7 RID: 21735
		public static int ID_ScaleX;

		// Token: 0x040054E8 RID: 21736
		public static int ID_ScaleY;

		// Token: 0x040054E9 RID: 21737
		public static int ID_PerspectiveFilter;

		// Token: 0x040054EA RID: 21738
		public static int ID_TextureWidth;

		// Token: 0x040054EB RID: 21739
		public static int ID_TextureHeight;

		// Token: 0x040054EC RID: 21740
		public static int ID_BevelAmount;

		// Token: 0x040054ED RID: 21741
		public static int ID_GlowColor;

		// Token: 0x040054EE RID: 21742
		public static int ID_GlowOffset;

		// Token: 0x040054EF RID: 21743
		public static int ID_GlowPower;

		// Token: 0x040054F0 RID: 21744
		public static int ID_GlowOuter;

		// Token: 0x040054F1 RID: 21745
		public static int ID_LightAngle;

		// Token: 0x040054F2 RID: 21746
		public static int ID_EnvMap;

		// Token: 0x040054F3 RID: 21747
		public static int ID_EnvMatrix;

		// Token: 0x040054F4 RID: 21748
		public static int ID_EnvMatrixRotation;

		// Token: 0x040054F5 RID: 21749
		public static int ID_MaskCoord;

		// Token: 0x040054F6 RID: 21750
		public static int ID_ClipRect;

		// Token: 0x040054F7 RID: 21751
		public static int ID_MaskSoftnessX;

		// Token: 0x040054F8 RID: 21752
		public static int ID_MaskSoftnessY;

		// Token: 0x040054F9 RID: 21753
		public static int ID_VertexOffsetX;

		// Token: 0x040054FA RID: 21754
		public static int ID_VertexOffsetY;

		// Token: 0x040054FB RID: 21755
		public static int ID_UseClipRect;

		// Token: 0x040054FC RID: 21756
		public static int ID_StencilID;

		// Token: 0x040054FD RID: 21757
		public static int ID_StencilOp;

		// Token: 0x040054FE RID: 21758
		public static int ID_StencilComp;

		// Token: 0x040054FF RID: 21759
		public static int ID_StencilReadMask;

		// Token: 0x04005500 RID: 21760
		public static int ID_StencilWriteMask;

		// Token: 0x04005501 RID: 21761
		public static int ID_ShaderFlags;

		// Token: 0x04005502 RID: 21762
		public static int ID_ScaleRatio_A;

		// Token: 0x04005503 RID: 21763
		public static int ID_ScaleRatio_B;

		// Token: 0x04005504 RID: 21764
		public static int ID_ScaleRatio_C;

		// Token: 0x04005505 RID: 21765
		public static string Keyword_Bevel = "BEVEL_ON";

		// Token: 0x04005506 RID: 21766
		public static string Keyword_Glow = "GLOW_ON";

		// Token: 0x04005507 RID: 21767
		public static string Keyword_Underlay = "UNDERLAY_ON";

		// Token: 0x04005508 RID: 21768
		public static string Keyword_Ratios = "RATIOS_OFF";

		// Token: 0x04005509 RID: 21769
		public static string Keyword_MASK_SOFT = "MASK_SOFT";

		// Token: 0x0400550A RID: 21770
		public static string Keyword_MASK_HARD = "MASK_HARD";

		// Token: 0x0400550B RID: 21771
		public static string Keyword_MASK_TEX = "MASK_TEX";

		// Token: 0x0400550C RID: 21772
		public static string ShaderTag_ZTestMode = "unity_GUIZTestMode";

		// Token: 0x0400550D RID: 21773
		public static string ShaderTag_CullMode = "_CullMode";

		// Token: 0x0400550E RID: 21774
		private static float m_clamp = 1f;

		// Token: 0x0400550F RID: 21775
		public static bool isInitialized;
	}
}
