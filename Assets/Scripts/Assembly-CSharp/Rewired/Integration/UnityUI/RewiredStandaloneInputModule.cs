using UnityEngine.EventSystems;
using UnityEngine;

namespace Rewired.Integration.UnityUI
{
	public class RewiredStandaloneInputModule : PointerInputModule
	{
		public override void Process()
		{
		}

		[SerializeField]
		private bool useAllRewiredGamePlayers;
		[SerializeField]
		private bool useRewiredSystemPlayer;
		[SerializeField]
		private int[] rewiredPlayerIds;
		[SerializeField]
		private bool usePlayingPlayersOnly;
		[SerializeField]
		private bool moveOneElementPerAxisPress;
		[SerializeField]
		private string m_HorizontalAxis;
		[SerializeField]
		private string m_VerticalAxis;
		[SerializeField]
		private string m_SubmitButton;
		[SerializeField]
		private string m_CancelButton;
		[SerializeField]
		private float m_InputActionsPerSecond;
		[SerializeField]
		private float m_RepeatDelay;
		[SerializeField]
		private bool m_allowMouseInput;
		[SerializeField]
		private bool m_allowMouseInputIfTouchSupported;
		[SerializeField]
		private bool m_ForceModuleActive;
	}
}
