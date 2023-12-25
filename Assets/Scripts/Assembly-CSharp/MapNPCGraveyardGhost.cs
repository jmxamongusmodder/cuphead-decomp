using System;
using System.Collections;

// Token: 0x02000952 RID: 2386
public class MapNPCGraveyardGhost : MapDialogueInteraction
{
	// Token: 0x060037BA RID: 14266 RVA: 0x001FFC08 File Offset: 0x001FE008
	protected override void Start()
	{
		base.Start();
		if (CharmCurse.IsMaxLevel(PlayerId.PlayerOne) || CharmCurse.IsMaxLevel(PlayerId.PlayerTwo))
		{
			Dialoguer.SetGlobalFloat(41, 2f);
		}
		else if (CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1 || CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1)
		{
			Dialoguer.SetGlobalFloat(41, 1f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(41, 0f);
		}
	}

	// Token: 0x17000486 RID: 1158
	// (get) Token: 0x060037BB RID: 14267 RVA: 0x001FFC77 File Offset: 0x001FE077
	protected override bool ChangesDepth
	{
		get
		{
			return false;
		}
	}

	// Token: 0x060037BC RID: 14268 RVA: 0x001FFC7A File Offset: 0x001FE07A
	public void TalkAfterPlayerGotCharm()
	{
		base.StartCoroutine(this.got_charm_notification_cr());
	}

	// Token: 0x060037BD RID: 14269 RVA: 0x001FFC8C File Offset: 0x001FE08C
	private IEnumerator got_charm_notification_cr()
	{
		Dialoguer.SetGlobalFloat(41, 1f);
		while (Map.Current.players[0].state == MapPlayerController.State.Stationary || (Map.Current.players[1] != null && Map.Current.players[1].state == MapPlayerController.State.Stationary))
		{
			yield return null;
		}
		this.StartSpeechBubble();
		while (this.currentlySpeaking)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x060037BE RID: 14270 RVA: 0x001FFCA8 File Offset: 0x001FE0A8
	protected override void Update()
	{
		base.Update();
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f < this.idleNormalizedTime)
			{
				this.idleCycleCount++;
				if (this.idleCycleCount % 3 == 0)
				{
					base.animator.SetTrigger("Puff");
				}
				if (this.idleCycleCount % 7 == 3)
				{
					base.animator.SetTrigger("BlinkOnce");
				}
				if (this.idleCycleCount % 7 == 6)
				{
					base.animator.SetTrigger("BlinkTwice");
				}
			}
			this.idleNormalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime % 1f;
		}
	}

	// Token: 0x04003FBC RID: 16316
	private const int GRAVEYARD_GHOST_STATE_INDEX = 41;

	// Token: 0x04003FBD RID: 16317
	private float idleNormalizedTime;

	// Token: 0x04003FBE RID: 16318
	private int idleCycleCount;

	// Token: 0x04003FBF RID: 16319
	private int nextPuffMultiplier = 4;
}
