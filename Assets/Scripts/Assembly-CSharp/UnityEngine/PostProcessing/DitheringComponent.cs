using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BA3 RID: 2979
	public sealed class DitheringComponent : PostProcessingComponentRenderTexture<DitheringModel>
	{
		// Token: 0x1700064E RID: 1614
		// (get) Token: 0x06004864 RID: 18532 RVA: 0x00260502 File Offset: 0x0025E902
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06004865 RID: 18533 RVA: 0x00260525 File Offset: 0x0025E925
		public override void OnDisable()
		{
			this.noiseTextures = null;
		}

		// Token: 0x06004866 RID: 18534 RVA: 0x00260530 File Offset: 0x0025E930
		private void LoadNoiseTextures()
		{
			this.noiseTextures = new Texture2D[64];
			for (int i = 0; i < 64; i++)
			{
				this.noiseTextures[i] = Resources.Load<Texture2D>("Bluenoise64/LDR_LLL1_" + i);
			}
		}

		// Token: 0x06004867 RID: 18535 RVA: 0x0026057C File Offset: 0x0025E97C
		public override void Prepare(Material uberMaterial)
		{
			if (++this.textureIndex >= 64)
			{
				this.textureIndex = 0;
			}
			float value = Random.value;
			float value2 = Random.value;
			if (this.noiseTextures == null)
			{
				this.LoadNoiseTextures();
			}
			Texture2D texture2D = this.noiseTextures[this.textureIndex];
			uberMaterial.EnableKeyword("DITHERING");
			uberMaterial.SetTexture(DitheringComponent.Uniforms._DitheringTex, texture2D);
			uberMaterial.SetVector(DitheringComponent.Uniforms._DitheringCoords, new Vector4((float)this.context.width / (float)texture2D.width, (float)this.context.height / (float)texture2D.height, value, value2));
		}

		// Token: 0x04004DD9 RID: 19929
		private Texture2D[] noiseTextures;

		// Token: 0x04004DDA RID: 19930
		private int textureIndex;

		// Token: 0x04004DDB RID: 19931
		private const int k_TextureCount = 64;

		// Token: 0x02000BA4 RID: 2980
		private static class Uniforms
		{
			// Token: 0x04004DDC RID: 19932
			internal static readonly int _DitheringTex = Shader.PropertyToID("_DitheringTex");

			// Token: 0x04004DDD RID: 19933
			internal static readonly int _DitheringCoords = Shader.PropertyToID("_DitheringCoords");
		}
	}
}
