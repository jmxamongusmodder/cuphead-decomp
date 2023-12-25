using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000407 RID: 1031
public class KingDiceCutscene : Cutscene
{
	// Token: 0x06000E5C RID: 3676 RVA: 0x00092E24 File Offset: 0x00091224
	protected override void Awake()
	{
		base.Awake();
		this.input = new CupheadInput.AnyPlayerInput(false);
		CutsceneGUI.Current.pause.pauseAllowed = false;
		if (PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world1BossLevels, Level.Mode.Normal) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world2BossLevels, Level.Mode.Normal) && PlayerData.Data.CheckLevelsHaveMinDifficulty(Level.world3BossLevels, Level.Mode.Normal))
		{
			base.StartCoroutine(this.have_all_contracts_cr());
		}
		else
		{
			base.StartCoroutine(this.missing_contracts_cr());
		}
	}

	// Token: 0x06000E5D RID: 3677 RVA: 0x00092EB4 File Offset: 0x000912B4
	private IEnumerator have_all_contracts_cr()
	{
		base.animator.Play("All_Contracts");
		int numScreens = 2;
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.25f);
			this.arrowVisible = true;
			while (!this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
		}
		SceneLoader.LoadScene(Scenes.scene_level_dice_palace_main, SceneLoader.Transition.Fade, SceneLoader.Transition.Iris, SceneLoader.Icon.Hourglass, null);
		yield break;
	}

	// Token: 0x06000E5E RID: 3678 RVA: 0x00092ED0 File Offset: 0x000912D0
	private IEnumerator missing_contracts_cr()
	{
		base.animator.Play("Missing_Contracts");
		int numScreens = 3;
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		for (int i = 0; i < numScreens; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, 1.25f);
			this.arrowVisible = true;
			while (!this.input.GetAnyButtonDown())
			{
				yield return null;
			}
			this.arrowVisible = false;
			base.animator.SetTrigger("Continue");
		}
		SceneLoader.LoadLastMap();
		yield return null;
		yield break;
	}

	// Token: 0x06000E5F RID: 3679 RVA: 0x00092EEC File Offset: 0x000912EC
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

	// Token: 0x06000E60 RID: 3680 RVA: 0x00092F56 File Offset: 0x00091356
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.arrow = null;
	}

	// Token: 0x040017A2 RID: 6050
	private CupheadInput.AnyPlayerInput input;

	// Token: 0x040017A3 RID: 6051
	[SerializeField]
	private Image arrow;

	// Token: 0x040017A4 RID: 6052
	private float arrowTransparency;

	// Token: 0x040017A5 RID: 6053
	private bool arrowVisible;
}
