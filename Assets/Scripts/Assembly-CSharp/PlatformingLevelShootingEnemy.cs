using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200086F RID: 2159
public class PlatformingLevelShootingEnemy : AbstractPlatformingLevelEnemy
{
	// Token: 0x06003234 RID: 12852 RVA: 0x001CF48D File Offset: 0x001CD88D
	protected override void Start()
	{
		base.Start();
		this._aim = new GameObject("Aim").transform;
		this._aim.SetParent(this._projectileRoot);
		this._aim.ResetLocalTransforms();
	}

	// Token: 0x06003235 RID: 12853 RVA: 0x001CF4C8 File Offset: 0x001CD8C8
	protected override void OnStart()
	{
		this._projectileDelay = base.Properties.ProjectileDelay.RandomFloat();
		switch (this._triggerType)
		{
		case PlatformingLevelShootingEnemy.TriggerType.Range:
			base.StartCoroutine(this.ranged_cr());
			break;
		case PlatformingLevelShootingEnemy.TriggerType.TriggerVolumes:
			base.StartCoroutine(this.triggerVolumes_cr());
			break;
		case PlatformingLevelShootingEnemy.TriggerType.OnScreen:
			base.StartCoroutine(this.onscreen_cr());
			break;
		case PlatformingLevelShootingEnemy.TriggerType.Indefinite:
			base.StartCoroutine(this.indefinite_cr());
			break;
		}
	}

	// Token: 0x06003236 RID: 12854 RVA: 0x001CF55A File Offset: 0x001CD95A
	protected virtual void StartShoot()
	{
		base.animator.SetTrigger("Shoot");
	}

	// Token: 0x06003237 RID: 12855 RVA: 0x001CF56C File Offset: 0x001CD96C
	protected virtual void Shoot()
	{
		float num = base.Properties.ProjectileAngle;
		float speed = base.Properties.ProjectileSpeed;
		if (this._target == null || this._target.IsDead)
		{
			this._target = PlayerManager.GetNext();
		}
		switch (base.Properties.ProjectileAimMode)
		{
		case EnemyProperties.AimMode.AimedAtPlayer:
			this._aim.LookAt2D(this._target.center);
			num = this._aim.transform.eulerAngles.z;
			break;
		case EnemyProperties.AimMode.ArcAimedAtPlayer:
		{
			float num2 = float.MaxValue;
			Vector2 vector = this._target.center - this._projectileRoot.position;
			vector.x = Mathf.Abs(vector.x);
			MinMax minMax = new MinMax(base.Properties.ArcProjectileMinAngle, base.Properties.ProjectileAngle);
			MinMax minMax2 = new MinMax(base.Properties.ArcProjectileMinSpeed, base.Properties.ProjectileSpeed);
			if (vector.y > 0f && this._ArcExtraSpeedUnderPlayerMultiplier > 0f)
			{
				float num3 = minMax2.max / base.Properties.ProjectileGravity;
				float num4 = minMax2.max * num3 - 0.5f * base.Properties.ProjectileGravity * num3 * num3;
				float num5 = num4 + vector.y * this._ArcExtraSpeedUnderPlayerMultiplier;
				float num6 = Mathf.Sqrt(2f * num5 / base.Properties.ProjectileGravity);
				minMax2.max = num6 * base.Properties.ProjectileGravity;
				minMax2.min *= minMax2.max / base.Properties.ProjectileSpeed;
			}
			float num7 = 0f;
			while (num7 < 1f)
			{
				float floatAt = minMax.GetFloatAt(num7);
				float floatAt2 = minMax2.GetFloatAt(num7);
				Vector2 vector2 = MathUtils.AngleToDirection(floatAt) * floatAt2;
				float num8 = vector.x / vector2.x;
				float num9 = vector2.y * num8 - 0.5f * base.Properties.ProjectileGravity * num8 * num8;
				float num10 = Mathf.Abs(vector.y - num9);
				if (base.Properties.ProjectileGravity <= 0.01f)
				{
					goto IL_292;
				}
				float num11 = vector2.y - base.Properties.ProjectileGravity * num8;
				if (num11 <= 0f)
				{
					goto IL_292;
				}
				IL_2A5:
				num7 += 0.01f;
				continue;
				IL_292:
				if (num10 < num2)
				{
					num2 = num10;
					num = floatAt;
					speed = floatAt2;
					goto IL_2A5;
				}
				goto IL_2A5;
			}
			if ((!this._hasFacingDirection && this._target.center.x < base.transform.position.x) || (this._hasFacingDirection && this._direction == PlatformingLevelShootingEnemy.Direction.Left))
			{
				num = 180f - num;
			}
			break;
		}
		case EnemyProperties.AimMode.Spread:
		{
			Vector3 v = MathUtils.AngleToDirection(base.Properties.ProjectileAngle);
			float num2 = float.MaxValue;
			Vector2 vector = v - this._projectileRoot.position;
			vector.x = Mathf.Abs(vector.x);
			MinMax minMax2 = new MinMax(base.Properties.ArcProjectileMinSpeed, base.Properties.ProjectileSpeed);
			if (vector.y > 0f)
			{
				float num12 = minMax2.max / base.Properties.ProjectileGravity;
				float num13 = minMax2.max * num12 - 0.5f * base.Properties.ProjectileGravity * num12 * num12;
				float num14 = num13 + vector.y * this._ArcExtraSpeedUnderPlayerMultiplier;
				float num15 = Mathf.Sqrt(2f * num14 / base.Properties.ProjectileGravity);
				minMax2.max = num15 * base.Properties.ProjectileGravity;
				minMax2.min *= minMax2.max / base.Properties.ProjectileSpeed;
			}
			float num16 = minMax2.RandomFloat();
			Vector2 vector3 = MathUtils.AngleToDirection(base.Properties.ProjectileAngle) * num16;
			float num17 = vector.x / vector3.x;
			float num18 = vector3.y * num17 - 0.5f * base.Properties.ProjectileGravity * num17 * num17;
			float num19 = Mathf.Abs(vector.y - num18);
			if (num19 < num2)
			{
				num = base.Properties.ProjectileAngle;
				speed = num16;
			}
			for (int i = 0; i < 2; i++)
			{
				float rotation = (i != 1) ? 90f : (180f - num);
				BasicProjectile basicProjectile = this.projectilePrefab.Create(this._projectileRoot.position, rotation, speed);
				basicProjectile.SetParryable(base.Properties.ProjectileParryable);
				basicProjectile.Gravity = base.Properties.ProjectileGravity;
			}
			break;
		}
		case EnemyProperties.AimMode.Arc:
		{
			Vector3 v = MathUtils.AngleToDirection(base.Properties.ProjectileAngle);
			float num2 = float.MaxValue;
			Vector2 vector = v - this._projectileRoot.position;
			vector.x = Mathf.Abs(vector.x);
			MinMax minMax2 = new MinMax(base.Properties.ArcProjectileMinSpeed, base.Properties.ProjectileSpeed);
			if (vector.y > 0f)
			{
				float num20 = minMax2.max / base.Properties.ProjectileGravity;
				float num21 = minMax2.max * num20 - 0.5f * base.Properties.ProjectileGravity * num20 * num20;
				float num22 = num21 + vector.y * this._ArcExtraSpeedUnderPlayerMultiplier;
				float num23 = Mathf.Sqrt(2f * num22 / base.Properties.ProjectileGravity);
				minMax2.max = num23 * base.Properties.ProjectileGravity;
				minMax2.min *= minMax2.max / base.Properties.ProjectileSpeed;
			}
			float num24 = minMax2.RandomFloat();
			Vector2 vector4 = MathUtils.AngleToDirection(base.Properties.ProjectileAngle) * num24;
			float num25 = vector.x / vector4.x;
			float num26 = vector4.y * num25 - 0.5f * base.Properties.ProjectileGravity * num25 * num25;
			float num27 = Mathf.Abs(vector.y - num26);
			if (num27 < num2)
			{
				num = base.Properties.ProjectileAngle;
				speed = num24;
			}
			break;
		}
		}
		BasicProjectile basicProjectile2 = this.projectilePrefab.Create(this._projectileRoot.position, num, speed);
		basicProjectile2.SetParryable(base.Properties.ProjectileParryable);
		basicProjectile2.SetStoneTime(base.Properties.ProjectileStoneTime);
		basicProjectile2.Gravity = base.Properties.ProjectileGravity;
		this.SpawnShootEffect();
	}

	// Token: 0x06003238 RID: 12856 RVA: 0x001CFCAF File Offset: 0x001CE0AF
	protected virtual void SpawnShootEffect()
	{
		if (this._shootEffect != null)
		{
			this._shootEffect.Create(this._effectRoot.position);
		}
	}

	// Token: 0x06003239 RID: 12857 RVA: 0x001CFCDC File Offset: 0x001CE0DC
	protected void setDirection(PlatformingLevelShootingEnemy.Direction direction)
	{
		this._direction = direction;
		base.transform.SetScale(new float?((float)((this._direction != PlatformingLevelShootingEnemy.Direction.Right) ? 1 : -1)), null, null);
	}

	// Token: 0x0600323A RID: 12858 RVA: 0x001CFD26 File Offset: 0x001CE126
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefab = null;
	}

	// Token: 0x0600323B RID: 12859 RVA: 0x001CFD38 File Offset: 0x001CE138
	private IEnumerator indefinite_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
		for (;;)
		{
			if (this._hasShootingAnimation)
			{
				this.StartShoot();
				yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
				this._target = PlayerManager.GetNext();
			}
			else
			{
				this._target = PlayerManager.GetNext();
				this.Shoot();
			}
			yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
		}
		yield break;
	}

	// Token: 0x0600323C RID: 12860 RVA: 0x001CFD54 File Offset: 0x001CE154
	private IEnumerator onscreen_cr()
	{
		for (;;)
		{
			while (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(-this.onScreenTriggerPadding, 0f)))
			{
				yield return null;
			}
			if (!this._hasFired)
			{
				yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
				this._hasFired = true;
			}
			else
			{
				if (this._hasShootingAnimation)
				{
					this.StartShoot();
					yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
					this._target = PlayerManager.GetNext();
				}
				else
				{
					this._target = PlayerManager.GetNext();
					this.Shoot();
				}
				yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
			}
		}
		yield break;
	}

	// Token: 0x0600323D RID: 12861 RVA: 0x001CFD70 File Offset: 0x001CE170
	private IEnumerator ranged_cr()
	{
		PlayerId lastPlayer = PlayerId.None;
		for (;;)
		{
			PlayerId currentPlayer = PlayerId.PlayerOne;
			bool inRange = false;
			while (!inRange)
			{
				bool cuphead = this.IsPlayerInRange(PlayerId.PlayerOne);
				bool mugman = PlayerManager.Multiplayer && this.IsPlayerInRange(PlayerId.PlayerTwo);
				if (cuphead && mugman)
				{
					currentPlayer = ((lastPlayer != PlayerId.PlayerOne) ? PlayerId.PlayerOne : PlayerId.PlayerTwo);
					inRange = true;
				}
				else if (cuphead && !mugman)
				{
					currentPlayer = PlayerId.PlayerOne;
					inRange = true;
				}
				else if (!cuphead && mugman)
				{
					currentPlayer = PlayerId.PlayerTwo;
					inRange = true;
				}
				lastPlayer = currentPlayer;
				this._target = PlayerManager.GetPlayer(currentPlayer);
				yield return null;
			}
			if (!this._hasFired)
			{
				yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
				this._hasFired = true;
			}
			else
			{
				if (this._hasShootingAnimation)
				{
					this.StartShoot();
					yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
					this._target = PlayerManager.GetPlayer(currentPlayer);
				}
				else
				{
					this._target = PlayerManager.GetPlayer(currentPlayer);
					this.Shoot();
				}
				yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
			}
		}
		yield break;
	}

	// Token: 0x0600323E RID: 12862 RVA: 0x001CFD8B File Offset: 0x001CE18B
	private bool IsPlayerInRange(PlayerId player)
	{
		return Vector2.Distance(base.transform.position, PlayerManager.GetPlayer(player).center) <= this.triggerRange;
	}

	// Token: 0x0600323F RID: 12863 RVA: 0x001CFDC0 File Offset: 0x001CE1C0
	private IEnumerator triggerVolumes_cr()
	{
		PlayerId lastPlayer = PlayerId.None;
		for (;;)
		{
			PlayerId currentPlayer = PlayerId.PlayerOne;
			bool within = false;
			while (!within)
			{
				bool cuphead = this.IsPlayerInVolumes(PlayerId.PlayerOne);
				bool mugman = PlayerManager.Multiplayer && this.IsPlayerInVolumes(PlayerId.PlayerTwo);
				if (cuphead && mugman)
				{
					currentPlayer = ((lastPlayer != PlayerId.PlayerOne) ? PlayerId.PlayerOne : PlayerId.PlayerTwo);
					within = true;
				}
				else if (cuphead && !mugman)
				{
					currentPlayer = PlayerId.PlayerOne;
					within = true;
				}
				else if (!cuphead && mugman)
				{
					currentPlayer = PlayerId.PlayerTwo;
					within = true;
				}
				lastPlayer = currentPlayer;
				yield return null;
			}
			if (!this._hasFired)
			{
				yield return CupheadTime.WaitForSeconds(this, this._initialShotDelay.RandomFloat());
				this._hasFired = true;
			}
			else
			{
				if (this._hasShootingAnimation)
				{
					this.StartShoot();
					yield return base.animator.WaitForAnimationToStart(this, "Shoot", false);
					this._target = PlayerManager.GetPlayer(currentPlayer);
				}
				else
				{
					this._target = PlayerManager.GetPlayer(currentPlayer);
					this.Shoot();
				}
				yield return CupheadTime.WaitForSeconds(this, this._projectileDelay);
			}
		}
		yield break;
	}

	// Token: 0x06003240 RID: 12864 RVA: 0x001CFDDC File Offset: 0x001CE1DC
	protected virtual bool IsPlayerInVolumes(PlayerId player)
	{
		Vector2 point = PlayerManager.GetPlayer(player).center;
		foreach (PlatformingLevelShootingEnemy.TriggerVolumeProperties triggerVolumeProperties in this._triggerVolumes)
		{
			PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape shape = triggerVolumeProperties.shape;
			if (shape != PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.BoxCollider)
			{
				if (shape == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.CircleCollider)
				{
					Vector2 position = triggerVolumeProperties.position;
					if (triggerVolumeProperties.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
					{
						position.x += base.transform.position.x;
						position.y += base.transform.position.y;
					}
					if (MathUtils.CircleContains(position, triggerVolumeProperties.circleRadius, point))
					{
						return true;
					}
				}
			}
			else
			{
				Rect rect = RectUtils.NewFromCenter(triggerVolumeProperties.position.x, triggerVolumeProperties.position.y, triggerVolumeProperties.boxSize.x, triggerVolumeProperties.boxSize.y);
				if (triggerVolumeProperties.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
				{
					rect.x += base.transform.position.x;
					rect.y += base.transform.position.y;
				}
				if (rect.Contains(point))
				{
					return true;
				}
			}
		}
		return false;
	}

	// Token: 0x06003241 RID: 12865 RVA: 0x001CFF84 File Offset: 0x001CE384
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003242 RID: 12866 RVA: 0x001CFF97 File Offset: 0x001CE397
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06003243 RID: 12867 RVA: 0x001CFFAC File Offset: 0x001CE3AC
	private void DrawGizmos(float alpha)
	{
		if (base.Properties == null)
		{
			return;
		}
		PlatformingLevelShootingEnemy.TriggerType triggerType = this._triggerType;
		if (triggerType != PlatformingLevelShootingEnemy.TriggerType.Range)
		{
			if (triggerType != PlatformingLevelShootingEnemy.TriggerType.TriggerVolumes)
			{
				if (triggerType == PlatformingLevelShootingEnemy.TriggerType.Indefinite)
				{
					this.DrawIndefiniteTriggerGizmos(alpha);
				}
			}
			else
			{
				this.DrawTriggerVolumesTriggerGizmos(alpha);
			}
		}
		else
		{
			this.DrawRangeTriggerGizmos(alpha);
		}
		EnemyProperties.AimMode projectileAimMode = base.Properties.ProjectileAimMode;
		if (projectileAimMode != EnemyProperties.AimMode.AimedAtPlayer)
		{
			if (projectileAimMode == EnemyProperties.AimMode.Straight)
			{
				this.DrawStraightAimGizmos(alpha);
			}
		}
		else
		{
			this.DrawAimedAtPlayerAimGizmos(alpha);
		}
	}

	// Token: 0x06003244 RID: 12868 RVA: 0x001D0044 File Offset: 0x001CE444
	private void DrawStraightAimGizmos(float alpha)
	{
		Color red = Color.red;
		red.a = alpha;
		Gizmos.color = red;
		Vector3 position = base.transform.position;
		Vector3 to = position + Quaternion.Euler(0f, 0f, base.Properties.ProjectileAngle) * Vector3.right * this.triggerRange;
		Vector3 to2 = position + Quaternion.Euler(0f, 0f, base.Properties.ProjectileAngle) * Vector3.right * 10000f;
		Gizmos.DrawLine(position, to);
		red.a *= 0.25f;
		Gizmos.color = red;
		Gizmos.DrawLine(position, to2);
	}

	// Token: 0x06003245 RID: 12869 RVA: 0x001D0104 File Offset: 0x001CE504
	private void DrawAimedAtPlayerAimGizmos(float alpha)
	{
		Color red = Color.red;
		red.a = alpha;
		Gizmos.color = red;
		Vector3 vector = base.transform.position + new Vector3(-100f, 100f, 0f);
		Vector3 size = Vector3.one * 40f / 2f;
		size.z = 0.001f;
		Vector3 vector2 = vector + new Vector3(-size.x / 2f, size.y / 2f, 0f);
		Vector3 to = vector2;
		to.y -= size.y * 2f;
		Gizmos.DrawWireCube(vector, size);
		Gizmos.DrawLine(vector2, to);
	}

	// Token: 0x06003246 RID: 12870 RVA: 0x001D01CC File Offset: 0x001CE5CC
	private void DrawRangeTriggerGizmos(float alpha)
	{
		Color yellow = Color.yellow;
		yellow.a = alpha;
		Gizmos.color = yellow;
		Gizmos.DrawWireSphere(base.transform.position, this.triggerRange);
	}

	// Token: 0x06003247 RID: 12871 RVA: 0x001D0204 File Offset: 0x001CE604
	private void DrawTriggerVolumesTriggerGizmos(float alpha)
	{
		Color yellow = Color.yellow;
		yellow.a = alpha;
		Gizmos.color = yellow;
		foreach (PlatformingLevelShootingEnemy.TriggerVolumeProperties triggerVolumeProperties in this._triggerVolumes)
		{
			Vector2 vector = triggerVolumeProperties.position;
			if (triggerVolumeProperties.space == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space.RelativeSpace)
			{
				vector += base.transform.position;
			}
			PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape shape = triggerVolumeProperties.shape;
			if (shape != PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.CircleCollider)
			{
				if (shape == PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape.BoxCollider)
				{
					Gizmos.DrawWireCube(vector, triggerVolumeProperties.boxSize);
				}
			}
			else
			{
				Gizmos.DrawWireSphere(vector, triggerVolumeProperties.circleRadius);
			}
		}
	}

	// Token: 0x06003248 RID: 12872 RVA: 0x001D02E4 File Offset: 0x001CE6E4
	private void DrawIndefiniteTriggerGizmos(float alpha)
	{
		Color yellow = Color.yellow;
		yellow.a = alpha;
		Gizmos.color = yellow;
		Vector3 vector = base.transform.position + new Vector3(100f, 100f, 0f);
		Vector3 vector2 = new Vector3(vector.x, vector.y + 10f, 0f);
		Vector3 vector3 = vector2;
		vector3.y -= 40f;
		Vector3 from = vector2;
		from.x -= 10f;
		Vector3 to = vector2;
		to.x += 10f;
		Vector3 from2 = vector3;
		from2.x -= 10f;
		Vector3 to2 = vector3;
		to2.x += 10f;
		Gizmos.DrawLine(vector2, vector3);
		Gizmos.DrawLine(from, to);
		Gizmos.DrawLine(from2, to2);
	}

	// Token: 0x04003A8A RID: 14986
	[Header("Trigger Properties")]
	[SerializeField]
	private PlatformingLevelShootingEnemy.TriggerType _triggerType;

	// Token: 0x04003A8B RID: 14987
	[SerializeField]
	private List<PlatformingLevelShootingEnemy.TriggerVolumeProperties> _triggerVolumes;

	// Token: 0x04003A8C RID: 14988
	[SerializeField]
	protected Effect _shootEffect;

	// Token: 0x04003A8D RID: 14989
	[SerializeField]
	protected Transform _effectRoot;

	// Token: 0x04003A8E RID: 14990
	[SerializeField]
	private Transform _projectileRoot;

	// Token: 0x04003A8F RID: 14991
	[SerializeField]
	private bool _hasShootingAnimation;

	// Token: 0x04003A90 RID: 14992
	[SerializeField]
	private MinMax _initialShotDelay;

	// Token: 0x04003A91 RID: 14993
	[SerializeField]
	private bool _hasFacingDirection;

	// Token: 0x04003A92 RID: 14994
	[SerializeField]
	private float _ArcExtraSpeedUnderPlayerMultiplier;

	// Token: 0x04003A93 RID: 14995
	[SerializeField]
	private BasicProjectile projectilePrefab;

	// Token: 0x04003A94 RID: 14996
	public float triggerRange = 1000f;

	// Token: 0x04003A95 RID: 14997
	public float onScreenTriggerPadding;

	// Token: 0x04003A96 RID: 14998
	protected AbstractPlayerController _target;

	// Token: 0x04003A97 RID: 14999
	private Transform _aim;

	// Token: 0x04003A98 RID: 15000
	private bool _hasFired;

	// Token: 0x04003A99 RID: 15001
	private float _projectileDelay;

	// Token: 0x04003A9A RID: 15002
	private PlatformingLevelShootingEnemy.Direction _direction;

	// Token: 0x04003A9B RID: 15003
	private const float GIZMO_LETTER_LENGTH = 40f;

	// Token: 0x02000870 RID: 2160
	public enum TriggerType
	{
		// Token: 0x04003A9D RID: 15005
		Range,
		// Token: 0x04003A9E RID: 15006
		TriggerVolumes,
		// Token: 0x04003A9F RID: 15007
		OnScreen,
		// Token: 0x04003AA0 RID: 15008
		Indefinite
	}

	// Token: 0x02000871 RID: 2161
	public enum Direction
	{
		// Token: 0x04003AA2 RID: 15010
		Left,
		// Token: 0x04003AA3 RID: 15011
		Right
	}

	// Token: 0x02000872 RID: 2162
	[Serializable]
	public class TriggerVolumeProperties
	{
		// Token: 0x0600324A RID: 12874 RVA: 0x001D0404 File Offset: 0x001CE804
		public Rect ToRect()
		{
			Rect result = new Rect(this.position, this.boxSize);
			result.x -= result.width / 2f;
			result.y -= result.height / 2f;
			return result;
		}

		// Token: 0x04003AA4 RID: 15012
		public PlatformingLevelShootingEnemy.TriggerVolumeProperties.Shape shape;

		// Token: 0x04003AA5 RID: 15013
		public PlatformingLevelShootingEnemy.TriggerVolumeProperties.Space space;

		// Token: 0x04003AA6 RID: 15014
		public Vector2 position = Vector2.zero;

		// Token: 0x04003AA7 RID: 15015
		public Vector2 boxSize = new Vector2(100f, 100f);

		// Token: 0x04003AA8 RID: 15016
		public float circleRadius = 100f;

		// Token: 0x02000873 RID: 2163
		public enum Shape
		{
			// Token: 0x04003AAA RID: 15018
			BoxCollider,
			// Token: 0x04003AAB RID: 15019
			CircleCollider
		}

		// Token: 0x02000874 RID: 2164
		public enum Space
		{
			// Token: 0x04003AAD RID: 15021
			RelativeSpace,
			// Token: 0x04003AAE RID: 15022
			WorldSpace
		}
	}
}
