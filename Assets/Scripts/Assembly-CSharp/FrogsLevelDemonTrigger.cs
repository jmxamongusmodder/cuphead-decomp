using System;
using UnityEngine;

// Token: 0x020006AA RID: 1706
public class FrogsLevelDemonTrigger : AbstractCollidableObject
{
	// Token: 0x06002428 RID: 9256 RVA: 0x00153721 File Offset: 0x00151B21
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002429 RID: 9257 RVA: 0x0015374C File Offset: 0x00151B4C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.isTriggered = true;
	}

	// Token: 0x0600242A RID: 9258 RVA: 0x0015376D File Offset: 0x00151B6D
	public bool getTrigger()
	{
		return this.isTriggered;
	}

	// Token: 0x04002CEB RID: 11499
	private DamageReceiver damageReceiver;

	// Token: 0x04002CEC RID: 11500
	private bool isTriggered;
}
