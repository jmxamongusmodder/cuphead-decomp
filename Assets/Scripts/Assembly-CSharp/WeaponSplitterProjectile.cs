using System;
using UnityEngine;

// Token: 0x02000A84 RID: 2692
public class WeaponSplitterProjectile : BasicProjectile
{
	// Token: 0x0600405A RID: 16474 RVA: 0x00231212 File Offset: 0x0022F612
	protected override void Start()
	{
		base.Start();
		if (this.splitDamage > -1f)
		{
			this.damageDealer.SetDamage(this.splitDamage);
		}
	}

	// Token: 0x0600405B RID: 16475 RVA: 0x0023123B File Offset: 0x0022F63B
	protected override void OnDieDistance()
	{
		if (base.dead)
		{
			return;
		}
		this.Die();
		base.animator.SetTrigger("OnDistanceDie");
	}

	// Token: 0x0600405C RID: 16476 RVA: 0x0023125F File Offset: 0x0022F65F
	protected override void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600405D RID: 16477 RVA: 0x0023126C File Offset: 0x0022F66C
	private void _OnDieAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0600405E RID: 16478 RVA: 0x0023127C File Offset: 0x0022F67C
	private void Split()
	{
		this.baseAngle = base.transform.eulerAngles.z;
		if (this.isMain)
		{
			this.damageDealer.SetDamage((this.nextDistance != WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB) ? WeaponProperties.LevelWeaponSplitter.Basic.bulletDamageA : WeaponProperties.LevelWeaponSplitter.Basic.bulletDamageB);
			WeaponSplitterProjectile weaponSplitterProjectile = UnityEngine.Object.Instantiate<WeaponSplitterProjectile>(this, base.transform.position, Quaternion.identity);
			weaponSplitterProjectile.isMain = false;
			weaponSplitterProjectile.splitAngle = -WeaponProperties.LevelWeaponSplitter.Basic.splitAngle;
			weaponSplitterProjectile.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle + this.splitAngle);
			weaponSplitterProjectile.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance;
			weaponSplitterProjectile.dist = this.dist;
			weaponSplitterProjectile.splitDamage = this.Damage;
			weaponSplitterProjectile = UnityEngine.Object.Instantiate<WeaponSplitterProjectile>(this, base.transform.position, Quaternion.identity);
			weaponSplitterProjectile.isMain = false;
			weaponSplitterProjectile.splitAngle = WeaponProperties.LevelWeaponSplitter.Basic.splitAngle;
			weaponSplitterProjectile.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle + this.splitAngle);
			weaponSplitterProjectile.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance;
			weaponSplitterProjectile.dist = this.dist;
			weaponSplitterProjectile.splitDamage = this.Damage;
		}
		else
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle + this.splitAngle);
			this.distancePastSplit = WeaponProperties.LevelWeaponSplitter.Basic.angleDistance;
		}
		if (this.nextDistance == WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB)
		{
			this.nextDistance = float.MaxValue;
		}
		else
		{
			this.nextDistance = WeaponProperties.LevelWeaponSplitter.Basic.splitDistanceB;
		}
	}

	// Token: 0x0600405F RID: 16479 RVA: 0x00231420 File Offset: 0x0022F820
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		this.dist += this.Speed * CupheadTime.FixedDelta;
		if (this.dist > this.nextDistance)
		{
			this.Split();
		}
		if (this.distancePastSplit > 0f)
		{
			this.distancePastSplit -= this.Speed * CupheadTime.FixedDelta;
			if (this.distancePastSplit <= 0f)
			{
				base.transform.eulerAngles = new Vector3(0f, 0f, this.baseAngle);
			}
		}
	}

	// Token: 0x0400472D RID: 18221
	public bool isMain;

	// Token: 0x0400472E RID: 18222
	public float nextDistance;

	// Token: 0x0400472F RID: 18223
	public float baseAngle;

	// Token: 0x04004730 RID: 18224
	private float distancePastSplit;

	// Token: 0x04004731 RID: 18225
	private float splitAngle;

	// Token: 0x04004732 RID: 18226
	private float dist;

	// Token: 0x04004733 RID: 18227
	private float splitDamage = -1f;
}
