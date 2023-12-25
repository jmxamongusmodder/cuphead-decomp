using System;
using TMPro;
using UnityEngine;

// Token: 0x0200090E RID: 2318
public class AdjustTMPMaterial : MonoBehaviour
{
	// Token: 0x0600365A RID: 13914 RVA: 0x001F7E04 File Offset: 0x001F6204
	private void Update()
	{
		if (!this.initialSetupComplete || Localization.language != this.previousLanguage)
		{
			this.initialSetupComplete = true;
			this.previousLanguage = Localization.language;
			Localization.Languages language = Localization.language;
			Material material = this.getMaterial(language);
			if (material != null)
			{
				this.text.fontMaterial = material;
			}
		}
	}

	// Token: 0x0600365B RID: 13915 RVA: 0x001F7E64 File Offset: 0x001F6264
	private Material getMaterial(Localization.Languages language)
	{
		foreach (AdjustTMPMaterial.MaterialData materialData in this.materials)
		{
			if (materialData.language == language)
			{
				return FontLoader.GetTMPMaterial(materialData.materialName);
			}
		}
		return this.defaultMaterial;
	}

	// Token: 0x04003E4A RID: 15946
	[SerializeField]
	private TextMeshProUGUI text;

	// Token: 0x04003E4B RID: 15947
	[SerializeField]
	private Material defaultMaterial;

	// Token: 0x04003E4C RID: 15948
	[SerializeField]
	private AdjustTMPMaterial.MaterialData[] materials;

	// Token: 0x04003E4D RID: 15949
	private Localization.Languages previousLanguage;

	// Token: 0x04003E4E RID: 15950
	private bool initialSetupComplete;

	// Token: 0x0200090F RID: 2319
	[Serializable]
	public struct MaterialData
	{
		// Token: 0x04003E4F RID: 15951
		public Localization.Languages language;

		// Token: 0x04003E50 RID: 15952
		public string materialName;
	}
}
