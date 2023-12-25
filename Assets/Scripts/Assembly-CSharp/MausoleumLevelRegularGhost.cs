using System;

// Token: 0x020006D8 RID: 1752
public class MausoleumLevelRegularGhost : MausoleumLevelGhostBase
{
	// Token: 0x06002554 RID: 9556 RVA: 0x0015D596 File Offset: 0x0015B996
	protected override void Start()
	{
		base.Start();
		base.animator.SetBool("IsA", Rand.Bool());
	}

	// Token: 0x06002555 RID: 9557 RVA: 0x0015D5B3 File Offset: 0x0015B9B3
	public override void OnParry(AbstractPlayerController player)
	{
		AudioManager.Play("mausoleum_regular_ghost_1_death");
		this.emitAudioFromObject.Add("mausoleum_regular_ghost_1_death");
		base.OnParry(player);
	}
}
