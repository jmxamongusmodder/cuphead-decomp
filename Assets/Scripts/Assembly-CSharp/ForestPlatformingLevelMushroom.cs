using System;

// Token: 0x02000883 RID: 2179
public class ForestPlatformingLevelMushroom : PlatformingLevelShootingEnemy
{
	// Token: 0x06003296 RID: 12950 RVA: 0x001D68D5 File Offset: 0x001D4CD5
	protected override void Awake()
	{
		base.Awake();
		ForestPlatformingLevelMushroomProjectile.numUntilPink = base.Properties.MushroomPinkNumber.RandomInt();
	}

	// Token: 0x06003297 RID: 12951 RVA: 0x001D68F2 File Offset: 0x001D4CF2
	protected override void Shoot()
	{
		base.Shoot();
	}

	// Token: 0x06003298 RID: 12952 RVA: 0x001D68FC File Offset: 0x001D4CFC
	private void EmergeFromGround()
	{
		base.setDirection((this._target.center.x <= base.transform.position.x) ? PlatformingLevelShootingEnemy.Direction.Left : PlatformingLevelShootingEnemy.Direction.Right);
	}

	// Token: 0x06003299 RID: 12953 RVA: 0x001D6941 File Offset: 0x001D4D41
	private void PlayMushroomSound()
	{
		AudioManager.Play("level_mushroom_shoot");
		this.emitAudioFromObject.Add("level_mushroom_shoot");
	}

	// Token: 0x0600329A RID: 12954 RVA: 0x001D695D File Offset: 0x001D4D5D
	protected override void Die()
	{
		AudioManager.Play("level_mermaid_turtle_shell_pop");
		this.emitAudioFromObject.Add("level_mermaid_turtle_shell_pop");
		base.Die();
	}
}
