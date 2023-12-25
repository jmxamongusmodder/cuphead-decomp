using System;
using UnityEngine;

// Token: 0x02000512 RID: 1298
public class BeeLevelGrunt : AbstractCollidableObject
{
	// Token: 0x0600171C RID: 5916 RVA: 0x000CFA78 File Offset: 0x000CDE78
	public BeeLevelGrunt Create(Vector2 pos, int xScale, int health, float speed)
	{
		BeeLevelGrunt beeLevelGrunt = UnityEngine.Object.Instantiate<BeeLevelGrunt>(this);
		beeLevelGrunt.speed = speed;
		beeLevelGrunt.health = (float)health;
		beeLevelGrunt.transform.SetScale(new float?((float)xScale), new float?(1f), new float?(1f));
		beeLevelGrunt.transform.position = pos;
		return beeLevelGrunt;
	}

	// Token: 0x0600171D RID: 5917 RVA: 0x000CFAD4 File Offset: 0x000CDED4
	protected override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 1f, true, false, false);
	}

	// Token: 0x0600171E RID: 5918 RVA: 0x000CFB0C File Offset: 0x000CDF0C
	private void Update()
	{
		if (this.dead)
		{
			return;
		}
		if (base.transform.position.x < -1280f || base.transform.position.x > 1280f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		base.transform.AddPosition(this.speed * CupheadTime.Delta * -base.transform.localScale.x, 0f, 0f);
	}

	// Token: 0x0600171F RID: 5919 RVA: 0x000CFBA6 File Offset: 0x000CDFA6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001720 RID: 5920 RVA: 0x000CFBC4 File Offset: 0x000CDFC4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06001721 RID: 5921 RVA: 0x000CFBF0 File Offset: 0x000CDFF0
	private void Die()
	{
		AudioManager.Play("level_bee_grunt_death");
		this.dead = true;
		this.briefcasePrefab.Create((int)base.transform.localScale.x, base.transform.position);
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Die");
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?((float)UnityEngine.Random.Range(0, 360)));
		base.transform.SetScale(new float?((float)MathUtils.PlusOrMinus()), new float?((float)MathUtils.PlusOrMinus()), new float?(1f));
	}

	// Token: 0x06001722 RID: 5922 RVA: 0x000CFCB5 File Offset: 0x000CE0B5
	private void OnDeathAnimComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001723 RID: 5923 RVA: 0x000CFCC2 File Offset: 0x000CE0C2
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.briefcasePrefab = null;
	}

	// Token: 0x04002062 RID: 8290
	[SerializeField]
	private BeeLevelGruntBriefcase briefcasePrefab;

	// Token: 0x04002063 RID: 8291
	private float health;

	// Token: 0x04002064 RID: 8292
	private float speed;

	// Token: 0x04002065 RID: 8293
	private DamageDealer damageDealer;

	// Token: 0x04002066 RID: 8294
	private bool dead;
}
