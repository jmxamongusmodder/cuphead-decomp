using UnityEngine;

public class SaltbakerLevelPillarHandler : LevelProperties.Saltbaker.Entity
{
	[SerializeField]
	private GameObject leftPillar;
	[SerializeField]
	private GameObject rightPillar;
	[SerializeField]
	private Animator leftAnimator;
	[SerializeField]
	private Animator rightAnimator;
	[SerializeField]
	private SaltbakerLevelHeart darkHeart;
	[SerializeField]
	private GameObject[] smallPlatform;
	[SerializeField]
	private GameObject[] medPlatform;
	[SerializeField]
	private GameObject[] bigPlatform;
	[SerializeField]
	private SaltbakerLevelGlassChunk chunkPrefab;
	[SerializeField]
	private string chunkOrder;
	[SerializeField]
	private string chunkPosition;
	[SerializeField]
	private string chunkSpawnTime;
	public bool suppressCenterPlatforms;
}
