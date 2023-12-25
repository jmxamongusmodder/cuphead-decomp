using System;
using UnityEngine;

// Token: 0x0200096C RID: 2412
public class MapDLC : Map
{
	// Token: 0x06003837 RID: 14391 RVA: 0x00202E63 File Offset: 0x00201263
	protected override void SelectMusic()
	{
		this.currentMusic = -2;
		this.CheckMusic(false);
		this.CheckIfBossesCompleted();
	}

	// Token: 0x06003838 RID: 14392 RVA: 0x00202E7C File Offset: 0x0020127C
	protected override void CheckMusic(bool isRecheck)
	{
		int num = this.currentMusic;
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne);
		PlayerData.PlayerLoadouts.PlayerLoadout playerLoadout2 = PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo);
		if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerOne) > -1) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.CalculateLevel(PlayerId.PlayerTwo) > -1))
		{
			if ((playerLoadout.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerOne)) || (PlayerManager.Multiplayer && playerLoadout2.charm == Charm.charm_curse && CharmCurse.IsMaxLevel(PlayerId.PlayerTwo)))
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 3 : 5);
			}
			else
			{
				num = ((!PlayerData.Data.pianoAudioEnabled) ? 2 : 4);
			}
		}
		else if (PlayerData.Data.pianoAudioEnabled)
		{
			num = 1;
		}
		else
		{
			num = ((!MapDLC.haveVisited) ? -1 : 0);
			MapDLC.haveVisited = true;
		}
		if ((this.currentMusic == -1 && num == 0) || (this.currentMusic == 0 && num == -1))
		{
			return;
		}
		if (num != this.currentMusic)
		{
			this.currentMusic = num;
			if (this.currentMusic == -1)
			{
				AudioManager.PlayBGM();
			}
			else
			{
				AudioManager.StartBGMAlternate(this.currentMusic);
			}
		}
	}

	// Token: 0x06003839 RID: 14393 RVA: 0x00202FF3 File Offset: 0x002013F3
	protected override void OnPlayerJoined(PlayerId playerId)
	{
		base.OnPlayerJoined(playerId);
		this.CheckMusic(true);
	}

	// Token: 0x0600383A RID: 14394 RVA: 0x00203003 File Offset: 0x00201403
	protected override void OnPlayerLeave(PlayerId playerId)
	{
		base.OnPlayerLeave(playerId);
		this.CheckMusic(true);
	}

	// Token: 0x0600383B RID: 14395 RVA: 0x00203013 File Offset: 0x00201413
	private void CheckIfBossesCompleted()
	{
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.worldDLCBossLevels, Level.Mode.Normal))
		{
			this.bakerySoundLoop.gameObject.SetActive(false);
		}
		else
		{
			this.bakerySoundLoop.Play();
		}
	}

	// Token: 0x04004014 RID: 16404
	private static bool haveVisited;

	// Token: 0x04004015 RID: 16405
	[SerializeField]
	private AudioSource bakerySoundLoop;
}
