using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000813 RID: 2067
public class TrainLevelEngineBossDropperProjectile : AbstractProjectile
{
	// Token: 0x06002FEC RID: 12268 RVA: 0x001C59C8 File Offset: 0x001C3DC8
	public TrainLevelEngineBossDropperProjectile Create(Vector2 pos, float upSpeed, float xSpeed, float gravity)
	{
		TrainLevelEngineBossDropperProjectile trainLevelEngineBossDropperProjectile = this.InstantiatePrefab<TrainLevelEngineBossDropperProjectile>();
		trainLevelEngineBossDropperProjectile.Init(pos, upSpeed, xSpeed, gravity);
		return trainLevelEngineBossDropperProjectile;
	}

	// Token: 0x06002FED RID: 12269 RVA: 0x001C59E8 File Offset: 0x001C3DE8
	private void Init(Vector2 pos, float upSpeed, float xSpeed, float gravity)
	{
		base.transform.position = pos;
		this.velocity.y = upSpeed;
		this.velocity.x = xSpeed;
		this.gravity = gravity;
		base.transform.localScale = Vector3.one * 0.5f;
		base.StartCoroutine(this.go_cr());
		base.StartCoroutine(this.scale_cr());
	}

	// Token: 0x06002FEE RID: 12270 RVA: 0x001C5A5C File Offset: 0x001C3E5C
	private IEnumerator scale_cr()
	{
		float t = 0f;
		while (t < 0.4f)
		{
			base.transform.localScale = Vector3.Lerp(Vector3.one * 0.5f, Vector3.one, t / 0.4f);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002FEF RID: 12271 RVA: 0x001C5A78 File Offset: 0x001C3E78
	private IEnumerator go_cr()
	{
		AbstractPlayerController target = PlayerManager.GetNext();
		while (base.transform.position.y > target.center.y)
		{
			Vector3 vel = Vector3.zero;
			base.transform.AddPosition(0f, this.velocity.y * CupheadTime.Delta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.Delta;
			yield return null;
			if (target == null || target.IsDead)
			{
				target = PlayerManager.GetNext();
			}
		}
		int direction = (target.center.x <= base.transform.position.x) ? -1 : 1;
		base.transform.localScale = new Vector3((float)(-(float)direction), 1f, 1f);
		base.animator.SetTrigger("Horizontal");
		this.dustFX.Create(base.transform.position, new Vector3((float)direction, 1f, 1f)).Play();
		this.verticalCollider.enabled = false;
		this.horizontalCollider.enabled = true;
		for (;;)
		{
			base.transform.AddPosition((float)direction * this.velocity.x * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002FF0 RID: 12272 RVA: 0x001C5A93 File Offset: 0x001C3E93
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
			this.Die();
		}
	}

	// Token: 0x06002FF1 RID: 12273 RVA: 0x001C5AB7 File Offset: 0x001C3EB7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.dustFX = null;
	}

	// Token: 0x040038C3 RID: 14531
	private const string HorizontalParameterName = "Horizontal";

	// Token: 0x040038C4 RID: 14532
	private const float ScaleTime = 0.4f;

	// Token: 0x040038C5 RID: 14533
	private const float StartScale = 0.5f;

	// Token: 0x040038C6 RID: 14534
	[SerializeField]
	private Effect dustFX;

	// Token: 0x040038C7 RID: 14535
	[SerializeField]
	private CircleCollider2D verticalCollider;

	// Token: 0x040038C8 RID: 14536
	[SerializeField]
	private BoxCollider2D horizontalCollider;

	// Token: 0x040038C9 RID: 14537
	private Vector2 velocity;

	// Token: 0x040038CA RID: 14538
	private float gravity;
}
