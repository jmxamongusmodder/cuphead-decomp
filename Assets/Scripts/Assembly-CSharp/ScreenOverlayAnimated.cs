using UnityStandardAssets.ImageEffects;
using UnityEngine;

public class ScreenOverlayAnimated : PostEffectsBase
{
	public enum OverlayBlendMode
	{
		Additive = 0,
		ScreenBlend = 1,
		Multiply = 2,
		Overlay = 3,
		AlphaBlend = 4,
	}

	public OverlayBlendMode blendMode;
	public float intensity;
	public bool animated;
	public Texture2D[] textures;
	public Shader overlayShader;
}
