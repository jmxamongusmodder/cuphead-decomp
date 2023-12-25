using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C67 RID: 3175
	public struct MaterialReference
	{
		// Token: 0x06004F13 RID: 20243 RVA: 0x0027B628 File Offset: 0x00279A28
		public MaterialReference(int index, TMP_FontAsset fontAsset, TMP_SpriteAsset spriteAsset, Material material, float padding)
		{
			this.index = index;
			this.fontAsset = fontAsset;
			this.spriteAsset = spriteAsset;
			this.material = material;
			this.isDefaultMaterial = (material.GetInstanceID() == fontAsset.material.GetInstanceID());
			this.isFallbackFont = false;
			this.padding = padding;
			this.referenceCount = 0;
		}

		// Token: 0x06004F14 RID: 20244 RVA: 0x0027B68C File Offset: 0x00279A8C
		public static bool Contains(MaterialReference[] materialReferences, TMP_FontAsset fontAsset)
		{
			int instanceID = fontAsset.GetInstanceID();
			int num = 0;
			while (num < materialReferences.Length && materialReferences[num].fontAsset != null)
			{
				if (materialReferences[num].fontAsset.GetInstanceID() == instanceID)
				{
					return true;
				}
				num++;
			}
			return false;
		}

		// Token: 0x06004F15 RID: 20245 RVA: 0x0027B6E8 File Offset: 0x00279AE8
		public static int AddMaterialReference(Material material, TMP_FontAsset fontAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int num = 0;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out num))
			{
				return num;
			}
			num = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = num;
			materialReferences[num].index = num;
			materialReferences[num].fontAsset = fontAsset;
			materialReferences[num].spriteAsset = null;
			materialReferences[num].material = material;
			materialReferences[num].isDefaultMaterial = (instanceID == fontAsset.material.GetInstanceID());
			materialReferences[num].referenceCount = 0;
			return num;
		}

		// Token: 0x06004F16 RID: 20246 RVA: 0x0027B784 File Offset: 0x00279B84
		public static int AddMaterialReference(Material material, TMP_SpriteAsset spriteAsset, MaterialReference[] materialReferences, Dictionary<int, int> materialReferenceIndexLookup)
		{
			int instanceID = material.GetInstanceID();
			int num = 0;
			if (materialReferenceIndexLookup.TryGetValue(instanceID, out num))
			{
				return num;
			}
			num = materialReferenceIndexLookup.Count;
			materialReferenceIndexLookup[instanceID] = num;
			materialReferences[num].index = num;
			materialReferences[num].fontAsset = materialReferences[0].fontAsset;
			materialReferences[num].spriteAsset = spriteAsset;
			materialReferences[num].material = material;
			materialReferences[num].isDefaultMaterial = true;
			materialReferences[num].referenceCount = 0;
			return num;
		}

		// Token: 0x04005215 RID: 21013
		public int index;

		// Token: 0x04005216 RID: 21014
		public TMP_FontAsset fontAsset;

		// Token: 0x04005217 RID: 21015
		public TMP_SpriteAsset spriteAsset;

		// Token: 0x04005218 RID: 21016
		public Material material;

		// Token: 0x04005219 RID: 21017
		public bool isDefaultMaterial;

		// Token: 0x0400521A RID: 21018
		public bool isFallbackFont;

		// Token: 0x0400521B RID: 21019
		public float padding;

		// Token: 0x0400521C RID: 21020
		public int referenceCount;
	}
}
