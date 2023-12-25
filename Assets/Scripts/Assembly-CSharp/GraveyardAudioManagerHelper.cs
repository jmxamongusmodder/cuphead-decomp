using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006C4 RID: 1732
public class GraveyardAudioManagerHelper : MonoBehaviour
{
	// Token: 0x170003B7 RID: 951
	// (get) Token: 0x060024D0 RID: 9424 RVA: 0x0015916C File Offset: 0x0015756C
	public static GraveyardAudioManagerHelper Instance
	{
		get
		{
			return GraveyardAudioManagerHelper._instance;
		}
	}

	// Token: 0x060024D1 RID: 9425 RVA: 0x00159174 File Offset: 0x00157574
	private void Awake()
	{
		if (GraveyardAudioManagerHelper._instance != null && GraveyardAudioManagerHelper._instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			GraveyardAudioManagerHelper._instance = this;
			this.sceneName = Scenes.scene_level_graveyard.ToString();
			base.transform.parent = null;
			UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
			SceneLoader.instance.ResetBgmVolume();
			AudioManager.PlayBGM();
		}
	}

	// Token: 0x060024D2 RID: 9426 RVA: 0x001591F4 File Offset: 0x001575F4
	private IEnumerator exit_level_cr()
	{
		AudioManager.ChangeBGMPitch(0.7f, 5f);
		while (SceneLoader.CurrentlyLoading)
		{
			yield return null;
		}
		AudioManager.ChangeBGMPitch(1f, 0f);
		yield return new WaitForEndOfFrame();
		SceneLoader.instance.ResetBgmVolume();
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060024D3 RID: 9427 RVA: 0x00159210 File Offset: 0x00157610
	private void Update()
	{
		if (this.exitingLevel)
		{
			return;
		}
		if (SceneLoader.CurrentlyLoading && SceneLoader.SceneName != this.sceneName)
		{
			this.exitingLevel = true;
			base.StartCoroutine(this.exit_level_cr());
		}
	}

	// Token: 0x04002D6F RID: 11631
	private bool exitingLevel;

	// Token: 0x04002D70 RID: 11632
	private string sceneName;

	// Token: 0x04002D71 RID: 11633
	private bool started;

	// Token: 0x04002D72 RID: 11634
	private static GraveyardAudioManagerHelper _instance;
}
