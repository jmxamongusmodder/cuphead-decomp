using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200077E RID: 1918
public class RobotLevelHatchShotbot : AbstractCollidableObject
{
	// Token: 0x06002A16 RID: 10774 RVA: 0x00189680 File Offset: 0x00187A80
	public void InitShotbot(int hp, int bulletSpeed, int pinkBulletCount, float shootDelay, int flightSpeed)
	{
		this.speedPCT = 200f / (float)flightSpeed;
		this.health = (float)hp;
		this.flightSpeed = flightSpeed;
		this.bulletSpeed = bulletSpeed;
		this.pinkBulletCount = pinkBulletCount;
		this.shotsFired = 0;
		this.shootDelay = shootDelay;
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		base.StartCoroutine(this.rotate_cr());
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002A17 RID: 10775 RVA: 0x00189714 File Offset: 0x00187B14
	public RobotLevelHatchShotbot Create()
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		gameObject.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(180f));
		return gameObject.GetComponent<RobotLevelHatchShotbot>();
	}

	// Token: 0x06002A18 RID: 10776 RVA: 0x0018975C File Offset: 0x00187B5C
	private IEnumerator intro_cr()
	{
		float rotTime = 0.15f;
		float scale = base.transform.localScale.x;
		base.transform.SetEulerAngles(null, null, new float?(180f));
		yield return CupheadTime.WaitForSeconds(this, 0.5f * this.speedPCT);
		yield return base.StartCoroutine(this.tweenRotation_cr(180f * scale, 270f * scale, rotTime / 3f * this.speedPCT));
		yield return base.StartCoroutine(this.tweenRotation_cr(270f * scale, 180f * scale, rotTime / 3f * this.speedPCT));
		yield return null;
		yield break;
	}

	// Token: 0x06002A19 RID: 10777 RVA: 0x00189778 File Offset: 0x00187B78
	private IEnumerator move_cr()
	{
		float scale = base.transform.localScale.x;
		for (;;)
		{
			Vector2 move = base.transform.right * (float)this.flightSpeed * CupheadTime.Delta * scale;
			base.transform.AddPosition(move.x, move.y, 0f);
			yield return null;
			if (base.transform.position.y > 460f)
			{
				this.End();
			}
		}
		yield break;
	}

	// Token: 0x06002A1A RID: 10778 RVA: 0x00189794 File Offset: 0x00187B94
	private IEnumerator rotate_cr()
	{
		float rotTime = 0.15f * this.speedPCT;
		float scale = base.transform.localScale.x;
		yield return CupheadTime.WaitForSeconds(this, 1.8f * this.speedPCT);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.time.x * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(180f * scale, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 0f, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.x * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(0f, 90f * scale, rotTime));
			yield return CupheadTime.WaitForSeconds(this, this.time.y * this.speedPCT);
			yield return base.StartCoroutine(this.tweenRotation_cr(90f * scale, 180f * scale, rotTime));
		}
		yield break;
	}

	// Token: 0x06002A1B RID: 10779 RVA: 0x001897B0 File Offset: 0x00187BB0
	private IEnumerator tweenRotation_cr(float start, float end, float time)
	{
		base.transform.SetEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.transform.SetEulerAngles(null, null, new float?(EaseUtils.Ease(EaseUtils.EaseType.linear, start, end, val)));
			t += CupheadTime.Delta / 3f;
			yield return null;
		}
		base.transform.SetEulerAngles(null, null, new float?(end));
		yield break;
	}

	// Token: 0x06002A1C RID: 10780 RVA: 0x001897E0 File Offset: 0x00187BE0
	private IEnumerator fire_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.shootDelay);
			AudioManager.Play("robot_shotbot_shoot");
			this.emitAudioFromObject.Add("robot_shotbot_shoot");
			GameObject proj = UnityEngine.Object.Instantiate<GameObject>(this.projectile);
			proj.transform.position = base.transform.position;
			proj.transform.right = (PlayerManager.GetNext().center - base.transform.position).normalized;
			proj.GetComponent<BasicProjectile>().Speed = (float)this.bulletSpeed;
			if (this.shotsFired >= this.pinkBulletCount)
			{
				this.shotsFired = 0;
				proj.GetComponent<SpriteRenderer>().sprite = this.spriteSpecial;
				proj.GetComponent<BasicProjectile>().SetParryable(true);
			}
			else
			{
				this.shotsFired++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002A1D RID: 10781 RVA: 0x001897FB File Offset: 0x00187BFB
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Dead();
		}
	}

	// Token: 0x06002A1E RID: 10782 RVA: 0x00189826 File Offset: 0x00187C26
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002A1F RID: 10783 RVA: 0x00189844 File Offset: 0x00187C44
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002A20 RID: 10784 RVA: 0x0018985C File Offset: 0x00187C5C
	private void Dead()
	{
		AudioManager.Play("robot_shotbot_death");
		this.emitAudioFromObject.Add("robot_shotbot_death");
		this.StopAllCoroutines();
		this.CreateSmoke();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002A21 RID: 10785 RVA: 0x0018988F File Offset: 0x00187C8F
	private void End()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002A22 RID: 10786 RVA: 0x001898A4 File Offset: 0x00187CA4
	private void CreateSmoke()
	{
		this.smokeEffect.Create(base.transform.position);
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x040032F4 RID: 13044
	[SerializeField]
	private Effect smokeEffect;

	// Token: 0x040032F5 RID: 13045
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x040032F6 RID: 13046
	[SerializeField]
	private GameObject projectile;

	// Token: 0x040032F7 RID: 13047
	[SerializeField]
	private Sprite spriteSpecial;

	// Token: 0x040032F8 RID: 13048
	[SerializeField]
	private Vector2 time;

	// Token: 0x040032F9 RID: 13049
	private float speedPCT;

	// Token: 0x040032FA RID: 13050
	private float health;

	// Token: 0x040032FB RID: 13051
	private int flightSpeed;

	// Token: 0x040032FC RID: 13052
	private int bulletSpeed;

	// Token: 0x040032FD RID: 13053
	private int pinkBulletCount;

	// Token: 0x040032FE RID: 13054
	private int shotsFired;

	// Token: 0x040032FF RID: 13055
	private float shootDelay;

	// Token: 0x04003300 RID: 13056
	private DamageDealer damageDealer;

	// Token: 0x04003301 RID: 13057
	private const int MAX_HEIGHT = 460;
}
