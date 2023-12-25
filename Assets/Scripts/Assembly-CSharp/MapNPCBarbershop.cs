using System;
using UnityEngine;

// Token: 0x02000948 RID: 2376
public class MapNPCBarbershop : AbstractMonoBehaviour
{
	// Token: 0x0600377D RID: 14205 RVA: 0x001FE3EE File Offset: 0x001FC7EE
	private void Start()
	{
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) > 0f)
		{
			this.NowFour();
			this.CleanUp();
		}
	}

	// Token: 0x0600377E RID: 14206 RVA: 0x001FE411 File Offset: 0x001FC811
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.DrawWireSphere(this.fourPosition + base.transform.parent.position, 1f);
	}

	// Token: 0x0600377F RID: 14207 RVA: 0x001FE43E File Offset: 0x001FC83E
	public void NowFour()
	{
		base.animator.runtimeAnimatorController = this.fourAnimatorController;
		base.transform.localPosition = this.fourPosition;
	}

	// Token: 0x06003780 RID: 14208 RVA: 0x001FE462 File Offset: 0x001FC862
	public void CleanUp()
	{
		if (this.mapDialogueInteraction)
		{
			UnityEngine.Object.Destroy(this.mapDialogueInteraction);
		}
		if (this.mapNPCDistanceAnimator)
		{
			UnityEngine.Object.Destroy(this.mapNPCDistanceAnimator);
		}
	}

	// Token: 0x06003781 RID: 14209 RVA: 0x001FE49A File Offset: 0x001FC89A
	private void SongLooped()
	{
	}

	// Token: 0x06003782 RID: 14210 RVA: 0x001FE49C File Offset: 0x001FC89C
	private void Show()
	{
		base.animator.SetTrigger("show");
	}

	// Token: 0x04003F91 RID: 16273
	[SerializeField]
	private RuntimeAnimatorController fourAnimatorController;

	// Token: 0x04003F92 RID: 16274
	[SerializeField]
	private Vector3 fourPosition;

	// Token: 0x04003F93 RID: 16275
	[SerializeField]
	protected MapNPCLostBarbershop mapNPCDistanceAnimator;

	// Token: 0x04003F94 RID: 16276
	public MapDialogueInteraction mapDialogueInteraction;

	// Token: 0x04003F95 RID: 16277
	[SerializeField]
	private int dialoguerVariableID = 10;
}
