using System;
using UnityEngine;

// Token: 0x02000AB5 RID: 2741
public class PlaneWeaponBombExplosion : Effect
{
	// Token: 0x060041D7 RID: 16855 RVA: 0x00239818 File Offset: 0x00237C18
	public void Create(Vector2 position, float damage, float damageMultiplier, float size)
	{
		PlaneWeaponBombExplosion planeWeaponBombExplosion = base.Create(position) as PlaneWeaponBombExplosion;
		planeWeaponBombExplosion.damageDealer.SetDamage(damage);
		planeWeaponBombExplosion.damageDealer.DamageMultiplier *= damageMultiplier;
		planeWeaponBombExplosion.damageDealer.SetDamageFlags(false, true, false);
		planeWeaponBombExplosion.transform.SetScale(new float?(size), new float?(size), null);
	}

	// Token: 0x060041D8 RID: 16856 RVA: 0x00239886 File Offset: 0x00237C86
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060041D9 RID: 16857 RVA: 0x00239899 File Offset: 0x00237C99
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060041DA RID: 16858 RVA: 0x002398B1 File Offset: 0x00237CB1
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		if (phase == CollisionPhase.Enter && this.damageDealer != null)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0400483B RID: 18491
	private DamageDealer damageDealer;
}
