using System;
using UnityEngine;

// Token: 0x02000635 RID: 1589
public class FlyingBlimpLevelEnemyDeathPart : AbstractProjectile
{
	// Token: 0x17000385 RID: 901
	// (get) Token: 0x06002091 RID: 8337 RVA: 0x0012C69D File Offset: 0x0012AA9D
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0.25f;
		}
	}

	// Token: 0x06002092 RID: 8338 RVA: 0x0012C6A4 File Offset: 0x0012AAA4
	public FlyingBlimpLevelEnemyDeathPart CreatePart(Vector3 position, LevelProperties.FlyingBlimp.Gear properties)
	{
		FlyingBlimpLevelEnemyDeathPart flyingBlimpLevelEnemyDeathPart = this.InstantiatePrefab<FlyingBlimpLevelEnemyDeathPart>();
		flyingBlimpLevelEnemyDeathPart.transform.position = position;
		flyingBlimpLevelEnemyDeathPart.properties = properties;
		return flyingBlimpLevelEnemyDeathPart;
	}

	// Token: 0x06002093 RID: 8339 RVA: 0x0012C6CC File Offset: 0x0012AACC
	protected override void Start()
	{
		base.Start();
		if (!this.gear)
		{
			this.velocity = new Vector2(UnityEngine.Random.Range(-500f, 500f), UnityEngine.Random.Range(500f, 1200f));
		}
		else
		{
			this.velocity = new Vector2(-500f, this.properties.bounceHeight);
		}
	}

	// Token: 0x06002094 RID: 8340 RVA: 0x0012C734 File Offset: 0x0012AB34
	protected override void FixedUpdate()
	{
		base.Update();
		base.transform.position += (this.velocity + new Vector2(-this.properties.bounceSpeed, this.accumulatedGravity)) * Time.fixedDeltaTime;
		this.accumulatedGravity += -100f;
		if (base.transform.position.y < -360f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x06002095 RID: 8341 RVA: 0x0012C7C8 File Offset: 0x0012ABC8
	public override void OnParry(AbstractPlayerController player)
	{
		if (!this.getNewWeapon)
		{
			if (this.parryCounter < (float)this.properties.parryCount)
			{
				this.parryCounter += 1f;
				this.accumulatedGravity = 0f;
			}
			else
			{
				base.GetComponent<SpriteRenderer>().color = ColorUtils.HexToColor("FF00EDFF");
				base.FrameDelayedCallback(new Action(this.SetWeapon), 5);
				this.accumulatedGravity = 0f;
			}
		}
		else
		{
			this.parriedIt = true;
			this.Die();
		}
	}

	// Token: 0x06002096 RID: 8342 RVA: 0x0012C860 File Offset: 0x0012AC60
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.getNewWeapon && !this.parriedIt)
		{
			AbstractPlayerController next = PlayerManager.GetNext();
			PlanePlayerController planePlayerController = next as PlanePlayerController;
			planePlayerController.weaponManager.SwitchToWeapon(Weapon.plane_weapon_laser);
			this.Die();
		}
	}

	// Token: 0x06002097 RID: 8343 RVA: 0x0012C8AE File Offset: 0x0012ACAE
	private void SetWeapon()
	{
		this.getNewWeapon = true;
	}

	// Token: 0x06002098 RID: 8344 RVA: 0x0012C8B7 File Offset: 0x0012ACB7
	protected override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x04002913 RID: 10515
	[SerializeField]
	private bool gear;

	// Token: 0x04002914 RID: 10516
	private LevelProperties.FlyingBlimp.Gear properties;

	// Token: 0x04002915 RID: 10517
	private const float VELOCITY_X_MIN = -500f;

	// Token: 0x04002916 RID: 10518
	private const float VELOCITY_X_MAX = 500f;

	// Token: 0x04002917 RID: 10519
	private const float VELOCITY_Y_MIN = 500f;

	// Token: 0x04002918 RID: 10520
	private const float VELOCITY_Y_MAX = 1200f;

	// Token: 0x04002919 RID: 10521
	private const float GRAVITY = -100f;

	// Token: 0x0400291A RID: 10522
	private Vector2 velocity;

	// Token: 0x0400291B RID: 10523
	private float accumulatedGravity;

	// Token: 0x0400291C RID: 10524
	private float parryCounter;

	// Token: 0x0400291D RID: 10525
	private bool getNewWeapon;

	// Token: 0x0400291E RID: 10526
	private bool parriedIt;
}
