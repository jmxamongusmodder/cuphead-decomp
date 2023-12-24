using UnityEngine;

public class SnowCultLevelTable : AbstractPausableComponent
{
	[SerializeField]
	private SnowCultLevelWizard wiz;
	[SerializeField]
	private SpriteRenderer rend;
	[SerializeField]
	private float accel;
	[SerializeField]
	private float decelOnDeactivate;
	[SerializeField]
	private float maxVel;
	[SerializeField]
	private float maxDistance;
}
