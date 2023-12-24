using UnityEngine;

public class HitFlash : AbstractMonoBehaviour
{
	[SerializeField]
	private Color damageColor;
	[SerializeField]
	private DamageReceiver damageReceiver;
	[SerializeField]
	private bool includeSelf;
	public SpriteRenderer[] otherRenderers;
}
