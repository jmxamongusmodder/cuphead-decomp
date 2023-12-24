using UnityEngine;
using System.Collections.Generic;

public class MapEquipUIChecklist : AbstractMapEquipUICardSide
{
	[SerializeField]
	private GameObject worldTop;
	[SerializeField]
	private GameObject finaleTop;
	[SerializeField]
	private GameObject localizedTop;
	[SerializeField]
	private GameObject worldTopLocalized;
	[SerializeField]
	private GameObject worldTopDLCLocalized;
	[SerializeField]
	private GameObject finaleTopLocalized;
	[SerializeField]
	private MapEquipUICursor cursor;
	[SerializeField]
	private MapEquipUICardChecklistIcon[] worldSelectionIcons;
	[SerializeField]
	private List<MapEquipUIChecklistItem> checklistItems;
	[SerializeField]
	private List<MapEquipUIChecklistItem> finaleItems;
	[SerializeField]
	private GameObject finaleGrid;
	[SerializeField]
	private GameObject rightArrow;
	[SerializeField]
	private GameObject leftArrow;
}
