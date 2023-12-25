using System;

// Token: 0x02000968 RID: 2408
public class SharedMapGate : MapLevelDependentObstacle
{
	// Token: 0x0600381C RID: 14364 RVA: 0x00201A34 File Offset: 0x001FFE34
	protected override bool ValidateSucess()
	{
		bool result = false;
		foreach (Levels levelID in this._levels)
		{
			PlayerData.PlayerLevelDataObject levelData = PlayerData.Data.GetLevelData(levelID);
			if (levelData.completed)
			{
				result = true;
				this.difficulty = levelData.difficultyBeaten;
				this.grade = levelData.grade;
				break;
			}
		}
		return result;
	}

	// Token: 0x0600381D RID: 14365 RVA: 0x00201AA0 File Offset: 0x001FFEA0
	protected override bool ValidateCondition(Levels level)
	{
		bool result = false;
		if (Level.PreviousLevel != level && PlayerData.Data.GetLevelData(level).completed)
		{
			this.previouslyWon = true;
		}
		if (this.previouslyWon)
		{
			return false;
		}
		if (!Level.PreviouslyWon && Level.Won)
		{
			result = true;
		}
		if (this.ReactToGradeChange && Level.Grade > Level.PreviousGrade)
		{
			this.gradeChanged = true;
			result = true;
		}
		if (this.ReactToDifficultyChange && Level.Difficulty > Level.PreviousDifficulty)
		{
			this.difficultyChanged = true;
			result = true;
		}
		return result;
	}

	// Token: 0x04003FFD RID: 16381
	private bool previouslyWon;
}
