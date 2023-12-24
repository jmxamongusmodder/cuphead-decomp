using System;
using UnityEngine;

namespace UnityEngine.PostProcessing
{
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		[Serializable]
		public struct FxaaSettings
		{
			public AntialiasingModel.FxaaPreset preset;
		}

		[Serializable]
		public struct TaaSettings
		{
			public float jitterSpread;
			public float sharpen;
			public float stationaryBlending;
			public float motionBlending;
		}

		[Serializable]
		public struct Settings
		{
			public AntialiasingModel.Method method;
			public AntialiasingModel.FxaaSettings fxaaSettings;
			public AntialiasingModel.TaaSettings taaSettings;
		}

		public enum Method
		{
			Fxaa = 0,
			Taa = 1,
		}

		public enum FxaaPreset
		{
			ExtremePerformance = 0,
			Performance = 1,
			Default = 2,
			Quality = 3,
			ExtremeQuality = 4,
		}

		[SerializeField]
		private Settings m_Settings;
	}
}
