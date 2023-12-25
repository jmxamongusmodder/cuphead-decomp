using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005FB RID: 1531
public class DragonLevelPotion : AbstractProjectile
{
	// Token: 0x17000377 RID: 887
	// (get) Token: 0x06001E78 RID: 7800 RVA: 0x001191A0 File Offset: 0x001175A0
	// (set) Token: 0x06001E79 RID: 7801 RVA: 0x001191A8 File Offset: 0x001175A8
	public DragonLevelPotion.State state { get; private set; }

	// Token: 0x06001E7A RID: 7802 RVA: 0x001191B4 File Offset: 0x001175B4
	public void Init(Vector2 pos, float hp, float rotation, LevelProperties.Dragon.Potions properties)
	{
		base.transform.position = pos;
		this.hp = hp;
		this.properties = properties;
		base.transform.SetScale(new float?(properties.potionScale), new float?(properties.potionScale), new float?(properties.potionScale));
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		this.state = DragonLevelPotion.State.Alive;
		this.moveRoutine = base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001E7B RID: 7803 RVA: 0x0011924E File Offset: 0x0011764E
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001E7C RID: 7804 RVA: 0x00119279 File Offset: 0x00117679
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E7D RID: 7805 RVA: 0x00119297 File Offset: 0x00117697
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001E7E RID: 7806 RVA: 0x001192B8 File Offset: 0x001176B8
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != DragonLevelPotion.State.Dead)
		{
			this.state = DragonLevelPotion.State.Dead;
			base.StopCoroutine(this.moveRoutine);
			base.StartCoroutine(this.handle_die_cr());
		}
	}

	// Token: 0x06001E7F RID: 7807 RVA: 0x00119314 File Offset: 0x00117714
	private IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.potionSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E80 RID: 7808 RVA: 0x00119330 File Offset: 0x00117730
	private IEnumerator handle_die_cr()
	{
		DragonLevelPotion.PotionType potionType = this.type;
		if (potionType != DragonLevelPotion.PotionType.Horizontal)
		{
			if (potionType != DragonLevelPotion.PotionType.Vertical)
			{
				if (potionType == DragonLevelPotion.PotionType.Both)
				{
					this.SpawnProjectile(Vector3.right);
					this.SpawnProjectile(-Vector3.right);
					this.SpawnProjectile(Vector3.up);
					this.SpawnProjectile(-Vector3.up);
				}
			}
			else
			{
				this.SpawnProjectile(Vector3.up);
				this.SpawnProjectile(-Vector3.up);
			}
		}
		else
		{
			this.SpawnProjectile(Vector3.right);
			this.SpawnProjectile(-Vector3.right);
		}
		base.animator.SetTrigger("Explode");
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06001E81 RID: 7809 RVA: 0x0011934C File Offset: 0x0011774C
	private void SpawnProjectile(Vector3 direction)
	{
		float rotation = MathUtils.DirectionToAngle(direction);
		this.bulletPrefab.Create(base.transform.position, rotation, this.properties.spitBulletSpeed).transform.SetScale(new float?(this.properties.explosionBulletScale), new float?(this.properties.explosionBulletScale), new float?(this.properties.explosionBulletScale));
	}

	// Token: 0x06001E82 RID: 7810 RVA: 0x001193C6 File Offset: 0x001177C6
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.bulletPrefab = null;
	}

	// Token: 0x04002751 RID: 10065
	private const string ExplodeTrigger = "Explode";

	// Token: 0x04002752 RID: 10066
	[SerializeField]
	private BasicProjectile bulletPrefab;

	// Token: 0x04002753 RID: 10067
	public DragonLevelPotion.PotionType type;

	// Token: 0x04002755 RID: 10069
	private LevelProperties.Dragon.Potions properties;

	// Token: 0x04002756 RID: 10070
	private DamageReceiver damageReceiver;

	// Token: 0x04002757 RID: 10071
	private float hp;

	// Token: 0x04002758 RID: 10072
	private Coroutine moveRoutine;

	// Token: 0x020005FC RID: 1532
	public enum PotionType
	{
		// Token: 0x0400275A RID: 10074
		Horizontal,
		// Token: 0x0400275B RID: 10075
		Vertical,
		// Token: 0x0400275C RID: 10076
		Both
	}

	// Token: 0x020005FD RID: 1533
	public enum State
	{
		// Token: 0x0400275E RID: 10078
		Alive,
		// Token: 0x0400275F RID: 10079
		Dead
	}
}
