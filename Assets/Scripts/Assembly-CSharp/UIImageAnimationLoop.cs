using UnityEngine;

public class UIImageAnimationLoop : AbstractMonoBehaviour
{
	public enum Mode
	{
		Linear = 0,
		Shuffle = 1,
		Random = 2,
	}

	[SerializeField]
	private Mode mode;
	[SerializeField]
	private float frameDelay;
	[SerializeField]
	private Sprite[] sprites;
	[SerializeField]
	private bool IgnoreGlobalTime;
}
