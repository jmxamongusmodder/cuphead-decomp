using System;

// Token: 0x02000A0E RID: 2574
public interface IParryAttack
{
	// Token: 0x17000524 RID: 1316
	// (get) Token: 0x06003CD8 RID: 15576
	// (set) Token: 0x06003CD9 RID: 15577
	bool AttackParryUsed { get; set; }

	// Token: 0x17000525 RID: 1317
	// (get) Token: 0x06003CDA RID: 15578
	// (set) Token: 0x06003CDB RID: 15579
	bool HasHitEnemy { get; set; }
}
