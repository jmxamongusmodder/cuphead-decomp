using System;

[Serializable]
public struct CoinPositionAndID
{
	public CoinPositionAndID(string id, float pos) : this()
	{
	}

	public string CoinID;
	public float xPos;
}
