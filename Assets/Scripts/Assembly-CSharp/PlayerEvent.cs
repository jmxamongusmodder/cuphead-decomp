using System;

// Token: 0x02000AC7 RID: 2759
public abstract class PlayerEvent<T> : GameEvent where T : PlayerEvent<T>, new()
{
	// Token: 0x0600423D RID: 16957 RVA: 0x0023C31D File Offset: 0x0023A71D
	public PlayerEvent()
	{
	}

	// Token: 0x170005CE RID: 1486
	// (get) Token: 0x0600423E RID: 16958 RVA: 0x0023C325 File Offset: 0x0023A725
	// (set) Token: 0x0600423F RID: 16959 RVA: 0x0023C32D File Offset: 0x0023A72D
	public PlayerId playerId { get; private set; }

	// Token: 0x06004240 RID: 16960 RVA: 0x0023C336 File Offset: 0x0023A736
	public static T Shared(PlayerId playerId)
	{
		if (PlayerEvent<T>._instance == null)
		{
			PlayerEvent<T>._instance = Activator.CreateInstance<T>();
		}
		PlayerEvent<T>._instance.playerId = playerId;
		return PlayerEvent<T>._instance;
	}

	// Token: 0x040048B1 RID: 18609
	public static T _instance;
}
