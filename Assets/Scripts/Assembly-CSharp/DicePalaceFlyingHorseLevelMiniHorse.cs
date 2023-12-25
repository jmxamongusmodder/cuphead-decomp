using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C4 RID: 1476
public class DicePalaceFlyingHorseLevelMiniHorse : AbstractProjectile
{
	// Token: 0x17000363 RID: 867
	// (get) Token: 0x06001CC7 RID: 7367 RVA: 0x00107988 File Offset: 0x00105D88
	protected override float DestroyLifetime
	{
		get
		{
			return 20f;
		}
	}

	// Token: 0x06001CC8 RID: 7368 RVA: 0x00107990 File Offset: 0x00105D90
	protected override void Awake()
	{
		base.Awake();
		this.jockey.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		this.damageReceiver = this.jockey.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.jockeyAnimator = this.jockey.GetComponent<Animator>();
	}

	// Token: 0x06001CC9 RID: 7369 RVA: 0x001079F9 File Offset: 0x00105DF9
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && !this.jockeyDead)
		{
			this.KillJockey();
			this.jockeyDead = true;
		}
	}

	// Token: 0x06001CCA RID: 7370 RVA: 0x00107A36 File Offset: 0x00105E36
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CCB RID: 7371 RVA: 0x00107A54 File Offset: 0x00105E54
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CCC RID: 7372 RVA: 0x00107A74 File Offset: 0x00105E74
	public void Init(Vector3 position, float hp, LevelProperties.DicePalaceFlyingHorse.MiniHorses properties, AbstractPlayerController player, DicePalaceFlyingHorseLevelHorse.MiniHorseType type, bool isPink, float threeProximity, int lane, Vector3 backgroundLane)
	{
		base.transform.position = position;
		this.hp = hp;
		this.properties = properties;
		this.isPink = isPink;
		this.player = player;
		this.threeProximity = threeProximity;
		this.backgroundLane = backgroundLane;
		base.animator.SetInteger("Horse", UnityEngine.Random.Range(1, 3));
		if (type != DicePalaceFlyingHorseLevelHorse.MiniHorseType.One)
		{
			if (type != DicePalaceFlyingHorseLevelHorse.MiniHorseType.Two)
			{
				if (type == DicePalaceFlyingHorseLevelHorse.MiniHorseType.Three)
				{
					this.jockeyAnimator.SetInteger("Caddy", 4);
					this.horseCoroutine = base.StartCoroutine(this.horse_three_cr());
				}
			}
			else
			{
				this.jockeyAnimator.SetInteger("Caddy", UnityEngine.Random.Range(1, 4));
				this.horseCoroutine = base.StartCoroutine(this.horse_two_cr());
			}
		}
		else
		{
			this.jockeyAnimator.SetInteger("Caddy", UnityEngine.Random.Range(1, 4));
		}
		for (int i = 0; i < this.renderers.Length; i++)
		{
			this.renderers[i].sortingOrder = this.renderers.Length * lane + this.renderers[i].sortingOrder;
		}
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001CCD RID: 7373 RVA: 0x00107BAC File Offset: 0x00105FAC
	private IEnumerator move_cr()
	{
		float speed = this.properties.miniSpeedRange.RandomFloat();
		while (base.transform.position.x > -740f)
		{
			base.transform.AddPosition(-speed * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		base.transform.position = this.backgroundLane;
		base.transform.localScale = new Vector3(-0.5f, 0.5f, 0.5f);
		SpriteRenderer horseRenderer = base.GetComponent<SpriteRenderer>();
		horseRenderer.color = ColorUtils.HexToColor("C5C5C5FF");
		horseRenderer.sortingLayerName = "Default";
		horseRenderer.sortingOrder -= 100;
		if (this.jockey != null)
		{
			SpriteRenderer component = this.jockey.GetComponent<SpriteRenderer>();
			component.material = horseRenderer.material;
			component.color = horseRenderer.color;
			component.sortingLayerName = "Default";
			component.sortingOrder -= 100;
			this.jockey.GetComponent<Collider2D>().enabled = false;
		}
		base.GetComponent<Collider2D>().enabled = false;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		while (base.transform.position.x < 740f)
		{
			base.transform.AddPosition(speed * CupheadTime.Delta * 0.5f, 0f, 0f);
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06001CCE RID: 7374 RVA: 0x00107BC8 File Offset: 0x00105FC8
	private IEnumerator horse_two_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.miniTwoShotDelayRange.RandomFloat());
		if (!this.jockeyDead)
		{
			this.ShootBullet();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001CCF RID: 7375 RVA: 0x00107BE4 File Offset: 0x00105FE4
	private void ShootBullet()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 v = this.player.transform.position - base.transform.position;
		float rotation = MathUtils.DirectionToAngle(v);
		if (this.isPink)
		{
			this.pinkBullet.Create(base.transform.position, rotation, this.properties.miniTwoBulletSpeed);
		}
		else
		{
			this.bullet.Create(base.transform.position, rotation, this.properties.miniTwoBulletSpeed);
		}
	}

	// Token: 0x06001CD0 RID: 7376 RVA: 0x00107CAC File Offset: 0x001060AC
	private IEnumerator horse_three_cr()
	{
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		float dist = base.transform.position.x - this.player.transform.position.x;
		while (dist > this.threeProximity)
		{
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			dist = base.transform.position.x - this.player.transform.position.x;
			yield return null;
		}
		if (!this.jockeyDead)
		{
			this.jockeyAnimator.SetTrigger("Attack");
			yield return this.jockeyAnimator.WaitForAnimationToStart(this, "CloakedAttack_End", false);
		}
		if (!this.jockeyDead)
		{
			this.jockey.transform.SetParent(null);
			this.jockey.transform.GetChild(0).SetParent(base.transform);
		}
		while (this.jockey.transform.position.y < 360f && !this.jockeyDead)
		{
			this.jockey.transform.AddPosition(0f, this.properties.miniThreeJockeySpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		if (!this.jockeyDead)
		{
			this.KillJockey();
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001CD1 RID: 7377 RVA: 0x00107CC7 File Offset: 0x001060C7
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x06001CD2 RID: 7378 RVA: 0x00107CD5 File Offset: 0x001060D5
	private void KillJockey()
	{
		base.StopCoroutine(this.horseCoroutine);
		UnityEngine.Object.Destroy(this.jockey);
	}

	// Token: 0x06001CD3 RID: 7379 RVA: 0x00107CEE File Offset: 0x001060EE
	public override void OnLevelEnd()
	{
	}

	// Token: 0x040025B3 RID: 9651
	[SerializeField]
	private BasicProjectile bullet;

	// Token: 0x040025B4 RID: 9652
	[SerializeField]
	private BasicProjectile pinkBullet;

	// Token: 0x040025B5 RID: 9653
	[SerializeField]
	private GameObject jockey;

	// Token: 0x040025B6 RID: 9654
	[SerializeField]
	private SpriteRenderer[] renderers;

	// Token: 0x040025B7 RID: 9655
	private LevelProperties.DicePalaceFlyingHorse.MiniHorses properties;

	// Token: 0x040025B8 RID: 9656
	private AbstractPlayerController player;

	// Token: 0x040025B9 RID: 9657
	private DamageReceiver damageReceiver;

	// Token: 0x040025BA RID: 9658
	private Coroutine horseCoroutine;

	// Token: 0x040025BB RID: 9659
	private float hp;

	// Token: 0x040025BC RID: 9660
	private float threeProximity;

	// Token: 0x040025BD RID: 9661
	private bool isPink;

	// Token: 0x040025BE RID: 9662
	private bool jockeyDead;

	// Token: 0x040025BF RID: 9663
	private Vector3 backgroundLane;

	// Token: 0x040025C0 RID: 9664
	private Animator jockeyAnimator;
}
