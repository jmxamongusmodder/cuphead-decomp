using UnityStandardAssets.ImageEffects;
using UnityEngine;

public class BlurGamma : PostEffectsBase
{
	public enum Filter
	{
		None = 0,
		TwoStrip = 1,
		BW = 2,
		Chalice = 3,
	}

	public float blurSize;
	public int blurIterations;
	public Shader blurShader;
}
