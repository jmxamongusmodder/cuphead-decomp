using System;
using UnityEngine;

// Token: 0x02000959 RID: 2393
public class MapNPCProfessional : MonoBehaviour
{
	// Token: 0x060037DD RID: 14301 RVA: 0x00200738 File Offset: 0x001FEB38
	private void Start()
	{
		this.AddDialoguerEvents();
		if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 3f)
		{
			int num = PlayerData.Data.CountLevelsHaveMinGrade(Level.world1BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world2BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world3BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.world4BossLevels, LevelScoringData.Grade.AMinus);
			num += PlayerData.Data.CountLevelsHaveMinGrade(Level.platformingLevels, LevelScoringData.Grade.AMinus);
			if (num >= 15)
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 3f);
				PlayerData.SaveCurrentFile();
			}
			else if (num >= 10)
			{
				if (Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 2f)
				{
					Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 2f);
					PlayerData.SaveCurrentFile();
				}
			}
			else if (num >= 5 && Dialoguer.GetGlobalFloat(this.dialoguerVariableID) < 1f)
			{
				Dialoguer.SetGlobalFloat(this.dialoguerVariableID, 1f);
				PlayerData.SaveCurrentFile();
			}
		}
	}

	// Token: 0x060037DE RID: 14302 RVA: 0x0020084C File Offset: 0x001FEC4C
	private void OnDestroy()
	{
		this.RemoveDialoguerEvents();
	}

	// Token: 0x060037DF RID: 14303 RVA: 0x00200854 File Offset: 0x001FEC54
	public void AddDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037E0 RID: 14304 RVA: 0x0020086C File Offset: 0x001FEC6C
	public void RemoveDialoguerEvents()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037E1 RID: 14305 RVA: 0x00200884 File Offset: 0x001FEC84
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (this.SkipDialogueEvent)
		{
			return;
		}
		if (message == "RetroColorUnlock")
		{
			MapEventNotification.Current.ShowTooltipEvent(TooltipEvent.Professional);
			PlayerData.Data.unlocked2Strip = true;
			PlayerData.SaveCurrentFile();
			MapUI.Current.Refresh();
		}
	}

	// Token: 0x04003FD6 RID: 16342
	[SerializeField]
	private int dialoguerVariableID = 20;

	// Token: 0x04003FD7 RID: 16343
	[HideInInspector]
	public bool SkipDialogueEvent;
}
