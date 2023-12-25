using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000954 RID: 2388
public class MapNPCLostBarbershop : AbstractMapInteractiveEntity
{
	// Token: 0x060037C6 RID: 14278 RVA: 0x00200066 File Offset: 0x001FE466
	private void Start()
	{
		this.AddDialoguerEvents();
	}

	// Token: 0x060037C7 RID: 14279 RVA: 0x0020006E File Offset: 0x001FE46E
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060037C8 RID: 14280 RVA: 0x0020007C File Offset: 0x001FE47C
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
	}

	// Token: 0x060037C9 RID: 14281 RVA: 0x002000CC File Offset: 0x001FE4CC
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
	}

	// Token: 0x060037CA RID: 14282 RVA: 0x0020011B File Offset: 0x001FE51B
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "LostBarberFound")
		{
			base.GetComponent<MapDialogueInteraction>().disabledActivations = true;
			this.reunited = true;
		}
	}

	// Token: 0x060037CB RID: 14283 RVA: 0x0020014C File Offset: 0x001FE54C
	protected override void Activate()
	{
	}

	// Token: 0x060037CC RID: 14284 RVA: 0x0020014E File Offset: 0x001FE54E
	protected override MapUIInteractionDialogue Show(PlayerInput player)
	{
		this.FoundBarbershopSFX();
		base.animator.SetTrigger(this.triggerShow);
		return null;
	}

	// Token: 0x060037CD RID: 14285 RVA: 0x00200168 File Offset: 0x001FE568
	private void OnDialogueEndedHandler()
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (this.reunited)
		{
			Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
			PlayerData.SaveCurrentFile();
			base.StartCoroutine(this.found_cr());
		}
	}

	// Token: 0x060037CE RID: 14286 RVA: 0x002001A4 File Offset: 0x001FE5A4
	private IEnumerator found_cr()
	{
		base.animator.SetTrigger(this.triggerHide);
		yield return base.animator.WaitForAnimationToEnd(this, "anim_map_barbershop_outtro_d", false, true);
		this.playerCanWalkBehind = true;
		base.SetLayer(base.GetComponent<SpriteRenderer>());
		for (int i = 0; i < this.mapNPCBarbershops.Length; i++)
		{
			this.mapNPCBarbershops[i].NowFour();
			if (!(this.mapNPCBarbershops[i].mapDialogueInteraction == null))
			{
				for (int j = 0; j < this.mapNPCBarbershops[i].mapDialogueInteraction.dialogues.Length; j++)
				{
					if (!(this.mapNPCBarbershops[i].mapDialogueInteraction.dialogues[j] == null))
					{
						this.mapNPCBarbershops[i].mapDialogueInteraction.Hide(this.mapNPCBarbershops[i].mapDialogueInteraction.dialogues[j]);
					}
				}
			}
		}
		this.Hide(null);
		while (CupheadMapCamera.Current != null && CupheadMapCamera.Current.IsCameraFarFromPlayer())
		{
			yield return null;
		}
		for (int k = 0; k < this.mapNPCBarbershops.Length; k++)
		{
			this.mapNPCBarbershops[k].CleanUp();
		}
		yield break;
	}

	// Token: 0x060037CF RID: 14287 RVA: 0x002001BF File Offset: 0x001FE5BF
	private void FoundBarbershopSFX()
	{
		if (!this.FirstTimeFoundSFX)
		{
			AudioManager.Play("find_barbershop_member");
			this.FirstTimeFoundSFX = true;
		}
	}

	// Token: 0x04003FC4 RID: 16324
	[SerializeField]
	private string triggerShow;

	// Token: 0x04003FC5 RID: 16325
	[SerializeField]
	private string triggerHide;

	// Token: 0x04003FC6 RID: 16326
	[SerializeField]
	private MapNPCBarbershop[] mapNPCBarbershops;

	// Token: 0x04003FC7 RID: 16327
	[SerializeField]
	private int dialoguerVariableID = 10;

	// Token: 0x04003FC8 RID: 16328
	private bool reunited;

	// Token: 0x04003FC9 RID: 16329
	private bool FirstTimeFoundSFX;

	// Token: 0x04003FCA RID: 16330
	private SpriteRenderer spriteRenderer;

	// Token: 0x04003FCB RID: 16331
	[HideInInspector]
	public bool SkipDialogueEvent;
}
