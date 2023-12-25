using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008F3 RID: 2291
public class MountainPlatformingLevelWallProjectile : AbstractProjectile
{
	// Token: 0x060035BE RID: 13758 RVA: 0x001F58CC File Offset: 0x001F3CCC
	public MountainPlatformingLevelWallProjectile Create(Vector2 pos, float rotation, Vector2 velocity, float gravity, float yGround)
	{
		MountainPlatformingLevelWallProjectile mountainPlatformingLevelWallProjectile = base.Create() as MountainPlatformingLevelWallProjectile;
		mountainPlatformingLevelWallProjectile.transform.position = pos;
		mountainPlatformingLevelWallProjectile.velocity = velocity;
		mountainPlatformingLevelWallProjectile.startVelocity = velocity;
		mountainPlatformingLevelWallProjectile.gravity = gravity;
		mountainPlatformingLevelWallProjectile.yGround = yGround;
		mountainPlatformingLevelWallProjectile.transform.SetEulerAngles(null, null, new float?(rotation));
		return mountainPlatformingLevelWallProjectile;
	}

	// Token: 0x060035BF RID: 13759 RVA: 0x001F5938 File Offset: 0x001F3D38
	protected override void Start()
	{
		base.Start();
		this.timeToApex = Mathf.Sqrt(2f * this.velocity.y / this.gravity);
		this.startVelocity.y = this.timeToApex * this.gravity;
		base.StartCoroutine(this.check_to_kill_cr());
	}

	// Token: 0x060035C0 RID: 13760 RVA: 0x001F5994 File Offset: 0x001F3D94
	private IEnumerator handle_hit_ground_cr()
	{
		yield return base.animator.WaitForAnimationToEnd(this, "Hit", false, true);
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.onGround = false;
		yield return null;
		yield break;
	}

	// Token: 0x060035C1 RID: 13761 RVA: 0x001F59AF File Offset: 0x001F3DAF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060035C2 RID: 13762 RVA: 0x001F59D0 File Offset: 0x001F3DD0
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.onGround)
		{
			return;
		}
		if (base.transform.position.y <= this.yGround)
		{
			this.onGround = true;
			this.HandleHitGround();
		}
	}

	// Token: 0x060035C3 RID: 13763 RVA: 0x001F5A30 File Offset: 0x001F3E30
	private void HandleHitGround()
	{
		this.velocity.y = this.startVelocity.y;
		base.animator.SetTrigger("OnHitGround");
		base.StartCoroutine(this.handle_hit_ground_cr());
	}

	// Token: 0x060035C4 RID: 13764 RVA: 0x001F5A68 File Offset: 0x001F3E68
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
		{
			return;
		}
		base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
		this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		base.transform.SetEulerAngles(null, null, new float?(Mathf.Atan2(-this.velocity.y, -this.velocity.x) * 57.29578f));
	}

	// Token: 0x060035C5 RID: 13765 RVA: 0x001F5B30 File Offset: 0x001F3F30
	private void ChangeRootEnd()
	{
		base.transform.position = this.root.transform.position;
		base.transform.SetEulerAngles(null, null, new float?(Mathf.Atan2(-this.velocity.y, -this.velocity.x) * 57.29578f));
	}

	// Token: 0x060035C6 RID: 13766 RVA: 0x001F5BA0 File Offset: 0x001F3FA0
	private void ChangeRootBeginning()
	{
		AudioManager.Play("castle_mountain_wall_oil_bounce");
		this.emitAudioFromObject.Add("castle_mountain_wall_oil_bounce");
		base.transform.position = this.root1.transform.position;
		base.transform.SetEulerAngles(null, null, new float?(0f));
	}

	// Token: 0x060035C7 RID: 13767 RVA: 0x001F5C0C File Offset: 0x001F400C
	private IEnumerator check_to_kill_cr()
	{
		while (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(0f, 1000f)))
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003DD2 RID: 15826
	[SerializeField]
	private Transform root;

	// Token: 0x04003DD3 RID: 15827
	[SerializeField]
	private Transform root1;

	// Token: 0x04003DD4 RID: 15828
	private Vector2 velocity;

	// Token: 0x04003DD5 RID: 15829
	private Vector2 startVelocity;

	// Token: 0x04003DD6 RID: 15830
	private float gravity;

	// Token: 0x04003DD7 RID: 15831
	private float timeToApex;

	// Token: 0x04003DD8 RID: 15832
	private float yGround;

	// Token: 0x04003DD9 RID: 15833
	private bool onGround;
}
