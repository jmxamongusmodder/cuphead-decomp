using UnityEngine;

public class AnimationHelper : AbstractMonoBehaviour
{
	[SerializeField]
	private CupheadTime.Layer layer;
	[SerializeField]
	private float speed;
	[SerializeField]
	private bool ignoreGlobal;
	[SerializeField]
	private bool autoUpdate;
}
