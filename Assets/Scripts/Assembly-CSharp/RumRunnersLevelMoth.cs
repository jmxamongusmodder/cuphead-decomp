using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000797 RID: 1943
public class RumRunnersLevelMoth : AbstractCollidableObject
{
	// Token: 0x06002B20 RID: 11040 RVA: 0x00192500 File Offset: 0x00190900
	private void Start()
	{
		this.sparkWarning.SetActive(false);
		if (base.GetComponent<DamageReceiver>())
		{
			this.damageReceiver = base.GetComponent<DamageReceiver>();
			this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		}
	}

	// Token: 0x06002B21 RID: 11041 RVA: 0x0019254C File Offset: 0x0019094C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06002B22 RID: 11042 RVA: 0x00192578 File Offset: 0x00190978
	public void Init(Vector3 pos, LevelProperties.RumRunners.Moth properties, RumRunnersLevelSpider parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.hp = properties.hp;
		this.StartAttack();
		this.parent = parent;
		this.parent.OnDeathEvent += this.Die;
	}

	// Token: 0x06002B23 RID: 11043 RVA: 0x001925C8 File Offset: 0x001909C8
	private void StartAttack()
	{
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
		base.StartCoroutine(this.life_timer_cr());
	}

	// Token: 0x06002B24 RID: 11044 RVA: 0x001925F4 File Offset: 0x001909F4
	private IEnumerator move_cr()
	{
		this.goingLeft = Rand.Bool();
		float dist = (!this.goingLeft) ? Mathf.Abs(540f - base.transform.position.x) : Mathf.Abs(-540f - base.transform.position.x);
		float time = dist / this.properties.mothSpeed;
		float t = 0f;
		float start = base.transform.position.x;
		float end = (!this.goingLeft) ? 540f : -540f;
		YieldInstruction wait = new WaitForFixedUpdate();
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			base.transform.SetPosition(new float?(Mathf.Lerp(start, end, t / time)), null, null);
			yield return wait;
		}
		dist = Mathf.Abs(-1080f);
		time = dist / this.properties.mothSpeed;
		while (!this.dead)
		{
			t = 0f;
			this.goingLeft = !this.goingLeft;
			start = base.transform.position.x;
			end = ((!this.goingLeft) ? 540f : -540f);
			while (t < time)
			{
				t += CupheadTime.FixedDelta;
				base.transform.SetPosition(new float?(Mathf.Lerp(start, end, t / time)), null, null);
				yield return wait;
			}
			yield return wait;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002B25 RID: 11045 RVA: 0x00192610 File Offset: 0x00190A10
	private IEnumerator shoot_cr()
	{
		while (!this.dead)
		{
			this.sparkWarning.SetActive(false);
			yield return CupheadTime.WaitForSeconds(this, this.properties.mothShootDelay);
			this.sparkWarning.SetActive(true);
			this.regularProjectile.Create(base.transform.position, -90f, this.properties.mothBulletSpeed);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002B26 RID: 11046 RVA: 0x0019262C File Offset: 0x00190A2C
	private IEnumerator life_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.mothLifetime);
		this.Die();
		yield break;
	}

	// Token: 0x06002B27 RID: 11047 RVA: 0x00192647 File Offset: 0x00190A47
	private void Die()
	{
		this.dead = true;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002B28 RID: 11048 RVA: 0x00192661 File Offset: 0x00190A61
	protected override void OnDestroy()
	{
		this.parent.OnDeathEvent -= this.Die;
		base.OnDestroy();
	}

	// Token: 0x040033DC RID: 13276
	private const float RIGHT_X = 540f;

	// Token: 0x040033DD RID: 13277
	private const float LEFT_X = -540f;

	// Token: 0x040033DE RID: 13278
	[SerializeField]
	private GameObject sparkWarning;

	// Token: 0x040033DF RID: 13279
	[SerializeField]
	private BasicProjectile regularProjectile;

	// Token: 0x040033E0 RID: 13280
	private RumRunnersLevelSpider parent;

	// Token: 0x040033E1 RID: 13281
	private LevelProperties.RumRunners.Moth properties;

	// Token: 0x040033E2 RID: 13282
	private DamageReceiver damageReceiver;

	// Token: 0x040033E3 RID: 13283
	private float hp;

	// Token: 0x040033E4 RID: 13284
	private bool goingLeft;

	// Token: 0x040033E5 RID: 13285
	private bool dead;
}
