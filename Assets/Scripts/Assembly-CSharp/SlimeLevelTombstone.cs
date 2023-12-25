using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007E1 RID: 2017
public class SlimeLevelTombstone : LevelProperties.Slime.Entity
{
	// Token: 0x17000414 RID: 1044
	// (get) Token: 0x06002E25 RID: 11813 RVA: 0x001B3593 File Offset: 0x001B1993
	// (set) Token: 0x06002E26 RID: 11814 RVA: 0x001B359B File Offset: 0x001B199B
	public SlimeLevelTombstone.State state { get; private set; }

	// Token: 0x06002E27 RID: 11815 RVA: 0x001B35A4 File Offset: 0x001B19A4
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.GetComponent<LevelBossDeathExploder>().enabled = false;
	}

	// Token: 0x06002E28 RID: 11816 RVA: 0x001B3617 File Offset: 0x001B1A17
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002E29 RID: 11817 RVA: 0x001B362F File Offset: 0x001B1A2F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.dealDamage && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002E2A RID: 11818 RVA: 0x001B3658 File Offset: 0x001B1A58
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002E2B RID: 11819 RVA: 0x001B366B File Offset: 0x001B1A6B
	public override void LevelInit(LevelProperties.Slime properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06002E2C RID: 11820 RVA: 0x001B3674 File Offset: 0x001B1A74
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.dustPrefab = null;
		this.smashDustBackPrefab = null;
		this.smashDustFrontPrefab = null;
		this.tinySlime = null;
	}

	// Token: 0x06002E2D RID: 11821 RVA: 0x001B3698 File Offset: 0x001B1A98
	public void StartIntro(float x)
	{
		this.state = SlimeLevelTombstone.State.Intro;
		base.transform.SetPosition(new float?(x), null, null);
		this.offsetIndex = UnityEngine.Random.Range(0, base.properties.CurrentState.tombstone.attackOffsetString.Length);
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = true;
		}
		base.properties.OnBossDeath += this.OnBossDeath;
		base.GetComponent<LevelBossDeathExploder>().enabled = true;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002E2E RID: 11822 RVA: 0x001B374C File Offset: 0x001B1B4C
	private IEnumerator intro_cr()
	{
		base.StartCoroutine(this.crush_slime_cr());
		AudioManager.Play("slime_tombstone_drop_onto_slime");
		this.emitAudioFromObject.Add("slime_tombstone_drop_onto_slime");
		yield return base.TweenPositionY(550f, -80f, 0.2f, EaseUtils.EaseType.linear);
		base.animator.SetTrigger("Continue");
		this.dustPrefab.Create(base.transform.position);
		this.StartMove();
		yield break;
	}

	// Token: 0x06002E2F RID: 11823 RVA: 0x001B3768 File Offset: 0x001B1B68
	private IEnumerator crush_slime_cr()
	{
		while (base.transform.position.y > 70f)
		{
			yield return null;
		}
		this.bigSlime.Explode();
		if (SlimeLevelSlime.TINIES)
		{
			SlimeLevelTinySlime slimeLevelTinySlime = UnityEngine.Object.Instantiate<SlimeLevelTinySlime>(this.tinySlime);
			SlimeLevelTinySlime slimeLevelTinySlime2 = UnityEngine.Object.Instantiate<SlimeLevelTinySlime>(this.tinySlime);
			slimeLevelTinySlime.Init(this.dust.transform.position, base.properties.CurrentState.tombstone, true, this);
			slimeLevelTinySlime2.Init(this.dust.transform.position, base.properties.CurrentState.tombstone, false, this);
		}
		CupheadLevelCamera.Current.Shake(20f, 0.7f, false);
		yield break;
	}

	// Token: 0x06002E30 RID: 11824 RVA: 0x001B3783 File Offset: 0x001B1B83
	private void StartMove()
	{
		this.state = SlimeLevelTombstone.State.Move;
		this.wantsToSmash = false;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.waitForSmash_cr());
	}

	// Token: 0x06002E31 RID: 11825 RVA: 0x001B37B0 File Offset: 0x001B1BB0
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		this.direction = ((!MathUtils.RandomBool()) ? SlimeLevelTombstone.Direction.Right : SlimeLevelTombstone.Direction.Left);
		string[] offsets = base.properties.CurrentState.tombstone.attackOffsetString.Split(new char[]
		{
			','
		});
		this.offsetIndex = (this.offsetIndex + 1) % offsets.Length;
		float offset = 0f;
		Parser.FloatTryParse(offsets[this.offsetIndex], out offset);
		bool justStarted = true;
		while (!this.wantsToSmash)
		{
			base.animator.SetTrigger((this.direction != SlimeLevelTombstone.Direction.Right) ? "MoveLeft" : "MoveRight");
			yield return base.animator.WaitForAnimationToStart(this, (this.direction != SlimeLevelTombstone.Direction.Right) ? "Move_Left" : "Move_Right", false);
			base.animator.Play("Dirt");
			if (justStarted)
			{
				base.animator.Play("Dust_Start");
			}
			else
			{
				base.animator.Play("Dust_Start_End");
			}
			AudioManager.Play("slime_tombstone_slide");
			this.emitAudioFromObject.Add("slime_tombstone_slide");
			float startX = base.transform.position.x;
			float endX = (this.direction != SlimeLevelTombstone.Direction.Right) ? -500f : 500f;
			float moveTime = Mathf.Abs(startX - endX) / base.properties.CurrentState.tombstone.moveSpeed;
			yield return base.TweenPositionX(startX, endX, moveTime, EaseUtils.EaseType.easeInOutSine);
			this.direction = ((this.direction != SlimeLevelTombstone.Direction.Right) ? SlimeLevelTombstone.Direction.Right : SlimeLevelTombstone.Direction.Left);
			justStarted = false;
		}
		base.animator.SetTrigger((this.direction != SlimeLevelTombstone.Direction.Right) ? "MoveLeft" : "MoveRight");
		yield return base.animator.WaitForAnimationToStart(this, (this.direction != SlimeLevelTombstone.Direction.Right) ? "Move_Left" : "Move_Right", false);
		base.animator.Play("Dust_Start_End");
		AudioManager.Play("slime_tombstone_slide");
		this.emitAudioFromObject.Add("slime_tombstone_slide");
		AbstractPlayerController player = PlayerManager.GetNext();
		float startX2 = base.transform.position.x;
		float endX2 = (this.direction != SlimeLevelTombstone.Direction.Right) ? -500f : 500f;
		float moveTime2 = Mathf.Abs(startX2 - endX2) / base.properties.CurrentState.tombstone.moveSpeed;
		float targetX = 0f;
		float t = 0f;
		bool centeredOnPlayer = false;
		while (!centeredOnPlayer && t < moveTime2)
		{
			yield return wait;
			t += CupheadTime.FixedDelta * this.hitPauseCoefficient();
			base.transform.SetPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, startX2, endX2, t / moveTime2)), null, null);
			if (player == null || player.IsDead)
			{
				player = PlayerManager.GetNext();
			}
			targetX = player.center.x + offset;
			if ((this.direction == SlimeLevelTombstone.Direction.Right && base.transform.position.x > targetX) || (this.direction == SlimeLevelTombstone.Direction.Left && base.transform.position.x < targetX))
			{
				centeredOnPlayer = true;
			}
		}
		base.transform.SetPosition(new float?(Mathf.Clamp(targetX, -500f, 500f)), null, null);
		base.animator.Play("Dust_End");
		base.animator.Play("Dirt_Off");
		this.StartSmash();
		yield break;
	}

	// Token: 0x06002E32 RID: 11826 RVA: 0x001B37CC File Offset: 0x001B1BCC
	private void DustDirection()
	{
		this.dirt.SetScale(new float?((float)((this.direction != SlimeLevelTombstone.Direction.Right) ? -1 : 1)), null, null);
		this.dust.SetScale(new float?((float)((this.direction != SlimeLevelTombstone.Direction.Right) ? 1 : -1)), null, null);
		this.dust2.SetScale(new float?((float)((this.direction != SlimeLevelTombstone.Direction.Right) ? -1 : 1)), null, null);
	}

	// Token: 0x06002E33 RID: 11827 RVA: 0x001B387B File Offset: 0x001B1C7B
	private float hitPauseCoefficient()
	{
		return (!base.GetComponent<DamageReceiver>().IsHitPaused) ? 1f : 0f;
	}

	// Token: 0x06002E34 RID: 11828 RVA: 0x001B389C File Offset: 0x001B1C9C
	private IEnumerator waitForSmash_cr()
	{
		float timeUntilAttack = base.properties.CurrentState.tombstone.attackDelay.RandomFloat();
		yield return CupheadTime.WaitForSeconds(this, timeUntilAttack);
		this.wantsToSmash = true;
		yield break;
	}

	// Token: 0x06002E35 RID: 11829 RVA: 0x001B38B7 File Offset: 0x001B1CB7
	private void StartSmash()
	{
		this.state = SlimeLevelTombstone.State.Smash;
		base.StartCoroutine(this.smash_cr());
	}

	// Token: 0x06002E36 RID: 11830 RVA: 0x001B38D0 File Offset: 0x001B1CD0
	private IEnumerator smash_cr()
	{
		base.animator.SetTrigger("StartSmash");
		yield return base.animator.WaitForAnimationToStart(this, "Smash_Pre_Hold", false);
		AudioManager.Play("slime_tombstone_splat");
		this.emitAudioFromObject.Add("slime_tombstone_splat");
		AudioManager.Play("slime_tombstone_splat_start");
		this.emitAudioFromObject.Add("slime_tombstone_splat_start");
		AudioManager.Stop("slime_tombstone_slide");
		this.emitAudioFromObject.Add("slime_tombstone_slide");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tombstone.anticipationHold);
		base.animator.SetTrigger("Continue");
		this.StartMove();
		yield break;
	}

	// Token: 0x06002E37 RID: 11831 RVA: 0x001B38EB File Offset: 0x001B1CEB
	private void DisableDamageReceiver()
	{
		this.damageReceiver.enabled = false;
	}

	// Token: 0x06002E38 RID: 11832 RVA: 0x001B38F9 File Offset: 0x001B1CF9
	private void EnableDamageReceiver()
	{
		this.damageReceiver.enabled = true;
	}

	// Token: 0x06002E39 RID: 11833 RVA: 0x001B3907 File Offset: 0x001B1D07
	private void EnableDamageDealer()
	{
		this.dealDamage = true;
	}

	// Token: 0x06002E3A RID: 11834 RVA: 0x001B3910 File Offset: 0x001B1D10
	private void DisableDamageDealer()
	{
		this.dealDamage = false;
	}

	// Token: 0x06002E3B RID: 11835 RVA: 0x001B391C File Offset: 0x001B1D1C
	private void OnSmash()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.7f, false);
		this.smashDustFrontPrefab.Create(base.transform.position);
		this.smashDustBackPrefab.Create(base.transform.position);
	}

	// Token: 0x06002E3C RID: 11836 RVA: 0x001B396C File Offset: 0x001B1D6C
	private void OnBossDeath()
	{
		if (this.onDeath != null)
		{
			this.onDeath();
		}
		this.StopAllCoroutines();
		base.animator.SetTrigger("Death");
		AudioManager.Play("slime_tombstone_death");
	}

	// Token: 0x06002E3D RID: 11837 RVA: 0x001B39A4 File Offset: 0x001B1DA4
	private void TombstoneTauntsAudio()
	{
		AudioManager.Play("slime_tombstone_taunts");
	}

	// Token: 0x0400369D RID: 13981
	private const float startY = 550f;

	// Token: 0x0400369E RID: 13982
	private const float onGroundY = -80f;

	// Token: 0x0400369F RID: 13983
	private const float maxX = 500f;

	// Token: 0x040036A0 RID: 13984
	private const float fallTime = 0.2f;

	// Token: 0x040036A1 RID: 13985
	private const float crushSlimeY = 70f;

	// Token: 0x040036A2 RID: 13986
	private int offsetIndex;

	// Token: 0x040036A3 RID: 13987
	private bool dealDamage;

	// Token: 0x040036A4 RID: 13988
	private DamageDealer damageDealer;

	// Token: 0x040036A5 RID: 13989
	private DamageReceiver damageReceiver;

	// Token: 0x040036A6 RID: 13990
	[SerializeField]
	private Transform dirt;

	// Token: 0x040036A7 RID: 13991
	[SerializeField]
	private Transform dust;

	// Token: 0x040036A8 RID: 13992
	[SerializeField]
	private Transform dust2;

	// Token: 0x040036A9 RID: 13993
	[SerializeField]
	private Effect dustPrefab;

	// Token: 0x040036AA RID: 13994
	[SerializeField]
	private SlimeLevelSlime bigSlime;

	// Token: 0x040036AB RID: 13995
	[SerializeField]
	private SlimeLevelTinySlime tinySlime;

	// Token: 0x040036AC RID: 13996
	[SerializeField]
	private Effect smashDustBackPrefab;

	// Token: 0x040036AD RID: 13997
	[SerializeField]
	private Effect smashDustFrontPrefab;

	// Token: 0x040036AE RID: 13998
	private SlimeLevelTombstone.Direction direction;

	// Token: 0x040036AF RID: 13999
	public Action onDeath;

	// Token: 0x040036B0 RID: 14000
	private bool wantsToSmash;

	// Token: 0x020007E2 RID: 2018
	public enum State
	{
		// Token: 0x040036B2 RID: 14002
		Init,
		// Token: 0x040036B3 RID: 14003
		Intro,
		// Token: 0x040036B4 RID: 14004
		Move,
		// Token: 0x040036B5 RID: 14005
		Smash
	}

	// Token: 0x020007E3 RID: 2019
	private enum Direction
	{
		// Token: 0x040036B7 RID: 14007
		Left,
		// Token: 0x040036B8 RID: 14008
		Right
	}
}
