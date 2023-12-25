using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004EF RID: 1263
public class BaronessLevelGumballProjectile : AbstractProjectile
{
	// Token: 0x0600160D RID: 5645 RVA: 0x000C6028 File Offset: 0x000C4428
	public BaronessLevelGumballProjectile Create(Vector2 pos, Vector2 velocity, float gravity)
	{
		BaronessLevelGumballProjectile baronessLevelGumballProjectile = base.Create() as BaronessLevelGumballProjectile;
		baronessLevelGumballProjectile.velocity = velocity;
		baronessLevelGumballProjectile.transform.position = pos;
		baronessLevelGumballProjectile.gravity = gravity;
		return baronessLevelGumballProjectile;
	}

	// Token: 0x0600160E RID: 5646 RVA: 0x000C6061 File Offset: 0x000C4461
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.spawn_trail_cr());
	}

	// Token: 0x0600160F RID: 5647 RVA: 0x000C6078 File Offset: 0x000C4478
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (base.transform.position.y <= -360f)
		{
			this.Die();
		}
	}

	// Token: 0x06001610 RID: 5648 RVA: 0x000C60C4 File Offset: 0x000C44C4
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.isDead)
		{
			return;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
	}

	// Token: 0x06001611 RID: 5649 RVA: 0x000C6133 File Offset: 0x000C4533
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001612 RID: 5650 RVA: 0x000C6154 File Offset: 0x000C4554
	private IEnumerator spawn_trail_cr()
	{
		for (;;)
		{
			yield return null;
			this.trail.Create(base.transform.position);
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
		}
		yield break;
	}

	// Token: 0x06001613 RID: 5651 RVA: 0x000C616F File Offset: 0x000C456F
	protected override void Die()
	{
		this.StopAllCoroutines();
		this.isDead = true;
		base.Die();
		base.animator.SetTrigger("Death");
	}

	// Token: 0x06001614 RID: 5652 RVA: 0x000C6194 File Offset: 0x000C4594
	private void Kill()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001615 RID: 5653 RVA: 0x000C61A1 File Offset: 0x000C45A1
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.trail = null;
	}

	// Token: 0x04001F5C RID: 8028
	[SerializeField]
	private Effect trail;

	// Token: 0x04001F5D RID: 8029
	private Vector2 velocity;

	// Token: 0x04001F5E RID: 8030
	private float gravity;

	// Token: 0x04001F5F RID: 8031
	private bool isDead;
}
