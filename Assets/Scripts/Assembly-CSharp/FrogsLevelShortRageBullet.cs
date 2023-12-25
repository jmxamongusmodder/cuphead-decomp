using System;

// Token: 0x020006BD RID: 1725
public class FrogsLevelShortRageBullet : BasicProjectile
{
	// Token: 0x0600249B RID: 9371 RVA: 0x001574C0 File Offset: 0x001558C0
	protected override void Die()
	{
		if (!base.CanParry)
		{
			return;
		}
		base.Die();
		this.move = true;
	}
}
