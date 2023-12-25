using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003EA RID: 1002
public class AbstractCollidableObject : AbstractPausableComponent
{
	// Token: 0x06000D72 RID: 3442 RVA: 0x00008489 File Offset: 0x00006889
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.UnregisterAllCollisionChildren();
	}

	// Token: 0x06000D73 RID: 3443 RVA: 0x00008497 File Offset: 0x00006897
	protected virtual void OnTriggerEnter2D(Collider2D col)
	{
		this.checkCollision(col, CollisionPhase.Enter);
	}

	// Token: 0x06000D74 RID: 3444 RVA: 0x000084A1 File Offset: 0x000068A1
	protected virtual void OnCollisionEnter2D(Collision2D col)
	{
		this.checkCollision(col.collider, CollisionPhase.Enter);
	}

	// Token: 0x06000D75 RID: 3445 RVA: 0x000084B0 File Offset: 0x000068B0
	protected virtual void OnTriggerStay2D(Collider2D col)
	{
		this.checkCollision(col, CollisionPhase.Stay);
	}

	// Token: 0x06000D76 RID: 3446 RVA: 0x000084BA File Offset: 0x000068BA
	protected virtual void OnCollisionStay2D(Collision2D col)
	{
		this.checkCollision(col.collider, CollisionPhase.Stay);
	}

	// Token: 0x06000D77 RID: 3447 RVA: 0x000084C9 File Offset: 0x000068C9
	protected virtual void OnTriggerExit2D(Collider2D col)
	{
		this.checkCollision(col, CollisionPhase.Exit);
	}

	// Token: 0x06000D78 RID: 3448 RVA: 0x000084D3 File Offset: 0x000068D3
	protected virtual void OnCollisionExit2D(Collision2D col)
	{
		this.checkCollision(col.collider, CollisionPhase.Exit);
	}

	// Token: 0x06000D79 RID: 3449 RVA: 0x000084E4 File Offset: 0x000068E4
	protected virtual void checkCollision(Collider2D col, CollisionPhase phase)
	{
		GameObject gameObject = col.gameObject;
		this.OnCollision(gameObject, phase);
		if (gameObject.CompareTag("Wall"))
		{
			this.OnCollisionWalls(gameObject, phase);
		}
		else if (gameObject.CompareTag("Ceiling"))
		{
			this.OnCollisionCeiling(gameObject, phase);
		}
		else if (gameObject.CompareTag("Ground"))
		{
			this.OnCollisionGround(gameObject, phase);
		}
		else if (gameObject.CompareTag("Enemy"))
		{
			if (this.allowCollisionEnemy)
			{
				this.OnCollisionEnemy(gameObject, phase);
			}
		}
		else if (gameObject.CompareTag("EnemyProjectile"))
		{
			this.OnCollisionEnemyProjectile(gameObject, phase);
		}
		else if (gameObject.CompareTag("Player"))
		{
			if (this.allowCollisionPlayer)
			{
				this.OnCollisionPlayer(gameObject, phase);
			}
		}
		else if (gameObject.CompareTag("PlayerProjectile"))
		{
			this.OnCollisionPlayerProjectile(gameObject, phase);
		}
		else
		{
			this.OnCollisionOther(gameObject, phase);
		}
	}

	// Token: 0x17000242 RID: 578
	// (get) Token: 0x06000D7A RID: 3450 RVA: 0x000085E9 File Offset: 0x000069E9
	protected virtual bool allowCollisionPlayer
	{
		get
		{
			return true;
		}
	}

	// Token: 0x17000243 RID: 579
	// (get) Token: 0x06000D7B RID: 3451 RVA: 0x000085EC File Offset: 0x000069EC
	protected virtual bool allowCollisionEnemy
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06000D7C RID: 3452 RVA: 0x000085EF File Offset: 0x000069EF
	protected virtual void OnCollision(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D7D RID: 3453 RVA: 0x000085F1 File Offset: 0x000069F1
	protected virtual void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D7E RID: 3454 RVA: 0x000085F3 File Offset: 0x000069F3
	protected virtual void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D7F RID: 3455 RVA: 0x000085F5 File Offset: 0x000069F5
	protected virtual void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D80 RID: 3456 RVA: 0x000085F7 File Offset: 0x000069F7
	protected virtual void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D81 RID: 3457 RVA: 0x000085F9 File Offset: 0x000069F9
	protected virtual void OnCollisionEnemyProjectile(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D82 RID: 3458 RVA: 0x000085FB File Offset: 0x000069FB
	protected virtual void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D83 RID: 3459 RVA: 0x000085FD File Offset: 0x000069FD
	protected virtual void OnCollisionPlayerProjectile(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D84 RID: 3460 RVA: 0x000085FF File Offset: 0x000069FF
	protected virtual void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
	}

	// Token: 0x06000D85 RID: 3461 RVA: 0x00008604 File Offset: 0x00006A04
	protected void RegisterCollisionChild(GameObject go)
	{
		CollisionChild component = go.GetComponent<CollisionChild>();
		if (component == null)
		{
			return;
		}
		this.RegisterCollisionChild(component);
	}

	// Token: 0x06000D86 RID: 3462 RVA: 0x0000862C File Offset: 0x00006A2C
	public void RegisterCollisionChild(CollisionChild s)
	{
		this.collisionChildren.Add(s);
		s.OnAnyCollision += this.OnCollision;
		s.OnWallCollision += this.OnCollisionWalls;
		s.OnGroundCollision += this.OnCollisionGround;
		s.OnCeilingCollision += this.OnCollisionCeiling;
		s.OnPlayerCollision += this.OnCollisionPlayer;
		s.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
		s.OnEnemyCollision += this.OnCollisionEnemy;
		s.OnEnemyProjectileCollision += this.OnCollisionEnemyProjectile;
		s.OnOtherCollision += this.OnCollisionOther;
	}

	// Token: 0x06000D87 RID: 3463 RVA: 0x000086F0 File Offset: 0x00006AF0
	protected void UnregisterCollisionChild(CollisionChild s)
	{
		if (this.collisionChildren.Contains(s))
		{
			s.OnAnyCollision -= this.OnCollision;
			s.OnWallCollision -= this.OnCollisionWalls;
			s.OnGroundCollision -= this.OnCollisionGround;
			s.OnCeilingCollision -= this.OnCollisionCeiling;
			s.OnPlayerCollision -= this.OnCollisionPlayer;
			s.OnPlayerProjectileCollision -= this.OnCollisionPlayerProjectile;
			s.OnEnemyCollision -= this.OnCollisionEnemy;
			s.OnEnemyProjectileCollision -= this.OnCollisionEnemyProjectile;
			s.OnOtherCollision -= this.OnCollisionOther;
			this.collisionChildren.Remove(s);
		}
	}

	// Token: 0x06000D88 RID: 3464 RVA: 0x000087C8 File Offset: 0x00006BC8
	protected void UnregisterAllCollisionChildren()
	{
		for (int i = this.collisionChildren.Count - 1; i >= 0; i--)
		{
			this.UnregisterCollisionChild(this.collisionChildren[i]);
		}
	}

	// Token: 0x04001702 RID: 5890
	private List<CollisionChild> collisionChildren = new List<CollisionChild>();
}
