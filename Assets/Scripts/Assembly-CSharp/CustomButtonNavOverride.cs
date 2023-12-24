using Rewired.UI.ControlMapper;
using UnityEngine.UI;
using UnityEngine;

public class CustomButtonNavOverride : CustomButton
{
	[SerializeField]
	private Selectable upOnSinglePlayer;
	[SerializeField]
	private Selectable downOnSinglePlayer;
	[SerializeField]
	private Selectable upOnMultiPlayer;
	[SerializeField]
	private Selectable downOnMultiPlayer;
	[SerializeField]
	private ControlMapper mapper;
}
