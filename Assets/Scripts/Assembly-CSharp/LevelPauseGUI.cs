using UnityEngine.UI;
using UnityEngine;

public class LevelPauseGUI : AbstractPauseGUI
{
	[SerializeField]
	private Text[] menuItems;
	[SerializeField]
	private LocalizationHelper retryLocHelper;
}
