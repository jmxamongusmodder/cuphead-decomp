using System;
using UnityEngine;

// Token: 0x020004AF RID: 1199
public class TestLevelStationaryJared : LevelProperties.Test.Entity
{
	// Token: 0x06001390 RID: 5008 RVA: 0x000AC2AA File Offset: 0x000AA6AA
	public override void LevelInit(LevelProperties.Test properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001391 RID: 5009 RVA: 0x000AC2B3 File Offset: 0x000AA6B3
	private void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001392 RID: 5010 RVA: 0x000AC2D8 File Offset: 0x000AA6D8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		AudioManager.Play("test_sound_2");
	}

	// Token: 0x06001393 RID: 5011 RVA: 0x000AC2E4 File Offset: 0x000AA6E4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001394 RID: 5012 RVA: 0x000AC2EE File Offset: 0x000AA6EE
	public override void OnParry(AbstractPlayerController player)
	{
		base.OnParry(player);
		player.stats.OnParry(1f, true);
	}

	// Token: 0x04001C9F RID: 7327
	[SerializeField]
	private Transform childSprite;

	// Token: 0x04001CA0 RID: 7328
	private DamageReceiver damageReceiver;
}
