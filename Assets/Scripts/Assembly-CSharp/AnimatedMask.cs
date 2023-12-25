using System;
using UnityEngine;

// Token: 0x02000456 RID: 1110
[RequireComponent(typeof(SpriteMask))]
public class AnimatedMask : MonoBehaviour
{
	// Token: 0x060010C7 RID: 4295 RVA: 0x000A0BC4 File Offset: 0x0009EFC4
	private void Start()
	{
		this.currentMask = this.maskRequest;
		this.mask = base.GetComponent<SpriteMask>();
		this.mask.sprite = this.currentMask;
	}

	// Token: 0x060010C8 RID: 4296 RVA: 0x000A0BEF File Offset: 0x0009EFEF
	private void LateUpdate()
	{
		if (this.currentMask != this.maskRequest)
		{
			this.currentMask = this.maskRequest;
			this.mask.sprite = this.currentMask;
		}
	}

	// Token: 0x04001A0F RID: 6671
	public Sprite maskRequest;

	// Token: 0x04001A10 RID: 6672
	private Sprite currentMask;

	// Token: 0x04001A11 RID: 6673
	private SpriteMask mask;
}
