using UnityEngine;
using System;

namespace Rewired.UI.ControlMapper
{
	public class CanvasScalerFitter : MonoBehaviour
	{
		[Serializable]
		private class BreakPoint
		{
			[SerializeField]
			public string name;
			[SerializeField]
			public float screenAspectRatio;
			[SerializeField]
			public Vector2 referenceResolution;
		}

		[SerializeField]
		private BreakPoint[] breakPoints;
	}
}
