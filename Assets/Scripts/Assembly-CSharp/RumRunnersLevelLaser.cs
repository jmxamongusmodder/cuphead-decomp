using UnityEngine;

public class RumRunnersLevelLaser : AbstractCollidableObject
{
	[SerializeField]
	private SpriteRenderer[] mainRenderers;
	[SerializeField]
	private SpriteRenderer[] warningRenderers;
	[SerializeField]
	private SpriteRenderer[] notesRenderers;
	[SerializeField]
	private CollisionChild[] childColliders;
	[SerializeField]
	private GameObject laserMaskPrefab;
	[SerializeField]
	private Effect sparklesEffect;
}
