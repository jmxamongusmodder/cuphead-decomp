using System;
using UnityEngine;

// Token: 0x0200094E RID: 2382
public class MapNPCCoinMoneyman : MonoBehaviour
{
	// Token: 0x060037AF RID: 14255 RVA: 0x001FF949 File Offset: 0x001FDD49
	private void Start()
	{
		this.UpdateCoins();
		this.LookAroundFinished();
	}

	// Token: 0x060037B0 RID: 14256 RVA: 0x001FF958 File Offset: 0x001FDD58
	public void UpdateCoins()
	{
		for (int i = 0; i < this.hiddenCoinIds.Length; i++)
		{
			if (!PlayerData.Data.coinManager.GetCoinCollected(this.hiddenCoinIds[i]))
			{
				return;
			}
		}
		Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
		PlayerData.SaveCurrentFile();
	}

	// Token: 0x060037B1 RID: 14257 RVA: 0x001FF9B0 File Offset: 0x001FDDB0
	private void Update()
	{
		if (this.waiting)
		{
			return;
		}
		this.durationBeforeNext -= CupheadTime.Delta;
		this.durationBeforeBlink -= CupheadTime.Delta;
		if (this.durationBeforeBlink <= 0f)
		{
			this.durationBeforeBlink = float.PositiveInfinity;
			this.animator.SetTrigger("blink");
		}
		if (this.durationBeforeNext <= 0f)
		{
			this.waiting = true;
			this.animator.SetTrigger("next");
		}
	}

	// Token: 0x060037B2 RID: 14258 RVA: 0x001FFA49 File Offset: 0x001FDE49
	private void LookAroundFinished()
	{
		this.durationBeforeNext = UnityEngine.Random.Range(this.idleDurationMin, this.idleDurationMax);
		this.durationBeforeBlink = UnityEngine.Random.Range(0f, this.durationBeforeNext);
		this.waiting = false;
	}

	// Token: 0x04003FB0 RID: 16304
	[SerializeField]
	private Animator animator;

	// Token: 0x04003FB1 RID: 16305
	[SerializeField]
	private float idleDurationMin;

	// Token: 0x04003FB2 RID: 16306
	[SerializeField]
	private float idleDurationMax;

	// Token: 0x04003FB3 RID: 16307
	private float durationBeforeNext;

	// Token: 0x04003FB4 RID: 16308
	private float durationBeforeBlink;

	// Token: 0x04003FB5 RID: 16309
	private bool waiting = true;

	// Token: 0x04003FB6 RID: 16310
	[SerializeField]
	private string[] hiddenCoinIds;

	// Token: 0x04003FB7 RID: 16311
	[SerializeField]
	private int dialoguerVariableID = 4;
}
