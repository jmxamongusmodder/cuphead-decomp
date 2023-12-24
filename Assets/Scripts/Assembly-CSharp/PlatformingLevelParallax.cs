using UnityEngine;

public class PlatformingLevelParallax : AbstractPausableComponent
{
	public enum Sides
	{
		Background = 0,
		Foreground = 1,
	}

	[SerializeField]
	private PlatformingLevel.Theme _theme;
	[SerializeField]
	private Color _color;
	[SerializeField]
	private Sides _side;
	[SerializeField]
	private int _layer;
	[SerializeField]
	private int _sortingOrderOffset;
	public Vector3 basePos;
	public Vector3 lastPos;
	public bool overrideLayerYSpeed;
	public float overrideYSpeed;
}
