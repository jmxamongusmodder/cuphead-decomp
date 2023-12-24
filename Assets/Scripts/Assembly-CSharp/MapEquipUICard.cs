using UnityEngine;
using UnityEngine.UI;

public class MapEquipUICard : AbstractMonoBehaviour
{
	public RectTransform container;
	[SerializeField]
	private MapEquipUICardFront front;
	[SerializeField]
	private MapEquipUICardBackSelect backSelect;
	[SerializeField]
	private MapEquipUICardBackReady backReady;
	[SerializeField]
	private MapEquipUIChecklist checkList;
	public Image[] cupheadImages;
	public Image[] mugmanImages;
	[SerializeField]
	private GameObject cupheadChaos;
	[SerializeField]
	private GameObject mugmanChaos;
	[SerializeField]
	private GameObject cuphead2POverlay;
	[SerializeField]
	private GameObject mugman1POverlay;
}
