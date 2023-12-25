using System;
using UnityEngine;

// Token: 0x020003AB RID: 939
public class DeferredAudioSource : MonoBehaviour
{
	// Token: 0x06000B95 RID: 2965 RVA: 0x00084004 File Offset: 0x00082404
	public void Initialize()
	{
		AudioSource component = base.GetComponent<AudioSource>();
		if (!DLCManager.DLCEnabled() && AssetLoader<AudioClip>.IsDLCAsset(this.audioClipName))
		{
			component.clip = null;
		}
		else
		{
			component.clip = AssetLoader<AudioClip>.GetCachedAsset(this.audioClipName);
		}
		if (this.playOnInitialize)
		{
			component.Play();
		}
	}

	// Token: 0x04001525 RID: 5413
	[SerializeField]
	private string audioClipName;

	// Token: 0x04001526 RID: 5414
	[SerializeField]
	private bool playOnInitialize;
}
