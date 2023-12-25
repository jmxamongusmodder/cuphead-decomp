using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000ABF RID: 2751
public class PlaneWeaponLaser : AbstractPlaneWeapon
{
	// Token: 0x170005C8 RID: 1480
	// (get) Token: 0x06004214 RID: 16916 RVA: 0x0023B346 File Offset: 0x00239746
	protected override bool rapidFire
	{
		get
		{
			return WeaponProperties.PlaneWeaponLaser.Basic.rapidFire;
		}
	}

	// Token: 0x170005C9 RID: 1481
	// (get) Token: 0x06004215 RID: 16917 RVA: 0x0023B34D File Offset: 0x0023974D
	protected override float rapidFireRate
	{
		get
		{
			return WeaponProperties.PlaneWeaponLaser.Basic.rapidFireRate;
		}
	}

	// Token: 0x06004216 RID: 16918 RVA: 0x0023B354 File Offset: 0x00239754
	protected override AbstractProjectile fireBasic()
	{
		BasicProjectile basicProjectile = base.fireBasic() as BasicProjectile;
		basicProjectile.Speed = WeaponProperties.PlaneWeaponLaser.Basic.speed;
		basicProjectile.Damage = WeaponProperties.PlaneWeaponLaser.Basic.damage;
		basicProjectile.PlayerId = this.player.id;
		float num = this.yPositions[this.currentY];
		this.currentY++;
		if (this.currentY >= this.yPositions.Length)
		{
			this.currentY = 0;
		}
		basicProjectile.transform.AddPosition(0f, num, 0f);
		if (this.player.Shrunk)
		{
			basicProjectile.Damage *= this.shrunkDamageMultiplier;
			basicProjectile.transform.AddPosition(0f, num * -0.5f, 0f);
			basicProjectile.DestroyDistance = (float)UnityEngine.Random.Range(200, 350);
			basicProjectile.DestroyDistanceAnimated = true;
		}
		return basicProjectile;
	}

	// Token: 0x06004217 RID: 16919 RVA: 0x0023B43D File Offset: 0x0023983D
	protected override AbstractProjectile fireEx()
	{
		base.StartCoroutine(this.ex_cr());
		return null;
	}

	// Token: 0x06004218 RID: 16920 RVA: 0x0023B450 File Offset: 0x00239850
	private IEnumerator ex_cr()
	{
		for (int wave = 0; wave < WeaponProperties.PlaneWeaponLaser.Ex.counts.Length; wave++)
		{
			int count = WeaponProperties.PlaneWeaponLaser.Ex.counts[wave];
			float angle = WeaponProperties.PlaneWeaponLaser.Ex.angles[wave];
			for (int i = 0; i < count; i++)
			{
				float value = Mathf.Lerp(0f, angle, (float)i / (float)count) - 90f;
				BasicProjectile basicProjectile = base.fireEx() as BasicProjectile;
				basicProjectile.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
				basicProjectile.Speed = WeaponProperties.PlaneWeaponLaser.Ex.speed;
				basicProjectile.Damage = WeaponProperties.PlaneWeaponLaser.Ex.damage;
				basicProjectile.PlayerId = this.player.id;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		yield break;
	}

	// Token: 0x04004890 RID: 18576
	private const float Y_POS = 20f;

	// Token: 0x04004891 RID: 18577
	private float[] yPositions = new float[]
	{
		20f,
		-20f
	};

	// Token: 0x04004892 RID: 18578
	private int currentY;
}
