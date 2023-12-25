using System;
using System.Collections.Generic;
using UnityEngine;

namespace TMPro
{
	// Token: 0x02000C79 RID: 3193
	public class TMP_SpriteAsset : TMP_Asset
	{
		// Token: 0x17000844 RID: 2116
		// (get) Token: 0x06005002 RID: 20482 RVA: 0x002953C7 File Offset: 0x002937C7
		public static TMP_SpriteAsset defaultSpriteAsset
		{
			get
			{
				if (TMP_SpriteAsset.m_defaultSpriteAsset == null)
				{
					TMP_SpriteAsset.m_defaultSpriteAsset = Resources.Load<TMP_SpriteAsset>("Sprite Assets/Default Sprite Asset");
				}
				return TMP_SpriteAsset.m_defaultSpriteAsset;
			}
		}

		// Token: 0x06005003 RID: 20483 RVA: 0x002953ED File Offset: 0x002937ED
		private void OnEnable()
		{
		}

		// Token: 0x06005004 RID: 20484 RVA: 0x002953F0 File Offset: 0x002937F0
		private Material GetDefaultSpriteMaterial()
		{
			ShaderUtilities.GetShaderPropertyIDs();
			Shader shader = Shader.Find("TMPro/Sprite");
			Material material = new Material(shader);
			material.SetTexture(ShaderUtilities.ID_MainTex, this.spriteSheet);
			material.hideFlags = HideFlags.HideInHierarchy;
			return material;
		}

		// Token: 0x06005005 RID: 20485 RVA: 0x00295430 File Offset: 0x00293830
		public int GetSpriteIndex(int hashCode)
		{
			for (int i = 0; i < this.spriteInfoList.Count; i++)
			{
				if (this.spriteInfoList[i].hashCode == hashCode)
				{
					return i;
				}
			}
			return -1;
		}

		// Token: 0x040052B0 RID: 21168
		public static TMP_SpriteAsset m_defaultSpriteAsset;

		// Token: 0x040052B1 RID: 21169
		public Texture spriteSheet;

		// Token: 0x040052B2 RID: 21170
		public List<TMP_Sprite> spriteInfoList;

		// Token: 0x040052B3 RID: 21171
		private List<Sprite> m_sprites;
	}
}
