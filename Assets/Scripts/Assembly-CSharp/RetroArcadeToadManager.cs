using System;
using UnityEngine;

// Token: 0x0200075F RID: 1887
public class RetroArcadeToadManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x0600291E RID: 10526 RVA: 0x0017FA34 File Offset: 0x0017DE34
	public void StartToad()
	{
		this.p = base.properties.CurrentState.toad;
		this.numDied = 0;
		this.toad1 = this.toadPrefab.Create(this, this.p, true);
		this.toad2 = this.toadPrefab.Create(this, this.p, false);
	}

	// Token: 0x0600291F RID: 10527 RVA: 0x0017FA90 File Offset: 0x0017DE90
	public void OnToadDie()
	{
		this.numDied++;
		if (this.numDied >= 2)
		{
			this.StopAllCoroutines();
			UnityEngine.Object.Destroy(this.toad1.gameObject);
			UnityEngine.Object.Destroy(this.toad2.gameObject);
			base.properties.DealDamageToNextNamedState();
		}
	}

	// Token: 0x04003212 RID: 12818
	[SerializeField]
	private RetroArcadeToad toadPrefab;

	// Token: 0x04003213 RID: 12819
	private LevelProperties.RetroArcade.Toad p;

	// Token: 0x04003214 RID: 12820
	private RetroArcadeToad toad1;

	// Token: 0x04003215 RID: 12821
	private RetroArcadeToad toad2;

	// Token: 0x04003216 RID: 12822
	private int numDied;
}
