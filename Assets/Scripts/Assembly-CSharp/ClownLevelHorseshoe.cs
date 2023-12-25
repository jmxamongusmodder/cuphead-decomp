using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000569 RID: 1385
public class ClownLevelHorseshoe : AbstractProjectile
{
	// Token: 0x06001A27 RID: 6695 RVA: 0x000EEED0 File Offset: 0x000ED2D0
	public void Init(Vector2 pos, float velocityX, float velocityY, bool onRight, float durationBeforeDrop, LevelProperties.Clown.Horse properties, ClownLevelClownHorse.HorseType horseType)
	{
		base.transform.position = pos;
		this.velocityX = velocityX;
		this.velocityY = velocityY;
		this.properties = properties;
		this.onRight = onRight;
		this.durationBeforeDrop = durationBeforeDrop;
		if (horseType != ClownLevelClownHorse.HorseType.Wave)
		{
			if (horseType == ClownLevelClownHorse.HorseType.Drop)
			{
				base.StartCoroutine(this.move_to_drop_point_cr());
				this.selectedSparkle = this.yellowSparkle;
				base.animator.SetInteger("type", 0);
			}
		}
		else
		{
			base.StartCoroutine(this.wave_cr());
			if (base.CanParry)
			{
				base.animator.SetInteger("type", 2);
				this.selectedSparkle = this.pinkSparkle;
			}
			else
			{
				base.animator.SetInteger("type", 1);
				this.selectedSparkle = this.greenSparkle;
			}
		}
		base.StartCoroutine(this.spawn_sparkle_cr());
	}

	// Token: 0x06001A28 RID: 6696 RVA: 0x000EEFC5 File Offset: 0x000ED3C5
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001A29 RID: 6697 RVA: 0x000EEFE3 File Offset: 0x000ED3E3
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001A2A RID: 6698 RVA: 0x000EF004 File Offset: 0x000ED404
	private IEnumerator wave_cr()
	{
		float angle = 0f;
		float speed = 0f;
		float loopSize = 0f;
		Vector3 moveX = base.transform.position;
		float edge = (!this.onRight) ? 690f : -690f;
		speed = ((!this.onRight) ? this.velocityX : (-this.velocityX));
		while ((!this.onRight) ? (base.transform.position.x < edge) : (base.transform.position.x > edge))
		{
			if (this.velocityY < 0f)
			{
				loopSize = -this.properties.WaveBulletAmount;
			}
			else
			{
				loopSize = this.properties.WaveBulletAmount;
			}
			angle += this.velocityY * CupheadTime.Delta;
			Vector3 moveY = new Vector3(0f, Mathf.Sin(angle + this.properties.WaveBulletAmount) * CupheadTime.Delta * 60f * loopSize / 2f);
			moveX = base.transform.right * speed * CupheadTime.Delta;
			base.transform.position += moveX + moveY;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001A2B RID: 6699 RVA: 0x000EF020 File Offset: 0x000ED420
	private IEnumerator move_to_drop_point_cr()
	{
		Vector3 pos = base.transform.position;
		if (this.onRight)
		{
			float leavePos = -740f;
			while (base.transform.position.x > leavePos)
			{
				base.transform.AddPosition(-this.velocityX * CupheadTime.Delta, 0f, 0f);
				yield return null;
			}
			pos.x = -740f;
		}
		else
		{
			float leavePos = 740f;
			while (base.transform.position.x < leavePos)
			{
				base.transform.AddPosition(this.velocityX * CupheadTime.Delta, 0f, 0f);
				yield return null;
			}
			pos.x = 740f;
		}
		pos.y = 260f;
		base.transform.position = pos;
		float dropPos = this.onRight ? (640f - this.velocityY) : (-640f + this.velocityY);
		base.animator.SetTrigger("onTop");
		yield return CupheadTime.WaitForSeconds(this, this.properties.DropBulletDelay);
		while (base.transform.position.x != dropPos)
		{
			pos.x = Mathf.MoveTowards(base.transform.position.x, dropPos, this.velocityX * CupheadTime.Delta);
			base.transform.position = pos;
			yield return null;
		}
		this.isSparkling = false;
		yield return CupheadTime.WaitForSeconds(this, this.durationBeforeDrop);
		this.isSparkling = true;
		base.animator.SetTrigger("down");
		AudioManager.Play("clown_horseshoe_drop");
		this.emitAudioFromObject.Add("clown_horseshoe_drop");
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			pos.y -= this.properties.DropBulletSpeedDown * CupheadTime.Delta;
			base.transform.position = pos;
			yield return null;
		}
		AudioManager.Play("clown_horseshoe_land");
		this.emitAudioFromObject.Add("clown_horseshoe_land");
		base.animator.SetTrigger("dead");
		this.deathPoof.Create(base.transform.position);
		yield return null;
		yield break;
	}

	// Token: 0x06001A2C RID: 6700 RVA: 0x000EF03C File Offset: 0x000ED43C
	public IEnumerator drop_cr()
	{
		Vector3 pos = base.transform.position;
		yield return null;
		yield break;
	}

	// Token: 0x06001A2D RID: 6701 RVA: 0x000EF058 File Offset: 0x000ED458
	private IEnumerator simple_cr()
	{
		float speed = 0f;
		float edge = (float)((!this.onRight) ? 640 : -640);
		speed = ((!this.onRight) ? this.velocityX : (-this.velocityX));
		while (base.transform.position.x != edge)
		{
			base.transform.AddPosition(speed * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001A2E RID: 6702 RVA: 0x000EF074 File Offset: 0x000ED474
	private IEnumerator spawn_sparkle_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
			if (this.isSparkling)
			{
				this.selectedSparkle.Create(base.transform.position);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001A2F RID: 6703 RVA: 0x000EF08F File Offset: 0x000ED48F
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.transform.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x06001A30 RID: 6704 RVA: 0x000EF0AE File Offset: 0x000ED4AE
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.greenSparkle = null;
		this.yellowSparkle = null;
		this.pinkSparkle = null;
		this.deathPoof = null;
	}

	// Token: 0x04002345 RID: 9029
	[SerializeField]
	private Effect greenSparkle;

	// Token: 0x04002346 RID: 9030
	[SerializeField]
	private Effect pinkSparkle;

	// Token: 0x04002347 RID: 9031
	[SerializeField]
	private Effect yellowSparkle;

	// Token: 0x04002348 RID: 9032
	[SerializeField]
	private Effect deathPoof;

	// Token: 0x04002349 RID: 9033
	private Effect selectedSparkle;

	// Token: 0x0400234A RID: 9034
	private LevelProperties.Clown.Horse properties;

	// Token: 0x0400234B RID: 9035
	private float velocityX;

	// Token: 0x0400234C RID: 9036
	private float velocityY;

	// Token: 0x0400234D RID: 9037
	private bool onRight;

	// Token: 0x0400234E RID: 9038
	private float durationBeforeDrop;

	// Token: 0x0400234F RID: 9039
	private bool isSparkling = true;
}
