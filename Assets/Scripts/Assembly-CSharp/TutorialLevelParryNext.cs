using UnityEngine;

public class TutorialLevelParryNext : AbstractCollidableObject
{
	[SerializeField]
	private TutorialLevelParryNext nextSphere;
	[SerializeField]
	private SpriteRenderer spriteRenderer;
	[SerializeField]
	private Sprite normalSprite;
	[SerializeField]
	private Sprite parrySprite;
	[SerializeField]
	private Material normalMaterial;
	[SerializeField]
	private Material parryMaterial;
	[SerializeField]
	private bool startAsParry;
	[SerializeField]
	private ParrySwitch parrySwitch;
}
