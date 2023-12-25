using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020007A4 RID: 1956
public class SallyStagePlayLevelAngel : LevelProperties.SallyStagePlay.Entity
{
	// Token: 0x17000403 RID: 1027
	// (get) Token: 0x06002BCD RID: 11213 RVA: 0x001992C5 File Offset: 0x001976C5
	// (set) Token: 0x06002BCE RID: 11214 RVA: 0x001992CD File Offset: 0x001976CD
	public SallyStagePlayLevelAngel.State state { get; private set; }

	// Token: 0x06002BCF RID: 11215 RVA: 0x001992D8 File Offset: 0x001976D8
	protected override void Awake()
	{
		base.Awake();
		this.signStart = this.sign.transform.position;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x06002BD0 RID: 11216 RVA: 0x0019933C File Offset: 0x0019773C
	public override void LevelInit(LevelProperties.SallyStagePlay properties)
	{
		base.LevelInit(properties);
		LevelProperties.SallyStagePlay.Lightning lightning = properties.CurrentState.lightning;
		SallyStagePlayLevelAngel.extraHP = properties.CurrentState.husband.deityHP;
		this.lightningMax = UnityEngine.Random.Range((int)lightning.lightningDelayRange.min, (int)lightning.lightningDelayRange.max);
		this.lightningShotIndex = UnityEngine.Random.Range(0, lightning.lightningShotCount.Split(new char[]
		{
			','
		}).Length);
		this.lightningAngleIndex = UnityEngine.Random.Range(0, lightning.lightningAngleString.Split(new char[]
		{
			','
		}).Length);
		this.lightningSpawnIndex = UnityEngine.Random.Range(0, lightning.lightningSpawnString.Split(new char[]
		{
			','
		}).Length);
		this.meteorSpawnIndex = UnityEngine.Random.Range(0, properties.CurrentState.meteor.meteorSpawnString.Split(new char[]
		{
			','
		}).Length);
		this.meteors = new List<SallyStagePlayLevelMeteor>();
		if (Level.Current.mode == Level.Mode.Easy)
		{
			Level.Current.OnWinEvent += this.OnEasyDeath;
		}
		else
		{
			Level.Current.OnWinEvent += this.OnDeath;
		}
	}

	// Token: 0x06002BD1 RID: 11217 RVA: 0x0019947C File Offset: 0x0019787C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.killedHusband && SallyStagePlayLevelAngel.extraHP > 0f)
		{
			SallyStagePlayLevelAngel.extraHP -= info.damage;
		}
		else
		{
			base.properties.DealDamage(info.damage);
		}
	}

	// Token: 0x06002BD2 RID: 11218 RVA: 0x001994CA File Offset: 0x001978CA
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002BD3 RID: 11219 RVA: 0x001994E2 File Offset: 0x001978E2
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002BD4 RID: 11220 RVA: 0x00199500 File Offset: 0x00197900
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.meteorPrefab = null;
		this.lightningPrefab = null;
		this.umbrellaPrefab = null;
	}

	// Token: 0x06002BD5 RID: 11221 RVA: 0x0019951D File Offset: 0x0019791D
	public void StartPhase3(bool killedHusband)
	{
		this.killedHusband = killedHusband;
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06002BD6 RID: 11222 RVA: 0x00199534 File Offset: 0x00197934
	private IEnumerator intro_cr()
	{
		float t = 0f;
		float time = 3f;
		Vector3 endPos = new Vector3(base.transform.position.x, this.phase3Root.position.y);
		Vector2 start = base.transform.position;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.sally_angel_intro_sound_cr());
		if (this.killedHusband)
		{
			base.StartCoroutine(this.spawn_husband_cr());
		}
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = endPos;
		this.nextAttack = 1;
		base.StartCoroutine(this.sign_slide_cr());
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.StartCoroutine(this.main_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002BD7 RID: 11223 RVA: 0x00199550 File Offset: 0x00197950
	private IEnumerator sally_angel_intro_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 4f);
		AudioManager.Play("sally_vox_maniacal");
		this.emitAudioFromObject.Add("sally_vox_maniacal");
		yield break;
	}

	// Token: 0x06002BD8 RID: 11224 RVA: 0x0019956C File Offset: 0x0019796C
	private IEnumerator spawn_husband_cr()
	{
		this.husband.gameObject.SetActive(true);
		this.husband.GetComponent<Collider2D>().enabled = true;
		float t = 0f;
		float time = 3.5f;
		Vector3 endPos = new Vector3(this.phase3Root.transform.position.x, this.husband.transform.position.y);
		Vector2 start = this.husband.transform.position;
		bool soundTriggered = false;
		while (t < time)
		{
			if (t / time >= 0.3f && !soundTriggered)
			{
				AudioManager.Play("sally_fiance_enter");
				this.emitAudioFromObject.Add("sally_fiance_enter");
				soundTriggered = true;
			}
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutBounce, 0f, 1f, t / time);
			this.husband.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.husband.Attack();
		yield return null;
		yield break;
	}

	// Token: 0x06002BD9 RID: 11225 RVA: 0x00199588 File Offset: 0x00197988
	private IEnumerator sign_slide_cr()
	{
		string attackName = string.Empty;
		int num = this.nextAttack;
		if (num != 0)
		{
			if (num != 1)
			{
				if (num == 2)
				{
					attackName = "Wave";
				}
			}
			else
			{
				attackName = "Meteor";
			}
		}
		else
		{
			attackName = "Lightning";
		}
		this.sign.Play(attackName);
		float t = 0f;
		float time = 0.1f;
		Vector3 start = this.sign.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.sign.transform.position = Vector3.Lerp(start, new Vector3(this.signStart.x, this.signStart.y - 100f), t / time);
			yield return null;
		}
		t = 0f;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		start = this.sign.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.sign.transform.position = Vector3.Lerp(start, this.signStart, t / time);
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002BDA RID: 11226 RVA: 0x001995A4 File Offset: 0x001979A4
	private IEnumerator slide_out_cr()
	{
		float t = 0f;
		float time = 0.1f;
		Vector3 start = this.sign.transform.position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			this.sign.transform.position = Vector3.Lerp(start, this.signStart, t / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BDB RID: 11227 RVA: 0x001995C0 File Offset: 0x001979C0
	private IEnumerator main_cr()
	{
		LevelProperties.SallyStagePlay.General p = base.properties.CurrentState.general;
		string[] main = p.attackString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int mainIndex = UnityEngine.Random.Range(0, main.Length);
		this.nextAttack = mainIndex;
		while (main[mainIndex] != "M")
		{
			mainIndex = (mainIndex + 1) % main.Length;
			yield return null;
		}
		for (;;)
		{
			while (this.state != SallyStagePlayLevelAngel.State.Idle)
			{
				yield return null;
			}
			base.animator.SetBool("OnPh3Attack", true);
			base.StartCoroutine(this.sign_slide_cr());
			yield return base.animator.WaitForAnimationToStart(this, "Phase3_Attack_Start", false);
			string text = main[mainIndex];
			if (text != null)
			{
				if (!(text == "L"))
				{
					if (!(text == "M"))
					{
						if (text == "T")
						{
							base.StartCoroutine(this.tidal_wave_cr());
						}
					}
					else
					{
						base.StartCoroutine(this.meteor_cr());
					}
				}
				else
				{
					base.StartCoroutine(this.lightning_cr());
				}
			}
			mainIndex = (mainIndex + 1) % main.Length;
			this.GetNextAttack(main[mainIndex]);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BDC RID: 11228 RVA: 0x001995DC File Offset: 0x001979DC
	private void GetNextAttack(string main)
	{
		if (main != null)
		{
			if (!(main == "L"))
			{
				if (!(main == "M"))
				{
					if (main == "T")
					{
						this.nextAttack = 2;
					}
				}
				else
				{
					this.nextAttack = 1;
				}
			}
			else
			{
				this.nextAttack = 0;
			}
		}
	}

	// Token: 0x06002BDD RID: 11229 RVA: 0x00199648 File Offset: 0x00197A48
	private IEnumerator lightning_cr()
	{
		this.state = SallyStagePlayLevelAngel.State.Lightning;
		LevelProperties.SallyStagePlay.Lightning p = base.properties.CurrentState.lightning;
		string[] shotString = p.lightningShotCount.Split(new char[]
		{
			','
		});
		string[] angleString = p.lightningAngleString.Split(new char[]
		{
			','
		});
		string[] spawnString = p.lightningSpawnString.Split(new char[]
		{
			','
		});
		float angle = 0f;
		float spawn = 0f;
		float rotation = 0f;
		int shotCount = 0;
		Parser.IntTryParse(shotString[this.lightningShotIndex], out shotCount);
		for (int i = 0; i < shotCount; i++)
		{
			Parser.FloatTryParse(spawnString[this.lightningSpawnIndex], out spawn);
			bool aimAtPlayer;
			if (this.lightningMaxCounter >= this.lightningMax)
			{
				aimAtPlayer = true;
				this.lightningMaxCounter = 0;
			}
			else
			{
				aimAtPlayer = false;
				if (this.lightningMaxCounter == 0)
				{
					this.lightningMax = UnityEngine.Random.Range((int)p.lightningDirectAimRange.min, (int)p.lightningDirectAimRange.max);
				}
				Parser.FloatTryParse(angleString[this.lightningAngleIndex], out angle);
				this.lightningAngleIndex = (this.lightningAngleIndex + 1) % angleString.Length;
				this.lightningMaxCounter++;
			}
			Vector3 pos = new Vector3(-640f + spawn, 460f);
			if (aimAtPlayer)
			{
				AbstractPlayerController next = PlayerManager.GetNext();
				Vector3 v = next.transform.position - pos;
				rotation = MathUtils.DirectionToAngle(v);
			}
			else
			{
				rotation = angle;
			}
			this.lightningPrefab.Create(pos, rotation, p.lightningSpeed, i == shotCount - 1);
			this.lightningSpawnIndex = (this.lightningSpawnIndex + 1) % spawnString.Length;
			yield return CupheadTime.WaitForSeconds(this, p.lightningDelayRange.RandomFloat());
		}
		base.animator.SetBool("OnPh3Attack", false);
		this.lightningShotIndex = (this.lightningShotIndex + 1) % shotString.Length;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.general.attackDelayRange.RandomFloat());
		this.state = SallyStagePlayLevelAngel.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002BDE RID: 11230 RVA: 0x00199664 File Offset: 0x00197A64
	private IEnumerator meteor_cr()
	{
		this.state = SallyStagePlayLevelAngel.State.Meteor;
		LevelProperties.SallyStagePlay.Meteor p = base.properties.CurrentState.meteor;
		string[] meteorSpawnString = p.meteorSpawnString.Split(new char[]
		{
			','
		});
		int index = 0;
		float spawn = 0f;
		Parser.FloatTryParse(meteorSpawnString[this.meteorSpawnIndex], out spawn);
		bool lockedPosition = false;
		for (int i = 0; i < this.meteors.Count; i++)
		{
			if (this.meteors[i].state == SallyStagePlayLevelMeteor.State.Leaving)
			{
				this.meteors.Remove(this.meteors[i]);
				i++;
			}
		}
		yield return null;
		for (int j = 0; j < this.meteors.Count; j++)
		{
			if (spawn == this.meteors[j].spawnPosition)
			{
				index = j;
				lockedPosition = true;
				break;
			}
		}
		bool positionTaken = false;
		int meteorCounter = 0;
		int spawnStringCounter = 0;
		while (lockedPosition)
		{
			while (meteorCounter < this.meteors.Count)
			{
				meteorCounter++;
				if (spawn == this.meteors[index].spawnPosition)
				{
					positionTaken = true;
				}
				index = (index + 1) % this.meteors.Count;
			}
			if (!positionTaken)
			{
				lockedPosition = false;
				break;
			}
			this.meteorSpawnIndex = (this.meteorSpawnIndex + 1) % meteorSpawnString.Length;
			Parser.FloatTryParse(meteorSpawnString[this.meteorSpawnIndex], out spawn);
			spawnStringCounter++;
			if (spawnStringCounter >= meteorSpawnString.Length)
			{
				break;
			}
			meteorCounter = 0;
			positionTaken = false;
			yield return null;
		}
		if (this.meteors.Count <= 0)
		{
			lockedPosition = false;
		}
		if (!lockedPosition)
		{
			this.meteors.Add(this.meteorPrefab.Create(spawn, (float)p.meteorHP, p));
			this.meteorSpawnIndex = (this.meteorSpawnIndex + 1) % meteorSpawnString.Length;
		}
		base.animator.SetBool("OnPh3Attack", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.general.attackDelayRange.RandomFloat());
		this.state = SallyStagePlayLevelAngel.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002BDF RID: 11231 RVA: 0x00199680 File Offset: 0x00197A80
	private IEnumerator tidal_wave_cr()
	{
		this.state = SallyStagePlayLevelAngel.State.Wave;
		LevelProperties.SallyStagePlay.Tidal p = base.properties.CurrentState.tidal;
		this.wave.StartWave(p);
		while (this.wave.isMoving)
		{
			yield return null;
		}
		base.animator.SetBool("OnPh3Attack", false);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.tidal.tidalHesitate);
		yield return base.animator.WaitForAnimationToEnd(this, "Phase3_Attack", false, false);
		this.state = SallyStagePlayLevelAngel.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002BE0 RID: 11232 RVA: 0x0019969C File Offset: 0x00197A9C
	public void OnPhase4()
	{
		AudioManager.Stop("sally_sally_lightning_move_loop");
		this.StopAllCoroutines();
		base.StartCoroutine(this.slide_out_cr());
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		base.animator.SetTrigger("OnPh3Death");
		base.StartCoroutine(this.start_phase_4_cr());
	}

	// Token: 0x06002BE1 RID: 11233 RVA: 0x001996F0 File Offset: 0x00197AF0
	private IEnumerator start_phase_4_cr()
	{
		base.GetComponent<SpriteRenderer>().material = this.phase4Material;
		for (int i = 0; i < this.meteors.Count; i++)
		{
			if (this.meteors[i] != null)
			{
				this.meteors[i].MeteorChangePhase();
			}
		}
		float t = 0f;
		float time = 2.5f;
		Vector3 endPos = new Vector3(base.transform.position.x, 860f);
		Vector2 start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		if (this.killedHusband)
		{
			this.husband.Dead();
			base.StartCoroutine(this.husband.move_cr());
		}
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		foreach (GameObject gameObject in this.shadows)
		{
			gameObject.SetActive(false);
		}
		base.animator.Play("Phase4_Idle");
		yield return CupheadTime.WaitForSeconds(this, 1f);
		t = 0f;
		time = 1f;
		Vector3 pos = base.transform.position;
		pos.x = -640f + base.transform.GetComponent<Renderer>().bounds.size.x / 2f;
		base.transform.position = pos;
		endPos = new Vector3(base.transform.position.x, this.phase4Root.position.y);
		start = base.transform.position;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, endPos, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.spawn_roses_cr());
		this.SpawnUmbrella();
		yield return null;
		yield break;
	}

	// Token: 0x06002BE2 RID: 11234 RVA: 0x0019970C File Offset: 0x00197B0C
	private void SpawnUmbrella()
	{
		SallyStagePlayLevelUmbrella sallyStagePlayLevelUmbrella = UnityEngine.Object.Instantiate<SallyStagePlayLevelUmbrella>(this.umbrellaPrefab);
		sallyStagePlayLevelUmbrella.GetProperties(base.properties);
		sallyStagePlayLevelUmbrella.EnableHoming = false;
		float x = (!Rand.Bool()) ? 140f : -140f;
		sallyStagePlayLevelUmbrella.transform.position = new Vector2(x, 460f);
		base.StartCoroutine(this.umbrella_cr(sallyStagePlayLevelUmbrella));
	}

	// Token: 0x06002BE3 RID: 11235 RVA: 0x0019977C File Offset: 0x00197B7C
	private IEnumerator umbrella_cr(SallyStagePlayLevelUmbrella umbrella)
	{
		for (;;)
		{
			umbrella.TrackingPlayer = PlayerManager.GetNext();
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.umbrella.homingUntilSwitchPlayer);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BE4 RID: 11236 RVA: 0x001997A0 File Offset: 0x00197BA0
	private IEnumerator move_cr()
	{
		float t = 0f;
		float time = base.properties.CurrentState.general.finalMovementSpeed;
		float sizeX = base.transform.GetComponent<Renderer>().bounds.size.x / 2f;
		EaseUtils.EaseType ease = EaseUtils.EaseType.easeInOutSine;
		float start = -640f + sizeX;
		float end = 640f - sizeX;
		for (;;)
		{
			t = 0f;
			while (t < time)
			{
				float val = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(end), null, null);
			t = 0f;
			while (t < time)
			{
				float val2 = t / time;
				base.transform.SetPosition(new float?(EaseUtils.Ease(ease, end, start, val2)), null, null);
				t += CupheadTime.Delta;
				yield return null;
			}
			base.transform.SetPosition(new float?(start), null, null);
		}
		yield break;
	}

	// Token: 0x06002BE5 RID: 11237 RVA: 0x001997BC File Offset: 0x00197BBC
	private IEnumerator spawn_roses_cr()
	{
		LevelProperties.SallyStagePlay.Roses p = base.properties.CurrentState.roses;
		string[] roseString = p.spawnString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int roseIndex = UnityEngine.Random.Range(0, roseString.Length);
		float yCoord = 460f;
		float xCoord = 0f;
		int maxCount = p.playerAimRange.RandomInt();
		int counter = 0;
		for (;;)
		{
			if (counter < maxCount)
			{
				Parser.FloatTryParse(roseString[roseIndex], out xCoord);
				counter++;
			}
			else
			{
				AbstractPlayerController next = PlayerManager.GetNext();
				xCoord = next.transform.position.x;
				counter = 0;
				maxCount = p.playerAimRange.RandomInt();
			}
			Vector3 position = new Vector3(-640f + xCoord, yCoord);
			this.applauseHandler.ThrowRose(position, p);
			roseIndex = (roseIndex + 1) % roseString.Length;
			yield return CupheadTime.WaitForSeconds(this, p.spawnDelayRange.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002BE6 RID: 11238 RVA: 0x001997D8 File Offset: 0x00197BD8
	private void OnEasyDeath()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		if (this.killedHusband)
		{
			this.husband.GetComponent<Animator>().SetTrigger("OnDeath");
		}
		base.animator.SetTrigger("OnPh3Death");
	}

	// Token: 0x06002BE7 RID: 11239 RVA: 0x00199827 File Offset: 0x00197C27
	private void OnDeath()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.sally_angel_death_sound_cr());
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnPh4Death");
		base.StartCoroutine(this.birds_death_cr());
	}

	// Token: 0x06002BE8 RID: 11240 RVA: 0x00199868 File Offset: 0x00197C68
	private IEnumerator sally_angel_death_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		AudioManager.Play("sally_p4_angel_death_vox");
		yield break;
	}

	// Token: 0x06002BE9 RID: 11241 RVA: 0x00199884 File Offset: 0x00197C84
	private IEnumerator birds_death_cr()
	{
		float t = 0f;
		float time = 2f;
		Vector3 pos = this.birdsDeath.transform.position;
		this.birdsDeath.SetActive(true);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			pos.y = Mathf.Lerp(pos.y, this.birdRoot.transform.position.y, val);
			this.birdsDeath.transform.position = pos;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002BEA RID: 11242 RVA: 0x0019989F File Offset: 0x00197C9F
	private void SoundAngelIdle()
	{
		AudioManager.Play("sally_angel_idle");
		this.emitAudioFromObject.Add("sally_angel_idle");
	}

	// Token: 0x06002BEB RID: 11243 RVA: 0x001998BB File Offset: 0x00197CBB
	private void SoundAngelDeath()
	{
		AudioManager.Play("sally_angel_death");
		this.emitAudioFromObject.Add("sally_angel_death");
	}

	// Token: 0x04003478 RID: 13432
	public static float extraHP;

	// Token: 0x04003479 RID: 13433
	[SerializeField]
	private Material phase4Material;

	// Token: 0x0400347A RID: 13434
	[SerializeField]
	private SallyStagePlayApplauseHandler applauseHandler;

	// Token: 0x0400347B RID: 13435
	[SerializeField]
	private Animator sign;

	// Token: 0x0400347C RID: 13436
	[SerializeField]
	private SallyStagePlayLevelWave wave;

	// Token: 0x0400347D RID: 13437
	[SerializeField]
	private SallyStagePlayLevelMeteor meteorPrefab;

	// Token: 0x0400347E RID: 13438
	[SerializeField]
	private SallyStagePlayLevelLightning lightningPrefab;

	// Token: 0x0400347F RID: 13439
	[SerializeField]
	private SallyStagePlayLevelUmbrella umbrellaPrefab;

	// Token: 0x04003480 RID: 13440
	[SerializeField]
	private GameObject birdsDeath;

	// Token: 0x04003481 RID: 13441
	[SerializeField]
	private SallyStagePlayLevelFianceDeity husband;

	// Token: 0x04003482 RID: 13442
	[SerializeField]
	private GameObject[] shadows;

	// Token: 0x04003483 RID: 13443
	[Space(10f)]
	[SerializeField]
	private Transform phase4Root;

	// Token: 0x04003484 RID: 13444
	[SerializeField]
	private Transform birdRoot;

	// Token: 0x04003485 RID: 13445
	[SerializeField]
	private Transform phase3Root;

	// Token: 0x04003487 RID: 13447
	private List<SallyStagePlayLevelMeteor> meteors;

	// Token: 0x04003488 RID: 13448
	private DamageDealer damageDealer;

	// Token: 0x04003489 RID: 13449
	private DamageReceiver damageReceiver;

	// Token: 0x0400348A RID: 13450
	private Vector3 signStart;

	// Token: 0x0400348B RID: 13451
	private bool killedHusband;

	// Token: 0x0400348C RID: 13452
	private int nextAttack;

	// Token: 0x0400348D RID: 13453
	private int lightningShotIndex;

	// Token: 0x0400348E RID: 13454
	private int lightningAngleIndex;

	// Token: 0x0400348F RID: 13455
	private int lightningSpawnIndex;

	// Token: 0x04003490 RID: 13456
	private int lightningMax;

	// Token: 0x04003491 RID: 13457
	private int lightningMaxCounter;

	// Token: 0x04003492 RID: 13458
	private int meteorSpawnIndex;

	// Token: 0x020007A5 RID: 1957
	public enum State
	{
		// Token: 0x04003494 RID: 13460
		Idle,
		// Token: 0x04003495 RID: 13461
		Lightning,
		// Token: 0x04003496 RID: 13462
		Wave,
		// Token: 0x04003497 RID: 13463
		Meteor
	}
}
