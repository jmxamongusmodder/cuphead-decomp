using System;
using UnityEngine;

// Token: 0x020003CE RID: 974
public class AudioNoiseHandler : AbstractMonoBehaviour
{
	// Token: 0x17000224 RID: 548
	// (get) Token: 0x06000CAC RID: 3244 RVA: 0x00089094 File Offset: 0x00087494
	public static AudioNoiseHandler Instance
	{
		get
		{
			if (AudioNoiseHandler.noiseHandler == null)
			{
				AudioNoiseHandler audioNoiseHandler = UnityEngine.Object.Instantiate(Resources.Load("Audio/AudioNoiseHandler")) as AudioNoiseHandler;
				audioNoiseHandler.name = "NoiseHandler";
			}
			return AudioNoiseHandler.noiseHandler;
		}
	}

	// Token: 0x06000CAD RID: 3245 RVA: 0x000890D6 File Offset: 0x000874D6
	protected override void Awake()
	{
		base.Awake();
		AudioNoiseHandler.noiseHandler = this;
		base.GetComponent<AudioSource>().ignoreListenerPause = true;
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
	}

	// Token: 0x06000CAE RID: 3246 RVA: 0x000890FB File Offset: 0x000874FB
	public void OpticalSound()
	{
		AudioManager.Play("optical_start");
	}

	// Token: 0x06000CAF RID: 3247 RVA: 0x00089107 File Offset: 0x00087507
	public void BoingSound()
	{
		AudioManager.Play("worldmap_level_select");
	}

	// Token: 0x0400163B RID: 5691
	private static AudioNoiseHandler noiseHandler;

	// Token: 0x0400163C RID: 5692
	private const string PATH = "Audio/AudioNoiseHandler";
}
