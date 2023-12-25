using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200066E RID: 1646
public class FlyingGenieLevelHelixProjectile : AbstractProjectile
{
	// Token: 0x0600229D RID: 8861 RVA: 0x00145378 File Offset: 0x00143778
	public FlyingGenieLevelHelixProjectile Create(Vector3 pos, LevelProperties.FlyingGenie.Coffin properties, bool topOne)
	{
		FlyingGenieLevelHelixProjectile flyingGenieLevelHelixProjectile = base.Create() as FlyingGenieLevelHelixProjectile;
		flyingGenieLevelHelixProjectile.properties = properties;
		flyingGenieLevelHelixProjectile.transform.position = pos;
		flyingGenieLevelHelixProjectile.topOne = topOne;
		return flyingGenieLevelHelixProjectile;
	}

	// Token: 0x0600229E RID: 8862 RVA: 0x001453AC File Offset: 0x001437AC
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600229F RID: 8863 RVA: 0x001453D5 File Offset: 0x001437D5
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060022A0 RID: 8864 RVA: 0x001453F3 File Offset: 0x001437F3
	protected override void Start()
	{
		base.Start();
		base.animator.SetBool("OnTop", this.topOne);
		base.StartCoroutine(this.moveY_cr());
	}

	// Token: 0x060022A1 RID: 8865 RVA: 0x00145420 File Offset: 0x00143820
	private IEnumerator moveY_cr()
	{
		float angle = 0f;
		float xSpeed = this.properties.heartShotXSpeed;
		float ySpeed = this.properties.heartShotYSpeed;
		Vector3 moveX = base.transform.position;
		while (base.transform.position.x != -640f)
		{
			float loopSize;
			if (this.topOne)
			{
				loopSize = this.properties.heartLoopYSize;
				ySpeed = this.properties.heartShotYSpeed;
			}
			else
			{
				loopSize = -this.properties.heartLoopYSize;
				ySpeed = -this.properties.heartShotYSpeed;
			}
			angle += ySpeed * CupheadTime.Delta;
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle + this.properties.heartLoopYSize) * CupheadTime.Delta * 60f * loopSize / 2f);
			moveX = -base.transform.right * xSpeed * CupheadTime.Delta;
			base.transform.position += moveX + moveY;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x04002B48 RID: 11080
	private LevelProperties.FlyingGenie.Coffin properties;

	// Token: 0x04002B49 RID: 11081
	private bool topOne;
}
