using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x020003F7 RID: 1015
public class DevilCutscene : Cutscene
{
	// Token: 0x06000DD1 RID: 3537 RVA: 0x0008F82D File Offset: 0x0008DC2D
	protected override void Start()
	{
		base.Start();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06000DD2 RID: 3538 RVA: 0x0008F860 File Offset: 0x0008DC60
	private IEnumerator main_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1.5f);
		this.arrowVisible = true;
		while (!this.input.GetAnyButtonDown())
		{
			yield return null;
		}
		this.arrowVisible = false;
		base.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, 1.25f);
		this.optionSelector.Show();
		yield break;
	}

	// Token: 0x06000DD3 RID: 3539 RVA: 0x0008F87B File Offset: 0x0008DC7B
	public void RefuseDevil()
	{
		this.ConfirmSFX();
		base.StartCoroutine(this.refuse_devil_cr());
	}

	// Token: 0x06000DD4 RID: 3540 RVA: 0x0008F890 File Offset: 0x0008DC90
	public void JoinDevil()
	{
		this.ConfirmSFX();
		base.StartCoroutine(this.join_devil_cr());
	}

	// Token: 0x06000DD5 RID: 3541 RVA: 0x0008F8A8 File Offset: 0x0008DCA8
	private IEnumerator join_devil_cr()
	{
		AudioManager.FadeBGMVolume(0f, 0.5f, true);
		AudioManager.PlayBGMPlaylistManually(false);
		this.evilVersionsBaseGame.SetActive(true);
		this.evilVersionsDLC.SetActive(false);
		if (DLCManager.DLCEnabled() && (PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerOne).charm == Charm.charm_chalice || (PlayerManager.Multiplayer && PlayerData.Data.Loadouts.GetPlayerLoadout(PlayerId.PlayerTwo).charm == Charm.charm_chalice)))
		{
			this.evilVersionsBaseGame.SetActive(false);
			this.evilVersionsDLC.SetActive(true);
		}
		base.animator.SetTrigger("joinedDevil");
		yield return CupheadTime.WaitForSeconds(this, 1.25f);
		this.arrowVisible = true;
		while (!this.input.GetAnyButtonDown())
		{
			yield return null;
		}
		this.arrowVisible = false;
		base.animator.SetTrigger("fadeOut");
		yield return base.animator.WaitForAnimationToEnd(this, "Fade_Out", 1, false, true);
		base.animator.SetTrigger("Continue");
		base.animator.SetTrigger("fadeIn");
		this.DevilEvilSFX();
		base.StartCoroutine(this.blink_cr());
		yield return CupheadTime.WaitForSeconds(this, 10f);
		this.KillSFX();
		CreditsScreen.goodEnding = false;
		Cutscene.Load(Scenes.scene_title, Scenes.scene_cutscene_credits, SceneLoader.Transition.Iris, SceneLoader.Transition.Fade, SceneLoader.Icon.None);
		yield break;
	}

	// Token: 0x06000DD6 RID: 3542 RVA: 0x0008F8C4 File Offset: 0x0008DCC4
	private IEnumerator blink_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(3f, 5f));
			base.animator.SetTrigger("Blink");
		}
		yield break;
	}

	// Token: 0x06000DD7 RID: 3543 RVA: 0x0008F8E0 File Offset: 0x0008DCE0
	private IEnumerator refuse_devil_cr()
	{
		base.animator.SetTrigger("refusedDevil");
		this.DevilAngrySFX();
		yield return CupheadTime.WaitForSeconds(this, 1.25f);
		this.arrowVisible = true;
		while (!this.input.GetAnyButtonDown())
		{
			yield return null;
		}
		this.arrowVisible = false;
		this.KillSFX();
		SceneLoader.LoadLevel(Levels.Devil, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x06000DD8 RID: 3544 RVA: 0x0008F8FC File Offset: 0x0008DCFC
	private void Update()
	{
		if (this.arrowVisible)
		{
			this.arrowTransparency = Mathf.Clamp01(this.arrowTransparency + Time.deltaTime / 0.25f);
		}
		else
		{
			this.arrowTransparency = 0f;
		}
		this.arrow.color = new Color(1f, 1f, 1f, this.arrowTransparency);
	}

	// Token: 0x06000DD9 RID: 3545 RVA: 0x0008F966 File Offset: 0x0008DD66
	private void ConfirmSFX()
	{
		AudioManager.Play("ui_confirm");
	}

	// Token: 0x06000DDA RID: 3546 RVA: 0x0008F972 File Offset: 0x0008DD72
	private void DevilEvilSFX()
	{
		AudioManager.PlayLoop("sfx_hell_fire");
		AudioManager.Play("devil_laugh");
	}

	// Token: 0x06000DDB RID: 3547 RVA: 0x0008F988 File Offset: 0x0008DD88
	private void DevilAngrySFX()
	{
		AudioManager.PlayLoop("sfx_hell_fire");
	}

	// Token: 0x06000DDC RID: 3548 RVA: 0x0008F994 File Offset: 0x0008DD94
	private void KillSFX()
	{
		AudioManager.FadeSFXVolume("sfx_hell_fire", 0f, 4f);
	}

	// Token: 0x06000DDD RID: 3549 RVA: 0x0008F9AA File Offset: 0x0008DDAA
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x04001737 RID: 5943
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x04001738 RID: 5944
	[SerializeField]
	private Image arrow;

	// Token: 0x04001739 RID: 5945
	[SerializeField]
	private DevilCutsceneOptionSelector optionSelector;

	// Token: 0x0400173A RID: 5946
	[SerializeField]
	private GameObject evilVersionsBaseGame;

	// Token: 0x0400173B RID: 5947
	[SerializeField]
	private GameObject evilVersionsDLC;

	// Token: 0x0400173C RID: 5948
	private float arrowTransparency;

	// Token: 0x0400173D RID: 5949
	private bool arrowVisible;
}
