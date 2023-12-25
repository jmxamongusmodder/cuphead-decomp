using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005B2 RID: 1458
public class DicePalaceDominoLevelBoomerang : AbstractProjectile
{
	// Token: 0x06001C3D RID: 7229 RVA: 0x00102E1C File Offset: 0x0010121C
	public DicePalaceDominoLevelBoomerang Create(Vector2 pos, float speed, float hp)
	{
		DicePalaceDominoLevelBoomerang dicePalaceDominoLevelBoomerang = base.Create(pos) as DicePalaceDominoLevelBoomerang;
		dicePalaceDominoLevelBoomerang.speed = speed;
		dicePalaceDominoLevelBoomerang.HP = hp;
		return dicePalaceDominoLevelBoomerang;
	}

	// Token: 0x06001C3E RID: 7230 RVA: 0x00102E45 File Offset: 0x00101245
	protected override void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.move_cr());
		base.Start();
	}

	// Token: 0x06001C3F RID: 7231 RVA: 0x00102E7D File Offset: 0x0010127D
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.HP -= info.damage;
		if (this.HP <= 0f && !this.isDead)
		{
			this.isDead = true;
			this.Killed();
		}
	}

	// Token: 0x06001C40 RID: 7232 RVA: 0x00102EBA File Offset: 0x001012BA
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001C41 RID: 7233 RVA: 0x00102ED8 File Offset: 0x001012D8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001C42 RID: 7234 RVA: 0x00102EF8 File Offset: 0x001012F8
	private IEnumerator move_cr()
	{
		float dropPoint = (float)Level.Current.Ground + base.GetComponent<Collider2D>().bounds.size.y;
		float goToPos = -440f;
		while (base.transform.position.x > goToPos)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnDrop");
		yield return base.animator.WaitForAnimationToStart(this, "Fly_Drop_Start", false);
		AudioManager.Play("dice_palace_domino_bird_dive");
		this.emitAudioFromObject.Add("dice_palace_domino_bird_dive");
		while (base.transform.position.y > dropPoint)
		{
			base.transform.position += Vector3.down * this.speed * CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetTrigger("OnStop");
		yield return null;
		yield break;
	}

	// Token: 0x06001C43 RID: 7235 RVA: 0x00102F13 File Offset: 0x00101313
	private void ChangeDirection()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.fly_right_cr());
	}

	// Token: 0x06001C44 RID: 7236 RVA: 0x00102F28 File Offset: 0x00101328
	private IEnumerator fly_right_cr()
	{
		while (base.transform.position.x < 840f)
		{
			base.transform.position += Vector3.right * this.speed * CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001C45 RID: 7237 RVA: 0x00102F43 File Offset: 0x00101343
	private void WingFlapSFX()
	{
		AudioManager.Play("bird_bird_flap");
		this.emitAudioFromObject.Add("bird_bird_flap");
	}

	// Token: 0x06001C46 RID: 7238 RVA: 0x00102F60 File Offset: 0x00101360
	private void Killed()
	{
		this.StopAllCoroutines();
		base.GetComponent<SpriteRenderer>().enabled = false;
		this.Die();
		AudioManager.Play("dice_bird_die");
		this.emitAudioFromObject.Add("dice_bird_die");
		base.animator.SetTrigger("OnDeath");
	}

	// Token: 0x06001C47 RID: 7239 RVA: 0x00102FAF File Offset: 0x001013AF
	private void SpawnEffect()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.deathPoof.Create(base.transform.position);
	}

	// Token: 0x06001C48 RID: 7240 RVA: 0x00102FD4 File Offset: 0x001013D4
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.deathPoof = null;
	}

	// Token: 0x04002545 RID: 9541
	[SerializeField]
	private Effect deathPoof;

	// Token: 0x04002546 RID: 9542
	private DamageReceiver damageReceiver;

	// Token: 0x04002547 RID: 9543
	private float speed;

	// Token: 0x04002548 RID: 9544
	private float HP;

	// Token: 0x04002549 RID: 9545
	private bool isDead;
}
