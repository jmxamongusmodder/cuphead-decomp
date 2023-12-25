using System;

// Token: 0x020004A0 RID: 1184
[Serializable]
public struct CoinPositionAndID
{
	// Token: 0x06001349 RID: 4937 RVA: 0x000AAA53 File Offset: 0x000A8E53
	public CoinPositionAndID(string id, float pos)
	{
		this.CoinID = id;
		this.xPos = pos;
	}

	// Token: 0x04001C70 RID: 7280
	public string CoinID;

	// Token: 0x04001C71 RID: 7281
	public float xPos;
}
