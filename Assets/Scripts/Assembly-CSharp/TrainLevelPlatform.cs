using UnityEngine;

public class TrainLevelPlatform : LevelPlatform
{
	[SerializeField]
	private ParrySwitch rightSwitch;
	[SerializeField]
	private ParrySwitch leftSwitch;
	[SerializeField]
	private Transform[] sparkRoots;
	[SerializeField]
	private Effect sparkEffectPrefab;
}
