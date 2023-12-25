using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004ED RID: 1261
public class BaronessLevelGumball : BaronessLevelMiniBossBase
{
	// Token: 0x17000321 RID: 801
	// (get) Token: 0x060015F9 RID: 5625 RVA: 0x000C5167 File Offset: 0x000C3567
	// (set) Token: 0x060015FA RID: 5626 RVA: 0x000C516F File Offset: 0x000C356F
	public BaronessLevelGumball.State state { get; private set; }

	// Token: 0x060015FB RID: 5627 RVA: 0x000C5178 File Offset: 0x000C3578
	protected override void Awake()
	{
		base.Awake();
		this.fadeTime = 0.6f;
		this.isDying = false;
		this.movingLeft = true;
		base.RegisterCollisionChild(this.headCollider);
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiverChild = this.headCollider.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageReceiverChild.OnDamageTaken += this.OnDamageTaken;
		this.headCollider.OnPlayerCollision += this.OnCollisionPlayer;
		this.headCollider.OnPlayerProjectileCollision += this.OnCollisionPlayerProjectile;
	}

	// Token: 0x060015FC RID: 5628 RVA: 0x000C5238 File Offset: 0x000C3638
	protected override void Start()
	{
		base.Start();
		this.legs.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.legs.GetComponent<SpriteRenderer>().sortingOrder = 130;
		this.lid.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.lid.GetComponent<SpriteRenderer>().sortingOrder = 140;
		AudioManager.PlayLoop("level_baroness_gumball_feet_loop");
		this.emitAudioFromObject.Add("level_baroness_gumball_feet_loop");
	}

	// Token: 0x060015FD RID: 5629 RVA: 0x000C52D0 File Offset: 0x000C36D0
	public void Init(LevelProperties.Baroness.Gumball properties, Vector2 pos, float health)
	{
		this.properties = properties;
		this.health = health;
		base.transform.position = pos;
		this.offTime = properties.gumballAttackDurationOffRange;
		base.StartCoroutine(this.leaving_castle_cr());
		base.StartCoroutine(this.switch_child_cr());
		base.StartCoroutine(this.gumball_off_timer_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060015FE RID: 5630 RVA: 0x000C5344 File Offset: 0x000C3744
	protected virtual IEnumerator leaving_castle_cr()
	{
		float t = 0f;
		float offTime = 0.22f;
		while (t < offTime)
		{
			this.lid.GetComponent<SpriteRenderer>().enabled = false;
			base.GetComponent<SpriteRenderer>().enabled = false;
			t += CupheadTime.Delta;
			yield return null;
		}
		this.lid.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<SpriteRenderer>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x060015FF RID: 5631 RVA: 0x000C5360 File Offset: 0x000C3760
	private IEnumerator switch_child_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 3f);
		this.legs.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.lid.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Background.ToString();
		this.lid.GetComponent<SpriteRenderer>().sortingOrder = 252;
		this.legs.GetComponent<SpriteRenderer>().sortingOrder = 251;
		yield break;
	}

	// Token: 0x06001600 RID: 5632 RVA: 0x000C537B File Offset: 0x000C377B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001601 RID: 5633 RVA: 0x000C5399 File Offset: 0x000C3799
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001602 RID: 5634 RVA: 0x000C53B4 File Offset: 0x000C37B4
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health > 0f)
		{
			base.OnDamageTaken(info);
		}
		this.health -= info.damage;
		if (this.health < 0f && this.state != BaronessLevelGumball.State.Dying)
		{
			DamageDealer.DamageInfo info2 = new DamageDealer.DamageInfo(this.health, info.direction, info.origin, info.damageSource);
			base.OnDamageTaken(info2);
			this.state = BaronessLevelGumball.State.Dying;
			base.StartCoroutine(this.death_cr());
		}
	}

	// Token: 0x06001603 RID: 5635 RVA: 0x000C5440 File Offset: 0x000C3840
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.projectilePrefabs = null;
	}

	// Token: 0x06001604 RID: 5636 RVA: 0x000C5450 File Offset: 0x000C3850
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		bool endedLoop = false;
		bool movingRight = false;
		float time = this.properties.gumballMovementSpeed;
		float end = 0f;
		float t = 0f;
		for (;;)
		{
			float start = base.transform.position.x;
			if (movingRight)
			{
				end = 640f - this.properties.offsetX.max;
			}
			else
			{
				end = -640f + this.properties.offsetX.min;
			}
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start, end, val)), null, null);
				if (val > 0.8f && !endedLoop)
				{
					if (this.isDying && !movingRight)
					{
						break;
					}
					if (this.state == BaronessLevelGumball.State.On)
					{
						this.headSpark.SetActive(false);
					}
					base.animator.SetBool("Turn", true);
					base.animator.Play("Run_Legs");
					endedLoop = true;
				}
				t += CupheadTime.FixedDelta;
				yield return wait;
			}
			if (this.isDying)
			{
				break;
			}
			endedLoop = false;
			t = 0f;
			base.transform.SetPosition(new float?(end), null, null);
			movingRight = !movingRight;
			yield return wait;
		}
		while (base.transform.position.x > -940f)
		{
			base.transform.AddPosition(-this.properties.gumballDeathSpeed * CupheadTime.FixedDelta, 0f, 0f);
			yield return wait;
		}
		AudioManager.Stop("level_baroness_gumball_feet_loop");
		this.Die();
		yield break;
	}

	// Token: 0x06001605 RID: 5637 RVA: 0x000C546C File Offset: 0x000C386C
	private void Switch()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		this.feetDust.SetScale(new float?(-this.feetDust.localScale.x), new float?(1f), new float?(1f));
		base.animator.SetBool("Turn", false);
		if (this.state == BaronessLevelGumball.State.On)
		{
			this.headSpark.SetActive(true);
		}
	}

	// Token: 0x06001606 RID: 5638 RVA: 0x000C5514 File Offset: 0x000C3914
	private IEnumerator on_cr()
	{
		float rateTime = 0f;
		float attackTime = 0f;
		float attackDuration = this.properties.gumballAttackDurationOnRange.RandomFloat();
		base.animator.SetBool("Open", true);
		yield return base.animator.WaitForAnimationToStart(this, "Run_Open_Trans", false);
		AudioManager.PlayLoop("level_baroness_gumball_shoot_loop");
		this.emitAudioFromObject.Add("level_baroness_gumball_shoot_loop");
		this.headSpark.SetActive(true);
		this.state = BaronessLevelGumball.State.On;
		while (attackTime < attackDuration)
		{
			if (this.isDying)
			{
				break;
			}
			attackTime += CupheadTime.Delta;
			if (rateTime > this.properties.rateOfFire)
			{
				this.fireProjectiles();
				rateTime = 0f;
			}
			else
			{
				rateTime += CupheadTime.Delta;
			}
			yield return null;
		}
		base.animator.SetBool("Open", false);
		yield return new WaitForEndOfFrame();
		base.StartCoroutine(this.gumball_off_timer_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001607 RID: 5639 RVA: 0x000C5530 File Offset: 0x000C3930
	private void fireProjectiles()
	{
		Vector2 zero = Vector2.zero;
		float num = (float)((!this.movingLeft) ? 200 : -200);
		zero.y = this.properties.velocityY.RandomFloat();
		zero.x = this.properties.velocityX.RandomFloat() + num;
		this.projectilePrefabs[UnityEngine.Random.Range(0, this.projectilePrefabs.Length - 1)].Create(this.projectileRoot.position, zero, this.properties.gravity);
	}

	// Token: 0x06001608 RID: 5640 RVA: 0x000C55C8 File Offset: 0x000C39C8
	private IEnumerator gumball_off_timer_cr()
	{
		this.headSpark.SetActive(false);
		AudioManager.Stop("level_baroness_gumball_shoot_loop");
		this.state = BaronessLevelGumball.State.Off;
		this.offTime = this.properties.gumballAttackDurationOffRange.RandomFloat();
		yield return CupheadTime.WaitForSeconds(this, this.offTime);
		base.StartCoroutine(this.on_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001609 RID: 5641 RVA: 0x000C55E4 File Offset: 0x000C39E4
	private IEnumerator death_cr()
	{
		this.StartExplosions();
		this.headCollider.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
		this.isDying = true;
		base.animator.Play("Run_Death");
		base.animator.SetTrigger("Death");
		yield return null;
		yield break;
	}

	// Token: 0x0600160A RID: 5642 RVA: 0x000C55FF File Offset: 0x000C39FF
	private void SoundGumballLidOpen()
	{
		AudioManager.Play("level_baroness_gumball_lid_open");
		this.emitAudioFromObject.Add("level_baroness_gumball_lid_open");
	}

	// Token: 0x0600160B RID: 5643 RVA: 0x000C561B File Offset: 0x000C3A1B
	private void SoundGumballLidClose()
	{
		AudioManager.Play("level_baroness_gumball_lid_close");
		this.emitAudioFromObject.Add("level_baroness_gumball_lid_close");
	}

	// Token: 0x04001F48 RID: 8008
	[SerializeField]
	private BaronessLevelGumballProjectile[] projectilePrefabs;

	// Token: 0x04001F49 RID: 8009
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x04001F4A RID: 8010
	[SerializeField]
	private SpriteRenderer lid;

	// Token: 0x04001F4B RID: 8011
	[SerializeField]
	private SpriteRenderer legs;

	// Token: 0x04001F4C RID: 8012
	[SerializeField]
	private CollisionChild headCollider;

	// Token: 0x04001F4D RID: 8013
	[SerializeField]
	private GameObject headSpark;

	// Token: 0x04001F4E RID: 8014
	[SerializeField]
	private Transform feetDust;

	// Token: 0x04001F4F RID: 8015
	private LevelProperties.Baroness.Gumball properties;

	// Token: 0x04001F50 RID: 8016
	private DamageDealer damageDealer;

	// Token: 0x04001F51 RID: 8017
	private DamageReceiver damageReceiver;

	// Token: 0x04001F52 RID: 8018
	private DamageReceiver damageReceiverChild;

	// Token: 0x04001F53 RID: 8019
	private float health;

	// Token: 0x04001F54 RID: 8020
	private float offTime;

	// Token: 0x04001F55 RID: 8021
	private float onTime;

	// Token: 0x04001F56 RID: 8022
	private bool movingLeft;

	// Token: 0x04001F57 RID: 8023
	private bool slowDown;

	// Token: 0x020004EE RID: 1262
	public enum State
	{
		// Token: 0x04001F59 RID: 8025
		On,
		// Token: 0x04001F5A RID: 8026
		Off,
		// Token: 0x04001F5B RID: 8027
		Dying
	}
}
