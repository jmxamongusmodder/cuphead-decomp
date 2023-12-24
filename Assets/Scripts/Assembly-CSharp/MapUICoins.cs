using UnityEngine;
using UnityEngine.UI;

public class MapUICoins : MonoBehaviour
{
	[SerializeField]
	private PlayerId playerId;
	[SerializeField]
	private Image coinImage;
	[SerializeField]
	private Image currencyNbImage;
	[SerializeField]
	private Sprite[] coinSprites;
	[SerializeField]
	private Transform doubleDigitCoinTransform;
}
