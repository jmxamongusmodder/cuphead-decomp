using System;
using UnityEngine;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class BloomModel : PostProcessingModel
	{
		[Serializable]
		public struct BloomSettings
		{
			public float intensity;
			public float threshold;
			public float softKnee;
			public float radius;
			public bool antiFlicker;
		}

		[Serializable]
		public struct LensDirtSettings
		{
			public Texture texture;
			public float intensity;
		}

		[Serializable]
		public struct Settings
		{
			public BloomModel.BloomSettings bloom;
			public BloomModel.LensDirtSettings lensDirt;
		}

		[SerializeField]
		private Settings m_Settings;
	}
}
