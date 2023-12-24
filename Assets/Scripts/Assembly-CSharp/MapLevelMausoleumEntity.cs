using UnityEngine;

public class MapLevelMausoleumEntity : AbstractMapLevelDependentEntity
{
	[SerializeField]
	private GameObject ToEnable;
	[SerializeField]
	private GameObject ToDisable;
	[SerializeField]
	private Effect poofPrefab;
	[SerializeField]
	private Transform poofRoot;
	[SerializeField]
	private Super superUnlock;
}
