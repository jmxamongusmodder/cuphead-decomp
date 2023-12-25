using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004E4 RID: 1252
public class BaronessLevelJellybeans : AbstractProjectile
{
	// Token: 0x1700031D RID: 797
	// (get) Token: 0x060015A9 RID: 5545 RVA: 0x000C214C File Offset: 0x000C054C
	// (set) Token: 0x060015AA RID: 5546 RVA: 0x000C2154 File Offset: 0x000C0554
	public BaronessLevelJellybeans.State state { get; private set; }

	// Token: 0x060015AB RID: 5547 RVA: 0x000C2160 File Offset: 0x000C0560
	public BaronessLevelJellybeans Create(LevelProperties.Baroness.Jellybeans properties, Vector3 pos, float speed, float health)
	{
		BaronessLevelJellybeans baronessLevelJellybeans = base.Create() as BaronessLevelJellybeans;
		baronessLevelJellybeans.properties = properties;
		baronessLevelJellybeans.speed = speed;
		baronessLevelJellybeans.health = health;
		baronessLevelJellybeans.transform.position = pos;
		return baronessLevelJellybeans;
	}

	// Token: 0x060015AC RID: 5548 RVA: 0x000C219C File Offset: 0x000C059C
	protected override void Start()
	{
		base.Start();
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.state = BaronessLevelJellybeans.State.Run;
		AudioManager.Play("level_baroness_jellybean_spawn");
		this.emitAudioFromObject.Add("level_baroness_jellybean_spawn");
		base.StartCoroutine(this.fade_color_cr());
		base.StartCoroutine(this.beginning_offset_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060015AD RID: 5549 RVA: 0x000C2232 File Offset: 0x000C0632
	private void KillJelly()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.Die();
	}

	// Token: 0x060015AE RID: 5550 RVA: 0x000C2246 File Offset: 0x000C0646
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060015AF RID: 5551 RVA: 0x000C2264 File Offset: 0x000C0664
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060015B0 RID: 5552 RVA: 0x000C2284 File Offset: 0x000C0684
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && this.state != BaronessLevelJellybeans.State.Dead)
		{
			this.state = BaronessLevelJellybeans.State.Dead;
			base.GetComponent<Collider2D>().enabled = false;
			base.animator.Play((!Rand.Bool()) ? "Jellybean_Death_B" : "Jellybean_Death_A");
		}
	}

	// Token: 0x060015B1 RID: 5553 RVA: 0x000C22F6 File Offset: 0x000C06F6
	protected virtual float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x060015B2 RID: 5554 RVA: 0x000C2318 File Offset: 0x000C0718
	private IEnumerator fade_color_cr()
	{
		Color endColor = base.GetComponent<SpriteRenderer>().color;
		float fadeTime = 0.2f;
		float t = 0f;
		Color start = new Color(0f, 0f, 0f, 1f);
		while (t < fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = Color.Lerp(start, endColor, t / fadeTime);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = endColor;
		yield return null;
		yield break;
	}

	// Token: 0x060015B3 RID: 5555 RVA: 0x000C2333 File Offset: 0x000C0733
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.explosion = null;
	}

	// Token: 0x060015B4 RID: 5556 RVA: 0x000C2344 File Offset: 0x000C0744
	private IEnumerator beginning_offset_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		Vector3 pos = base.transform.position;
		Vector3 startPos = base.transform.position;
		this.velocity = this.properties.jumpSpeed;
		pos.y = base.transform.position.y;
		startPos.y = base.transform.position.y + 40f;
		this.originalPos = pos;
		base.transform.position = startPos;
		while (base.transform.position.y >= pos.y)
		{
			if (this.state == BaronessLevelJellybeans.State.Run)
			{
				base.transform.AddPosition(0f, -100f * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060015B5 RID: 5557 RVA: 0x000C2360 File Offset: 0x000C0760
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.state = BaronessLevelJellybeans.State.Run;
		float offset = 200f;
		while (base.transform.position.x > -640f - offset)
		{
			if (this.state != BaronessLevelJellybeans.State.Jump)
			{
				Vector3 pos = base.transform.position;
				pos.x += -this.speed * CupheadTime.FixedDelta * this.hitPauseCoefficient();
				base.transform.position = pos;
			}
			yield return wait;
		}
		this.Die();
		yield break;
	}

	// Token: 0x060015B6 RID: 5558 RVA: 0x000C237B File Offset: 0x000C077B
	private void StartJump()
	{
		this.state = BaronessLevelJellybeans.State.Jump;
		base.StartCoroutine(this.jump_cr());
	}

	// Token: 0x060015B7 RID: 5559 RVA: 0x000C2394 File Offset: 0x000C0794
	private IEnumerator jump_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.velocity = this.properties.jumpSpeed;
		float decrement = 1f;
		Vector3 pos = base.transform.position;
		bool jumping = true;
		bool landing = false;
		base.animator.Play("Jellybean_Jump_Antic");
		yield return base.animator.WaitForAnimationToEnd(this, "Jellybean_Jump_Antic", false, true);
		while (jumping)
		{
			base.transform.AddPosition(0f, this.velocity * CupheadTime.FixedDelta * this.hitPauseCoefficient(), 0f);
			if (base.transform.position.y >= this.properties.heightDefault + this.properties.jumpHeight.RandomFloat())
			{
				this.velocity -= decrement;
				if (!landing)
				{
					this.velocity = -this.velocity;
					base.animator.SetTrigger("Land");
					landing = true;
				}
			}
			if (base.transform.position.y <= this.originalPos.y)
			{
				if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Jellybean_Jump_Land"))
				{
					yield return base.animator.WaitForAnimationToEnd(this, "Jellybean_Jump_Land", false, true);
				}
				jumping = false;
			}
			yield return wait;
		}
		base.StartCoroutine(this.timer_cr());
		pos.y = this.originalPos.y;
		base.transform.position = pos;
		this.state = BaronessLevelJellybeans.State.Run;
		yield return null;
		yield break;
	}

	// Token: 0x060015B8 RID: 5560 RVA: 0x000C23B0 File Offset: 0x000C07B0
	private IEnumerator timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.afterJumpDuration);
		yield break;
	}

	// Token: 0x060015B9 RID: 5561 RVA: 0x000C23CB File Offset: 0x000C07CB
	private void DeathComplete()
	{
		this.explosion.Create(base.transform.position);
		AudioManager.Play("level_baroness_jellybean_death");
		this.emitAudioFromObject.Add("level_baroness_jellybean_death");
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060015BA RID: 5562 RVA: 0x000C2409 File Offset: 0x000C0809
	protected override void Die()
	{
		base.Die();
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
	}

	// Token: 0x04001F04 RID: 7940
	[SerializeField]
	private Effect explosion;

	// Token: 0x04001F06 RID: 7942
	private float health;

	// Token: 0x04001F07 RID: 7943
	private float speed;

	// Token: 0x04001F08 RID: 7944
	private float velocity;

	// Token: 0x04001F09 RID: 7945
	private Vector3 originalPos;

	// Token: 0x04001F0A RID: 7946
	private LevelProperties.Baroness.Jellybeans properties;

	// Token: 0x04001F0B RID: 7947
	private DamageReceiver damageReceiver;

	// Token: 0x020004E5 RID: 1253
	public enum State
	{
		// Token: 0x04001F0D RID: 7949
		Dead,
		// Token: 0x04001F0E RID: 7950
		Run,
		// Token: 0x04001F0F RID: 7951
		Jump
	}
}
