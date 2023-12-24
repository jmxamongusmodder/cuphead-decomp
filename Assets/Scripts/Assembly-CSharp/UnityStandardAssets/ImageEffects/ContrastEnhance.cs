using UnityEngine;

namespace UnityStandardAssets.ImageEffects
{
	internal class ContrastEnhance : PostEffectsBase
	{
		public float intensity;
		public float threshold;
		public float blurSpread;
		public Shader separableBlurShader;
		public Shader contrastCompositeShader;
	}
}
