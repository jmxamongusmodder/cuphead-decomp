using System;
using UnityEngine;
using System.Collections.Generic;

public class PlatformingLevelShootingEnemy : AbstractPlatformingLevelEnemy
{
	[Serializable]
	public class TriggerVolumeProperties
	{
		public enum Shape
		{
			BoxCollider = 0,
			CircleCollider = 1,
		}

		public enum Space
		{
			RelativeSpace = 0,
			WorldSpace = 1,
		}

		public Shape shape;
		public Space space;
		public Vector2 position;
		public Vector2 boxSize;
		public float circleRadius;
	}

	public enum TriggerType
	{
		Range = 0,
		TriggerVolumes = 1,
		OnScreen = 2,
		Indefinite = 3,
	}

	[SerializeField]
	private TriggerType _triggerType;
	[SerializeField]
	private List<PlatformingLevelShootingEnemy.TriggerVolumeProperties> _triggerVolumes;
	[SerializeField]
	protected Effect _shootEffect;
	[SerializeField]
	protected Transform _effectRoot;
	[SerializeField]
	private Transform _projectileRoot;
	[SerializeField]
	private bool _hasShootingAnimation;
	[SerializeField]
	private MinMax _initialShotDelay;
	[SerializeField]
	private bool _hasFacingDirection;
	[SerializeField]
	private float _ArcExtraSpeedUnderPlayerMultiplier;
	[SerializeField]
	private BasicProjectile projectilePrefab;
	public float triggerRange;
	public float onScreenTriggerPadding;
}
