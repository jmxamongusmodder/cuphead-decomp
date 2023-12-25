using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007BA RID: 1978
public class SallyStagePlayLevelUmbrellaProjectile : AbstractProjectile
{
	// Token: 0x06002CB2 RID: 11442 RVA: 0x001A5BCF File Offset: 0x001A3FCF
	protected override void Update()
	{
		this.damageDealer.Update();
		base.Update();
	}

	// Token: 0x06002CB3 RID: 11443 RVA: 0x001A5BE4 File Offset: 0x001A3FE4
	public void InitProjectile(LevelProperties.SallyStagePlay properties, int direction)
	{
		this.properties = properties;
		this.active = false;
		this.direction = direction;
		base.transform.SetScale(new float?((float)(-(float)direction)), null, null);
		this.currentVelocity = Vector3.down * properties.CurrentState.umbrella.objectSpeed;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.check_bounds_cr());
	}

	// Token: 0x06002CB4 RID: 11444 RVA: 0x001A5C68 File Offset: 0x001A4068
	private IEnumerator move_cr()
	{
		bool isFalling = false;
		for (;;)
		{
			if (this.active)
			{
				for (int i = 0; i < 2; i++)
				{
					AbstractPlayerController next = PlayerManager.GetNext();
					if (base.transform.position.x >= next.center.x - 10f && base.transform.position.x <= next.center.x + 10f)
					{
						if (!isFalling)
						{
							base.animator.SetTrigger("OnFall");
							isFalling = true;
						}
						this.currentVelocity = Vector3.down * this.properties.CurrentState.umbrella.objectDropSpeed;
					}
				}
			}
			base.transform.position += this.currentVelocity * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CB5 RID: 11445 RVA: 0x001A5C84 File Offset: 0x001A4084
	private IEnumerator check_bounds_cr()
	{
		float offset = 50f;
		bool goingVertically = false;
		bool goingUp = false;
		for (;;)
		{
			if (base.transform.position.y >= 360f - offset && goingVertically)
			{
				base.transform.position = new Vector3(base.transform.position.x, 360f - offset, 0f);
				this.currentVelocity = Vector3.left * this.properties.CurrentState.umbrella.objectSpeed * (float)this.direction;
				this.active = true;
				goingVertically = false;
			}
			else if (base.transform.position.y <= -360f + offset && goingVertically)
			{
				this.currentVelocity = Vector3.right * this.properties.CurrentState.umbrella.objectSpeed * (float)this.direction;
				goingVertically = false;
			}
			else if ((base.transform.position.x <= -630f && !goingVertically) || (base.transform.position.x >= 630f && !goingVertically))
			{
				if (!goingUp)
				{
					base.animator.SetTrigger("OnClimb");
					goingUp = true;
				}
				if (!this.dropped)
				{
					base.GetComponent<SpriteRenderer>().material = this.change;
					this.dropped = true;
				}
				this.currentVelocity = Vector3.up * this.properties.CurrentState.umbrella.objectSpeed;
				goingVertically = true;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002CB6 RID: 11446 RVA: 0x001A5C9F File Offset: 0x001A409F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x06002CB7 RID: 11447 RVA: 0x001A5CC4 File Offset: 0x001A40C4
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		if (this.active)
		{
			this.Die();
		}
		else if (!this.dropped)
		{
			base.GetComponent<SpriteRenderer>().material = this.change;
			base.animator.SetTrigger("OnDrop");
			this.dropped = true;
		}
		if (phase == CollisionPhase.Enter)
		{
			this.currentVelocity = Vector3.right * this.properties.CurrentState.umbrella.objectSpeed * (float)this.direction;
		}
		base.OnCollisionGround(hit, phase);
	}

	// Token: 0x06002CB8 RID: 11448 RVA: 0x001A5D5C File Offset: 0x001A415C
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.animator.SetTrigger("OnDeath");
		foreach (SpriteDeathParts spriteDeathParts in this.sprites)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
		base.Die();
	}

	// Token: 0x06002CB9 RID: 11449 RVA: 0x001A5DB6 File Offset: 0x001A41B6
	private void Kill()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002CBA RID: 11450 RVA: 0x001A5DC3 File Offset: 0x001A41C3
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.sprites = null;
	}

	// Token: 0x04003535 RID: 13621
	private bool active;

	// Token: 0x04003536 RID: 13622
	private bool dropped;

	// Token: 0x04003537 RID: 13623
	private int direction;

	// Token: 0x04003538 RID: 13624
	private Vector3 currentVelocity;

	// Token: 0x04003539 RID: 13625
	private LevelProperties.SallyStagePlay properties;

	// Token: 0x0400353A RID: 13626
	[SerializeField]
	private Material change;

	// Token: 0x0400353B RID: 13627
	[SerializeField]
	private SpriteDeathParts[] sprites;
}
