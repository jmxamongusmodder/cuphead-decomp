using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200095A RID: 2394
public class MapNPCQuadratus : AbstractMapInteractiveEntity
{
	// Token: 0x060037E3 RID: 14307 RVA: 0x002008E2 File Offset: 0x001FECE2
	private void Start()
	{
		this.AddDialoguerEvents();
		if (this.entitySpriteRenderer == null)
		{
			this.entitySpriteRenderer = base.GetComponent<SpriteRenderer>();
		}
	}

	// Token: 0x060037E4 RID: 14308 RVA: 0x00200907 File Offset: 0x001FED07
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060037E5 RID: 14309 RVA: 0x00200918 File Offset: 0x001FED18
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x060037E6 RID: 14310 RVA: 0x00200968 File Offset: 0x001FED68
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
	}

	// Token: 0x060037E7 RID: 14311 RVA: 0x002009B7 File Offset: 0x001FEDB7
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "QuadratusGift")
		{
		}
	}

	// Token: 0x060037E8 RID: 14312 RVA: 0x002009C9 File Offset: 0x001FEDC9
	protected override void Activate()
	{
	}

	// Token: 0x060037E9 RID: 14313 RVA: 0x002009CC File Offset: 0x001FEDCC
	protected override MapUIInteractionDialogue Show(PlayerInput player)
	{
		int num = PlayerData.Data.DeathCount(PlayerId.Any);
		float num2 = Mathf.Sqrt((float)num);
		if (num > 48 && num2 == Mathf.Round(num2))
		{
			Dialoguer.SetGlobalFloat(15, 1f);
		}
		else
		{
			Dialoguer.SetGlobalFloat(15, 0f);
		}
		Dialoguer.SetGlobalFloat(this.dialoguerScholarVariableID, 3f);
		PlayerData.SaveCurrentFile();
		base.StartCoroutine(this.tween_cr(this.entitySpriteRenderer.color.a, 0.65f, EaseUtils.EaseType.easeInOutCubic, 0.5f));
		return null;
	}

	// Token: 0x060037EA RID: 14314 RVA: 0x00200A64 File Offset: 0x001FEE64
	public override void Hide(MapUIInteractionDialogue dialogue)
	{
		if (Map.Current.CurrentState != Map.State.Ready)
		{
			return;
		}
		if (Map.Current.players[0].state != MapPlayerController.State.Walking)
		{
			return;
		}
		base.StartCoroutine(this.tween_cr(this.entitySpriteRenderer.color.a, 0f, EaseUtils.EaseType.easeInOutCubic, 0.5f));
	}

	// Token: 0x060037EB RID: 14315 RVA: 0x00200AC4 File Offset: 0x001FEEC4
	private void OnDialogueEndedHandler()
	{
	}

	// Token: 0x060037EC RID: 14316 RVA: 0x00200AC8 File Offset: 0x001FEEC8
	private IEnumerator tween_cr(float start, float end, EaseUtils.EaseType ease, float time)
	{
		if (start == end)
		{
			yield break;
		}
		float t = 0f;
		Color currentColor = Color.white;
		currentColor.a = start;
		this.entitySpriteRenderer.color = currentColor;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, start, end, t / time);
			currentColor.a = val;
			this.entitySpriteRenderer.color = currentColor;
			t += CupheadTime.Delta;
			yield return null;
		}
		currentColor.a = end;
		this.entitySpriteRenderer.color = currentColor;
		yield return null;
		yield break;
	}

	// Token: 0x04003FD8 RID: 16344
	private const int QUADRATUS_STATE_ID = 15;

	// Token: 0x04003FD9 RID: 16345
	[SerializeField]
	private SpriteRenderer entitySpriteRenderer;

	// Token: 0x04003FDA RID: 16346
	[SerializeField]
	private int dialoguerScholarVariableID = 11;
}
