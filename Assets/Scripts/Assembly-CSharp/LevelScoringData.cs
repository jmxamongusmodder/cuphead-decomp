using System;
using UnityEngine;

// Token: 0x02000B2D RID: 2861
public class LevelScoringData
{
	// Token: 0x06004564 RID: 17764 RVA: 0x00247FDC File Offset: 0x002463DC
	public LevelScoringData.Grade CalculateGrade()
	{
		if (this.pacifistRun && !this.usedDjimmi)
		{
			return LevelScoringData.Grade.P;
		}
		ScoringEditorData scoringProperties = Cuphead.Current.ScoringProperties;
		float num = Mathf.Clamp(100f - 100f * (this.time - this.goalTime) / (this.goalTime * (scoringProperties.bestTimeMultiplierForNoScore - 1f)), 0f, 100f);
		float num2 = Mathf.Clamp(100f - 100f * ((scoringProperties.hitsForNoScore - (float)this.finalHP) / scoringProperties.hitsForNoScore), 0f, 100f);
		float num3 = 100f * Mathf.Min((float)this.numParries, scoringProperties.parriesForHighestGrade + scoringProperties.bonusParries) / scoringProperties.parriesForHighestGrade;
		float num4 = 100f * Mathf.Min((float)this.superMeterUsed, scoringProperties.superMeterUsageForHighestGrade + scoringProperties.bonusSuperMeterUsage) / scoringProperties.superMeterUsageForHighestGrade;
		if (this.useCoinsInsteadOfSuperMeter)
		{
			num4 = 100f * ((float)this.coinsCollected / 5f);
		}
		float num5 = num * scoringProperties.timeWeight + num2 * scoringProperties.hitsWeight + num3 * scoringProperties.parriesWeight + num4 * scoringProperties.superMeterUsageWeight;
		ScoringEditorData.GradingCurveEntry[] array = (this.difficulty != Level.Mode.Easy) ? ((this.difficulty != Level.Mode.Normal) ? scoringProperties.hardGradingCurve : scoringProperties.mediumGradingCurve) : scoringProperties.easyGradingCurve;
		LevelScoringData.Grade grade = LevelScoringData.Grade.DMinus;
		foreach (ScoringEditorData.GradingCurveEntry gradingCurveEntry in array)
		{
			grade = gradingCurveEntry.grade;
			if (num5 < gradingCurveEntry.upperLimit)
			{
				break;
			}
		}
		if (this.usedDjimmi && grade > LevelScoringData.Grade.BPlus)
		{
			grade = LevelScoringData.Grade.BPlus;
		}
		return grade;
	}

	// Token: 0x04004AF6 RID: 19190
	public float time;

	// Token: 0x04004AF7 RID: 19191
	public float goalTime;

	// Token: 0x04004AF8 RID: 19192
	public int finalHP;

	// Token: 0x04004AF9 RID: 19193
	public int numTimesHit;

	// Token: 0x04004AFA RID: 19194
	public int numParries;

	// Token: 0x04004AFB RID: 19195
	public int superMeterUsed;

	// Token: 0x04004AFC RID: 19196
	public int coinsCollected;

	// Token: 0x04004AFD RID: 19197
	public Level.Mode difficulty;

	// Token: 0x04004AFE RID: 19198
	public bool pacifistRun;

	// Token: 0x04004AFF RID: 19199
	public bool useCoinsInsteadOfSuperMeter;

	// Token: 0x04004B00 RID: 19200
	public bool usedDjimmi;

	// Token: 0x04004B01 RID: 19201
	public bool player1IsChalice;

	// Token: 0x04004B02 RID: 19202
	public bool player2IsChalice;

	// Token: 0x02000B2E RID: 2862
	public enum Grade
	{
		// Token: 0x04004B04 RID: 19204
		DMinus,
		// Token: 0x04004B05 RID: 19205
		D,
		// Token: 0x04004B06 RID: 19206
		DPlus,
		// Token: 0x04004B07 RID: 19207
		CMinus,
		// Token: 0x04004B08 RID: 19208
		C,
		// Token: 0x04004B09 RID: 19209
		CPlus,
		// Token: 0x04004B0A RID: 19210
		BMinus,
		// Token: 0x04004B0B RID: 19211
		B,
		// Token: 0x04004B0C RID: 19212
		BPlus,
		// Token: 0x04004B0D RID: 19213
		AMinus,
		// Token: 0x04004B0E RID: 19214
		A,
		// Token: 0x04004B0F RID: 19215
		APlus,
		// Token: 0x04004B10 RID: 19216
		S,
		// Token: 0x04004B11 RID: 19217
		P
	}
}
