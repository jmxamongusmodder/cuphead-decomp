using System;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Rewired.ComponentControls
{
	[Serializable]
	public class TouchInteractable : TouchControl
	{
		public class InteractionStateTransitionArgs
		{
		}

		[Serializable]
		public class InteractionStateTransitionEventHandler : UnityEvent<TouchInteractable.InteractionStateTransitionArgs>
		{
		}

		[Serializable]
		public class VisibilityChangedEventHandler : UnityEvent<bool>
		{
		}

		public enum MouseButtonFlags
		{
			None = 0,
			LeftButton = 1,
			RightButton = 2,
			MiddleButton = 4,
			AnyButton = -1,
		}

		public enum TransitionTypeFlags
		{
			None = 0,
			ColorTint = 1,
			SpriteSwap = 2,
			Animation = 4,
		}

		[SerializeField]
		private bool _interactable;
		[SerializeField]
		private bool _visible;
		[SerializeField]
		private bool _hideWhenIdle;
		[SerializeField]
		private MouseButtonFlags _allowedMouseButtons;
		[SerializeField]
		private TransitionTypeFlags _transitionType;
		[SerializeField]
		private ColorBlock _transitionColorTint;
		[SerializeField]
		private SpriteState _transitionSpriteState;
		[SerializeField]
		private AnimationTriggers _transitionAnimationTriggers;
		[SerializeField]
		private Graphic _targetGraphic;
		[SerializeField]
		private InteractionStateTransitionEventHandler _onInteractionStateTransition;
		[SerializeField]
		private VisibilityChangedEventHandler _onVisibilityChanged;
		[SerializeField]
		private UnityEvent _onInteractionStateChangedToNormal;
		[SerializeField]
		private UnityEvent _onInteractionStateChangedToHighlighted;
		[SerializeField]
		private UnityEvent _onInteractionStateChangedToPressed;
		[SerializeField]
		private UnityEvent _onInteractionStateChangedToDisabled;
	}
}
