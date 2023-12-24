using UnityEngine;

public class MapLevelDependentObstacle : AbstractMapLevelDependentEntity
{
	[SerializeField]
	private GameObject ToEnable;
	[SerializeField]
	private bool seeEnableOnlyDuringTransition;
	[SerializeField]
	private GameObject ToDisable;
	[SerializeField]
	private bool seeDisabledOnlyDuringTransition;
	[SerializeField]
	private Effect poofPrefab;
	[SerializeField]
	private Material flashMaterial;
	[SerializeField]
	private Transform poofRoot;
	[SerializeField]
	private bool DontPlayPoofSFX;
}
