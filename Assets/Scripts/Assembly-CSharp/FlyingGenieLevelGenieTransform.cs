using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200066B RID: 1643
public class FlyingGenieLevelGenieTransform : LevelProperties.FlyingGenie.Entity
{
	// Token: 0x17000394 RID: 916
	// (get) Token: 0x0600226A RID: 8810 RVA: 0x001422AB File Offset: 0x001406AB
	// (set) Token: 0x0600226B RID: 8811 RVA: 0x001422B3 File Offset: 0x001406B3
	public FlyingGenieLevelGenieTransform.State state { get; private set; }

	// Token: 0x17000395 RID: 917
	// (get) Token: 0x0600226C RID: 8812 RVA: 0x001422BC File Offset: 0x001406BC
	// (set) Token: 0x0600226D RID: 8813 RVA: 0x001422C4 File Offset: 0x001406C4
	public bool skipMarionette { get; private set; }

	// Token: 0x0600226E RID: 8814 RVA: 0x001422D0 File Offset: 0x001406D0
	protected override void Awake()
	{
		base.Awake();
		this.skipMarionette = false;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0600226F RID: 8815 RVA: 0x00142324 File Offset: 0x00140724
	public override void LevelInit(LevelProperties.FlyingGenie properties)
	{
		base.LevelInit(properties);
		this.state = FlyingGenieLevelGenieTransform.State.Intro;
		this.pyramids = new List<FlyingGenieLevelPyramid>();
	}

	// Token: 0x06002270 RID: 8816 RVA: 0x00142340 File Offset: 0x00140740
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		if (this.skipMarionette && this.state == FlyingGenieLevelGenieTransform.State.Giant && this.transitionHP > 0f)
		{
			float num = this.transitionHP;
			this.transitionHP -= info.damage;
			Level.Current.timeline.DealDamage(Mathf.Clamp(num - this.transitionHP, 0f, num));
		}
		else if (base.properties.CurrentHealth <= 0f && this.state != FlyingGenieLevelGenieTransform.State.Dead)
		{
			this.state = FlyingGenieLevelGenieTransform.State.Dead;
			if (Level.Current.mode == Level.Mode.Easy)
			{
				this.MarionetteDead();
			}
			else
			{
				this.StartDeath();
			}
		}
	}

	// Token: 0x06002271 RID: 8817 RVA: 0x0014240E File Offset: 0x0014080E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002272 RID: 8818 RVA: 0x0014242C File Offset: 0x0014082C
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002273 RID: 8819 RVA: 0x00142444 File Offset: 0x00140844
	public void StartMarionette(Vector3 spawnPos, FlyingGenieLevelMeditateFX meditateP1, FlyingGenieLevelMeditateFX meditateP2)
	{
		base.GetComponent<Collider2D>().enabled = true;
		base.transform.position = spawnPos;
		this.meditateP1 = meditateP1;
		this.meditateP2 = meditateP2;
		base.StartCoroutine(this.phase2_intro_cr());
	}

	// Token: 0x06002274 RID: 8820 RVA: 0x0014247C File Offset: 0x0014087C
	private IEnumerator phase2_intro_cr()
	{
		AudioManager.Play("genie_return");
		this.emitAudioFromObject.Add("genie_return");
		LevelProperties.FlyingGenie.Scan p = base.properties.CurrentState.scan;
		float timer = 0f;
		float P1ShrinkTimer = 0f;
		float P2ShrinkTimer = 0f;
		PlanePlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne) as PlanePlayerController;
		PlanePlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as PlanePlayerController;
		bool player2In = player2 != null;
		while (timer < p.scanDuration)
		{
			timer += CupheadTime.Delta;
			if (Level.Current.mode != Level.Mode.Easy)
			{
				if (player.Shrunk)
				{
					P1ShrinkTimer += CupheadTime.Delta;
				}
				if (player2In && player2.Shrunk)
				{
					P2ShrinkTimer += CupheadTime.Delta;
				}
			}
			yield return null;
		}
		if (P1ShrinkTimer >= p.miniDuration)
		{
			if (player2In)
			{
				if (P2ShrinkTimer >= p.miniDuration)
				{
					this.skipMarionette = true;
				}
				else
				{
					this.skipMarionette = false;
				}
			}
			else
			{
				this.skipMarionette = true;
			}
		}
		base.animator.SetTrigger("Continue");
		this.pyramidsGoingClockwise = Rand.Bool();
		if (this.skipMarionette)
		{
			this.transitionHP = p.transitionDamage;
			base.animator.SetBool("IsPuppet", true);
			this.state = FlyingGenieLevelGenieTransform.State.Giant;
			base.StartCoroutine(this.move_up_puppet_cr());
			base.properties.DealDamageToNextNamedState();
		}
		else
		{
			base.animator.SetBool("IsPuppet", false);
			this.state = FlyingGenieLevelGenieTransform.State.Marionette;
			yield return base.animator.WaitForAnimationToEnd(this, "Marionette_Intro", false, true);
			this.startPos = base.transform.position;
			base.StartCoroutine(this.move_cr());
			base.StartCoroutine(this.shoot_cr());
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002275 RID: 8821 RVA: 0x00142497 File Offset: 0x00140897
	private void EndFX()
	{
		if (this.meditateP1 != null)
		{
			this.meditateP1.EndEffect();
		}
		if (this.meditateP2 != null)
		{
			this.meditateP2.EndEffect();
		}
	}

	// Token: 0x06002276 RID: 8822 RVA: 0x001424D4 File Offset: 0x001408D4
	private void SnapPosition()
	{
		this.HandSFX();
		base.transform.position = this.morphRoot.position;
		this.bottomLayer.transform.localPosition = new Vector3(-160f, this.bottomLayer.transform.localPosition.y);
		base.StartCoroutine(this.handle_carpet_fadeout_cr());
	}

	// Token: 0x06002277 RID: 8823 RVA: 0x0014253C File Offset: 0x0014093C
	private IEnumerator move_up_puppet_cr()
	{
		float t = 0f;
		float timer = 0.4f;
		float slowTimer = 0.8f;
		float midTimer = 0.5f;
		float midEnd = 300f;
		float slowEnd = 410f;
		float end = 1071f;
		float start = base.transform.position.y;
		yield return base.animator.WaitForAnimationToEnd(this, "Genie_Morph_Puppet", false, true);
		this.tinyMarionette.gameObject.SetActive(true);
		while (t < slowTimer)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutSine, 0f, 1f, t / slowTimer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, slowEnd, val)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		start = base.transform.position.y;
		bool startIntro = false;
		while (t < midTimer)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / midTimer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, midEnd, val2)), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		t = 0f;
		start = base.transform.position.y;
		while (t < timer)
		{
			float val3 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / timer);
			base.transform.SetPosition(null, new float?(Mathf.Lerp(start, end, val3)), null);
			if (t / timer > 0.95f && !startIntro)
			{
				this.tinyMarionette.animator.SetTrigger("OnIntro");
				startIntro = true;
			}
			t += CupheadTime.Delta;
			yield return null;
		}
		this.tinyMarionette.transform.parent = null;
		base.properties.DealDamage(base.properties.CurrentState.scan.transitionDamage);
		Vector3 endPos = new Vector3(this.pyramidPivotPoint.position.x, this.pyramidPivotPoint.position.y + 145f);
		this.tinyMarionette.Activate(endPos, base.properties.CurrentState.scan, !this.pyramidsGoingClockwise);
		this.EndMarionette();
		yield return null;
		yield break;
	}

	// Token: 0x06002278 RID: 8824 RVA: 0x00142558 File Offset: 0x00140958
	private IEnumerator handle_carpet_fadeout_cr()
	{
		this.bottomLayer.color = new Color(1f, 1f, 1f, 1f);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			this.bottomLayer.color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.Play("Hands_Off");
		this.bottomLayer.color = new Color(1f, 1f, 1f, 1f);
		this.bottomLayer.transform.localPosition = Vector3.zero;
		yield return null;
		yield break;
	}

	// Token: 0x06002279 RID: 8825 RVA: 0x00142574 File Offset: 0x00140974
	private void SpawnTurban()
	{
		if (!this.skipMarionette)
		{
			this.spawner = this.spawnerPrefab.Create(new Vector3(base.transform.position.x, (float)Level.Current.Height + 100f), PlayerManager.GetNext(), base.properties.CurrentState.bullets);
			this.spawner.isDead = false;
		}
	}

	// Token: 0x0600227A RID: 8826 RVA: 0x001425EC File Offset: 0x001409EC
	private IEnumerator shoot_cr()
	{
		LevelProperties.FlyingGenie.Bullets p = base.properties.CurrentState.bullets;
		int mainShotIndex = UnityEngine.Random.Range(0, p.shotCount.Length);
		string[] shotCount = p.shotCount[mainShotIndex].Split(new char[]
		{
			','
		});
		int shotIndex = 0;
		string[] pinkCount = p.pinkString.Split(new char[]
		{
			','
		});
		int pinkIndex = 0;
		while (this.state == FlyingGenieLevelGenieTransform.State.Marionette)
		{
			this.isShooting = false;
			shotCount = p.shotCount[mainShotIndex].Split(new char[]
			{
				','
			});
			yield return CupheadTime.WaitForSeconds(this, p.hesitateRange.RandomFloat());
			this.isShooting = true;
			base.animator.SetBool("IsAttacking", true);
			yield return base.animator.WaitForAnimationToEnd(this, "Marionette_Attack_Start", false, true);
			AudioManager.Play("genie_voice_laugh_reverb");
			AbstractPlayerController player = PlayerManager.GetNext();
			base.animator.Play("Marionette_Spark");
			for (int i = 0; i < shotCount.Length; i++)
			{
				for (int j = 0; j < Parser.IntParse(shotCount[shotIndex]); j++)
				{
					if (player == null || player.IsDead)
					{
						player = PlayerManager.GetNext();
					}
					Vector3 dir = player.transform.position - this.marionetteShootRoot.transform.position;
					if (dir.x > 0f)
					{
						dir.x = 0f;
					}
					if (pinkCount[pinkIndex][0] == 'P')
					{
						this.pinkBullet.Create(this.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed);
						AudioManager.Play("genie_puppet_shoot");
						this.emitAudioFromObject.Add("genie_puppet_shoot");
					}
					else if (pinkCount[pinkIndex][0] == 'R')
					{
						this.shotBullet.Create(this.marionetteShootRoot.transform.position, MathUtils.DirectionToAngle(dir), p.shotSpeed);
						AudioManager.Play("genie_puppet_shoot");
						this.emitAudioFromObject.Add("genie_puppet_shoot");
					}
					yield return this.WaitWhileShooting(p.shotDelay, p.shotSpeed);
					pinkIndex = (pinkIndex + 1) % pinkCount.Length;
				}
				if (player == null || player.IsDead)
				{
					player = PlayerManager.GetNext();
				}
				yield return this.WaitWhileShooting(p.shotDelay, p.shotSpeed);
				if (shotIndex < shotCount.Length - 1)
				{
					shotIndex++;
				}
				else
				{
					mainShotIndex = (mainShotIndex + 1) % p.shotCount.Length;
					shotIndex = 0;
				}
				yield return null;
			}
			yield return null;
			base.animator.SetBool("IsAttacking", false);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600227B RID: 8827 RVA: 0x00142608 File Offset: 0x00140A08
	private IEnumerator WaitWhileShooting(float time, float shootSpeed)
	{
		bool pointingUp = false;
		float timeEsalpsed = 0f;
		float timeSinceSubShot = 0f;
		while (timeEsalpsed <= time)
		{
			if (timeSinceSubShot >= 0.12f)
			{
				this.shootBullet.Create(this.marionetteShootRoot.transform.position, (float)((!pointingUp) ? -100 : 100), shootSpeed);
				pointingUp = !pointingUp;
				timeSinceSubShot = 0f;
			}
			timeEsalpsed += CupheadTime.Delta;
			timeSinceSubShot += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600227C RID: 8828 RVA: 0x00142634 File Offset: 0x00140A34
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (this.state == FlyingGenieLevelGenieTransform.State.Marionette)
		{
			if (!this.isShooting)
			{
				if (base.transform.position.x > -this.startPos.x)
				{
					base.transform.AddPosition(-base.properties.CurrentState.bullets.marionetteMoveSpeed * CupheadTime.FixedDelta, 0f, 0f);
				}
			}
			else if (base.transform.position.x < this.startPos.x)
			{
				base.transform.AddPosition(base.properties.CurrentState.bullets.marionetteReturnSpeed * CupheadTime.FixedDelta, 0f, 0f);
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x0600227D RID: 8829 RVA: 0x00142650 File Offset: 0x00140A50
	public void EndMarionette()
	{
		if (!this.skipMarionette)
		{
			AudioManager.Play("genie_puppet_exit");
			this.emitAudioFromObject.Add("genie_puppet_exit");
		}
		if (this.spawner != null)
		{
			this.spawner.isDead = true;
		}
		this.spark.SetActive(false);
		this.StopAllCoroutines();
		this.state = FlyingGenieLevelGenieTransform.State.Giant;
		base.StartCoroutine(this.genie_intro_cr());
	}

	// Token: 0x0600227E RID: 8830 RVA: 0x001426C5 File Offset: 0x00140AC5
	private void MarionetteDead()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("MarionetteDeath");
	}

	// Token: 0x0600227F RID: 8831 RVA: 0x001426E4 File Offset: 0x00140AE4
	private IEnumerator genie_intro_cr()
	{
		float pullSpeed = 700f;
		float size = base.GetComponent<SpriteRenderer>().bounds.size.x;
		float angle = 120f;
		int number = 1;
		if (!this.skipMarionette)
		{
			base.animator.SetTrigger("MarionetteDeath");
			base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		while (base.transform.position.y < 960f)
		{
			base.transform.AddPosition(0f, pullSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		if (!this.skipMarionette)
		{
			base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.7f);
		base.animator.Play("Giant_Intro");
		base.transform.position = new Vector3(640f + size / 3f, 0f);
		Vector3 startPos = base.transform.position;
		float t = 0f;
		float time = 1f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(startPos, this.giantRoot.position, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = this.giantRoot.position;
		for (int i = 0; i < 3; i++)
		{
			this.SpawnPyramids(angle * 0.017453292f * (float)i, number);
			number++;
		}
		base.StartCoroutine(this.attack_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002280 RID: 8832 RVA: 0x001426FF File Offset: 0x00140AFF
	private void IntroHands()
	{
		base.StartCoroutine(this.intro_hands_cr());
	}

	// Token: 0x06002281 RID: 8833 RVA: 0x00142710 File Offset: 0x00140B10
	private IEnumerator intro_hands_cr()
	{
		Vector3 end = this.handFront.transform.position;
		Vector3 start = this.handFront.transform.position;
		start.y = this.handFront.transform.position.y - 500f;
		this.handFront.transform.position = start;
		this.handBack.transform.position = start;
		base.animator.Play("Giant_Hands");
		float t = 0f;
		float time = 1.25f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.handFront.transform.position = Vector2.Lerp(start, end, val);
			this.handBack.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, 0.8f);
		t = 0f;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(EaseUtils.EaseType.linear, 0f, 1f, t / time);
			this.handFront.transform.position = Vector2.Lerp(end, start, val2);
			this.handBack.transform.position = Vector2.Lerp(end, start, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.Play("Hands_Off");
		base.StartCoroutine(this.gem_stone_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002282 RID: 8834 RVA: 0x0014272C File Offset: 0x00140B2C
	private void SpawnPyramids(float startingAngle, int number)
	{
		LevelProperties.FlyingGenie.Pyramids pyramids = base.properties.CurrentState.pyramids;
		FlyingGenieLevelPyramid flyingGenieLevelPyramid = UnityEngine.Object.Instantiate<FlyingGenieLevelPyramid>(this.pyramidPrefab);
		flyingGenieLevelPyramid.Init(pyramids, base.transform.position, startingAngle, pyramids.speedRotation, this.pyramidPivotPoint, number, this.pyramidsGoingClockwise);
		flyingGenieLevelPyramid.GetComponent<Collider2D>().enabled = false;
		this.pyramids.Add(flyingGenieLevelPyramid);
	}

	// Token: 0x06002283 RID: 8835 RVA: 0x0014279C File Offset: 0x00140B9C
	private IEnumerator attack_cr()
	{
		LevelProperties.FlyingGenie.Pyramids p = base.properties.CurrentState.pyramids;
		string[] delayString = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] attackString = p.pyramidAttackString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
		int attackIndex = UnityEngine.Random.Range(0, attackString.Length);
		float delay = 0f;
		int numberReceived = 0;
		float t = 0f;
		float time = 2.5f;
		foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid in this.pyramids)
		{
			flyingGenieLevelPyramid.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
		}
		while (t < time)
		{
			t += CupheadTime.Delta;
			foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid2 in this.pyramids)
			{
				flyingGenieLevelPyramid2.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, t / time);
			}
			yield return null;
		}
		foreach (FlyingGenieLevelPyramid flyingGenieLevelPyramid3 in this.pyramids)
		{
			flyingGenieLevelPyramid3.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			flyingGenieLevelPyramid3.GetComponent<Collider2D>().enabled = true;
		}
		for (;;)
		{
			for (int i = attackIndex; i < attackString.Length; i++)
			{
				Parser.FloatTryParse(delayString[delayIndex], out delay);
				yield return CupheadTime.WaitForSeconds(this, delay);
				string[] attackOrder = attackString[i].Split(new char[]
				{
					'-'
				});
				foreach (string s in attackOrder)
				{
					Parser.IntTryParse(s, out numberReceived);
					for (int l = 0; l < this.pyramids.Count; l++)
					{
						if (this.pyramids[l].number == numberReceived)
						{
							base.StartCoroutine(this.pyramids[l].beam_cr());
						}
					}
				}
				for (int j = 0; j < this.pyramids.Count; j++)
				{
					if (this.pyramids[j].number == numberReceived)
					{
						while (!this.pyramids[j].finishedATK)
						{
							yield return null;
						}
					}
				}
				attackIndex = 0;
				i %= attackString.Length;
				delayIndex = (delayIndex + 1) % delayString.Length;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002284 RID: 8836 RVA: 0x001427B8 File Offset: 0x00140BB8
	private IEnumerator gem_stone_cr()
	{
		LevelProperties.FlyingGenie.GemStone p = base.properties.CurrentState.gemStone;
		string[] attackDelayPattern = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, attackDelayPattern.Length);
		this.pinkString = p.pinkString.Split(new char[]
		{
			','
		});
		this.pinkIndex = UnityEngine.Random.Range(0, this.pinkString.Length);
		float delay = 0f;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, p.warningDuration);
			Parser.FloatTryParse(attackDelayPattern[delayIndex], out delay);
			base.animator.SetTrigger("OnGiantAttack");
			yield return base.animator.WaitForAnimationToEnd(this, "Giant_Attack", false, true);
			yield return CupheadTime.WaitForSeconds(this, delay);
			delayIndex = (delayIndex + 1) % attackDelayPattern.Length;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002285 RID: 8837 RVA: 0x001427D4 File Offset: 0x00140BD4
	private void OnRing()
	{
		LevelProperties.FlyingGenie.GemStone gemStone = base.properties.CurrentState.gemStone;
		this.gemStone.LookAt2D(PlayerManager.GetNext().center);
		bool isPink;
		FlyingGenieLevelRing ring;
		if (this.pinkString[this.pinkIndex][0] == 'P')
		{
			isPink = true;
			ring = (this.pinkRingPrefab.Create(this.gemStone.position, this.gemStone.eulerAngles.z, gemStone.bulletSpeed) as FlyingGenieLevelRing);
		}
		else
		{
			isPink = false;
			ring = (this.ringPrefab.Create(this.gemStone.position, this.gemStone.eulerAngles.z, gemStone.bulletSpeed) as FlyingGenieLevelRing);
		}
		base.StartCoroutine(this.ring_cr(ring, isPink));
		this.pinkIndex = (this.pinkIndex + 1) % this.pinkString.Length;
	}

	// Token: 0x06002286 RID: 8838 RVA: 0x001428CC File Offset: 0x00140CCC
	private IEnumerator ring_cr(FlyingGenieLevelRing ring, bool isPink)
	{
		ring.isMain = true;
		int frameCount = 0;
		float frameTime = 0f;
		FlyingGenieLevelRing trailRing = (!isPink) ? this.ringPrefab : this.pinkRingPrefab;
		FlyingGenieLevelRing lastRing = null;
		while (ring != null)
		{
			frameTime += CupheadTime.Delta;
			if (frameTime > 0.041666668f)
			{
				if (frameCount < 3)
				{
					frameCount++;
				}
				else
				{
					frameCount = 0;
					if (lastRing != null)
					{
						lastRing.DisableCollision();
					}
					lastRing = (trailRing.Create(ring.transform.position, this.gemStone.eulerAngles.z, 0.1f) as FlyingGenieLevelRing);
				}
				frameTime -= 0.041666668f;
				yield return null;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002287 RID: 8839 RVA: 0x001428F5 File Offset: 0x00140CF5
	private void StartDeath()
	{
		if (this.skipMarionette && this.tinyMarionette != null)
		{
			this.tinyMarionette.Die();
		}
		base.animator.SetTrigger("Death");
	}

	// Token: 0x06002288 RID: 8840 RVA: 0x0014292E File Offset: 0x00140D2E
	private void SpawnPuff()
	{
		this.deathPuffEffect.Create(this.deathPuffRoot.transform.position);
	}

	// Token: 0x06002289 RID: 8841 RVA: 0x0014294C File Offset: 0x00140D4C
	private void HandSFX()
	{
		AudioManager.Play("genie_puppet_hand_enter");
		this.emitAudioFromObject.Add("genie_puppet_hand_enter");
	}

	// Token: 0x0600228A RID: 8842 RVA: 0x00142968 File Offset: 0x00140D68
	private void SoundGenieVoiceMorph()
	{
		AudioManager.Play("genie_voice_excited");
		this.emitAudioFromObject.Add("genie_voice_excited");
	}

	// Token: 0x0600228B RID: 8843 RVA: 0x00142984 File Offset: 0x00140D84
	private void SoundPuppetRun()
	{
		AudioManager.Play("genie_puppet_run");
		this.emitAudioFromObject.Add("genie_puppet_run");
	}

	// Token: 0x0600228C RID: 8844 RVA: 0x001429A0 File Offset: 0x00140DA0
	private void SoundGenieVoicePhase3Intro()
	{
		AudioManager.Play("genie_voice_phase3_intro");
		this.emitAudioFromObject.Add("genie_voice_phase3_intro");
	}

	// Token: 0x0600228D RID: 8845 RVA: 0x001429BC File Offset: 0x00140DBC
	private void SoundGenieMindShoot()
	{
		AudioManager.Play("genie_phase3_mind_shoot");
		this.emitAudioFromObject.Add("genie_phase3_mind_shoot");
	}

	// Token: 0x0600228E RID: 8846 RVA: 0x001429D8 File Offset: 0x00140DD8
	private void SoundPuppetSmallEnterMobile()
	{
		AudioManager.Play("genie_puppetsmall_enter_mobile");
		this.emitAudioFromObject.Add("genie_puppetsmall_enter_mobile");
	}

	// Token: 0x04002B14 RID: 11028
	private const float FRAME_TIME = 0.041666668f;

	// Token: 0x04002B16 RID: 11030
	[SerializeField]
	private Effect deathPuffEffect;

	// Token: 0x04002B17 RID: 11031
	[SerializeField]
	private SpriteRenderer bottomLayer;

	// Token: 0x04002B18 RID: 11032
	[Space(10f)]
	[SerializeField]
	private FlyingGenieLevelSpawner spawnerPrefab;

	// Token: 0x04002B19 RID: 11033
	[SerializeField]
	private Transform marionetteShootRoot;

	// Token: 0x04002B1A RID: 11034
	[SerializeField]
	private BasicProjectile shotBullet;

	// Token: 0x04002B1B RID: 11035
	[SerializeField]
	private BasicProjectile pinkBullet;

	// Token: 0x04002B1C RID: 11036
	[SerializeField]
	private BasicProjectile shootBullet;

	// Token: 0x04002B1D RID: 11037
	[SerializeField]
	private BasicProjectile spreadProjectile;

	// Token: 0x04002B1E RID: 11038
	[SerializeField]
	private FlyingGenieLevelRing ringPrefab;

	// Token: 0x04002B1F RID: 11039
	[SerializeField]
	private FlyingGenieLevelRing pinkRingPrefab;

	// Token: 0x04002B20 RID: 11040
	[SerializeField]
	private FlyingGenieLevelPyramid pyramidPrefab;

	// Token: 0x04002B21 RID: 11041
	[SerializeField]
	private FlyingGenieLevelTinyMarionette tinyMarionette;

	// Token: 0x04002B22 RID: 11042
	[Space(10f)]
	[SerializeField]
	private Transform pyramidPivotPoint;

	// Token: 0x04002B23 RID: 11043
	[SerializeField]
	private Transform gemStone;

	// Token: 0x04002B24 RID: 11044
	[SerializeField]
	private Transform pipe;

	// Token: 0x04002B25 RID: 11045
	[SerializeField]
	private Transform giantRoot;

	// Token: 0x04002B26 RID: 11046
	[SerializeField]
	private Transform handFront;

	// Token: 0x04002B27 RID: 11047
	[SerializeField]
	private Transform handBack;

	// Token: 0x04002B28 RID: 11048
	[SerializeField]
	private Transform deathPuffRoot;

	// Token: 0x04002B29 RID: 11049
	[SerializeField]
	private Transform morphRoot;

	// Token: 0x04002B2A RID: 11050
	[SerializeField]
	private Transform marionetteRoot;

	// Token: 0x04002B2B RID: 11051
	[SerializeField]
	private GameObject spark;

	// Token: 0x04002B2C RID: 11052
	private FlyingGenieLevelMeditateFX meditateP1;

	// Token: 0x04002B2D RID: 11053
	private FlyingGenieLevelMeditateFX meditateP2;

	// Token: 0x04002B2E RID: 11054
	private FlyingGenieLevelSpawner spawner;

	// Token: 0x04002B2F RID: 11055
	private List<FlyingGenieLevelBomb> bombs;

	// Token: 0x04002B30 RID: 11056
	private List<FlyingGenieLevelPyramid> pyramids;

	// Token: 0x04002B31 RID: 11057
	private DamageDealer damageDealer;

	// Token: 0x04002B32 RID: 11058
	private DamageReceiver damageReceiver;

	// Token: 0x04002B33 RID: 11059
	private Vector3 startPos;

	// Token: 0x04002B34 RID: 11060
	private bool pyramidsGoingClockwise;

	// Token: 0x04002B35 RID: 11061
	private bool isShooting;

	// Token: 0x04002B37 RID: 11063
	private float transitionHP;

	// Token: 0x04002B38 RID: 11064
	private int pinkIndex;

	// Token: 0x04002B39 RID: 11065
	private string[] pinkString;

	// Token: 0x0200066C RID: 1644
	public enum State
	{
		// Token: 0x04002B3B RID: 11067
		Intro,
		// Token: 0x04002B3C RID: 11068
		Idle,
		// Token: 0x04002B3D RID: 11069
		Marionette,
		// Token: 0x04002B3E RID: 11070
		Giant,
		// Token: 0x04002B3F RID: 11071
		Dead
	}
}
