using System;
using System.Collections;
using UnityEngine;

// Token: 0x020009C8 RID: 2504
public class AbstractPausableComponent : AbstractMonoBehaviour
{
	// Token: 0x170004D2 RID: 1234
	// (get) Token: 0x06003ADC RID: 15068 RVA: 0x0000836A File Offset: 0x0000676A
	protected virtual Transform emitTransform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x06003ADD RID: 15069 RVA: 0x00008372 File Offset: 0x00006772
	protected override void Awake()
	{
		base.Awake();
		PauseManager.AddChild(this);
		this.preEnabled = base.enabled;
		this.emitAudioFromObject = new SoundEmitter(this);
	}

	// Token: 0x06003ADE RID: 15070 RVA: 0x00008398 File Offset: 0x00006798
	protected virtual void OnDestroy()
	{
		PauseManager.RemoveChild(this);
	}

	// Token: 0x06003ADF RID: 15071 RVA: 0x000083A0 File Offset: 0x000067A0
	public virtual void OnPause()
	{
	}

	// Token: 0x06003AE0 RID: 15072 RVA: 0x000083A2 File Offset: 0x000067A2
	public virtual void OnUnpause()
	{
	}

	// Token: 0x06003AE1 RID: 15073 RVA: 0x000083A4 File Offset: 0x000067A4
	protected IEnumerator WaitForPause_CR()
	{
		while (PauseManager.state == PauseManager.State.Paused)
		{
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003AE2 RID: 15074 RVA: 0x000083B8 File Offset: 0x000067B8
	public virtual void OnLevelEnd()
	{
		if (this != null)
		{
			this.StopAllCoroutines();
			base.enabled = false;
		}
	}

	// Token: 0x06003AE3 RID: 15075 RVA: 0x000083D3 File Offset: 0x000067D3
	public void EmitSound(string key)
	{
		AudioManager.FollowObject(key, this.emitTransform);
	}

	// Token: 0x040042A1 RID: 17057
	[NonSerialized]
	public bool preEnabled;

	// Token: 0x040042A2 RID: 17058
	protected SoundEmitter emitAudioFromObject;
}
