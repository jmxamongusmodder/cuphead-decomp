using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200073D RID: 1853
public class RetroArcadeCaterpillarBodyPart : RetroArcadeEnemy
{
	// Token: 0x06002868 RID: 10344 RVA: 0x00178BE8 File Offset: 0x00176FE8
	public RetroArcadeCaterpillarBodyPart Create(int index, RetroArcadeCaterpillarBodyPart.Direction direction, RetroArcadeCaterpillarManager manager, LevelProperties.RetroArcade.Caterpillar properties)
	{
		RetroArcadeCaterpillarBodyPart retroArcadeCaterpillarBodyPart = this.InstantiatePrefab<RetroArcadeCaterpillarBodyPart>();
		retroArcadeCaterpillarBodyPart.transform.SetPosition(new float?((direction != RetroArcadeCaterpillarBodyPart.Direction.Right) ? 320f : -320f), new float?(300f + (float)index * 50f), null);
		retroArcadeCaterpillarBodyPart.direction = direction;
		retroArcadeCaterpillarBodyPart.manager = manager;
		retroArcadeCaterpillarBodyPart.properties = properties;
		retroArcadeCaterpillarBodyPart.manager = manager;
		retroArcadeCaterpillarBodyPart.targetPos = new Vector2(retroArcadeCaterpillarBodyPart.transform.position.x, 230f);
		retroArcadeCaterpillarBodyPart.moveY = true;
		retroArcadeCaterpillarBodyPart.hp = properties.hp;
		return retroArcadeCaterpillarBodyPart;
	}

	// Token: 0x06002869 RID: 10345 RVA: 0x00178C93 File Offset: 0x00177093
	protected override void Start()
	{
		base.PointsWorth = this.properties.pointsGained;
		base.PointsBonus = this.properties.pointsBonus;
	}

	// Token: 0x0600286A RID: 10346 RVA: 0x00178CB8 File Offset: 0x001770B8
	protected override void FixedUpdate()
	{
		if (this.movingY)
		{
			return;
		}
		float num = this.manager.moveSpeed * CupheadTime.FixedDelta;
		float magnitude = (this.targetPos - base.transform.position).magnitude;
		if (magnitude > num)
		{
			this.move(num);
		}
		else
		{
			base.transform.position = this.targetPos;
			if (this.moveY)
			{
				this.moveY = false;
				this.targetPos = new Vector2((this.direction != RetroArcadeCaterpillarBodyPart.Direction.Left) ? 320f : -320f, base.transform.position.y);
				if (this.atBottom && this.isHead)
				{
					this.manager.OnReachBottom();
				}
			}
			else
			{
				this.moveY = true;
				this.direction = ((this.direction != RetroArcadeCaterpillarBodyPart.Direction.Left) ? RetroArcadeCaterpillarBodyPart.Direction.Left : RetroArcadeCaterpillarBodyPart.Direction.Right);
				if (this.atBottom)
				{
					this.targetPos = new Vector2(base.transform.position.x, 230f);
					this.atBottom = false;
				}
				else if (this.timesDropped >= this.properties.dropCount)
				{
					this.targetPos = new Vector2(base.transform.position.x, -120f);
					this.timesDropped = 0;
					this.atBottom = true;
				}
				else
				{
					this.targetPos = new Vector2(base.transform.position.x, base.transform.position.y - 50f);
					this.timesDropped++;
					if (this.bulletPrefab != null)
					{
						this.Shoot();
					}
				}
			}
			this.move(num - magnitude);
		}
	}

	// Token: 0x0600286B RID: 10347 RVA: 0x00178EB0 File Offset: 0x001772B0
	private void move(float distance)
	{
		base.transform.position = base.transform.position + (this.targetPos - base.transform.position).normalized * distance;
	}

	// Token: 0x0600286C RID: 10348 RVA: 0x00178F0B File Offset: 0x0017730B
	public override void Dead()
	{
		base.Dead();
		if (!this.isHead)
		{
			this.manager.OnBodyPartDie(this);
		}
	}

	// Token: 0x0600286D RID: 10349 RVA: 0x00178F2C File Offset: 0x0017732C
	public void Shoot()
	{
		float rotation = MathUtils.DirectionToAngle(PlayerManager.GetNext().transform.position - this.bulletRoot.position);
		this.bulletPrefab.Create(this.bulletRoot.position, rotation, this.properties.shotSpeed);
	}

	// Token: 0x0600286E RID: 10350 RVA: 0x00178F8B File Offset: 0x0017738B
	public void OnWaveEnd()
	{
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x0600286F RID: 10351 RVA: 0x00178F9C File Offset: 0x0017739C
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(420f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003126 RID: 12582
	private const float TOP_Y = 230f;

	// Token: 0x04003127 RID: 12583
	private const float BOTTOM_Y = -120f;

	// Token: 0x04003128 RID: 12584
	private const float SPACING = 50f;

	// Token: 0x04003129 RID: 12585
	private const float OFFSCREEN_Y = 300f;

	// Token: 0x0400312A RID: 12586
	private const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x0400312B RID: 12587
	public const float TURNAROUND_X = 320f;

	// Token: 0x0400312C RID: 12588
	[SerializeField]
	private BasicProjectile bulletPrefab;

	// Token: 0x0400312D RID: 12589
	[SerializeField]
	private Transform bulletRoot;

	// Token: 0x0400312E RID: 12590
	[SerializeField]
	private bool isHead;

	// Token: 0x0400312F RID: 12591
	private LevelProperties.RetroArcade.Caterpillar properties;

	// Token: 0x04003130 RID: 12592
	private RetroArcadeCaterpillarManager manager;

	// Token: 0x04003131 RID: 12593
	private RetroArcadeCaterpillarBodyPart.Direction direction;

	// Token: 0x04003132 RID: 12594
	private Vector2 targetPos;

	// Token: 0x04003133 RID: 12595
	private bool moveY;

	// Token: 0x04003134 RID: 12596
	private int timesDropped;

	// Token: 0x04003135 RID: 12597
	private bool atBottom;

	// Token: 0x0200073E RID: 1854
	public enum Direction
	{
		// Token: 0x04003137 RID: 12599
		Left,
		// Token: 0x04003138 RID: 12600
		Right
	}
}
