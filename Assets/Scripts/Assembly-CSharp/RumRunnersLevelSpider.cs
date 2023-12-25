using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200079F RID: 1951
public class RumRunnersLevelSpider : LevelProperties.RumRunners.Entity
{
	// Token: 0x1400004B RID: 75
	// (add) Token: 0x06002B6F RID: 11119 RVA: 0x00194310 File Offset: 0x00192710
	// (remove) Token: 0x06002B70 RID: 11120 RVA: 0x00194348 File Offset: 0x00192748
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x170003FE RID: 1022
	// (get) Token: 0x06002B71 RID: 11121 RVA: 0x0019437E File Offset: 0x0019277E
	// (set) Token: 0x06002B72 RID: 11122 RVA: 0x00194386 File Offset: 0x00192786
	public bool goingLeft { get; private set; }

	// Token: 0x170003FF RID: 1023
	// (get) Token: 0x06002B73 RID: 11123 RVA: 0x0019438F File Offset: 0x0019278F
	// (set) Token: 0x06002B74 RID: 11124 RVA: 0x00194397 File Offset: 0x00192797
	public bool moving { get; private set; }

	// Token: 0x17000400 RID: 1024
	// (get) Token: 0x06002B75 RID: 11125 RVA: 0x001943A0 File Offset: 0x001927A0
	private float dir
	{
		get
		{
			return (float)((!this.goingLeft) ? 1 : -1);
		}
	}

	// Token: 0x06002B76 RID: 11126 RVA: 0x001943B8 File Offset: 0x001927B8
	private void Start()
	{
		this.goingLeft = true;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.collider = base.GetComponent<Collider2D>();
		this.scaleX = base.transform.localScale.x;
		base.transform.SetScale(new float?(this.scaleX * this.dir), null, null);
		this.SetUpMinePositions();
		this.grubEnterVariant = UnityEngine.Random.Range(0, 3);
		this.grubVariant = UnityEngine.Random.Range(0, 4);
	}

	// Token: 0x06002B77 RID: 11127 RVA: 0x00194470 File Offset: 0x00192870
	public override void LevelInit(LevelProperties.RumRunners properties)
	{
		base.LevelInit(properties);
		this.mineMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.mine.minePlacementString.Length);
		this.mineIndex = UnityEngine.Random.Range(0, properties.CurrentState.mine.minePlacementString[this.mineMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.bouncingPattern = new PatternString(properties.CurrentState.bouncing.shootBeetleAngleString, true, true);
		this.grubDelayString = new PatternString(properties.CurrentState.grubs.delayString, true, true);
		this.grubPositionString = new PatternString(properties.CurrentState.grubs.appearPositionString, true, false);
		Level.Current.OnLevelStartEvent += this.OnIntroEnd;
	}

	// Token: 0x06002B78 RID: 11128 RVA: 0x00194544 File Offset: 0x00192944
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float x = base.transform.position.x;
		float num = (float)(-(float)Level.Current.Width) * 0.5f + this.deathInvincibilityBuffer;
		float num2 = (float)Level.Current.Width * 0.5f - this.deathInvincibilityBuffer;
		float num3 = base.properties.CurrentHealth - base.properties.GetNextStateHealthTrigger() * base.properties.TotalHealth;
		if (info.damage > num3 && (x > num2 || x < num))
		{
			return;
		}
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002B79 RID: 11129 RVA: 0x001945EA File Offset: 0x001929EA
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase == CollisionPhase.Enter)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002B7A RID: 11130 RVA: 0x00194612 File Offset: 0x00192A12
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002B7B RID: 11131 RVA: 0x0019462C File Offset: 0x00192A2C
	private void OnIntroEnd()
	{
		this.policeman.SetProperties(base.properties.CurrentState.spider, this);
		Level.Current.OnLevelStartEvent -= this.OnIntroEnd;
		base.StartCoroutine(this.introExit());
	}

	// Token: 0x06002B7C RID: 11132 RVA: 0x00194678 File Offset: 0x00192A78
	private IEnumerator introExit()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		base.animator.SetTrigger("IntroExit");
		yield return base.animator.WaitForAnimationToEnd(this, "IntroExit", false, true);
		base.StartCoroutine(this.run_cr());
		yield break;
	}

	// Token: 0x06002B7D RID: 11133 RVA: 0x00194693 File Offset: 0x00192A93
	private void SummonSelection()
	{
		base.StartCoroutine(this.check_to_start_summon_cr());
	}

	// Token: 0x06002B7E RID: 11134 RVA: 0x001946A4 File Offset: 0x00192AA4
	private IEnumerator check_to_start_summon_cr()
	{
		RumRunnersLevelSpider.SummonType summonType = this.summonType;
		if (summonType != RumRunnersLevelSpider.SummonType.Bouncing)
		{
			if (summonType != RumRunnersLevelSpider.SummonType.Mine)
			{
				if (summonType == RumRunnersLevelSpider.SummonType.Grubs)
				{
					base.animator.Play("GrubSummonWait");
					yield return null;
					this.isSummoning = true;
					this.StartGrubs();
					yield return base.animator.WaitForAnimationToEnd(this, "GrubSummonWait", false, true);
					this.isSummoning = false;
				}
			}
			else
			{
				base.animator.Play("MineSummon");
				yield return null;
				this.isSummoning = true;
				while (base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 0.5f)
				{
					yield return null;
				}
				this.StartMine();
				yield return base.animator.WaitForAnimationToEnd(this, "MineSummon", false, true);
				this.isSummoning = false;
			}
		}
		else
		{
			base.animator.Play("Kick");
			yield return null;
			this.isSummoning = true;
			yield return base.animator.WaitForAnimationToEnd(this, "Kick", false, true);
			this.isSummoning = false;
		}
		yield break;
	}

	// Token: 0x06002B7F RID: 11135 RVA: 0x001946BF File Offset: 0x00192ABF
	private void animationEvent_SpawnFrontPuffEffect()
	{
	}

	// Token: 0x06002B80 RID: 11136 RVA: 0x001946C1 File Offset: 0x00192AC1
	private void animationEvent_SpawnBackPuffEffect()
	{
	}

	// Token: 0x06002B81 RID: 11137 RVA: 0x001946C4 File Offset: 0x00192AC4
	private IEnumerator run_cr()
	{
		LevelProperties.RumRunners.Spider p = base.properties.CurrentState.spider;
		bool hasSummoned = false;
		bool spawnedCop = false;
		this.moving = false;
		PatternString copPositionString = new PatternString(p.copPositionString, true, true);
		PatternString copBulletTypeString = new PatternString(p.copBulletTypeString, true, true);
		PatternString spiderPositionString = new PatternString(p.spiderPositionString, true, true);
		PatternString spiderActionString = new PatternString(p.spiderActionString, true, true);
		PatternString spiderActionPositionString = new PatternString(p.spiderActionPositionString, true);
		YieldInstruction wait = new WaitForFixedUpdate();
		this.copSpawnPos = p.copSpawnSpiderDist;
		bool isInitial = true;
		for (;;)
		{
			char summonChar;
			if (isInitial)
			{
				summonChar = ((!Rand.Bool()) ? 'M' : 'N');
				spawnedCop = true;
			}
			else
			{
				yield return CupheadTime.WaitForSeconds(this, p.spiderEnterDelay);
				summonChar = spiderActionString.PopLetter();
			}
			int spawnPointIndex = spiderPositionString.PopInt();
			float popOutX = (float)((!this.goingLeft) ? -640 : 640);
			float summonPos = Mathf.Lerp(popOutX, -popOutX, spiderActionPositionString.PopFloat());
			if (summonChar != 'B')
			{
				if (summonChar != 'G')
				{
					if (summonChar != 'M')
					{
						this.summonType = RumRunnersLevelSpider.SummonType.None;
						hasSummoned = true;
					}
					else
					{
						this.summonType = RumRunnersLevelSpider.SummonType.Mine;
					}
				}
				else
				{
					this.summonType = RumRunnersLevelSpider.SummonType.Grubs;
				}
			}
			else
			{
				summonPos = Mathf.Lerp(popOutX, -popOutX, 0.05f);
				this.summonType = RumRunnersLevelSpider.SummonType.Bouncing;
				if (spawnPointIndex == 2)
				{
					spawnPointIndex = ((!Rand.Bool()) ? 1 : 0);
				}
			}
			if (!isInitial)
			{
				base.transform.position = new Vector3(((float)Level.Current.Right + 350f) * -this.dir, this.spawnPoints[spawnPointIndex].position.y);
			}
			if (isInitial && this.summonType == RumRunnersLevelSpider.SummonType.Mine)
			{
				hasSummoned = false;
				summonPos = Mathf.Lerp(popOutX, -popOutX, 0.75f);
			}
			float timeToSummon = Mathf.Abs(base.transform.position.x - summonPos) / p.spiderSpeed;
			float animatorStartTime = 1f - timeToSummon / this.runClip.length;
			int copPos = copPositionString.PopInt();
			this.nextCopPosition = new Vector3(this.spawnPoints[copPos].position.x * this.dir, this.spawnPoints[copPos].position.y);
			base.transform.SetScale(new float?(this.scaleX * this.dir), null, null);
			if (!isInitial)
			{
				string stateName = "Run";
				if (this.summonType == RumRunnersLevelSpider.SummonType.Grubs)
				{
					stateName = "GrubSummonEnter";
				}
				else if (this.summonType == RumRunnersLevelSpider.SummonType.Bouncing)
				{
					stateName = "RunCaterpillar";
				}
				base.animator.Play(stateName, 0, animatorStartTime);
			}
			bool isPink = copBulletTypeString.PopLetter() == 'P';
			this.moving = true;
			if (this.summonType == RumRunnersLevelSpider.SummonType.Grubs)
			{
				this.SFX_RUMRUN_Spider_GrubSummon_PhoneTinyVoice();
			}
			while ((this.goingLeft && base.transform.position.x > popOutX) || (!this.goingLeft && base.transform.position.x < popOutX))
			{
				base.transform.position += Vector3.right * p.spiderSpeed * CupheadTime.FixedDelta * this.dir;
				base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, 0f, 200f)), null);
				yield return wait;
			}
			while (this.moving)
			{
				if (!hasSummoned && ((this.goingLeft && base.transform.position.x <= summonPos) || (!this.goingLeft && base.transform.position.x >= summonPos)))
				{
					this.SummonSelection();
					hasSummoned = true;
				}
				while (this.isSummoning)
				{
					yield return null;
				}
				if ((!this.goingLeft && (float)Level.Current.Right + 350f > base.transform.position.x) || (this.goingLeft && (float)Level.Current.Left - 350f < base.transform.position.x))
				{
					float copSpawnDistanceRemaining = this.copSpawnPos - this.dir * base.transform.position.x;
					if (!spawnedCop && copSpawnDistanceRemaining < 0f)
					{
						this.policeman.CopAppear(this.nextCopPosition, isPink, this.goingLeft);
						spawnedCop = true;
						this.nextCopPosition = Vector3.up * 5000f;
					}
					base.transform.position += Vector3.right * p.spiderSpeed * CupheadTime.FixedDelta * this.dir;
					base.transform.SetPosition(null, new float?(RumRunnersLevel.GroundWalkingPosY(base.transform.position, this.collider, 0f, 200f)), null);
					yield return wait;
				}
				else
				{
					this.moving = false;
				}
			}
			hasSummoned = false;
			spawnedCop = false;
			this.goingLeft = !this.goingLeft;
			isInitial = false;
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06002B82 RID: 11138 RVA: 0x001946E0 File Offset: 0x00192AE0
	public bool GrubCanEnter(Vector3 pos, float enterTime)
	{
		if (!this.moving)
		{
			return false;
		}
		if (Mathf.Abs(base.transform.position.y - pos.y) < 100f && (Mathf.Abs(base.transform.position.x + base.properties.CurrentState.spider.spiderSpeed * this.dir * enterTime - pos.x) < 500f || Mathf.Abs(base.transform.position.x - pos.x) < 500f))
		{
			return false;
		}
		if (Mathf.Abs(base.transform.position.x) > 400f)
		{
			if (this.policeman.isActive && Mathf.Abs(this.policeman.transform.position.y - pos.y) < 100f && Mathf.Sign(this.policeman.transform.position.x) == Mathf.Sign(pos.x))
			{
				return false;
			}
			if (Mathf.Abs(this.nextCopPosition.y - pos.y) < 100f && Mathf.Sign(this.nextCopPosition.x) == Mathf.Sign(pos.x))
			{
				return false;
			}
		}
		this.grubList.RemoveAll((RumRunnersLevelGrub g) => g == null);
		for (int i = 0; i < this.grubList.Count; i++)
		{
			if (this.grubList[i].startedEntering && Mathf.Abs(this.grubList[i].transform.position.y - pos.y) < 100f)
			{
				bool flag = !this.grubList[i].moving;
				if (Mathf.Abs((this.grubList[i].transform.position + Vector3.right * this.grubList[i].speed * (enterTime - this.grubList[i].GetTimeToMove())).x - pos.x) < 200f)
				{
					return false;
				}
				if (flag && Mathf.Abs((this.grubList[i].transform.position + Vector3.left * this.grubList[i].speed * (enterTime - this.grubList[i].GetTimeToMove())).x - pos.x) < 200f)
				{
					return false;
				}
			}
		}
		return true;
	}

	// Token: 0x06002B83 RID: 11139 RVA: 0x00194A0D File Offset: 0x00192E0D
	private void StartGrubs()
	{
		base.StartCoroutine(this.grubs_cr());
	}

	// Token: 0x06002B84 RID: 11140 RVA: 0x00194A1C File Offset: 0x00192E1C
	private IEnumerator grubs_cr()
	{
		LevelProperties.RumRunners.Grubs p = base.properties.CurrentState.grubs;
		float delay = 0f;
		int y = 0;
		int x = 0;
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.grubs.grubSummonWarning);
		int count = this.grubPositionString.SubStringLength();
		this.grubPositionString.SetSubStringIndex(count);
		for (int i = 0; i < count; i++)
		{
			AbstractPlayerController player = PlayerManager.GetNext();
			int appearPosition = this.grubPositionString.PopInt();
			x = appearPosition % 6;
			y = appearPosition / 6;
			if (x != 5 || y != 2)
			{
				this.grubList.RemoveAll((RumRunnersLevelGrub g) => g == null);
				bool canSpawn = true;
				for (int j = 0; j < this.grubList.Count; j++)
				{
					if (!this.grubList[j].startedEntering && this.grubList[j].x == x && this.grubList[j].y == y)
					{
						canSpawn = false;
					}
				}
				if (canSpawn)
				{
					this.grubList.Add(this.grubPrefab.Create(this.grubPaths[y * 6 + x], 0f, p.movementSpeed, p.warningDuration, p.hp, this, this.grubEnterVariant, this.grubVariant, count - i, x, y));
					this.grubEnterVariant = (this.grubEnterVariant + 1) % 3;
					this.grubVariant = (this.grubVariant + 1) % 4;
					if (i < count - 1)
					{
						delay = this.grubDelayString.PopFloat();
						yield return CupheadTime.WaitForSeconds(this, delay);
					}
				}
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002B85 RID: 11141 RVA: 0x00194A38 File Offset: 0x00192E38
	private void SetUpMinePositions()
	{
		this.minePositions = new Vector3[5, 3];
		this.minePositions[0, 0] = new Vector3(-553f, 354f);
		this.minePositions[1, 0] = new Vector3(-282f, 321f);
		this.minePositions[2, 0] = new Vector3(16f, 354f);
		this.minePositions[3, 0] = new Vector3(311f, 313f);
		this.minePositions[4, 0] = new Vector3(545f, 343f);
		this.minePositions[0, 1] = new Vector3(-492f, 33f);
		this.minePositions[1, 1] = new Vector3(-247f, 19f);
		this.minePositions[2, 1] = new Vector3(42f, 35f);
		this.minePositions[3, 1] = new Vector3(287f, 7f);
		this.minePositions[4, 1] = new Vector3(509f, 36f);
		this.minePositions[0, 2] = new Vector3(-524f, -284f);
		this.minePositions[1, 2] = new Vector3(-224f, -265f);
		this.minePositions[2, 2] = new Vector3(-17f, -294f);
		this.minePositions[3, 2] = new Vector3(253f, -252f);
		this.minePositions[4, 2] = new Vector3(575f, -291f);
	}

	// Token: 0x06002B86 RID: 11142 RVA: 0x00194C41 File Offset: 0x00193041
	public void StartMine()
	{
		base.StartCoroutine(this.mine_cr());
	}

	// Token: 0x06002B87 RID: 11143 RVA: 0x00194C50 File Offset: 0x00193050
	private IEnumerator mine_cr()
	{
		LevelProperties.RumRunners.Mine p = base.properties.CurrentState.mine;
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		Vector2 pos = Vector2.zero;
		string[] minePlacementString = p.minePlacementString[this.mineMainIndex].Split(new char[]
		{
			','
		});
		this.mineList.RemoveAll((RumRunnersLevelMine m) => m == null);
		this.mineIndex = 0;
		int i = 0;
		while ((float)i < Mathf.Min(p.mineNumber, (float)minePlacementString.Length))
		{
			pos = this.GetMinePos(Parser.IntParse(minePlacementString[this.mineIndex]));
			bool foundFreeSpot = false;
			int checkedPositionsCount = 0;
			while (!foundFreeSpot && checkedPositionsCount < 15)
			{
				float distP = 1000f;
				float distP2 = 1000f;
				bool spotOccupied = false;
				for (int j = 0; j < this.mineList.Count; j++)
				{
					if (this.mineList[j].xPos == (int)pos.x && this.mineList[j].yPos == (int)pos.y)
					{
						spotOccupied = true;
					}
				}
				if (!spotOccupied)
				{
					if (!player.IsDead)
					{
						distP = Vector3.Distance(player.transform.position, this.minePositions[(int)pos.x, (int)pos.y]);
					}
					if (player2 != null && !player2.IsDead)
					{
						distP2 = Vector3.Distance(player2.transform.position, this.minePositions[(int)pos.x, (int)pos.y]);
					}
				}
				if (distP > p.mineCheckToLand && distP2 > p.mineCheckToLand && !spotOccupied)
				{
					foundFreeSpot = true;
					break;
				}
				if (this.mineIndex < minePlacementString.Length - 1)
				{
					this.mineIndex++;
				}
				else
				{
					this.mineMainIndex = (this.mineMainIndex + 1) % p.minePlacementString.Length;
					this.mineIndex = 0;
				}
				pos = this.GetMinePos(Parser.IntParse(minePlacementString[this.mineIndex]));
				checkedPositionsCount++;
				yield return null;
			}
			if (checkedPositionsCount < 15)
			{
				RumRunnersLevelMine rumRunnersLevelMine = this.minePrefab.Spawn<RumRunnersLevelMine>();
				this.mineList.Add(rumRunnersLevelMine.Init(this.minePositions[(int)pos.x, (int)pos.y], p, this, Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y)));
			}
			yield return CupheadTime.WaitForSeconds(this, 0.2f);
			if (this.mineIndex < minePlacementString.Length - 1)
			{
				this.mineIndex++;
			}
			else
			{
				this.mineMainIndex = (this.mineMainIndex + 1) % p.minePlacementString.Length;
				this.mineIndex = 0;
			}
			i++;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002B88 RID: 11144 RVA: 0x00194C6C File Offset: 0x0019306C
	private Vector2 GetMinePos(int mineNum)
	{
		Vector2 zero = Vector2.zero;
		zero.x = (float)(mineNum % 5);
		zero.y = (float)(mineNum / 5);
		if (zero.x > 4f || zero.x < 0f || zero.y > 2f || zero.y < 0f)
		{
			global::Debug.Break();
		}
		return zero;
	}

	// Token: 0x06002B89 RID: 11145 RVA: 0x00194CE0 File Offset: 0x001930E0
	private void animationEvent_StartBouncing()
	{
		LevelProperties.RumRunners.Bouncing bouncing = base.properties.CurrentState.bouncing;
		do
		{
			this.beetleList.RemoveAll((RumRunnersLevelBouncingBeetle b) => b == null || b.leaveScreen);
			if (this.beetleList.Count >= bouncing.maxBeetleCount)
			{
				this.beetleList[0].leaveScreen = true;
			}
		}
		while (this.beetleList.Count >= bouncing.maxBeetleCount);
		float num = this.bouncingPattern.PopFloat();
		num = Mathf.Clamp(num, 10f, 80f);
		if (this.dir < 0f)
		{
			num = 180f - num;
		}
		Vector3 vector = MathUtils.AngleToDirection(num);
		RumRunnersLevelBouncingBeetle rumRunnersLevelBouncingBeetle = this.caterpillarPrefab.Spawn<RumRunnersLevelBouncingBeetle>();
		rumRunnersLevelBouncingBeetle.Init(this.caterpillarSpawnPoint.position + vector * 110f, vector, bouncing.shootBeetleInitialSpeed, (float)bouncing.shootBeetleTimeToSlowdown, bouncing.shootBeetleSpeed, bouncing.shootBeetleHealth);
		this.beetleList.Add(rumRunnersLevelBouncingBeetle);
		this.kickFXEffect.Create(this.kickFXSpawnPoint.position + vector * 110f * 0.8f);
	}

	// Token: 0x06002B8A RID: 11146 RVA: 0x00194E30 File Offset: 0x00193230
	public void Die()
	{
		base.animator.SetTrigger("Dead");
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Effects.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 0;
		this.StopAllCoroutines();
		this.deathExplodeEffect.Create(base.transform.position);
		foreach (RumRunnersLevelBouncingBeetle rumRunnersLevelBouncingBeetle in this.beetleList)
		{
			rumRunnersLevelBouncingBeetle.leaveScreen = true;
		}
		this.mineList.RemoveAll((RumRunnersLevelMine m) => m == null);
		this.mineList.Sort((RumRunnersLevelMine m1, RumRunnersLevelMine m2) => m1.endPhaseExplodePriority.CompareTo(m2.endPhaseExplodePriority));
		for (int i = 0; i < this.mineList.Count; i++)
		{
			this.mineList[i].SetTimer((float)i * 0.6f);
		}
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.SFX_RUMRUN_ExitPhase1_SpiderFalling();
	}

	// Token: 0x06002B8B RID: 11147 RVA: 0x00194F84 File Offset: 0x00193384
	private void AniEvent_ChangeToForeground()
	{
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Foreground.ToString();
		base.GetComponent<SpriteRenderer>().sortingOrder = 100;
	}

	// Token: 0x06002B8C RID: 11148 RVA: 0x00194FB8 File Offset: 0x001933B8
	private void animationEvent_ShakeScreen()
	{
		CupheadLevelCamera.Current.Shake(30f, 0.6f, false);
	}

	// Token: 0x06002B8D RID: 11149 RVA: 0x00194FCF File Offset: 0x001933CF
	private void AniEvent_DeathComplete()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002B8E RID: 11150 RVA: 0x00194FDC File Offset: 0x001933DC
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		if (Level.Current)
		{
			float x = this.copSpawnPos * this.dir;
			Vector3 from = new Vector3(x, -360f);
			Vector3 to = new Vector3(x, 360f);
			Gizmos.color = Color.blue;
			Gizmos.DrawLine(from, to);
		}
		Gizmos.DrawWireSphere(this.caterpillarSpawnPoint.position, 110f);
	}

	// Token: 0x06002B8F RID: 11151 RVA: 0x0019504C File Offset: 0x0019344C
	private void AnimationEvent_SFX_RUMRUN_Mine_SpiderButtonPress()
	{
		AudioManager.Play("sfx_dlc_rumrun_mine_spiderbuttonpress");
	}

	// Token: 0x06002B90 RID: 11152 RVA: 0x00195058 File Offset: 0x00193458
	private void AnimationEvent_SFX_RUMRUN_Spider_GrubSummon_Phone()
	{
		AudioManager.Play("sfx_dlc_rumrun_spider_grubsummon_phone");
		AudioManager.Stop("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice");
	}

	// Token: 0x06002B91 RID: 11153 RVA: 0x0019506E File Offset: 0x0019346E
	private void SFX_RUMRUN_Spider_GrubSummon_PhoneTinyVoice()
	{
		AudioManager.Play("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_spider_grubsummon_phonetinyvoice");
	}

	// Token: 0x06002B92 RID: 11154 RVA: 0x0019508A File Offset: 0x0019348A
	private void AnimationEvent_SFX_RUMRUN_CaterpillarBall_SpiderKick()
	{
		AudioManager.Play("sfx_dlc_rumrun_caterpillarball_spiderkick");
	}

	// Token: 0x06002B93 RID: 11155 RVA: 0x00195096 File Offset: 0x00193496
	private void SFX_RUMRUN_ExitPhase1_SpiderFalling()
	{
		AudioManager.Play("sfx_DLC_RUMRUN_ExitPhase1_SpiderFalling");
		AudioManager.FadeSFXVolume("sfx_DLC_RUMRUN_ExitPhase1_SpiderFalling", 1f, 10f);
	}

	// Token: 0x0400341C RID: 13340
	private const float INTRO_EXIT_DELAY = 0.2f;

	// Token: 0x0400341D RID: 13341
	private const float EDGE_OFFSET = 350f;

	// Token: 0x0400341E RID: 13342
	private const float MINE_DELAY = 0.2f;

	// Token: 0x0400341F RID: 13343
	private const float MINE_EXPLODE_INTERVAL_ON_PHASE_END = 0.6f;

	// Token: 0x04003420 RID: 13344
	private const float GRUB_MIN_SPIDER_DISTANCE_TO_ENTER = 500f;

	// Token: 0x04003421 RID: 13345
	private const float GRUB_MIN_OTHER_GRUB_DISTANCE_TO_ENTER = 200f;

	// Token: 0x04003422 RID: 13346
	private const float BOUNCER_MIN_ANGLE = 10f;

	// Token: 0x04003423 RID: 13347
	private const float BOUNCER_MAX_ANGLE = 80f;

	// Token: 0x04003424 RID: 13348
	private const float KICK_SPAWN_RADIUS = 110f;

	// Token: 0x04003425 RID: 13349
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x04003426 RID: 13350
	[SerializeField]
	private AnimationClip runClip;

	// Token: 0x04003427 RID: 13351
	[SerializeField]
	private RumRunnersLevelPoliceman policeman;

	// Token: 0x04003428 RID: 13352
	[SerializeField]
	private float deathInvincibilityBuffer;

	// Token: 0x04003429 RID: 13353
	[SerializeField]
	private Effect deathExplodeEffect;

	// Token: 0x0400342A RID: 13354
	[Header("Summons")]
	[SerializeField]
	private RumRunnersLevelGrub grubPrefab;

	// Token: 0x0400342B RID: 13355
	[SerializeField]
	private RumRunnersLevelGrubPath[] grubPaths;

	// Token: 0x0400342C RID: 13356
	[SerializeField]
	private RumRunnersLevelMine minePrefab;

	// Token: 0x0400342D RID: 13357
	[SerializeField]
	private RumRunnersLevelBouncingBeetle caterpillarPrefab;

	// Token: 0x0400342E RID: 13358
	[SerializeField]
	private Transform caterpillarSpawnPoint;

	// Token: 0x0400342F RID: 13359
	[SerializeField]
	private Effect kickFXEffect;

	// Token: 0x04003430 RID: 13360
	[SerializeField]
	private Transform kickFXSpawnPoint;

	// Token: 0x04003433 RID: 13363
	private RumRunnersLevelSpider.SummonType summonType;

	// Token: 0x04003434 RID: 13364
	public bool isSummoning;

	// Token: 0x04003435 RID: 13365
	private Vector3 nextCopPosition;

	// Token: 0x04003436 RID: 13366
	private PatternString grubDelayString;

	// Token: 0x04003437 RID: 13367
	private PatternString grubPositionString;

	// Token: 0x04003438 RID: 13368
	private List<RumRunnersLevelGrub> grubList = new List<RumRunnersLevelGrub>();

	// Token: 0x04003439 RID: 13369
	private int mineMainIndex;

	// Token: 0x0400343A RID: 13370
	private int mineIndex;

	// Token: 0x0400343B RID: 13371
	private List<RumRunnersLevelMine> mineList = new List<RumRunnersLevelMine>();

	// Token: 0x0400343C RID: 13372
	private Vector3[,] minePositions;

	// Token: 0x0400343D RID: 13373
	private PatternString bouncingPattern;

	// Token: 0x0400343E RID: 13374
	private int grubEnterVariant;

	// Token: 0x0400343F RID: 13375
	private int grubVariant;

	// Token: 0x04003440 RID: 13376
	private DamageDealer damageDealer;

	// Token: 0x04003441 RID: 13377
	private DamageReceiver damageReceiver;

	// Token: 0x04003442 RID: 13378
	private Collider2D collider;

	// Token: 0x04003443 RID: 13379
	private float scaleX;

	// Token: 0x04003444 RID: 13380
	private float copSpawnPos;

	// Token: 0x04003445 RID: 13381
	private List<RumRunnersLevelBouncingBeetle> beetleList = new List<RumRunnersLevelBouncingBeetle>();

	// Token: 0x020007A0 RID: 1952
	private enum SummonType
	{
		// Token: 0x0400344B RID: 13387
		Grubs,
		// Token: 0x0400344C RID: 13388
		Mine,
		// Token: 0x0400344D RID: 13389
		Bouncing,
		// Token: 0x0400344E RID: 13390
		None
	}
}
