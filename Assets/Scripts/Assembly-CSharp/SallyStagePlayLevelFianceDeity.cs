using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007AC RID: 1964
public class SallyStagePlayLevelFianceDeity : LevelProperties.SallyStagePlay.Entity
{
	// Token: 0x06002C24 RID: 11300 RVA: 0x0019F5C0 File Offset: 0x0019D9C0
	private void Start()
	{
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002C25 RID: 11301 RVA: 0x0019F5F1 File Offset: 0x0019D9F1
	public void Attack()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06002C26 RID: 11302 RVA: 0x0019F600 File Offset: 0x0019DA00
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (SallyStagePlayLevelAngel.extraHP > 0f)
		{
			SallyStagePlayLevelAngel.extraHP -= info.damage;
		}
		else
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x06002C27 RID: 11303 RVA: 0x0019F638 File Offset: 0x0019DA38
	private IEnumerator attack_cr()
	{
		LevelProperties.SallyStagePlay.Husband p = base.properties.CurrentState.husband;
		while (!this.isDead)
		{
			yield return CupheadTime.WaitForSeconds(this, p.shotDelayRange.RandomFloat());
			base.GetComponent<Animator>().SetBool("OnAttack", true);
			yield return base.GetComponent<Animator>().WaitForAnimationToEnd(this, "Puppet_Attack_Start", false, true);
			this.cherubProjectile.Create(this.husbandRoot.position, 0f, p.shotSpeed);
			base.GetComponent<Animator>().SetBool("OnAttack", false);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002C28 RID: 11304 RVA: 0x0019F653 File Offset: 0x0019DA53
	public void Dead()
	{
		this.isDead = true;
		this.StopAllCoroutines();
		this.damageReceiver.enabled = false;
		base.GetComponent<Animator>().SetTrigger("OnDeath");
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002C29 RID: 11305 RVA: 0x0019F68C File Offset: 0x0019DA8C
	public IEnumerator move_cr()
	{
		float t = 0f;
		float time = 3f;
		Vector3 endPos = new Vector3(-1140f, base.transform.position.y);
		Vector2 start = base.transform.position;
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		yield return CupheadTime.WaitForSeconds(this, 0.8f);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		base.GetComponent<Collider2D>().enabled = false;
		yield break;
	}

	// Token: 0x06002C2A RID: 11306 RVA: 0x0019F6A7 File Offset: 0x0019DAA7
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.cherubProjectile = null;
	}

	// Token: 0x040034D6 RID: 13526
	[SerializeField]
	private SallyStagePlayLevelCherubProjectile cherubProjectile;

	// Token: 0x040034D7 RID: 13527
	[SerializeField]
	private Transform husbandRoot;

	// Token: 0x040034D8 RID: 13528
	private bool isDead;

	// Token: 0x040034D9 RID: 13529
	private float health;

	// Token: 0x040034DA RID: 13530
	private DamageReceiver damageReceiver;
}
