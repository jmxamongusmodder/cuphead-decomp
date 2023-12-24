using Rewired.UI.ControlMapper;
using UnityEngine;
using UnityEngine.UI;

public class CustomButtonPlayerSelect : CustomButton
{
	public ControlMapper mapper;
	[SerializeField]
	private ButtonInfo myInfo;
	[SerializeField]
	private Image[] selectionTabs;
}
