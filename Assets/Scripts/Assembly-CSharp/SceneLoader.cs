using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;

// Token: 0x02000AFC RID: 2812
public class SceneLoader : AbstractMonoBehaviour
{
	// Token: 0x06004417 RID: 17431 RVA: 0x00240E2B File Offset: 0x0023F22B
	static SceneLoader()
	{
		SceneLoader.CurrentLevel = Levels.Veggies;
		SceneLoader.properties = new SceneLoader.Properties();
	}

	// Token: 0x17000616 RID: 1558
	// (get) Token: 0x06004419 RID: 17433 RVA: 0x00240E71 File Offset: 0x0023F271
	public static bool Exists
	{
		get
		{
			return SceneLoader._instance != null;
		}
	}

	// Token: 0x17000617 RID: 1559
	// (get) Token: 0x0600441A RID: 17434 RVA: 0x00240E7E File Offset: 0x0023F27E
	public static SceneLoader instance
	{
		get
		{
			if (SceneLoader._instance == null)
			{
				SceneLoader._instance = (UnityEngine.Object.Instantiate(Resources.Load("UI/Scene_Loader")) as GameObject).GetComponent<SceneLoader>();
			}
			return SceneLoader._instance;
		}
	}

	// Token: 0x17000618 RID: 1560
	// (get) Token: 0x0600441B RID: 17435 RVA: 0x00240EB3 File Offset: 0x0023F2B3
	// (set) Token: 0x0600441C RID: 17436 RVA: 0x00240EBA File Offset: 0x0023F2BA
	public static Levels CurrentLevel { get; private set; }

	// Token: 0x17000619 RID: 1561
	// (get) Token: 0x0600441D RID: 17437 RVA: 0x00240EC2 File Offset: 0x0023F2C2
	// (set) Token: 0x0600441E RID: 17438 RVA: 0x00240EC9 File Offset: 0x0023F2C9
	public static string SceneName { get; private set; } = string.Empty;

	// Token: 0x1700061A RID: 1562
	// (get) Token: 0x0600441F RID: 17439 RVA: 0x00240ED1 File Offset: 0x0023F2D1
	// (set) Token: 0x06004420 RID: 17440 RVA: 0x00240ED8 File Offset: 0x0023F2D8
	public static SceneLoader.Properties properties { get; private set; }

	// Token: 0x1700061B RID: 1563
	// (get) Token: 0x06004421 RID: 17441 RVA: 0x00240EE0 File Offset: 0x0023F2E0
	// (set) Token: 0x06004422 RID: 17442 RVA: 0x00240EE7 File Offset: 0x0023F2E7
	public static SceneLoader.Context CurrentContext { get; private set; }

	// Token: 0x1700061C RID: 1564
	// (get) Token: 0x06004423 RID: 17443 RVA: 0x00240EEF File Offset: 0x0023F2EF
	public static bool CurrentlyLoading
	{
		get
		{
			return SceneLoader.currentlyLoading;
		}
	}

	// Token: 0x06004424 RID: 17444 RVA: 0x00240EF8 File Offset: 0x0023F2F8
	public static void LoadScene(string sceneName, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass, SceneLoader.Context context = null)
	{
		Scenes scene = Scenes.scene_start;
		if (!EnumUtils.TryParse<Scenes>(sceneName, out scene))
		{
			return;
		}
		SceneLoader.LoadScene(scene, transitionStart, transitionEnd, icon, context);
	}

	// Token: 0x06004425 RID: 17445 RVA: 0x00240F20 File Offset: 0x0023F320
	public static void LoadScene(Scenes scene, SceneLoader.Transition transitionStart, SceneLoader.Transition transitionEnd, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass, SceneLoader.Context context = null)
	{
		if (SceneLoader.currentlyLoading)
		{
			return;
		}
		InterruptingPrompt.SetCanInterrupt(false);
		SceneLoader.properties.transitionStart = transitionStart;
		SceneLoader.properties.transitionEnd = transitionEnd;
		SceneLoader.properties.icon = icon;
		SceneLoader.EndTransitionDelay = 0.6f;
		SceneLoader.previousSceneName = SceneLoader.SceneName;
		SceneLoader.SceneName = scene.ToString();
		SceneLoader.CurrentContext = context;
		SceneLoader.instance.load();
	}

	// Token: 0x06004426 RID: 17446 RVA: 0x00240F96 File Offset: 0x0023F396
	public static void LoadLevel(Levels level, SceneLoader.Transition transitionStart, SceneLoader.Icon icon = SceneLoader.Icon.Hourglass, SceneLoader.Context context = null)
	{
		SceneLoader.CurrentLevel = level;
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(level), transitionStart, SceneLoader.Transition.Iris, icon, context);
	}

	// Token: 0x06004427 RID: 17447 RVA: 0x00240FB0 File Offset: 0x0023F3B0
	public static void LoadDicePalaceLevel(DicePalaceLevels dicePalaceLevel)
	{
		Levels dicePalaceLevel2 = LevelProperties.GetDicePalaceLevel(dicePalaceLevel);
		SceneLoader.CurrentLevel = dicePalaceLevel2;
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(dicePalaceLevel2), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x06004428 RID: 17448 RVA: 0x00240FD9 File Offset: 0x0023F3D9
	public static void SetCurrentLevel(Levels level)
	{
		SceneLoader.CurrentLevel = level;
	}

	// Token: 0x06004429 RID: 17449 RVA: 0x00240FE4 File Offset: 0x0023F3E4
	public static void ContinueTowerOfPower()
	{
		int current_TURN = TowerOfPowerLevelGameInfo.CURRENT_TURN;
		int turn_COUNTER = TowerOfPowerLevelGameInfo.TURN_COUNTER;
		if (current_TURN == turn_COUNTER)
		{
			TowerOfPowerLevelGameInfo.TURN_COUNTER++;
		}
		if (TowerOfPowerLevelGameInfo.TURN_COUNTER == TowerOfPowerLevelGameInfo.allStageSpaces.Count)
		{
			TowerOfPowerLevelGameInfo.GameInfo.CleanUp();
			SceneLoader.LoadLastMap();
		}
		else
		{
			SceneLoader.LoadScene(LevelProperties.GetLevelScene(Levels.TowerOfPower), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		}
	}

	// Token: 0x0600442A RID: 17450 RVA: 0x0024104B File Offset: 0x0023F44B
	public static void ResetTheTowerOfPower()
	{
		TowerOfPowerLevelGameInfo.ResetTowerOfPower();
		SceneLoader.LoadScene(LevelProperties.GetLevelScene(Levels.TowerOfPower), SceneLoader.Transition.Fade, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
	}

	// Token: 0x0600442B RID: 17451 RVA: 0x00241068 File Offset: 0x0023F468
	public static void ReloadLevel()
	{
		if (Level.IsTowerOfPower)
		{
			if (TowerOfPowerLevelGameInfo.IsTokenLeft(0))
			{
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP = 3;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].BonusHP = 3;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].SuperCharge = 0f;
				TowerOfPowerLevelGameInfo.ReduceToken(0);
			}
			else
			{
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].HP = 0;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].BonusHP = 0;
				TowerOfPowerLevelGameInfo.PLAYER_STATS[0].SuperCharge = 0f;
			}
			if (PlayerManager.Multiplayer)
			{
				if (TowerOfPowerLevelGameInfo.IsTokenLeft(1))
				{
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].HP = 3;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].BonusHP = 3;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].SuperCharge = 0f;
					TowerOfPowerLevelGameInfo.ReduceToken(1);
				}
				else
				{
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].HP = 0;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].BonusHP = 0;
					TowerOfPowerLevelGameInfo.PLAYER_STATS[1].SuperCharge = 0f;
				}
			}
		}
		else
		{
			if (Level.IsDicePalace)
			{
				SceneLoader.LoadDicePalaceLevel(DicePalaceLevels.DicePalaceMain);
				return;
			}
			if (Level.IsGraveyard)
			{
				SceneLoader.LoadScene(LevelProperties.GetLevelScene(SceneLoader.CurrentLevel), SceneLoader.Transition.Fade, SceneLoader.Transition.Blur, SceneLoader.Icon.None, null);
				return;
			}
			if (Level.IsChessBoss)
			{
				if (SceneLoader.CurrentContext is GauntletContext && !((GauntletContext)SceneLoader.CurrentContext).complete)
				{
					Scenes scene = Scenes.scene_level_chess_pawn;
					SceneLoader.Transition transitionStart = SceneLoader.Transition.Fade;
					SceneLoader.Transition transitionEnd = SceneLoader.Transition.Iris;
					GauntletContext context = new GauntletContext(false);
					SceneLoader.LoadScene(scene, transitionStart, transitionEnd, SceneLoader.Icon.Hourglass, context);
					return;
				}
				PlayerData.Data.IncrementKingOfGamesCounter();
				PlayerData.SaveCurrentFile();
			}
		}
		float transitionStartTime = SceneLoader.properties.transitionStartTime;
		SceneLoader.properties.transitionStartTime = 0.25f;
		SceneLoader.LoadLevel(SceneLoader.CurrentLevel, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
		SceneLoader.properties.transitionStartTime = transitionStartTime;
	}

	// Token: 0x0600442C RID: 17452 RVA: 0x00241224 File Offset: 0x0023F624
	public static void LoadLastMap()
	{
		if (Level.IsGraveyard)
		{
			SceneLoader.LoadScene(PlayerData.Data.CurrentMap, SceneLoader.Transition.Fade, SceneLoader.Transition.Blur, SceneLoader.Icon.Hourglass, null);
			SceneLoader.IsInBlurTransition = true;
		}
		else
		{
			Scenes scene = PlayerData.Data.CurrentMap;
			if (Level.IsChessBoss)
			{
				PlayerData.Data.IncrementKingOfGamesCounter();
				PlayerData.SaveCurrentFile();
				bool flag = PlayerData.Data.CountLevelsCompleted(Level.kingOfGamesLevels) == Level.kingOfGamesLevels.Length;
				if (flag)
				{
					scene = Scenes.scene_level_chess_castle;
				}
			}
			SceneLoader.LoadScene(scene, SceneLoader.Transition.Iris, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		}
	}

	// Token: 0x0600442D RID: 17453 RVA: 0x002412A9 File Offset: 0x0023F6A9
	public static void TransitionOut()
	{
		SceneLoader.TransitionOut(SceneLoader.properties.transitionStart);
	}

	// Token: 0x0600442E RID: 17454 RVA: 0x002412BA File Offset: 0x0023F6BA
	public static void TransitionOut(SceneLoader.Transition transition)
	{
		SceneLoader.TransitionOut(transition, SceneLoader.properties.transitionStartTime);
	}

	// Token: 0x0600442F RID: 17455 RVA: 0x002412CC File Offset: 0x0023F6CC
	public static void TransitionOut(SceneLoader.Transition transition, float time)
	{
		SceneLoader.properties.transitionStart = transition;
		SceneLoader.properties.transitionStartTime = time;
		SceneLoader.instance.Out();
	}

	// Token: 0x140000C0 RID: 192
	// (add) Token: 0x06004430 RID: 17456 RVA: 0x002412F0 File Offset: 0x0023F6F0
	// (remove) Token: 0x06004431 RID: 17457 RVA: 0x00241324 File Offset: 0x0023F724
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event SceneLoader.FadeHandler OnFadeInStartEvent;

	// Token: 0x140000C1 RID: 193
	// (add) Token: 0x06004432 RID: 17458 RVA: 0x00241358 File Offset: 0x0023F758
	// (remove) Token: 0x06004433 RID: 17459 RVA: 0x0024138C File Offset: 0x0023F78C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnFadeInEndEvent;

	// Token: 0x140000C2 RID: 194
	// (add) Token: 0x06004434 RID: 17460 RVA: 0x002413C0 File Offset: 0x0023F7C0
	// (remove) Token: 0x06004435 RID: 17461 RVA: 0x002413F4 File Offset: 0x0023F7F4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event SceneLoader.FadeHandler OnFadeOutStartEvent;

	// Token: 0x140000C3 RID: 195
	// (add) Token: 0x06004436 RID: 17462 RVA: 0x00241428 File Offset: 0x0023F828
	// (remove) Token: 0x06004437 RID: 17463 RVA: 0x0024145C File Offset: 0x0023F85C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnFadeOutEndEvent;

	// Token: 0x140000C4 RID: 196
	// (add) Token: 0x06004438 RID: 17464 RVA: 0x00241490 File Offset: 0x0023F890
	// (remove) Token: 0x06004439 RID: 17465 RVA: 0x002414C4 File Offset: 0x0023F8C4
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event SceneLoader.FadeHandler OnFaderValue;

	// Token: 0x140000C5 RID: 197
	// (add) Token: 0x0600443A RID: 17466 RVA: 0x002414F8 File Offset: 0x0023F8F8
	// (remove) Token: 0x0600443B RID: 17467 RVA: 0x0024152C File Offset: 0x0023F92C
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public static event Action OnLoaderCompleteEvent;

	// Token: 0x0600443C RID: 17468 RVA: 0x00241560 File Offset: 0x0023F960
	protected override void Awake()
	{
		base.Awake();
		SceneLoader._instance = this;
		this.SetIconAlpha(0f);
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x0600443D RID: 17469 RVA: 0x00241584 File Offset: 0x0023F984
	private void load()
	{
		if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
		{
			AudioManager.HandleSnapshot(AudioManager.Snapshots.Loadscreen.ToString(), 5f);
		}
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x0600443E RID: 17470 RVA: 0x002415F4 File Offset: 0x0023F9F4
	private void In()
	{
		base.StartCoroutine(this.in_cr());
	}

	// Token: 0x0600443F RID: 17471 RVA: 0x00241603 File Offset: 0x0023FA03
	private void Out()
	{
		if (!base.gameObject.activeInHierarchy)
		{
			if (SceneLoader.OnFadeOutEndEvent != null)
			{
				SceneLoader.OnFadeOutEndEvent();
			}
			return;
		}
		base.StartCoroutine(this.out_cr());
	}

	// Token: 0x06004440 RID: 17472 RVA: 0x00241637 File Offset: 0x0023FA37
	private void UpdateProgress(float progress)
	{
	}

	// Token: 0x06004441 RID: 17473 RVA: 0x00241639 File Offset: 0x0023FA39
	private void SetIconAlpha(float a)
	{
		this.SetImageAlpha(this.icon, a);
	}

	// Token: 0x06004442 RID: 17474 RVA: 0x00241648 File Offset: 0x0023FA48
	private void SetFaderAlpha(float a)
	{
		this.SetImageAlpha(this.fader, a);
	}

	// Token: 0x06004443 RID: 17475 RVA: 0x00241658 File Offset: 0x0023FA58
	private void SetImageAlpha(Image i, float a)
	{
		Color color = i.color;
		color.a = a;
		i.color = color;
	}

	// Token: 0x06004444 RID: 17476 RVA: 0x0024167C File Offset: 0x0023FA7C
	private IEnumerator loop_cr()
	{
		SceneLoader.currentlyLoading = true;
		yield return base.StartCoroutine(this.in_cr());
		base.StartCoroutine(this.load_cr());
		yield return base.StartCoroutine(this.iconFadeIn_cr());
		while (!this.doneLoadingSceneAsync)
		{
			yield return null;
		}
		if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString())
		{
			AudioManager.SnapshotReset(SceneLoader.SceneName, 0.15f);
		}
		AsyncOperation op = Resources.UnloadUnusedAssets();
		while (!op.isDone)
		{
			yield return null;
		}
		yield return base.StartCoroutine(this.iconFadeOut_cr());
		yield return base.StartCoroutine(this.out_cr());
		SceneLoader.properties.Reset();
		SceneLoader.currentlyLoading = false;
		yield break;
	}

	// Token: 0x06004445 RID: 17477 RVA: 0x00241698 File Offset: 0x0023FA98
	private IEnumerator load_cr()
	{
		this.doneLoadingSceneAsync = false;
		GC.Collect();
		if (SceneLoader.SceneName != SceneLoader.previousSceneName && SceneLoader.SceneName != Scenes.scene_slot_select.ToString())
		{
			string text = null;
			if (!Array.Exists<Levels>(Level.kingOfGamesLevelsWithCastle, (Levels level) => LevelProperties.GetLevelScene(level) == SceneLoader.SceneName))
			{
				text = Scenes.scene_level_chess_castle.ToString();
			}
			AssetBundleLoader.UnloadAssetBundles();
			AssetLoader<SpriteAtlas>.UnloadAssets(new string[]
			{
				text
			});
			if (SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
			{
				AssetLoader<AudioClip>.UnloadAssets(new string[0]);
			}
			AssetLoader<Texture2D[]>.UnloadAssets(new string[0]);
		}
		if (SceneLoader.SceneName == Scenes.scene_title.ToString())
		{
			DLCManager.RefreshDLC();
		}
		AssetLoaderOption atlasOption = AssetLoaderOption.None();
		if (SceneLoader.SceneName == Scenes.scene_level_chess_castle.ToString())
		{
			atlasOption = AssetLoaderOption.PersistInCacheTagged(SceneLoader.SceneName);
		}
		string[] preloadAtlases = AssetLoader<SpriteAtlas>.GetPreloadAssetNames(SceneLoader.SceneName);
		string[] preloadMusic = AssetLoader<AudioClip>.GetPreloadAssetNames(SceneLoader.SceneName);
		if (SceneLoader.SceneName != SceneLoader.previousSceneName && (preloadAtlases.Length > 0 || preloadMusic.Length > 0))
		{
			AsyncOperation intermediateSceneAsyncOp = SceneManager.LoadSceneAsync(this.LOAD_SCENE_NAME);
			while (!intermediateSceneAsyncOp.isDone)
			{
				yield return null;
			}
			for (int i = 0; i < preloadAtlases.Length; i++)
			{
				yield return AssetLoader<SpriteAtlas>.LoadAsset(preloadAtlases[i], atlasOption);
			}
			AssetLoaderOption musicOption = AssetLoaderOption.None();
			for (int j = 0; j < preloadMusic.Length; j++)
			{
				yield return AssetLoader<AudioClip>.LoadAsset(preloadMusic[j], musicOption);
			}
			Coroutine[] persistentAssetsCoroutines = DLCManager.LoadPersistentAssets();
			if (persistentAssetsCoroutines != null)
			{
				for (int k = 0; k < persistentAssetsCoroutines.Length; k++)
				{
					yield return persistentAssetsCoroutines[k];
				}
			}
			yield return null;
		}
		AsyncOperation async = SceneManager.LoadSceneAsync(SceneLoader.SceneName);
		while (!async.isDone || AssetBundleLoader.loadCounter > 0)
		{
			this.UpdateProgress(async.progress);
			yield return null;
		}
		this.doneLoadingSceneAsync = true;
		yield break;
	}

	// Token: 0x06004446 RID: 17478 RVA: 0x002416B4 File Offset: 0x0023FAB4
	private IEnumerator in_cr()
	{
		switch (SceneLoader.properties.transitionStart)
		{
		default:
			if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
			{
				this.FadeOutBGM(0.6f);
			}
			yield return base.StartCoroutine(this.irisIn_cr());
			break;
		case SceneLoader.Transition.Fade:
			if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && SceneLoader.SceneName != Scenes.scene_level_graveyard.ToString() && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString() && (SceneLoader.CurrentLevel != Levels.Saltbaker || SceneLoader.SceneName != Scenes.scene_win.ToString()))
			{
				this.FadeOutBGM(SceneLoader.properties.transitionEndTime);
			}
			yield return base.StartCoroutine(this.faderFadeIn_cr());
			break;
		case SceneLoader.Transition.Blur:
			yield return base.StartCoroutine(this.blurIn_cr());
			break;
		case SceneLoader.Transition.None:
			this.SetFaderAlpha(1f);
			break;
		}
		yield break;
	}

	// Token: 0x06004447 RID: 17479 RVA: 0x002416D0 File Offset: 0x0023FAD0
	private IEnumerator out_cr()
	{
		yield return null;
		switch (SceneLoader.properties.transitionEnd)
		{
		default:
			yield return base.StartCoroutine(this.irisOut_cr());
			break;
		case SceneLoader.Transition.Fade:
			yield return base.StartCoroutine(this.faderFadeOut_cr());
			break;
		case SceneLoader.Transition.Blur:
			yield return base.StartCoroutine(this.blurOut_cr());
			break;
		case SceneLoader.Transition.None:
			this.SetFaderAlpha(0f);
			break;
		}
		if (SceneLoader.SceneName != Scenes.scene_slot_select.ToString() && !Level.IsGraveyard && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
		{
			this.ResetBgmVolume();
		}
		if (SceneLoader.OnLoaderCompleteEvent != null)
		{
			SceneLoader.OnLoaderCompleteEvent();
		}
		SceneLoader.OnLoaderCompleteEvent = null;
		yield break;
	}

	// Token: 0x06004448 RID: 17480 RVA: 0x002416EC File Offset: 0x0023FAEC
	private IEnumerator irisIn_cr()
	{
		SceneLoader.IsInIrisTransition = true;
		Animator animator = this.fader.GetComponent<Animator>();
		animator.SetTrigger("Iris_In");
		this.SetFaderAlpha(1f);
		if (SceneLoader.OnFadeInStartEvent != null)
		{
			SceneLoader.OnFadeInStartEvent(0.6f);
		}
		yield return new WaitForSeconds(0.6f);
		if (SceneLoader.OnFadeInEndEvent != null)
		{
			SceneLoader.OnFadeInEndEvent();
		}
		yield break;
	}

	// Token: 0x06004449 RID: 17481 RVA: 0x00241708 File Offset: 0x0023FB08
	private IEnumerator irisOut_cr()
	{
		Animator animator = this.fader.GetComponent<Animator>();
		animator.SetTrigger("Iris_Out");
		this.SetFaderAlpha(1f);
		if (SceneLoader.OnFadeOutStartEvent != null)
		{
			SceneLoader.OnFadeOutStartEvent(0.6f);
		}
		yield return new WaitForSeconds(0.6f);
		if (SceneLoader.OnFadeOutEndEvent != null)
		{
			SceneLoader.OnFadeOutEndEvent();
		}
		SceneLoader.IsInIrisTransition = false;
		yield break;
	}

	// Token: 0x0600444A RID: 17482 RVA: 0x00241724 File Offset: 0x0023FB24
	private IEnumerator faderFadeIn_cr()
	{
		SceneLoader.IsInIrisTransition = false;
		this.SetFaderAlpha(0f);
		Animator animator = this.fader.GetComponent<Animator>();
		animator.SetTrigger("Black");
		if (SceneLoader.OnFadeInStartEvent != null)
		{
			SceneLoader.OnFadeInStartEvent(SceneLoader.properties.transitionStartTime);
		}
		yield return base.StartCoroutine(this.imageFade_cr(this.fader, SceneLoader.properties.transitionStartTime, 0f, 1f, false));
		if (SceneLoader.OnFadeInEndEvent != null)
		{
			SceneLoader.OnFadeInEndEvent();
		}
		yield break;
	}

	// Token: 0x0600444B RID: 17483 RVA: 0x00241740 File Offset: 0x0023FB40
	private IEnumerator faderFadeOut_cr()
	{
		if (SceneLoader.OnFadeOutStartEvent != null)
		{
			SceneLoader.OnFadeOutStartEvent(SceneLoader.properties.transitionEndTime);
		}
		yield return base.StartCoroutine(this.imageFade_cr(this.fader, SceneLoader.properties.transitionEndTime, 1f, 0f, false));
		if (SceneLoader.OnFadeOutEndEvent != null)
		{
			SceneLoader.OnFadeOutEndEvent();
		}
		yield break;
	}

	// Token: 0x0600444C RID: 17484 RVA: 0x0024175C File Offset: 0x0023FB5C
	private IEnumerator blurIn_cr()
	{
		SceneLoader.IsInBlurTransition = true;
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		AbstractCupheadGameCamera cam = (!(CupheadLevelCamera.Current != null)) ? CupheadMapCamera.Current : CupheadLevelCamera.Current;
		cam.StartBlur(0.5f, 2f);
		AudioManager.ChangeBGMPitch(0.9f, 0.5f);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		cam.EndBlur(0.5f);
		AudioManager.ChangeBGMPitch(1f, 0.5f);
		yield return CupheadTime.WaitForSeconds(this, 1f);
		cam.StartBlur(3f, 5f);
		AudioManager.ChangeBGMPitch(0.7f, 7f);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		SceneLoader.properties.transitionStartTime = 3f;
		this.FadeOutBGM(6f);
		yield return base.StartCoroutine(this.faderFadeIn_cr());
		yield break;
	}

	// Token: 0x0600444D RID: 17485 RVA: 0x00241778 File Offset: 0x0023FB78
	private IEnumerator blurOut_cr()
	{
		SceneLoader.IsInBlurTransition = true;
		AbstractCupheadGameCamera cam = (!(CupheadLevelCamera.Current != null)) ? CupheadMapCamera.Current : CupheadLevelCamera.Current;
		cam.StartBlur(0.01f, 5f);
		yield return new WaitForSeconds(0.015f);
		cam.EndBlur(2.5f, 5f);
		SceneLoader.properties.transitionEndTime = 2f;
		yield return base.StartCoroutine(this.faderFadeOut_cr());
		cam.StartBlur(0.5f, 5f);
		yield return new WaitForSeconds(0.5f);
		cam.EndBlur(0.5f, 5f);
		yield return new WaitForSeconds(0.5f);
		SceneLoader.IsInBlurTransition = false;
		yield break;
	}

	// Token: 0x0600444E RID: 17486 RVA: 0x00241794 File Offset: 0x0023FB94
	private IEnumerator iconFadeIn_cr()
	{
		if (SceneLoader.properties.icon == SceneLoader.Icon.None)
		{
			this.SetIconAlpha(0f);
		}
		else
		{
			Animator animator = this.icon.GetComponent<Animator>();
			animator.SetTrigger(SceneLoader.properties.icon.ToString());
			yield return base.StartCoroutine(this.imageFade_cr(this.icon, 0.4f, 0f, 1f, true));
		}
		yield break;
	}

	// Token: 0x0600444F RID: 17487 RVA: 0x002417B0 File Offset: 0x0023FBB0
	private IEnumerator iconFadeOut_cr()
	{
		if (SceneLoader.properties.icon == SceneLoader.Icon.None)
		{
			this.SetIconAlpha(0f);
			yield return new WaitForSeconds(0.6f);
		}
		else
		{
			float startAlpha = this.icon.color.a;
			yield return base.StartCoroutine(this.imageFade_cr(this.icon, 0.6f * startAlpha, startAlpha, 0f, false));
			if (startAlpha < 1f)
			{
				yield return new WaitForSeconds(0.6f * (1f - startAlpha));
			}
		}
		yield break;
	}

	// Token: 0x06004450 RID: 17488 RVA: 0x002417CC File Offset: 0x0023FBCC
	private IEnumerator imageFade_cr(Image image, float time, float start, float end, bool interruptOnLoad = false)
	{
		float t = 0f;
		this.SetImageAlpha(image, start);
		while (t < time && (!interruptOnLoad || !this.doneLoadingSceneAsync))
		{
			float val = Mathf.Lerp(start, end, t / time);
			this.SetImageAlpha(image, val);
			t += Time.deltaTime;
			if (SceneLoader.OnFaderValue != null)
			{
				SceneLoader.OnFaderValue(t / time);
			}
			if (interruptOnLoad)
			{
				SceneLoader.EndTransitionDelay = val * 0.6f;
			}
			yield return null;
		}
		this.SetImageAlpha(image, end);
		if (interruptOnLoad && !this.doneLoadingSceneAsync)
		{
			SceneLoader.EndTransitionDelay = 0.6f;
		}
		yield break;
	}

	// Token: 0x06004451 RID: 17489 RVA: 0x0024180C File Offset: 0x0023FC0C
	private IEnumerator fadeBGM_cr(float time)
	{
		if (AudioNoiseHandler.Instance != null)
		{
			AudioNoiseHandler.Instance.OpticalSound();
		}
		this.bgmVolumeStart = AudioManager.bgmOptionsVolume;
		this.bgmVolume = AudioManager.bgmOptionsVolume;
		this.sfxVolumeStart = AudioManager.sfxOptionsVolume;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			AudioManager.bgmOptionsVolume = Mathf.Lerp(this.bgmVolume, -80f, val);
			t += Time.deltaTime;
			yield return null;
		}
		AudioManager.bgmOptionsVolume = -80f;
		AudioManager.StopBGM();
		yield break;
	}

	// Token: 0x06004452 RID: 17490 RVA: 0x0024182E File Offset: 0x0023FC2E
	private void FadeOutBGM(float time)
	{
		if (this.bgmCoroutine != null)
		{
			base.StopCoroutine(this.bgmCoroutine);
		}
		this.bgmCoroutine = base.StartCoroutine(this.fadeBGM_cr(time));
	}

	// Token: 0x06004453 RID: 17491 RVA: 0x0024185A File Offset: 0x0023FC5A
	public void ResetBgmVolume()
	{
		if (this.bgmCoroutine != null)
		{
			base.StopCoroutine(this.bgmCoroutine);
		}
		AudioManager.bgmOptionsVolume = this.bgmVolumeStart;
		AudioManager.sfxOptionsVolume = this.sfxVolumeStart;
	}

	// Token: 0x040049C6 RID: 18886
	private const string SCENE_LOADER_PATH = "UI/Scene_Loader";

	// Token: 0x040049C7 RID: 18887
	private const float ICON_IN_TIME = 0.4f;

	// Token: 0x040049C8 RID: 18888
	private const float ICON_OUT_TIME = 0.6f;

	// Token: 0x040049C9 RID: 18889
	private const float ICON_WAIT_TIME = 1f;

	// Token: 0x040049CA RID: 18890
	private const float ICON_NONE_TIME = 1f;

	// Token: 0x040049CB RID: 18891
	private const float FADER_DELAY = 0.5f;

	// Token: 0x040049CC RID: 18892
	private const float IRIS_TIME = 0.6f;

	// Token: 0x040049CD RID: 18893
	private readonly string LOAD_SCENE_NAME = Scenes.scene_load_helper.ToString();

	// Token: 0x040049CE RID: 18894
	public static float EndTransitionDelay;

	// Token: 0x040049CF RID: 18895
	public static bool IsInIrisTransition;

	// Token: 0x040049D0 RID: 18896
	public static bool IsInBlurTransition;

	// Token: 0x040049D1 RID: 18897
	private static SceneLoader _instance;

	// Token: 0x040049D3 RID: 18899
	private static string previousSceneName;

	// Token: 0x040049D7 RID: 18903
	private static bool currentlyLoading;

	// Token: 0x040049DE RID: 18910
	[SerializeField]
	private Canvas canvas;

	// Token: 0x040049DF RID: 18911
	[SerializeField]
	private Image fader;

	// Token: 0x040049E0 RID: 18912
	[SerializeField]
	private Image icon;

	// Token: 0x040049E1 RID: 18913
	[SerializeField]
	private SceneLoaderCamera camera;

	// Token: 0x040049E2 RID: 18914
	private bool doneLoadingSceneAsync;

	// Token: 0x040049E3 RID: 18915
	private float bgmVolume;

	// Token: 0x040049E4 RID: 18916
	private float bgmLevelVolume;

	// Token: 0x040049E5 RID: 18917
	private float bgmVolumeStart;

	// Token: 0x040049E6 RID: 18918
	private float bgmLevelVolumeStart;

	// Token: 0x040049E7 RID: 18919
	private float sfxVolumeStart;

	// Token: 0x040049E8 RID: 18920
	private Coroutine bgmCoroutine;

	// Token: 0x02000AFD RID: 2813
	public abstract class Context
	{
	}

	// Token: 0x02000AFE RID: 2814
	// (Invoke) Token: 0x06004456 RID: 17494
	public delegate void FadeHandler(float time);

	// Token: 0x02000AFF RID: 2815
	public enum Transition
	{
		// Token: 0x040049EA RID: 18922
		Iris,
		// Token: 0x040049EB RID: 18923
		Fade,
		// Token: 0x040049EC RID: 18924
		Blur,
		// Token: 0x040049ED RID: 18925
		None
	}

	// Token: 0x02000B00 RID: 2816
	public enum Icon
	{
		// Token: 0x040049EF RID: 18927
		None,
		// Token: 0x040049F0 RID: 18928
		Random,
		// Token: 0x040049F1 RID: 18929
		Cuphead_Head,
		// Token: 0x040049F2 RID: 18930
		Cuphead_Running,
		// Token: 0x040049F3 RID: 18931
		Cuphead_Jumping,
		// Token: 0x040049F4 RID: 18932
		Screen_OneMoment,
		// Token: 0x040049F5 RID: 18933
		Hourglass,
		// Token: 0x040049F6 RID: 18934
		HourglassBroken
	}

	// Token: 0x02000B01 RID: 2817
	public class Properties
	{
		// Token: 0x06004459 RID: 17497 RVA: 0x00241889 File Offset: 0x0023FC89
		public Properties()
		{
			this.Reset();
		}

		// Token: 0x0600445A RID: 17498 RVA: 0x00241897 File Offset: 0x0023FC97
		public void Reset()
		{
			this.icon = SceneLoader.Icon.Hourglass;
			this.transitionStart = SceneLoader.Transition.Fade;
			this.transitionEnd = SceneLoader.Transition.Fade;
			this.transitionStartTime = 0.4f;
			this.transitionEndTime = 0.4f;
		}

		// Token: 0x040049F7 RID: 18935
		public const float FADE_START_DEFAULT = 0.4f;

		// Token: 0x040049F8 RID: 18936
		public const float FADE_END_DEFAULT = 0.4f;

		// Token: 0x040049F9 RID: 18937
		public SceneLoader.Icon icon;

		// Token: 0x040049FA RID: 18938
		public SceneLoader.Transition transitionStart;

		// Token: 0x040049FB RID: 18939
		public SceneLoader.Transition transitionEnd;

		// Token: 0x040049FC RID: 18940
		public float transitionStartTime;

		// Token: 0x040049FD RID: 18941
		public float transitionEndTime;
	}
}
