using System;
using UnityEngine;

// Token: 0x02000509 RID: 1289
public class BatLevelMiniBat : BasicSineProjectile
{
	// Token: 0x060016DD RID: 5853 RVA: 0x000CDA54 File Offset: 0x000CBE54
	public BatLevelMiniBat Create(Vector2 pos, float rotation, float velocity, float sinVelocity, float sinSize, float health)
	{
		BatLevelMiniBat batLevelMiniBat = base.Create(pos, rotation, velocity, sinVelocity, sinSize) as BatLevelMiniBat;
		batLevelMiniBat.health = health;
		return batLevelMiniBat;
	}

	// Token: 0x060016DE RID: 5854 RVA: 0x000CDA7D File Offset: 0x000CBE7D
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060016DF RID: 5855 RVA: 0x000CDAA8 File Offset: 0x000CBEA8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x04002028 RID: 8232
	private DamageReceiver damageReceiver;

	// Token: 0x04002029 RID: 8233
	private float health;
}
