using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000B9F RID: 2975
	public sealed class ColorGradingComponent : PostProcessingComponentRenderTexture<ColorGradingModel>
	{
		// Token: 0x1700064C RID: 1612
		// (get) Token: 0x06004842 RID: 18498 RVA: 0x0025EFF3 File Offset: 0x0025D3F3
		public override bool active
		{
			get
			{
				return base.model.enabled && !this.context.interrupted;
			}
		}

		// Token: 0x06004843 RID: 18499 RVA: 0x0025F016 File Offset: 0x0025D416
		private float StandardIlluminantY(float x)
		{
			return 2.87f * x - 3f * x * x - 0.27509508f;
		}

		// Token: 0x06004844 RID: 18500 RVA: 0x0025F030 File Offset: 0x0025D430
		private Vector3 CIExyToLMS(float x, float y)
		{
			float num = 1f;
			float num2 = num * x / y;
			float num3 = num * (1f - x - y) / y;
			float x2 = 0.7328f * num2 + 0.4296f * num - 0.1624f * num3;
			float y2 = -0.7036f * num2 + 1.6975f * num + 0.0061f * num3;
			float z = 0.003f * num2 + 0.0136f * num + 0.9834f * num3;
			return new Vector3(x2, y2, z);
		}

		// Token: 0x06004845 RID: 18501 RVA: 0x0025F0AC File Offset: 0x0025D4AC
		private Vector3 CalculateColorBalance(float temperature, float tint)
		{
			float num = temperature / 55f;
			float num2 = tint / 55f;
			float x = 0.31271f - num * ((num >= 0f) ? 0.05f : 0.1f);
			float y = this.StandardIlluminantY(x) + num2 * 0.05f;
			Vector3 vector = new Vector3(0.949237f, 1.03542f, 1.08728f);
			Vector3 vector2 = this.CIExyToLMS(x, y);
			return new Vector3(vector.x / vector2.x, vector.y / vector2.y, vector.z / vector2.z);
		}

		// Token: 0x06004846 RID: 18502 RVA: 0x0025F150 File Offset: 0x0025D550
		private static Color NormalizeColor(Color c)
		{
			float num = (c.r + c.g + c.b) / 3f;
			if (Mathf.Approximately(num, 0f))
			{
				return new Color(1f, 1f, 1f, c.a);
			}
			return new Color
			{
				r = c.r / num,
				g = c.g / num,
				b = c.b / num,
				a = c.a
			};
		}

		// Token: 0x06004847 RID: 18503 RVA: 0x0025F1EE File Offset: 0x0025D5EE
		private static Vector3 ClampVector(Vector3 v, float min, float max)
		{
			return new Vector3(Mathf.Clamp(v.x, min, max), Mathf.Clamp(v.y, min, max), Mathf.Clamp(v.z, min, max));
		}

		// Token: 0x06004848 RID: 18504 RVA: 0x0025F220 File Offset: 0x0025D620
		public static Vector3 GetLiftValue(Color lift)
		{
			Color color = ColorGradingComponent.NormalizeColor(lift);
			float num = (color.r + color.g + color.b) / 3f;
			float x = (color.r - num) * 0.1f + lift.a;
			float y = (color.g - num) * 0.1f + lift.a;
			float z = (color.b - num) * 0.1f + lift.a;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), -1f, 1f);
		}

		// Token: 0x06004849 RID: 18505 RVA: 0x0025F2B4 File Offset: 0x0025D6B4
		public static Vector3 GetGammaValue(Color gamma)
		{
			Color color = ColorGradingComponent.NormalizeColor(gamma);
			float num = (color.r + color.g + color.b) / 3f;
			gamma.a *= ((gamma.a >= 0f) ? 5f : 0.8f);
			float b = Mathf.Pow(2f, (color.r - num) * 0.5f) + gamma.a;
			float b2 = Mathf.Pow(2f, (color.g - num) * 0.5f) + gamma.a;
			float b3 = Mathf.Pow(2f, (color.b - num) * 0.5f) + gamma.a;
			float x = 1f / Mathf.Max(0.01f, b);
			float y = 1f / Mathf.Max(0.01f, b2);
			float z = 1f / Mathf.Max(0.01f, b3);
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0f, 5f);
		}

		// Token: 0x0600484A RID: 18506 RVA: 0x0025F3D0 File Offset: 0x0025D7D0
		public static Vector3 GetGainValue(Color gain)
		{
			Color color = ColorGradingComponent.NormalizeColor(gain);
			float num = (color.r + color.g + color.b) / 3f;
			gain.a *= ((gain.a <= 0f) ? 1f : 3f);
			float x = Mathf.Pow(2f, (color.r - num) * 0.5f) + gain.a;
			float y = Mathf.Pow(2f, (color.g - num) * 0.5f) + gain.a;
			float z = Mathf.Pow(2f, (color.b - num) * 0.5f) + gain.a;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0f, 4f);
		}

		// Token: 0x0600484B RID: 18507 RVA: 0x0025F4AF File Offset: 0x0025D8AF
		public static void CalculateLiftGammaGain(Color lift, Color gamma, Color gain, out Vector3 outLift, out Vector3 outGamma, out Vector3 outGain)
		{
			outLift = ColorGradingComponent.GetLiftValue(lift);
			outGamma = ColorGradingComponent.GetGammaValue(gamma);
			outGain = ColorGradingComponent.GetGainValue(gain);
		}

		// Token: 0x0600484C RID: 18508 RVA: 0x0025F4D8 File Offset: 0x0025D8D8
		public static Vector3 GetSlopeValue(Color slope)
		{
			Color color = ColorGradingComponent.NormalizeColor(slope);
			float num = (color.r + color.g + color.b) / 3f;
			slope.a *= 0.5f;
			float x = (color.r - num) * 0.1f + slope.a + 1f;
			float y = (color.g - num) * 0.1f + slope.a + 1f;
			float z = (color.b - num) * 0.1f + slope.a + 1f;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0f, 2f);
		}

		// Token: 0x0600484D RID: 18509 RVA: 0x0025F590 File Offset: 0x0025D990
		public static Vector3 GetPowerValue(Color power)
		{
			Color color = ColorGradingComponent.NormalizeColor(power);
			float num = (color.r + color.g + color.b) / 3f;
			power.a *= 0.5f;
			float b = (color.r - num) * 0.1f + power.a + 1f;
			float b2 = (color.g - num) * 0.1f + power.a + 1f;
			float b3 = (color.b - num) * 0.1f + power.a + 1f;
			float x = 1f / Mathf.Max(0.01f, b);
			float y = 1f / Mathf.Max(0.01f, b2);
			float z = 1f / Mathf.Max(0.01f, b3);
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), 0.5f, 2.5f);
		}

		// Token: 0x0600484E RID: 18510 RVA: 0x0025F684 File Offset: 0x0025DA84
		public static Vector3 GetOffsetValue(Color offset)
		{
			Color color = ColorGradingComponent.NormalizeColor(offset);
			float num = (color.r + color.g + color.b) / 3f;
			offset.a *= 0.5f;
			float x = (color.r - num) * 0.05f + offset.a;
			float y = (color.g - num) * 0.05f + offset.a;
			float z = (color.b - num) * 0.05f + offset.a;
			return ColorGradingComponent.ClampVector(new Vector3(x, y, z), -0.8f, 0.8f);
		}

		// Token: 0x0600484F RID: 18511 RVA: 0x0025F72A File Offset: 0x0025DB2A
		public static void CalculateSlopePowerOffset(Color slope, Color power, Color offset, out Vector3 outSlope, out Vector3 outPower, out Vector3 outOffset)
		{
			outSlope = ColorGradingComponent.GetSlopeValue(slope);
			outPower = ColorGradingComponent.GetPowerValue(power);
			outOffset = ColorGradingComponent.GetOffsetValue(offset);
		}

		// Token: 0x06004850 RID: 18512 RVA: 0x0025F752 File Offset: 0x0025DB52
		private TextureFormat GetCurveFormat()
		{
			if (SystemInfo.SupportsTextureFormat(TextureFormat.RGBAHalf))
			{
				return TextureFormat.RGBAHalf;
			}
			return TextureFormat.RGBA32;
		}

		// Token: 0x06004851 RID: 18513 RVA: 0x0025F764 File Offset: 0x0025DB64
		private Texture2D GetCurveTexture()
		{
			if (this.m_GradingCurves == null)
			{
				this.m_GradingCurves = new Texture2D(128, 2, this.GetCurveFormat(), false, true)
				{
					name = "Internal Curves Texture",
					hideFlags = HideFlags.DontSave,
					anisoLevel = 0,
					wrapMode = TextureWrapMode.Clamp,
					filterMode = FilterMode.Bilinear
				};
			}
			ColorGradingModel.CurvesSettings curves = base.model.settings.curves;
			curves.hueVShue.Cache();
			curves.hueVSsat.Cache();
			for (int i = 0; i < 128; i++)
			{
				float t = (float)i * 0.0078125f;
				float r = curves.hueVShue.Evaluate(t);
				float g = curves.hueVSsat.Evaluate(t);
				float b = curves.satVSsat.Evaluate(t);
				float a = curves.lumVSsat.Evaluate(t);
				this.m_pixels[i] = new Color(r, g, b, a);
				float a2 = curves.master.Evaluate(t);
				float r2 = curves.red.Evaluate(t);
				float g2 = curves.green.Evaluate(t);
				float b2 = curves.blue.Evaluate(t);
				this.m_pixels[i + 128] = new Color(r2, g2, b2, a2);
			}
			this.m_GradingCurves.SetPixels(this.m_pixels);
			this.m_GradingCurves.Apply(false, false);
			return this.m_GradingCurves;
		}

		// Token: 0x06004852 RID: 18514 RVA: 0x0025F8F7 File Offset: 0x0025DCF7
		private bool IsLogLutValid(RenderTexture lut)
		{
			return lut != null && lut.IsCreated() && lut.height == 32;
		}

		// Token: 0x06004853 RID: 18515 RVA: 0x0025F91D File Offset: 0x0025DD1D
		private RenderTextureFormat GetLutFormat()
		{
			if (SystemInfo.SupportsRenderTextureFormat(RenderTextureFormat.ARGBHalf))
			{
				return RenderTextureFormat.ARGBHalf;
			}
			return RenderTextureFormat.ARGB32;
		}

		// Token: 0x06004854 RID: 18516 RVA: 0x0025F930 File Offset: 0x0025DD30
		private void GenerateLut()
		{
			ColorGradingModel.Settings settings = base.model.settings;
			if (!this.IsLogLutValid(base.model.bakedLut))
			{
				GraphicsUtils.Destroy(base.model.bakedLut);
				base.model.bakedLut = new RenderTexture(1024, 32, 0, this.GetLutFormat())
				{
					name = "Color Grading Log LUT",
					hideFlags = HideFlags.DontSave,
					filterMode = FilterMode.Bilinear,
					wrapMode = TextureWrapMode.Clamp,
					anisoLevel = 0
				};
			}
			Material material = this.context.materialFactory.Get("Hidden/Post FX/Lut Generator");
			material.SetVector(ColorGradingComponent.Uniforms._LutParams, new Vector4(32f, 0.00048828125f, 0.015625f, 1.032258f));
			material.shaderKeywords = null;
			ColorGradingModel.TonemappingSettings tonemapping = settings.tonemapping;
			ColorGradingModel.Tonemapper tonemapper = tonemapping.tonemapper;
			if (tonemapper != ColorGradingModel.Tonemapper.Neutral)
			{
				if (tonemapper == ColorGradingModel.Tonemapper.ACES)
				{
					material.EnableKeyword("TONEMAPPING_FILMIC");
				}
			}
			else
			{
				material.EnableKeyword("TONEMAPPING_NEUTRAL");
				float num = tonemapping.neutralBlackIn * 20f + 1f;
				float num2 = tonemapping.neutralBlackOut * 10f + 1f;
				float num3 = tonemapping.neutralWhiteIn / 20f;
				float num4 = 1f - tonemapping.neutralWhiteOut / 20f;
				float t = num / num2;
				float t2 = num3 / num4;
				float y = Mathf.Max(0f, Mathf.LerpUnclamped(0.57f, 0.37f, t));
				float z = Mathf.LerpUnclamped(0.01f, 0.24f, t2);
				float w = Mathf.Max(0f, Mathf.LerpUnclamped(0.02f, 0.2f, t));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams1, new Vector4(0.2f, y, z, w));
				material.SetVector(ColorGradingComponent.Uniforms._NeutralTonemapperParams2, new Vector4(0.02f, 0.3f, tonemapping.neutralWhiteLevel, tonemapping.neutralWhiteClip / 10f));
			}
			material.SetFloat(ColorGradingComponent.Uniforms._HueShift, settings.basic.hueShift / 360f);
			material.SetFloat(ColorGradingComponent.Uniforms._Saturation, settings.basic.saturation);
			material.SetFloat(ColorGradingComponent.Uniforms._Contrast, settings.basic.contrast);
			material.SetVector(ColorGradingComponent.Uniforms._Balance, this.CalculateColorBalance(settings.basic.temperature, settings.basic.tint));
			Vector3 v;
			Vector3 v2;
			Vector3 v3;
			ColorGradingComponent.CalculateLiftGammaGain(settings.colorWheels.linear.lift, settings.colorWheels.linear.gamma, settings.colorWheels.linear.gain, out v, out v2, out v3);
			material.SetVector(ColorGradingComponent.Uniforms._Lift, v);
			material.SetVector(ColorGradingComponent.Uniforms._InvGamma, v2);
			material.SetVector(ColorGradingComponent.Uniforms._Gain, v3);
			Vector3 v4;
			Vector3 v5;
			Vector3 v6;
			ColorGradingComponent.CalculateSlopePowerOffset(settings.colorWheels.log.slope, settings.colorWheels.log.power, settings.colorWheels.log.offset, out v4, out v5, out v6);
			material.SetVector(ColorGradingComponent.Uniforms._Slope, v4);
			material.SetVector(ColorGradingComponent.Uniforms._Power, v5);
			material.SetVector(ColorGradingComponent.Uniforms._Offset, v6);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerRed, settings.channelMixer.red);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerGreen, settings.channelMixer.green);
			material.SetVector(ColorGradingComponent.Uniforms._ChannelMixerBlue, settings.channelMixer.blue);
			material.SetTexture(ColorGradingComponent.Uniforms._Curves, this.GetCurveTexture());
			Graphics.Blit(null, base.model.bakedLut, material, 0);
		}

		// Token: 0x06004855 RID: 18517 RVA: 0x0025FD0C File Offset: 0x0025E10C
		public override void Prepare(Material uberMaterial)
		{
			if (base.model.isDirty || !this.IsLogLutValid(base.model.bakedLut))
			{
				this.GenerateLut();
				base.model.isDirty = false;
			}
			uberMaterial.EnableKeyword((!this.context.profile.debugViews.IsModeActive(BuiltinDebugViewsModel.Mode.PreGradingLog)) ? "COLOR_GRADING" : "COLOR_GRADING_LOG_VIEW");
			RenderTexture bakedLut = base.model.bakedLut;
			uberMaterial.SetTexture(ColorGradingComponent.Uniforms._LogLut, bakedLut);
			uberMaterial.SetVector(ColorGradingComponent.Uniforms._LogLut_Params, new Vector3(1f / (float)bakedLut.width, 1f / (float)bakedLut.height, (float)bakedLut.height - 1f));
			float value = Mathf.Exp(base.model.settings.basic.postExposure * 0.6931472f);
			uberMaterial.SetFloat(ColorGradingComponent.Uniforms._ExposureEV, value);
		}

		// Token: 0x06004856 RID: 18518 RVA: 0x0025FE08 File Offset: 0x0025E208
		public void OnGUI()
		{
			RenderTexture bakedLut = base.model.bakedLut;
			Rect position = new Rect(this.context.viewport.x * (float)Screen.width + 8f, 8f, (float)bakedLut.width, (float)bakedLut.height);
			GUI.DrawTexture(position, bakedLut);
		}

		// Token: 0x06004857 RID: 18519 RVA: 0x0025FE62 File Offset: 0x0025E262
		public override void OnDisable()
		{
			GraphicsUtils.Destroy(this.m_GradingCurves);
			GraphicsUtils.Destroy(base.model.bakedLut);
			this.m_GradingCurves = null;
			base.model.bakedLut = null;
		}

		// Token: 0x04004DB2 RID: 19890
		private const int k_InternalLogLutSize = 32;

		// Token: 0x04004DB3 RID: 19891
		private const int k_CurvePrecision = 128;

		// Token: 0x04004DB4 RID: 19892
		private const float k_CurveStep = 0.0078125f;

		// Token: 0x04004DB5 RID: 19893
		private Texture2D m_GradingCurves;

		// Token: 0x04004DB6 RID: 19894
		private Color[] m_pixels = new Color[256];

		// Token: 0x02000BA0 RID: 2976
		private static class Uniforms
		{
			// Token: 0x04004DB7 RID: 19895
			internal static readonly int _LutParams = Shader.PropertyToID("_LutParams");

			// Token: 0x04004DB8 RID: 19896
			internal static readonly int _NeutralTonemapperParams1 = Shader.PropertyToID("_NeutralTonemapperParams1");

			// Token: 0x04004DB9 RID: 19897
			internal static readonly int _NeutralTonemapperParams2 = Shader.PropertyToID("_NeutralTonemapperParams2");

			// Token: 0x04004DBA RID: 19898
			internal static readonly int _HueShift = Shader.PropertyToID("_HueShift");

			// Token: 0x04004DBB RID: 19899
			internal static readonly int _Saturation = Shader.PropertyToID("_Saturation");

			// Token: 0x04004DBC RID: 19900
			internal static readonly int _Contrast = Shader.PropertyToID("_Contrast");

			// Token: 0x04004DBD RID: 19901
			internal static readonly int _Balance = Shader.PropertyToID("_Balance");

			// Token: 0x04004DBE RID: 19902
			internal static readonly int _Lift = Shader.PropertyToID("_Lift");

			// Token: 0x04004DBF RID: 19903
			internal static readonly int _InvGamma = Shader.PropertyToID("_InvGamma");

			// Token: 0x04004DC0 RID: 19904
			internal static readonly int _Gain = Shader.PropertyToID("_Gain");

			// Token: 0x04004DC1 RID: 19905
			internal static readonly int _Slope = Shader.PropertyToID("_Slope");

			// Token: 0x04004DC2 RID: 19906
			internal static readonly int _Power = Shader.PropertyToID("_Power");

			// Token: 0x04004DC3 RID: 19907
			internal static readonly int _Offset = Shader.PropertyToID("_Offset");

			// Token: 0x04004DC4 RID: 19908
			internal static readonly int _ChannelMixerRed = Shader.PropertyToID("_ChannelMixerRed");

			// Token: 0x04004DC5 RID: 19909
			internal static readonly int _ChannelMixerGreen = Shader.PropertyToID("_ChannelMixerGreen");

			// Token: 0x04004DC6 RID: 19910
			internal static readonly int _ChannelMixerBlue = Shader.PropertyToID("_ChannelMixerBlue");

			// Token: 0x04004DC7 RID: 19911
			internal static readonly int _Curves = Shader.PropertyToID("_Curves");

			// Token: 0x04004DC8 RID: 19912
			internal static readonly int _LogLut = Shader.PropertyToID("_LogLut");

			// Token: 0x04004DC9 RID: 19913
			internal static readonly int _LogLut_Params = Shader.PropertyToID("_LogLut_Params");

			// Token: 0x04004DCA RID: 19914
			internal static readonly int _ExposureEV = Shader.PropertyToID("_ExposureEV");
		}
	}
}
