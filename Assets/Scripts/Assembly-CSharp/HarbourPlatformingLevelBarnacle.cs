using System;

// Token: 0x020008C2 RID: 2242
public class HarbourPlatformingLevelBarnacle : PlatformingLevelShootingEnemy
{
	// Token: 0x06003466 RID: 13414 RVA: 0x001E6FC5 File Offset: 0x001E53C5
	private void AttackSFX()
	{
		AudioManager.Play("harbour_barnacle_attack");
		this.emitAudioFromObject.Add("harbour_barnacle_attack");
	}

	// Token: 0x06003467 RID: 13415 RVA: 0x001E6FE1 File Offset: 0x001E53E1
	protected override void Die()
	{
		AudioManager.Play("harbour_barnacle_death");
		this.emitAudioFromObject.Add("harbour_barnacle_death");
		base.Die();
	}
}
