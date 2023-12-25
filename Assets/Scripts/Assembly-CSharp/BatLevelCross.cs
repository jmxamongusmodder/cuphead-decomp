using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000505 RID: 1285
public class BatLevelCross : AbstractCollidableObject
{
	// Token: 0x060016BC RID: 5820 RVA: 0x000CC9FC File Offset: 0x000CADFC
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060016BD RID: 5821 RVA: 0x000CCA0C File Offset: 0x000CAE0C
	public void Init(Vector2 pos, LevelProperties.Bat.CrossToss properties, int maxCount, AbstractPlayerController player)
	{
		base.transform.position = pos;
		this.startPos = pos;
		this.properties = properties;
		this.maxCount = maxCount;
		this.player = player;
		this.FindPlayer();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016BE RID: 5822 RVA: 0x000CCA5F File Offset: 0x000CAE5F
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016BF RID: 5823 RVA: 0x000CCA77 File Offset: 0x000CAE77
	protected override void OnCollisionCeiling(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.goBack = true;
		}
	}

	// Token: 0x060016C0 RID: 5824 RVA: 0x000CCA8E File Offset: 0x000CAE8E
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.goBack = true;
		}
	}

	// Token: 0x060016C1 RID: 5825 RVA: 0x000CCAA5 File Offset: 0x000CAEA5
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionCeiling(hit, phase);
		if (phase == CollisionPhase.Enter)
		{
			this.goBack = true;
		}
	}

	// Token: 0x060016C2 RID: 5826 RVA: 0x000CCABC File Offset: 0x000CAEBC
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060016C3 RID: 5827 RVA: 0x000CCAD4 File Offset: 0x000CAED4
	private IEnumerator move_cr()
	{
		int count = 0;
		bool startAgain = false;
		while (count < this.maxCount)
		{
			if (!this.goBack)
			{
				base.transform.position += this.velocity * this.properties.projectileSpeed * CupheadTime.FixedDelta;
			}
			else
			{
				startAgain = false;
				Vector2 v = base.transform.position;
				v = Vector3.MoveTowards(base.transform.position, this.startPos, this.properties.projectileSpeed * CupheadTime.Delta);
				base.transform.position = v;
				if (base.transform.position == this.startPos && !startAgain)
				{
					count++;
					this.goBack = false;
					this.FindPlayer();
					startAgain = true;
				}
			}
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060016C4 RID: 5828 RVA: 0x000CCAF0 File Offset: 0x000CAEF0
	private void FindPlayer()
	{
		float x = this.player.transform.position.x - base.transform.position.x;
		float y = this.player.transform.position.y - base.transform.position.y;
		float value = Mathf.Atan2(y, x) * 57.29578f;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(value));
		this.velocity = base.transform.right;
	}

	// Token: 0x060016C5 RID: 5829 RVA: 0x000CCB9F File Offset: 0x000CAF9F
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400200D RID: 8205
	private LevelProperties.Bat.CrossToss properties;

	// Token: 0x0400200E RID: 8206
	private AbstractPlayerController player;

	// Token: 0x0400200F RID: 8207
	private DamageDealer damageDealer;

	// Token: 0x04002010 RID: 8208
	private Vector3 velocity;

	// Token: 0x04002011 RID: 8209
	private Vector3 startPos;

	// Token: 0x04002012 RID: 8210
	private int maxCount;

	// Token: 0x04002013 RID: 8211
	private bool goBack;
}
