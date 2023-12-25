using System;
using UnityEngine;

// Token: 0x02000A8D RID: 2701
public class WeaponWideShotExProjectile : AbstractProjectile
{
	// Token: 0x06004099 RID: 16537 RVA: 0x002328FC File Offset: 0x00230CFC
	protected override void Start()
	{
		base.Start();
		base.transform.position += base.transform.right * 100f;
		this.damageDealer.isDLCWeapon = true;
	}

	// Token: 0x0600409A RID: 16538 RVA: 0x0023293B File Offset: 0x00230D3B
	protected override void Update()
	{
		base.Update();
		if (this.mainTimer < this.mainDuration)
		{
			this.mainTimer += CupheadTime.Delta;
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0600409B RID: 16539 RVA: 0x0023297C File Offset: 0x00230D7C
	protected override void OnDealDamage(float damage, DamageReceiver receiver, DamageDealer damageDealer)
	{
		base.OnDealDamage(damage, receiver, damageDealer);
		Collider2D componentInChildren = receiver.GetComponentInChildren<Collider2D>();
		Vector3 b = receiver.transform.position;
		if (componentInChildren != null)
		{
			b = componentInChildren.transform.position + new Vector3(componentInChildren.offset.x * receiver.transform.lossyScale.x, componentInChildren.offset.y * receiver.transform.lossyScale.y);
		}
		Vector3 a = MathUtils.AngleToDirection(base.transform.eulerAngles.z);
		Vector3 a2 = this.origin + a * Vector3.Distance(this.origin, b);
		this.hitsparkPrefab.Create(Vector3.Lerp(a2, b, 0.5f) + MathUtils.RandomPointInUnitCircle() * 30f);
	}

	// Token: 0x0600409C RID: 16540 RVA: 0x00232A7F File Offset: 0x00230E7F
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionEnemy(hit, phase);
		this.damageDealer.DealDamage(hit);
		this.damageDealer.OnDealDamage += this.OnDealDamage;
	}

	// Token: 0x04004754 RID: 18260
	public float mainDuration;

	// Token: 0x04004755 RID: 18261
	private float mainTimer;

	// Token: 0x04004756 RID: 18262
	public Vector3 origin;

	// Token: 0x04004757 RID: 18263
	[SerializeField]
	private Effect hitsparkPrefab;
}
