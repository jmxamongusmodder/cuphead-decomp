using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005F1 RID: 1521
public class DragonLevelLeftSideDragon : LevelProperties.Dragon.Entity
{
	// Token: 0x17000375 RID: 885
	// (get) Token: 0x06001E40 RID: 7744 RVA: 0x001168B8 File Offset: 0x00114CB8
	// (set) Token: 0x06001E41 RID: 7745 RVA: 0x001168C0 File Offset: 0x00114CC0
	public DragonLevelLeftSideDragon.State state { get; private set; }

	// Token: 0x06001E42 RID: 7746 RVA: 0x001168CC File Offset: 0x00114CCC
	protected override void Awake()
	{
		base.Awake();
		this.state = DragonLevelLeftSideDragon.State.UnSpawned;
		this.headPicked = DragonLevelLeftSideDragon.HeadPicked.None;
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = this.damageBox.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.middleHead.GetComponent<Collider2D>().enabled = false;
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = false;
		}
		this.damageReceiver.enabled = false;
		this.fire.SetColliderEnabled(false);
		this.xPos = base.transform.position.x;
		Vector3 position = base.transform.position;
		position.x = -10000f;
		base.transform.position = position;
	}

	// Token: 0x06001E43 RID: 7747 RVA: 0x001169B0 File Offset: 0x00114DB0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.dead)
		{
			return;
		}
		base.properties.DealDamage(info.damage);
		if (base.properties.CurrentHealth <= 0f && this.state != DragonLevelLeftSideDragon.State.Dead)
		{
			this.state = DragonLevelLeftSideDragon.State.Dead;
			this.StartDeath();
		}
	}

	// Token: 0x06001E44 RID: 7748 RVA: 0x00116A08 File Offset: 0x00114E08
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001E45 RID: 7749 RVA: 0x00116A20 File Offset: 0x00114E20
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E46 RID: 7750 RVA: 0x00116A4C File Offset: 0x00114E4C
	public override void LevelInit(LevelProperties.Dragon properties)
	{
		base.LevelInit(properties);
		this.potionTypeMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.potions.potionTypeString.Length);
		this.potionTypeIndex = UnityEngine.Random.Range(0, properties.CurrentState.potions.potionTypeString[this.potionTypeMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.shotPositionMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.potions.shotPositionString.Length);
		this.shotPositionIndex = UnityEngine.Random.Range(0, properties.CurrentState.potions.shotPositionString[this.shotPositionMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.attackCountMainIndex = UnityEngine.Random.Range(0, properties.CurrentState.potions.attackCount.Length);
		this.attackCountIndex = UnityEngine.Random.Range(0, properties.CurrentState.potions.attackCount[this.attackCountMainIndex].Split(new char[]
		{
			','
		}).Length);
		this.AttackFrames = 36 - (properties.CurrentState.blowtorch.warningDurationOne + properties.CurrentState.blowtorch.warningDurationTwo);
	}

	// Token: 0x06001E47 RID: 7751 RVA: 0x00116B83 File Offset: 0x00114F83
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.fireMarcherPrefabs = null;
		this.fireMarcherLeaderPrefab = null;
		this.bothPotionPrefab = null;
		this.horizontalPotionPrefab = null;
		this.verticalPotionPrefab = null;
	}

	// Token: 0x06001E48 RID: 7752 RVA: 0x00116BAE File Offset: 0x00114FAE
	public void StartIntro()
	{
		base.StartCoroutine(this.intro_cr());
	}

	// Token: 0x06001E49 RID: 7753 RVA: 0x00116BC0 File Offset: 0x00114FC0
	private IEnumerator intro_cr()
	{
		AudioManager.Play("level_dragon_left_dragon_intro");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_intro");
		yield return base.TweenPositionX(this.xPos, -350f, 1.3f, EaseUtils.EaseType.easeInSine);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro", 0, false, true);
		foreach (Collider2D collider2D in base.GetComponents<Collider2D>())
		{
			collider2D.enabled = true;
		}
		this.damageReceiver.enabled = true;
		base.StartCoroutine(this.fire_cr());
		this.StartFireMarchers();
		yield break;
	}

	// Token: 0x06001E4A RID: 7754 RVA: 0x00116BDB File Offset: 0x00114FDB
	private void TongueIntroSFX()
	{
		AudioManager.Play("level_dragon_left_dragon_tongue_intro");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_tongue_intro");
	}

	// Token: 0x06001E4B RID: 7755 RVA: 0x00116BF8 File Offset: 0x00114FF8
	private IEnumerator fire_cr()
	{
		this.state = DragonLevelLeftSideDragon.State.Fire;
		int patternIndex = 0;
		this.fire.transform.parent = null;
		for (;;)
		{
			string[] pattern = base.properties.CurrentState.fireAndSmoke.PatternString.Split(new char[]
			{
				','
			});
			string text = pattern[patternIndex % pattern.Length];
			if (text != null)
			{
				if (!(text == "F"))
				{
					if (text == "S")
					{
						AudioManager.Play("level_dragon_left_dragon_smoke_start");
						this.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_start");
						base.animator.SetTrigger("StartSmoke");
						yield return base.animator.WaitForAnimationToStart(this, "Smoke_Loop", 2, false);
						AudioManager.Play("level_dragon_left_dragon_smoke_loop");
						this.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_loop");
						yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.66f, 1.32f));
						AudioManager.Play("level_dragon_left_dragon_smoke_end");
						this.emitAudioFromObject.Add("level_dragon_left_dragon_smoke_end");
					}
				}
				else
				{
					AudioManager.Play("level_dragon_left_dragon_fire_start");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_fire_start");
					base.animator.SetTrigger("StartFire");
					yield return base.animator.WaitForAnimationToStart(this, "Fire_Loop", 2, false);
					AudioManager.Play("level_dragon_left_dragon_fire_loop");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_fire_loop");
					this.fire.SetColliderEnabled(true);
					yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(0.25f, 1.75f));
					AudioManager.Play("level_dragon_left_dragon_fire_end");
					this.emitAudioFromObject.Add("level_dragon_left_dragon_fire_end");
					this.fire.SetColliderEnabled(false);
				}
			}
			base.animator.SetTrigger("Continue");
			yield return base.animator.WaitForAnimationToStart(this, "Idle", 2, false);
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
			patternIndex++;
		}
		yield break;
	}

	// Token: 0x06001E4C RID: 7756 RVA: 0x00116C13 File Offset: 0x00115013
	private void StartFireMarchers()
	{
		base.StartCoroutine(this.spawnFireMarchers_cr());
		base.StartCoroutine(this.fireMarchersJump_cr());
	}

	// Token: 0x06001E4D RID: 7757 RVA: 0x00116C30 File Offset: 0x00115030
	private IEnumerator spawnFireMarchers_cr()
	{
		this.fireMarcherLeaderPrefab.Create(this.fireMarcherRoot, base.properties.CurrentState.fireMarchers);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fireMarchers.spawnDelay);
			this.lastFireMarcher = this.fireMarcherPrefabs.RandomChoice<DragonLevelFireMarcher>().Create(this.fireMarcherRoot, base.properties.CurrentState.fireMarchers);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E4E RID: 7758 RVA: 0x00116C4C File Offset: 0x0011504C
	private IEnumerator fireMarchersJump_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.fireMarchers.jumpDelay.RandomFloat());
			DragonLevelFireMarcher[] fireMarchers = UnityEngine.Object.FindObjectsOfType<DragonLevelFireMarcher>();
			fireMarchers.Shuffle<DragonLevelFireMarcher>();
			foreach (DragonLevelFireMarcher dragonLevelFireMarcher in fireMarchers)
			{
				if (dragonLevelFireMarcher.CanJump())
				{
					dragonLevelFireMarcher.StartJump(PlayerManager.GetNext());
					break;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E4F RID: 7759 RVA: 0x00116C67 File Offset: 0x00115067
	public void StartThreeHeads()
	{
		this.StopAllCoroutines();
		this.state = DragonLevelLeftSideDragon.State.Transition;
		this.fire.gameObject.SetActive(false);
		base.StartCoroutine(this.three_heads_cr());
	}

	// Token: 0x06001E50 RID: 7760 RVA: 0x00116C94 File Offset: 0x00115094
	private IEnumerator three_heads_cr()
	{
		base.animator.SetTrigger("StartThree");
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		while (this.lastFireMarcher != null)
		{
			yield return null;
		}
		base.animator.SetTrigger("FoldTongue");
		yield return base.animator.WaitForAnimationToEnd(this, "Intro_Reverse", 1, false, true);
		base.animator.SetTrigger("ToThree");
		yield return base.animator.WaitForAnimationToStart(this, "Three_Intro", false);
		base.GetComponent<LevelBossDeathExploder>().StopExplosions();
		AudioManager.Play("level_dragon_three_dragon_intro");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_intro");
		this.state = DragonLevelLeftSideDragon.State.ThreeHeads;
		foreach (DragonLevelBackgroundChange dragonLevelBackgroundChange in this.backgrounds)
		{
			dragonLevelBackgroundChange.StartChange();
		}
		for (int j = 0; j < this.backgroundsToHide.Length; j++)
		{
			this.backgroundsToHide[j].SetActive(false);
		}
		this.spire.StartChange();
		this.rain.StartRain();
		base.StartCoroutine(this.potion_cr());
		base.StartCoroutine(this.blow_torch_cr());
		yield return null;
		yield break;
	}

	// Token: 0x06001E51 RID: 7761 RVA: 0x00116CAF File Offset: 0x001150AF
	private void ActivateHeadLayers()
	{
		base.animator.SetTrigger("StartHeads");
	}

	// Token: 0x06001E52 RID: 7762 RVA: 0x00116CC4 File Offset: 0x001150C4
	private void SpawnPotion(int type)
	{
		this.spittle.gameObject.SetActive(false);
		Vector3 vector = Vector3.zero;
		DragonLevelPotion original = this.horizontalPotionPrefab;
		LevelProperties.Dragon.Potions potions = base.properties.CurrentState.potions;
		string[] array = potions.potionTypeString[this.potionTypeMainIndex].Split(new char[]
		{
			','
		});
		if (array[this.potionTypeIndex][0] == 'H')
		{
			original = this.horizontalPotionPrefab;
		}
		else if (array[this.potionTypeIndex][0] == 'V')
		{
			original = this.verticalPotionPrefab;
		}
		else if (array[this.potionTypeIndex][0] == 'X')
		{
			original = this.bothPotionPrefab;
		}
		if (type == 1 || type == 3)
		{
			vector = this.topHead.position;
		}
		else if (type == 2 || type == 4)
		{
			vector = this.bottomHead.position;
		}
		AbstractPlayerController next = PlayerManager.GetNext();
		Vector3 v = Vector3.zero;
		if (next.transform.position.x > base.transform.position.x)
		{
			v = next.transform.position - vector;
		}
		else
		{
			v = MathUtils.AngleToDirection(90f);
		}
		DragonLevelPotion dragonLevelPotion = UnityEngine.Object.Instantiate<DragonLevelPotion>(original);
		dragonLevelPotion.Init(vector, base.properties.CurrentState.potions.potionHP, MathUtils.DirectionToAngle(v), base.properties.CurrentState.potions);
		if (this.potionTypeIndex < array.Length - 1)
		{
			this.potionTypeIndex++;
		}
		else
		{
			this.potionTypeMainIndex = (this.potionTypeMainIndex + 1) % potions.potionTypeString.Length;
			this.potionTypeIndex = 0;
		}
		this.spittle.gameObject.SetActive(true);
		this.spittle.position = vector;
	}

	// Token: 0x06001E53 RID: 7763 RVA: 0x00116EC0 File Offset: 0x001152C0
	private IEnumerator potion_cr()
	{
		LevelProperties.Dragon.Potions p = base.properties.CurrentState.potions;
		string[] attackCountString = p.attackCount[this.attackCountMainIndex].Split(new char[]
		{
			','
		});
		string[] shotPositionString = p.shotPositionString[this.shotPositionMainIndex].Split(new char[]
		{
			','
		});
		int attackCount = 0;
		for (;;)
		{
			attackCountString = p.attackCount[this.attackCountMainIndex].Split(new char[]
			{
				','
			});
			Parser.IntTryParse(attackCountString[this.attackCountIndex], out attackCount);
			for (int i = 0; i < attackCount; i++)
			{
				shotPositionString = p.shotPositionString[this.shotPositionMainIndex].Split(new char[]
				{
					','
				});
				string[] pickedDragon = shotPositionString[this.shotPositionIndex].Split(new char[]
				{
					':'
				});
				foreach (string picked in pickedDragon)
				{
					while (this.torch)
					{
						yield return null;
					}
					if (shotPositionString[this.shotPositionIndex][0] == 'T')
					{
						this.animationString = "High_Attack";
					}
					else if (shotPositionString[this.shotPositionIndex][0] == 'B')
					{
						this.animationString = "Low_Attack";
					}
					if (picked == "A")
					{
						this.layer = 5;
					}
					else if (picked == "C")
					{
						this.layer = 6;
					}
				}
				if (this.layer == 5 && this.animationString == "High_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.CTop;
				}
				else if (this.layer == 6 && this.animationString == "High_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.ATop;
				}
				else if (this.layer == 5 && this.animationString == "Low_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.CBottom;
				}
				else if (this.layer == 6 && this.animationString == "Low_Attack")
				{
					this.headPicked = DragonLevelLeftSideDragon.HeadPicked.ABottom;
				}
				yield return base.animator.WaitForAnimationToEnd(this, this.animationString, this.layer, false, true);
				if (this.shotPositionIndex < shotPositionString.Length - 1)
				{
					this.shotPositionIndex++;
				}
				else
				{
					this.shotPositionMainIndex = (this.shotPositionMainIndex + 1) % p.shotPositionString.Length;
					this.shotPositionIndex = 0;
				}
				yield return CupheadTime.WaitForSeconds(this, p.repeatDelay);
			}
			if (this.attackCountIndex < attackCountString.Length - 1)
			{
				this.attackCountIndex++;
			}
			else
			{
				this.attackCountMainIndex = (this.attackCountMainIndex + 1) % p.attackCount.Length;
				this.attackCountIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, p.attackMainDelay);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E54 RID: 7764 RVA: 0x00116EDC File Offset: 0x001152DC
	private void PotionAttack(DragonLevelLeftSideDragon.HeadPicked picked)
	{
		if (picked == this.headPicked)
		{
			AudioManager.Play("level_dragon_three_dragon_head_attack");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_attack");
			base.animator.Play(this.animationString, this.layer);
			this.headPicked = DragonLevelLeftSideDragon.HeadPicked.None;
		}
	}

	// Token: 0x06001E55 RID: 7765 RVA: 0x00116F30 File Offset: 0x00115330
	private IEnumerator blow_torch_cr()
	{
		LevelProperties.Dragon.Blowtorch p = base.properties.CurrentState.blowtorch;
		string[] delayPattern = p.attackDelayString.GetRandom<string>().Split(new char[]
		{
			','
		});
		this.middleHead.SetScale(new float?(this.middleHead.transform.localScale.x), new float?(p.fireSize), new float?(1f));
		float delay = 0f;
		int delayCountIndex = UnityEngine.Random.Range(0, delayPattern.Length);
		for (;;)
		{
			Parser.FloatTryParse(delayPattern[delayCountIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay);
			delayCountIndex = (delayCountIndex + 1) % delayPattern.Length;
			this.torch = true;
			yield return base.animator.WaitForAnimationToEnd(this, "Dragon_Head_Idle_Loop", 3, false, true);
			AudioManager.Play("level_dragon_torch_warning_1_start");
			this.emitAudioFromObject.Add("level_dragon_torch_warning_1_start");
			base.animator.Play("Torch_Warning_One", 4);
			yield return base.animator.WaitForAnimationToEnd(this, "Torch_End", 4, false, true);
			this.torch = false;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001E56 RID: 7766 RVA: 0x00116F4C File Offset: 0x0011534C
	private void ActivateTorch()
	{
		this.middleHead.GetComponent<Animator>().SetBool("TorchOn", true);
		AudioManager.Play("level_dragon_three_dragon_head_b_torch_attack_burst");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_attack_burst");
		AudioManager.PlayLoop("level_dragon_three_dragon_head_b_torch_attack_loop");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_attack_loop");
	}

	// Token: 0x06001E57 RID: 7767 RVA: 0x00116FA3 File Offset: 0x001153A3
	private void DeactivateTorch()
	{
		this.middleHead.GetComponent<Animator>().SetBool("TorchOn", false);
		AudioManager.Stop("level_dragon_three_dragon_head_b_torch_attack_loop");
	}

	// Token: 0x06001E58 RID: 7768 RVA: 0x00116FC8 File Offset: 0x001153C8
	private void Torch1Counter()
	{
		if (this.Counter >= base.properties.CurrentState.blowtorch.warningDurationOne)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_continue_one");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_continue_one");
			base.animator.Play("Torch_Continue", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x06001E59 RID: 7769 RVA: 0x0011703C File Offset: 0x0011543C
	private void Attack1Counter()
	{
		if (this.Counter >= this.AttackFrames / 2 + this.AttackFrames % 2)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_warning_2_start");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_warning_2_start");
			base.animator.Play("Torch_Warning_Two", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x06001E5A RID: 7770 RVA: 0x001170AC File Offset: 0x001154AC
	private void Torch2Counter()
	{
		if (this.Counter >= base.properties.CurrentState.blowtorch.warningDurationTwo)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_continue_one");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_continue_one");
			base.animator.Play("Torch_Continue_Two", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x06001E5B RID: 7771 RVA: 0x00117120 File Offset: 0x00115520
	private void Attack2Counter()
	{
		if (this.Counter >= this.AttackFrames / 2)
		{
			this.Counter = 0;
			AudioManager.Play("level_dragon_three_dragon_head_b_torch_end");
			this.emitAudioFromObject.Add("level_dragon_three_dragon_head_b_torch_end");
			base.animator.Play("Torch_End", 4);
		}
		else
		{
			this.Counter++;
		}
	}

	// Token: 0x06001E5C RID: 7772 RVA: 0x00117188 File Offset: 0x00115588
	private void StartDeath()
	{
		AudioManager.Play("level_dragon_three_dragon_death");
		this.emitAudioFromObject.Add("level_dragon_three_dragon_death");
		this.StopAllCoroutines();
		this.middleHead.gameObject.SetActive(false);
		base.animator.SetTrigger("Continue");
		if (Level.Current.mode == Level.Mode.Easy)
		{
			base.animator.SetTrigger("DeadEASY");
		}
		else
		{
			base.animator.SetTrigger("Dead");
		}
	}

	// Token: 0x04002708 RID: 9992
	private const float FRAME_RATE = 0.041666668f;

	// Token: 0x0400270A RID: 9994
	private DragonLevelLeftSideDragon.HeadPicked headPicked;

	// Token: 0x0400270B RID: 9995
	private const int MAIN_LAYER = 0;

	// Token: 0x0400270C RID: 9996
	private const int TONGUE_LAYER = 1;

	// Token: 0x0400270D RID: 9997
	private const int FIRE_LAYER = 2;

	// Token: 0x0400270E RID: 9998
	private const float introTime = 1.3f;

	// Token: 0x0400270F RID: 9999
	private const float mainX = -350f;

	// Token: 0x04002710 RID: 10000
	[SerializeField]
	private Collider2D damageBox;

	// Token: 0x04002711 RID: 10001
	[SerializeField]
	private DragonLevelSpire spire;

	// Token: 0x04002712 RID: 10002
	[SerializeField]
	private DragonLevelRain rain;

	// Token: 0x04002713 RID: 10003
	[SerializeField]
	private DragonLevelBackgroundChange[] backgrounds;

	// Token: 0x04002714 RID: 10004
	[SerializeField]
	private GameObject[] backgroundsToHide;

	// Token: 0x04002715 RID: 10005
	[SerializeField]
	private DragonLevelFire fire;

	// Token: 0x04002716 RID: 10006
	[SerializeField]
	private Transform fireMarcherRoot;

	// Token: 0x04002717 RID: 10007
	[SerializeField]
	private DragonLevelFireMarcher[] fireMarcherPrefabs;

	// Token: 0x04002718 RID: 10008
	[SerializeField]
	private DragonLevelFireMarcher fireMarcherLeaderPrefab;

	// Token: 0x04002719 RID: 10009
	[SerializeField]
	private Transform topHead;

	// Token: 0x0400271A RID: 10010
	[SerializeField]
	private Transform bottomHead;

	// Token: 0x0400271B RID: 10011
	[SerializeField]
	private Transform middleHead;

	// Token: 0x0400271C RID: 10012
	[SerializeField]
	private DragonLevelPotion horizontalPotionPrefab;

	// Token: 0x0400271D RID: 10013
	[SerializeField]
	private DragonLevelPotion verticalPotionPrefab;

	// Token: 0x0400271E RID: 10014
	[SerializeField]
	private DragonLevelPotion bothPotionPrefab;

	// Token: 0x0400271F RID: 10015
	[SerializeField]
	private Transform spittle;

	// Token: 0x04002720 RID: 10016
	private DragonLevelFireMarcher lastFireMarcher;

	// Token: 0x04002721 RID: 10017
	private DamageDealer damageDealer;

	// Token: 0x04002722 RID: 10018
	private DamageReceiver damageReceiver;

	// Token: 0x04002723 RID: 10019
	private bool dead;

	// Token: 0x04002724 RID: 10020
	private bool torch;

	// Token: 0x04002725 RID: 10021
	private int potionTypeIndex;

	// Token: 0x04002726 RID: 10022
	private int potionTypeMainIndex;

	// Token: 0x04002727 RID: 10023
	private int attackCountIndex;

	// Token: 0x04002728 RID: 10024
	private int attackCountMainIndex;

	// Token: 0x04002729 RID: 10025
	private int shotPositionIndex;

	// Token: 0x0400272A RID: 10026
	private int shotPositionMainIndex;

	// Token: 0x0400272B RID: 10027
	private int AttackFrames;

	// Token: 0x0400272C RID: 10028
	private int Counter;

	// Token: 0x0400272D RID: 10029
	private string animationString;

	// Token: 0x0400272E RID: 10030
	private int layer;

	// Token: 0x0400272F RID: 10031
	private float xPos;

	// Token: 0x020005F2 RID: 1522
	public enum State
	{
		// Token: 0x04002731 RID: 10033
		UnSpawned,
		// Token: 0x04002732 RID: 10034
		Fire,
		// Token: 0x04002733 RID: 10035
		Transition,
		// Token: 0x04002734 RID: 10036
		ThreeHeads,
		// Token: 0x04002735 RID: 10037
		Dead
	}

	// Token: 0x020005F3 RID: 1523
	private enum HeadPicked
	{
		// Token: 0x04002737 RID: 10039
		ATop,
		// Token: 0x04002738 RID: 10040
		ABottom,
		// Token: 0x04002739 RID: 10041
		CTop,
		// Token: 0x0400273A RID: 10042
		CBottom,
		// Token: 0x0400273B RID: 10043
		None
	}
}
