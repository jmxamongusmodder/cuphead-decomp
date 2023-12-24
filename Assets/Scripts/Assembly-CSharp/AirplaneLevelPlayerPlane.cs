using UnityEngine;

public class AirplaneLevelPlayerPlane : LevelProperties.Airplane.Entity
{
	[SerializeField]
	private float pitchIncreaseFactor;
	[SerializeField]
	private float pitchIncreaseFactorHighSpeed;
	[SerializeField]
	private MinMax volume;
	[SerializeField]
	private MinMax volumeHighSpeed;
	[SerializeField]
	private float highSpeedVolumeIncreaseRate;
	[SerializeField]
	private float highSpeedVolumeDecreaseRate;
	[SerializeField]
	private float volumeHighSpeedSpeedFloor;
	[SerializeField]
	private Transform edgeLeft;
	[SerializeField]
	private Transform edgeRight;
	[SerializeField]
	private Transform airplane1;
	[SerializeField]
	private Transform tiltable;
	[SerializeField]
	private Transform[] planeParts;
	[SerializeField]
	private float[] planePartAngleRanges;
	[SerializeField]
	private Vector2[] planePartPosOffsets;
	[SerializeField]
	private Effect planePuffFX;
	[SerializeField]
	private Transform[] planePuffPos;
	public bool autoX;
	public bool autoY;
	public Vector3 autoDest;
}
