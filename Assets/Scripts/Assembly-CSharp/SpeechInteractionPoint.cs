using System;
using UnityEngine;

// Token: 0x0200042D RID: 1069
public class SpeechInteractionPoint : AbstractLevelInteractiveEntity
{
	// Token: 0x06000F9B RID: 3995 RVA: 0x00097BEE File Offset: 0x00095FEE
	protected override void Awake()
	{
		base.Awake();
		this.dialogueProperties.text = "Talk";
		this.isDisabledP1 = false;
	}

	// Token: 0x06000F9C RID: 3996 RVA: 0x00097C10 File Offset: 0x00096010
	protected override void Check()
	{
		base.Check();
		if (base.PlayerWithinDistance(PlayerId.PlayerOne))
		{
			PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent<LevelPlayerMotor>().DisableJump();
			this.isDisabledP1 = true;
		}
		else if (base.PlayerWithinDistance(PlayerId.PlayerTwo))
		{
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent<LevelPlayerMotor>().DisableJump();
			this.isDisabledP2 = true;
		}
		else if (this.isDisabledP1)
		{
			PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent<LevelPlayerMotor>().EnableJump();
			this.isDisabledP1 = false;
		}
		else if (this.isDisabledP2)
		{
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent<LevelPlayerMotor>().EnableJump();
			this.isDisabledP2 = false;
		}
	}

	// Token: 0x06000F9D RID: 3997 RVA: 0x00097CBC File Offset: 0x000960BC
	protected override void Activate()
	{
		base.Activate();
		if (!this.activated)
		{
			if (base.PlayerWithinDistance(PlayerId.PlayerOne))
			{
				this.Show(PlayerId.PlayerOne);
			}
			if (base.PlayerWithinDistance(PlayerId.PlayerTwo) && PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
			{
				this.Show(PlayerId.PlayerTwo);
			}
			this.activated = true;
		}
	}

	// Token: 0x040018CE RID: 6350
	[SerializeField]
	protected string[] allDialogue;

	// Token: 0x040018CF RID: 6351
	private bool activated;

	// Token: 0x040018D0 RID: 6352
	private bool isDisabledP1;

	// Token: 0x040018D1 RID: 6353
	private bool isDisabledP2;
}
