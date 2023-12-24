using UnityStandardAssets.ImageEffects;
using UnityEngine;

public class ChromaticAberrationFilmGrain : PostEffectsBase
{
	public Shader shader;
	public float intensity;
	public bool animated;
	public int earlyLoopPoint;
	public Vector2 r;
	public Vector2 g;
	public Vector2 b;
}
