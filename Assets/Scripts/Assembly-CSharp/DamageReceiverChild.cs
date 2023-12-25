using System;
using UnityEngine;

// Token: 0x02000AF5 RID: 2805
public class DamageReceiverChild : AbstractMonoBehaviour
{
	// Token: 0x17000614 RID: 1556
	// (get) Token: 0x06004401 RID: 17409 RVA: 0x002409FE File Offset: 0x0023EDFE
	public DamageReceiver Receiver
	{
		get
		{
			return this.receiver;
		}
	}

	// Token: 0x06004402 RID: 17410 RVA: 0x00240A06 File Offset: 0x0023EE06
	private void Start()
	{
		base.tag = this.receiver.tag;
	}

	// Token: 0x0400499D RID: 18845
	[SerializeField]
	private DamageReceiver receiver;
}
