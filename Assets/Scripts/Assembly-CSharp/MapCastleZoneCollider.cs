using UnityEngine;

public class MapCastleZoneCollider : AbstractCollidableObject
{
	[SerializeField]
	public MapCastleZones.Zone zone;
	[SerializeField]
	public Transform interactionPoint;
	[SerializeField]
	public bool enableLadderShadow;
	[SerializeField]
	public AbstractMapInteractiveEntity.PositionProperties returnPositions;
}
