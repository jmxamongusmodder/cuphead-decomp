using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D2 RID: 1746
public class KitchenSaltbakerCounter : DialogueInteractionPoint
{
	// Token: 0x0600252A RID: 9514 RVA: 0x0015C894 File Offset: 0x0015AC94
	protected override void Start()
	{
		base.Start();
		Dialoguer.events.onTextPhase += this.onDialogueAdvancedHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		if (Dialoguer.GetGlobalFloat(23) == 0f)
		{
			base.StartCoroutine(this.dialogue_on_first_visit_cr());
		}
	}

	// Token: 0x0600252B RID: 9515 RVA: 0x0015C8F1 File Offset: 0x0015ACF1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onTextPhase -= this.onDialogueAdvancedHandler;
		Dialoguer.events.onEnded -= this.onDialogueEndedHandler;
	}

	// Token: 0x0600252C RID: 9516 RVA: 0x0015C928 File Offset: 0x0015AD28
	private void onDialogueAdvancedHandler(DialoguerTextData data)
	{
		if (!base.animator.GetCurrentAnimatorStateInfo(0).IsName("Talk"))
		{
			base.animator.SetTrigger("Talk");
		}
	}

	// Token: 0x0600252D RID: 9517 RVA: 0x0015C963 File Offset: 0x0015AD63
	private void onDialogueEndedHandler()
	{
		base.animator.SetBool("PlayerClose", false);
	}

	// Token: 0x0600252E RID: 9518 RVA: 0x0015C976 File Offset: 0x0015AD76
	protected override void Activate()
	{
		base.animator.SetBool("PlayerClose", true);
		base.animator.SetTrigger("Talk");
		base.Activate();
	}

	// Token: 0x0600252F RID: 9519 RVA: 0x0015C9A0 File Offset: 0x0015ADA0
	private IEnumerator dialogue_on_first_visit_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.speechBubble.waitForRealease = false;
		this.Activate();
		this.Hide(PlayerId.PlayerOne);
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			this.Hide(PlayerId.PlayerTwo);
		}
		yield break;
	}

	// Token: 0x06002530 RID: 9520 RVA: 0x0015C9BC File Offset: 0x0015ADBC
	private void Update()
	{
		base.animator.SetBool("PlayerClose", this.conversationIsActive);
		this.blinkTimer -= CupheadTime.Delta;
		if (this.blinkTimer < 0f)
		{
			this.blinkTimer = this.blinkRange.RandomFloat();
			base.animator.SetTrigger("Blink");
		}
	}

	// Token: 0x04002DD5 RID: 11733
	private const int DIALOGUER_VAR_ID = 23;

	// Token: 0x04002DD6 RID: 11734
	[SerializeField]
	private MinMax blinkRange = new MinMax(2.5f, 4.5f);

	// Token: 0x04002DD7 RID: 11735
	private float blinkTimer;
}
