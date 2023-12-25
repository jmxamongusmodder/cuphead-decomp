using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BFE RID: 3070
	public class PostProcessingProfile : ScriptableObject
	{
		// Token: 0x04004F65 RID: 20325
		public BuiltinDebugViewsModel debugViews = new BuiltinDebugViewsModel();

		// Token: 0x04004F66 RID: 20326
		public FogModel fog = new FogModel();

		// Token: 0x04004F67 RID: 20327
		public AntialiasingModel antialiasing = new AntialiasingModel();

		// Token: 0x04004F68 RID: 20328
		public AmbientOcclusionModel ambientOcclusion = new AmbientOcclusionModel();

		// Token: 0x04004F69 RID: 20329
		public ScreenSpaceReflectionModel screenSpaceReflection = new ScreenSpaceReflectionModel();

		// Token: 0x04004F6A RID: 20330
		public DepthOfFieldModel depthOfField = new DepthOfFieldModel();

		// Token: 0x04004F6B RID: 20331
		public MotionBlurModel motionBlur = new MotionBlurModel();

		// Token: 0x04004F6C RID: 20332
		public EyeAdaptationModel eyeAdaptation = new EyeAdaptationModel();

		// Token: 0x04004F6D RID: 20333
		public BloomModel bloom = new BloomModel();

		// Token: 0x04004F6E RID: 20334
		public ColorGradingModel colorGrading = new ColorGradingModel();

		// Token: 0x04004F6F RID: 20335
		public UserLutModel userLut = new UserLutModel();

		// Token: 0x04004F70 RID: 20336
		public ChromaticAberrationModel chromaticAberration = new ChromaticAberrationModel();

		// Token: 0x04004F71 RID: 20337
		public GrainModel grain = new GrainModel();

		// Token: 0x04004F72 RID: 20338
		public VignetteModel vignette = new VignetteModel();

		// Token: 0x04004F73 RID: 20339
		public DitheringModel dithering = new DitheringModel();
	}
}
