using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BB8 RID: 3000
	public sealed class UserLutComponent : PostProcessingComponentRenderTexture<UserLutModel>
	{
		// Token: 0x17000659 RID: 1625
		// (get) Token: 0x060048B4 RID: 18612 RVA: 0x002630C4 File Offset: 0x002614C4
		public override bool active
		{
			get
			{
				UserLutModel.Settings settings = base.model.settings;
				return base.model.enabled && settings.lut != null && settings.contribution > 0f && settings.lut.height == (int)Mathf.Sqrt((float)settings.lut.width) && !this.context.interrupted;
			}
		}

		// Token: 0x060048B5 RID: 18613 RVA: 0x00263148 File Offset: 0x00261548
		public override void Prepare(Material uberMaterial)
		{
			UserLutModel.Settings settings = base.model.settings;
			uberMaterial.EnableKeyword("USER_LUT");
			uberMaterial.SetTexture(UserLutComponent.Uniforms._UserLut, settings.lut);
			uberMaterial.SetVector(UserLutComponent.Uniforms._UserLut_Params, new Vector4(1f / (float)settings.lut.width, 1f / (float)settings.lut.height, (float)settings.lut.height - 1f, settings.contribution));
		}

		// Token: 0x060048B6 RID: 18614 RVA: 0x002631D0 File Offset: 0x002615D0
		public void OnGUI()
		{
			UserLutModel.Settings settings = base.model.settings;
			Rect position = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)settings.lut.width, (float)settings.lut.height);
			GUI.DrawTexture(position, settings.lut);
		}

		// Token: 0x02000BB9 RID: 3001
		private static class Uniforms
		{
			// Token: 0x04004E6B RID: 20075
			internal static readonly int _UserLut = Shader.PropertyToID("_UserLut");

			// Token: 0x04004E6C RID: 20076
			internal static readonly int _UserLut_Params = Shader.PropertyToID("_UserLut_Params");
		}
	}
}
