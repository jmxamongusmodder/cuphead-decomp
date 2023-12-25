using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005DA RID: 1498
public class DicePalacePachinkoLevelPipeBall : AbstractProjectile
{
	// Token: 0x06001D95 RID: 7573 RVA: 0x0010FB90 File Offset: 0x0010DF90
	protected override void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (base.transform.position.y < (float)(Level.Current.Ground - 20))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		base.Update();
	}

	// Token: 0x06001D96 RID: 7574 RVA: 0x0010FBEC File Offset: 0x0010DFEC
	public void InitBall(LevelProperties.DicePalacePachinko properties)
	{
		this.properties = properties;
		this.directionIndex = UnityEngine.Random.Range(0, properties.CurrentState.balls.directionString.Split(new char[]
		{
			','
		}).Length);
		this.speed = properties.CurrentState.balls.movementSpeed;
		this.onGround = false;
		base.StartCoroutine(this.pick_dir_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001D97 RID: 7575 RVA: 0x0010FC68 File Offset: 0x0010E068
	private IEnumerator move_cr()
	{
		for (;;)
		{
			if (this.bouncing)
			{
				yield return null;
			}
			else
			{
				if (this.onGround)
				{
					base.transform.localPosition += Vector3.right * this.speed * CupheadTime.Delta;
				}
				else
				{
					base.transform.localPosition += Vector3.down * this.speed * CupheadTime.Delta;
				}
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06001D98 RID: 7576 RVA: 0x0010FC84 File Offset: 0x0010E084
	private IEnumerator pick_dir_cr()
	{
		while (!this.onGround)
		{
			yield return null;
		}
		this.directionIndex++;
		this.ChangeDirection();
		yield break;
	}

	// Token: 0x06001D99 RID: 7577 RVA: 0x0010FCA0 File Offset: 0x0010E0A0
	private void ChangeDirection()
	{
		if (this.directionIndex >= this.properties.CurrentState.balls.directionString.Split(new char[]
		{
			','
		}).Length)
		{
			this.directionIndex = 0;
		}
		char c = this.properties.CurrentState.balls.directionString.Split(new char[]
		{
			','
		})[this.directionIndex][0];
		if (c != 'L')
		{
			if (c == 'R')
			{
				this.speed = this.properties.CurrentState.balls.movementSpeed;
			}
		}
		else
		{
			this.speed = -this.properties.CurrentState.balls.movementSpeed;
		}
	}

	// Token: 0x06001D9A RID: 7578 RVA: 0x0010FD74 File Offset: 0x0010E174
	private IEnumerator changeState_cr(bool grounded, bool forceDirection)
	{
		yield return null;
		this.ChangeDirection();
		if (grounded)
		{
			this.onGround = true;
			if (this.currentCollider == this.lastCollider)
			{
				yield break;
			}
			base.animator.SetTrigger("Bounce");
			this.bouncing = true;
			yield return null;
			yield return base.animator.WaitForAnimationToEnd(this, "Bounce", 1, false, true);
			this.lastCollider = this.currentCollider;
			Animator platformAnimnator = this.currentCollider.GetComponent<Animator>();
			if (platformAnimnator == null)
			{
				this.bouncing = false;
				yield break;
			}
			if (base.transform.position.x - this.currentCollider.transform.position.x > 0f)
			{
				platformAnimnator.SetTrigger("Right");
				this.speed = this.properties.CurrentState.balls.movementSpeed;
			}
			else
			{
				platformAnimnator.SetTrigger("Left");
				this.speed = -this.properties.CurrentState.balls.movementSpeed;
			}
			base.transform.SetParent(platformAnimnator.transform, true);
			yield return null;
			this.bouncing = false;
			float finalSpeed = this.speed;
			float acceleration = 0f;
			while (this.onGround)
			{
				acceleration += CupheadTime.Delta;
				this.speed = Mathf.Min(Mathf.Lerp(0f, finalSpeed, acceleration * 2f), finalSpeed);
				yield return null;
			}
			platformAnimnator.SetTrigger("Back");
			base.transform.SetParent(null, true);
			base.transform.rotation = Quaternion.identity;
		}
		else
		{
			this.onGround = false;
			this.speed = this.properties.CurrentState.balls.movementSpeed;
		}
		yield break;
	}

	// Token: 0x06001D9B RID: 7579 RVA: 0x0010FD98 File Offset: 0x0010E198
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		if (phase == CollisionPhase.Enter && !this.onGround)
		{
			this.currentCollider = hit.GetComponent<Collider2D>();
			if (hit.GetComponent<LevelPlatform>() != null)
			{
				base.StartCoroutine(this.changeState_cr(true, true));
				base.OnCollisionOther(hit, phase);
			}
			else if (hit.GetComponent<DicePalacePachinkoLevelPeg>() != null)
			{
				base.StartCoroutine(this.changeState_cr(true, hit.GetComponent<DicePalacePachinkoLevelPeg>().forceDirection));
				base.OnCollisionOther(hit, phase);
			}
		}
		else if (phase == CollisionPhase.Exit)
		{
			base.StartCoroutine(this.changeState_cr(false, false));
			base.OnCollisionOther(hit, phase);
		}
	}

	// Token: 0x06001D9C RID: 7580 RVA: 0x0010FE44 File Offset: 0x0010E244
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x06001D9D RID: 7581 RVA: 0x0010FE4E File Offset: 0x0010E24E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06001D9E RID: 7582 RVA: 0x0010FE5D File Offset: 0x0010E25D
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04002675 RID: 9845
	private bool onGround;

	// Token: 0x04002676 RID: 9846
	private float speed;

	// Token: 0x04002677 RID: 9847
	private int directionIndex;

	// Token: 0x04002678 RID: 9848
	private LevelProperties.DicePalacePachinko properties;

	// Token: 0x04002679 RID: 9849
	private bool bouncing;

	// Token: 0x0400267A RID: 9850
	private Collider2D lastCollider;

	// Token: 0x0400267B RID: 9851
	private Collider2D currentCollider;
}
