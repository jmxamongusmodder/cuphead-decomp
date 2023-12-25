using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BCB RID: 3019
	[Serializable]
	public class BuiltinDebugViewsModel : PostProcessingModel
	{
		// Token: 0x17000666 RID: 1638
		// (get) Token: 0x060048D4 RID: 18644 RVA: 0x00263935 File Offset: 0x00261D35
		// (set) Token: 0x060048D5 RID: 18645 RVA: 0x0026393D File Offset: 0x00261D3D
		public BuiltinDebugViewsModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
			}
		}

		// Token: 0x17000667 RID: 1639
		// (get) Token: 0x060048D6 RID: 18646 RVA: 0x00263946 File Offset: 0x00261D46
		public bool willInterrupt
		{
			get
			{
				return !this.IsModeActive(BuiltinDebugViewsModel.Mode.None) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.EyeAdaptation) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.LogLut) && !this.IsModeActive(BuiltinDebugViewsModel.Mode.UserLut);
			}
		}

		// Token: 0x060048D7 RID: 18647 RVA: 0x00263986 File Offset: 0x00261D86
		public override void Reset()
		{
			this.settings = BuiltinDebugViewsModel.Settings.defaultSettings;
		}

		// Token: 0x060048D8 RID: 18648 RVA: 0x00263993 File Offset: 0x00261D93
		public bool IsModeActive(BuiltinDebugViewsModel.Mode mode)
		{
			return this.m_Settings.mode == mode;
		}

		// Token: 0x04004EA4 RID: 20132
		[SerializeField]
		private BuiltinDebugViewsModel.Settings m_Settings = BuiltinDebugViewsModel.Settings.defaultSettings;

		// Token: 0x02000BCC RID: 3020
		[Serializable]
		public struct DepthSettings
		{
			// Token: 0x17000668 RID: 1640
			// (get) Token: 0x060048D9 RID: 18649 RVA: 0x002639A4 File Offset: 0x00261DA4
			public static BuiltinDebugViewsModel.DepthSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.DepthSettings
					{
						scale = 1f
					};
				}
			}

			// Token: 0x04004EA5 RID: 20133
			[Range(0f, 1f)]
			[Tooltip("Scales the camera far plane before displaying the depth map.")]
			public float scale;
		}

		// Token: 0x02000BCD RID: 3021
		[Serializable]
		public struct MotionVectorsSettings
		{
			// Token: 0x17000669 RID: 1641
			// (get) Token: 0x060048DA RID: 18650 RVA: 0x002639C8 File Offset: 0x00261DC8
			public static BuiltinDebugViewsModel.MotionVectorsSettings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.MotionVectorsSettings
					{
						sourceOpacity = 1f,
						motionImageOpacity = 0f,
						motionImageAmplitude = 16f,
						motionVectorsOpacity = 1f,
						motionVectorsResolution = 24,
						motionVectorsAmplitude = 64f
					};
				}
			}

			// Token: 0x04004EA6 RID: 20134
			[Range(0f, 1f)]
			[Tooltip("Opacity of the source render.")]
			public float sourceOpacity;

			// Token: 0x04004EA7 RID: 20135
			[Range(0f, 1f)]
			[Tooltip("Opacity of the per-pixel motion vector colors.")]
			public float motionImageOpacity;

			// Token: 0x04004EA8 RID: 20136
			[Min(0f)]
			[Tooltip("Because motion vectors are mainly very small vectors, you can use this setting to make them more visible.")]
			public float motionImageAmplitude;

			// Token: 0x04004EA9 RID: 20137
			[Range(0f, 1f)]
			[Tooltip("Opacity for the motion vector arrows.")]
			public float motionVectorsOpacity;

			// Token: 0x04004EAA RID: 20138
			[Range(8f, 64f)]
			[Tooltip("The arrow density on screen.")]
			public int motionVectorsResolution;

			// Token: 0x04004EAB RID: 20139
			[Min(0f)]
			[Tooltip("Tweaks the arrows length.")]
			public float motionVectorsAmplitude;
		}

		// Token: 0x02000BCE RID: 3022
		public enum Mode
		{
			// Token: 0x04004EAD RID: 20141
			None,
			// Token: 0x04004EAE RID: 20142
			Depth,
			// Token: 0x04004EAF RID: 20143
			Normals,
			// Token: 0x04004EB0 RID: 20144
			MotionVectors,
			// Token: 0x04004EB1 RID: 20145
			AmbientOcclusion,
			// Token: 0x04004EB2 RID: 20146
			EyeAdaptation,
			// Token: 0x04004EB3 RID: 20147
			FocusPlane,
			// Token: 0x04004EB4 RID: 20148
			PreGradingLog,
			// Token: 0x04004EB5 RID: 20149
			LogLut,
			// Token: 0x04004EB6 RID: 20150
			UserLut
		}

		// Token: 0x02000BCF RID: 3023
		[Serializable]
		public struct Settings
		{
			// Token: 0x1700066A RID: 1642
			// (get) Token: 0x060048DB RID: 18651 RVA: 0x00263A24 File Offset: 0x00261E24
			public static BuiltinDebugViewsModel.Settings defaultSettings
			{
				get
				{
					return new BuiltinDebugViewsModel.Settings
					{
						mode = BuiltinDebugViewsModel.Mode.None,
						depth = BuiltinDebugViewsModel.DepthSettings.defaultSettings,
						motionVectors = BuiltinDebugViewsModel.MotionVectorsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04004EB7 RID: 20151
			public BuiltinDebugViewsModel.Mode mode;

			// Token: 0x04004EB8 RID: 20152
			public BuiltinDebugViewsModel.DepthSettings depth;

			// Token: 0x04004EB9 RID: 20153
			public BuiltinDebugViewsModel.MotionVectorsSettings motionVectors;
		}
	}
}
