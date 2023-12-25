using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007BD RID: 1981
public class SallyStagePlayLevelWindowProjectile : AbstractProjectile
{
	// Token: 0x06002CD5 RID: 11477 RVA: 0x001A6AA0 File Offset: 0x001A4EA0
	public SallyStagePlayLevelWindowProjectile Create(Vector2 pos, float rotation, float speed, SallyStagePlayLevel parent)
	{
		SallyStagePlayLevelWindowProjectile sallyStagePlayLevelWindowProjectile = base.Create() as SallyStagePlayLevelWindowProjectile;
		sallyStagePlayLevelWindowProjectile.transform.position = pos;
		sallyStagePlayLevelWindowProjectile.rotation = rotation;
		sallyStagePlayLevelWindowProjectile.speed = speed;
		return sallyStagePlayLevelWindowProjectile;
	}

	// Token: 0x06002CD6 RID: 11478 RVA: 0x001A6AD9 File Offset: 0x001A4ED9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002CD7 RID: 11479 RVA: 0x001A6AF7 File Offset: 0x001A4EF7
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002CD8 RID: 11480 RVA: 0x001A6B18 File Offset: 0x001A4F18
	private IEnumerator move_cr()
	{
		this.move = true;
		Vector3 dir = MathUtils.AngleToDirection(this.rotation);
		while (this.move)
		{
			base.transform.position += dir * this.speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002CD9 RID: 11481 RVA: 0x001A6B34 File Offset: 0x001A4F34
	protected override void Start()
	{
		base.Start();
		if (this.child != null)
		{
			this.child.transform.SetEulerAngles(null, null, new float?(0f));
			this.child.OnPlayerCollision += this.OnCollisionPlayer;
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.on_ground_hit_cr());
	}

	// Token: 0x06002CDA RID: 11482 RVA: 0x001A6BB6 File Offset: 0x001A4FB6
	private void OnPhase3()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002CDB RID: 11483 RVA: 0x001A6BC3 File Offset: 0x001A4FC3
	protected override void Die()
	{
		base.Die();
	}

	// Token: 0x06002CDC RID: 11484 RVA: 0x001A6BCC File Offset: 0x001A4FCC
	private IEnumerator on_ground_hit_cr()
	{
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			yield return null;
		}
		this.move = false;
		if (this.isBaby)
		{
			base.animator.SetTrigger("OnSmash");
			AudioManager.Play("sally_bottle_smash");
			this.emitAudioFromObject.Add("sally_bottle_smash");
		}
		else
		{
			base.animator.Play("Death");
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002CDD RID: 11485 RVA: 0x001A6BE7 File Offset: 0x001A4FE7
	protected override void OnDestroy()
	{
		base.OnDestroy();
	}

	// Token: 0x0400354E RID: 13646
	[SerializeField]
	private bool isBaby;

	// Token: 0x0400354F RID: 13647
	[SerializeField]
	private CollisionChild child;

	// Token: 0x04003550 RID: 13648
	private float speed;

	// Token: 0x04003551 RID: 13649
	private float rotation;

	// Token: 0x04003552 RID: 13650
	private bool move;
}
