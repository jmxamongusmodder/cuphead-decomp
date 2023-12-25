using System;
using UnityEngine;

// Token: 0x02000799 RID: 1945
public class RumRunnersLevelPoliceBullet : BasicProjectile
{
	// Token: 0x170003FA RID: 1018
	// (get) Token: 0x06002B35 RID: 11061 RVA: 0x00192C59 File Offset: 0x00191059
	// (set) Token: 0x06002B36 RID: 11062 RVA: 0x00192C61 File Offset: 0x00191061
	public float spiderDamage { get; set; }

	// Token: 0x170003FB RID: 1019
	// (get) Token: 0x06002B37 RID: 11063 RVA: 0x00192C6A File Offset: 0x0019106A
	// (set) Token: 0x06002B38 RID: 11064 RVA: 0x00192C72 File Offset: 0x00191072
	public RumRunnersLevelPoliceman.Direction direction { get; set; }

	// Token: 0x06002B39 RID: 11065 RVA: 0x00192C7C File Offset: 0x0019107C
	protected override void Start()
	{
		base.Start();
		this.spiderDamageDealer = new DamageDealer(this);
		this.spiderDamageDealer.SetDamage(this.spiderDamage);
		this.spiderDamageDealer.OnDealDamage += this.OnDealDamage;
		this.spiderDamageDealer.SetStoneTime(base.StoneTime);
		this.spiderDamageDealer.PlayerId = this.PlayerId;
	}

	// Token: 0x06002B3A RID: 11066 RVA: 0x00192CE8 File Offset: 0x001910E8
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			DamageReceiver damageReceiver = hit.GetComponent<DamageReceiver>();
			if (damageReceiver == null)
			{
				DamageReceiverChild component = hit.GetComponent<DamageReceiverChild>();
				if (component != null)
				{
					damageReceiver = component.Receiver;
				}
			}
			if (damageReceiver != null && damageReceiver.GetComponent<RumRunnersLevelSpider>() != null)
			{
				this.spiderDamageDealer.DealDamage(hit);
				base.OnCollisionEnemy(hit, phase);
				this.Die();
			}
		}
	}

	// Token: 0x040033E8 RID: 13288
	private DamageDealer spiderDamageDealer;
}
