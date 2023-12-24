using System.Collections.Generic;
using UnityEngine;

public class DragonLevelPlatformManager : AbstractPausableComponent
{
	[SerializeField]
	public List<DragonLevelCloudPlatform> platforms;
	[SerializeField]
	private DragonLevelCloudPlatform platformPrefab;
}
