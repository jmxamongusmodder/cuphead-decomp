using System;
using UnityEngine;

// Token: 0x020007A9 RID: 1961
public class SallyStagePlayLevelBowingAnimation : AbstractPausableComponent
{
	// Token: 0x06002C12 RID: 11282 RVA: 0x0019EAE2 File Offset: 0x0019CEE2
	private void Start()
	{
		Level.Current.OnWinEvent += this.OnDeath;
	}

	// Token: 0x06002C13 RID: 11283 RVA: 0x0019EAFA File Offset: 0x0019CEFA
	private void PickNumber()
	{
		this.maxCounter = UnityEngine.Random.Range(12, 21);
	}

	// Token: 0x06002C14 RID: 11284 RVA: 0x0019EB0C File Offset: 0x0019CF0C
	private void Counter()
	{
		if (this.counter < this.maxCounter)
		{
			this.counter++;
		}
		else
		{
			foreach (Animator animator in this.animators)
			{
				animator.SetTrigger("OnBow");
				this.counter = 0;
			}
		}
	}

	// Token: 0x06002C15 RID: 11285 RVA: 0x0019EB70 File Offset: 0x0019CF70
	private void OnDeath()
	{
		foreach (Animator animator in this.animators)
		{
			animator.SetTrigger("OnDeath");
		}
	}

	// Token: 0x06002C16 RID: 11286 RVA: 0x0019EBA7 File Offset: 0x0019CFA7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Level.Current.OnWinEvent -= this.OnDeath;
	}

	// Token: 0x040034CA RID: 13514
	[SerializeField]
	private Animator[] animators;

	// Token: 0x040034CB RID: 13515
	private int counter;

	// Token: 0x040034CC RID: 13516
	private int maxCounter;
}
