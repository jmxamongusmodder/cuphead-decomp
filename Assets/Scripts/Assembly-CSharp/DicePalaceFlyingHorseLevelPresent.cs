using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C5 RID: 1477
public class DicePalaceFlyingHorseLevelPresent : AbstractProjectile
{
	// Token: 0x06001CD5 RID: 7381 RVA: 0x001083AF File Offset: 0x001067AF
	public void Init(Vector3 startPos, Vector3 targetPos, LevelProperties.DicePalaceFlyingHorse.GiftBombs properties)
	{
		base.transform.position = startPos;
		this.targetPos = targetPos;
		this.properties = properties;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001CD6 RID: 7382 RVA: 0x001083D8 File Offset: 0x001067D8
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001CD7 RID: 7383 RVA: 0x00108401 File Offset: 0x00106801
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CD8 RID: 7384 RVA: 0x00108420 File Offset: 0x00106820
	private IEnumerator move_cr()
	{
		while (base.transform.position != this.targetPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPos, this.properties.initialSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.explosionTime);
		string[] spreadCountPattern = this.properties.spreadCount.Split(new char[]
		{
			','
		});
		float angle = 0f;
		int parryIndex = UnityEngine.Random.Range(0, spreadCountPattern.Length);
		for (int i = 0; i < spreadCountPattern.Length; i++)
		{
			Parser.FloatTryParse(spreadCountPattern[i], out angle);
			this.SpawnBullet(angle, parryIndex == i);
		}
		yield return null;
		this.Die();
		yield break;
	}

	// Token: 0x06001CD9 RID: 7385 RVA: 0x0010843C File Offset: 0x0010683C
	private void SpawnBullet(float angle, bool parryable)
	{
		AudioManager.Play("projectile_explo");
		this.emitAudioFromObject.Add("projectile_explo");
		BasicProjectile basicProjectile = this.bullet.Create(base.transform.position, angle, this.properties.explosionSpeed);
		basicProjectile.SetParryable(parryable);
	}

	// Token: 0x06001CDA RID: 7386 RVA: 0x00108492 File Offset: 0x00106892
	protected override void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x040025C1 RID: 9665
	[SerializeField]
	private BasicProjectile bullet;

	// Token: 0x040025C2 RID: 9666
	private LevelProperties.DicePalaceFlyingHorse.GiftBombs properties;

	// Token: 0x040025C3 RID: 9667
	private Vector3 targetPos;
}
