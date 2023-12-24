using UnityEngine;
using UnityEngine.UI;

public class WinScreenTicker : AbstractMonoBehaviour
{
	public enum TickerType
	{
		Time = 0,
		Health = 1,
		Score = 2,
		Stars = 3,
	}

	public TickerType tickerType;
	[SerializeField]
	private Animator[] stars;
	[SerializeField]
	private Text[] leaderDots;
	[SerializeField]
	private Text label;
	[SerializeField]
	private Text valueText;
}
