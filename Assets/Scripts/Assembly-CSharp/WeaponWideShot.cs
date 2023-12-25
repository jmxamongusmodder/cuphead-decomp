using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A8C RID: 2700
public class WeaponWideShot : AbstractLevelWeapon
{
	// Token: 0x1700059B RID: 1435
	// (get) Token: 0x06004090 RID: 16528 RVA: 0x00232503 File Offset: 0x00230903
	protected override bool rapidFire
	{
		get
		{
			return true;
		}
	}

	// Token: 0x1700059C RID: 1436
	// (get) Token: 0x06004091 RID: 16529 RVA: 0x00232506 File Offset: 0x00230906
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.LevelWeaponWideShot.Basic.rapidFireRate;
		}
	}

	// Token: 0x06004092 RID: 16530 RVA: 0x0023250D File Offset: 0x0023090D
	private void Start()
	{
		this.maxAngle = WeaponProperties.LevelWeaponWideShot.Basic.angleRange.max;
		base.StartCoroutine(this.angle_cr());
		this.isInitialized = true;
	}

	// Token: 0x06004093 RID: 16531 RVA: 0x00232533 File Offset: 0x00230933
	protected override void OnEnable()
	{
		base.OnEnable();
		if (this.isInitialized)
		{
			base.StartCoroutine(this.angle_cr());
		}
	}

	// Token: 0x06004094 RID: 16532 RVA: 0x00232553 File Offset: 0x00230953
	public override void BeginBasic()
	{
		base.BeginBasic();
		this.BasicSoundOneShot("player_wide_shot_start", "player_wide_shot_start_p2");
	}

	// Token: 0x06004095 RID: 16533 RVA: 0x0023256C File Offset: 0x0023096C
	protected override AbstractProjectile fireBasic()
	{
		this.BasicSoundOneShot("player_wide_shot_shoot", "player_wide_shot_shoot_p2");
		float damage = WeaponProperties.LevelWeaponWideShot.Basic.damage;
		BasicProjectile basicProjectile = null;
		MinMax minMax = new MinMax(0f, this.maxAngle);
		this.animationCycleCount++;
		int num = 0;
		while ((float)num < 3f)
		{
			float floatAt = minMax.GetFloatAt((float)num / 2f);
			float num2 = minMax.max / 2f;
			basicProjectile = ((num != 0) ? (base.fireBasicNoEffect() as BasicProjectile) : (base.fireBasic() as BasicProjectile));
			basicProjectile.Speed = WeaponProperties.LevelWeaponWideShot.Basic.speed;
			basicProjectile.DestroyDistance = WeaponProperties.LevelWeaponWideShot.Basic.distance - 20f * (float)(num + 1);
			basicProjectile.Damage = damage;
			basicProjectile.PlayerId = this.player.id;
			basicProjectile.transform.AddEulerAngles(0f, 0f, floatAt - num2);
			basicProjectile.transform.position += basicProjectile.transform.right * 50f;
			basicProjectile.animator.SetInteger("Variant", (this.animationCycleCount + num) % 3);
			num++;
		}
		return basicProjectile;
	}

	// Token: 0x06004096 RID: 16534 RVA: 0x002326A0 File Offset: 0x00230AA0
	protected override AbstractProjectile fireEx()
	{
		WeaponWideShotExProjectile weaponWideShotExProjectile = base.fireEx() as WeaponWideShotExProjectile;
		weaponWideShotExProjectile.Damage = WeaponProperties.LevelWeaponWideShot.Ex.exDamage;
		weaponWideShotExProjectile.DamageRate = 0f;
		weaponWideShotExProjectile.origin = weaponWideShotExProjectile.transform.position;
		weaponWideShotExProjectile.mainDuration = WeaponProperties.LevelWeaponWideShot.Ex.exDuration;
		weaponWideShotExProjectile.GetComponent<BoxCollider2D>().size = new Vector2(2000f, WeaponProperties.LevelWeaponWideShot.Ex.exHeight);
		weaponWideShotExProjectile.PlayerId = this.player.id;
		MeterScoreTracker meterScoreTracker = new MeterScoreTracker(MeterScoreTracker.Type.Ex);
		meterScoreTracker.Add(weaponWideShotExProjectile);
		return weaponWideShotExProjectile;
	}

	// Token: 0x06004097 RID: 16535 RVA: 0x00232728 File Offset: 0x00230B28
	private IEnumerator angle_cr()
	{
		float openTimeMax = WeaponProperties.LevelWeaponWideShot.Basic.openingAngleSpeed;
		float closeTimeMax = WeaponProperties.LevelWeaponWideShot.Basic.closingAngleSpeed;
		float t = 0f;
		float val = 0f;
		bool playerLocked = false;
		for (;;)
		{
			if (playerLocked)
			{
				if (val < 1f)
				{
					val = t / closeTimeMax;
					t += CupheadTime.Delta;
				}
				else
				{
					val = 1f;
					t = 1f;
				}
			}
			else if (val > 0f)
			{
				val = t / openTimeMax;
				t -= CupheadTime.Delta;
			}
			else
			{
				val = 0f;
				t = 0f;
			}
			playerLocked = this.player.input.actions.GetButton(6);
			this.maxAngle = WeaponProperties.LevelWeaponWideShot.Basic.angleRange.GetFloatAt(val);
			yield return null;
		}
		yield break;
	}

	// Token: 0x04004751 RID: 18257
	private float maxAngle;

	// Token: 0x04004752 RID: 18258
	private bool isInitialized;

	// Token: 0x04004753 RID: 18259
	private int animationCycleCount;
}
