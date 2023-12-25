using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B3 RID: 2227
public class FunhousePlatformingLevelDuck : AbstractPlatformingLevelEnemy
{
	// Token: 0x060033E7 RID: 13287 RVA: 0x001E1AA9 File Offset: 0x001DFEA9
	protected override void OnStart()
	{
	}

	// Token: 0x060033E8 RID: 13288 RVA: 0x001E1AAC File Offset: 0x001DFEAC
	protected override void Start()
	{
		base.Start();
		if (this.child != null)
		{
			this.child.OnAnyCollision += this.OnCollision;
			this.child.OnPlayerCollision += this.OnCollisionPlayer;
		}
		if (this.parryable)
		{
			this._canParry = true;
		}
		if (!this.isBigDuck)
		{
			base.StartCoroutine(this.idle_sound_cr());
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060033E9 RID: 13289 RVA: 0x001E1B37 File Offset: 0x001DFF37
	protected override void OnCollision(GameObject hit, CollisionPhase phase)
	{
		base.OnCollision(hit, phase);
		if (hit.GetComponent<FunhousePlatformingLevelCar>())
		{
			this.Die();
		}
	}

	// Token: 0x060033EA RID: 13290 RVA: 0x001E1B58 File Offset: 0x001DFF58
	private IEnumerator idle_sound_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(5f, 15f));
			AudioManager.Play("funhouse_small_duck_idle_sweet");
			this.emitAudioFromObject.Add("funhouse_small_duck_idle_sweet");
			yield return null;
		}
		yield break;
	}

	// Token: 0x060033EB RID: 13291 RVA: 0x001E1B74 File Offset: 0x001DFF74
	private IEnumerator move_cr()
	{
		if (this.isBigDuck)
		{
			AudioManager.PlayLoop("funhouse_big_duck_idle");
			this.emitAudioFromObject.Add("funhouse_big_duck_idle");
		}
		else if (this.smallFirst)
		{
			AudioManager.PlayLoop("funhouse_small_duck_idle_loop");
			this.emitAudioFromObject.Add("funhouse_small_duck_idle_loop");
		}
		float size = base.GetComponent<Collider2D>().bounds.size.x;
		while (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - size)
		{
			base.transform.position -= base.transform.right * base.Properties.MoveSpeed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		this.DoneAnimation();
		yield break;
	}

	// Token: 0x060033EC RID: 13292 RVA: 0x001E1B90 File Offset: 0x001DFF90
	protected override void Die()
	{
		this.StopAllCoroutines();
		if (this.smallLast)
		{
			AudioManager.Stop("funhouse_small_duck_idle_loop");
		}
		if (this.isBigDuck)
		{
			AudioManager.Stop("funhouse_big_duck_idle");
			AudioManager.Play("funhouse_big_duck_death");
			this.emitAudioFromObject.Add("funhouse_big_duck_death");
			base.animator.SetTrigger("OnDeath");
		}
		else
		{
			AudioManager.Play("funhouse_small_duck_death");
			AudioManager.Play("funhouse_small_duck_death");
			base.Die();
		}
	}

	// Token: 0x060033ED RID: 13293 RVA: 0x001E1C16 File Offset: 0x001E0016
	private void DoneAnimation()
	{
		if (this.isBigDuck)
		{
			AudioManager.Stop("funhouse_big_duck_idle");
		}
		if (this.smallLast)
		{
			AudioManager.Stop("funhouse_small_duck_idle_loop");
		}
		base.Die();
	}

	// Token: 0x04003C2C RID: 15404
	[SerializeField]
	private bool isBigDuck;

	// Token: 0x04003C2D RID: 15405
	[SerializeField]
	private bool parryable;

	// Token: 0x04003C2E RID: 15406
	[SerializeField]
	private CollisionChild child;

	// Token: 0x04003C2F RID: 15407
	public bool smallFirst;

	// Token: 0x04003C30 RID: 15408
	public bool smallLast;
}
