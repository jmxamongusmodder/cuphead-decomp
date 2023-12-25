using System;
using UnityEngine;

// Token: 0x020006DA RID: 1754
public class MausoleumLevelUrn : AbstractCollidableObject
{
	// Token: 0x170003BF RID: 959
	// (get) Token: 0x0600255F RID: 9567 RVA: 0x0015D97D File Offset: 0x0015BD7D
	// (set) Token: 0x06002560 RID: 9568 RVA: 0x0015D984 File Offset: 0x0015BD84
	public static Vector3 URN_POS { get; private set; }

	// Token: 0x06002561 RID: 9569 RVA: 0x0015D98C File Offset: 0x0015BD8C
	protected override void Awake()
	{
		base.Awake();
		MausoleumLevelUrn.URN_POS = base.transform.position;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002562 RID: 9570 RVA: 0x0015D9AF File Offset: 0x0015BDAF
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x04002DF1 RID: 11761
	private DamageDealer damageDealer;
}
