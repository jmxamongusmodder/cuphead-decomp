using UnityEngine;
using Rewired.ComponentControls;
using UnityEngine.UI;

namespace Rewired.ComponentControls.Effects
{
	public class TouchInteractableTransitioner : MonoBehaviour
	{
		private TouchInteractableTransitioner()
		{
		}

		[SerializeField]
		private bool _visible;
		[SerializeField]
		private TouchInteractable.TransitionTypeFlags _transitionType;
		[SerializeField]
		private ColorBlock _transitionColorTint;
		[SerializeField]
		private SpriteState _transitionSpriteState;
		[SerializeField]
		private AnimationTriggers _transitionAnimationTriggers;
		[SerializeField]
		private Graphic _targetGraphic;
		[SerializeField]
		private bool _syncFadeDurationWithTransitionEvent;
		[SerializeField]
		private bool _syncColorTintWithTransitionEvent;
	}
}
