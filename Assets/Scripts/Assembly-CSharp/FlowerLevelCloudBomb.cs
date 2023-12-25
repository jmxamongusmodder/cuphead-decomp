using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000606 RID: 1542
public class FlowerLevelCloudBomb : AbstractCollidableObject
{
	// Token: 0x06001EB4 RID: 7860 RVA: 0x0011AAA9 File Offset: 0x00118EA9
	public void OnCloudBombStart(Vector3 target, float s, float delay)
	{
		this.playerPos = target;
		base.transform.LookAt2D(this.playerPos);
		this.speed = s;
		this.detonationDelay = delay;
		this.hasDetonated = false;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001EB5 RID: 7861 RVA: 0x0011AAE3 File Offset: 0x00118EE3
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001EB6 RID: 7862 RVA: 0x0011AAFC File Offset: 0x00118EFC
	private void FixedUpdate()
	{
		if (!this.hasDetonated)
		{
			if ((this.playerPos - base.transform.position).magnitude > (base.transform.right * (this.speed * CupheadTime.FixedDelta)).magnitude)
			{
				base.transform.position += base.transform.right * (this.speed * CupheadTime.FixedDelta);
			}
			else
			{
				this.hasDetonated = true;
				base.StartCoroutine(this.explode_cr());
				base.transform.position += this.playerPos - base.transform.position;
			}
		}
	}

	// Token: 0x06001EB7 RID: 7863 RVA: 0x0011ABD2 File Offset: 0x00118FD2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001EB8 RID: 7864 RVA: 0x0011ABFC File Offset: 0x00118FFC
	private IEnumerator explode_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.detonationDelay);
		base.animator.SetTrigger("Explode");
		BoxCollider2D collider = base.GetComponent<BoxCollider2D>();
		collider.size = base.GetComponent<SpriteRenderer>().bounds.size;
		yield break;
	}

	// Token: 0x06001EB9 RID: 7865 RVA: 0x0011AC17 File Offset: 0x00119017
	protected void Die()
	{
		AudioManager.Play("flower_minion_simple_deathpop_high");
		this.emitAudioFromObject.Add("flower_minion_simple_deathpop_high");
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400277D RID: 10109
	private bool hasDetonated;

	// Token: 0x0400277E RID: 10110
	private float speed;

	// Token: 0x0400277F RID: 10111
	private float detonationDelay;

	// Token: 0x04002780 RID: 10112
	private Vector3 playerPos;

	// Token: 0x04002781 RID: 10113
	private DamageDealer damageDealer;
}
