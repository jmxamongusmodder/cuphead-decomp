using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000768 RID: 1896
public class RetroArcadeWorm : RetroArcadeEnemy
{
	// Token: 0x0600294A RID: 10570 RVA: 0x00181309 File Offset: 0x0017F709
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x0600294B RID: 10571 RVA: 0x00181314 File Offset: 0x0017F714
	public void StartWorm()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.worm;
		this.hp = this.p.hp;
		base.PointsWorth = this.p.pointsGained;
		this.platform.Rise();
		base.transform.SetPosition(new float?(0f), new float?(-650f), null);
		this.direction = ((!Rand.Bool()) ? RetroArcadeWorm.Direction.Right : RetroArcadeWorm.Direction.Left);
		this.tongue.transform.parent = null;
		this.tongue.Init(this.p);
		this.tongue.Extend();
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.rocket_cr());
	}

	// Token: 0x0600294C RID: 10572 RVA: 0x001813F8 File Offset: 0x0017F7F8
	private IEnumerator move_cr()
	{
		base.MoveY(430f, 100f);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		for (;;)
		{
			float normalizedHpRemaining = this.hp / this.p.hp;
			float speed = this.p.moveSpeed.min * Mathf.Pow(this.p.moveSpeed.max / this.p.moveSpeed.min, 1f - normalizedHpRemaining);
			base.transform.AddPosition((float)((this.direction != RetroArcadeWorm.Direction.Left) ? 1 : -1) * speed * CupheadTime.FixedDelta, 0f, 0f);
			if ((this.direction == RetroArcadeWorm.Direction.Left && base.transform.position.x < -160f) || (this.direction == RetroArcadeWorm.Direction.Right && base.transform.position.x > 160f))
			{
				this.direction = ((this.direction != RetroArcadeWorm.Direction.Left) ? RetroArcadeWorm.Direction.Left : RetroArcadeWorm.Direction.Right);
			}
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x0600294D RID: 10573 RVA: 0x00181414 File Offset: 0x0017F814
	public override void Dead()
	{
		this.StopAllCoroutines();
		foreach (Collider2D collider2D in base.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		this.properties.DealDamageToNextNamedState();
		this.tongue.Retract();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x0600294E RID: 10574 RVA: 0x00181470 File Offset: 0x0017F870
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(-430f, 100f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0600294F RID: 10575 RVA: 0x0018148C File Offset: 0x0017F88C
	private IEnumerator rocket_cr()
	{
		RetroArcadeWormRocket.Direction rocketDirection = (!Rand.Bool()) ? RetroArcadeWormRocket.Direction.Right : RetroArcadeWormRocket.Direction.Left;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.p.rocketSpawnDelay);
			this.rocketPrefab.Create(rocketDirection, this.p);
		}
		yield break;
	}

	// Token: 0x04003246 RID: 12870
	private const float OFFSCREEN_Y = -650f;

	// Token: 0x04003247 RID: 12871
	private const float ONSCREEN_Y = -220f;

	// Token: 0x04003248 RID: 12872
	private const float MOVE_Y_SPEED = 100f;

	// Token: 0x04003249 RID: 12873
	private const float TURNAROUND_X = 160f;

	// Token: 0x0400324A RID: 12874
	private LevelProperties.RetroArcade properties;

	// Token: 0x0400324B RID: 12875
	private LevelProperties.RetroArcade.Worm p;

	// Token: 0x0400324C RID: 12876
	[SerializeField]
	private RetroArcadeWormPlatform platform;

	// Token: 0x0400324D RID: 12877
	[SerializeField]
	private RetroArcadeWormTongue tongue;

	// Token: 0x0400324E RID: 12878
	[SerializeField]
	private RetroArcadeWormRocket rocketPrefab;

	// Token: 0x0400324F RID: 12879
	private RetroArcadeWorm.Direction direction;

	// Token: 0x02000769 RID: 1897
	private enum Direction
	{
		// Token: 0x04003251 RID: 12881
		Left,
		// Token: 0x04003252 RID: 12882
		Right
	}
}
