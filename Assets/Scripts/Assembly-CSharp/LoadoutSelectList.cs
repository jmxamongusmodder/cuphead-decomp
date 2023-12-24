using UnityEngine;
using UnityEngine.UI;

public class LoadoutSelectList : AbstractMonoBehaviour
{
	public enum Mode
	{
		Primary = 0,
		Secondary = 1,
		Super = 2,
		Charm = 3,
		Difficulty = 4,
	}

	[SerializeField]
	public Mode mode;
	public Button button;
	public Button selectedButton;
	public RectTransform contentPanel;
}
