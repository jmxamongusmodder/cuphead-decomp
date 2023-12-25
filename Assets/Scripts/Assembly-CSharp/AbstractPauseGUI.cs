using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200044A RID: 1098
[RequireComponent(typeof(CanvasGroup))]
public abstract class AbstractPauseGUI : AbstractMonoBehaviour
{
	// Token: 0x17000290 RID: 656
	// (get) Token: 0x06001062 RID: 4194 RVA: 0x000937BE File Offset: 0x00091BBE
	// (set) Token: 0x06001063 RID: 4195 RVA: 0x000937C6 File Offset: 0x00091BC6
	public AbstractPauseGUI.State state { get; protected set; }

	// Token: 0x17000291 RID: 657
	// (get) Token: 0x06001064 RID: 4196 RVA: 0x000937CF File Offset: 0x00091BCF
	protected virtual CupheadButton LevelInputButton
	{
		get
		{
			return CupheadButton.Pause;
		}
	}

	// Token: 0x17000292 RID: 658
	// (get) Token: 0x06001065 RID: 4197 RVA: 0x000937D2 File Offset: 0x00091BD2
	protected virtual CupheadButton UIInputButton
	{
		get
		{
			return CupheadButton.EquipMenu;
		}
	}

	// Token: 0x17000293 RID: 659
	// (get) Token: 0x06001066 RID: 4198 RVA: 0x000937D6 File Offset: 0x00091BD6
	protected virtual AbstractPauseGUI.InputActionSet CheckedActionSet
	{
		get
		{
			return AbstractPauseGUI.InputActionSet.LevelInput;
		}
	}

	// Token: 0x17000294 RID: 660
	// (get) Token: 0x06001067 RID: 4199
	protected abstract bool CanPause { get; }

	// Token: 0x17000295 RID: 661
	// (get) Token: 0x06001068 RID: 4200 RVA: 0x000937D9 File Offset: 0x00091BD9
	protected virtual bool CanUnpause
	{
		get
		{
			return false;
		}
	}

	// Token: 0x17000296 RID: 662
	// (get) Token: 0x06001069 RID: 4201 RVA: 0x000937DC File Offset: 0x00091BDC
	protected virtual bool RespondToDeadPlayer
	{
		get
		{
			return false;
		}
	}

	// Token: 0x0600106A RID: 4202 RVA: 0x000937DF File Offset: 0x00091BDF
	protected override void Awake()
	{
		base.Awake();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
		this.HideImmediate();
	}

	// Token: 0x0600106B RID: 4203 RVA: 0x000937F9 File Offset: 0x00091BF9
	protected virtual void Update()
	{
		this.UpdateInput();
	}

	// Token: 0x0600106C RID: 4204 RVA: 0x00093804 File Offset: 0x00091C04
	private void UpdateInput()
	{
		if (!this.CanPause)
		{
			return;
		}
		bool flag = (this.CheckedActionSet != AbstractPauseGUI.InputActionSet.LevelInput) ? this.GetButtonDown(this.UIInputButton) : this.GetButtonDown(this.LevelInputButton);
		if (flag)
		{
			base.StartCoroutine(this.ShowPauseMenu());
		}
	}

	// Token: 0x0600106D RID: 4205 RVA: 0x0009385C File Offset: 0x00091C5C
	public IEnumerator ShowPauseMenu()
	{
		if (MapEventNotification.Current != null)
		{
			while (MapEventNotification.Current.showing)
			{
				yield return null;
			}
		}
		if (this.state == AbstractPauseGUI.State.Unpaused && PauseManager.state == PauseManager.State.Unpaused)
		{
			this.Pause();
		}
		else if (this.state == AbstractPauseGUI.State.Paused && this.CanUnpause)
		{
			this.Unpause();
		}
		yield break;
	}

	// Token: 0x0600106E RID: 4206 RVA: 0x00093877 File Offset: 0x00091C77
	public virtual void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements, RestartTowerConfirmGUI restartTowerConfirmGUI)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x0600106F RID: 4207 RVA: 0x00093885 File Offset: 0x00091C85
	public virtual void Init(bool checkIfDead, OptionsGUI options, AchievementsGUI achievements)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06001070 RID: 4208 RVA: 0x00093893 File Offset: 0x00091C93
	public virtual void Init(bool checkIfDead)
	{
		this.input = new CupheadInput.AnyPlayerInput(checkIfDead);
	}

	// Token: 0x06001071 RID: 4209 RVA: 0x000938A1 File Offset: 0x00091CA1
	protected virtual void Pause()
	{
		if (this.state == AbstractPauseGUI.State.Unpaused && PauseManager.state == PauseManager.State.Unpaused)
		{
			base.StartCoroutine(this.pause_cr());
		}
	}

	// Token: 0x06001072 RID: 4210 RVA: 0x000938C5 File Offset: 0x00091CC5
	protected virtual void Unpause()
	{
		if (this.state == AbstractPauseGUI.State.Paused)
		{
			base.StartCoroutine(this.unpause_cr());
		}
	}

	// Token: 0x06001073 RID: 4211 RVA: 0x000938E0 File Offset: 0x00091CE0
	protected virtual void OnPause()
	{
		this.OnPauseSound();
		if (PlatformHelper.GarbageCollectOnPause)
		{
			GC.Collect();
		}
	}

	// Token: 0x06001074 RID: 4212 RVA: 0x000938F7 File Offset: 0x00091CF7
	protected virtual void OnPauseComplete()
	{
	}

	// Token: 0x06001075 RID: 4213 RVA: 0x000938F9 File Offset: 0x00091CF9
	protected virtual void OnUnpause()
	{
		if (PlatformHelper.GarbageCollectOnPause)
		{
			GC.Collect();
		}
		this.OnUnpauseSound();
	}

	// Token: 0x06001076 RID: 4214 RVA: 0x00093910 File Offset: 0x00091D10
	protected virtual void OnUnpauseComplete()
	{
	}

	// Token: 0x06001077 RID: 4215 RVA: 0x00093914 File Offset: 0x00091D14
	protected virtual void OnPauseSound()
	{
		AudioManager.HandleSnapshot(AudioManager.Snapshots.Paused.ToString(), 0.15f);
		AudioManager.PauseAllSFX();
	}

	// Token: 0x06001078 RID: 4216 RVA: 0x00093940 File Offset: 0x00091D40
	protected virtual void OnUnpauseSound()
	{
		AudioManager.SnapshotReset((!this.isWorldMap) ? Level.Current.CurrentScene.ToString() : PlayerData.Data.CurrentMap.ToString(), 0.1f);
		AudioManager.UnpauseAllSFX();
	}

	// Token: 0x06001079 RID: 4217 RVA: 0x0009399C File Offset: 0x00091D9C
	protected virtual void HideImmediate()
	{
		this.canvasGroup.alpha = 0f;
		this.SetInteractable(false);
	}

	// Token: 0x0600107A RID: 4218 RVA: 0x000939B5 File Offset: 0x00091DB5
	protected virtual void ShowImmediate()
	{
		this.canvasGroup.alpha = 1f;
		this.SetInteractable(true);
	}

	// Token: 0x0600107B RID: 4219 RVA: 0x000939CE File Offset: 0x00091DCE
	private void SetInteractable(bool interactable)
	{
		this.canvasGroup.interactable = interactable;
		this.canvasGroup.blocksRaycasts = interactable;
	}

	// Token: 0x0600107C RID: 4220 RVA: 0x000939E8 File Offset: 0x00091DE8
	protected IEnumerator pause_cr()
	{
		Vibrator.StopVibrating(PlayerId.PlayerOne);
		Vibrator.StopVibrating(PlayerId.PlayerTwo);
		this.OnPause();
		this.PauseGameplay();
		this.SetInteractable(true);
		yield return base.StartCoroutine(this.animate_cr(this.InTime, new AbstractPauseGUI.AnimationDelegate(this.InAnimation), 0f, 1f));
		this.state = AbstractPauseGUI.State.Paused;
		this.OnPauseComplete();
		yield break;
	}

	// Token: 0x0600107D RID: 4221 RVA: 0x00093A04 File Offset: 0x00091E04
	protected IEnumerator unpause_cr()
	{
		this.OnUnpause();
		this.SetInteractable(true);
		this.UnpauseGameplay();
		yield return base.StartCoroutine(this.animate_cr(this.OutTime, new AbstractPauseGUI.AnimationDelegate(this.OutAnimation), 1f, 0f));
		this.state = AbstractPauseGUI.State.Unpaused;
		this.SetInteractable(false);
		this.OnUnpauseComplete();
		yield break;
	}

	// Token: 0x0600107E RID: 4222 RVA: 0x00093A1F File Offset: 0x00091E1F
	protected virtual void PauseGameplay()
	{
		PauseManager.Pause();
	}

	// Token: 0x0600107F RID: 4223 RVA: 0x00093A26 File Offset: 0x00091E26
	protected virtual void UnpauseGameplay()
	{
		PauseManager.Unpause();
	}

	// Token: 0x06001080 RID: 4224 RVA: 0x00093A30 File Offset: 0x00091E30
	private IEnumerator animate_cr(float time, AbstractPauseGUI.AnimationDelegate anim, float start, float end)
	{
		anim(0f);
		this.state = AbstractPauseGUI.State.Animating;
		this.canvasGroup.alpha = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.canvasGroup.alpha = Mathf.Lerp(start, end, val);
			anim(val);
			t += Time.deltaTime;
			yield return null;
		}
		this.canvasGroup.alpha = end;
		anim(1f);
		yield break;
	}

	// Token: 0x06001081 RID: 4225
	protected abstract void InAnimation(float i);

	// Token: 0x06001082 RID: 4226
	protected abstract void OutAnimation(float i);

	// Token: 0x17000297 RID: 663
	// (get) Token: 0x06001083 RID: 4227 RVA: 0x00093A68 File Offset: 0x00091E68
	protected virtual float InTime
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x17000298 RID: 664
	// (get) Token: 0x06001084 RID: 4228 RVA: 0x00093A6F File Offset: 0x00091E6F
	protected virtual float OutTime
	{
		get
		{
			return 0.15f;
		}
	}

	// Token: 0x06001085 RID: 4229 RVA: 0x00093A76 File Offset: 0x00091E76
	protected bool GetButtonDown(CupheadButton button)
	{
		return (!(AbstractEquipUI.Current != null) || AbstractEquipUI.Current.CurrentState != AbstractEquipUI.ActiveState.Active || button != CupheadButton.EquipMenu) && this.input.GetButtonDown(button);
	}

	// Token: 0x06001086 RID: 4230 RVA: 0x00093AB6 File Offset: 0x00091EB6
	protected void MenuSelectSound()
	{
		AudioManager.Play("level_menu_select");
	}

	// Token: 0x06001087 RID: 4231 RVA: 0x00093AC2 File Offset: 0x00091EC2
	protected void MenuMoveSound()
	{
		AudioManager.Play("level_menu_move");
	}

	// Token: 0x06001088 RID: 4232 RVA: 0x00093ACE File Offset: 0x00091ECE
	protected bool GetButton(CupheadButton button)
	{
		return this.input.GetButton(button);
	}

	// Token: 0x040019B6 RID: 6582
	[SerializeField]
	private bool isWorldMap;

	// Token: 0x040019B8 RID: 6584
	protected CanvasGroup canvasGroup;

	// Token: 0x040019B9 RID: 6585
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x0200044B RID: 1099
	public enum State
	{
		// Token: 0x040019BB RID: 6587
		Unpaused,
		// Token: 0x040019BC RID: 6588
		Paused,
		// Token: 0x040019BD RID: 6589
		Animating
	}

	// Token: 0x0200044C RID: 1100
	public enum InputActionSet
	{
		// Token: 0x040019BF RID: 6591
		LevelInput,
		// Token: 0x040019C0 RID: 6592
		UIInput
	}

	// Token: 0x0200044D RID: 1101
	// (Invoke) Token: 0x0600108A RID: 4234
	private delegate void AnimationDelegate(float i);
}
