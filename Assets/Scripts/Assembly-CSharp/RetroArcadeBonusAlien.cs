using System;
using UnityEngine;

// Token: 0x02000738 RID: 1848
public class RetroArcadeBonusAlien : RetroArcadeEnemy
{
	// Token: 0x06002848 RID: 10312 RVA: 0x00177D90 File Offset: 0x00176190
	public RetroArcadeBonusAlien Create(RetroArcadeBonusAlien.Direction direction, LevelProperties.RetroArcade.Aliens properties)
	{
		RetroArcadeBonusAlien retroArcadeBonusAlien = this.InstantiatePrefab<RetroArcadeBonusAlien>();
		retroArcadeBonusAlien.transform.position = new Vector2((direction != RetroArcadeBonusAlien.Direction.Left) ? -400f : 400f, 270f);
		retroArcadeBonusAlien.properties = properties;
		retroArcadeBonusAlien.direction = direction;
		retroArcadeBonusAlien.hp = 1f;
		return retroArcadeBonusAlien;
	}

	// Token: 0x06002849 RID: 10313 RVA: 0x00177DF0 File Offset: 0x001761F0
	protected override void FixedUpdate()
	{
		base.transform.AddPosition((float)((this.direction != RetroArcadeBonusAlien.Direction.Right) ? -1 : 1) * this.properties.bonusMoveSpeed * CupheadTime.FixedDelta, 0f, 0f);
		if ((this.direction != RetroArcadeBonusAlien.Direction.Left) ? (base.transform.position.x > 400f) : (base.transform.position.x < -400f))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400310D RID: 12557
	private const float SPAWN_X = 400f;

	// Token: 0x0400310E RID: 12558
	private const float SPAWN_Y = 270f;

	// Token: 0x0400310F RID: 12559
	private LevelProperties.RetroArcade.Aliens properties;

	// Token: 0x04003110 RID: 12560
	private RetroArcadeBonusAlien.Direction direction;

	// Token: 0x02000739 RID: 1849
	public enum Direction
	{
		// Token: 0x04003112 RID: 12562
		Left,
		// Token: 0x04003113 RID: 12563
		Right
	}
}
