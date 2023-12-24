using UnityEngine;

public class BaronessLevelJawbreaker : BaronessLevelMiniBossBase
{
	[SerializeField]
	private Transform sprite;
	[SerializeField]
	private BaronessLevelJawbreakerMini miniBluePrefab;
	[SerializeField]
	private BaronessLevelJawbreakerMini miniRedPrefab;
	[SerializeField]
	private Transform followPoint;
	[SerializeField]
	private BaronessLevelJawbreakerGhost ghostPrefab;
}
