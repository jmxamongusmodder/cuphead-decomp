using System;

// Token: 0x0200059D RID: 1437
public abstract class AbstractDicePalaceLevel : Level
{
	// Token: 0x1700035A RID: 858
	// (get) Token: 0x06001B89 RID: 7049
	public abstract DicePalaceLevels CurrentDicePalaceLevel { get; }

	// Token: 0x06001B8A RID: 7050 RVA: 0x0005E564 File Offset: 0x0005C964
	protected override void Awake()
	{
		base.Awake();
		if (DicePalaceMainLevelGameInfo.GameInfo != null)
		{
			Level.Current.OnLoseEvent += DicePalaceMainLevelGameInfo.GameInfo.CleanUp;
		}
		base.OnLoseEvent += this.ResetScore;
	}

	// Token: 0x06001B8B RID: 7051 RVA: 0x0005E5B3 File Offset: 0x0005C9B3
	protected override void OnDestroy()
	{
		base.OnDestroy();
		base.OnLoseEvent -= this.ResetScore;
	}

	// Token: 0x06001B8C RID: 7052 RVA: 0x0005E5CD File Offset: 0x0005C9CD
	private void ResetScore()
	{
		base.OnLoseEvent -= this.ResetScore;
		base.CleanUpScore();
	}

	// Token: 0x06001B8D RID: 7053 RVA: 0x0005E5E7 File Offset: 0x0005C9E7
	protected override void CheckIfInABossesHub()
	{
		base.CheckIfInABossesHub();
		if (!Level.IsTowerOfPower)
		{
			Level.IsDicePalace = true;
		}
	}
}
