using UnityEngine;

public class DLCCutsceneParallaxLayer : AbstractPausableComponent
{
	public enum Type
	{
		MinMax = 0,
		Comparative = 1,
		Centered = 2,
	}

	public Type type;
	public float percentage;
	public Vector2 bottomLeft;
	public Vector2 topRight;
	[SerializeField]
	private bool overrideCameraRange;
	[SerializeField]
	private MinMax overrideCameraX;
	[SerializeField]
	private MinMax overrideCameraY;
}
