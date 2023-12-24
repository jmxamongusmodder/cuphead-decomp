using UnityEngine;

public class BeeLevelBackground : LevelProperties.Bee.Entity
{
	[SerializeField]
	private BeeLevelPlatforms platformGroup;
	[SerializeField]
	private BeeLevelBackgroundGroup[] groups;
	[SerializeField]
	private Transform[] middleGroups;
	[SerializeField]
	private ScrollingSprite back;
}
