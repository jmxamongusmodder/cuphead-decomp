using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000747 RID: 1863
public class RetroArcadePaddleShip : RetroArcadeEnemy
{
	// Token: 0x06002897 RID: 10391 RVA: 0x0017AAD8 File Offset: 0x00178ED8
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002898 RID: 10392 RVA: 0x0017AAE4 File Offset: 0x00178EE4
	public void StartPaddleShip()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.paddleShip;
		this.ySpeed = this.p.ySpeed.RandomFloat();
		base.PointsBonus = this.p.pointsBonus;
		base.PointsWorth = this.p.pointsGained;
		base.transform.SetPosition(new float?(UnityEngine.Random.Range(-300f, 300f)), new float?(350f), null);
		this.moveDir = new Trilean2((!Rand.Bool()) ? 1 : -1, -1);
		this.hp = this.p.hp;
		base.StartCoroutine(this.moveY_cr());
		base.StartCoroutine(this.moveX_cr());
	}

	// Token: 0x06002899 RID: 10393 RVA: 0x0017ABC8 File Offset: 0x00178FC8
	private IEnumerator moveY_cr()
	{
		while (base.transform.position.y > (float)Level.Current.Ceiling - 80f)
		{
			yield return new WaitForFixedUpdate();
			base.transform.AddPosition(0f, -this.ySpeed * CupheadTime.FixedDelta, 0f);
		}
		base.transform.SetPosition(null, new float?((float)Level.Current.Ceiling - 80f), null);
		while (this.paddle.position.y > (float)Level.Current.Ground + 20f)
		{
			yield return new WaitForFixedUpdate();
			this.paddle.AddPosition(0f, -this.ySpeed * CupheadTime.FixedDelta, 0f);
		}
		this.paddle.SetPosition(null, new float?((float)Level.Current.Ground + 20f), null);
		for (;;)
		{
			yield return new WaitForFixedUpdate();
			if ((this.moveDir.y > 0 && base.transform.position.y > (float)Level.Current.Ceiling - 80f) || (this.moveDir.y < 0 && base.transform.position.y < (float)Level.Current.Ground + 80f))
			{
				this.moveDir.y = this.moveDir.y * -1;
			}
			base.transform.AddPosition(0f, this.moveDir.y * this.ySpeed * CupheadTime.FixedDelta, 0f);
			this.paddle.SetPosition(null, new float?((float)Level.Current.Ground + 20f), null);
		}
		yield break;
	}

	// Token: 0x0600289A RID: 10394 RVA: 0x0017ABE4 File Offset: 0x00178FE4
	private IEnumerator moveX_cr()
	{
		for (;;)
		{
			yield return new WaitForFixedUpdate();
			if ((this.moveDir.x > 0 && base.transform.position.x > 300f) || (this.moveDir.x < 0 && base.transform.position.x < -300f))
			{
				this.moveDir.x = this.moveDir.x * -1;
			}
			base.transform.AddPosition(this.moveDir.x * this.p.xSpeed * CupheadTime.FixedDelta, 0f, 0f);
		}
		yield break;
	}

	// Token: 0x0600289B RID: 10395 RVA: 0x0017AC00 File Offset: 0x00179000
	public override void Dead()
	{
		this.StopAllCoroutines();
		foreach (Collider2D collider2D in base.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.IsDead = true;
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.color = new Color(0f, 0f, 0f, 0.25f);
		}
		this.properties.DealDamageToNextNamedState();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x0600289C RID: 10396 RVA: 0x0017ACA0 File Offset: 0x001790A0
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(350f - ((float)Level.Current.Ground + 20f), 500f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x0400316C RID: 12652
	private const float OFFSCREEN_Y = 350f;

	// Token: 0x0400316D RID: 12653
	private const float TURNAROUND_X = 300f;

	// Token: 0x0400316E RID: 12654
	private const float PADDLE_PADDING = 20f;

	// Token: 0x0400316F RID: 12655
	private const float SHIP_PADDING = 80f;

	// Token: 0x04003170 RID: 12656
	private const float MOVE_OFFSCREEN_SPEED = 500f;

	// Token: 0x04003171 RID: 12657
	[SerializeField]
	private Transform paddle;

	// Token: 0x04003172 RID: 12658
	private LevelProperties.RetroArcade properties;

	// Token: 0x04003173 RID: 12659
	private LevelProperties.RetroArcade.PaddleShip p;

	// Token: 0x04003174 RID: 12660
	private float ySpeed;

	// Token: 0x04003175 RID: 12661
	private Trilean2 moveDir;
}
