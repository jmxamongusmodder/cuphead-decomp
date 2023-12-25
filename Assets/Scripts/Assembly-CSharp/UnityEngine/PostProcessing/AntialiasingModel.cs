﻿using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BBF RID: 3007
	[Serializable]
	public class AntialiasingModel : PostProcessingModel
	{
		// Token: 0x1700065D RID: 1629
		// (get) Token: 0x060048C2 RID: 18626 RVA: 0x002634C2 File Offset: 0x002618C2
		// (set) Token: 0x060048C3 RID: 18627 RVA: 0x002634CA File Offset: 0x002618CA
		public AntialiasingModel.Settings settings
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

		// Token: 0x060048C4 RID: 18628 RVA: 0x002634D3 File Offset: 0x002618D3
		public override void Reset()
		{
			this.m_Settings = AntialiasingModel.Settings.defaultSettings;
		}

		// Token: 0x04004E7F RID: 20095
		[SerializeField]
		private AntialiasingModel.Settings m_Settings = AntialiasingModel.Settings.defaultSettings;

		// Token: 0x02000BC0 RID: 3008
		public enum Method
		{
			// Token: 0x04004E81 RID: 20097
			Fxaa,
			// Token: 0x04004E82 RID: 20098
			Taa
		}

		// Token: 0x02000BC1 RID: 3009
		public enum FxaaPreset
		{
			// Token: 0x04004E84 RID: 20100
			ExtremePerformance,
			// Token: 0x04004E85 RID: 20101
			Performance,
			// Token: 0x04004E86 RID: 20102
			Default,
			// Token: 0x04004E87 RID: 20103
			Quality,
			// Token: 0x04004E88 RID: 20104
			ExtremeQuality
		}

		// Token: 0x02000BC2 RID: 3010
		[Serializable]
		public struct FxaaQualitySettings
		{
			// Token: 0x04004E89 RID: 20105
			[Tooltip("The amount of desired sub-pixel aliasing removal. Effects the sharpeness of the output.")]
			[Range(0f, 1f)]
			public float subpixelAliasingRemovalAmount;

			// Token: 0x04004E8A RID: 20106
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.063f, 0.333f)]
			public float edgeDetectionThreshold;

			// Token: 0x04004E8B RID: 20107
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0f, 0.0833f)]
			public float minimumRequiredLuminance;

			// Token: 0x04004E8C RID: 20108
			public static AntialiasingModel.FxaaQualitySettings[] presets = new AntialiasingModel.FxaaQualitySettings[]
			{
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0f,
					edgeDetectionThreshold = 0.333f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.25f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 0.75f,
					edgeDetectionThreshold = 0.166f,
					minimumRequiredLuminance = 0.0833f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.0625f
				},
				new AntialiasingModel.FxaaQualitySettings
				{
					subpixelAliasingRemovalAmount = 1f,
					edgeDetectionThreshold = 0.063f,
					minimumRequiredLuminance = 0.0312f
				}
			};
		}

		// Token: 0x02000BC3 RID: 3011
		[Serializable]
		public struct FxaaConsoleSettings
		{
			// Token: 0x04004E8D RID: 20109
			[Tooltip("The amount of spread applied to the sampling coordinates while sampling for subpixel information.")]
			[Range(0.33f, 0.5f)]
			public float subpixelSpreadAmount;

			// Token: 0x04004E8E RID: 20110
			[Tooltip("This value dictates how sharp the edges in the image are kept; a higher value implies sharper edges.")]
			[Range(2f, 8f)]
			public float edgeSharpnessAmount;

			// Token: 0x04004E8F RID: 20111
			[Tooltip("The minimum amount of local contrast required to qualify a region as containing an edge.")]
			[Range(0.125f, 0.25f)]
			public float edgeDetectionThreshold;

			// Token: 0x04004E90 RID: 20112
			[Tooltip("Local contrast adaptation value to disallow the algorithm from executing on the darker regions.")]
			[Range(0.04f, 0.06f)]
			public float minimumRequiredLuminance;

			// Token: 0x04004E91 RID: 20113
			public static AntialiasingModel.FxaaConsoleSettings[] presets = new AntialiasingModel.FxaaConsoleSettings[]
			{
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.25f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.33f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.06f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 8f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.05f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 4f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				},
				new AntialiasingModel.FxaaConsoleSettings
				{
					subpixelSpreadAmount = 0.5f,
					edgeSharpnessAmount = 2f,
					edgeDetectionThreshold = 0.125f,
					minimumRequiredLuminance = 0.04f
				}
			};
		}

		// Token: 0x02000BC4 RID: 3012
		[Serializable]
		public struct FxaaSettings
		{
			// Token: 0x1700065E RID: 1630
			// (get) Token: 0x060048C7 RID: 18631 RVA: 0x0026378C File Offset: 0x00261B8C
			public static AntialiasingModel.FxaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.FxaaSettings
					{
						preset = AntialiasingModel.FxaaPreset.Default
					};
				}
			}

			// Token: 0x04004E92 RID: 20114
			public AntialiasingModel.FxaaPreset preset;
		}

		// Token: 0x02000BC5 RID: 3013
		[Serializable]
		public struct TaaSettings
		{
			// Token: 0x1700065F RID: 1631
			// (get) Token: 0x060048C8 RID: 18632 RVA: 0x002637AC File Offset: 0x00261BAC
			public static AntialiasingModel.TaaSettings defaultSettings
			{
				get
				{
					return new AntialiasingModel.TaaSettings
					{
						jitterSpread = 0.75f,
						sharpen = 0.3f,
						stationaryBlending = 0.95f,
						motionBlending = 0.85f
					};
				}
			}

			// Token: 0x04004E93 RID: 20115
			[Tooltip("The diameter (in texels) inside which jitter samples are spread. Smaller values result in crisper but more aliased output, while larger values result in more stable but blurrier output.")]
			[Range(0.1f, 1f)]
			public float jitterSpread;

			// Token: 0x04004E94 RID: 20116
			[Tooltip("Controls the amount of sharpening applied to the color buffer.")]
			[Range(0f, 3f)]
			public float sharpen;

			// Token: 0x04004E95 RID: 20117
			[Tooltip("The blend coefficient for a stationary fragment. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float stationaryBlending;

			// Token: 0x04004E96 RID: 20118
			[Tooltip("The blend coefficient for a fragment with significant motion. Controls the percentage of history sample blended into the final color.")]
			[Range(0f, 0.99f)]
			public float motionBlending;
		}

		// Token: 0x02000BC6 RID: 3014
		[Serializable]
		public struct Settings
		{
			// Token: 0x17000660 RID: 1632
			// (get) Token: 0x060048C9 RID: 18633 RVA: 0x002637F4 File Offset: 0x00261BF4
			public static AntialiasingModel.Settings defaultSettings
			{
				get
				{
					return new AntialiasingModel.Settings
					{
						method = AntialiasingModel.Method.Fxaa,
						fxaaSettings = AntialiasingModel.FxaaSettings.defaultSettings,
						taaSettings = AntialiasingModel.TaaSettings.defaultSettings
					};
				}
			}

			// Token: 0x04004E97 RID: 20119
			public AntialiasingModel.Method method;

			// Token: 0x04004E98 RID: 20120
			public AntialiasingModel.FxaaSettings fxaaSettings;

			// Token: 0x04004E99 RID: 20121
			public AntialiasingModel.TaaSettings taaSettings;
		}
	}
}
