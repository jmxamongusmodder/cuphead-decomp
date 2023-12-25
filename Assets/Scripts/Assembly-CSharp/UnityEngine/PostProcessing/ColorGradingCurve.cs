using System;

namespace UnityEngine.PostProcessing
{
	// Token: 0x02000BFF RID: 3071
	[Serializable]
	public sealed class ColorGradingCurve
	{
		// Token: 0x06004951 RID: 18769 RVA: 0x00265338 File Offset: 0x00263738
		public ColorGradingCurve(AnimationCurve curve, float zeroValue, bool loop, Vector2 bounds)
		{
			this.curve = curve;
			this.m_ZeroValue = zeroValue;
			this.m_Loop = loop;
			this.m_Range = bounds.magnitude;
		}

		// Token: 0x06004952 RID: 18770 RVA: 0x00265364 File Offset: 0x00263764
		public void Cache()
		{
			if (!this.m_Loop)
			{
				return;
			}
			int length = this.curve.length;
			if (length < 2)
			{
				return;
			}
			if (this.m_InternalLoopingCurve == null)
			{
				this.m_InternalLoopingCurve = new AnimationCurve();
			}
			Keyframe key = this.curve[length - 1];
			key.time -= this.m_Range;
			Keyframe key2 = this.curve[0];
			key2.time += this.m_Range;
			this.m_InternalLoopingCurve.keys = this.curve.keys;
			this.m_InternalLoopingCurve.AddKey(key);
			this.m_InternalLoopingCurve.AddKey(key2);
		}

		// Token: 0x06004953 RID: 18771 RVA: 0x0026541C File Offset: 0x0026381C
		public float Evaluate(float t)
		{
			if (this.curve.length == 0)
			{
				return this.m_ZeroValue;
			}
			if (!this.m_Loop || this.curve.length == 1)
			{
				return this.curve.Evaluate(t);
			}
			return this.m_InternalLoopingCurve.Evaluate(t);
		}

		// Token: 0x04004F74 RID: 20340
		public AnimationCurve curve;

		// Token: 0x04004F75 RID: 20341
		[SerializeField]
		private bool m_Loop;

		// Token: 0x04004F76 RID: 20342
		[SerializeField]
		private float m_ZeroValue;

		// Token: 0x04004F77 RID: 20343
		[SerializeField]
		private float m_Range;

		// Token: 0x04004F78 RID: 20344
		private AnimationCurve m_InternalLoopingCurve;
	}
}
