using System;
using UnityEngine;

// Token: 0x0200073F RID: 1855
public class RetroArcadeCaterpillarManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x170003D8 RID: 984
	// (get) Token: 0x06002871 RID: 10353 RVA: 0x0017907C File Offset: 0x0017747C
	// (set) Token: 0x06002872 RID: 10354 RVA: 0x00179084 File Offset: 0x00177484
	public float moveSpeed { get; private set; }

	// Token: 0x06002873 RID: 10355 RVA: 0x00179090 File Offset: 0x00177490
	public void StartCaterpillar()
	{
		this.p = base.properties.CurrentState.caterpillar;
		this.bodyParts = new RetroArcadeCaterpillarBodyPart[this.p.bodyParts.Length + 1];
		RetroArcadeCaterpillarBodyPart.Direction direction = (!Rand.Bool()) ? RetroArcadeCaterpillarBodyPart.Direction.Right : RetroArcadeCaterpillarBodyPart.Direction.Left;
		this.bodyParts[0] = this.bodyPartPrefabs[0].Create(0, direction, this, this.p);
		for (int i = 0; i < this.p.bodyParts.Length; i++)
		{
			this.bodyParts[i + 1] = this.bodyPartPrefabs[this.p.bodyParts[i]].Create(i + 1, direction, this, this.p);
		}
		this.numDied = 0;
		this.numSpidersSpawned = 0;
		this.moveSpeed = 640f / this.p.moveTime;
	}

	// Token: 0x06002874 RID: 10356 RVA: 0x00179170 File Offset: 0x00177570
	public void OnBodyPartDie(RetroArcadeCaterpillarBodyPart alien)
	{
		this.numDied++;
		this.moveSpeed = 640f / (this.p.moveTime - (float)this.numDied * this.p.moveTimeDecrease);
		if (this.numDied >= this.bodyParts.Length - 1)
		{
			this.StopAllCoroutines();
			this.bodyParts[0].Dead();
			foreach (RetroArcadeCaterpillarBodyPart retroArcadeCaterpillarBodyPart in this.bodyParts)
			{
				retroArcadeCaterpillarBodyPart.OnWaveEnd();
			}
			base.properties.DealDamageToNextNamedState();
		}
	}

	// Token: 0x06002875 RID: 10357 RVA: 0x00179210 File Offset: 0x00177610
	public void OnReachBottom()
	{
		if (this.numSpidersSpawned < this.p.spiderCount)
		{
			this.numSpidersSpawned++;
			this.spiderPrefab.Create((!Rand.Bool()) ? RetroArcadeCaterpillarSpider.Direction.Right : RetroArcadeCaterpillarSpider.Direction.Left, this.p);
		}
	}

	// Token: 0x04003139 RID: 12601
	[SerializeField]
	private RetroArcadeCaterpillarBodyPart[] bodyPartPrefabs;

	// Token: 0x0400313A RID: 12602
	[SerializeField]
	private RetroArcadeCaterpillarSpider spiderPrefab;

	// Token: 0x0400313B RID: 12603
	private RetroArcadeCaterpillarBodyPart[] bodyParts;

	// Token: 0x0400313D RID: 12605
	private LevelProperties.RetroArcade.Caterpillar p;

	// Token: 0x0400313E RID: 12606
	private int numDied;

	// Token: 0x0400313F RID: 12607
	private int numSpidersSpawned;
}
