using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E5 RID: 2277
public class MountainPlatformingLevelMiner : PlatformingLevelGroundMovementEnemy
{
	// Token: 0x06003557 RID: 13655 RVA: 0x001F1322 File Offset: 0x001EF722
	protected override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
		base.StartCoroutine(this.descend_cr());
	}

	// Token: 0x06003558 RID: 13656 RVA: 0x001F1348 File Offset: 0x001EF748
	private IEnumerator descend_cr()
	{
		this.floating = false;
		this.landing = true;
		float t = 0f;
		float time = base.Properties.minerDescendTime;
		Vector3 endPos = new Vector3(base.transform.position.x, base.transform.position.y - 400f);
		Vector3 startPos = base.transform.position;
		AudioManager.Play("castle_miner_spawn");
		this.emitAudioFromObject.Add("castle_miner_spawn");
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, endPos, val);
			yield return null;
		}
		this.rope.transform.parent = null;
		yield return CupheadTime.WaitForSeconds(this, 0.4f);
		base.animator.SetTrigger("Continue");
		this.rope.animator.SetTrigger("Jump");
		this.floating = true;
		this.landing = false;
		yield return base.animator.WaitForAnimationToEnd(this, "Jump_Start", false, true);
		this.rope.animator.SetTrigger("Pull");
		while (!base.Grounded)
		{
			yield return null;
		}
		base.animator.SetTrigger("Land");
		this.rope.transform.parent = null;
		this.rope.PullRope(base.Properties.minerRopeAscendTime, startPos);
		this.leftRope = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Jump_End", false, true);
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.look_direction_cr());
		base.StartCoroutine(this.face_direction_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06003559 RID: 13657 RVA: 0x001F1364 File Offset: 0x001EF764
	private IEnumerator look_direction_cr()
	{
		float maxDist = 30f;
		while (this.player == null)
		{
			yield return null;
		}
		for (;;)
		{
			float dist = this.player.transform.position.y - this.lookAt.transform.position.y;
			if (dist < maxDist && dist > -maxDist)
			{
				this.straight.enabled = true;
				this.down.enabled = false;
				this.up.enabled = false;
			}
			else if (dist > maxDist)
			{
				this.straight.enabled = false;
				this.down.enabled = false;
				this.up.enabled = true;
			}
			else
			{
				this.straight.enabled = false;
				this.down.enabled = true;
				this.up.enabled = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600355A RID: 13658 RVA: 0x001F1380 File Offset: 0x001EF780
	private IEnumerator face_direction_cr()
	{
		while (this.player == null)
		{
			yield return null;
		}
		for (;;)
		{
			if (!this.inAttack && ((this.player.transform.position.x > base.transform.position.x && base.direction == PlatformingLevelGroundMovementEnemy.Direction.Left) || (this.player.transform.position.x < base.transform.position.x && base.direction == PlatformingLevelGroundMovementEnemy.Direction.Right)) && !base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
			{
				this.Turn();
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600355B RID: 13659 RVA: 0x001F139C File Offset: 0x001EF79C
	private IEnumerator shoot_cr()
	{
		for (;;)
		{
			while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMax + this.offset || base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMin - this.offset)
			{
				yield return null;
			}
			this.player = PlayerManager.GetNext();
			yield return CupheadTime.WaitForSeconds(this, base.Properties.minerShotDelay.RandomFloat());
			this.inAttack = true;
			base.animator.SetTrigger("Shoot");
			while (this.currentPickaxe != null || !this.isShooting)
			{
				yield return null;
			}
			base.animator.Play("Catch");
			this.isShooting = false;
			this.inAttack = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600355C RID: 13660 RVA: 0x001F13B8 File Offset: 0x001EF7B8
	private void ShootPickaxe()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 v = this.player.transform.position - this.root.transform.position;
		Vector3 targetPos = this.root.transform.position + v.normalized * base.Properties.minerDistance;
		this.currentPickaxe = this.pickaxe.Create(this.root.transform.position, MathUtils.DirectionToAngle(v), base.Properties.minerShootSpeed, this, targetPos, this.catchRoot.transform.position);
		this.isShooting = true;
	}

	// Token: 0x0600355D RID: 13661 RVA: 0x001F1499 File Offset: 0x001EF899
	private void ShootSFX()
	{
		AudioManager.Play("castle_miner_throw");
		this.emitAudioFromObject.Add("castle_miner_throw");
	}

	// Token: 0x0600355E RID: 13662 RVA: 0x001F14B5 File Offset: 0x001EF8B5
	private void CatchSFX()
	{
		AudioManager.Play("castle_miner_catch_pick");
		this.emitAudioFromObject.Add("castle_miner_catch_pick");
	}

	// Token: 0x0600355F RID: 13663 RVA: 0x001F14D4 File Offset: 0x001EF8D4
	private void Offset()
	{
		if (base.direction == PlatformingLevelGroundMovementEnemy.Direction.Left)
		{
			base.transform.AddPosition(47f, 0f, 0f);
		}
		else
		{
			base.transform.AddPosition(-47f, 0f, 0f);
		}
	}

	// Token: 0x06003560 RID: 13664 RVA: 0x001F1528 File Offset: 0x001EF928
	protected override void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		if (!this.leftRope)
		{
			this.rope.animator.SetTrigger("Jump");
			this.rope.animator.SetTrigger("Pull");
			this.rope.transform.parent = null;
			this.rope.PullRope(base.Properties.minerRopeAscendTime, this.startPos);
		}
		AudioManager.Play("castle_generic_death");
		this.emitAudioFromObject.Add("castle_generic_death");
		base.Die();
	}

	// Token: 0x06003561 RID: 13665 RVA: 0x001F15CE File Offset: 0x001EF9CE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.pickaxe = null;
	}

	// Token: 0x04003D7A RID: 15738
	[SerializeField]
	private MountainPlatformingLevelPickaxeProjectile pickaxe;

	// Token: 0x04003D7B RID: 15739
	[SerializeField]
	private SpriteRenderer straight;

	// Token: 0x04003D7C RID: 15740
	[SerializeField]
	private SpriteRenderer up;

	// Token: 0x04003D7D RID: 15741
	[SerializeField]
	private SpriteRenderer down;

	// Token: 0x04003D7E RID: 15742
	[SerializeField]
	private Transform lookAt;

	// Token: 0x04003D7F RID: 15743
	[SerializeField]
	private MountainPlatformingLevelMinerRope rope;

	// Token: 0x04003D80 RID: 15744
	[SerializeField]
	private Transform root;

	// Token: 0x04003D81 RID: 15745
	[SerializeField]
	private Transform catchRoot;

	// Token: 0x04003D82 RID: 15746
	private Vector3 startPos;

	// Token: 0x04003D83 RID: 15747
	private AbstractPlayerController player;

	// Token: 0x04003D84 RID: 15748
	private MountainPlatformingLevelPickaxeProjectile currentPickaxe;

	// Token: 0x04003D85 RID: 15749
	private bool isShooting;

	// Token: 0x04003D86 RID: 15750
	private bool inAttack;

	// Token: 0x04003D87 RID: 15751
	private bool leftRope;

	// Token: 0x04003D88 RID: 15752
	private float offset = 50f;
}
