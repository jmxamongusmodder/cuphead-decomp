using System;
using UnityEngine;

// Token: 0x02000549 RID: 1353
public class ChessQueenLevelCannonball : BasicProjectile
{
	// Token: 0x060018F9 RID: 6393 RVA: 0x000E281F File Offset: 0x000E0C1F
	protected override void Awake()
	{
		base.Awake();
		this.DamagesType.Player = false;
	}

	// Token: 0x060018FA RID: 6394 RVA: 0x000E2834 File Offset: 0x000E0C34
	public override AbstractProjectile Create(Vector2 position, float rotation)
	{
		ChessQueenLevelCannonball chessQueenLevelCannonball = this.Create(position) as ChessQueenLevelCannonball;
		chessQueenLevelCannonball.vel = MathUtils.AngleToDirection(rotation);
		return chessQueenLevelCannonball;
	}

	// Token: 0x060018FB RID: 6395 RVA: 0x000E2860 File Offset: 0x000E0C60
	protected override void Move()
	{
		if (this.hit)
		{
			return;
		}
		base.transform.position += this.vel * this.Speed * CupheadTime.FixedDelta;
		this.frameTimer += CupheadTime.FixedDelta;
		if (this.frameTimer >= 0.041666668f)
		{
			float num = Mathf.Lerp(1f, this.minScale, Mathf.InverseLerp(-150f, 440f, base.transform.position.y));
			this.sprite.transform.localScale = new Vector3(num, num);
			this.frameTimer -= 0.041666668f;
		}
	}

	// Token: 0x060018FC RID: 6396 RVA: 0x000E2928 File Offset: 0x000E0D28
	public void HitQueen()
	{
		this.hit = true;
		this.sprite.transform.localScale = new Vector3(1f, 1f);
		this.sprite.flipX = Rand.Bool();
		this.sprite.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		base.animator.Play("Explode");
		base.animator.Update(0f);
	}

	// Token: 0x0400220F RID: 8719
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x04002210 RID: 8720
	private Vector3 vel;

	// Token: 0x04002211 RID: 8721
	[SerializeField]
	private float minScale = 0.75f;

	// Token: 0x04002212 RID: 8722
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04002213 RID: 8723
	private float frameTimer;

	// Token: 0x04002214 RID: 8724
	private bool hit;
}
