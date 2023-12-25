using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000755 RID: 1877
public class RetroArcadeSheriff : RetroArcadeEnemy
{
	// Token: 0x060028E8 RID: 10472 RVA: 0x0017CDC8 File Offset: 0x0017B1C8
	public RetroArcadeSheriff Create(Vector3 pos, float speed, bool clockwise, float offset, LevelProperties.RetroArcade.Sheriff properties)
	{
		RetroArcadeSheriff retroArcadeSheriff = this.InstantiatePrefab<RetroArcadeSheriff>();
		retroArcadeSheriff.transform.position = pos;
		retroArcadeSheriff.properties = properties;
		retroArcadeSheriff.speed = speed;
		retroArcadeSheriff.clockwise = clockwise;
		retroArcadeSheriff.offset = offset;
		return retroArcadeSheriff;
	}

	// Token: 0x060028E9 RID: 10473 RVA: 0x0017CE07 File Offset: 0x0017B207
	protected override void Start()
	{
		this.side = RetroArcadeSheriff.Side.Right;
		this.SelectDirection();
	}

	// Token: 0x060028EA RID: 10474 RVA: 0x0017CE18 File Offset: 0x0017B218
	private void SelectDirection()
	{
		if (base.transform.position.y > 200f)
		{
			this.side = RetroArcadeSheriff.Side.Top;
			this.direction = ((!this.clockwise) ? Vector3.left : Vector3.right);
		}
		else if (base.transform.position.y < -100f)
		{
			this.side = RetroArcadeSheriff.Side.Bottom;
			this.direction = ((!this.clockwise) ? Vector3.right : Vector3.left);
		}
		else if (base.transform.position.x < 0f)
		{
			this.side = RetroArcadeSheriff.Side.Left;
			this.direction = ((!this.clockwise) ? Vector3.down : Vector3.up);
		}
		else
		{
			this.side = RetroArcadeSheriff.Side.Right;
			this.direction = ((!this.clockwise) ? Vector3.up : Vector3.down);
		}
	}

	// Token: 0x060028EB RID: 10475 RVA: 0x0017CF27 File Offset: 0x0017B327
	public void StartMoving()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060028EC RID: 10476 RVA: 0x0017CF38 File Offset: 0x0017B338
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			switch (this.side)
			{
			case RetroArcadeSheriff.Side.Top:
				if (this.clockwise && base.transform.position.x >= (float)Level.Current.Right - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Right;
					this.direction = Vector3.down;
				}
				else if (!this.clockwise && base.transform.position.x <= (float)Level.Current.Left + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Left;
					this.direction = Vector3.down;
				}
				break;
			case RetroArcadeSheriff.Side.Bottom:
				if (this.clockwise && base.transform.position.x <= (float)Level.Current.Left + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Left;
					this.direction = Vector3.up;
				}
				else if (!this.clockwise && base.transform.position.x >= (float)Level.Current.Right - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Right;
					this.direction = Vector3.up;
				}
				break;
			case RetroArcadeSheriff.Side.Left:
				if (this.clockwise && base.transform.position.y >= (float)Level.Current.Ceiling - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Top;
					this.direction = Vector3.right;
				}
				else if (!this.clockwise && base.transform.position.y <= (float)Level.Current.Ground + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Bottom;
					this.direction = Vector3.right;
				}
				break;
			case RetroArcadeSheriff.Side.Right:
				if (!this.clockwise && base.transform.position.y >= (float)Level.Current.Ceiling - this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Top;
					this.direction = Vector3.left;
				}
				else if (this.clockwise && base.transform.position.y <= (float)Level.Current.Ground + this.offset)
				{
					this.side = RetroArcadeSheriff.Side.Bottom;
					this.direction = Vector3.left;
				}
				break;
			}
			Vector3 pos = base.transform.position;
			pos.x = Mathf.Clamp(base.transform.position.x, (float)Level.Current.Left + this.offset, (float)Level.Current.Right - this.offset);
			pos.y = Mathf.Clamp(base.transform.position.y, (float)Level.Current.Ground + this.offset, (float)Level.Current.Ceiling - this.offset);
			base.transform.position = pos;
			base.transform.position += this.direction * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060028ED RID: 10477 RVA: 0x0017CF54 File Offset: 0x0017B354
	public void Shoot(AbstractPlayerController player)
	{
		Vector3 v = player.transform.position - base.transform.position;
		this.projectile.Create(base.transform.position, MathUtils.DirectionToAngle(v), this.properties.shotSpeed);
	}

	// Token: 0x040031C5 RID: 12741
	public float speed;

	// Token: 0x040031C6 RID: 12742
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x040031C7 RID: 12743
	private const float Y_TOP_THRESHOLD = 200f;

	// Token: 0x040031C8 RID: 12744
	private const float Y_BOTTOM_THRESHOLD = -100f;

	// Token: 0x040031C9 RID: 12745
	private const float X_THRESHOLD = 290f;

	// Token: 0x040031CA RID: 12746
	public RetroArcadeSheriff.Side side;

	// Token: 0x040031CB RID: 12747
	private LevelProperties.RetroArcade.Sheriff properties;

	// Token: 0x040031CC RID: 12748
	private float offset;

	// Token: 0x040031CD RID: 12749
	private bool clockwise;

	// Token: 0x040031CE RID: 12750
	private Vector3 direction;

	// Token: 0x040031CF RID: 12751
	private Vector2 targetPos;

	// Token: 0x02000756 RID: 1878
	public enum Side
	{
		// Token: 0x040031D1 RID: 12753
		Top,
		// Token: 0x040031D2 RID: 12754
		Bottom,
		// Token: 0x040031D3 RID: 12755
		Left,
		// Token: 0x040031D4 RID: 12756
		Right
	}
}
