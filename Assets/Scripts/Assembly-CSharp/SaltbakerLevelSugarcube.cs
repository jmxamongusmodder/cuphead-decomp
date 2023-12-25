using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007D8 RID: 2008
public class SaltbakerLevelSugarcube : SaltbakerLevelPhaseOneProjectile
{
	// Token: 0x06002DD8 RID: 11736 RVA: 0x001B0AAF File Offset: 0x001AEEAF
	protected override void OnDieDistance()
	{
	}

	// Token: 0x06002DD9 RID: 11737 RVA: 0x001B0AB1 File Offset: 0x001AEEB1
	protected override void OnDieLifetime()
	{
	}

	// Token: 0x06002DDA RID: 11738 RVA: 0x001B0AB4 File Offset: 0x001AEEB4
	public virtual SaltbakerLevelSugarcube Init(Vector2 pos, bool onLeft, LevelProperties.Saltbaker.Sugarcubes properties, float phase, SaltbakerLevelSaltbaker parent, int anim, bool parryable)
	{
		base.ResetLifetime();
		base.ResetDistance();
		this.root = pos;
		base.transform.position = pos;
		this.properties = properties;
		this.onLeft = onLeft;
		this.Move();
		this.phase = phase * 0.017453292f;
		base.animator.Play((!parryable) ? anim.ToString() : "Pink");
		this.SetParryable(parryable);
		return this;
	}

	// Token: 0x06002DDB RID: 11739 RVA: 0x001B0B3E File Offset: 0x001AEF3E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002DDC RID: 11740 RVA: 0x001B0B5C File Offset: 0x001AEF5C
	private new void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002DDD RID: 11741 RVA: 0x001B0B6C File Offset: 0x001AEF6C
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float xVelocity = this.properties.sineFreq * this.properties.sineWavelength;
		xVelocity = ((!this.onLeft) ? (-xVelocity) : xVelocity);
		base.transform.localScale = new Vector3(Mathf.Sign(xVelocity), 1f);
		float t = 0f;
		bool ismoving = true;
		while (ismoving)
		{
			t += CupheadTime.FixedDelta;
			Vector3 pos = base.transform.position;
			float yAbsolute = this.properties.sineAmplitude * Mathf.Sin(this.properties.sineFreq * t + this.phase);
			pos.y = this.root.y + yAbsolute;
			pos.x += xVelocity * CupheadTime.FixedDelta;
			base.transform.position = pos;
			base.HandleShadow(50f, 0f);
			if ((this.onLeft && base.transform.position.x > (float)Level.Current.Right + 50f) || (!this.onLeft && base.transform.position.x < (float)Level.Current.Left - 50f))
			{
				ismoving = false;
			}
			yield return wait;
		}
		this.Death();
		yield return null;
		yield break;
	}

	// Token: 0x06002DDE RID: 11742 RVA: 0x001B0B87 File Offset: 0x001AEF87
	private void Death()
	{
		this.Recycle<SaltbakerLevelSugarcube>();
	}

	// Token: 0x0400365E RID: 13918
	private LevelProperties.Saltbaker.Sugarcubes properties;

	// Token: 0x0400365F RID: 13919
	private Vector3 root;

	// Token: 0x04003660 RID: 13920
	private bool onLeft;

	// Token: 0x04003661 RID: 13921
	private float phase;
}
