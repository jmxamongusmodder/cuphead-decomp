using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000091 RID: 145
public class ChessCastleLevel : Level
{
	// Token: 0x06000193 RID: 403 RVA: 0x00058538 File Offset: 0x00056938
	protected override void PartialInit()
	{
		this.properties = LevelProperties.ChessCastle.GetMode(base.mode);
		this.properties.OnStateChange += base.zHack_OnStateChanged;
		this.properties.OnBossDeath += base.zHack_OnWin;
		base.timeline = this.properties.CreateTimeline(base.mode);
		this.goalTimes = this.properties.goalTimes;
		this.properties.OnBossDamaged += base.timeline.DealDamage;
		base.PartialInit();
	}

	// Token: 0x17000043 RID: 67
	// (get) Token: 0x06000194 RID: 404 RVA: 0x000585CE File Offset: 0x000569CE
	public override Levels CurrentLevel
	{
		get
		{
			return Levels.ChessCastle;
		}
	}

	// Token: 0x17000044 RID: 68
	// (get) Token: 0x06000195 RID: 405 RVA: 0x000585D5 File Offset: 0x000569D5
	public override Scenes CurrentScene
	{
		get
		{
			return Scenes.scene_level_chess_castle;
		}
	}

	// Token: 0x17000045 RID: 69
	// (get) Token: 0x06000196 RID: 406 RVA: 0x000585D9 File Offset: 0x000569D9
	public override Sprite BossPortrait
	{
		get
		{
			return this._bossPortrait;
		}
	}

	// Token: 0x17000046 RID: 70
	// (get) Token: 0x06000197 RID: 407 RVA: 0x000585E1 File Offset: 0x000569E1
	public override string BossQuote
	{
		get
		{
			return this._bossQuote;
		}
	}

	// Token: 0x17000047 RID: 71
	// (get) Token: 0x06000198 RID: 408 RVA: 0x000585E9 File Offset: 0x000569E9
	// (set) Token: 0x06000199 RID: 409 RVA: 0x000585F1 File Offset: 0x000569F1
	public bool rotating { get; private set; }

	// Token: 0x17000048 RID: 72
	// (get) Token: 0x0600019A RID: 410 RVA: 0x000585FA File Offset: 0x000569FA
	public float rotationMultiplier
	{
		get
		{
			return this._rotationMultiplier;
		}
	}

	// Token: 0x0600019B RID: 411 RVA: 0x00058604 File Offset: 0x00056A04
	protected override void OnEnable()
	{
		base.OnEnable();
		SceneLoader.OnFadeOutStartEvent += this.onFadeOutStartEventHandler;
		base.OnIntroEvent += this.onIntroEventHandler;
		Dialoguer.events.onStarted += this.onDialogueStartedHandler;
		Dialoguer.events.onMessageEvent += this.onDialogueMessageHandler;
		Dialoguer.events.onEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded += this.onDialogueEndedHandler;
		Dialoguer.events.onTextPhase += this.onDialogueAdvancedHandler;
	}

	// Token: 0x0600019C RID: 412 RVA: 0x000586A8 File Offset: 0x00056AA8
	protected override void OnDisable()
	{
		base.OnDisable();
		SceneLoader.OnFadeOutStartEvent -= this.onFadeOutStartEventHandler;
		base.OnIntroEvent -= this.onIntroEventHandler;
		Dialoguer.events.onStarted -= this.onDialogueStartedHandler;
		Dialoguer.events.onMessageEvent -= this.onDialogueMessageHandler;
		Dialoguer.events.onEnded -= this.onDialogueEndedHandler;
		Dialoguer.events.onInstantlyEnded -= this.onDialogueEndedHandler;
		Dialoguer.events.onTextPhase -= this.onDialogueAdvancedHandler;
	}

	// Token: 0x0600019D RID: 413 RVA: 0x0005874C File Offset: 0x00056B4C
	protected override void Awake()
	{
		this.previousLevel = Level.PreviousLevel;
		this.previouslyWon = Level.Won;
		if (this.previouslyWon)
		{
			this.attemptsToBeat = PlayerData.Data.chessBossAttemptCounter;
			PlayerData.Data.ResetKingOfGamesCounter();
			PlayerData.SaveCurrentFile();
		}
		base.Awake();
	}

	// Token: 0x0600019E RID: 414 RVA: 0x000587A0 File Offset: 0x00056BA0
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.startEntity = (this.exitEntity = null);
		this.playerStartLevelEffects = null;
		this.dialogueInteractionPoint = null;
		this.speechBubble = null;
		this.coinPrefab = null;
		this.kingAnimator = null;
		this.castleAnimator = null;
		this.platformAnimator = null;
		this.cloudPrefab = null;
	}

	// Token: 0x0600019F RID: 415 RVA: 0x000587FC File Offset: 0x00056BFC
	protected override void Start()
	{
		base.Start();
		this.speechBubbleBasePosition = this.speechBubble.basePosition;
		this.updateDialogueState();
		bool flag = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length;
		base.StartCoroutine(this.cloudSpawn_cr());
		AudioManager.PlayLoop("sfx_dlc_kog_castle_amb_wind_loop");
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_amb_wind_loop", 0.4f, 1f);
		Levels levels;
		if (this.previouslyWon && SceneLoader.CurrentContext is GauntletContext && ((GauntletContext)SceneLoader.CurrentContext).complete)
		{
			levels = Levels.ChessQueen;
			this.movePlayersToDialoguePositions();
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC;
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -3f);
		}
		else if (this.previouslyWon && (!flag || Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) != (float)ChessCastleLevel.KingOfGamesFinalDialogueState))
		{
			this.movePlayersToDialoguePositions();
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGamesVictory_WDLC;
			levels = this.calculatePreviousLevel();
			if (flag)
			{
				levels = Levels.ChessQueen;
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -2f);
			}
			else if (this.attemptsToBeat < ChessCastleLevel.MaxAttemptsToContinue)
			{
				int num = (int)Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerCountIndex);
				num++;
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerCountIndex, (float)num);
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, 0f);
			}
			else
			{
				Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -1f);
			}
		}
		else
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesVictoryDialoguerStateIndex, -2f);
			if (flag)
			{
				if (Array.Exists<Levels>(Level.kingOfGamesLevels, (Levels level) => level == this.previousLevel))
				{
					levels = this.previousLevel;
					this.movePlayersToDialoguePositions();
				}
				else
				{
					levels = Levels.ChessPawn;
				}
			}
			else
			{
				levels = this.calculateCurrentLevel();
			}
		}
		if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) == 0f)
		{
			this.firstEntry = true;
		}
		if (!this.previouslyWon)
		{
			this.introCameraMovementCoroutine = base.StartCoroutine(this.panCamera_cr());
		}
		if (this.firstEntry || this.previouslyWon)
		{
			string text = ChessCastleLevel.LevelPrefixes[Array.IndexOf<Levels>(Level.kingOfGamesLevels, levels)];
			this.castleAnimator.Play(text + "Idle", ChessCastleLevel.CastleBaseLayer);
			this.castleAnimator.Play(text + "Open", ChessCastleLevel.CastleDoorLayer, 1f);
			if (levels == Levels.ChessBishop || levels == Levels.ChessQueen)
			{
				this.castleAnimator.Play(text, ChessCastleLevel.CastleFlairLayer, 1f);
			}
			this.platformAnimator.Play("Stop", ChessCastleLevel.PlatformBaseLayer, 1f);
			this.setTargetLevel(levels, false);
		}
		else
		{
			this.rotate(Levels.ChessPawn, levels, false, true);
		}
	}

	// Token: 0x060001A0 RID: 416 RVA: 0x00058ACC File Offset: 0x00056ECC
	protected override void Update()
	{
		base.Update();
		if (this.introCameraMovementCoroutine == null)
		{
			this.cameraSineAccumulator += CupheadTime.Delta;
			Vector3 manualFloat = new Vector3(0f, this.sineAmplitude * Mathf.Sin(this.cameraSineAccumulator / this.sinePeriod * 2f * 3.1415927f + 1.5707964f));
			CupheadLevelCamera.Current.SetManualFloat(manualFloat);
		}
	}

	// Token: 0x060001A1 RID: 417 RVA: 0x00058B43 File Offset: 0x00056F43
	protected override void OnLevelStart()
	{
		if (this.firstEntry)
		{
			base.StartCoroutine(this.firstEntry_cr());
		}
		else if (this.dialogueInteractionPoint.dialogueInteraction == DialoguerDialogues.KingOfGamesVictory_WDLC)
		{
			base.StartCoroutine(this.postWinEntry_cr());
		}
	}

	// Token: 0x060001A2 RID: 418 RVA: 0x00058B84 File Offset: 0x00056F84
	protected override void OnTransitionInComplete()
	{
		bool flag = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length;
		base.OnTransitionInComplete();
		if (flag)
		{
			AudioManager.StartBGMAlternate(1);
		}
		else if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) == 0f)
		{
			AudioManager.PlayBGM();
		}
		else
		{
			AudioManager.StartBGMAlternate(0);
		}
	}

	// Token: 0x060001A3 RID: 419 RVA: 0x00058BE8 File Offset: 0x00056FE8
	private IEnumerator panCamera_cr()
	{
		CupheadLevelCamera.Current.SetManualFloat(new Vector3(0f, -this.introPanAmount));
		while (!this.beginIntroPan)
		{
			yield return null;
		}
		float elapsedTime = 0f;
		while (elapsedTime < this.introPanDuration)
		{
			yield return null;
			elapsedTime += CupheadTime.Delta;
			CupheadLevelCamera.Current.SetManualFloat(new Vector3(0f, EaseUtils.EaseOutCubic(-this.introPanAmount, this.sineAmplitude, elapsedTime / this.introPanDuration)));
		}
		this.introCameraMovementCoroutine = null;
		yield break;
	}

	// Token: 0x060001A4 RID: 420 RVA: 0x00058C04 File Offset: 0x00057004
	private IEnumerator firstEntry_cr()
	{
		this.castleAnimator.Play(ChessCastleLevel.LevelPrefixes[0] + "Close", ChessCastleLevel.CastleDoorLayer, 1f);
		this.startEntity.enabled = false;
		AudioSource castleIntroMusic = GameObject.Find("MUS_CastleIntro").GetComponent<AudioSource>();
		while (castleIntroMusic.isPlaying)
		{
			yield return null;
		}
		AudioManager.StartBGMAlternate(0);
		yield break;
	}

	// Token: 0x060001A5 RID: 421 RVA: 0x00058C20 File Offset: 0x00057020
	private IEnumerator postWinEntry_cr()
	{
		AudioManager.StopBGM();
		Behaviour behaviour = this.startEntity;
		bool flag = false;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour.enabled = flag;
		yield return CupheadTime.WaitForSeconds(this, 0.35f);
		if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) != 0f)
		{
			AudioManager.StartBGMAlternate(0);
		}
		this.dialogueInteractionPoint.BeginDialogue();
		yield return CupheadTime.WaitForSeconds(this, 2f);
		Behaviour behaviour2 = this.startEntity;
		flag = true;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour2.enabled = flag;
		yield break;
	}

	// Token: 0x060001A6 RID: 422 RVA: 0x00058C3B File Offset: 0x0005703B
	private void rotate(Levels startLevel, Levels endLevel, bool gauntlet, bool intro)
	{
		if (this.rotationCoroutine != null)
		{
			return;
		}
		this.rotationCoroutine = base.StartCoroutine(this.rotate_cr(startLevel, endLevel, gauntlet, intro));
	}

	// Token: 0x060001A7 RID: 423 RVA: 0x00058C60 File Offset: 0x00057060
	private IEnumerator rotate_cr(Levels startLevel, Levels endLevel, bool gauntlet, bool intro)
	{
		if (this.gauntletSparklesCoroutine != null)
		{
			base.StopCoroutine(this.gauntletSparklesCoroutine);
			this.gauntletSparklesCoroutine = null;
		}
		int startIndex = Array.IndexOf<Levels>(Level.kingOfGamesLevels, startLevel);
		int destinationIndex = Array.IndexOf<Levels>(Level.kingOfGamesLevels, endLevel);
		string startPrefix = ChessCastleLevel.LevelPrefixes[startIndex];
		string endPrefix = ChessCastleLevel.LevelPrefixes[destinationIndex];
		Behaviour behaviour = this.startEntity;
		bool flag = false;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour.enabled = flag;
		if (intro)
		{
			float normalizedTime = (float)destinationIndex / (float)Level.kingOfGamesLevels.Length - 0.25f;
			this.castleAnimator.Play("FullRotation", ChessCastleLevel.CastleBaseLayer, normalizedTime);
			this.castleAnimator.Play("Off", ChessCastleLevel.CastleDoorLayer);
			this.kingAnimator.Play("LeverPull", 0, 0.2f);
		}
		else
		{
			this.castleAnimator.Play(startPrefix + "Close", ChessCastleLevel.CastleDoorLayer);
			AudioManager.Play(ChessCastleLevel.DoorSounds[startIndex] + "_close");
			this.kingAnimator.SetTrigger("PullLever");
			this.kingAnimator.SetBool("Talking", false);
			yield return this.kingAnimator.WaitForNormalizedTime(this, 0.6101695f, "LeverPull", 0, false, false, true);
			yield return this.castleAnimator.WaitForNormalizedTimeLooping(this, 0.9166667f, startPrefix + "Idle", ChessCastleLevel.CastleBaseLayer, true, false, true);
			this.castleAnimator.Play(startPrefix + "Start");
		}
		this.castleAnimator.SetInteger("Destination", destinationIndex);
		this.castleAnimator.SetBool("Gauntlet", gauntlet);
		this.castleAnimator.Play("Off", ChessCastleLevel.CastleFlairLayer);
		this.platformAnimator.Play("Start", ChessCastleLevel.PlatformBaseLayer);
		this.rotating = true;
		CupheadLevelCamera.Current.StartShake(2f);
		if (destinationIndex - startIndex != 1)
		{
			AudioManager.PlayLoop("sfx_dlc_kog_castle_kog_rotate_loop");
			AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_kog_rotate_loop", 0f, 0.2f, 0.2f);
		}
		else
		{
			AudioManager.Play("sfx_dlc_kog_castle_kog_rotate");
		}
		yield return this.castleAnimator.WaitForAnimationToStart(this, endPrefix + "Stop", ChessCastleLevel.CastleBaseLayer, false);
		AudioManager.FadeSFXVolumeLinear("sfx_dlc_kog_castle_kog_rotate_loop", 0f, 0.2f);
		AudioManager.Play("sfx_dlc_kog_castle_kog_roateend");
		yield return this.castleAnimator.WaitForNormalizedTime(this, 1f, endPrefix + "Stop", ChessCastleLevel.CastleBaseLayer, true, false, true);
		this.rotating = false;
		CupheadLevelCamera.Current.EndShake(0.2f);
		this.castleAnimator.Play(endPrefix + "Idle", ChessCastleLevel.CastleBaseLayer);
		this.castleAnimator.Play(endPrefix + "Open", ChessCastleLevel.CastleDoorLayer);
		AudioManager.Play(ChessCastleLevel.DoorSounds[destinationIndex] + "_open");
		if (endLevel == Levels.ChessPawn || endLevel == Levels.ChessBishop || endLevel == Levels.ChessRook || endLevel == Levels.ChessQueen)
		{
			this.castleAnimator.Play(endPrefix, ChessCastleLevel.CastleFlairLayer);
		}
		this.platformAnimator.Play("Stop", ChessCastleLevel.PlatformBaseLayer);
		this.setTargetLevel(endLevel, gauntlet);
		Behaviour behaviour2 = this.startEntity;
		flag = true;
		this.dialogueInteractionPoint.enabled = flag;
		flag = flag;
		this.exitEntity.enabled = flag;
		behaviour2.enabled = flag;
		this.rotationCoroutine = null;
		if (gauntlet)
		{
			this.gauntletSparklesCoroutine = base.StartCoroutine(this.gauntletSparkles_cr());
		}
		yield break;
	}

	// Token: 0x060001A8 RID: 424 RVA: 0x00058C98 File Offset: 0x00057098
	public void StartChessLevel()
	{
		base.StartCoroutine(this.startChessLevel_cr());
	}

	// Token: 0x060001A9 RID: 425 RVA: 0x00058CA8 File Offset: 0x000570A8
	private IEnumerator startChessLevel_cr()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.playerStartLevelEffects[0].gameObject.SetActive(true);
		this.playerStartLevelEffects[0].transform.position = player.transform.position;
		player.gameObject.SetActive(false);
		this.playerStartLevelEffects[0].animator.SetTrigger("OnStartTutorial");
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			this.playerStartLevelEffects[1].gameObject.SetActive(true);
			this.playerStartLevelEffects[1].transform.position = player.transform.position;
			player.gameObject.SetActive(false);
			this.playerStartLevelEffects[1].animator.SetTrigger("OnStartTutorial");
		}
		AudioManager.Play("sfx_dlc_kog_castle_kog_entercastle");
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		GauntletContext context = (!this.currentIsGauntlet) ? null : new GauntletContext(false);
		Levels level = this.currentTargetLevel;
		SceneLoader.Transition transitionStart = SceneLoader.Transition.Iris;
		GauntletContext context2 = context;
		SceneLoader.LoadLevel(level, transitionStart, SceneLoader.Icon.Hourglass, context2);
		yield break;
	}

	// Token: 0x060001AA RID: 426 RVA: 0x00058CC4 File Offset: 0x000570C4
	public void Exit()
	{
		foreach (AbstractPlayerController abstractPlayerController in PlayerManager.GetAllPlayers())
		{
			LevelPlayerController levelPlayerController = (LevelPlayerController)abstractPlayerController;
			if (!(levelPlayerController == null))
			{
				levelPlayerController.DisableInput();
			}
		}
		SceneLoader.LoadScene(Scenes.scene_map_world_DLC, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
	}

	// Token: 0x060001AB RID: 427 RVA: 0x00058D40 File Offset: 0x00057140
	private IEnumerator cloudSpawn_cr()
	{
		MinMax cloudSpawnTime = new MinMax(0.8f, 1.4f);
		for (;;)
		{
			float elapsedTime = 0f;
			float duration = cloudSpawnTime.RandomFloat();
			while (elapsedTime < ((!this.rotating) ? duration : (duration / this.rotationMultiplier)))
			{
				yield return null;
				elapsedTime += CupheadTime.Delta;
			}
			GameObject obj = UnityEngine.Object.Instantiate<GameObject>(this.cloudPrefab);
			obj.GetComponent<ChessCastleLevelCloud>().Initialize(this);
		}
		yield break;
	}

	// Token: 0x060001AC RID: 428 RVA: 0x00058D5B File Offset: 0x0005715B
	private void AnimateCoins(int count)
	{
		base.StartCoroutine(this.coin_cr(count));
	}

	// Token: 0x060001AD RID: 429 RVA: 0x00058D6C File Offset: 0x0005716C
	private IEnumerator coin_cr(int count)
	{
		for (int i = 0; i < count; i++)
		{
			string animationName = (i % 2 != 0) ? "CoinB" : "CoinA";
			this.kingAnimator.Play(animationName, 1);
			yield return this.kingAnimator.WaitForAnimationToEnd(this, animationName, 1, false, true);
			AudioManager.Play("sfx_coin_pickup");
			GameObject coinSpark = UnityEngine.Object.Instantiate<GameObject>(this.coinPrefab, this.coinSparkSpawnPoint.position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0f, 360f)));
			Renderer coinSparkRenderer = coinSpark.GetComponent<Renderer>();
			coinSparkRenderer.sortingLayerName = "Effects";
			coinSparkRenderer.sortingOrder = 50;
			coinSpark.GetComponent<Animator>().Play("anim_level_coin_death");
		}
		yield break;
	}

	// Token: 0x060001AE RID: 430 RVA: 0x00058D90 File Offset: 0x00057190
	private IEnumerator gauntletSparkles_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.35f);
			Vector3 position = this.sparklesCenter.position + UnityEngine.Random.insideUnitCircle * 70f;
			Effect effect = this.sparkleEffect.Create(position);
			effect.GetComponent<AnimationHelper>().Speed = 0.33333334f;
			effect.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
		}
		yield break;
	}

	// Token: 0x060001AF RID: 431 RVA: 0x00058DAB File Offset: 0x000571AB
	private void onDialogueStartedHandler()
	{
		base.Ending = true;
		AudioManager.Play("sfx_dlc_kog_castle_kingvoice");
	}

	// Token: 0x060001B0 RID: 432 RVA: 0x00058DC0 File Offset: 0x000571C0
	private void onDialogueMessageHandler(string message, string metadata)
	{
		if (message == "GiveCoins")
		{
			string[] array;
			if (ChessCastleLevel.Coins.TryGetValue(this.currentTargetLevel, out array))
			{
				this.AnimateCoins(array.Length);
			}
		}
		else if (message == "SetupChooseLevel")
		{
			this.setupChooseLevel();
		}
		else if (message == "ChooseLevel")
		{
			this.revertChooseLevel();
			if (metadata == "-1")
			{
				return;
			}
			int num;
			if (Parser.IntTryParse(metadata, out num))
			{
				if (num == 5)
				{
					this.rotate(this.currentTargetLevel, Levels.ChessPawn, true, false);
				}
				else
				{
					Levels endLevel = Level.kingOfGamesLevels[num];
					this.rotate(this.currentTargetLevel, endLevel, false, false);
				}
			}
		}
	}

	// Token: 0x060001B1 RID: 433 RVA: 0x00058E88 File Offset: 0x00057288
	private void onDialogueEndedHandler()
	{
		base.Ending = false;
		this.stopTalk();
		bool flag = SceneLoader.CurrentContext is GauntletContext && ((GauntletContext)SceneLoader.CurrentContext).complete;
		if (flag)
		{
			OnlineManager.Instance.Interface.UnlockAchievement(PlayerId.Any, OnlineAchievementData.DLC.DefeatKOGGauntlet);
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGames_WDLC;
		}
		else if (this.dialogueInteractionPoint.dialogueInteraction == DialoguerDialogues.KingOfGamesVictory_WDLC)
		{
			PlayerData.SaveCurrentFile();
			this.dialogueInteractionPoint.dialogueInteraction = DialoguerDialogues.KingOfGames_WDLC;
			if (this.attemptsToBeat < ChessCastleLevel.MaxAttemptsToContinue && Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) != (float)ChessCastleLevel.KingOfGamesFinalDialogueState)
			{
				this.rotate(this.currentTargetLevel, this.calculateCurrentLevel(), false, false);
			}
			else
			{
				this.Exit();
			}
		}
		else if (this.firstEntry)
		{
			PlayerData.SaveCurrentFile();
			if (!this.startEntity.enabled)
			{
				AudioManager.Play("sfx_dlc_kog_castle_door_wooddoor_open");
			}
			this.castleAnimator.Play(ChessCastleLevel.LevelPrefixes[0] + "Open", ChessCastleLevel.CastleDoorLayer);
			this.startEntity.enabled = true;
		}
	}

	// Token: 0x060001B2 RID: 434 RVA: 0x00058FBC File Offset: 0x000573BC
	private void onDialogueAdvancedHandler(DialoguerTextData data)
	{
		if (!this.kingAnimator.GetCurrentAnimatorStateInfo(0).IsName("Talk"))
		{
			this.kingAnimator.SetTrigger("Talk");
		}
	}

	// Token: 0x060001B3 RID: 435 RVA: 0x00058FF7 File Offset: 0x000573F7
	public void StartTalkAnimation()
	{
		this.kingAnimator.SetBool("Talking", true);
	}

	// Token: 0x060001B4 RID: 436 RVA: 0x0005900A File Offset: 0x0005740A
	public void EndTalkAnimation()
	{
		this.kingAnimator.SetBool("Talking", false);
	}

	// Token: 0x060001B5 RID: 437 RVA: 0x0005901D File Offset: 0x0005741D
	private void onIntroEventHandler()
	{
		this.beginIntroPan = true;
	}

	// Token: 0x060001B6 RID: 438 RVA: 0x00059026 File Offset: 0x00057426
	private void onFadeOutStartEventHandler(float time)
	{
		this.beginIntroPan = true;
	}

	// Token: 0x060001B7 RID: 439 RVA: 0x0005902F File Offset: 0x0005742F
	private void stopTalk()
	{
		this.kingAnimator.SetBool("Talking", false);
	}

	// Token: 0x060001B8 RID: 440 RVA: 0x00059044 File Offset: 0x00057444
	private void updateDialogueState()
	{
		int num = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels);
		if (num == 1)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 2f);
		}
		else if (num == 2)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 3f);
		}
		else if (num == 3)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 4f);
		}
		else if (num == 4)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, 5f);
		}
		else if (Dialoguer.GetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex) == 7f)
		{
			Dialoguer.SetGlobalFloat(ChessCastleLevel.KingOfGamesDialoguerStateIndex, (float)ChessCastleLevel.KingOfGamesFinalDialogueState);
		}
	}

	// Token: 0x060001B9 RID: 441 RVA: 0x000590F4 File Offset: 0x000574F4
	private Levels calculateCurrentLevel()
	{
		foreach (Levels levels in Level.kingOfGamesLevels)
		{
			if (!PlayerData.Data.CheckLevelCompleted(levels))
			{
				return levels;
			}
		}
		return Level.kingOfGamesLevels.GetLast<Levels>();
	}

	// Token: 0x060001BA RID: 442 RVA: 0x0005913C File Offset: 0x0005753C
	private Levels calculatePreviousLevel()
	{
		Levels[] kingOfGamesLevels = Level.kingOfGamesLevels;
		if (!PlayerData.Data.CheckLevelCompleted(kingOfGamesLevels[0]))
		{
			return Levels.Test;
		}
		for (int i = 1; i < Level.kingOfGamesLevels.Length; i++)
		{
			if (!PlayerData.Data.CheckLevelCompleted(kingOfGamesLevels[i]))
			{
				return kingOfGamesLevels[i - 1];
			}
		}
		return Levels.Test;
	}

	// Token: 0x060001BB RID: 443 RVA: 0x00059194 File Offset: 0x00057594
	private void setTargetLevel(Levels level, bool gauntlet)
	{
		this.currentTargetLevel = level;
		this.currentIsGauntlet = gauntlet;
	}

	// Token: 0x060001BC RID: 444 RVA: 0x000591A4 File Offset: 0x000575A4
	private void setupChooseLevel()
	{
		int i = Array.IndexOf<Levels>(Level.kingOfGamesLevels, this.currentTargetLevel);
		this.speechBubble.HideOptionByIndex(i);
		Vector2 basePosition = this.speechBubbleBasePosition;
		basePosition.y -= 130f;
		this.speechBubble.basePosition = basePosition;
	}

	// Token: 0x060001BD RID: 445 RVA: 0x000591F4 File Offset: 0x000575F4
	private void revertChooseLevel()
	{
		this.speechBubble.basePosition = this.speechBubbleBasePosition;
	}

	// Token: 0x060001BE RID: 446 RVA: 0x00059208 File Offset: 0x00057608
	private void movePlayersToDialoguePositions()
	{
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		if (player != null)
		{
			Vector3 position = player.transform.position;
			position.x = this.dialogueInteractionPoint.playerOneDialoguePosition.x;
			player.transform.position = position;
		}
		player = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (player != null)
		{
			Vector3 position2 = player.transform.position;
			position2.x = this.dialogueInteractionPoint.playerTwoDialoguePosition.x;
			player.transform.position = position2;
		}
	}

	// Token: 0x0400030A RID: 778
	private LevelProperties.ChessCastle properties;

	// Token: 0x0400030B RID: 779
	private static readonly int MaxAttemptsToContinue = 5;

	// Token: 0x0400030C RID: 780
	private static readonly int KingOfGamesDialoguerStateIndex = 36;

	// Token: 0x0400030D RID: 781
	private static readonly int KingOfGamesVictoryDialoguerCountIndex = 37;

	// Token: 0x0400030E RID: 782
	private static readonly int KingOfGamesVictoryDialoguerStateIndex = 42;

	// Token: 0x0400030F RID: 783
	private static readonly int KingOfGamesFinalDialogueState = 6;

	// Token: 0x04000310 RID: 784
	private static readonly int CastleBaseLayer = 0;

	// Token: 0x04000311 RID: 785
	private static readonly int CastleDoorLayer = 1;

	// Token: 0x04000312 RID: 786
	private static readonly int CastleFlairLayer = 2;

	// Token: 0x04000313 RID: 787
	private static readonly string[] LevelPrefixes = new string[]
	{
		"Pawn",
		"Knight",
		"Bishop",
		"Rook",
		"Queen"
	};

	// Token: 0x04000314 RID: 788
	private static readonly int PlatformBaseLayer = 0;

	// Token: 0x04000315 RID: 789
	public static readonly Dictionary<Levels, string[]> Coins = new Dictionary<Levels, string[]>
	{
		{
			Levels.ChessPawn,
			new string[]
			{
				"a37b3d37-a32e-4b88-a583-34489496494d",
				"25f15554-d229-4330-96cc-ac8a13c18ea0"
			}
		},
		{
			Levels.ChessKnight,
			new string[]
			{
				"eacf4228-e200-4839-9d79-3439cfcc5824",
				"47f7edb1-b5c5-4afb-9acb-a46f5e6df557"
			}
		},
		{
			Levels.ChessBishop,
			new string[]
			{
				"3826615a-498b-4158-af7b-0d01acbc18c8",
				"d52b1cc6-414c-4a7c-9f8a-250316566d58"
			}
		},
		{
			Levels.ChessRook,
			new string[]
			{
				"fc2c48cd-5dec-472a-ae18-dccfc94232c6",
				"16732bc8-7230-467a-a9ac-ff9c62ab7657"
			}
		},
		{
			Levels.ChessQueen,
			new string[]
			{
				"e0c6e8bc-0c56-4e52-a9a1-c53887f5ca4c",
				"19090606-09e8-4e56-92ac-e08200926b94",
				"39bfe6d8-0dbc-4886-9998-52c67b57969e"
			}
		}
	};

	// Token: 0x04000316 RID: 790
	private static readonly string[] DoorSounds = new string[]
	{
		"sfx_dlc_kog_castle_door_wooddoor",
		"sfx_dlc_kog_castle_door_drawbridge",
		"sfx_dlc_kog_castle_door_tall",
		"sfx_dlc_kog_castle_door_portcullis",
		"sfx_dlc_kog_castle_door_queen"
	};

	// Token: 0x04000317 RID: 791
	[Header("Boss Info")]
	[SerializeField]
	private Sprite _bossPortrait;

	// Token: 0x04000318 RID: 792
	[SerializeField]
	[Multiline]
	private string _bossQuote;

	// Token: 0x04000319 RID: 793
	[SerializeField]
	private AbstractLevelInteractiveEntity startEntity;

	// Token: 0x0400031A RID: 794
	[SerializeField]
	private AbstractLevelInteractiveEntity exitEntity;

	// Token: 0x0400031B RID: 795
	[SerializeField]
	private PlayerDeathEffect[] playerStartLevelEffects;

	// Token: 0x0400031C RID: 796
	[SerializeField]
	private ChessCastleLevelKingInteractionPoint dialogueInteractionPoint;

	// Token: 0x0400031D RID: 797
	[SerializeField]
	private SpeechBubble speechBubble;

	// Token: 0x0400031E RID: 798
	[SerializeField]
	private Animator castleAnimator;

	// Token: 0x0400031F RID: 799
	[SerializeField]
	private Animator platformAnimator;

	// Token: 0x04000320 RID: 800
	[SerializeField]
	private GameObject cloudPrefab;

	// Token: 0x04000321 RID: 801
	[SerializeField]
	private GameObject coinPrefab;

	// Token: 0x04000322 RID: 802
	[SerializeField]
	private Transform coinSparkSpawnPoint;

	// Token: 0x04000323 RID: 803
	[SerializeField]
	private Animator kingAnimator;

	// Token: 0x04000324 RID: 804
	[SerializeField]
	private Effect sparkleEffect;

	// Token: 0x04000325 RID: 805
	[SerializeField]
	private Transform sparklesCenter;

	// Token: 0x04000326 RID: 806
	[SerializeField]
	private float sinePeriod;

	// Token: 0x04000327 RID: 807
	[SerializeField]
	private float sineAmplitude;

	// Token: 0x04000328 RID: 808
	[SerializeField]
	private float _rotationMultiplier;

	// Token: 0x04000329 RID: 809
	[SerializeField]
	private float introPanAmount;

	// Token: 0x0400032A RID: 810
	[SerializeField]
	private float introPanDuration;

	// Token: 0x0400032B RID: 811
	private bool firstEntry;

	// Token: 0x0400032C RID: 812
	private Levels previousLevel;

	// Token: 0x0400032D RID: 813
	private bool previouslyWon;

	// Token: 0x0400032E RID: 814
	private int attemptsToBeat;

	// Token: 0x0400032F RID: 815
	private Levels currentTargetLevel;

	// Token: 0x04000330 RID: 816
	private bool currentIsGauntlet;

	// Token: 0x04000331 RID: 817
	private Coroutine rotationCoroutine;

	// Token: 0x04000332 RID: 818
	private Coroutine gauntletSparklesCoroutine;

	// Token: 0x04000333 RID: 819
	private Vector2 speechBubbleBasePosition;

	// Token: 0x04000334 RID: 820
	private float cameraSineAccumulator;

	// Token: 0x04000335 RID: 821
	private Coroutine introCameraMovementCoroutine;

	// Token: 0x04000336 RID: 822
	private bool beginIntroPan;
}
