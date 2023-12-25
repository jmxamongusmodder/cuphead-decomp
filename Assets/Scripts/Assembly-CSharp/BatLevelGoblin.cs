using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000506 RID: 1286
public class BatLevelGoblin : AbstractCollidableObject
{
	// Token: 0x060016C7 RID: 5831 RVA: 0x000CCDB3 File Offset: 0x000CB1B3
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060016C8 RID: 5832 RVA: 0x000CCDE9 File Offset: 0x000CB1E9
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health < 0f)
		{
			this.Die();
		}
	}

	// Token: 0x060016C9 RID: 5833 RVA: 0x000CCE14 File Offset: 0x000CB214
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060016CA RID: 5834 RVA: 0x000CCE2C File Offset: 0x000CB22C
	public void Init(LevelProperties.Bat.Goblins properties, Vector2 pos, bool onLeft, bool isShooter, float health)
	{
		base.transform.position = pos;
		this.onLeft = onLeft;
		this.isShooter = isShooter;
		this.properties = properties;
		this.health = health;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060016CB RID: 5835 RVA: 0x000CCE6A File Offset: 0x000CB26A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060016CC RID: 5836 RVA: 0x000CCE84 File Offset: 0x000CB284
	private IEnumerator move_cr()
	{
		float endpos = (float)((!this.onLeft) ? -640 : 640);
		float t = 0f;
		while (base.transform.position.x != endpos)
		{
			Vector3 pos = base.transform.position;
			pos.x = Mathf.MoveTowards(base.transform.position.x, endpos, this.properties.runSpeed * CupheadTime.Delta);
			base.transform.position = pos;
			if (this.isShooter)
			{
				t += CupheadTime.Delta;
				if (t >= this.properties.timeBeforeShoot)
				{
					yield return CupheadTime.WaitForSeconds(this, this.properties.initalShotDelay);
					this.ShootBullet();
					yield return CupheadTime.WaitForSeconds(this, this.properties.shooterHold);
					this.isShooter = false;
				}
			}
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x060016CD RID: 5837 RVA: 0x000CCEA0 File Offset: 0x000CB2A0
	private void ShootBullet()
	{
		float x;
		if (this.onLeft)
		{
			x = 15469.86f;
		}
		else
		{
			x = -5156.62f;
		}
		float rotation = Mathf.Atan2(base.transform.position.y, x) * 57.29578f;
		this.projectile.Create(base.transform.position, rotation, this.properties.bulletSpeed);
	}

	// Token: 0x060016CE RID: 5838 RVA: 0x000CCF17 File Offset: 0x000CB317
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002014 RID: 8212
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x04002015 RID: 8213
	private LevelProperties.Bat.Goblins properties;

	// Token: 0x04002016 RID: 8214
	private bool onLeft;

	// Token: 0x04002017 RID: 8215
	private bool isShooter;

	// Token: 0x04002018 RID: 8216
	private float health;

	// Token: 0x04002019 RID: 8217
	private DamageDealer damageDealer;

	// Token: 0x0400201A RID: 8218
	private DamageReceiver damageReceiver;
}
