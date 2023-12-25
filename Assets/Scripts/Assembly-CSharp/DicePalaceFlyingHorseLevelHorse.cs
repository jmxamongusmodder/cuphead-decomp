using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005C1 RID: 1473
public class DicePalaceFlyingHorseLevelHorse : LevelProperties.DicePalaceFlyingHorse.Entity
{
	// Token: 0x17000362 RID: 866
	// (get) Token: 0x06001CB0 RID: 7344 RVA: 0x001069ED File Offset: 0x00104DED
	// (set) Token: 0x06001CB1 RID: 7345 RVA: 0x001069F5 File Offset: 0x00104DF5
	public DicePalaceFlyingHorseLevelHorse.MiniHorseType miniHorseType { get; private set; }

	// Token: 0x06001CB2 RID: 7346 RVA: 0x001069FE File Offset: 0x00104DFE
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06001CB3 RID: 7347 RVA: 0x00106A34 File Offset: 0x00104E34
	public override void LevelInit(LevelProperties.DicePalaceFlyingHorse properties)
	{
		base.LevelInit(properties);
		Level.Current.OnLevelStartEvent += this.StartAttacks;
		Level.Current.OnWinEvent += this.Death;
		this.giftPosXMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringX.Length);
		this.giftPosYMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringY.Length);
		this.giftPosXIndex = UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringX[this.giftPosXMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.giftPosYIndex = UnityEngine.Random.Range(0, properties.CurrentState.giftBombs.giftPositionStringY[this.giftPosYMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.playerAimMaxCounter = properties.CurrentState.giftBombs.playerAimRange.RandomInt();
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001CB4 RID: 7348 RVA: 0x00106B42 File Offset: 0x00104F42
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06001CB5 RID: 7349 RVA: 0x00106B55 File Offset: 0x00104F55
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001CB6 RID: 7350 RVA: 0x00106B73 File Offset: 0x00104F73
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001CB7 RID: 7351 RVA: 0x00106B8C File Offset: 0x00104F8C
	private IEnumerator intro_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 2f);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("dice_palace_flying_horse_intro");
		this.emitAudioFromObject.Add("dice_palace_flying_horse_intro");
		yield return null;
		yield break;
	}

	// Token: 0x06001CB8 RID: 7352 RVA: 0x00106BA7 File Offset: 0x00104FA7
	private void StartAttacks()
	{
		base.StartCoroutine(this.presents_cr());
		base.StartCoroutine(this.mini_horses_cr());
	}

	// Token: 0x06001CB9 RID: 7353 RVA: 0x00106BC3 File Offset: 0x00104FC3
	private void SpawnPresent()
	{
		base.StartCoroutine(this.spawn_present_cr());
	}

	// Token: 0x06001CBA RID: 7354 RVA: 0x00106BD4 File Offset: 0x00104FD4
	private IEnumerator spawn_present_cr()
	{
		LevelProperties.DicePalaceFlyingHorse.GiftBombs p = base.properties.CurrentState.giftBombs;
		float positionX = 0f;
		float positionY = 0f;
		Vector3 endPos = Vector3.zero;
		AbstractPlayerController player = PlayerManager.GetNext();
		string[] giftPositionXPattern = p.giftPositionStringX[this.giftPosXMainIndex].Split(new char[]
		{
			','
		});
		string[] giftPositionYPattern = p.giftPositionStringY[this.giftPosYMainIndex].Split(new char[]
		{
			','
		});
		if (this.playerAimCounter >= this.playerAimMaxCounter)
		{
			endPos = player.transform.position;
			player = PlayerManager.GetNext();
			this.playerAimMaxCounter = p.playerAimRange.RandomInt();
			this.playerAimCounter = 0;
		}
		else
		{
			Parser.FloatTryParse(giftPositionXPattern[this.giftPosXIndex], out positionX);
			Parser.FloatTryParse(giftPositionYPattern[this.giftPosYIndex], out positionY);
			endPos.x = -640f + positionX;
			endPos.y = 360f - positionY;
			this.playerAimCounter++;
		}
		DicePalaceFlyingHorseLevelPresent present = UnityEngine.Object.Instantiate<DicePalaceFlyingHorseLevelPresent>(this.presentPrefab);
		present.Init(this.projectileRoot.position, endPos, base.properties.CurrentState.giftBombs);
		if (this.giftPosXIndex < giftPositionXPattern[this.giftPosXIndex].Length)
		{
			this.giftPosXIndex++;
		}
		else
		{
			this.giftPosXMainIndex = (this.giftPosXMainIndex + 1) % p.giftPositionStringX.Length;
			this.giftPosXIndex = 0;
		}
		if (this.giftPosYIndex < giftPositionYPattern[this.giftPosYIndex].Length)
		{
			this.giftPosYIndex++;
		}
		else
		{
			this.giftPosYMainIndex = (this.giftPosYMainIndex + 1) % p.giftPositionStringY.Length;
			this.giftPosYIndex = 0;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06001CBB RID: 7355 RVA: 0x00106BF0 File Offset: 0x00104FF0
	private IEnumerator presents_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.giftBombs.giftDelay);
			base.animator.SetTrigger("OnAttack");
			yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
			AudioManager.Play("dice_palace_flying_horse_attack");
			this.emitAudioFromObject.Add("dice_palace_flying_horse_attack");
			yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CBC RID: 7356 RVA: 0x00106C0B File Offset: 0x0010500B
	private void AttackVox()
	{
		AudioManager.Play("dice_palace_horse_vox");
		this.emitAudioFromObject.Add("dice_palace_horse_vox");
	}

	// Token: 0x06001CBD RID: 7357 RVA: 0x00106C27 File Offset: 0x00105027
	private void TrotSFX()
	{
		AudioManager.Play("dice_horse_trot");
		this.emitAudioFromObject.Add("dice_horse_trot");
	}

	// Token: 0x06001CBE RID: 7358 RVA: 0x00106C43 File Offset: 0x00105043
	private void DieSFX()
	{
		AudioManager.Play("dice_horse_death");
		this.emitAudioFromObject.Add("dice_horse_death");
	}

	// Token: 0x06001CBF RID: 7359 RVA: 0x00106C60 File Offset: 0x00105060
	private void SpawnMiniHorses(Vector3 startPos, DicePalaceFlyingHorseLevelMiniHorse prefab, DicePalaceFlyingHorseLevelHorse.MiniHorseType type, bool isPink, float threeProx, int lane)
	{
		LevelProperties.DicePalaceFlyingHorse.MiniHorses miniHorses = base.properties.CurrentState.miniHorses;
		AbstractPlayerController next = PlayerManager.GetNext();
		DicePalaceFlyingHorseLevelMiniHorse dicePalaceFlyingHorseLevelMiniHorse = UnityEngine.Object.Instantiate<DicePalaceFlyingHorseLevelMiniHorse>(prefab);
		Vector3 position;
		if (lane == 0)
		{
			position = this.topLineBackground.position;
		}
		else if (lane == 1)
		{
			position = this.middleLineBackground.position;
		}
		else
		{
			position = this.bottomLineBackground.position;
		}
		dicePalaceFlyingHorseLevelMiniHorse.Init(startPos, miniHorses.HP, base.properties.CurrentState.miniHorses, next, type, isPink, threeProx, lane, position);
	}

	// Token: 0x06001CC0 RID: 7360 RVA: 0x00106CF0 File Offset: 0x001050F0
	private IEnumerator mini_horses_cr()
	{
		LevelProperties.DicePalaceFlyingHorse.MiniHorses p = base.properties.CurrentState.miniHorses;
		string[] typePattern = p.miniTypeString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] delayPattern = p.delayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] pinkPattern = p.miniTwoPinkString.GetRandom<string>().Split(new char[]
		{
			','
		});
		string[] proxPattern = p.miniThreeProxString.GetRandom<string>().Split(new char[]
		{
			','
		});
		int typeIndex = UnityEngine.Random.Range(0, typePattern.Length);
		int delayIndex = UnityEngine.Random.Range(0, delayPattern.Length);
		int pinkIndex = UnityEngine.Random.Range(0, pinkPattern.Length);
		int proxIndex = UnityEngine.Random.Range(0, proxPattern.Length);
		int type = 0;
		int trackCounter = 0;
		int pinkCounter = 0;
		int maxPink = 0;
		int threeProximity = 0;
		float delay = 0f;
		bool isPink = false;
		int lane = 0;
		Vector3 position = base.transform.position;
		DicePalaceFlyingHorseLevelMiniHorse prefab = null;
		DicePalaceFlyingHorseLevelHorse.MiniHorseType getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.One;
		position.x = (float)Level.Current.Right + 100f;
		for (;;)
		{
			for (int i = typeIndex; i < typePattern.Length; i++)
			{
				Parser.IntTryParse(typePattern[i], out type);
				Parser.FloatTryParse(delayPattern[delayIndex], out delay);
				trackCounter++;
				if (trackCounter <= 1)
				{
					position.y = this.topLine.position.y;
					lane = 0;
				}
				else if (trackCounter == 2)
				{
					position.y = this.middleLine.position.y;
					lane = 1;
				}
				else if (trackCounter >= 3)
				{
					position.y = this.bottomLine.position.y;
					trackCounter = 0;
					lane = 2;
				}
				if (type != 1)
				{
					if (type != 2)
					{
						if (type == 3)
						{
							prefab = this.miniHorse3Prefab;
							getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.Three;
							Parser.IntTryParse(proxPattern[proxIndex], out threeProximity);
							proxIndex = (proxIndex + 1) % proxPattern.Length;
						}
					}
					else
					{
						prefab = this.miniHorse2Prefab;
						getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.Two;
						if (pinkCounter == 0)
						{
							isPink = false;
							Parser.IntTryParse(pinkPattern[pinkIndex], out maxPink);
							pinkIndex %= pinkPattern.Length;
							pinkCounter++;
						}
						else if (pinkCounter >= maxPink)
						{
							isPink = true;
							pinkCounter = 0;
						}
						else
						{
							isPink = false;
							pinkCounter++;
						}
					}
				}
				else
				{
					prefab = this.miniHorse1Prefab;
					getType = DicePalaceFlyingHorseLevelHorse.MiniHorseType.One;
				}
				this.SpawnMiniHorses(position, prefab, getType, isPink, (float)threeProximity, lane);
				yield return CupheadTime.WaitForSeconds(this, delay);
				delayIndex = (delayIndex + 1) % delayPattern.Length;
				i %= typePattern.Length;
				typeIndex = 0;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001CC1 RID: 7361 RVA: 0x00106D0C File Offset: 0x0010510C
	private void Death()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.SetTrigger("OnDeath");
		AudioManager.PlayLoop("dice_palace_flying_horse_death_loop");
		this.emitAudioFromObject.Add("dice_palace_flying_horse_death_loop");
		this.DieSFX();
	}

	// Token: 0x0400259A RID: 9626
	[SerializeField]
	private Transform bottomLine;

	// Token: 0x0400259B RID: 9627
	[SerializeField]
	private Transform middleLine;

	// Token: 0x0400259C RID: 9628
	[SerializeField]
	private Transform topLine;

	// Token: 0x0400259D RID: 9629
	[SerializeField]
	private Transform bottomLineBackground;

	// Token: 0x0400259E RID: 9630
	[SerializeField]
	private Transform middleLineBackground;

	// Token: 0x0400259F RID: 9631
	[SerializeField]
	private Transform topLineBackground;

	// Token: 0x040025A0 RID: 9632
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x040025A1 RID: 9633
	[SerializeField]
	private DicePalaceFlyingHorseLevelMiniHorse miniHorse1Prefab;

	// Token: 0x040025A2 RID: 9634
	[SerializeField]
	private DicePalaceFlyingHorseLevelMiniHorse miniHorse2Prefab;

	// Token: 0x040025A3 RID: 9635
	[SerializeField]
	private DicePalaceFlyingHorseLevelMiniHorse miniHorse3Prefab;

	// Token: 0x040025A4 RID: 9636
	[SerializeField]
	private DicePalaceFlyingHorseLevelPresent presentPrefab;

	// Token: 0x040025A5 RID: 9637
	private DamageDealer damageDealer;

	// Token: 0x040025A6 RID: 9638
	private DamageReceiver damageReceiver;

	// Token: 0x040025A7 RID: 9639
	private int giftPosYMainIndex;

	// Token: 0x040025A8 RID: 9640
	private int giftPosYIndex;

	// Token: 0x040025A9 RID: 9641
	private int giftPosXMainIndex;

	// Token: 0x040025AA RID: 9642
	private int giftPosXIndex;

	// Token: 0x040025AB RID: 9643
	private int playerAimMaxCounter;

	// Token: 0x040025AC RID: 9644
	private int playerAimCounter;

	// Token: 0x020005C2 RID: 1474
	public enum MiniHorseType
	{
		// Token: 0x040025AE RID: 9646
		One,
		// Token: 0x040025AF RID: 9647
		Two,
		// Token: 0x040025B0 RID: 9648
		Three
	}
}
