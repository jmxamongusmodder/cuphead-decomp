using System;
using UnityEngine;

namespace Rewired.ComponentControls.Data
{
	[Serializable]
	public class CustomControllerSelector
	{
		[SerializeField]
		private bool _findUsingSourceId;
		[SerializeField]
		private int _sourceId;
		[SerializeField]
		private bool _findUsingTag;
		[SerializeField]
		private string _tag;
		[SerializeField]
		private bool _findInPlayer;
		[SerializeField]
		private int _playerId;
	}
}
