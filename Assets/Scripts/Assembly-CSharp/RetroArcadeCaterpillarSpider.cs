using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000740 RID: 1856
public class RetroArcadeCaterpillarSpider : RetroArcadeEnemy
{
	// Token: 0x06002877 RID: 10359 RVA: 0x0017926C File Offset: 0x0017766C
	public RetroArcadeCaterpillarSpider Create(RetroArcadeCaterpillarSpider.Direction direction, LevelProperties.RetroArcade.Caterpillar properties)
	{
		RetroArcadeCaterpillarSpider retroArcadeCaterpillarSpider = this.InstantiatePrefab<RetroArcadeCaterpillarSpider>();
		retroArcadeCaterpillarSpider.transform.SetPosition(new float?((direction != RetroArcadeCaterpillarSpider.Direction.Right) ? 320f : -320f), new float?(300f), null);
		retroArcadeCaterpillarSpider.direction = direction;
		retroArcadeCaterpillarSpider.properties = properties;
		retroArcadeCaterpillarSpider.targetPos = new Vector2(retroArcadeCaterpillarSpider.transform.position.x, properties.spiderPathY.max);
		retroArcadeCaterpillarSpider.state = RetroArcadeCaterpillarSpider.State.Entering;
		retroArcadeCaterpillarSpider.hp = 1f;
		return retroArcadeCaterpillarSpider;
	}

	// Token: 0x06002878 RID: 10360 RVA: 0x00179304 File Offset: 0x00177704
	protected override void FixedUpdate()
	{
		if (base.IsDead)
		{
			return;
		}
		float num = this.properties.spiderSpeed * CupheadTime.FixedDelta;
		float magnitude = (this.targetPos - base.transform.position).magnitude;
		if (magnitude > num)
		{
			this.move(num);
		}
		else
		{
			base.transform.position = this.targetPos;
			switch (this.state)
			{
			case RetroArcadeCaterpillarSpider.State.Entering:
				this.state = RetroArcadeCaterpillarSpider.State.ZigZagDown;
				break;
			case RetroArcadeCaterpillarSpider.State.ZigZagDown:
			case RetroArcadeCaterpillarSpider.State.ZigZagUp:
				if (this.numZigZags >= this.properties.spiderNumZigZags)
				{
					this.state = RetroArcadeCaterpillarSpider.State.Leaving;
				}
				else
				{
					this.state = ((this.state != RetroArcadeCaterpillarSpider.State.ZigZagUp) ? RetroArcadeCaterpillarSpider.State.ZigZagUp : RetroArcadeCaterpillarSpider.State.ZigZagDown);
					this.numZigZags++;
				}
				break;
			case RetroArcadeCaterpillarSpider.State.Leaving:
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			RetroArcadeCaterpillarSpider.State state = this.state;
			if (state != RetroArcadeCaterpillarSpider.State.ZigZagUp && state != RetroArcadeCaterpillarSpider.State.ZigZagDown)
			{
				if (state == RetroArcadeCaterpillarSpider.State.Leaving)
				{
					this.targetPos.y = 300f;
				}
			}
			else
			{
				this.targetPos.x = (float)((this.direction != RetroArcadeCaterpillarSpider.Direction.Right) ? -1 : 1) * Mathf.Lerp(-320f, 320f, (float)this.numZigZags / (float)this.properties.spiderNumZigZags);
				this.targetPos.y = ((this.state != RetroArcadeCaterpillarSpider.State.ZigZagUp) ? this.properties.spiderPathY.min : this.properties.spiderPathY.max);
			}
			this.move(num - magnitude);
		}
	}

	// Token: 0x06002879 RID: 10361 RVA: 0x001794CC File Offset: 0x001778CC
	private void move(float distance)
	{
		base.transform.position = base.transform.position + (this.targetPos - base.transform.position).normalized * distance;
	}

	// Token: 0x0600287A RID: 10362 RVA: 0x00179527 File Offset: 0x00177927
	public override void Dead()
	{
		base.Dead();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x0600287B RID: 10363 RVA: 0x0017953C File Offset: 0x0017793C
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(300f - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003140 RID: 12608
	private const float OFFSCREEN_Y = 300f;

	// Token: 0x04003141 RID: 12609
	private const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x04003142 RID: 12610
	public const float MAX_X = 320f;

	// Token: 0x04003143 RID: 12611
	private LevelProperties.RetroArcade.Caterpillar properties;

	// Token: 0x04003144 RID: 12612
	private RetroArcadeCaterpillarSpider.Direction direction;

	// Token: 0x04003145 RID: 12613
	private Vector2 targetPos;

	// Token: 0x04003146 RID: 12614
	private RetroArcadeCaterpillarSpider.State state;

	// Token: 0x04003147 RID: 12615
	private int numZigZags;

	// Token: 0x02000741 RID: 1857
	public enum Direction
	{
		// Token: 0x04003149 RID: 12617
		Left,
		// Token: 0x0400314A RID: 12618
		Right
	}

	// Token: 0x02000742 RID: 1858
	public enum State
	{
		// Token: 0x0400314C RID: 12620
		Entering,
		// Token: 0x0400314D RID: 12621
		ZigZagDown,
		// Token: 0x0400314E RID: 12622
		ZigZagUp,
		// Token: 0x0400314F RID: 12623
		Leaving
	}
}
