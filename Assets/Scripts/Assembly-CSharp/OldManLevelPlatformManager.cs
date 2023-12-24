using UnityEngine;

public class OldManLevelPlatformManager : LevelProperties.OldMan.Entity
{
	[SerializeField]
	private OldManLevelPlatform[] allPlatforms;
	[SerializeField]
	private Animator[] beardSettles;
	[SerializeField]
	private SpriteRenderer mainBeardTufts;
	[SerializeField]
	private float wobbleBeforeRemoveTime;
}
