using UnityEngine;

public class FlyingBirdLevelNurses : AbstractCollidableObject
{
	[SerializeField]
	private AbstractProjectile pillPrefab;
	[SerializeField]
	private Transform shootRightPosRoot;
	[SerializeField]
	private Transform shootLeftPosRoot;
	[SerializeField]
	private GameObject spitFXLeft;
	[SerializeField]
	private GameObject spitFXRight;
	public Transform[] nurses;
}
