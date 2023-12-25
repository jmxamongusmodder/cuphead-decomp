using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace TMPro
{
	// Token: 0x02000C73 RID: 3187
	public static class TMP_MaterialManager
	{
		// Token: 0x06004FDC RID: 20444 RVA: 0x00294B08 File Offset: 0x00292F08
		public static Material GetStencilMaterial(Material baseMaterial, int stencilID)
		{
			if (!baseMaterial.HasProperty(ShaderUtilities.ID_StencilID))
			{
				return baseMaterial;
			}
			int instanceID = baseMaterial.GetInstanceID();
			for (int i = 0; i < TMP_MaterialManager.m_materialList.Count; i++)
			{
				if (TMP_MaterialManager.m_materialList[i].baseMaterial.GetInstanceID() == instanceID && TMP_MaterialManager.m_materialList[i].stencilID == stencilID)
				{
					TMP_MaterialManager.m_materialList[i].count++;
					return TMP_MaterialManager.m_materialList[i].stencilMaterial;
				}
			}
			Material material = new Material(baseMaterial);
			material.hideFlags = HideFlags.HideAndDontSave;
			Material material2 = material;
			material2.name = material2.name + " Masking ID:" + stencilID;
			material.shaderKeywords = baseMaterial.shaderKeywords;
			ShaderUtilities.GetShaderPropertyIDs();
			material.SetFloat(ShaderUtilities.ID_StencilID, (float)stencilID);
			material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
			TMP_MaterialManager.MaskingMaterial maskingMaterial = new TMP_MaterialManager.MaskingMaterial();
			maskingMaterial.baseMaterial = baseMaterial;
			maskingMaterial.stencilMaterial = material;
			maskingMaterial.stencilID = stencilID;
			maskingMaterial.count = 1;
			TMP_MaterialManager.m_materialList.Add(maskingMaterial);
			return material;
		}

		// Token: 0x06004FDD RID: 20445 RVA: 0x00294C2C File Offset: 0x0029302C
		public static void ReleaseStencilMaterial(Material stencilMaterial)
		{
			int instanceID = stencilMaterial.GetInstanceID();
			for (int i = 0; i < TMP_MaterialManager.m_materialList.Count; i++)
			{
				if (TMP_MaterialManager.m_materialList[i].stencilMaterial.GetInstanceID() == instanceID)
				{
					if (TMP_MaterialManager.m_materialList[i].count > 1)
					{
						TMP_MaterialManager.m_materialList[i].count--;
					}
					else
					{
						UnityEngine.Object.DestroyImmediate(TMP_MaterialManager.m_materialList[i].stencilMaterial);
						TMP_MaterialManager.m_materialList.RemoveAt(i);
						stencilMaterial = null;
					}
					break;
				}
			}
		}

		// Token: 0x06004FDE RID: 20446 RVA: 0x00294CD4 File Offset: 0x002930D4
		public static Material GetBaseMaterial(Material stencilMaterial)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num == -1)
			{
				return null;
			}
			return TMP_MaterialManager.m_materialList[num].baseMaterial;
		}

		// Token: 0x06004FDF RID: 20447 RVA: 0x00294D1E File Offset: 0x0029311E
		public static Material SetStencil(Material material, int stencilID)
		{
			material.SetFloat(ShaderUtilities.ID_StencilID, (float)stencilID);
			if (stencilID == 0)
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 8f);
			}
			else
			{
				material.SetFloat(ShaderUtilities.ID_StencilComp, 4f);
			}
			return material;
		}

		// Token: 0x06004FE0 RID: 20448 RVA: 0x00294D5C File Offset: 0x0029315C
		public static void AddMaskingMaterial(Material baseMaterial, Material stencilMaterial, int stencilID)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num == -1)
			{
				TMP_MaterialManager.MaskingMaterial maskingMaterial = new TMP_MaterialManager.MaskingMaterial();
				maskingMaterial.baseMaterial = baseMaterial;
				maskingMaterial.stencilMaterial = stencilMaterial;
				maskingMaterial.stencilID = stencilID;
				maskingMaterial.count = 1;
				TMP_MaterialManager.m_materialList.Add(maskingMaterial);
			}
			else
			{
				stencilMaterial = TMP_MaterialManager.m_materialList[num].stencilMaterial;
				TMP_MaterialManager.m_materialList[num].count++;
			}
		}

		// Token: 0x06004FE1 RID: 20449 RVA: 0x00294DFC File Offset: 0x002931FC
		public static void RemoveStencilMaterial(Material stencilMaterial)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.stencilMaterial == stencilMaterial);
			if (num != -1)
			{
				TMP_MaterialManager.m_materialList.RemoveAt(num);
			}
		}

		// Token: 0x06004FE2 RID: 20450 RVA: 0x00294E40 File Offset: 0x00293240
		public static void ReleaseBaseMaterial(Material baseMaterial)
		{
			int num = TMP_MaterialManager.m_materialList.FindIndex((TMP_MaterialManager.MaskingMaterial item) => item.baseMaterial == baseMaterial);
			if (num != -1)
			{
				if (TMP_MaterialManager.m_materialList[num].count > 1)
				{
					TMP_MaterialManager.m_materialList[num].count--;
				}
				else
				{
					UnityEngine.Object.DestroyImmediate(TMP_MaterialManager.m_materialList[num].stencilMaterial);
					TMP_MaterialManager.m_materialList.RemoveAt(num);
				}
			}
		}

		// Token: 0x06004FE3 RID: 20451 RVA: 0x00294ED0 File Offset: 0x002932D0
		public static void ClearMaterials()
		{
			if (TMP_MaterialManager.m_materialList.Count<TMP_MaterialManager.MaskingMaterial>() == 0)
			{
				return;
			}
			for (int i = 0; i < TMP_MaterialManager.m_materialList.Count<TMP_MaterialManager.MaskingMaterial>(); i++)
			{
				Material stencilMaterial = TMP_MaterialManager.m_materialList[i].stencilMaterial;
				UnityEngine.Object.DestroyImmediate(stencilMaterial);
				TMP_MaterialManager.m_materialList.RemoveAt(i);
			}
		}

		// Token: 0x06004FE4 RID: 20452 RVA: 0x00294F2C File Offset: 0x0029332C
		public static int GetStencilID(GameObject obj)
		{
			int num = 0;
			List<Mask> list = TMP_ListPool<Mask>.Get();
			obj.GetComponentsInParent<Mask>(false, list);
			for (int i = 0; i < list.Count; i++)
			{
				if (list[i].MaskEnabled())
				{
					num++;
				}
			}
			TMP_ListPool<Mask>.Release(list);
			return Mathf.Min((1 << num) - 1, 255);
		}

		// Token: 0x06004FE5 RID: 20453 RVA: 0x00294F90 File Offset: 0x00293390
		public static Material GetFallbackMaterial(Material source, Texture mainTex)
		{
			int instanceID = source.GetInstanceID();
			int instanceID2 = mainTex.GetInstanceID();
			for (int i = 0; i < TMP_MaterialManager.m_fallbackMaterialList.Count; i++)
			{
				if (TMP_MaterialManager.m_fallbackMaterialList[i].fallbackMaterial == null)
				{
					TMP_MaterialManager.m_fallbackMaterialList.RemoveAt(i);
				}
				else if (TMP_MaterialManager.m_fallbackMaterialList[i].baseMaterial.GetInstanceID() == instanceID && TMP_MaterialManager.m_fallbackMaterialList[i].fallbackMaterial.mainTexture.GetInstanceID() == instanceID2)
				{
					TMP_MaterialManager.m_fallbackMaterialList[i].count++;
					return TMP_MaterialManager.m_fallbackMaterialList[i].fallbackMaterial;
				}
			}
			Material material = new Material(source);
			Material material2 = material;
			material2.name += " (Fallback Instance)";
			material.mainTexture = mainTex;
			TMP_MaterialManager.FallbackMaterial fallbackMaterial = new TMP_MaterialManager.FallbackMaterial();
			fallbackMaterial.baseID = instanceID;
			fallbackMaterial.baseMaterial = source;
			fallbackMaterial.fallbackMaterial = material;
			fallbackMaterial.count = 1;
			TMP_MaterialManager.m_fallbackMaterialList.Add(fallbackMaterial);
			return material;
		}

		// Token: 0x04005293 RID: 21139
		private static List<TMP_MaterialManager.MaskingMaterial> m_materialList = new List<TMP_MaterialManager.MaskingMaterial>();

		// Token: 0x04005294 RID: 21140
		private static List<TMP_MaterialManager.FallbackMaterial> m_fallbackMaterialList = new List<TMP_MaterialManager.FallbackMaterial>();

		// Token: 0x02000C74 RID: 3188
		private class FallbackMaterial
		{
			// Token: 0x04005295 RID: 21141
			public int baseID;

			// Token: 0x04005296 RID: 21142
			public Material baseMaterial;

			// Token: 0x04005297 RID: 21143
			public Material fallbackMaterial;

			// Token: 0x04005298 RID: 21144
			public int count;
		}

		// Token: 0x02000C75 RID: 3189
		private class MaskingMaterial
		{
			// Token: 0x04005299 RID: 21145
			public Material baseMaterial;

			// Token: 0x0400529A RID: 21146
			public Material stencilMaterial;

			// Token: 0x0400529B RID: 21147
			public int count;

			// Token: 0x0400529C RID: 21148
			public int stencilID;
		}
	}
}
