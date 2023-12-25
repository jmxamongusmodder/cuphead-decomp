using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200020D RID: 525
public class MausoleumLevel : Level
{
	// Token: 0x060005DF RID: 1503 RVA: 0x0006A884 File Offset: 0x00068C84
	protected override void PartialInit()
	{
		this.properties = LevelProperties.Mausoleum.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000107 RID: 263
	// (get) Token: 0x060005E0 RID: 1504 RVA: 0x0006A91A File Offset: 0x00068D1A
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.Mausoleum;
		}
	}

	// Token: 0x17000108 RID: 264
	// (get) Token: 0x060005E1 RID: 1505 RVA: 0x0006A921 File Offset: 0x00068D21
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_mausoleum;
		}
	}

	// Token: 0x17000109 RID: 265
	// (get) Token: 0x060005E2 RID: 1506 RVA: 0x0006A928 File Offset: 0x00068D28
	public override Sprite BossPortrait
	{
		get
		{
			switch (base.mode)
			{
			case Level.Mode.Easy:
				return this._bossPortraitEasy;
			case Level.Mode.Normal:
				return this._bossPortraitNormal;
			case Level.Mode.Hard:
				return this._bossPortraitHard;
			default:
				global::Debug.LogError("Couldn't find portrait for state " + base.mode + ". Using Main.", null);
				return this._bossPortraitEasy;
			}
		}
	}

	// Token: 0x1700010A RID: 266
	// (get) Token: 0x060005E3 RID: 1507 RVA: 0x0006A990 File Offset: 0x00068D90
	public override string BossQuote
	{
		get
		{
			switch (base.mode)
			{
			case Level.Mode.Easy:
				return this._bossQuoteEasy;
			case Level.Mode.Normal:
				return this._bossQuoteNormal;
			case Level.Mode.Hard:
				return this._bossQuoteHard;
			default:
				global::Debug.LogError("Couldn't find quote for state " + base.mode + ". Using Main.", null);
				return this._bossQuoteEasy;
			}
		}
	}

	// Token: 0x060005E4 RID: 1508 RVA: 0x0006A9F8 File Offset: 0x00068DF8
	protected override void Awake()
	{
		Level.OverrideDifficulty = true;
		this.LoseGame = (Action)Delegate.Combine(this.LoseGame, new Action(this.Failure));
		Scenes currentMap = PlayerData.Data.CurrentMap;
		if (currentMap != Scenes.scene_map_world_1)
		{
			if (currentMap != Scenes.scene_map_world_2)
			{
				if (currentMap == Scenes.scene_map_world_3)
				{
					base.mode = Level.Mode.Hard;
				}
			}
			else
			{
				base.mode = Level.Mode.Normal;
			}
		}
		else
		{
			base.mode = Level.Mode.Easy;
		}
		this.originalMode = Level.CurrentMode;
		Level.SetCurrentMode(base.mode);
		foreach (GameObject gameObject in this.WorldBackgrounds)
		{
			gameObject.SetActive(false);
		}
		this.WorldBackgrounds[(int)base.mode].SetActive(true);
		this.currentUrnAnimator = this.urnsAnimator[(int)base.mode];
		this.currentChaliceAnimator = this.chaliceCharacterAnimators[(int)base.mode];
		if ((PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_beam) && base.mode == Level.Mode.Easy) || (PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_invincible) && base.mode == Level.Mode.Normal) || (PlayerData.Data.IsUnlocked(PlayerId.Any, Super.level_super_ghost) && base.mode == Level.Mode.Hard))
		{
			this.noChalice = true;
			this.helpSignAnimator.gameObject.SetActive(false);
			this.currentUrnAnimator.SetTrigger("NoGlow");
		}
		base.Awake();
	}

	// Token: 0x060005E5 RID: 1509 RVA: 0x0006AB88 File Offset: 0x00068F88
	protected override void Start()
	{
		this.isMausoleum = true;
		base.Start();
		Dialoguer.events.onMessageEvent += this.OnMessageEvent;
		this.dialogue.chaliceAnimator = this.currentChaliceAnimator;
		Scenes currentMap = PlayerData.Data.CurrentMap;
		if (currentMap != Scenes.scene_map_world_1)
		{
			if (currentMap != Scenes.scene_map_world_2)
			{
				if (currentMap == Scenes.scene_map_world_3)
				{
					this.super = Super.level_super_ghost;
				}
			}
			else
			{
				this.super = Super.level_super_invincible;
			}
		}
		else
		{
			this.super = Super.level_super_beam;
		}
	}

	// Token: 0x060005E6 RID: 1510 RVA: 0x0006AC1E File Offset: 0x0006901E
	protected override void OnLevelStart()
	{
		base.StartCoroutine(this.main_pattern_cr());
		if (this.noChalice)
		{
			return;
		}
		base.StartCoroutine(this.helpsignDisappear_cr());
		base.StartCoroutine(this.urnrandomanimation_cr());
	}

	// Token: 0x060005E7 RID: 1511 RVA: 0x0006AC53 File Offset: 0x00069053
	protected override void OnStateChanged()
	{
		base.OnStateChanged();
	}

	// Token: 0x060005E8 RID: 1512 RVA: 0x0006AC5B File Offset: 0x0006905B
	public void OnMessageEvent(string message, string metaData)
	{
		if (message == "PowerUpGiven")
		{
			this.currentChaliceAnimator.Play("Chalice_Magic_Burst");
			base.StartCoroutine(this.chalice_animation_cr());
			base.StartCoroutine(this.play_chalice_sound_cr());
		}
	}

	// Token: 0x060005E9 RID: 1513 RVA: 0x0006AC98 File Offset: 0x00069098
	private IEnumerator chalice_animation_cr()
	{
		yield return this.currentChaliceAnimator.WaitForAnimationToEnd(this, "Chalice_Magic_Burst_mid", false, true);
		this.PlaySuperPowerup();
		yield return null;
		yield break;
	}

	// Token: 0x060005EA RID: 1514 RVA: 0x0006ACB4 File Offset: 0x000690B4
	private void PlaySuperPowerup()
	{
		foreach (AbstractPlayerController abstractPlayerController in this.players)
		{
			if (!(abstractPlayerController == null))
			{
				if (!this.PowerUpSFXActive)
				{
					this.PowerUpSFXActive = true;
				}
				float y = (abstractPlayerController.transform.position.y >= -195f) ? 368f : 146f;
				this.chaliceBeamEffect.Create(new Vector3(abstractPlayerController.transform.position.x - 10f, y));
				abstractPlayerController.animator.Play("Super_Power_Up");
			}
		}
	}

	// Token: 0x060005EB RID: 1515 RVA: 0x0006AD70 File Offset: 0x00069170
	private IEnumerator play_chalice_sound_cr()
	{
		yield return this.currentChaliceAnimator.WaitForAnimationToEnd(this, "Chalice_Magic_Burst", false, true);
		AudioManager.Play("player_power_up");
		this.emitAudioFromObject.Add("player_power_up");
		yield return null;
		yield break;
	}

	// Token: 0x060005EC RID: 1516 RVA: 0x0006AD8C File Offset: 0x0006918C
	private IEnumerator mausoleumPattern_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		for (;;)
		{
			yield return base.StartCoroutine(this.nextPattern_cr());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060005ED RID: 1517 RVA: 0x0006ADA8 File Offset: 0x000691A8
	private IEnumerator nextPattern_cr()
	{
		LevelProperties.Mausoleum.Pattern p = this.properties.CurrentState.NextPattern;
		yield return CupheadTime.WaitForSeconds(this, 1f);
		yield break;
	}

	// Token: 0x060005EE RID: 1518 RVA: 0x0006ADC4 File Offset: 0x000691C4
	private IEnumerator helpsignDisappear_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.helpSignAnimator.SetTrigger("Outro");
		yield break;
	}

	// Token: 0x060005EF RID: 1519 RVA: 0x0006ADE0 File Offset: 0x000691E0
	private IEnumerator urnrandomanimation_cr()
	{
		while (!this.isLevelOver)
		{
			yield return CupheadTime.WaitForSeconds(this, 2f);
			AudioManager.Play("mausoleum_ghost_jar_shake");
			this.emitAudioFromObject.Add("mausoleum_ghost_jar_shake");
			int rand = UnityEngine.Random.Range(1, 4);
			if (rand == 1)
			{
				this.currentUrnAnimator.SetTrigger("Shake");
			}
			else if (rand == 2)
			{
				this.currentUrnAnimator.SetTrigger("SmallVibrate");
			}
			else if (rand == 3)
			{
				this.currentUrnAnimator.SetTrigger("BigVibrate");
			}
		}
		yield break;
	}

	// Token: 0x060005F0 RID: 1520 RVA: 0x0006ADFC File Offset: 0x000691FC
	private IEnumerator legendarychaliceappear_cr()
	{
		AudioManager.StopBGM();
		AudioManager.PlayBGMPlaylistManually(false);
		this.currentUrnAnimator.SetTrigger("Sparkle");
		yield return CupheadTime.WaitForSeconds(this, 2.5f);
		AudioManager.Play("mausoleum_lid_pop");
		this.currentUrnAnimator.SetTrigger("Pop");
		yield return CupheadTime.WaitForSeconds(this, 2f);
		float t = 0f;
		float TIME = 1.5f;
		float arcHeight = 150f;
		float arcHeightAdd = 0f;
		Vector3 startPosition = this.currentChaliceAnimator.gameObject.transform.localPosition;
		Vector3 endPosition = this.currentChaliceAnimator.gameObject.transform.localPosition + new Vector3(400f, 0f, 0f);
		AudioManager.Play("mausoleum_ghost_jar_travel");
		this.emitAudioFromObject.Add("mausoleum_ghost_jar_travel");
		while (t < TIME)
		{
			float val = t / TIME;
			Vector3 newPosition = Vector3.Lerp(startPosition, endPosition, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val));
			if (t < TIME / 2f)
			{
				arcHeightAdd = Mathf.Lerp(arcHeight, 0f, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, 1f - val * 2f));
			}
			else
			{
				arcHeightAdd = Mathf.Lerp(arcHeight, 0f, EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, val * 2f - 1f));
			}
			newPosition.y += arcHeightAdd;
			this.currentChaliceAnimator.gameObject.transform.localPosition = newPosition;
			t += Time.deltaTime;
			yield return null;
		}
		AudioManager.Play("mausoleum_ghost_jar_queen_ghost_appear");
		this.emitAudioFromObject.Add("mausoleum_ghost_jar_queen_ghost_appear");
		this.currentChaliceAnimator.SetTrigger("Transition");
		this.dialogue.BeginDialogue();
		yield return CupheadTime.WaitForSeconds(this, 1f);
		float currentDialogueFloat = Dialoguer.GetGlobalFloat(this.dialoguerVariableID);
		Dialoguer.SetGlobalFloat(14, currentDialogueFloat + 1f);
		PlayerData.SaveCurrentFile();
		yield break;
	}

	// Token: 0x060005F1 RID: 1521 RVA: 0x0006AE18 File Offset: 0x00069218
	private void SetupTimeline()
	{
		base.timeline = new Level.Timeline();
		base.timeline.health = 0f;
		List<float> list = new List<float>();
		int num = 3;
		for (int i = 0; i < num; i++)
		{
			base.timeline.health += (float)this.properties.CurrentState.main.ghostCount;
			list.Add((float)this.properties.CurrentState.main.ghostCount);
		}
		for (int j = 0; j < num - 1; j++)
		{
			base.timeline.AddEventAtHealth(j.ToStringInvariant(), base.timeline.GetHealthOfLastEvent() + (int)list[j]);
		}
	}

	// Token: 0x060005F2 RID: 1522 RVA: 0x0006AED8 File Offset: 0x000692D8
	private IEnumerator main_pattern_cr()
	{
		this.SetupTimeline();
		yield return null;
		for (;;)
		{
			LevelProperties.Mausoleum.Main p = this.properties.CurrentState.main;
			int delayMainIndex = UnityEngine.Random.Range(0, p.delayString.Length);
			string[] delayString = p.delayString[delayMainIndex].Split(new char[]
			{
				','
			});
			int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
			int spawnMainIndex = UnityEngine.Random.Range(0, p.spawnString.Length);
			string[] spawnString = p.spawnString[spawnMainIndex].Split(new char[]
			{
				','
			});
			int spawnIndex = UnityEngine.Random.Range(0, spawnString.Length);
			int ghostTypeIndex = UnityEngine.Random.Range(0, p.ghostTypeString.Length);
			string[] ghostString = p.ghostTypeString[ghostTypeIndex].Split(new char[]
			{
				','
			});
			int ghostIndex = UnityEngine.Random.Range(0, ghostString.Length);
			float delay = 0f;
			int spawnPos = 0;
			this.maxCounter = p.ghostCount;
			MausoleumLevel.SPAWNCOUNTER = 0;
			while (MausoleumLevel.SPAWNCOUNTER < this.maxCounter)
			{
				delayString = p.delayString[delayMainIndex].Split(new char[]
				{
					','
				});
				ghostString = p.ghostTypeString[ghostTypeIndex].Split(new char[]
				{
					','
				});
				string[] ghostSplit = ghostString[ghostIndex].Split(new char[]
				{
					'-'
				});
				float extraDelay = 0f;
				int splitcount = 0;
				foreach (string split in ghostSplit)
				{
					spawnString = p.spawnString[spawnMainIndex].Split(new char[]
					{
						','
					});
					if (Parser.IntParse(spawnString[spawnIndex]) >= 6)
					{
						spawnPos = Parser.IntParse(spawnString[spawnIndex]) - 2;
					}
					else
					{
						spawnPos = Parser.IntParse(spawnString[spawnIndex]) - 1;
					}
					Vector3 direction = this.urn.transform.position - this.positions[spawnPos].transform.position;
					float angle = MathUtils.DirectionToAngle(direction);
					int repeatCount = 0;
					Parser.IntTryParse(ghostString[ghostIndex].Substring(1), out repeatCount);
					if (Parser.IntParse(spawnString[spawnIndex]) == 2 || Parser.IntParse(spawnString[spawnIndex]) == 8)
					{
						MausoleumLevelDelayGhost mausoleumLevelDelayGhost = this.delayGhost.Create(this.positions[spawnPos].transform.position, angle, 0f, this.properties.CurrentState.delayGhost);
						mausoleumLevelDelayGhost.GetParent(this);
					}
					else
					{
						char c2 = ghostString[ghostIndex][0];
						switch (c2)
						{
						case 'B':
							if (repeatCount != 0)
							{
								for (int i = 0; i < repeatCount; i++)
								{
									MausoleumLevelBigGhost b = this.bigGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.bigGhost.speed, this.properties.CurrentState.bigGhost, this.urn.gameObject);
									b.Counts = (splitcount == 0);
									b.GetParent(this);
									yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.bigGhost.multiDelay);
								}
								extraDelay += this.properties.CurrentState.bigGhost.mainAddDelay;
							}
							else
							{
								MausoleumLevelBigGhost mausoleumLevelBigGhost = this.bigGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.bigGhost.speed, this.properties.CurrentState.bigGhost, this.urn.gameObject);
								mausoleumLevelBigGhost.Counts = (splitcount == 0);
								mausoleumLevelBigGhost.GetParent(this);
								extraDelay += this.properties.CurrentState.bigGhost.mainAddDelay;
							}
							break;
						case 'C':
						{
							MausoleumLevelCircleGhost c = this.circleGhost.Create(this.positions[spawnPos].transform.position, this.urn.transform.position, angle, this.properties.CurrentState.circleGhost.circleSpeed, this.properties.CurrentState.circleGhost.circleRate) as MausoleumLevelCircleGhost;
							c.Counts = (splitcount == 0);
							c.GetParent(this);
							break;
						}
						case 'D':
						{
							MausoleumLevelDelayGhost d = this.delayGhost.Create(this.positions[spawnPos].transform.position, angle, 0f, this.properties.CurrentState.delayGhost);
							d.Counts = (splitcount == 0);
							d.GetParent(this);
							break;
						}
						default:
							if (c2 != 'R')
							{
								if (c2 == 'S')
								{
									if (repeatCount != 0)
									{
										for (int j = 0; j < repeatCount; j++)
										{
											MausoleumLevelSineGhost s = this.sineGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.sineGhost.ghostSpeed, this.properties.CurrentState.sineGhost);
											s.Counts = (splitcount == 0);
											s.GetParent(this);
											yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.sineGhost.multiDelay);
										}
										extraDelay = this.properties.CurrentState.sineGhost.mainAddDelay;
									}
									else
									{
										MausoleumLevelSineGhost mausoleumLevelSineGhost = this.sineGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.sineGhost.ghostSpeed, this.properties.CurrentState.sineGhost);
										mausoleumLevelSineGhost.Counts = (splitcount == 0);
										mausoleumLevelSineGhost.GetParent(this);
										extraDelay = this.properties.CurrentState.sineGhost.mainAddDelay;
									}
								}
							}
							else if (repeatCount != 0)
							{
								for (int k = 0; k < repeatCount; k++)
								{
									MausoleumLevelRegularGhost g = this.regularGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.regularGhost.speed) as MausoleumLevelRegularGhost;
									g.Counts = (splitcount == 0);
									g.GetParent(this);
									yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.regularGhost.multiDelay);
								}
								extraDelay += this.properties.CurrentState.regularGhost.mainAddDelay;
							}
							else
							{
								MausoleumLevelRegularGhost mausoleumLevelRegularGhost = this.regularGhost.Create(this.positions[spawnPos].transform.position, angle, this.properties.CurrentState.regularGhost.speed) as MausoleumLevelRegularGhost;
								mausoleumLevelRegularGhost.Counts = (splitcount == 0);
								mausoleumLevelRegularGhost.GetParent(this);
								extraDelay += this.properties.CurrentState.regularGhost.mainAddDelay;
							}
							break;
						}
					}
					splitcount++;
					if (spawnIndex < spawnString.Length - 1)
					{
						spawnIndex++;
					}
					else
					{
						spawnMainIndex = (spawnMainIndex + 1) % p.spawnString.Length;
						spawnIndex = 0;
					}
				}
				base.timeline.DealDamage(1f);
				yield return null;
				delay = Parser.FloatParse(delayString[delayIndex]) + extraDelay;
				yield return CupheadTime.WaitForSeconds(this, delay);
				extraDelay = 0f;
				if (delayIndex < delayString.Length - 1)
				{
					delayIndex++;
				}
				else
				{
					delayMainIndex = (delayMainIndex + 1) % p.delayString.Length;
					delayIndex = 0;
				}
				if (ghostIndex < ghostString.Length - 1)
				{
					ghostIndex++;
				}
				else
				{
					ghostTypeIndex = (ghostTypeIndex + 1) % p.ghostTypeString.Length;
					ghostIndex = 0;
				}
				yield return null;
			}
			MausoleumLevelGhostBase[] ghosts = UnityEngine.Object.FindObjectsOfType(typeof(MausoleumLevelGhostBase)) as MausoleumLevelGhostBase[];
			bool ghostsAlive = true;
			int ghostCounter = 0;
			while (ghostsAlive)
			{
				ghosts = (UnityEngine.Object.FindObjectsOfType(typeof(MausoleumLevelGhostBase)) as MausoleumLevelGhostBase[]);
				for (int m = 0; m < ghosts.Length; m++)
				{
					if (ghosts[m].isDead)
					{
						ghostCounter++;
						if (ghostCounter >= ghosts.Length)
						{
							ghostsAlive = false;
							break;
						}
					}
				}
				ghostCounter = 0;
				if (!ghostsAlive)
				{
					break;
				}
				yield return CupheadTime.WaitForSeconds(this, 0.25f);
				yield return null;
			}
			this.properties.DealDamageToNextNamedState();
			yield return null;
		}
		yield break;
	}

	// Token: 0x060005F3 RID: 1523 RVA: 0x0006AEF4 File Offset: 0x000692F4
	private void Failure()
	{
		base._OnLose();
		AudioManager.Play("mausoleum_ghost_jar_burst");
		this.emitAudioFromObject.Add("mausoleum_ghost_jar_burst");
		PlayerManager.GetPlayer(PlayerId.PlayerOne).GetComponent<LevelPlayerAnimationController>().ScaredSprite(this.FacingLeft(PlayerManager.GetPlayer(PlayerId.PlayerOne)));
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			PlayerManager.GetPlayer(PlayerId.PlayerTwo).GetComponent<LevelPlayerAnimationController>().ScaredSprite(this.FacingLeft(PlayerManager.GetPlayer(PlayerId.PlayerTwo)));
		}
		base.timeline.OnPlayerDeath(PlayerId.PlayerOne);
		if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
		{
			base.timeline.OnPlayerDeath(PlayerId.PlayerTwo);
		}
	}

	// Token: 0x060005F4 RID: 1524 RVA: 0x0006AF94 File Offset: 0x00069394
	private bool FacingLeft(AbstractPlayerController player)
	{
		if (player.transform.position.x > this.urn.transform.position.x)
		{
			return player.transform.localScale.x == 1f;
		}
		return player.transform.localScale.x == -1f;
	}

	// Token: 0x060005F5 RID: 1525 RVA: 0x0006B014 File Offset: 0x00069414
	protected override void OnDestroy()
	{
		base.OnDestroy();
		Level.SetCurrentMode(this.originalMode);
		base.StopCoroutine(this.urnrandomanimation_cr());
		Dialoguer.events.onMessageEvent -= this.OnMessageEvent;
		this.circleGhost = null;
		this.regularGhost = null;
		this.bigGhost = null;
		this.delayGhost = null;
		this.sineGhost = null;
		this._bossPortraitEasy = null;
		this._bossPortraitHard = null;
		this._bossPortraitNormal = null;
	}

	// Token: 0x060005F6 RID: 1526 RVA: 0x0006B08C File Offset: 0x0006948C
	protected override void OnPreWin()
	{
		this.isLevelOver = true;
		base.StopCoroutine(this.urnrandomanimation_cr());
		if (this.noChalice)
		{
			base.StartCoroutine(this.win_no_chalice());
			return;
		}
		base.StartCoroutine(this.legendarychaliceappear_cr());
	}

	// Token: 0x060005F7 RID: 1527 RVA: 0x0006B0C8 File Offset: 0x000694C8
	private IEnumerator win_no_chalice()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		SceneLoader.LoadLastMap();
		yield break;
	}

	// Token: 0x060005F8 RID: 1528 RVA: 0x0006B0E4 File Offset: 0x000694E4
	protected override void OnWin()
	{
		base.OnWin();
		if (!PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, this.super))
		{
			PlayerData.Data.Buy(PlayerId.PlayerOne, this.super);
			PlayerData.Data.Buy(PlayerId.PlayerTwo, this.super);
			Level.SuperUnlocked = true;
		}
		if (PlayerData.Data.NumSupers(PlayerId.PlayerOne) >= 3)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, "UnlockedAllSupers");
		}
	}

	// Token: 0x04000AB9 RID: 2745
	private LevelProperties.Mausoleum properties;

	// Token: 0x04000ABA RID: 2746
	public static int SPAWNCOUNTER;

	// Token: 0x04000ABB RID: 2747
	[SerializeField]
	private GameObject[] WorldBackgrounds;

	// Token: 0x04000ABC RID: 2748
	[SerializeField]
	private MausoleumLevelCircleGhost circleGhost;

	// Token: 0x04000ABD RID: 2749
	[SerializeField]
	private MausoleumLevelRegularGhost regularGhost;

	// Token: 0x04000ABE RID: 2750
	[SerializeField]
	private MausoleumLevelBigGhost bigGhost;

	// Token: 0x04000ABF RID: 2751
	[SerializeField]
	private MausoleumLevelDelayGhost delayGhost;

	// Token: 0x04000AC0 RID: 2752
	[SerializeField]
	private MausoleumLevelSineGhost sineGhost;

	// Token: 0x04000AC1 RID: 2753
	[SerializeField]
	private Transform[] positions;

	// Token: 0x04000AC2 RID: 2754
	[SerializeField]
	private MausoleumLevelUrn urn;

	// Token: 0x04000AC3 RID: 2755
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortraitEasy;

	// Token: 0x04000AC4 RID: 2756
	[SerializeField]
	private Sprite _bossPortraitNormal;

	// Token: 0x04000AC5 RID: 2757
	[SerializeField]
	private Sprite _bossPortraitHard;

	// Token: 0x04000AC6 RID: 2758
	[SerializeField]
	private string _bossQuoteEasy;

	// Token: 0x04000AC7 RID: 2759
	[SerializeField]
	private string _bossQuoteNormal;

	// Token: 0x04000AC8 RID: 2760
	[SerializeField]
	private string _bossQuoteHard;

	// Token: 0x04000AC9 RID: 2761
	[SerializeField]
	private Animator helpSignAnimator;

	// Token: 0x04000ACA RID: 2762
	[SerializeField]
	private Animator[] urnsAnimator;

	// Token: 0x04000ACB RID: 2763
	[SerializeField]
	private Animator[] chaliceCharacterAnimators;

	// Token: 0x04000ACC RID: 2764
	private Animator currentUrnAnimator;

	// Token: 0x04000ACD RID: 2765
	private Animator currentChaliceAnimator;

	// Token: 0x04000ACE RID: 2766
	[SerializeField]
	private Effect chaliceBeamEffect;

	// Token: 0x04000ACF RID: 2767
	[SerializeField]
	private MausoleumDialogueInteraction dialogue;

	// Token: 0x04000AD0 RID: 2768
	[SerializeField]
	private int dialoguerVariableID = 14;

	// Token: 0x04000AD1 RID: 2769
	private bool isLevelOver;

	// Token: 0x04000AD2 RID: 2770
	private bool PowerUpSFXActive;

	// Token: 0x04000AD3 RID: 2771
	private Super super = Super.level_super_beam;

	// Token: 0x04000AD4 RID: 2772
	private int maxCounter;

	// Token: 0x04000AD5 RID: 2773
	private bool noChalice;

	// Token: 0x04000AD6 RID: 2774
	public Action WinGame;

	// Token: 0x04000AD7 RID: 2775
	public Action LoseGame;

	// Token: 0x04000AD8 RID: 2776
	private Level.Mode originalMode;
}
