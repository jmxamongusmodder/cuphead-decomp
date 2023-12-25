using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006D0 RID: 1744
public class KitchenAudioManagerHelper : MonoBehaviour
{
	// Token: 0x170003BA RID: 954
	// (get) Token: 0x06002523 RID: 9507 RVA: 0x0015C612 File Offset: 0x0015AA12
	public static KitchenAudioManagerHelper Instance
	{
		get
		{
			return KitchenAudioManagerHelper._instance;
		}
	}

	// Token: 0x06002524 RID: 9508 RVA: 0x0015C61C File Offset: 0x0015AA1C
	private void Awake()
	{
		if (KitchenAudioManagerHelper._instance != null && KitchenAudioManagerHelper._instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			KitchenAudioManagerHelper._instance = this;
			this.sceneName = Scenes.scene_level_kitchen.ToString();
			base.transform.parent = null;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SceneLoader.instance.ResetBgmVolume();
		}
	}

	// Token: 0x06002525 RID: 9509 RVA: 0x0015C698 File Offset: 0x0015AA98
	private IEnumerator exit_level_cr()
	{
		while (SceneLoader.CurrentlyLoading)
		{
			yield return null;
		}
		yield return new WaitForEndOfFrame();
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002526 RID: 9510 RVA: 0x0015C6B4 File Offset: 0x0015AAB4
	private void Update()
	{
		if (this.exitingLevel)
		{
			return;
		}
		if (SceneLoader.CurrentlyLoading && SceneLoader.SceneName != this.sceneName && SceneLoader.SceneName != Scenes.scene_cutscene_dlc_saltbaker_prebattle.ToString())
		{
			this.exitingLevel = true;
			base.StartCoroutine(this.exit_level_cr());
		}
	}

	// Token: 0x04002DCC RID: 11724
	private bool exitingLevel;

	// Token: 0x04002DCD RID: 11725
	private string sceneName;

	// Token: 0x04002DCE RID: 11726
	private bool started;

	// Token: 0x04002DCF RID: 11727
	private static KitchenAudioManagerHelper _instance;
}
