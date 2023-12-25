using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009B2 RID: 2482
public class StartScreen : AbstractMonoBehaviour
{
	// Token: 0x170004B9 RID: 1209
	// (get) Token: 0x06003A36 RID: 14902 RVA: 0x002116CE File Offset: 0x0020FACE
	// (set) Token: 0x06003A37 RID: 14903 RVA: 0x002116D6 File Offset: 0x0020FAD6
	public StartScreen.State state { get; private set; }

	// Token: 0x06003A38 RID: 14904 RVA: 0x002116DF File Offset: 0x0020FADF
	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Debug.Log("Build version " + Application.version);
		Cuphead.Init(false);
		CupheadTime.Reset();
		PauseManager.Reset();
		this.shouldLoadSlotSelect = false;
		PlayerData.inGame = false;
		PlayerManager.ResetPlayers();
	}

	// Token: 0x06003A39 RID: 14905 RVA: 0x00211720 File Offset: 0x0020FB20
	private void Start()
	{
		if (PlatformHelper.PreloadSettingsData)
		{
			SettingsData.ApplySettingsOnStartup();
		}
		if (AudioNoiseHandler.Instance != null)
		{
			AudioNoiseHandler.Instance.OpticalSound();
		}
		if (StartScreenAudio.Instance == null)
		{
			StartScreenAudio startScreenAudio = UnityEngine.Object.Instantiate(Resources.Load("Audio/TitleScreenAudio")) as StartScreenAudio;
			startScreenAudio.name = "StartScreenAudio";
		}
		SettingsData.ApplySettingsOnStartup();
		base.FrameDelayedCallback(new Action(this.StartFrontendSnapshot), 1);
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x06003A3A RID: 14906 RVA: 0x002117B0 File Offset: 0x0020FBB0
	private void Update()
	{
		StartScreen.State state = this.state;
		if (state != StartScreen.State.MDHR_Splash)
		{
			if (state == StartScreen.State.Title)
			{
				this.UpdateTitleScreen();
			}
		}
		else
		{
			this.UpdateSplashMDHR();
		}
	}

	// Token: 0x06003A3B RID: 14907 RVA: 0x002117ED File Offset: 0x0020FBED
	private void UpdateSplashMDHR()
	{
	}

	// Token: 0x06003A3C RID: 14908 RVA: 0x002117EF File Offset: 0x0020FBEF
	private void UpdateTitleScreen()
	{
		if (this.shouldLoadSlotSelect)
		{
			AudioManager.Play("ui_playerconfirm");
			AudioManager.Play("level_select");
			SceneLoader.LoadScene(Scenes.scene_slot_select, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None, null);
			base.enabled = false;
			return;
		}
	}

	// Token: 0x06003A3D RID: 14909 RVA: 0x00211822 File Offset: 0x0020FC22
	private void onPlayerJoined(PlayerId playerId)
	{
		this.shouldLoadSlotSelect = true;
	}

	// Token: 0x06003A3E RID: 14910 RVA: 0x0021182C File Offset: 0x0020FC2C
	private IEnumerator loop_cr()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.Play("mdhr_logo_sting");
		yield return base.StartCoroutine(this.tweenRenderer_cr(this.fader, 1f));
		this.mdhrSplash.Play("Logo");
		yield return this.mdhrSplash.WaitForAnimationToEnd(this, "Logo", false, true);
		AudioManager.SnapshotReset(Scenes.scene_title.ToString(), 0.3f);
		if (!CreditsScreen.goodEnding)
		{
			AudioManager.PlayBGM();
		}
		else if (DLCManager.DLCEnabled() && !this.forceOriginalTitleScreen())
		{
			AudioManager.StartBGMAlternate(0);
			this.titleAnimation.SetActive(false);
			this.titleAnimationDLC.SetActive(true);
		}
		else
		{
			AudioManager.PlayBGMPlaylistManually(true);
		}
		StartScreen.initialLoadData = null;
		CreditsScreen.goodEnding = true;
		SettingsData.Data.hasBootedUpGame = true;
		yield return base.StartCoroutine(this.tweenRenderer_cr(this.mdhrSplash.GetComponent<SpriteRenderer>(), 0.4f));
		this.state = StartScreen.State.Title;
		PlayerManager.OnPlayerJoinedEvent += this.onPlayerJoined;
		PlayerManager.SetPlayerCanJoin(PlayerId.PlayerOne, true, false);
		yield break;
	}

	// Token: 0x06003A3F RID: 14911 RVA: 0x00211847 File Offset: 0x0020FC47
	private void OnDestroy()
	{
		PlayerManager.OnPlayerJoinedEvent -= this.onPlayerJoined;
	}

	// Token: 0x06003A40 RID: 14912 RVA: 0x0021185C File Offset: 0x0020FC5C
	private IEnumerator tweenRenderer_cr(SpriteRenderer renderer, float time)
	{
		float t = 0f;
		Color c = renderer.color;
		c.a = 1f;
		yield return null;
		while (t < time)
		{
			c.a = 1f - t / time;
			renderer.color = c;
			t += Time.deltaTime;
			yield return null;
		}
		c.a = 0f;
		renderer.color = c;
		yield return null;
		yield break;
	}

	// Token: 0x06003A41 RID: 14913 RVA: 0x0021187E File Offset: 0x0020FC7E
	private bool forceOriginalTitleScreen()
	{
		if (StartScreen.initialLoadData != null)
		{
			return StartScreen.initialLoadData.forceOriginalTitleScreen;
		}
		return SettingsData.Data.forceOriginalTitleScreen;
	}

	// Token: 0x06003A42 RID: 14914 RVA: 0x002118A0 File Offset: 0x0020FCA0
	protected virtual void StartFrontendSnapshot()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.FrontEnd.ToString(), 0.15f);
	}

	// Token: 0x0400426C RID: 17004
	public static StartScreen.InitialLoadData initialLoadData;

	// Token: 0x0400426E RID: 17006
	public AudioClip[] SelectSound;

	// Token: 0x0400426F RID: 17007
	[SerializeField]
	private Animator mdhrSplash;

	// Token: 0x04004270 RID: 17008
	[SerializeField]
	private SpriteRenderer fader;

	// Token: 0x04004271 RID: 17009
	[SerializeField]
	private GameObject titleAnimation;

	// Token: 0x04004272 RID: 17010
	[SerializeField]
	private GameObject titleAnimationDLC;

	// Token: 0x04004273 RID: 17011
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04004274 RID: 17012
	private bool shouldLoadSlotSelect;

	// Token: 0x04004275 RID: 17013
	private const string PATH = "Audio/TitleScreenAudio";

	// Token: 0x020009B3 RID: 2483
	public class InitialLoadData
	{
		// Token: 0x04004276 RID: 17014
		public bool forceOriginalTitleScreen;
	}

	// Token: 0x020009B4 RID: 2484
	public enum State
	{
		// Token: 0x04004278 RID: 17016
		Animating,
		// Token: 0x04004279 RID: 17017
		MDHR_Splash,
		// Token: 0x0400427A RID: 17018
		Title
	}
}
