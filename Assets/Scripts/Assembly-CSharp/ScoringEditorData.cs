using System;

// Token: 0x02000B2F RID: 2863
public class ScoringEditorData : AbstractMonoBehaviour
{
	// Token: 0x04004B12 RID: 19218
	public float bestTimeMultiplierForNoScore;

	// Token: 0x04004B13 RID: 19219
	public float hitsForNoScore;

	// Token: 0x04004B14 RID: 19220
	public float parriesForHighestGrade;

	// Token: 0x04004B15 RID: 19221
	public float bonusParries;

	// Token: 0x04004B16 RID: 19222
	public float superMeterUsageForHighestGrade;

	// Token: 0x04004B17 RID: 19223
	public float bonusSuperMeterUsage;

	// Token: 0x04004B18 RID: 19224
	public ScoringEditorData.GradingCurveEntry[] easyGradingCurve;

	// Token: 0x04004B19 RID: 19225
	public ScoringEditorData.GradingCurveEntry[] mediumGradingCurve;

	// Token: 0x04004B1A RID: 19226
	public ScoringEditorData.GradingCurveEntry[] hardGradingCurve;

	// Token: 0x04004B1B RID: 19227
	public float timeWeight;

	// Token: 0x04004B1C RID: 19228
	public float hitsWeight;

	// Token: 0x04004B1D RID: 19229
	public float parriesWeight;

	// Token: 0x04004B1E RID: 19230
	public float superMeterUsageWeight;

	// Token: 0x02000B30 RID: 2864
	[Serializable]
	public class GradingCurveEntry
	{
		// Token: 0x04004B1F RID: 19231
		public LevelScoringData.Grade grade;

		// Token: 0x04004B20 RID: 19232
		public float upperLimit;
	}
}
