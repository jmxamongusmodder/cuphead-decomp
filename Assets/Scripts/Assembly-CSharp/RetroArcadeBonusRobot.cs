using System;
using UnityEngine;

// Token: 0x02000750 RID: 1872
public class RetroArcadeBonusRobot : RetroArcadeEnemy
{
	// Token: 0x060028D4 RID: 10452 RVA: 0x0017C70C File Offset: 0x0017AB0C
	public RetroArcadeBonusRobot Create(RetroArcadeBonusRobot.Direction direction, LevelProperties.RetroArcade.Robots properties)
	{
		RetroArcadeBonusRobot retroArcadeBonusRobot = this.InstantiatePrefab<RetroArcadeBonusRobot>();
		retroArcadeBonusRobot.transform.position = new Vector2((direction != RetroArcadeBonusRobot.Direction.Left) ? -400f : 400f, 250f);
		retroArcadeBonusRobot.properties = properties;
		retroArcadeBonusRobot.direction = direction;
		retroArcadeBonusRobot.hp = properties.bonusHp;
		return retroArcadeBonusRobot;
	}

	// Token: 0x060028D5 RID: 10453 RVA: 0x0017C76A File Offset: 0x0017AB6A
	protected override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x060028D6 RID: 10454 RVA: 0x0017C790 File Offset: 0x0017AB90
	protected override void FixedUpdate()
	{
		base.transform.AddPosition((float)((this.direction != RetroArcadeBonusRobot.Direction.Right) ? -1 : 1) * this.properties.bonusMoveSpeed * CupheadTime.FixedDelta, 0f, 0f);
		if ((this.direction != RetroArcadeBonusRobot.Direction.Left) ? (base.transform.position.x > 400f) : (base.transform.position.x < -400f))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x040031AE RID: 12718
	private const float SPAWN_X = 400f;

	// Token: 0x040031AF RID: 12719
	private const float SPAWN_Y = 250f;

	// Token: 0x040031B0 RID: 12720
	private LevelProperties.RetroArcade.Robots properties;

	// Token: 0x040031B1 RID: 12721
	private RetroArcadeBonusRobot.Direction direction;

	// Token: 0x02000751 RID: 1873
	public enum Direction
	{
		// Token: 0x040031B3 RID: 12723
		Left,
		// Token: 0x040031B4 RID: 12724
		Right
	}
}
