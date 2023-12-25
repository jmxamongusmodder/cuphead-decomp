using System;
using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	// Token: 0x02000CD7 RID: 3287
	[ExecuteInEditMode]
	[RequireComponent(typeof(Camera))]
	[AddComponentMenu("Image Effects/Edge Detection/Edge Detection")]
	public class EdgeDetection : PostEffectsBase
	{
		// Token: 0x06005213 RID: 21011 RVA: 0x002A170C File Offset: 0x0029FB0C
		public override bool CheckResources()
		{
			base.CheckSupport(true);
			this.edgeDetectMaterial = base.CheckShaderAndCreateMaterial(this.edgeDetectShader, this.edgeDetectMaterial);
			if (this.mode != this.oldMode)
			{
				this.SetCameraFlag();
			}
			this.oldMode = this.mode;
			if (!this.isSupported)
			{
				base.ReportAutoDisable();
			}
			return this.isSupported;
		}

		// Token: 0x06005214 RID: 21012 RVA: 0x002A1773 File Offset: 0x0029FB73
		private new void Start()
		{
			this.oldMode = this.mode;
		}

		// Token: 0x06005215 RID: 21013 RVA: 0x002A1784 File Offset: 0x0029FB84
		private void SetCameraFlag()
		{
			if (this.mode == EdgeDetection.EdgeDetectMode.SobelDepth || this.mode == EdgeDetection.EdgeDetectMode.SobelDepthThin)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.Depth;
			}
			else if (this.mode == EdgeDetection.EdgeDetectMode.TriangleDepthNormals || this.mode == EdgeDetection.EdgeDetectMode.RobertsCrossDepthNormals)
			{
				base.GetComponent<Camera>().depthTextureMode |= DepthTextureMode.DepthNormals;
			}
		}

		// Token: 0x06005216 RID: 21014 RVA: 0x002A17EB File Offset: 0x0029FBEB
		private void OnEnable()
		{
			this.SetCameraFlag();
		}

		// Token: 0x06005217 RID: 21015 RVA: 0x002A17F4 File Offset: 0x0029FBF4
		[ImageEffectOpaque]
		private void OnRenderImage(RenderTexture source, RenderTexture destination)
		{
			if (!this.CheckResources())
			{
				Graphics.Blit(source, destination);
				return;
			}
			Vector2 vector = new Vector2(this.sensitivityDepth, this.sensitivityNormals);
			this.edgeDetectMaterial.SetVector("_Sensitivity", new Vector4(vector.x, vector.y, 1f, vector.y));
			this.edgeDetectMaterial.SetFloat("_BgFade", this.edgesOnly);
			this.edgeDetectMaterial.SetFloat("_SampleDistance", this.sampleDist);
			this.edgeDetectMaterial.SetVector("_BgColor", this.edgesOnlyBgColor);
			this.edgeDetectMaterial.SetFloat("_Exponent", this.edgeExp);
			this.edgeDetectMaterial.SetFloat("_Threshold", this.lumThreshold);
			Graphics.Blit(source, destination, this.edgeDetectMaterial, (int)this.mode);
		}

		// Token: 0x04005665 RID: 22117
		public EdgeDetection.EdgeDetectMode mode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x04005666 RID: 22118
		public float sensitivityDepth = 1f;

		// Token: 0x04005667 RID: 22119
		public float sensitivityNormals = 1f;

		// Token: 0x04005668 RID: 22120
		public float lumThreshold = 0.2f;

		// Token: 0x04005669 RID: 22121
		public float edgeExp = 1f;

		// Token: 0x0400566A RID: 22122
		public float sampleDist = 1f;

		// Token: 0x0400566B RID: 22123
		public float edgesOnly;

		// Token: 0x0400566C RID: 22124
		public Color edgesOnlyBgColor = Color.white;

		// Token: 0x0400566D RID: 22125
		public Shader edgeDetectShader;

		// Token: 0x0400566E RID: 22126
		private Material edgeDetectMaterial;

		// Token: 0x0400566F RID: 22127
		private EdgeDetection.EdgeDetectMode oldMode = EdgeDetection.EdgeDetectMode.SobelDepthThin;

		// Token: 0x02000CD8 RID: 3288
		public enum EdgeDetectMode
		{
			// Token: 0x04005671 RID: 22129
			TriangleDepthNormals,
			// Token: 0x04005672 RID: 22130
			RobertsCrossDepthNormals,
			// Token: 0x04005673 RID: 22131
			SobelDepth,
			// Token: 0x04005674 RID: 22132
			SobelDepthThin,
			// Token: 0x04005675 RID: 22133
			TriangleLuminance
		}
	}
}
