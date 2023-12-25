using System;
using UnityEngine;

// Token: 0x02000955 RID: 2389
public class MapNPCMusic : MonoBehaviour
{
	// Token: 0x060037D1 RID: 14289 RVA: 0x00200417 File Offset: 0x001FE817
	private void Start()
	{
		Dialoguer.events.onMessageEvent += this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037D2 RID: 14290 RVA: 0x0020042F File Offset: 0x001FE82F
	private void OnDestroy()
	{
		Dialoguer.events.onMessageEvent -= this.OnDialoguerMessageEvent;
	}

	// Token: 0x060037D3 RID: 14291 RVA: 0x00200448 File Offset: 0x001FE848
	private void OnDialoguerMessageEvent(string message, string metadata)
	{
		if (message == "MinimalistMusic" && this.musicType == MapNPCMusic.MusicType.Minimalist)
		{
			PlayerData.Data.pianoAudioEnabled = true;
			PlayerData.SaveCurrentFile();
			Map.Current.OnNPCChangeMusic();
		}
		else if (message == "RegularMusic" && this.musicType == MapNPCMusic.MusicType.Regular)
		{
			PlayerData.Data.pianoAudioEnabled = false;
			PlayerData.SaveCurrentFile();
			Map.Current.OnNPCChangeMusic();
		}
	}

	// Token: 0x04003FCC RID: 16332
	[SerializeField]
	private MapNPCMusic.MusicType musicType;

	// Token: 0x02000956 RID: 2390
	public enum MusicType
	{
		// Token: 0x04003FCE RID: 16334
		Regular,
		// Token: 0x04003FCF RID: 16335
		Minimalist
	}
}
