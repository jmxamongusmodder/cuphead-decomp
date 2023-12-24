using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MapEquipUICardBackSelect : AbstractMapEquipUICardSide
{
	public bool lockInput;
	[SerializeField]
	private LocalizationHelper headerText;
	[SerializeField]
	private Text titleText;
	[SerializeField]
	private Text exText;
	[SerializeField]
	private TMP_Text descriptionText;
	[SerializeField]
	private MapEquipUICursor cursor;
	[SerializeField]
	private MapEquipUICardBackSelectSelectionCursor selectionCursor;
	[SerializeField]
	private Image iconsBack;
	[SerializeField]
	private Image superIconsBack;
	[SerializeField]
	private Image DLCIconsBack;
	[SerializeField]
	private MapEquipUICardBackSelectIcon[] normalIcons;
	[SerializeField]
	private MapEquipUICardBackSelectIcon[] superIcons;
	[SerializeField]
	private MapEquipUICardBackSelectIcon[] DLCIcons;
}
