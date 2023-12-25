using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BD2 RID: 3026
	[Serializable]
	public class ColorGradingModel : PostProcessingModel
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060048E2 RID: 18658 RVA: 0x00263AC9 File Offset: 0x00261EC9
		// (set) Token: 0x060048E3 RID: 18659 RVA: 0x00263AD1 File Offset: 0x00261ED1
		public ColorGradingModel.Settings settings
		{
			get
			{
				return this.m_Settings;
			}
			set
			{
				this.m_Settings = value;
				this.OnValidate();
			}
		}

		// Token: 0x1700066E RID: 1646
		// (get) Token: 0x060048E4 RID: 18660 RVA: 0x00263AE0 File Offset: 0x00261EE0
		// (set) Token: 0x060048E5 RID: 18661 RVA: 0x00263AE8 File Offset: 0x00261EE8
		public bool isDirty { get; internal set; }

		// Token: 0x1700066F RID: 1647
		// (get) Token: 0x060048E6 RID: 18662 RVA: 0x00263AF1 File Offset: 0x00261EF1
		// (set) Token: 0x060048E7 RID: 18663 RVA: 0x00263AF9 File Offset: 0x00261EF9
		public RenderTexture bakedLut { get; internal set; }

		// Token: 0x060048E8 RID: 18664 RVA: 0x00263B02 File Offset: 0x00261F02
		public override void Reset()
		{
			this.m_Settings = ColorGradingModel.Settings.defaultSettings;
			this.OnValidate();
		}

		// Token: 0x060048E9 RID: 18665 RVA: 0x00263B15 File Offset: 0x00261F15
		public override void OnValidate()
		{
			this.isDirty = true;
		}

		// Token: 0x04004EBD RID: 20157
		[SerializeField]
		private ColorGradingModel.Settings m_Settings = ColorGradingModel.Settings.defaultSettings;

		// Token: 0x02000BD3 RID: 3027
		public enum Tonemapper
		{
			// Token: 0x04004EC1 RID: 20161
			None,
			// Token: 0x04004EC2 RID: 20162
			ACES,
			// Token: 0x04004EC3 RID: 20163
			Neutral
		}

		// Token: 0x02000BD4 RID: 3028
		[Serializable]
		public struct TonemappingSettings
		{
			// Token: 0x17000670 RID: 1648
			// (get) Token: 0x060048EA RID: 18666 RVA: 0x00263B20 File Offset: 0x00261F20
			public static ColorGradingModel.TonemappingSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.TonemappingSettings
					{
						tonemapper = ColorGradingModel.Tonemapper.Neutral,
						neutralBlackIn = 0.02f,
						neutralWhiteIn = 10f,
						neutralBlackOut = 0f,
						neutralWhiteOut = 10f,
						neutralWhiteLevel = 5.3f,
						neutralWhiteClip = 10f
					};
				}
			}

			// Token: 0x04004EC4 RID: 20164
			[Tooltip("Tonemapping algorithm to use at the end of the color grading process. Use \"Neutral\" if you need a customizable tonemapper or \"Filmic\" to give a standard filmic look to your scenes.")]
			public ColorGradingModel.Tonemapper tonemapper;

			// Token: 0x04004EC5 RID: 20165
			[Range(-0.1f, 0.1f)]
			public float neutralBlackIn;

			// Token: 0x04004EC6 RID: 20166
			[Range(1f, 20f)]
			public float neutralWhiteIn;

			// Token: 0x04004EC7 RID: 20167
			[Range(-0.09f, 0.1f)]
			public float neutralBlackOut;

			// Token: 0x04004EC8 RID: 20168
			[Range(1f, 19f)]
			public float neutralWhiteOut;

			// Token: 0x04004EC9 RID: 20169
			[Range(0.1f, 20f)]
			public float neutralWhiteLevel;

			// Token: 0x04004ECA RID: 20170
			[Range(1f, 10f)]
			public float neutralWhiteClip;
		}

		// Token: 0x02000BD5 RID: 3029
		[Serializable]
		public struct BasicSettings
		{
			// Token: 0x17000671 RID: 1649
			// (get) Token: 0x060048EB RID: 18667 RVA: 0x00263B88 File Offset: 0x00261F88
			public static ColorGradingModel.BasicSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.BasicSettings
					{
						postExposure = 0f,
						temperature = 0f,
						tint = 0f,
						hueShift = 0f,
						saturation = 1f,
						contrast = 1f
					};
				}
			}

			// Token: 0x04004ECB RID: 20171
			[Tooltip("Adjusts the overall exposure of the scene in EV units. This is applied after HDR effect and right before tonemapping so it won't affect previous effects in the chain.")]
			public float postExposure;

			// Token: 0x04004ECC RID: 20172
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to a custom color temperature.")]
			public float temperature;

			// Token: 0x04004ECD RID: 20173
			[Range(-100f, 100f)]
			[Tooltip("Sets the white balance to compensate for a green or magenta tint.")]
			public float tint;

			// Token: 0x04004ECE RID: 20174
			[Range(-180f, 180f)]
			[Tooltip("Shift the hue of all colors.")]
			public float hueShift;

			// Token: 0x04004ECF RID: 20175
			[Range(0f, 2f)]
			[Tooltip("Pushes the intensity of all colors.")]
			public float saturation;

			// Token: 0x04004ED0 RID: 20176
			[Range(0f, 2f)]
			[Tooltip("Expands or shrinks the overall range of tonal values.")]
			public float contrast;
		}

		// Token: 0x02000BD6 RID: 3030
		[Serializable]
		public struct ChannelMixerSettings
		{
			// Token: 0x17000672 RID: 1650
			// (get) Token: 0x060048EC RID: 18668 RVA: 0x00263BE8 File Offset: 0x00261FE8
			public static ColorGradingModel.ChannelMixerSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ChannelMixerSettings
					{
						red = new Vector3(1f, 0f, 0f),
						green = new Vector3(0f, 1f, 0f),
						blue = new Vector3(0f, 0f, 1f),
						currentEditingChannel = 0
					};
				}
			}

			// Token: 0x04004ED1 RID: 20177
			public Vector3 red;

			// Token: 0x04004ED2 RID: 20178
			public Vector3 green;

			// Token: 0x04004ED3 RID: 20179
			public Vector3 blue;

			// Token: 0x04004ED4 RID: 20180
			[HideInInspector]
			public int currentEditingChannel;
		}

		// Token: 0x02000BD7 RID: 3031
		[Serializable]
		public struct LogWheelsSettings
		{
			// Token: 0x17000673 RID: 1651
			// (get) Token: 0x060048ED RID: 18669 RVA: 0x00263C58 File Offset: 0x00262058
			public static ColorGradingModel.LogWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LogWheelsSettings
					{
						slope = Color.clear,
						power = Color.clear,
						offset = Color.clear
					};
				}
			}

			// Token: 0x04004ED5 RID: 20181
			[Trackball("GetSlopeValue")]
			public Color slope;

			// Token: 0x04004ED6 RID: 20182
			[Trackball("GetPowerValue")]
			public Color power;

			// Token: 0x04004ED7 RID: 20183
			[Trackball("GetOffsetValue")]
			public Color offset;
		}

		// Token: 0x02000BD8 RID: 3032
		[Serializable]
		public struct LinearWheelsSettings
		{
			// Token: 0x17000674 RID: 1652
			// (get) Token: 0x060048EE RID: 18670 RVA: 0x00263C94 File Offset: 0x00262094
			public static ColorGradingModel.LinearWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.LinearWheelsSettings
					{
						lift = Color.clear,
						gamma = Color.clear,
						gain = Color.clear
					};
				}
			}

			// Token: 0x04004ED8 RID: 20184
			[Trackball("GetLiftValue")]
			public Color lift;

			// Token: 0x04004ED9 RID: 20185
			[Trackball("GetGammaValue")]
			public Color gamma;

			// Token: 0x04004EDA RID: 20186
			[Trackball("GetGainValue")]
			public Color gain;
		}

		// Token: 0x02000BD9 RID: 3033
		public enum ColorWheelMode
		{
			// Token: 0x04004EDC RID: 20188
			Linear,
			// Token: 0x04004EDD RID: 20189
			Log
		}

		// Token: 0x02000BDA RID: 3034
		[Serializable]
		public struct ColorWheelsSettings
		{
			// Token: 0x17000675 RID: 1653
			// (get) Token: 0x060048EF RID: 18671 RVA: 0x00263CD0 File Offset: 0x002620D0
			public static ColorGradingModel.ColorWheelsSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.ColorWheelsSettings
					{
						mode = ColorGradingModel.ColorWheelMode.Log,
						log = ColorGradingModel.LogWheelsSettings.defaultSettings,
						linear = ColorGradingModel.LinearWheelsSettings.defaultSettings
					};
				}
			}

			// Token: 0x04004EDE RID: 20190
			public ColorGradingModel.ColorWheelMode mode;

			// Token: 0x04004EDF RID: 20191
			[TrackballGroup]
			public ColorGradingModel.LogWheelsSettings log;

			// Token: 0x04004EE0 RID: 20192
			[TrackballGroup]
			public ColorGradingModel.LinearWheelsSettings linear;
		}

		// Token: 0x02000BDB RID: 3035
		[Serializable]
		public struct CurvesSettings
		{
			// Token: 0x17000676 RID: 1654
			// (get) Token: 0x060048F0 RID: 18672 RVA: 0x00263D08 File Offset: 0x00262108
			public static ColorGradingModel.CurvesSettings defaultSettings
			{
				get
				{
					return new ColorGradingModel.CurvesSettings
					{
						master = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						red = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						green = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						blue = new ColorGradingCurve(new AnimationCurve(new Keyframe[]
						{
							new Keyframe(0f, 0f, 1f, 1f),
							new Keyframe(1f, 1f, 1f, 1f)
						}), 0f, false, new Vector2(0f, 1f)),
						hueVShue = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						hueVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, true, new Vector2(0f, 1f)),
						satVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						lumVSsat = new ColorGradingCurve(new AnimationCurve(), 0.5f, false, new Vector2(0f, 1f)),
						e_CurrentEditingCurve = 0,
						e_CurveY = true,
						e_CurveR = false,
						e_CurveG = false,
						e_CurveB = false
					};
				}
			}

			// Token: 0x04004EE1 RID: 20193
			public ColorGradingCurve master;

			// Token: 0x04004EE2 RID: 20194
			public ColorGradingCurve red;

			// Token: 0x04004EE3 RID: 20195
			public ColorGradingCurve green;

			// Token: 0x04004EE4 RID: 20196
			public ColorGradingCurve blue;

			// Token: 0x04004EE5 RID: 20197
			public ColorGradingCurve hueVShue;

			// Token: 0x04004EE6 RID: 20198
			public ColorGradingCurve hueVSsat;

			// Token: 0x04004EE7 RID: 20199
			public ColorGradingCurve satVSsat;

			// Token: 0x04004EE8 RID: 20200
			public ColorGradingCurve lumVSsat;

			// Token: 0x04004EE9 RID: 20201
			[HideInInspector]
			public int e_CurrentEditingCurve;

			// Token: 0x04004EEA RID: 20202
			[HideInInspector]
			public bool e_CurveY;

			// Token: 0x04004EEB RID: 20203
			[HideInInspector]
			public bool e_CurveR;

			// Token: 0x04004EEC RID: 20204
			[HideInInspector]
			public bool e_CurveG;

			// Token: 0x04004EED RID: 20205
			[HideInInspector]
			public bool e_CurveB;
		}

		// Token: 0x02000BDC RID: 3036
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000677 RID: 1655
			// (get) Token: 0x060048F1 RID: 18673 RVA: 0x00263FB8 File Offset: 0x002623B8
			public static ColorGradingModel.Settings defaultSettings
			{
				get
				{
					return new ColorGradingModel.Settings
					{
						tonemapping = ColorGradingModel.TonemappingSettings.defaultSettings,
						basic = ColorGradingModel.BasicSettings.defaultSettings,
						channelMixer = ColorGradingModel.ChannelMixerSettings.defaultSettings,
						colorWheels = ColorGradingModel.ColorWheelsSettings.defaultSettings,
						curves = ColorGradingModel.CurvesSettings.defaultSettings
					};
				}
			}

			// Token: 0x04004EEE RID: 20206
			public ColorGradingModel.TonemappingSettings tonemapping;

			// Token: 0x04004EEF RID: 20207
			public ColorGradingModel.BasicSettings basic;

			// Token: 0x04004EF0 RID: 20208
			public ColorGradingModel.ChannelMixerSettings channelMixer;

			// Token: 0x04004EF1 RID: 20209
			public ColorGradingModel.ColorWheelsSettings colorWheels;

			// Token: 0x04004EF2 RID: 20210
			public ColorGradingModel.CurvesSettings curves;
		}
	}
}
