using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006BB RID: 1723
public class FrogsLevelShortClapBullet : AbstractProjectile
{
	// Token: 0x06002495 RID: 9365 RVA: 0x0015702C File Offset: 0x0015542C
	public FrogsLevelShortClapBullet Create(FrogsLevelShort.Direction frogDir, FrogsLevelShortClapBullet.Direction dir, Vector2 pos, Vector2 velocity)
	{
		FrogsLevelShortClapBullet frogsLevelShortClapBullet = base.Create(pos) as FrogsLevelShortClapBullet;
		frogsLevelShortClapBullet.CollisionDeath.OnlyPlayer();
		frogsLevelShortClapBullet.DamagesType.OnlyPlayer();
		frogsLevelShortClapBullet.Init(frogDir, dir, pos, velocity);
		return frogsLevelShortClapBullet;
	}

	// Token: 0x06002496 RID: 9366 RVA: 0x00157069 File Offset: 0x00155469
	private void Init(FrogsLevelShort.Direction frogDir, FrogsLevelShortClapBullet.Direction dir, Vector2 pos, Vector2 velocity)
	{
		this.frogDirection = frogDir;
		this.velocity = velocity;
		this.direction = dir;
		base.StartCoroutine(this.go_cr());
	}

	// Token: 0x06002497 RID: 9367 RVA: 0x0015708E File Offset: 0x0015548E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002498 RID: 9368 RVA: 0x001570A4 File Offset: 0x001554A4
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
	}

	// Token: 0x06002499 RID: 9369 RVA: 0x001570B4 File Offset: 0x001554B4
	private IEnumerator go_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float upY = this.velocity.y;
		float downY = -this.velocity.y;
		float x = (this.frogDirection != FrogsLevelShort.Direction.Right) ? (-this.velocity.x) : this.velocity.x;
		float y = (this.direction != FrogsLevelShortClapBullet.Direction.Up) ? downY : upY;
		if (this.direction == FrogsLevelShortClapBullet.Direction.Up)
		{
			base.transform.LookAt2D(base.transform.position + new Vector2(x, upY));
		}
		else
		{
			base.transform.LookAt2D(base.transform.position + new Vector2(x, downY));
		}
		for (;;)
		{
			if (this.direction == FrogsLevelShortClapBullet.Direction.Up)
			{
				if (base.transform.position.y >= 360f)
				{
					AudioManager.Play("level_frogs_short_clap_bounce");
					this.emitAudioFromObject.Add("level_frogs_short_clap_bounce");
					this.direction = FrogsLevelShortClapBullet.Direction.Down;
					this.bounceEffect.Create(base.transform.position, new Vector3(1f, -1f, 1f));
					y = downY;
					base.transform.LookAt2D(base.transform.position + new Vector2(x, y));
				}
			}
			else if (base.transform.position.y <= (float)Level.Current.Ground)
			{
				AudioManager.Play("level_frogs_short_clap_bounce");
				this.emitAudioFromObject.Add("level_frogs_short_clap_bounce");
				this.direction = FrogsLevelShortClapBullet.Direction.Up;
				this.bounceEffect.Create(base.transform.position);
				y = upY;
				base.transform.LookAt2D(base.transform.position + new Vector2(x, y));
			}
			if (base.transform.position.x > 640f + base.GetComponent<SpriteRenderer>().bounds.size.x / 2f)
			{
				break;
			}
			base.transform.AddPosition(x * CupheadTime.FixedDelta, y * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x04002D40 RID: 11584
	private const float MAX_Y = 360f;

	// Token: 0x04002D41 RID: 11585
	[SerializeField]
	private Effect bounceEffect;

	// Token: 0x04002D42 RID: 11586
	private Vector2 velocity;

	// Token: 0x04002D43 RID: 11587
	private FrogsLevelShort.Direction frogDirection;

	// Token: 0x04002D44 RID: 11588
	private FrogsLevelShortClapBullet.Direction direction;

	// Token: 0x020006BC RID: 1724
	public enum Direction
	{
		// Token: 0x04002D46 RID: 11590
		Up,
		// Token: 0x04002D47 RID: 11591
		Down
	}
}
