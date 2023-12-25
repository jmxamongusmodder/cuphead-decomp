using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000A7D RID: 2685
public class WeaponHomingProjectile : AbstractProjectile
{
	// Token: 0x1700058D RID: 1421
	// (get) Token: 0x0600402E RID: 16430 RVA: 0x0022FF02 File Offset: 0x0022E302
	protected override float DestroyLifetime
	{
		get
		{
			return 100f;
		}
	}

	// Token: 0x0600402F RID: 16431 RVA: 0x0022FF0C File Offset: 0x0022E30C
	protected override void OnCollisionDie(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionDie(hit, phase);
		if (base.tag == "PlayerProjectile" && phase == CollisionPhase.Enter)
		{
			if (hit.GetComponent<DamageReceiver>() && hit.GetComponent<DamageReceiver>().enabled)
			{
				AudioManager.Play("player_shoot_hit_cuphead");
			}
			else
			{
				AudioManager.Play("player_weapon_homing_impact");
			}
		}
	}

	// Token: 0x06004030 RID: 16432 RVA: 0x0022FF78 File Offset: 0x0022E378
	public override AbstractProjectile Create(Vector2 position, float rotation, Vector2 scale)
	{
		WeaponHomingProjectile weaponHomingProjectile = base.Create(position, rotation, scale) as WeaponHomingProjectile;
		for (int i = 0; i < this.trailPositions.Length; i++)
		{
			weaponHomingProjectile.trailPositions[i] = position;
			weaponHomingProjectile.trailRotations[i] = base.transform.eulerAngles.z;
		}
		if (MathUtils.RandomBool())
		{
			this.trail.SetScale(new float?(-1f), null, null);
		}
		if (MathUtils.RandomBool())
		{
			this.trail.SetScale(null, new float?(-1f), null);
		}
		return weaponHomingProjectile;
	}

	// Token: 0x06004031 RID: 16433 RVA: 0x0023003D File Offset: 0x0022E43D
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x06004032 RID: 16434 RVA: 0x00230045 File Offset: 0x0022E445
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (hit.tag == "Parry")
		{
			return;
		}
		base.OnCollisionOther(hit, phase);
	}

	// Token: 0x06004033 RID: 16435 RVA: 0x00230065 File Offset: 0x0022E465
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		this.DealDamage(hit);
		if (this.isEx)
		{
			AudioManager.Play("player_ex_impact_hit");
			this.emitAudioFromObject.Add("player_ex_impact_hit");
		}
		base.OnCollisionEnemy(hit, phase);
	}

	// Token: 0x06004034 RID: 16436 RVA: 0x0023009B File Offset: 0x0022E49B
	private void DealDamage(GameObject hit)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06004035 RID: 16437 RVA: 0x002300AC File Offset: 0x0022E4AC
	protected override void Die()
	{
		this.move = false;
		AudioManager.Play("player_weapon_peashot_miss");
		EffectSpawner component = base.GetComponent<EffectSpawner>();
		if (component != null)
		{
			UnityEngine.Object.Destroy(component);
		}
		this.trail.gameObject.SetActive(false);
		base.Die();
	}

	// Token: 0x06004036 RID: 16438 RVA: 0x002300FC File Offset: 0x0022E4FC
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		WeaponHomingProjectile.State state = this.state;
		if (state != WeaponHomingProjectile.State.Homing)
		{
			if (state == WeaponHomingProjectile.State.Swirling)
			{
				this.UpdateSwirling();
			}
		}
		else
		{
			this.UpdateHoming();
		}
		this.trailFollowIndex = (this.trailFollowIndex + 1) % this.trailFollowFrames;
		this.trailRotation = this.trailRotations[this.trailFollowIndex];
		this.trail.transform.position = this.trailPositions[this.trailFollowIndex];
		this.trailRotations[this.trailFollowIndex] = this.rotation;
		this.trailPositions[this.trailFollowIndex] = base.transform.position;
	}

	// Token: 0x06004037 RID: 16439 RVA: 0x002301CC File Offset: 0x0022E5CC
	private void UpdateHoming()
	{
		if (!this.move)
		{
			return;
		}
		this.t += CupheadTime.FixedDelta;
		if (this.target != null && this.target.gameObject.activeInHierarchy && this.target.isActiveAndEnabled && this.t < WeaponProperties.LevelWeaponHoming.Basic.maxHomingTime)
		{
			float num;
			for (num = MathUtils.DirectionToAngle(this.target.bounds.center - base.transform.position); num > this.rotation + 180f; num -= 360f)
			{
			}
			while (num < this.rotation - 180f)
			{
				num += 360f;
			}
			float num2 = this.rotationSpeed.min;
			if (this.t > this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime)
			{
				num2 = this.rotationSpeed.max;
			}
			else if (this.t > this.timeBeforeEaseRotationSpeed)
			{
				num2 = this.rotationSpeed.GetFloatAt((this.t - this.timeBeforeEaseRotationSpeed) / this.rotationSpeedEaseTime);
			}
			if (Mathf.Abs(num - this.rotation) < num2 * CupheadTime.FixedDelta)
			{
				this.rotation = num;
			}
			else if (num > this.rotation)
			{
				this.rotation += num2 * CupheadTime.FixedDelta;
			}
			else
			{
				this.rotation -= num2 * CupheadTime.FixedDelta;
			}
		}
		Vector3 a = MathUtils.AngleToDirection(this.rotation);
		base.transform.position += a * this.speed * CupheadTime.FixedDelta;
		if (!CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(this.destroyPadding, this.destroyPadding)))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06004038 RID: 16440 RVA: 0x002303E8 File Offset: 0x0022E7E8
	private void UpdateSwirling()
	{
		if (!this.move)
		{
			return;
		}
		if (this.player.IsDead)
		{
			this.StopSwirling();
			return;
		}
		this.t += CupheadTime.FixedDelta;
		Vector2 a = this.swirlLaunchPos + MathUtils.AngleToDirection(this.swirlLaunchRotation) * this.t * this.speed;
		float num = 360f * this.speed / (this.swirlDistance * 2f * 3.1415927f);
		this.swirlRotation += num * CupheadTime.FixedDelta;
		Vector2 vector = this.player.center + MathUtils.AngleToDirection(this.swirlRotation) * this.swirlDistance;
		if (this.t < this.swirlEaseTime)
		{
			Vector2 b = base.transform.position;
			base.transform.position = Vector2.Lerp(a, vector, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, this.t / this.swirlEaseTime));
			this.rotation = MathUtils.DirectionToAngle(base.transform.position - b);
		}
		else
		{
			base.transform.position = vector;
			this.rotation = this.swirlRotation + 90f;
		}
	}

	// Token: 0x06004039 RID: 16441 RVA: 0x00230558 File Offset: 0x0022E958
	protected override void Update()
	{
		base.Update();
		this.timeSinceUpdateRotation += CupheadTime.Delta;
		if (this.timeSinceUpdateRotation > 0.041666668f)
		{
			this.timeSinceUpdateRotation -= 0.041666668f;
			Vector2 v = this.trail.transform.position;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.rotation + this.spriteRotation));
			this.trail.SetEulerAngles(new float?(0f), new float?(0f), new float?(this.trailRotation + this.trailSpriteRotation));
			this.trail.position = v;
		}
	}

	// Token: 0x0600403A RID: 16442 RVA: 0x0023062D File Offset: 0x0022EA2D
	public void FindTarget()
	{
		this.target = this.findBestTarget(AbstractProjectile.FindOverlapScreenDamageReceivers());
	}

	// Token: 0x0600403B RID: 16443 RVA: 0x00230640 File Offset: 0x0022EA40
	private Collider2D findBestTarget(IEnumerable<DamageReceiver> damageReceivers)
	{
		Vector2 a = base.transform.position + this.speed * (this.timeBeforeEaseRotationSpeed + this.rotationSpeedEaseTime * 0.75f) * MathUtils.AngleToDirection(this.rotation);
		float num = float.MaxValue;
		Collider2D result = null;
		foreach (DamageReceiver damageReceiver in damageReceivers)
		{
			if (damageReceiver.gameObject.activeInHierarchy && damageReceiver.enabled && damageReceiver.type == DamageReceiver.Type.Enemy)
			{
				foreach (Collider2D collider2D in damageReceiver.GetComponents<Collider2D>())
				{
					if (collider2D.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D.bounds.center, collider2D.bounds.size / 2f))
					{
						float sqrMagnitude = (a - collider2D.bounds.center).sqrMagnitude;
						if (sqrMagnitude < num)
						{
							num = sqrMagnitude;
							result = collider2D;
						}
					}
				}
				foreach (DamageReceiverChild damageReceiverChild in damageReceiver.GetComponentsInChildren<DamageReceiverChild>())
				{
					foreach (Collider2D collider2D2 in damageReceiverChild.GetComponents<Collider2D>())
					{
						if (collider2D2.isActiveAndEnabled && CupheadLevelCamera.Current.ContainsPoint(collider2D2.bounds.center, collider2D2.bounds.size / 2f))
						{
							float sqrMagnitude2 = (a - collider2D2.bounds.center).sqrMagnitude;
							if (sqrMagnitude2 < num)
							{
								num = sqrMagnitude2;
								result = collider2D2;
							}
						}
					}
				}
			}
		}
		return result;
	}

	// Token: 0x0600403C RID: 16444 RVA: 0x00230894 File Offset: 0x0022EC94
	public void StartSwirling(int index, int bulletCount, float spread, AbstractPlayerController player)
	{
		this.state = WeaponHomingProjectile.State.Swirling;
		this.swirlLaunchRotation = base.transform.eulerAngles.z + ((float)index / (float)(bulletCount - 1) - 0.5f) * spread;
		this.swirlRotation = base.transform.eulerAngles.z + ((float)index / (float)(bulletCount - 1) - 0.5f) * (((float)bulletCount - 1f) / (float)bulletCount) * 360f;
		this.swirlLaunchPos = base.transform.position;
		this.rotation = this.swirlLaunchRotation;
		base.animator.Play("A", 0, (float)index / (float)bulletCount);
		base.animator.Play("Idle", 1, (float)index / (float)bulletCount);
		this.player = player;
	}

	// Token: 0x0600403D RID: 16445 RVA: 0x00230960 File Offset: 0x0022ED60
	public void StopSwirling()
	{
		this.state = WeaponHomingProjectile.State.Homing;
		this.FindTarget();
		this.t = 0f;
	}

	// Token: 0x040046F3 RID: 18163
	[SerializeField]
	private float spriteRotation;

	// Token: 0x040046F4 RID: 18164
	[SerializeField]
	private float trailSpriteRotation;

	// Token: 0x040046F5 RID: 18165
	[SerializeField]
	private Transform trail;

	// Token: 0x040046F6 RID: 18166
	[SerializeField]
	private float destroyPadding;

	// Token: 0x040046F7 RID: 18167
	public float speed;

	// Token: 0x040046F8 RID: 18168
	public MinMax rotationSpeed;

	// Token: 0x040046F9 RID: 18169
	public float timeBeforeEaseRotationSpeed;

	// Token: 0x040046FA RID: 18170
	public float rotationSpeedEaseTime;

	// Token: 0x040046FB RID: 18171
	public float rotation;

	// Token: 0x040046FC RID: 18172
	public float swirlDistance;

	// Token: 0x040046FD RID: 18173
	public float swirlEaseTime;

	// Token: 0x040046FE RID: 18174
	public int trailFollowFrames;

	// Token: 0x040046FF RID: 18175
	private WeaponHomingProjectile.State state;

	// Token: 0x04004700 RID: 18176
	private Vector2 velocity;

	// Token: 0x04004701 RID: 18177
	private float t;

	// Token: 0x04004702 RID: 18178
	private bool move = true;

	// Token: 0x04004703 RID: 18179
	private Collider2D target;

	// Token: 0x04004704 RID: 18180
	private float swirlLaunchRotation;

	// Token: 0x04004705 RID: 18181
	private float swirlRotation;

	// Token: 0x04004706 RID: 18182
	private AbstractPlayerController player;

	// Token: 0x04004707 RID: 18183
	private Vector2 swirlLaunchPos;

	// Token: 0x04004708 RID: 18184
	private float timeSinceUpdateRotation = 0.041666668f;

	// Token: 0x04004709 RID: 18185
	private float trailRotation;

	// Token: 0x0400470A RID: 18186
	public bool isEx;

	// Token: 0x0400470B RID: 18187
	private Vector2[] trailPositions = new Vector2[10];

	// Token: 0x0400470C RID: 18188
	private float[] trailRotations = new float[10];

	// Token: 0x0400470D RID: 18189
	private int trailFollowIndex;

	// Token: 0x02000A7E RID: 2686
	public enum State
	{
		// Token: 0x0400470F RID: 18191
		Homing,
		// Token: 0x04004710 RID: 18192
		Swirling
	}
}
