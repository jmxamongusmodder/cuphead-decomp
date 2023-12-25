using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005B3 RID: 1459
public class DicePalaceDominoLevelBouncyBall : AbstractProjectile
{
	// Token: 0x06001C4A RID: 7242 RVA: 0x0010334E File Offset: 0x0010174E
	public void InitBouncyBall(float speed, Vector3 direction)
	{
		this.deltaPosition = direction * speed;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.checkCollisions_cr());
	}

	// Token: 0x06001C4B RID: 7243 RVA: 0x00103377 File Offset: 0x00101777
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		if (parryable)
		{
			base.animator.SetInteger("Variation", 3);
		}
		else
		{
			base.animator.SetInteger("Variation", UnityEngine.Random.Range(1, 3));
		}
	}

	// Token: 0x06001C4C RID: 7244 RVA: 0x001033B4 File Offset: 0x001017B4
	private IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += this.deltaPosition * CupheadTime.Delta;
			for (int i = 0; i < this.toRotate.Length; i++)
			{
				this.toRotate[i].Rotate(Vector3.forward, 180f * CupheadTime.Delta);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C4D RID: 7245 RVA: 0x001033D0 File Offset: 0x001017D0
	private IEnumerator checkCollisions_cr()
	{
		for (;;)
		{
			if (base.transform.position.y > (float)Level.Current.Ceiling)
			{
				this.deltaPosition.y = this.deltaPosition.y * -1f;
				this.BounceSFX();
				base.animator.SetTrigger("Bounce");
				this.hitEffectPrefab.Create(base.transform.position, new Vector3(1f, -1f, 1f));
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			if (base.transform.position.y < (float)Level.Current.Ground)
			{
				this.deltaPosition.y = this.deltaPosition.y * -1f;
				this.BounceSFX();
				base.animator.SetTrigger("Bounce");
				this.hitEffectPrefab.Create(base.transform.position);
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			if (base.transform.position.x > (float)Level.Current.Right)
			{
				this.deltaPosition.x = this.deltaPosition.x * -1f;
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			if (base.transform.position.x < (float)Level.Current.Left)
			{
				this.deltaPosition.x = this.deltaPosition.x * -1f;
				yield return CupheadTime.WaitForSeconds(this, 1f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001C4E RID: 7246 RVA: 0x001033EB File Offset: 0x001017EB
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionWalls(hit, phase);
		if (hit.GetComponent<BasicDamageDealingObject>())
		{
			this.Die();
		}
	}

	// Token: 0x06001C4F RID: 7247 RVA: 0x0010340B File Offset: 0x0010180B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001C50 RID: 7248 RVA: 0x00103429 File Offset: 0x00101829
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.hitEffectPrefab = null;
		this.explosion = null;
	}

	// Token: 0x06001C51 RID: 7249 RVA: 0x00103445 File Offset: 0x00101845
	protected override void Die()
	{
		this.BounceSFX();
		this.explosion.Create(base.transform.position);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001C52 RID: 7250 RVA: 0x0010346F File Offset: 0x0010186F
	private void BounceSFX()
	{
		AudioManager.Play("dice_projectile_bounce");
		this.emitAudioFromObject.Add("dice_projectile_bounce");
	}

	// Token: 0x0400254A RID: 9546
	private const float RotationFactor = 180f;

	// Token: 0x0400254B RID: 9547
	[SerializeField]
	private Effect hitEffectPrefab;

	// Token: 0x0400254C RID: 9548
	[SerializeField]
	private Effect explosion;

	// Token: 0x0400254D RID: 9549
	[SerializeField]
	private Transform[] toRotate;

	// Token: 0x0400254E RID: 9550
	private Vector3 deltaPosition;

	// Token: 0x020005B4 RID: 1460
	public enum Colour
	{
		// Token: 0x04002550 RID: 9552
		blue,
		// Token: 0x04002551 RID: 9553
		green,
		// Token: 0x04002552 RID: 9554
		red,
		// Token: 0x04002553 RID: 9555
		yellow,
		// Token: 0x04002554 RID: 9556
		none
	}
}
