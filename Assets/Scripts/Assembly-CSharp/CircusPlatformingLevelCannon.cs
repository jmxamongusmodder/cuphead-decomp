using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200089F RID: 2207
public class CircusPlatformingLevelCannon : AbstractPausableComponent
{
	// Token: 0x0600335B RID: 13147 RVA: 0x001DDF9C File Offset: 0x001DC39C
	private void Start()
	{
		this.goingBackwards = Rand.Bool();
		this.shootIndex = UnityEngine.Random.Range(0, this.shootRoots.Length);
		this.pinkSplits = this.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = UnityEngine.Random.Range(0, this.pinkSplits.Length);
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		foreach (DamageReceiver damageReceiver in this.cannons)
		{
			damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x0600335C RID: 13148 RVA: 0x001DE050 File Offset: 0x001DC450
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f && !this.isDead)
		{
			this.isDead = true;
			this.StopAllCoroutines();
			base.StartCoroutine(this.slide_off_cr());
		}
	}

	// Token: 0x0600335D RID: 13149 RVA: 0x001DE0A8 File Offset: 0x001DC4A8
	private IEnumerator shoot_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			while (PlayerManager.GetNext().transform.position.x < this.startTrigger.transform.position.x)
			{
				yield return null;
			}
			base.animator.SetInteger("Cannon", this.shootIndex + 1);
			yield return CupheadTime.WaitForSeconds(this, this.projectileDelay);
			if (PlayerManager.GetNext().transform.position.x > this.endTrigger.position.x)
			{
				while (PlayerManager.GetNext().transform.position.x > this.endTrigger.position.x)
				{
					yield return null;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600335E RID: 13150 RVA: 0x001DE0C4 File Offset: 0x001DC4C4
	private void Shoot()
	{
		CircusPlatformingLevelCannonProjectile circusPlatformingLevelCannonProjectile = this.projectile.Create(this.shootRoots[this.shootIndex].transform.position, 0f, -this.projectileSpeed) as CircusPlatformingLevelCannonProjectile;
		circusPlatformingLevelCannonProjectile.SetColor(this.pinkSplits[this.pinkIndex]);
		circusPlatformingLevelCannonProjectile.DestroyDistance = 0f;
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkSplits.Length;
		if (this.goingBackwards)
		{
			if (this.shootIndex > 0)
			{
				this.shootIndex--;
			}
			else
			{
				this.shootIndex = this.shootRoots.Length - 1;
			}
		}
		else
		{
			this.shootIndex = (this.shootIndex + 1) % this.shootRoots.Length;
		}
		base.animator.SetInteger("Cannon", 0);
	}

	// Token: 0x0600335F RID: 13151 RVA: 0x001DE1A8 File Offset: 0x001DC5A8
	private IEnumerator slide_off_cr()
	{
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.animator.SetTrigger("Droop");
		float slideOffSpeed = 500f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.y < 1220f)
		{
			base.transform.AddPosition(0f, slideOffSpeed * CupheadTime.FixedDelta, 0f);
			yield return wait;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		yield break;
	}

	// Token: 0x06003360 RID: 13152 RVA: 0x001DE1C4 File Offset: 0x001DC5C4
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(new Vector2(this.startTrigger.transform.position.x, this.startTrigger.transform.position.y - 1000f), new Vector2(this.startTrigger.transform.position.x, this.startTrigger.transform.position.y + 1000f));
		Gizmos.DrawLine(new Vector2(this.endTrigger.transform.position.x, this.endTrigger.transform.position.y - 1000f), new Vector2(this.endTrigger.transform.position.x, this.endTrigger.transform.position.y + 1000f));
	}

	// Token: 0x06003361 RID: 13153 RVA: 0x001DE2E5 File Offset: 0x001DC6E5
	private void ShootSFX()
	{
		AudioManager.Play("circus_cannon_shoot");
		this.emitAudioFromObject.Add("circus_cannon_shoot");
	}

	// Token: 0x06003362 RID: 13154 RVA: 0x001DE301 File Offset: 0x001DC701
	private void DroopSFX()
	{
		AudioManager.Play("circus_cannon_droop");
		this.emitAudioFromObject.Add("circus_cannon_droop");
	}

	// Token: 0x04003BA1 RID: 15265
	private const string ShootParameterName = "Cannon";

	// Token: 0x04003BA2 RID: 15266
	[SerializeField]
	private float health;

	// Token: 0x04003BA3 RID: 15267
	[SerializeField]
	private DamageReceiver[] cannons;

	// Token: 0x04003BA4 RID: 15268
	[SerializeField]
	private Transform[] shootRoots;

	// Token: 0x04003BA5 RID: 15269
	[SerializeField]
	private CircusPlatformingLevelCannonProjectile projectile;

	// Token: 0x04003BA6 RID: 15270
	[SerializeField]
	private float projectileSpeed;

	// Token: 0x04003BA7 RID: 15271
	[SerializeField]
	private float projectileDelay;

	// Token: 0x04003BA8 RID: 15272
	[SerializeField]
	private Transform startTrigger;

	// Token: 0x04003BA9 RID: 15273
	[SerializeField]
	private Transform endTrigger;

	// Token: 0x04003BAA RID: 15274
	[SerializeField]
	private string pinkString;

	// Token: 0x04003BAB RID: 15275
	private int shootIndex;

	// Token: 0x04003BAC RID: 15276
	private bool goingBackwards;

	// Token: 0x04003BAD RID: 15277
	private bool isDead;

	// Token: 0x04003BAE RID: 15278
	private string[] pinkSplits;

	// Token: 0x04003BAF RID: 15279
	private int pinkIndex;
}
