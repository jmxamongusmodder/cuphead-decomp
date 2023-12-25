using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000714 RID: 1812
public class OldManLevelSplashHandler : AbstractPausableComponent
{
	// Token: 0x06002766 RID: 10086 RVA: 0x00171F90 File Offset: 0x00170390
	public void SplashOut(float posX)
	{
		this.splashOut.Create(new Vector3(posX, base.transform.position.y));
	}

	// Token: 0x06002767 RID: 10087 RVA: 0x00171FC4 File Offset: 0x001703C4
	public void SplashIn(float posX)
	{
		this.splashIn.Create(new Vector3(posX, base.transform.position.y));
	}

	// Token: 0x06002768 RID: 10088 RVA: 0x00171FF8 File Offset: 0x001703F8
	private void Update()
	{
		Dictionary<int, AbstractPlayerController>.ValueCollection allPlayers = PlayerManager.GetAllPlayers();
		for (int i = 0; i < 2; i++)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer((PlayerId)i);
			if (player == null || player.IsDead)
			{
				this.lastKnownPlayerPos[i] = Vector3.zero;
			}
			else
			{
				if (this.lastKnownPlayerPos[i].y < base.transform.position.y)
				{
					if (player.transform.position.y > base.transform.position.y)
					{
					}
				}
				else if (player.transform.position.y <= base.transform.position.y)
				{
					this.SplashIn(player.transform.position.x);
					this.SFX_PlayerSplashIn();
				}
				this.lastKnownPlayerPos[i] = player.transform.position;
			}
		}
	}

	// Token: 0x06002769 RID: 10089 RVA: 0x0017211A File Offset: 0x0017051A
	private void SFX_PlayerSplashIn()
	{
		AudioManager.Play("sfx_dlc_omm_p3_stomachacid_splash");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p3_stomachacid_splash");
	}

	// Token: 0x04003022 RID: 12322
	[SerializeField]
	private Effect splashIn;

	// Token: 0x04003023 RID: 12323
	[SerializeField]
	private Effect splashOut;

	// Token: 0x04003024 RID: 12324
	private Vector3[] lastKnownPlayerPos = new Vector3[]
	{
		Vector3.zero,
		Vector3.zero
	};
}
