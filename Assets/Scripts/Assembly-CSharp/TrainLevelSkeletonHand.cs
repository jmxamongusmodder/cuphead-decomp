using System;
using UnityEngine;

// Token: 0x0200082B RID: 2091
public class TrainLevelSkeletonHand : AbstractTrainLevelSkeletonPart
{
	// Token: 0x06003094 RID: 12436 RVA: 0x001C9890 File Offset: 0x001C7C90
	private void PlaySlapFX()
	{
		this.effectPrefab.Create(this.effectRoot.position, base.transform.localScale);
		CupheadLevelCamera.Current.Shake(20f, 0.6f, false);
	}

	// Token: 0x06003095 RID: 12437 RVA: 0x001C98C9 File Offset: 0x001C7CC9
	public void Slap()
	{
		base.animator.Play("Slap");
	}

	// Token: 0x06003096 RID: 12438 RVA: 0x001C98DB File Offset: 0x001C7CDB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.effectPrefab = null;
	}

	// Token: 0x04003935 RID: 14645
	[SerializeField]
	private Transform effectRoot;

	// Token: 0x04003936 RID: 14646
	[SerializeField]
	private Effect effectPrefab;
}
