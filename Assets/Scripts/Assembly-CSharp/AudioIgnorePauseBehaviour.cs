using System;
using UnityEngine;

// Token: 0x020003B6 RID: 950
public class AudioIgnorePauseBehaviour : AbstractMonoBehaviour
{
	// Token: 0x06000BC0 RID: 3008 RVA: 0x00084BD4 File Offset: 0x00082FD4
	protected override void Awake()
	{
		base.Awake();
		this.audioSource = base.GetComponent<AudioSource>();
		if (this.audioSource != null)
		{
			this.audioSource.ignoreListenerPause = true;
		}
	}

	// Token: 0x04001571 RID: 5489
	private AudioSource audioSource;
}
