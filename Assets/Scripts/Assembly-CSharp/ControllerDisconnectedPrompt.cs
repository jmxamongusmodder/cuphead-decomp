using UnityEngine.UI;
using UnityEngine;

public class ControllerDisconnectedPrompt : InterruptingPrompt
{
	public PlayerId currentPlayer;
	public bool allowedToShow;
	[SerializeField]
	private Text playerText;
	[SerializeField]
	private LocalizationHelper localizationHelper;
}
