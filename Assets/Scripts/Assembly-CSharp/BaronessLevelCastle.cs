using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x020004DF RID: 1247
public class BaronessLevelCastle : LevelProperties.Baroness.Entity
{
	// Token: 0x1700031A RID: 794
	// (get) Token: 0x06001569 RID: 5481 RVA: 0x000BF6E6 File Offset: 0x000BDAE6
	// (set) Token: 0x0600156A RID: 5482 RVA: 0x000BF6ED File Offset: 0x000BDAED
	public static BaronessLevelMiniBossBase CURRENT_MINI_BOSS { get; private set; }

	// Token: 0x1700031B RID: 795
	// (get) Token: 0x0600156B RID: 5483 RVA: 0x000BF6F5 File Offset: 0x000BDAF5
	// (set) Token: 0x0600156C RID: 5484 RVA: 0x000BF6FD File Offset: 0x000BDAFD
	public BaronessLevelCastle.State state { get; private set; }

	// Token: 0x1700031C RID: 796
	// (get) Token: 0x0600156D RID: 5485 RVA: 0x000BF706 File Offset: 0x000BDB06
	// (set) Token: 0x0600156E RID: 5486 RVA: 0x000BF70E File Offset: 0x000BDB0E
	public BaronessLevelCastle.TeethState teethState { get; private set; }

	// Token: 0x1400003E RID: 62
	// (add) Token: 0x0600156F RID: 5487 RVA: 0x000BF718 File Offset: 0x000BDB18
	// (remove) Token: 0x06001570 RID: 5488 RVA: 0x000BF750 File Offset: 0x000BDB50
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06001571 RID: 5489 RVA: 0x000BF788 File Offset: 0x000BDB88
	protected override void Awake()
	{
		base.Awake();
		this.maxMiniBosses = false;
		this.continueTransition = false;
		this.originalEmergePos = this.emergePoint.position;
		this.originalBaronessPoint = this.baronessPhase1.transform.position;
		this.teethState = BaronessLevelCastle.TeethState.Unspawned;
		this.baronessPhase2.gameObject.SetActive(false);
		this.blink.enabled = false;
		this.blinkCounterMax = UnityEngine.Random.Range(4, 7);
		this.damageReceiver = this.baronessPhase2.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.damageDealer = new DamageDealer(1f, 1f);
	}

	// Token: 0x06001572 RID: 5490 RVA: 0x000BF840 File Offset: 0x000BDC40
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != BaronessLevelCastle.State.Dead)
		{
			this.state = BaronessLevelCastle.State.Dead;
			this.StartDeath();
		}
	}

	// Token: 0x06001573 RID: 5491 RVA: 0x000BF88C File Offset: 0x000BDC8C
	private void Update()
	{
		this.damageDealer.Update();
		this.player = PlayerManager.GetNext();
		this.distToGround = this.player.transform.position.y - -360f;
		if (this.state == BaronessLevelCastle.State.Idle)
		{
			if (BaronessLevelCastle.CURRENT_MINI_BOSS == null)
			{
				if (!this.maxMiniBosses)
				{
					this.jellyChangeDelay = true;
					this.StartOpen();
				}
				else if (Level.Current.mode != Level.Mode.Easy)
				{
					this.state = BaronessLevelCastle.State.ChaseIntro;
					this.StartChase();
				}
			}
			else if (Level.Current.mode == Level.Mode.Easy && this.state != BaronessLevelCastle.State.EasyFinal && BaronessLevelCastle.CURRENT_MINI_BOSS.isDying && this.maxMiniBosses)
			{
				this.state = BaronessLevelCastle.State.EasyFinal;
				base.StartCoroutine(this.shoot_easy_cr());
			}
		}
	}

	// Token: 0x06001574 RID: 5492 RVA: 0x000BF976 File Offset: 0x000BDD76
	public override void LevelInit(LevelProperties.Baroness properties)
	{
		base.LevelInit(properties);
		this.baronessPhase1.getProperties(properties, (float)properties.CurrentState.baronessVonBonbon.HP, this);
		this.platform.getProperties(properties.CurrentState.platform);
	}

	// Token: 0x06001575 RID: 5493 RVA: 0x000BF9B3 File Offset: 0x000BDDB3
	public void StartIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001576 RID: 5494 RVA: 0x000BF9C4 File Offset: 0x000BDDC4
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.6f);
		this.baronessPhase1.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.Play("Castle_Open");
		AudioManager.Play("level_baroness_castle_gate_open");
		yield return base.animator.WaitForAnimationToEnd(this, "Castle_Open", false, true);
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.state = BaronessLevelCastle.State.Idle;
		yield break;
	}

	// Token: 0x06001577 RID: 5495 RVA: 0x000BF9DF File Offset: 0x000BDDDF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001578 RID: 5496 RVA: 0x000BF9FD File Offset: 0x000BDDFD
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.peppermintPrefab = null;
		this.cupcakePrefab = null;
		this.wafflePrefab = null;
		this.gumballPrefab = null;
		this.jawBreakerPrefab = null;
		this.candyCornPrefab = null;
		this.greenJellyPrefab = null;
		this.pinkJellyPrefab = null;
	}

	// Token: 0x06001579 RID: 5497 RVA: 0x000BFA3D File Offset: 0x000BDE3D
	public void StartOpen()
	{
		this.state = BaronessLevelCastle.State.Open;
		base.StartCoroutine(this.open_cr());
	}

	// Token: 0x0600157A RID: 5498 RVA: 0x000BFA53 File Offset: 0x000BDE53
	private void SetEyes()
	{
		base.animator.SetBool("ToCastleLoop", true);
	}

	// Token: 0x0600157B RID: 5499 RVA: 0x000BFA68 File Offset: 0x000BDE68
	private IEnumerator open_cr()
	{
		LevelProperties.Baroness.Open p = base.properties.CurrentState.open;
		this.castleOpen = true;
		if (this.baronessPoppedUp)
		{
			this.baronessPoppedUp = false;
			yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this, "Baroness_Leave", false, true);
		}
		if (this.bossIndex != 0)
		{
			AudioManager.Play("level_baroness_castle_gate_open");
			base.animator.Play("Castle_Open");
			yield return base.animator.WaitForAnimationToEnd(this, "Castle_Open", false, true);
			this.baronessPhase1.animator.Play("Baroness_Mad_Start");
			AudioManager.Play("level_baroness_stick_head_pop");
			while (this.baronessPhase1.popUpCounter < 4)
			{
				yield return null;
			}
			this.baronessPhase1.animator.SetTrigger("PopIn");
			AudioManager.Play("level_baroness_stick_head_pop");
			this.baronessPhase1.popUpCounter = 0;
			yield return CupheadTime.WaitForSeconds(this, 1.5f);
		}
		switch ((BaronessLevelCastle.BossPossibility)Enum.Parse(typeof(BaronessLevelCastle.BossPossibility), BaronessLevel.PICKED_BOSSES[this.bossIndex]))
		{
		case BaronessLevelCastle.BossPossibility.Gumball:
			this.SpawnGumball();
			break;
		case BaronessLevelCastle.BossPossibility.Waffle:
			this.SpawnWaffle();
			break;
		case BaronessLevelCastle.BossPossibility.CandyCorn:
			this.SpawnCandyCorn();
			break;
		case BaronessLevelCastle.BossPossibility.Cupcake:
			this.SpawnCupcake();
			break;
		case BaronessLevelCastle.BossPossibility.Jawbreaker:
			this.SpawnJawbreaker();
			break;
		}
		yield return CupheadTime.WaitForSeconds(this, this.setWaitTime);
		base.animator.SetBool("ToCastleLoop", false);
		if (this.bossIndex < p.miniBossAmount)
		{
			this.bossIndex++;
		}
		if (base.properties.CurrentState.jellybeans.startingPoint == (float)this.bossIndex)
		{
			this.StartJellybeans();
		}
		if (this.bossIndex == p.miniBossAmount)
		{
			this.maxMiniBosses = true;
		}
		AudioManager.Play("level_baroness_castle_gate_close");
		yield return base.animator.WaitForAnimationToEnd(this, "Castle_Close", false, true);
		this.castleOpen = false;
		if (base.properties.CurrentState.baronessVonBonbon.miniBossStart == (float)this.bossIndex)
		{
			this.StartBaronessShoot();
		}
		this.state = BaronessLevelCastle.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600157C RID: 5500 RVA: 0x000BFA84 File Offset: 0x000BDE84
	private void Blink()
	{
		if (this.blinkCounter < this.blinkCounterMax)
		{
			this.blink.enabled = false;
			this.blinkCounter++;
		}
		else
		{
			this.blink.enabled = true;
			this.blinkCounter = 0;
			this.blinkCounterMax = UnityEngine.Random.Range(4, 7);
		}
	}

	// Token: 0x0600157D RID: 5501 RVA: 0x000BFAE4 File Offset: 0x000BDEE4
	private void SpawnGumball()
	{
		Vector3 position = this.emergePoint.position;
		position.y = this.emergePoint.position.y + 100f;
		this.emergePoint.position = position;
		BaronessLevelGumball baronessLevelGumball = UnityEngine.Object.Instantiate<BaronessLevelGumball>(this.gumballPrefab);
		LevelProperties.Baroness.Gumball gumball = base.properties.CurrentState.gumball;
		baronessLevelGumball.Init(gumball, this.emergePoint.position, (float)gumball.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelGumball;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Gumball;
		this.setWaitTime = 1f;
		this.emergePoint.position = this.originalEmergePos;
	}

	// Token: 0x0600157E RID: 5502 RVA: 0x000BFB94 File Offset: 0x000BDF94
	private void SpawnWaffle()
	{
		BaronessLevelWaffle baronessLevelWaffle = UnityEngine.Object.Instantiate<BaronessLevelWaffle>(this.wafflePrefab);
		LevelProperties.Baroness.Waffle waffle = base.properties.CurrentState.waffle;
		baronessLevelWaffle.Init(waffle, this.emergePoint.position, this.pivotPoint, waffle.movementSpeed, (float)waffle.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelWaffle;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Waffle;
		this.setWaitTime = 1f;
	}

	// Token: 0x0600157F RID: 5503 RVA: 0x000BFC04 File Offset: 0x000BE004
	private void SpawnCandyCorn()
	{
		BaronessLevelCandyCorn baronessLevelCandyCorn = UnityEngine.Object.Instantiate<BaronessLevelCandyCorn>(this.candyCornPrefab);
		LevelProperties.Baroness.CandyCorn candyCorn = base.properties.CurrentState.candyCorn;
		baronessLevelCandyCorn.Init(candyCorn, new Vector3(this.emergePoint.position.x, this.emergePoint.position.y + 40f), candyCorn.movementSpeed, (float)candyCorn.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelCandyCorn;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.CandyCorn;
		this.setWaitTime = 1f;
	}

	// Token: 0x06001580 RID: 5504 RVA: 0x000BFC94 File Offset: 0x000BE094
	private void SpawnCupcake()
	{
		Vector3 position = this.emergePoint.position;
		position.y = this.emergePoint.position.y;
		position.x = this.emergePoint.position.x + 200f;
		this.emergePoint.position = position;
		BaronessLevelCupcake baronessLevelCupcake = UnityEngine.Object.Instantiate<BaronessLevelCupcake>(this.cupcakePrefab);
		LevelProperties.Baroness.Cupcake cupcake = base.properties.CurrentState.cupcake;
		baronessLevelCupcake.Init(cupcake, this.emergePoint.position, (float)cupcake.HP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelCupcake;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Cupcake;
		this.setWaitTime = 1f;
		this.emergePoint.position = this.originalEmergePos;
	}

	// Token: 0x06001581 RID: 5505 RVA: 0x000BFD60 File Offset: 0x000BE160
	private void SpawnJawbreaker()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		BaronessLevelJawbreaker baronessLevelJawbreaker = UnityEngine.Object.Instantiate<BaronessLevelJawbreaker>(this.jawBreakerPrefab);
		LevelProperties.Baroness.Jawbreaker jawbreaker = base.properties.CurrentState.jawbreaker;
		baronessLevelJawbreaker.Init(jawbreaker, next, new Vector3(this.emergePoint.position.x, this.emergePoint.position.y + 10f), jawbreaker.jawbreakerHomingRotation, (float)jawbreaker.jawbreakerHomingHP);
		BaronessLevelCastle.CURRENT_MINI_BOSS = baronessLevelJawbreaker;
		BaronessLevelCastle.CURRENT_MINI_BOSS.bossId = BaronessLevelCastle.BossPossibility.Jawbreaker;
		this.setWaitTime = 3f;
		for (int i = 0; i < jawbreaker.jawbreakerMinis; i++)
		{
			this.setWaitTime += 0.5f;
		}
	}

	// Token: 0x06001582 RID: 5506 RVA: 0x000BFE25 File Offset: 0x000BE225
	public void StartBaronessShoot()
	{
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06001583 RID: 5507 RVA: 0x000BFE34 File Offset: 0x000BE234
	private IEnumerator shoot_cr()
	{
		this.state = BaronessLevelCastle.State.Idle;
		LevelProperties.Baroness.BaronessVonBonbon p = base.properties.CurrentState.baronessVonBonbon;
		string[] pattern = p.timeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.timeIndex = UnityEngine.Random.Range(0, pattern.Length);
		Collider2D collider = this.baronessPhase1.shootPoint.GetComponent<Collider2D>();
		for (;;)
		{
			float timeShoot;
			Parser.FloatTryParse(pattern[this.timeIndex], out timeShoot);
			yield return CupheadTime.WaitForSeconds(this, timeShoot);
			this.baronessPhase1.shotEnough = false;
			if (this.castleOpen)
			{
				yield return base.animator.WaitForAnimationToEnd(this, "Castle_Close", false, true);
			}
			this.baronessPoppedUp = true;
			AudioManager.Play("level_baroness_stick_head_open");
			this.baronessPhase1.animator.Play("Baroness_Pop_Up");
			while (this.baronessPoppedUp)
			{
				collider.enabled = true;
				if ((float)this.baronessPhase1.shootCounter >= p.attackCount.RandomFloat() || this.baronessPhase1.shotEnough)
				{
					break;
				}
				this.baronessPhase1.animator.SetTrigger("ToShoot");
				yield return CupheadTime.WaitForSeconds(this, p.attackDelay);
				yield return null;
			}
			this.baronessPoppedUp = false;
			collider.enabled = false;
			this.baronessPhase1.shootCounter = 0;
			AudioManager.Play("level_baroness_stick_head_closed");
			this.baronessPhase1.animator.SetTrigger("Leave");
			if (this.timeIndex < pattern.Length - 1)
			{
				this.timeIndex++;
			}
			else
			{
				this.timeIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001584 RID: 5508 RVA: 0x000BFE50 File Offset: 0x000BE250
	private IEnumerator shoot_easy_cr()
	{
		float t = 0f;
		this.baronessPhase1.isEasyFinal = true;
		LevelProperties.Baroness.BaronessVonBonbon p = base.properties.CurrentState.baronessVonBonbon;
		Collider2D collider = this.baronessPhase1.shootPoint.GetComponent<Collider2D>();
		this.baronessPoppedUp = true;
		AudioManager.Play("level_baroness_stick_head_open");
		this.baronessPhase1.animator.Play("Baroness_Pop_Up");
		collider.enabled = true;
		if (this.castleOpen)
		{
			yield return base.animator.WaitForAnimationToEnd(this, "Castle_Close", false, true);
		}
		while (this.baronessPhase1.isEasyFinal)
		{
			this.baronessPhase1.animator.SetTrigger("ToShoot");
			while (t < p.attackDelay && this.baronessPhase1.isEasyFinal)
			{
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
		}
		this.StartDeathEasy();
		yield break;
	}

	// Token: 0x06001585 RID: 5509 RVA: 0x000BFE6B File Offset: 0x000BE26B
	public void StartJellybeans()
	{
		base.StartCoroutine(this.spawnJellybeans_cr());
	}

	// Token: 0x06001586 RID: 5510 RVA: 0x000BFE7C File Offset: 0x000BE27C
	private void SpawnJellyBeans(BaronessLevelJellybeans prefab)
	{
		Vector3 position = this.emergePoint.position;
		this.emergePoint.position = position;
		position.y = this.emergePoint.position.y - 20f;
		LevelProperties.Baroness.Jellybeans jellybeans = base.properties.CurrentState.jellybeans;
		prefab.Create(base.properties.CurrentState.jellybeans, position, jellybeans.movementSpeed, (float)jellybeans.HP);
		this.emergePoint.position = this.originalEmergePos;
	}

	// Token: 0x06001587 RID: 5511 RVA: 0x000BFF08 File Offset: 0x000BE308
	private IEnumerator spawnJellybeans_cr()
	{
		LevelProperties.Baroness.Jellybeans p = base.properties.CurrentState.jellybeans;
		string[] typePattern = p.typeArray.GetRandom<string>().Split(new char[]
		{
			','
		});
		float change = 0f;
		while (this.state != BaronessLevelCastle.State.ChaseIntro)
		{
			for (int i = 0; i < typePattern.Length; i++)
			{
				BaronessLevelJellybeans toSpawn = null;
				float beanSpawnDelay = p.spawnDelay.RandomFloat();
				if (typePattern[i][0] == 'R')
				{
					toSpawn = this.greenJellyPrefab;
				}
				else if (typePattern[i][0] == 'P')
				{
					toSpawn = this.pinkJellyPrefab;
				}
				if ((BaronessLevelCastle.CURRENT_MINI_BOSS != null && this.state == BaronessLevelCastle.State.Idle) || this.state == BaronessLevelCastle.State.EasyFinal)
				{
					this.SpawnJellyBeans(toSpawn);
					yield return CupheadTime.WaitForSeconds(this, beanSpawnDelay - change);
				}
				else
				{
					yield return null;
				}
				if (this.jellyChangeDelay)
				{
					change += beanSpawnDelay - beanSpawnDelay * (1f - p.spawnDelayChangePercentage / 100f);
					this.jellyChangeDelay = false;
				}
			}
		}
		yield break;
	}

	// Token: 0x06001588 RID: 5512 RVA: 0x000BFF23 File Offset: 0x000BE323
	private void StartDeathEasy()
	{
		this.StopAllCoroutines();
		this.state = BaronessLevelCastle.State.Dead;
		base.StartCoroutine(this.death_easy_cr());
	}

	// Token: 0x06001589 RID: 5513 RVA: 0x000BFF40 File Offset: 0x000BE340
	private IEnumerator death_easy_cr()
	{
		float offset = 100f;
		float speed = 400f;
		if (!this.baronessPoppedUp)
		{
			Vector3 position = this.baronessPhase1.transform.position;
			position.y -= offset;
			this.baronessPhase1.transform.position = position;
		}
		this.baronessPhase1.animator.SetTrigger("Death");
		base.animator.SetTrigger("DeathEasy");
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		base.properties.WinInstantly();
		if (!this.baronessPoppedUp)
		{
			while (this.baronessPhase1.transform.position != this.originalBaronessPoint)
			{
				this.baronessPhase1.transform.position = Vector3.MoveTowards(this.baronessPhase1.transform.position, this.originalBaronessPoint, speed * CupheadTime.Delta);
				yield return null;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600158A RID: 5514 RVA: 0x000BFF5B File Offset: 0x000BE35B
	private void StartDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x0600158B RID: 5515 RVA: 0x000BFF70 File Offset: 0x000BE370
	private IEnumerator death_cr()
	{
		this.pauseScrolling = true;
		this.teethState = BaronessLevelCastle.TeethState.Off;
		base.animator.SetTrigger("Death");
		Vector3 pos = this.castleWallFix.transform.position;
		pos.y = -45f;
		this.castleWallFix.transform.position = pos;
		this.castleWallFix.sortingLayerName = "Background";
		this.castleWallFix.sortingOrder = 15;
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				if (this.scrollForce != null)
				{
					levelPlayerController.motor.RemoveForce(this.scrollForce);
				}
			}
		}
		this.blackCastleHole.gameObject.GetComponent<SpriteRenderer>().enabled = false;
		base.animator.Play("Castle_Death");
		yield return null;
		yield break;
	}

	// Token: 0x0600158C RID: 5516 RVA: 0x000BFF8B File Offset: 0x000BE38B
	public void StartChase()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.chase_intro_cr());
	}

	// Token: 0x0600158D RID: 5517 RVA: 0x000BFFA0 File Offset: 0x000BE3A0
	private IEnumerator chase_intro_cr()
	{
		this.baronessPhase1.transformCounter = 0;
		if (this.baronessPoppedUp)
		{
			this.baronessPhase1.animator.SetTrigger("Leave");
			this.baronessPhase1.animator.WaitForAnimationToEnd(this, "Baroness_Leave", false, true);
			this.baronessPoppedUp = false;
			yield return CupheadTime.WaitForSeconds(this, 1f);
		}
		this.baronessPhase2.gameObject.SetActive(true);
		this.baronessPhase1.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Background";
		this.baronessPhase1.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 6;
		this.baronessPhase1.animator.Play("Baroness_To_Idle_1");
		while (this.baronessPhase1.transformCounter <= 2)
		{
			yield return null;
		}
		this.baronessPhase1.animator.SetTrigger("Continue");
		yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this.baronessPhase1, "Baroness_Transition_1", false, true);
		this.baronessPhase1.transformCounter = 0;
		while (this.baronessPhase1.transformCounter <= 2 && this.continueTransition)
		{
			this.inAnimationLoop = true;
			yield return null;
		}
		this.baronessPhase1.animator.SetTrigger("Continue");
		yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this.baronessPhase1, "Baroness_Transition_2_Loop", false, true);
		this.baronessPhase1.animator.SetTrigger("OnCandyCaneExit");
		yield return this.baronessPhase1.animator.WaitForAnimationToEnd(this.baronessPhase1, "Baroness_Transition_3", false, true);
		base.animator.SetTrigger("StartPhase2");
		this.baronessPhase1.transformCounter = 0;
		AudioManager.Play("level_baroness_grab_castle");
		while (this.transitionCounter <= 4)
		{
			yield return null;
		}
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Castle_Transition_11", false, true);
		base.animator.SetTrigger("LoopArms");
		this.castleWallFix.sortingLayerName = "Default";
		this.baronessPhase2.gameObject.SetActive(true);
		this.castleCollidePhase2.SetActive(true);
		this.state = BaronessLevelCastle.State.Chase;
		base.StartCoroutine(this.handle_scroll_cr());
		base.StartCoroutine(this.peppermint_cr());
		base.StartCoroutine(this.final_shoot_cr());
		yield return null;
		yield break;
	}

	// Token: 0x0600158E RID: 5518 RVA: 0x000BFFBB File Offset: 0x000BE3BB
	private void PauseScroll()
	{
		this.pauseScrolling = !this.pauseScrolling;
		this.activateForce = !this.activateForce;
	}

	// Token: 0x0600158F RID: 5519 RVA: 0x000BFFDB File Offset: 0x000BE3DB
	private void ActivateTeeth()
	{
		base.animator.Play("Castle_Chase_Arms", 2);
		this.teethState = BaronessLevelCastle.TeethState.Idle;
	}

	// Token: 0x06001590 RID: 5520 RVA: 0x000BFFF5 File Offset: 0x000BE3F5
	private void HitCastleFrame()
	{
		if (this.inAnimationLoop)
		{
			this.continueTransition = true;
		}
	}

	// Token: 0x06001591 RID: 5521 RVA: 0x000C0009 File Offset: 0x000BE409
	private void TransitionCounter()
	{
		this.transitionCounter++;
	}

	// Token: 0x06001592 RID: 5522 RVA: 0x000C001C File Offset: 0x000BE41C
	private void SwitchLayersToDefault()
	{
		this.baronessPhase2.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		this.baronessPhase2.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 0;
		this.blackCastleHole.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		this.blackCastleHole.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 1;
		base.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		base.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 2;
		this.castlePhase2TopLayer.gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Default";
		this.castlePhase2TopLayer.gameObject.GetComponent<SpriteRenderer>().sortingOrder = 5;
	}

	// Token: 0x06001593 RID: 5523 RVA: 0x000C00E0 File Offset: 0x000BE4E0
	private IEnumerator handle_scroll_cr()
	{
		this.scrollForce = new LevelPlayerMotor.VelocityManager.Force(LevelPlayerMotor.VelocityManager.Force.Type.Ground, 190f);
		for (;;)
		{
			foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
			{
				LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
				if (!(levelPlayerController == null))
				{
					if (this.distToGround < 200f && this.activateForce)
					{
						levelPlayerController.motor.AddForce(this.scrollForce);
					}
					else
					{
						levelPlayerController.motor.RemoveForce(this.scrollForce);
					}
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001594 RID: 5524 RVA: 0x000C00FB File Offset: 0x000BE4FB
	private void HandsSound()
	{
		AudioManager.Play("level_baroness_castle_hands");
	}

	// Token: 0x06001595 RID: 5525 RVA: 0x000C0107 File Offset: 0x000BE507
	private void CastleRoar()
	{
		AudioManager.Play("level_baroness_castle_roar");
	}

	// Token: 0x06001596 RID: 5526 RVA: 0x000C0113 File Offset: 0x000BE513
	private void PointSound()
	{
		AudioManager.Play("level_baroness_go_castle");
	}

	// Token: 0x06001597 RID: 5527 RVA: 0x000C0120 File Offset: 0x000BE520
	private IEnumerator peppermint_cr()
	{
		for (;;)
		{
			float seconds = base.properties.CurrentState.peppermint.peppermintSpawnDurationRange.RandomFloat();
			yield return CupheadTime.WaitForSeconds(this, seconds);
			this.teethState = BaronessLevelCastle.TeethState.StartOpen;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001598 RID: 5528 RVA: 0x000C013B File Offset: 0x000BE53B
	private void OpenTeeth()
	{
		base.StartCoroutine(this.open_teeth_cr());
	}

	// Token: 0x06001599 RID: 5529 RVA: 0x000C014C File Offset: 0x000BE54C
	private IEnumerator open_teeth_cr()
	{
		if (this.teethState == BaronessLevelCastle.TeethState.StartOpen)
		{
			this.teethState = BaronessLevelCastle.TeethState.Open;
			base.animator.SetBool("TeethOpen", true);
			yield return CupheadTime.WaitForSeconds(this, 1f);
			BaronessLevelPeppermint peppermint = UnityEngine.Object.Instantiate<BaronessLevelPeppermint>(this.peppermintPrefab);
			LevelProperties.Baroness.Peppermint p = base.properties.CurrentState.peppermint;
			peppermint.Init(this.emergePoint.position, p.peppermintSpeed);
			yield return CupheadTime.WaitForSeconds(this, 0.5f);
			this.teethState = BaronessLevelCastle.TeethState.Off;
		}
		yield break;
	}

	// Token: 0x0600159A RID: 5530 RVA: 0x000C0167 File Offset: 0x000BE567
	private void CloseTeeth()
	{
		if (this.teethState == BaronessLevelCastle.TeethState.Off)
		{
			base.animator.SetBool("TeethOpen", false);
			this.teethState = BaronessLevelCastle.TeethState.Idle;
		}
	}

	// Token: 0x0600159B RID: 5531 RVA: 0x000C018D File Offset: 0x000BE58D
	private void HideTeeth()
	{
		this.teeth.enabled = false;
	}

	// Token: 0x0600159C RID: 5532 RVA: 0x000C019B File Offset: 0x000BE59B
	private void ShowTeeth()
	{
		this.teeth.enabled = true;
	}

	// Token: 0x0600159D RID: 5533 RVA: 0x000C01AC File Offset: 0x000BE5AC
	private IEnumerator final_shoot_cr()
	{
		LevelProperties.Baroness.BaronessVonBonbon p = base.properties.CurrentState.baronessVonBonbon;
		string[] headString = p.finalProjectileHeadToss.Split(new char[]
		{
			','
		});
		int headIndex = UnityEngine.Random.Range(0, headString.Length);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.baronessVonBonbon.finalProjectileInitialDelay);
		for (;;)
		{
			if (headString[headIndex][0] == 'H')
			{
				base.animator.SetBool("Toss", true);
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.baronessVonBonbon.finalProjectileAttackDelayRange.RandomFloat());
			}
			else
			{
				string[] delayString = headString[headIndex].Split(new char[]
				{
					':'
				});
				yield return CupheadTime.WaitForSeconds(this, Parser.FloatParse(delayString[1]));
			}
			headIndex = (headIndex + 1) % headString.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600159E RID: 5534 RVA: 0x000C01C7 File Offset: 0x000BE5C7
	private void FireHead()
	{
		this.baronessPhase1.FireFinalProjectile();
		base.animator.SetBool("Toss", false);
	}

	// Token: 0x04001EBD RID: 7869
	public bool pauseScrolling;

	// Token: 0x04001EC1 RID: 7873
	private Transform[] homingRoots;

	// Token: 0x04001EC2 RID: 7874
	[SerializeField]
	private SpriteRenderer teeth;

	// Token: 0x04001EC3 RID: 7875
	[SerializeField]
	private SpriteRenderer blink;

	// Token: 0x04001EC4 RID: 7876
	[SerializeField]
	private BaronessLevelPlatform platform;

	// Token: 0x04001EC5 RID: 7877
	[SerializeField]
	private BaronessLevelBaroness baronessPhase1;

	// Token: 0x04001EC6 RID: 7878
	[SerializeField]
	private Transform baronessPhase2;

	// Token: 0x04001EC7 RID: 7879
	[SerializeField]
	private BaronessLevelPeppermint peppermintPrefab;

	// Token: 0x04001EC8 RID: 7880
	[SerializeField]
	private Transform blackCastleHole;

	// Token: 0x04001EC9 RID: 7881
	[SerializeField]
	private Transform castlePhase2TopLayer;

	// Token: 0x04001ECA RID: 7882
	[SerializeField]
	private BaronessLevelCupcake cupcakePrefab;

	// Token: 0x04001ECB RID: 7883
	[SerializeField]
	private BaronessLevelWaffle wafflePrefab;

	// Token: 0x04001ECC RID: 7884
	[SerializeField]
	private BaronessLevelGumball gumballPrefab;

	// Token: 0x04001ECD RID: 7885
	[SerializeField]
	private BaronessLevelJawbreaker jawBreakerPrefab;

	// Token: 0x04001ECE RID: 7886
	[SerializeField]
	private BaronessLevelCandyCorn candyCornPrefab;

	// Token: 0x04001ECF RID: 7887
	[SerializeField]
	private BaronessLevelJellybeans greenJellyPrefab;

	// Token: 0x04001ED0 RID: 7888
	[SerializeField]
	private BaronessLevelJellybeans pinkJellyPrefab;

	// Token: 0x04001ED1 RID: 7889
	[SerializeField]
	private Transform emergePoint;

	// Token: 0x04001ED2 RID: 7890
	[SerializeField]
	private Transform pivotPoint;

	// Token: 0x04001ED3 RID: 7891
	[SerializeField]
	private GameObject castleCollidePhase2;

	// Token: 0x04001ED4 RID: 7892
	[SerializeField]
	private SpriteRenderer castleWallFix;

	// Token: 0x04001ED5 RID: 7893
	private int bossIndex;

	// Token: 0x04001ED6 RID: 7894
	private int timeIndex;

	// Token: 0x04001ED7 RID: 7895
	private int transitionCounter;

	// Token: 0x04001ED8 RID: 7896
	private int blinkCounter;

	// Token: 0x04001ED9 RID: 7897
	private int blinkCounterMax;

	// Token: 0x04001EDA RID: 7898
	private float setWaitTime;

	// Token: 0x04001EDB RID: 7899
	private float distToGround;

	// Token: 0x04001EDC RID: 7900
	private bool maxMiniBosses;

	// Token: 0x04001EDD RID: 7901
	private bool castleOpen;

	// Token: 0x04001EDE RID: 7902
	private bool baronessPoppedUp;

	// Token: 0x04001EDF RID: 7903
	private bool continueTransition;

	// Token: 0x04001EE0 RID: 7904
	private bool inAnimationLoop;

	// Token: 0x04001EE1 RID: 7905
	private bool jellyChangeDelay;

	// Token: 0x04001EE2 RID: 7906
	private bool openTeeth;

	// Token: 0x04001EE3 RID: 7907
	private bool activateForce = true;

	// Token: 0x04001EE4 RID: 7908
	private Vector3 originalEmergePos;

	// Token: 0x04001EE5 RID: 7909
	private Vector3 originalBaronessPoint;

	// Token: 0x04001EE6 RID: 7910
	private AbstractPlayerController player;

	// Token: 0x04001EE7 RID: 7911
	private LevelPlayerMotor.VelocityManager.Force scrollForce;

	// Token: 0x04001EE8 RID: 7912
	private DamageReceiver damageReceiver;

	// Token: 0x04001EE9 RID: 7913
	private DamageDealer damageDealer;

	// Token: 0x020004E0 RID: 1248
	public enum State
	{
		// Token: 0x04001EEB RID: 7915
		Intro,
		// Token: 0x04001EEC RID: 7916
		Idle,
		// Token: 0x04001EED RID: 7917
		ChaseIntro,
		// Token: 0x04001EEE RID: 7918
		Chase,
		// Token: 0x04001EEF RID: 7919
		Open,
		// Token: 0x04001EF0 RID: 7920
		Dead,
		// Token: 0x04001EF1 RID: 7921
		EasyFinal
	}

	// Token: 0x020004E1 RID: 1249
	public enum TeethState
	{
		// Token: 0x04001EF3 RID: 7923
		Unspawned,
		// Token: 0x04001EF4 RID: 7924
		Idle,
		// Token: 0x04001EF5 RID: 7925
		Off,
		// Token: 0x04001EF6 RID: 7926
		StartOpen,
		// Token: 0x04001EF7 RID: 7927
		Open
	}

	// Token: 0x020004E2 RID: 1250
	public enum BossPossibility
	{
		// Token: 0x04001EF9 RID: 7929
		Gumball = 1,
		// Token: 0x04001EFA RID: 7930
		Waffle,
		// Token: 0x04001EFB RID: 7931
		CandyCorn,
		// Token: 0x04001EFC RID: 7932
		Cupcake,
		// Token: 0x04001EFD RID: 7933
		Jawbreaker
	}
}
