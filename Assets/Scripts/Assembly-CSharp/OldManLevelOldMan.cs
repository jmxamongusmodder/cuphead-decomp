using UnityEngine;

public class OldManLevelOldMan : LevelProperties.OldMan.Entity
{
	[SerializeField]
	private OldManLevelGoose goosePrefab;
	[SerializeField]
	private OldManLevelBear bearBeam;
	[SerializeField]
	private Transform spitRoot;
	[SerializeField]
	private Transform spitEndArc;
	[SerializeField]
	private OldManLevelSpitProjectile spitProjectile;
	[SerializeField]
	private OldManLevelSpitProjectile spitProjectilePink;
	[SerializeField]
	private OldManLevelPlatformManager platformManager;
	[SerializeField]
	private OldManLevelSockPuppetHandler sockPuppets;
	[SerializeField]
	private SpriteRenderer eyeRenderer;
	[SerializeField]
	private GameObject cauldron;
	[SerializeField]
	private GameObject cauldronEyes;
	[SerializeField]
	private Animator gooseFXAnimator;
	[SerializeField]
	private GameObject rightWall;
}
