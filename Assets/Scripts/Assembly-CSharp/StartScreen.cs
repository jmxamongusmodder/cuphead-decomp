using UnityEngine;

public class StartScreen : AbstractMonoBehaviour
{
	public class InitialLoadData
	{
		public bool forceOriginalTitleScreen;
	}

	public AudioClip[] SelectSound;
	[SerializeField]
	private Animator mdhrSplash;
	[SerializeField]
	private SpriteRenderer fader;
	[SerializeField]
	private GameObject titleAnimation;
	[SerializeField]
	private GameObject titleAnimationDLC;
}
