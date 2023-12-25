using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006A4 RID: 1700
public class FlyingMermaidLevelYellProjectile : AbstractProjectile
{
	// Token: 0x0600240B RID: 9227 RVA: 0x0015279C File Offset: 0x00150B9C
	public FlyingMermaidLevelYellProjectile Create(Vector2 pos, float trackSpeed, float angle, AbstractPlayerController target)
	{
		FlyingMermaidLevelYellProjectile flyingMermaidLevelYellProjectile = base.Create() as FlyingMermaidLevelYellProjectile;
		flyingMermaidLevelYellProjectile.trackSpeed = trackSpeed;
		flyingMermaidLevelYellProjectile.target = target;
		flyingMermaidLevelYellProjectile.direction = MathUtils.AngleToDirection(angle);
		flyingMermaidLevelYellProjectile.transform.position = pos;
		return flyingMermaidLevelYellProjectile;
	}

	// Token: 0x0600240C RID: 9228 RVA: 0x001527E2 File Offset: 0x00150BE2
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600240D RID: 9229 RVA: 0x001527F7 File Offset: 0x00150BF7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600240E RID: 9230 RVA: 0x00152818 File Offset: 0x00150C18
	private IEnumerator move_cr()
	{
		float speed = this.launchSpeed;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.FixedDelta;
			FlyingMermaidLevelYellProjectile.State state = this.state;
			if (state != FlyingMermaidLevelYellProjectile.State.Slowing)
			{
				if (state != FlyingMermaidLevelYellProjectile.State.Stopped)
				{
					if (state == FlyingMermaidLevelYellProjectile.State.Tracking)
					{
						if (t < this.attackEaseTime)
						{
							speed = EaseUtils.EaseInSine(0f, this.trackSpeed, t / this.attackEaseTime);
						}
						else
						{
							speed = this.trackSpeed;
						}
					}
				}
				else if (t >= this.waitTime)
				{
					this.state = FlyingMermaidLevelYellProjectile.State.Tracking;
					t = 0f;
					if (this.target == null || this.target.IsDead)
					{
						this.target = PlayerManager.GetNext();
					}
					if (this.target != null)
					{
						this.direction = (this.target.center - base.transform.position).normalized;
						base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(this.direction) + 180f));
						base.animator.SetTrigger("Continue");
					}
				}
			}
			else if (t < this.stopTime)
			{
				speed = EaseUtils.EaseOutSine(this.launchSpeed, 0f, t / this.stopTime);
			}
			else
			{
				speed = 0f;
				this.state = FlyingMermaidLevelYellProjectile.State.Stopped;
				t = 0f;
			}
			Vector2 pos = base.transform.localPosition;
			pos += speed * CupheadTime.FixedDelta * this.direction;
			base.transform.localPosition = pos;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04002CD6 RID: 11478
	public float launchSpeed;

	// Token: 0x04002CD7 RID: 11479
	public float stopTime;

	// Token: 0x04002CD8 RID: 11480
	public float waitTime;

	// Token: 0x04002CD9 RID: 11481
	public float attackEaseTime;

	// Token: 0x04002CDA RID: 11482
	private FlyingMermaidLevelYellProjectile.State state;

	// Token: 0x04002CDB RID: 11483
	private float trackSpeed;

	// Token: 0x04002CDC RID: 11484
	private AbstractPlayerController target;

	// Token: 0x04002CDD RID: 11485
	private Vector2 direction;

	// Token: 0x020006A5 RID: 1701
	public enum State
	{
		// Token: 0x04002CDF RID: 11487
		Slowing,
		// Token: 0x04002CE0 RID: 11488
		Stopped,
		// Token: 0x04002CE1 RID: 11489
		Tracking
	}
}
