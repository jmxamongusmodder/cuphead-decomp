using UnityEngine;

public class MapZoomOut : AbstractCollidableObject
{
	[SerializeField]
	private CupheadMapCamera _camera;
	[SerializeField]
	private float _maxZoomOut;
	[SerializeField]
	private float ZoomSharpness;
}
