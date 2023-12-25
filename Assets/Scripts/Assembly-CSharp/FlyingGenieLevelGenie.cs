using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000668 RID: 1640
public class FlyingGenieLevelGenie : LevelProperties.FlyingGenie.Entity
{
	// Token: 0x17000393 RID: 915
	// (get) Token: 0x0600222B RID: 8747 RVA: 0x0013E30C File Offset: 0x0013C70C
	// (set) Token: 0x0600222C RID: 8748 RVA: 0x0013E314 File Offset: 0x0013C714
	public FlyingGenieLevelGenie.State state { get; private set; }

	// Token: 0x0600222D RID: 8749 RVA: 0x0013E320 File Offset: 0x0013C720
	protected override void Awake()
	{
		base.Awake();
		this.defaultColor = base.GetComponent<SpriteRenderer>().color;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x0600222E RID: 8750 RVA: 0x0013E374 File Offset: 0x0013C774
	private void Start()
	{
		base.StartCoroutine(this.intro_cr());
		this.casketStartPos = this.casket.transform.position;
		base.GetComponent<Collider2D>().enabled = false;
		this.hiero.speed = base.properties.CurrentState.obelisk.obeliskMovementSpeed;
		this.brick.speed = base.properties.CurrentState.obelisk.obeliskMovementSpeed;
	}

	// Token: 0x0600222F RID: 8751 RVA: 0x0013E3F0 File Offset: 0x0013C7F0
	public override void LevelInit(LevelProperties.FlyingGenie properties)
	{
		base.LevelInit(properties);
		this.treasureAttacks = new List<int>
		{
			0,
			1,
			2
		};
		this.swordPinkPattern = properties.CurrentState.swords.swordPinkString.Split(new char[]
		{
			','
		});
		this.swordPinkIndex = UnityEngine.Random.Range(0, this.swordPinkPattern.Length);
		this.gemPinkPattern = properties.CurrentState.gems.gemPinkString.Split(new char[]
		{
			','
		});
		this.gemPinkIndex = UnityEngine.Random.Range(0, this.gemPinkPattern.Length);
		this.sphinxPinkPattern = properties.CurrentState.sphinx.scarabPinkString.Split(new char[]
		{
			','
		});
		this.sphinxPinkIndex = UnityEngine.Random.Range(0, this.sphinxPinkPattern.Length);
	}

	// Token: 0x06002230 RID: 8752 RVA: 0x0013E4D4 File Offset: 0x0013C8D4
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
	}

	// Token: 0x06002231 RID: 8753 RVA: 0x0013E4E7 File Offset: 0x0013C8E7
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002232 RID: 8754 RVA: 0x0013E505 File Offset: 0x0013C905
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002233 RID: 8755 RVA: 0x0013E520 File Offset: 0x0013C920
	private IEnumerator intro_cr()
	{
		this.state = FlyingGenieLevelGenie.State.Intro;
		yield return CupheadTime.WaitForSeconds(this, 1.3f);
		base.animator.SetTrigger("Continue");
		this.GenieIntroSFX();
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.main.introHesitate);
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_End", false, true);
		this.state = FlyingGenieLevelGenie.State.Idle;
		this.StartTreasure();
		base.StartCoroutine(this.skull_attack_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06002234 RID: 8756 RVA: 0x0013E53B File Offset: 0x0013C93B
	private void SpawnPuff()
	{
		base.StartCoroutine(this.handle_puff_cr(this.puffEffect.Create(this.puffRoot.position)));
	}

	// Token: 0x06002235 RID: 8757 RVA: 0x0013E560 File Offset: 0x0013C960
	private void StartCarpet()
	{
		base.animator.Play("Idle_Carpet");
		base.StartCoroutine(this.handle_carpet_fadein());
	}

	// Token: 0x06002236 RID: 8758 RVA: 0x0013E57F File Offset: 0x0013C97F
	private void EndCarpet()
	{
		base.StartCoroutine(this.handle_carpet_fadeout());
	}

	// Token: 0x06002237 RID: 8759 RVA: 0x0013E590 File Offset: 0x0013C990
	private IEnumerator handle_puff_cr(Effect puff)
	{
		yield return puff.animator.WaitForAnimationToEnd(this, "Start", false, true);
		SpriteRenderer puffRenderer = puff.GetComponent<SpriteRenderer>();
		while (puff.transform.position.x > -740f)
		{
			puff.transform.position -= Vector3.right * 200f * CupheadTime.Delta;
			if (puff.transform.position.x < -540f)
			{
				Color color = puffRenderer.color;
				color.a -= 1f * CupheadTime.Delta;
				puffRenderer.color = color;
			}
			yield return null;
		}
		UnityEngine.Object.Destroy(puff.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002238 RID: 8760 RVA: 0x0013E5B4 File Offset: 0x0013C9B4
	private IEnumerator handle_carpet_fadein()
	{
		this.carpet.color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			this.carpet.color = new Color(1f, 1f, 1f, t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.carpet.color = new Color(1f, 1f, 1f, 1f);
		yield return null;
		yield break;
	}

	// Token: 0x06002239 RID: 8761 RVA: 0x0013E5D0 File Offset: 0x0013C9D0
	private IEnumerator handle_carpet_fadeout()
	{
		this.carpet.color = new Color(1f, 1f, 1f, 1f);
		float t = 0f;
		float time = 2f;
		while (t < time)
		{
			this.carpet.color = new Color(1f, 1f, 1f, 1f - t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.carpet.color = new Color(1f, 1f, 1f, 0f);
		yield return null;
		yield break;
	}

	// Token: 0x0600223A RID: 8762 RVA: 0x0013E5EB File Offset: 0x0013C9EB
	public void HitTrigger()
	{
		this.attackLooping = false;
	}

	// Token: 0x0600223B RID: 8763 RVA: 0x0013E5F4 File Offset: 0x0013C9F4
	public void StartTreasure()
	{
		this.state = FlyingGenieLevelGenie.State.Treasure;
		this.skullCounter = 0;
		base.animator.SetBool("OnTreasure", true);
		this.attackLooping = true;
		int num = UnityEngine.Random.Range(0, this.treasureAttacks.Count);
		this.treasureCounter = this.treasureAttacks[num];
		switch (this.treasureCounter)
		{
		case 0:
			this.StartSwords();
			break;
		case 1:
			this.StartGems();
			break;
		case 2:
			this.StartSphinx();
			break;
		default:
			global::Debug.LogError("The counter is messed up: " + this.treasureCounter, null);
			break;
		}
		this.treasureAttacks.Remove(num);
	}

	// Token: 0x0600223C RID: 8764 RVA: 0x0013E6B8 File Offset: 0x0013CAB8
	private IEnumerator skull_attack_cr()
	{
		for (;;)
		{
			if (base.animator.GetCurrentAnimatorStateInfo(0).IsName("Chest_Idle") && this.skullCounter < base.properties.CurrentState.skull.skullCount)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.skull.skullDelayRange);
				base.animator.SetTrigger("OnSkull");
				AudioManager.Play("genie_skull_release");
				this.emitAudioFromObject.Add("genie_skull_release");
				yield return base.animator.WaitForAnimationToEnd(this, "Chest_Skull_Attack", false, true);
				this.skullCounter++;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600223D RID: 8765 RVA: 0x0013E6D4 File Offset: 0x0013CAD4
	private void SpawnSkull()
	{
		this.skullPrefab.Create(this.skullRoot.transform.position, 0f, -base.properties.CurrentState.skull.skullSpeed);
		AudioManager.Play("genie_skull_release_projectile");
		this.emitAudioFromObject.Add("genie_skull_release_projectile");
	}

	// Token: 0x0600223E RID: 8766 RVA: 0x0013E737 File Offset: 0x0013CB37
	private void DisableCarpet()
	{
		base.animator.Play("Off");
	}

	// Token: 0x0600223F RID: 8767 RVA: 0x0013E749 File Offset: 0x0013CB49
	private void EnableCarpet()
	{
		base.animator.Play("Idle_Carpet");
	}

	// Token: 0x06002240 RID: 8768 RVA: 0x0013E75B File Offset: 0x0013CB5B
	private void EnableChestIdle()
	{
		base.animator.Play("Idle_Carpet_Chest");
	}

	// Token: 0x06002241 RID: 8769 RVA: 0x0013E76D File Offset: 0x0013CB6D
	public void StartSwords()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.swords_cr());
	}

	// Token: 0x06002242 RID: 8770 RVA: 0x0013E798 File Offset: 0x0013CB98
	private IEnumerator swords_cr()
	{
		this.attackLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Intro", false, true);
		LevelProperties.FlyingGenie.Swords p = base.properties.CurrentState.swords;
		int positionIndex = UnityEngine.Random.Range(0, p.patternPositionStrings.Length);
		string[] positionPattern = p.patternPositionStrings[positionIndex].Split(new char[]
		{
			','
		});
		Vector3 endPosition = Vector3.zero;
		float location = 0f;
		while (this.attackLooping)
		{
			positionPattern = p.patternPositionStrings[positionIndex].Split(new char[]
			{
				','
			});
			for (int i = 0; i < positionPattern.Length; i++)
			{
				string[] coordinates = positionPattern[i].Split(new char[]
				{
					'-'
				});
				for (int j = 0; j < coordinates.Length; j++)
				{
					Parser.FloatTryParse(coordinates[j], out location);
					if (j % 2 == 0)
					{
						endPosition.x = -640f + location;
					}
					else
					{
						endPosition.y = 360f - location;
					}
				}
				this.SpawnSwords(endPosition);
				if (!this.attackLooping)
				{
					break;
				}
				yield return CupheadTime.WaitForSeconds(this, p.spawnDelay);
			}
			if (!this.attackLooping)
			{
				break;
			}
			yield return CupheadTime.WaitForSeconds(this, p.repeatDelay);
			positionIndex = (positionIndex + 1) % p.patternPositionStrings.Length;
			yield return null;
		}
		base.animator.SetBool("OnTreasure", false);
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Outro", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002243 RID: 8771 RVA: 0x0013E7B4 File Offset: 0x0013CBB4
	private void SpawnSwords(Vector3 pos)
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		FlyingGenieLevelSword flyingGenieLevelSword = UnityEngine.Object.Instantiate<FlyingGenieLevelSword>(this.swordPrefab);
		flyingGenieLevelSword.Init(this.treasureRoot.position, pos, base.properties.CurrentState.swords, next);
		flyingGenieLevelSword.SetParryable(this.swordPinkPattern[this.swordPinkIndex][0] == 'P');
		this.swordPinkIndex = (this.swordPinkIndex + 1) % this.swordPinkPattern.Length;
	}

	// Token: 0x06002244 RID: 8772 RVA: 0x0013E82A File Offset: 0x0013CC2A
	public void StartGems()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.gems_cr());
	}

	// Token: 0x06002245 RID: 8773 RVA: 0x0013E858 File Offset: 0x0013CC58
	private IEnumerator gems_cr()
	{
		this.attackLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Intro", false, true);
		AudioManager.Play("genie_chest_jewel_escape");
		this.emitAudioFromObject.Add("genie_chest_jewel_escape");
		AudioManager.PlayLoop("genie_chest_magic_loop");
		this.emitAudioFromObject.Add("genie_chest_magic_loop");
		while (this.attackLooping)
		{
			this.smallGemTimerUp = false;
			this.bigGemTimerUp = false;
			if (this.bigGemsRoutine != null)
			{
				base.StopCoroutine(this.bigGemsRoutine);
			}
			this.bigGemsRoutine = base.StartCoroutine(this.big_gems_cr());
			if (this.smallGemsRoutine != null)
			{
				base.StopCoroutine(this.smallGemsRoutine);
			}
			this.smallGemsRoutine = base.StartCoroutine(this.small_gems_cr());
			while (!this.smallGemTimerUp && !this.bigGemTimerUp)
			{
				if (!this.attackLooping)
				{
					break;
				}
				yield return null;
			}
			if (this.attackLooping)
			{
				yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.repeatDelay);
			}
			yield return null;
		}
		base.animator.SetBool("OnTreasure", false);
		yield return base.animator.WaitForAnimationToStart(this, "Chest_Outro", false);
		AudioManager.Stop("genie_chest_magic_loop");
		AudioManager.Play("genie_chest_magic_loop_end");
		this.emitAudioFromObject.Add("genie_chest_magic_loop_end");
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Outro", false, true);
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x06002246 RID: 8774 RVA: 0x0013E874 File Offset: 0x0013CC74
	private IEnumerator small_gems_cr()
	{
		LevelProperties.FlyingGenie.Gems p = base.properties.CurrentState.gems;
		this.smallGemTimerUp = false;
		int mainOffsetIndex = UnityEngine.Random.Range(0, p.gemSmallAimOffset.Length);
		string[] smallOffsetString = p.gemSmallAimOffset[mainOffsetIndex].Split(new char[]
		{
			','
		});
		int offsetIndex = UnityEngine.Random.Range(0, smallOffsetString.Length);
		float offset = 0f;
		base.StartCoroutine(this.small_gem_timer_cr());
		while (!this.smallGemTimerUp && this.attackLooping)
		{
			smallOffsetString = p.gemSmallAimOffset[mainOffsetIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(smallOffsetString[offsetIndex], out offset);
			AbstractPlayerController player = PlayerManager.GetNext();
			this.gemPrefab.Create(this.treasureRoot.position, player, offset, p.gemSmallSpeed, this.gemPinkPattern[this.gemPinkIndex][0] == 'P', false);
			this.gemPinkIndex = (this.gemPinkIndex + 1) % this.gemPinkPattern.Length;
			yield return CupheadTime.WaitForSeconds(this, p.gemSmallDelayRange.RandomFloat());
			if (offsetIndex < smallOffsetString.Length - 1)
			{
				offsetIndex++;
			}
			else
			{
				mainOffsetIndex = (mainOffsetIndex + 1) % p.gemSmallAimOffset.Length;
				offsetIndex = 0;
			}
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002247 RID: 8775 RVA: 0x0013E890 File Offset: 0x0013CC90
	private IEnumerator small_gem_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.gemSmallAttackDuration);
		this.smallGemTimerUp = true;
		yield break;
	}

	// Token: 0x06002248 RID: 8776 RVA: 0x0013E8AC File Offset: 0x0013CCAC
	private IEnumerator big_gems_cr()
	{
		LevelProperties.FlyingGenie.Gems p = base.properties.CurrentState.gems;
		this.bigGemTimerUp = false;
		int mainOffsetIndex = UnityEngine.Random.Range(0, p.gemBigAimOffset.Length);
		string[] bigOffsetString = p.gemBigAimOffset[mainOffsetIndex].Split(new char[]
		{
			','
		});
		int offsetIndex = UnityEngine.Random.Range(0, bigOffsetString.Length);
		float offset = 0f;
		base.StartCoroutine(this.big_gems_timer_cr());
		while (!this.bigGemTimerUp && this.attackLooping)
		{
			Parser.FloatTryParse(bigOffsetString[offsetIndex], out offset);
			AbstractPlayerController player = PlayerManager.GetNext();
			this.gemPrefab.Create(this.treasureRoot.position, player, offset, p.gemBigSpeed, false, true);
			yield return CupheadTime.WaitForSeconds(this, p.gemBigDelayRange.RandomFloat());
			if (offsetIndex < bigOffsetString.Length - 1)
			{
				offsetIndex++;
			}
			else
			{
				mainOffsetIndex = (mainOffsetIndex + 1) % p.gemBigAimOffset.Length;
				offsetIndex = 0;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002249 RID: 8777 RVA: 0x0013E8C8 File Offset: 0x0013CCC8
	private IEnumerator big_gems_timer_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.gems.gemBigAttackDuration);
		this.bigGemTimerUp = true;
		yield break;
	}

	// Token: 0x0600224A RID: 8778 RVA: 0x0013E8E3 File Offset: 0x0013CCE3
	public void StartSphinx()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		this.patternCoroutine = base.StartCoroutine(this.sphinx_cr());
	}

	// Token: 0x0600224B RID: 8779 RVA: 0x0013E910 File Offset: 0x0013CD10
	private IEnumerator sphinx_cr()
	{
		this.attackLooping = true;
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Intro", false, true);
		AudioManager.Play("genie_chest_jewel_escape");
		this.emitAudioFromObject.Add("genie_chest_jewel_escape");
		AudioManager.PlayLoop("genie_chest_magic_loop_nojingle");
		this.emitAudioFromObject.Add("genie_chest_magic_loop_nojingle");
		LevelProperties.FlyingGenie.Sphinx p = base.properties.CurrentState.sphinx;
		int mainCountIndex = UnityEngine.Random.Range(0, p.sphinxCount.Length);
		string[] sphinxCountPattern = p.sphinxCount[mainCountIndex].Split(new char[]
		{
			','
		});
		int countIndex = UnityEngine.Random.Range(0, sphinxCountPattern.Length);
		int mainXIndex = UnityEngine.Random.Range(0, p.sphinxAimX.Length);
		string[] sphinxPosXPattern = p.sphinxAimX[mainXIndex].Split(new char[]
		{
			','
		});
		int posXIndex = UnityEngine.Random.Range(0, sphinxPosXPattern.Length);
		int mainYIndex = UnityEngine.Random.Range(0, p.sphinxAimY.Length);
		string[] sphinxPosYPattern = p.sphinxAimY[mainYIndex].Split(new char[]
		{
			','
		});
		int posYIndex = UnityEngine.Random.Range(0, sphinxPosYPattern.Length);
		float sphinxCount = 0f;
		while (this.attackLooping)
		{
			sphinxCountPattern = p.sphinxCount[mainCountIndex].Split(new char[]
			{
				','
			});
			sphinxPosXPattern = p.sphinxAimX[mainXIndex].Split(new char[]
			{
				','
			});
			sphinxPosYPattern = p.sphinxAimY[mainYIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(sphinxCountPattern[countIndex], out sphinxCount);
			int i = 0;
			while ((float)i < sphinxCount)
			{
				this.SpawnSphinx();
				yield return CupheadTime.WaitForSeconds(this, p.sphinxMainDelay);
				if (posXIndex < p.sphinxAimX.Length - 1)
				{
					posXIndex++;
				}
				else
				{
					mainXIndex = (mainXIndex + 1) % p.sphinxAimX.Length;
					posXIndex = 0;
				}
				if (posYIndex < p.sphinxAimY.Length - 1)
				{
					posYIndex++;
				}
				else
				{
					mainYIndex = (mainYIndex + 1) % p.sphinxAimY.Length;
					posYIndex = 0;
				}
				if (!this.attackLooping)
				{
					break;
				}
				i++;
			}
			if (this.attackLooping)
			{
				yield return CupheadTime.WaitForSeconds(this, p.repeatDelay);
			}
			if (countIndex < p.sphinxCount.Length - 1)
			{
				countIndex++;
			}
			else
			{
				mainCountIndex = (mainCountIndex + 1) % p.sphinxCount.Length;
				countIndex = 0;
			}
		}
		base.animator.SetBool("OnTreasure", false);
		yield return base.animator.WaitForAnimationToStart(this, "Chest_Outro", false);
		AudioManager.Stop("genie_chest_magic_loop_nojingle");
		AudioManager.Play("genie_chest_magic_loop_nojingle_end");
		this.emitAudioFromObject.Add("genie_chest_magic_loop_nojingle_end");
		yield return base.animator.WaitForAnimationToEnd(this, "Chest_Outro", false, true);
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600224C RID: 8780 RVA: 0x0013E92C File Offset: 0x0013CD2C
	private void SpawnSphinx()
	{
		AbstractPlayerController next = PlayerManager.GetNext();
		FlyingGenieLevelSphinx flyingGenieLevelSphinx = UnityEngine.Object.Instantiate<FlyingGenieLevelSphinx>(this.sphinxPrefab);
		flyingGenieLevelSphinx.Init(this.treasureRoot.position, base.properties.CurrentState.sphinx, next, this.sphinxPinkPattern, this.sphinxPinkIndex);
		this.sphinxPinkIndex = (this.sphinxPinkIndex + (int)base.properties.CurrentState.sphinx.sphinxSpawnNum) % this.sphinxPinkPattern.Length;
	}

	// Token: 0x0600224D RID: 8781 RVA: 0x0013E9A5 File Offset: 0x0013CDA5
	public void StartCoffin()
	{
		this.state = FlyingGenieLevelGenie.State.Coffin;
		base.animator.SetBool("OnDisappear", true);
		base.StartCoroutine(this.coffin_cr());
	}

	// Token: 0x0600224E RID: 8782 RVA: 0x0013E9CC File Offset: 0x0013CDCC
	private IEnumerator coffin_cr()
	{
		this.attackLooping = true;
		LevelProperties.FlyingGenie.Coffin p = base.properties.CurrentState.coffin;
		int mainPosIndex = UnityEngine.Random.Range(0, p.mummyAppearString.Length);
		string[] coffinPosPattern = p.mummyAppearString[mainPosIndex].Split(new char[]
		{
			','
		});
		int posIndex = UnityEngine.Random.Range(0, coffinPosPattern.Length);
		int mainAngleIndex = UnityEngine.Random.Range(0, p.mummyGenieDirection.Length);
		string[] coffinAnglePattern = p.mummyGenieDirection[mainAngleIndex].Split(new char[]
		{
			','
		});
		int angleIndex = UnityEngine.Random.Range(0, coffinAnglePattern.Length);
		int mainTypeIndex = UnityEngine.Random.Range(0, p.mummyTypeString.Length);
		string[] coffinTypePattern = p.mummyTypeString[mainTypeIndex].Split(new char[]
		{
			','
		});
		int typeIndex = UnityEngine.Random.Range(0, coffinTypePattern.Length);
		Vector3 pos = Vector3.zero;
		float position = 0f;
		float angle = 0f;
		int sortingOrder = 0;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AudioManager.Play("genie_sarcophagus_enter");
		this.emitAudioFromObject.Add("genie_sarcophagus_enter");
		while (this.casket.transform.position.y > -30f)
		{
			this.casket.transform.AddPosition(0f, -800f * CupheadTime.Delta, 0f);
			yield return null;
		}
		CupheadLevelCamera.Current.Shake(10f, 0.4f, false);
		this.casket.GetComponent<Animator>().SetTrigger("StartCasket");
		yield return this.casket.GetComponent<Animator>().WaitForAnimationToEnd(this, "Open_Start", false, true);
		this.goop.ActivateGoop();
		yield return this.goop.GetComponent<Animator>().WaitForAnimationToEnd(this, "Intro", false, true);
		while (this.attackLooping && base.properties.CurrentState.stateName == LevelProperties.FlyingGenie.States.Disappear)
		{
			coffinPosPattern = p.mummyAppearString[mainPosIndex].Split(new char[]
			{
				','
			});
			coffinTypePattern = p.mummyTypeString[mainTypeIndex].Split(new char[]
			{
				','
			});
			coffinAnglePattern = p.mummyGenieDirection[mainAngleIndex].Split(new char[]
			{
				','
			});
			Parser.FloatTryParse(coffinPosPattern[posIndex], out position);
			Parser.FloatTryParse(coffinAnglePattern[angleIndex], out angle);
			pos = new Vector3(this.casket.transform.position.x + 200f, position, 0f);
			if (coffinTypePattern[typeIndex][0] == 'A')
			{
				this.mummyClassic.Create(pos, -p.mummyASpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Classic, p.mummyGenieHP, sortingOrder);
			}
			else if (coffinTypePattern[typeIndex][0] == 'B')
			{
				this.mummyChomper.Create(pos, -p.mummyBSpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Chomper, p.mummyGenieHP, sortingOrder);
			}
			else if (coffinTypePattern[typeIndex][0] == 'C')
			{
				this.mummyChaser.Create(pos, -p.mummyCSpeed, -angle, p, FlyingGenieLevelMummy.MummyType.Grabby, p.mummyGenieHP, sortingOrder);
			}
			yield return CupheadTime.WaitForSeconds(this, p.mummyGenieDelay);
			if (posIndex < coffinPosPattern.Length - 1)
			{
				posIndex++;
			}
			else
			{
				mainPosIndex = (mainPosIndex + 1) % p.mummyAppearString.Length;
				posIndex = 0;
			}
			if (typeIndex < coffinTypePattern.Length - 1)
			{
				typeIndex++;
			}
			else
			{
				mainTypeIndex = (mainTypeIndex + 1) % p.mummyTypeString.Length;
				typeIndex = 0;
			}
			if (angleIndex < coffinAnglePattern.Length - 1)
			{
				angleIndex++;
			}
			else
			{
				mainAngleIndex = (mainAngleIndex + 1) % p.mummyGenieDirection.Length;
				angleIndex = 0;
			}
			sortingOrder += 2;
		}
		this.goop.StartDeath();
		LevelBossDeathExploder explosion = this.casket.GetComponent<LevelBossDeathExploder>();
		explosion.StartExplosion();
		this.casket.GetComponent<Animator>().SetTrigger("OnClose");
		AudioManager.Play("genie_sarcophagus_exit");
		this.emitAudioFromObject.Add("genie_sarcophagus_exit");
		yield return this.casket.GetComponent<Animator>().WaitForAnimationToEnd(this, "Close", false, true);
		while (this.casket.transform.position.x < 1140f)
		{
			this.casket.transform.AddPosition(200f * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		this.casket.transform.position = this.casketStartPos;
		this.casket.GetComponent<Animator>().SetTrigger("EndCasket");
		explosion.StopExplosions();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		this.FadeIntoIdle();
		yield return CupheadTime.WaitForSeconds(this, p.hesitate);
		this.state = FlyingGenieLevelGenie.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x0600224F RID: 8783 RVA: 0x0013E9E8 File Offset: 0x0013CDE8
	public void StartObelisk()
	{
		if (this.patternCoroutine != null)
		{
			base.StopCoroutine(this.patternCoroutine);
		}
		if (this.bigGemsRoutine != null)
		{
			base.StopCoroutine(this.bigGemsRoutine);
		}
		if (this.smallGemsRoutine != null)
		{
			base.StopCoroutine(this.smallGemsRoutine);
		}
		this.state = FlyingGenieLevelGenie.State.Disappear;
		base.animator.SetBool("OnDisappear", true);
		base.StartCoroutine(this.obelisk_cr());
		base.StartCoroutine(this.genie_laugh_sound_cr());
	}

	// Token: 0x06002250 RID: 8784 RVA: 0x0013EA6C File Offset: 0x0013CE6C
	private IEnumerator obelisk_cr()
	{
		LevelProperties.FlyingGenie.Obelisk p = base.properties.CurrentState.obelisk;
		this.attackLooping = true;
		Vector3 startPos = Vector3.zero;
		startPos.x = 1340f;
		startPos.y = 360f;
		float t = 0f;
		float time = 1f;
		float angle = 0f;
		bool firstPillar = true;
		this.obelisks = new List<FlyingGenieLevelObelisk>();
		int obelisksListIndex = 0;
		int obeliskPoolSize = 6;
		int obeliskCounter = 0;
		int mainObeliskIndex = UnityEngine.Random.Range(0, p.obeliskGeniePos.Length);
		string[] blockOrderPattern = p.obeliskGeniePos[mainObeliskIndex].Split(new char[]
		{
			','
		});
		int obeliskIndex = UnityEngine.Random.Range(0, blockOrderPattern.Length);
		int mainBouncerIndex = UnityEngine.Random.Range(0, p.bouncerAngleString.Length);
		string[] bouncerPattern = p.bouncerAngleString[mainBouncerIndex].Split(new char[]
		{
			','
		});
		int bouncerIndex = UnityEngine.Random.Range(0, bouncerPattern.Length);
		for (int i = 0; i < obeliskPoolSize; i++)
		{
			FlyingGenieLevelObelisk flyingGenieLevelObelisk = UnityEngine.Object.Instantiate<FlyingGenieLevelObelisk>(this.obeliskPrefab);
			flyingGenieLevelObelisk.Init(startPos, p, this, i == 0);
			this.obelisks.Add(flyingGenieLevelObelisk);
		}
		yield return base.animator.WaitForAnimationToStart(this, "Genie_Meditate", false);
		this.sawMask.gameObject.SetActive(true);
		while (t < time)
		{
			Vector3 pos = this.hieroBG.position;
			Vector3 pos2 = this.brickBG.position;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInBounce, 0f, 1f, t / time);
			pos.y = Mathf.Lerp(this.hieroBG.position.y, 340f, val);
			pos2.y = Mathf.Lerp(this.brickBG.position.y, -320f, val);
			this.hieroBG.position = pos;
			this.brickBG.position = pos2;
			t += CupheadTime.Delta;
			yield return null;
		}
		while (obeliskCounter < p.obeliskCount)
		{
			Parser.FloatTryParse(bouncerPattern[bouncerIndex], out angle);
			string[] headLocations = blockOrderPattern[obeliskIndex].Split(new char[]
			{
				'-'
			});
			this.obelisks[obelisksListIndex].ActivateObelisk(headLocations);
			if (p.bounceShotOn)
			{
				if (!firstPillar)
				{
					int index;
					if (obelisksListIndex <= 0)
					{
						index = this.obelisks.Count - 1;
					}
					else
					{
						index = obelisksListIndex - 1;
					}
					this.SpawnBouncer(this.obelisks[obelisksListIndex], this.obelisks[index], angle);
				}
				else
				{
					float num = Vector3.Distance(this.obelisks[obelisksListIndex + 1].transform.position, base.transform.position);
					this.obelisks[obelisksListIndex].SetColliders((this.obelisks[obelisksListIndex + 1].transform.position.x + Mathf.Abs(num / 2f)) / 2f, base.transform.position.x - num / 2f);
					firstPillar = false;
				}
			}
			obelisksListIndex = (obelisksListIndex + 1) % this.obelisks.Count;
			yield return null;
			yield return CupheadTime.WaitForSeconds(this, p.obeliskAppearDelay);
			if (obeliskIndex < blockOrderPattern.Length - 1)
			{
				obeliskIndex++;
			}
			else
			{
				mainObeliskIndex = (mainObeliskIndex + 1) % p.obeliskGeniePos.Length;
				obeliskIndex = 0;
			}
			if (bouncerIndex < bouncerPattern.Length - 1)
			{
				bouncerIndex++;
			}
			else
			{
				mainBouncerIndex = (mainBouncerIndex + 1) % p.bouncerAngleString.Length;
				bouncerIndex = 0;
			}
			blockOrderPattern = p.obeliskGeniePos[mainObeliskIndex].Split(new char[]
			{
				','
			});
			bouncerPattern = p.bouncerAngleString[mainBouncerIndex].Split(new char[]
			{
				','
			});
			obeliskCounter++;
			yield return null;
		}
		foreach (FlyingGenieLevelObelisk obelisk in this.obelisks)
		{
			if (obelisk.isOn)
			{
				while (obelisk.transform.position.x > -640f)
				{
					yield return null;
				}
			}
		}
		AudioManager.Stop("genie_pillar_main_loop");
		AudioManager.Stop("genie_pillar_destructable_loop");
		this.sawMask.gameObject.SetActive(false);
		base.StartCoroutine(this.delete_obelisks_cr(this.obelisks));
		this.state = FlyingGenieLevelGenie.State.Idle;
		this.StartCoffin();
		yield return null;
		yield break;
	}

	// Token: 0x06002251 RID: 8785 RVA: 0x0013EA88 File Offset: 0x0013CE88
	private IEnumerator delete_obelisks_cr(List<FlyingGenieLevelObelisk> obelisks)
	{
		float t = 0f;
		float time = 2f;
		foreach (FlyingGenieLevelObelisk obelisk in obelisks)
		{
			if (obelisk.isOn)
			{
				while (obelisk.transform.position.x > -740f)
				{
					yield return null;
				}
			}
			UnityEngine.Object.Destroy(obelisk.gameObject);
			yield return null;
		}
		while (t < time)
		{
			Vector3 pos = this.hieroBG.position;
			Vector3 pos2 = this.brickBG.position;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeOutBounce, 0f, 1f, t / time);
			pos.y = Mathf.Lerp(this.hieroBG.position.y, 460f, val);
			pos2.y = Mathf.Lerp(this.brickBG.position.y, -460f, val);
			this.hieroBG.position = pos;
			this.brickBG.position = pos2;
			t += CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002252 RID: 8786 RVA: 0x0013EAAC File Offset: 0x0013CEAC
	private void SpawnBouncer(FlyingGenieLevelObelisk currentObelisk, FlyingGenieLevelObelisk lastObelisk, float angle)
	{
		float num = Vector3.Distance(lastObelisk.transform.position, currentObelisk.transform.position);
		Vector3 position = lastObelisk.transform.position;
		position.x = currentObelisk.transform.position.x - num / 2f;
		float num2 = 150f;
		float num3 = 180f - (90f + angle / 2f);
		currentObelisk.SetColliders((lastObelisk.transform.position.x + Mathf.Abs(num / 2f)) / 2f, currentObelisk.transform.position.x - num / 2f);
		position.y = UnityEngine.Random.Range((float)Level.Current.Ceiling - num2, (float)Level.Current.Ground + num2);
		FlyingGenieLevelBouncer flyingGenieLevelBouncer = UnityEngine.Object.Instantiate<FlyingGenieLevelBouncer>(this.bouncerPrefab).Init(position, base.properties.CurrentState.obelisk, -num3);
		flyingGenieLevelBouncer.transform.parent = currentObelisk.transform;
	}

	// Token: 0x06002253 RID: 8787 RVA: 0x0013EBC5 File Offset: 0x0013CFC5
	public void DoDamage(float damage)
	{
		base.properties.DealDamage(damage);
	}

	// Token: 0x06002254 RID: 8788 RVA: 0x0013EBD3 File Offset: 0x0013CFD3
	private void FadeIntoIdle()
	{
		base.StartCoroutine(this.handle_fade_in_idle());
	}

	// Token: 0x06002255 RID: 8789 RVA: 0x0013EBE4 File Offset: 0x0013CFE4
	private IEnumerator handle_fade_in_idle()
	{
		base.GetComponent<SpriteRenderer>().color = new Color(this.defaultColor.r, this.defaultColor.g, this.defaultColor.a, 0f);
		this.carpet.color = new Color(1f, 1f, 1f, 0f);
		float t = 0f;
		float time = 0.3f;
		base.animator.Play("To_Phase_2");
		while (t < time)
		{
			this.carpet.color = new Color(1f, 1f, 1f, t / time);
			base.GetComponent<SpriteRenderer>().color = new Color(this.defaultColor.r, this.defaultColor.g, this.defaultColor.a, t / time);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = new Color(this.defaultColor.r, this.defaultColor.g, this.defaultColor.a, 1f);
		this.carpet.color = new Color(1f, 1f, 1f, 1f);
		base.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002256 RID: 8790 RVA: 0x0013EBFF File Offset: 0x0013CFFF
	public void StartPhase2()
	{
		base.StartCoroutine(this.start_phase_2_cr());
	}

	// Token: 0x06002257 RID: 8791 RVA: 0x0013EC10 File Offset: 0x0013D010
	private IEnumerator start_phase_2_cr()
	{
		this.genieTransformed.animator.Play("Genie_Head_Roll");
		yield return new WaitForEndOfFrame();
		this.genieTransformed.StartMarionette(base.transform.position, this.meditateP1, this.meditateP2);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x06002258 RID: 8792 RVA: 0x0013EC2C File Offset: 0x0013D02C
	private void CreateMeditateFX()
	{
		PlanePlayerController player = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerOne);
		PlanePlayerController player2 = PlayerManager.GetPlayer<PlanePlayerController>(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.meditateP1 = UnityEngine.Object.Instantiate<FlyingGenieLevelMeditateFX>(this.meditateEffect);
			this.meditateP1.transform.position = player.transform.position;
			this.meditateP1.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
			this.meditateP1.transform.parent = player.transform;
		}
		if (player2 != null)
		{
			this.meditateP2 = UnityEngine.Object.Instantiate<FlyingGenieLevelMeditateFX>(this.meditateEffect);
			this.meditateP2.transform.position = player2.transform.position;
			this.meditateP2.transform.localScale = new Vector3(0.5f, 0.5f, 0f);
			this.meditateP2.transform.parent = player2.transform;
		}
	}

	// Token: 0x06002259 RID: 8793 RVA: 0x0013ED2B File Offset: 0x0013D12B
	private void GenieIntroSFX()
	{
		AudioManager.Play("genie_entrance");
		this.emitAudioFromObject.Add("genie_entrance");
	}

	// Token: 0x0600225A RID: 8794 RVA: 0x0013ED47 File Offset: 0x0013D147
	private void SoundGenieVoiceIntro()
	{
		AudioManager.Play("genie_voice_intro_intake");
		this.emitAudioFromObject.Add("genie_voice_intro_intake");
	}

	// Token: 0x0600225B RID: 8795 RVA: 0x0013ED63 File Offset: 0x0013D163
	private void SoundGenieVoiceEffort()
	{
		AudioManager.Play("genie_voice_effort");
		this.emitAudioFromObject.Add("genie_voice_effort");
	}

	// Token: 0x0600225C RID: 8796 RVA: 0x0013ED7F File Offset: 0x0013D17F
	private void SoundGenieVoiceLaugh()
	{
		AudioManager.Play("genie_voice_laugh");
		this.emitAudioFromObject.Add("genie_voice_laugh");
	}

	// Token: 0x0600225D RID: 8797 RVA: 0x0013ED9B File Offset: 0x0013D19B
	private void SoundGenieVoiceLure()
	{
		AudioManager.Play("genie_voice_lure");
		this.emitAudioFromObject.Add("genie_voice_lure");
	}

	// Token: 0x0600225E RID: 8798 RVA: 0x0013EDB7 File Offset: 0x0013D1B7
	private void SoundGenieVoiceMeditate()
	{
		AudioManager.Play("genie_voice_meditate");
		this.emitAudioFromObject.Add("genie_voice_meditate");
	}

	// Token: 0x0600225F RID: 8799 RVA: 0x0013EDD3 File Offset: 0x0013D1D3
	private void SoundGenieChestOpen()
	{
		AudioManager.Play("genie_chest_attack_open");
		this.emitAudioFromObject.Add("genie_chest_attack_open");
	}

	// Token: 0x06002260 RID: 8800 RVA: 0x0013EDF0 File Offset: 0x0013D1F0
	private IEnumerator genie_laugh_sound_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 6f);
		AudioManager.Play("genie_voice_laugh_reverb");
		yield break;
	}

	// Token: 0x06002261 RID: 8801 RVA: 0x0013EE0B File Offset: 0x0013D20B
	private void SoundGenieTeleportDisappear()
	{
		AudioManager.Play("genie_teleport_disappear");
		this.emitAudioFromObject.Add("genie_teleport_disappear");
	}

	// Token: 0x04002AD6 RID: 10966
	private const float MUMMY_SPAWN_OFFSET = 200f;

	// Token: 0x04002AD8 RID: 10968
	[SerializeField]
	private Transform hieroBG;

	// Token: 0x04002AD9 RID: 10969
	[SerializeField]
	private Transform brickBG;

	// Token: 0x04002ADA RID: 10970
	[SerializeField]
	private ScrollingSprite hiero;

	// Token: 0x04002ADB RID: 10971
	[SerializeField]
	private ScrollingSprite brick;

	// Token: 0x04002ADC RID: 10972
	[SerializeField]
	private Transform sawMask;

	// Token: 0x04002ADD RID: 10973
	[SerializeField]
	private GameObject casket;

	// Token: 0x04002ADE RID: 10974
	[SerializeField]
	private FlyingGenieLevelMeditateFX meditateEffect;

	// Token: 0x04002ADF RID: 10975
	[SerializeField]
	private BasicProjectile skullPrefab;

	// Token: 0x04002AE0 RID: 10976
	[SerializeField]
	private FlyingGenieLevelBouncer bouncerPrefab;

	// Token: 0x04002AE1 RID: 10977
	[SerializeField]
	private FlyingGenieLevelObelisk obeliskPrefab;

	// Token: 0x04002AE2 RID: 10978
	[SerializeField]
	private FlyingGenieLevelSphinx sphinxPrefab;

	// Token: 0x04002AE3 RID: 10979
	[Space(10f)]
	[SerializeField]
	private FlyingGenieLevelGem gemPrefab;

	// Token: 0x04002AE4 RID: 10980
	[Space(10f)]
	[SerializeField]
	private FlyingGenieLevelGoop goop;

	// Token: 0x04002AE5 RID: 10981
	[SerializeField]
	private FlyingGenieLevelMummy mummyClassic;

	// Token: 0x04002AE6 RID: 10982
	[SerializeField]
	private FlyingGenieLevelMummy mummyChomper;

	// Token: 0x04002AE7 RID: 10983
	[SerializeField]
	private FlyingGenieLevelMummy mummyChaser;

	// Token: 0x04002AE8 RID: 10984
	[SerializeField]
	private FlyingGenieLevelSword swordPrefab;

	// Token: 0x04002AE9 RID: 10985
	[SerializeField]
	private FlyingGenieLevelGenieTransform genieTransformed;

	// Token: 0x04002AEA RID: 10986
	[Space(10f)]
	[SerializeField]
	private Effect puffEffect;

	// Token: 0x04002AEB RID: 10987
	[SerializeField]
	private Transform puffRoot;

	// Token: 0x04002AEC RID: 10988
	[SerializeField]
	private Transform skullRoot;

	// Token: 0x04002AED RID: 10989
	[SerializeField]
	private SpriteRenderer carpet;

	// Token: 0x04002AEE RID: 10990
	[SerializeField]
	private Transform morphRoot;

	// Token: 0x04002AEF RID: 10991
	[SerializeField]
	private Transform treasureRoot;

	// Token: 0x04002AF0 RID: 10992
	private List<int> treasureAttacks;

	// Token: 0x04002AF1 RID: 10993
	private List<FlyingGenieLevelObelisk> obelisks;

	// Token: 0x04002AF2 RID: 10994
	private FlyingGenieLevelMeditateFX meditate;

	// Token: 0x04002AF3 RID: 10995
	private DamageReceiver damageReceiver;

	// Token: 0x04002AF4 RID: 10996
	private DamageDealer damageDealer;

	// Token: 0x04002AF5 RID: 10997
	private bool attackLooping;

	// Token: 0x04002AF6 RID: 10998
	private bool smallGemTimerUp;

	// Token: 0x04002AF7 RID: 10999
	private bool bigGemTimerUp;

	// Token: 0x04002AF8 RID: 11000
	private int skullCounter;

	// Token: 0x04002AF9 RID: 11001
	private int treasureCounter;

	// Token: 0x04002AFA RID: 11002
	private Vector3 casketStartPos;

	// Token: 0x04002AFB RID: 11003
	private Coroutine patternCoroutine;

	// Token: 0x04002AFC RID: 11004
	private Coroutine smallGemsRoutine;

	// Token: 0x04002AFD RID: 11005
	private Coroutine bigGemsRoutine;

	// Token: 0x04002AFE RID: 11006
	private FlyingGenieLevelMeditateFX meditateP1;

	// Token: 0x04002AFF RID: 11007
	private FlyingGenieLevelMeditateFX meditateP2;

	// Token: 0x04002B00 RID: 11008
	private Color defaultColor;

	// Token: 0x04002B01 RID: 11009
	private string[] swordPinkPattern;

	// Token: 0x04002B02 RID: 11010
	private int swordPinkIndex;

	// Token: 0x04002B03 RID: 11011
	private string[] gemPinkPattern;

	// Token: 0x04002B04 RID: 11012
	private int gemPinkIndex;

	// Token: 0x04002B05 RID: 11013
	private string[] sphinxPinkPattern;

	// Token: 0x04002B06 RID: 11014
	private int sphinxPinkIndex;

	// Token: 0x02000669 RID: 1641
	public enum State
	{
		// Token: 0x04002B08 RID: 11016
		Intro,
		// Token: 0x04002B09 RID: 11017
		Idle,
		// Token: 0x04002B0A RID: 11018
		Transform,
		// Token: 0x04002B0B RID: 11019
		Treasure,
		// Token: 0x04002B0C RID: 11020
		Disappear,
		// Token: 0x04002B0D RID: 11021
		Dead,
		// Token: 0x04002B0E RID: 11022
		Coffin
	}
}
