using System;
using UnityEngine;

// Token: 0x0200095C RID: 2396
public class MapNPCTurtle : MapDialogueInteraction
{
	// Token: 0x060037F2 RID: 14322 RVA: 0x00200D34 File Offset: 0x001FF134
	protected override void Start()
	{
		base.Start();
		Dialoguer.events.onEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.OnDialogueEndedHandler;
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 2f)
		{
			if (PlayerData.Data.CheckLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P))
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
				PlayerData.SaveCurrentFile();
			}
			else if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 1f && PlayerData.Data.CountLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.P) > 1)
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
				PlayerData.SaveCurrentFile();
			}
		}
	}

	// Token: 0x060037F3 RID: 14323 RVA: 0x00200E10 File Offset: 0x001FF210
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Dialoguer.events.onEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.OnDialogueEndedHandler;
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037F4 RID: 14324 RVA: 0x00200E68 File Offset: 0x001FF268
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "Pacifist")
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Turtle);
			PlayerData.Data.unlockedBlackAndWhite = true;
			PlayerData.SaveCurrentFile();
			MapUI.Current.Refresh();
		}
	}

	// Token: 0x060037F5 RID: 14325 RVA: 0x00200EB8 File Offset: 0x001FF2B8
	protected override void Activate(MapPlayerController player)
	{
		if (this.dialogues[(int)player.id].transform.localScale.x == 1f)
		{
			base.Activate(player);
			if (this.colliderB.OverlapPoint(player.transform.position))
			{
				base.animator.SetTrigger("turn_b");
			}
			else
			{
				base.animator.SetTrigger("turn_a");
			}
		}
	}

	// Token: 0x060037F6 RID: 14326 RVA: 0x00200F3A File Offset: 0x001FF33A
	private void OnDialogueEndedHandler()
	{
		base.animator.SetTrigger("return");
	}

	// Token: 0x04003FE0 RID: 16352
	[SerializeField]
	private BoxCollider2D colliderB;

	// Token: 0x04003FE1 RID: 16353
	[SerializeField]
	private int dialoguerVariableID = 19;

	// Token: 0x04003FE2 RID: 16354
	[HideInInspector]
	public bool SkipDialogueEvent;
}
