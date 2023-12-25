using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A77 RID: 2679
public class WeaponExploderProjectile : BasicProjectile
{
	// Token: 0x06004003 RID: 16387 RVA: 0x0022EC95 File Offset: 0x0022D095
	protected override void Awake()
	{
		base.Awake();
	}

	// Token: 0x06004004 RID: 16388 RVA: 0x0022EC9D File Offset: 0x0022D09D
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (!this.isEx)
		{
			this.UpdateDamageState();
		}
	}

	// Token: 0x06004005 RID: 16389 RVA: 0x0022ECB8 File Offset: 0x0022D0B8
	private void UpdateDamageState()
	{
		if (base.lifetime < WeaponProperties.LevelWeaponExploder.Basic.timeStateTwo)
		{
			this.Damage = WeaponProperties.LevelWeaponExploder.Basic.baseDamage;
			base.transform.SetScale(new float?(1f), new float?(1f), null);
			this.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.baseExplosionRadius;
		}
		else if (base.lifetime < WeaponProperties.LevelWeaponExploder.Basic.timeStateThree)
		{
			this.Damage = WeaponProperties.LevelWeaponExploder.Basic.damageStateTwo;
			base.transform.SetScale(new float?(1.5f), new float?(1.5f), null);
			this.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.explosionRadiusStateTwo;
		}
		else
		{
			this.Damage = WeaponProperties.LevelWeaponExploder.Basic.damageStateThree;
			base.transform.SetScale(new float?(2.5f), new float?(2.5f), null);
			this.explodeRadius = WeaponProperties.LevelWeaponExploder.Basic.explosionRadiusStateThree;
		}
	}

	// Token: 0x06004006 RID: 16390 RVA: 0x0022EDAC File Offset: 0x0022D1AC
	protected override void Die()
	{
		base.Die();
		this.explosionPrefab.Create(base.transform.position, this.explodeRadius, this.Damage, base.DamageMultiplier, this.weapon, this.tracker);
		if (this.shrapnelPrefab != null)
		{
			BasicProjectile basicProjectile = this.shrapnelPrefab.Create(base.transform.position, base.transform.eulerAngles.z + 180f, WeaponProperties.LevelWeaponExploder.Ex.shrapnelSpeed);
			if (!WeaponProperties.LevelWeaponExploder.Ex.damageOn)
			{
				basicProjectile.DamagesType.Player = false;
			}
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06004007 RID: 16391 RVA: 0x0022EE64 File Offset: 0x0022D264
	public override void AddToMeterScoreTracker(MeterScoreTracker tracker)
	{
		base.AddToMeterScoreTracker(tracker);
		this.tracker = tracker;
	}

	// Token: 0x06004008 RID: 16392 RVA: 0x0022EE74 File Offset: 0x0022D274
	public void EaseSpeed()
	{
		base.StartCoroutine(this.ease_speed_cr());
	}

	// Token: 0x06004009 RID: 16393 RVA: 0x0022EE84 File Offset: 0x0022D284
	private IEnumerator ease_speed_cr()
	{
		float t = 0f;
		float time = this.easeTime;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.Speed = this.minMaxSpeed.GetFloatAt(t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x040046CE RID: 18126
	[SerializeField]
	private WeaponExploderProjectileExplosion explosionPrefab;

	// Token: 0x040046CF RID: 18127
	[SerializeField]
	private BasicProjectile shrapnelPrefab;

	// Token: 0x040046D0 RID: 18128
	[SerializeField]
	private bool isEx;

	// Token: 0x040046D1 RID: 18129
	public float explodeRadius;

	// Token: 0x040046D2 RID: 18130
	public float easeTime;

	// Token: 0x040046D3 RID: 18131
	public MinMax minMaxSpeed;

	// Token: 0x040046D4 RID: 18132
	public WeaponExploder weapon;

	// Token: 0x040046D5 RID: 18133
	private new MeterScoreTracker tracker;
}
