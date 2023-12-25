using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000633 RID: 1587
public class FlyingBlimpLevelEnemy : AbstractCollidableObject
{
	// Token: 0x17000384 RID: 900
	// (get) Token: 0x06002080 RID: 8320 RVA: 0x0012BB8F File Offset: 0x00129F8F
	// (set) Token: 0x06002081 RID: 8321 RVA: 0x0012BB97 File Offset: 0x00129F97
	public FlyingBlimpLevelEnemy.State state { get; private set; }

	// Token: 0x06002082 RID: 8322 RVA: 0x0012BBA0 File Offset: 0x00129FA0
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002083 RID: 8323 RVA: 0x0012BBD6 File Offset: 0x00129FD6
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002084 RID: 8324 RVA: 0x0012BBF0 File Offset: 0x00129FF0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state == FlyingBlimpLevelEnemy.State.Spawned)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.dying_cr());
		}
	}

	// Token: 0x06002085 RID: 8325 RVA: 0x0012BC3F File Offset: 0x0012A03F
	private void Start()
	{
		this.startPoint = base.transform.position;
	}

	// Token: 0x06002086 RID: 8326 RVA: 0x0012BC54 File Offset: 0x0012A054
	public void Init(LevelProperties.FlyingBlimp properties, Vector3 startPoint, float stopPoint, bool Aparryable, FlyingBlimpLevelBlimpLady parent)
	{
		this.enemyProperties = properties.CurrentState.enemy;
		this.properties = properties;
		this.parent = parent;
		this.startPoint = startPoint;
		this.stopPoint = stopPoint;
		this.parent.OnDeathEvent += this.Die;
		this.parryable = Aparryable;
		base.StartCoroutine(this.emerge_cr());
	}

	// Token: 0x06002087 RID: 8327 RVA: 0x0012BCBC File Offset: 0x0012A0BC
	private void CreatePieces()
	{
		foreach (FlyingBlimpLevelEnemyDeathPart flyingBlimpLevelEnemyDeathPart in this.deathPieces)
		{
			flyingBlimpLevelEnemyDeathPart.CreatePart(base.transform.position, this.properties.CurrentState.gear);
		}
	}

	// Token: 0x06002088 RID: 8328 RVA: 0x0012BD0A File Offset: 0x0012A10A
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.parent.OnDeathEvent -= this.Die;
		this.projectilePrefab = null;
		this.parryablePrefab = null;
		this.deathPieces = null;
	}

	// Token: 0x06002089 RID: 8329 RVA: 0x0012BD3E File Offset: 0x0012A13E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600208A RID: 8330 RVA: 0x0012BD5C File Offset: 0x0012A15C
	public IEnumerator emerge_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.state = FlyingBlimpLevelEnemy.State.Spawned;
		this.hp = (float)this.enemyProperties.hp;
		Collider2D collider = base.GetComponent<Collider2D>();
		collider.enabled = true;
		while (base.transform.position.x > this.stopPoint)
		{
			base.transform.position += base.transform.right * -this.enemyProperties.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		yield return CupheadTime.WaitForSeconds(this, this.enemyProperties.shotDelay);
		base.animator.Play("Enemy_Attack");
		AudioManager.Play("level_flying_blimp_cannon_ship_fire");
		yield return base.animator.WaitForAnimationToEnd(this, "Enemy_Attack", false, true);
		while (base.transform.position.x <= this.startPoint.x)
		{
			base.transform.position += base.transform.right * this.enemyProperties.speed * CupheadTime.FixedDelta;
			yield return wait;
			if (base.transform.position.x > this.startPoint.x)
			{
				this.Die();
			}
		}
		yield break;
	}

	// Token: 0x0600208B RID: 8331 RVA: 0x0012BD78 File Offset: 0x0012A178
	private void FireSpreadshot()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float x = next.transform.position.x - base.transform.position.x;
		float y = next.transform.position.y - base.transform.position.y;
		Effect effect = UnityEngine.Object.Instantiate<Effect>(this.bulletEffect);
		effect.transform.position = this.projectileRoot.transform.position;
		effect.GetComponent<Animator>().SetInteger("PickAni", UnityEngine.Random.Range(0, 3));
		for (int i = 0; i < this.enemyProperties.numBullets; i++)
		{
			float num = this.enemyProperties.spreadAngle.GetFloatAt((float)i / ((float)this.enemyProperties.numBullets - 1f));
			float num2 = this.enemyProperties.spreadAngle.max / 2f;
			num -= num2;
			float num3 = Mathf.Atan2(y, x) * 57.29578f;
			this.animationPicker = UnityEngine.Random.Range(0, 3);
			if (next.transform.position.x > base.transform.position.x)
			{
				num3 = (float)((next.transform.position.y <= base.transform.position.y) ? -90 : 90);
			}
			int num4 = this.animationPicker;
			if (num4 != 0)
			{
				if (num4 != 1)
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num3 + num, this.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_3");
				}
				else
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num3 + num, this.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_2");
				}
			}
			else
			{
				this.projectilePrefab.Create(this.projectileRoot.position, num3 + num, this.enemyProperties.BSpeed).GetComponent<Animator>().Play("Bullet_1");
			}
		}
	}

	// Token: 0x0600208C RID: 8332 RVA: 0x0012BFE4 File Offset: 0x0012A3E4
	private void FireSingle()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		float x = next.transform.position.x - base.transform.position.x;
		float y = next.transform.position.y - base.transform.position.y;
		float num = -3f;
		float num2 = Mathf.Atan2(y, x) * 57.29578f;
		Effect effect = UnityEngine.Object.Instantiate<Effect>(this.bulletEffect);
		effect.transform.position = this.projectileRoot.transform.position;
		effect.GetComponent<Animator>().SetInteger("PickAni", UnityEngine.Random.Range(0, 3));
		if (next.transform.position.x > base.transform.position.x)
		{
			num2 = (float)((next.transform.position.y <= base.transform.position.y) ? -90 : 90);
		}
		if (!this.parryable)
		{
			this.animationPicker = UnityEngine.Random.Range(0, 3);
		}
		else
		{
			this.animationPicker = UnityEngine.Random.Range(0, 2);
		}
		if (!this.parryable)
		{
			int num3 = this.animationPicker;
			if (num3 != 0)
			{
				if (num3 != 1)
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num2 + num, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_3");
				}
				else
				{
					this.projectilePrefab.Create(this.projectileRoot.position, num2 + num, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_2");
				}
			}
			else
			{
				this.projectilePrefab.Create(this.projectileRoot.position, num2 + num, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_1");
			}
		}
		else
		{
			int num4 = this.animationPicker;
			if (num4 != 0)
			{
				this.parryablePrefab.Create(this.projectileRoot.position, num2 + num, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_2");
			}
			else
			{
				this.parryablePrefab.Create(this.projectileRoot.position, num2 + num, this.enemyProperties.ASpeed).GetComponent<Animator>().Play("Bullet_1");
			}
		}
	}

	// Token: 0x0600208D RID: 8333 RVA: 0x0012C2AD File Offset: 0x0012A6AD
	private void FlipEnemy()
	{
		base.GetComponent<SpriteRenderer>().flipX = !base.GetComponent<SpriteRenderer>().flipX;
	}

	// Token: 0x0600208E RID: 8334 RVA: 0x0012C2C8 File Offset: 0x0012A6C8
	private IEnumerator dying_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		AudioManager.Play("level_flying_blimp_cannon_ship_death");
		base.animator.Play("Enemy_Explode");
		yield return base.animator.WaitForAnimationToEnd(this, "Enemy_Explode", false, true);
		this.Die();
		yield break;
	}

	// Token: 0x0600208F RID: 8335 RVA: 0x0012C2E3 File Offset: 0x0012A6E3
	private void Die()
	{
		this.state = FlyingBlimpLevelEnemy.State.Unspawned;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002900 RID: 10496
	private LevelProperties.FlyingBlimp.Enemy enemyProperties;

	// Token: 0x04002901 RID: 10497
	private LevelProperties.FlyingBlimp properties;

	// Token: 0x04002902 RID: 10498
	private Vector3 startPoint;

	// Token: 0x04002903 RID: 10499
	[SerializeField]
	private Effect bulletEffect;

	// Token: 0x04002904 RID: 10500
	[SerializeField]
	private FlyingBlimpLevelEnemyDeathPart[] deathPieces;

	// Token: 0x04002905 RID: 10501
	[SerializeField]
	private FlyingBlimpLevelEnemyProjectile projectilePrefab;

	// Token: 0x04002906 RID: 10502
	[SerializeField]
	private FlyingBlimpLevelEnemyProjectile parryablePrefab;

	// Token: 0x04002907 RID: 10503
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04002908 RID: 10504
	private AbstractPlayerController player;

	// Token: 0x04002909 RID: 10505
	private FlyingBlimpLevelBlimpLady parent;

	// Token: 0x0400290A RID: 10506
	private DamageDealer damageDealer;

	// Token: 0x0400290B RID: 10507
	private DamageReceiver damageReceiver;

	// Token: 0x0400290C RID: 10508
	private float hp;

	// Token: 0x0400290D RID: 10509
	private float stopPoint;

	// Token: 0x0400290E RID: 10510
	private bool parryable;

	// Token: 0x0400290F RID: 10511
	private int animationPicker;

	// Token: 0x02000634 RID: 1588
	public enum State
	{
		// Token: 0x04002911 RID: 10513
		Unspawned,
		// Token: 0x04002912 RID: 10514
		Spawned
	}
}
