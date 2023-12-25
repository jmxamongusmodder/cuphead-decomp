using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000889 RID: 2185
public class TreePlatformingLevelButterflySpawner : AbstractPausableComponent
{
	// Token: 0x060032CE RID: 13006 RVA: 0x001D81F9 File Offset: 0x001D65F9
	protected override void Awake()
	{
		base.Awake();
		base.StartCoroutine(this.instantiate_butterflies());
		base.StartCoroutine(this.spawner_cr());
	}

	// Token: 0x060032CF RID: 13007 RVA: 0x001D821C File Offset: 0x001D661C
	private IEnumerator instantiate_butterflies()
	{
		this.butterflies = new List<TreePlatformingLevelButterfly>();
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		for (int i = 0; i < 5; i++)
		{
			TreePlatformingLevelButterfly treePlatformingLevelButterfly = UnityEngine.Object.Instantiate<TreePlatformingLevelButterfly>(this.butterflySmall);
			treePlatformingLevelButterfly.transform.parent = base.transform;
			treePlatformingLevelButterfly.transform.position = new Vector3(base.transform.position.x, base.transform.position.y - 10000f);
			this.butterflies.Add(treePlatformingLevelButterfly);
		}
		yield return null;
		yield break;
	}

	// Token: 0x060032D0 RID: 13008 RVA: 0x001D8238 File Offset: 0x001D6638
	private IEnumerator spawner_cr()
	{
		bool keepChecking = true;
		TreePlatformingLevelButterfly spawn = null;
		yield return CupheadTime.WaitForSeconds(this, this.initalDelay);
		for (;;)
		{
			while (PlayerManager.GetNext().transform.position.x < this.startButterflies.position.x)
			{
				yield return null;
			}
			if (this.endButterflies != null)
			{
				while (PlayerManager.GetNext().transform.position.y > this.endButterflies.position.y)
				{
					yield return null;
				}
			}
			keepChecking = true;
			while (keepChecking)
			{
				foreach (TreePlatformingLevelButterfly treePlatformingLevelButterfly in this.butterflies)
				{
					if (!treePlatformingLevelButterfly.isActive)
					{
						spawn = treePlatformingLevelButterfly;
						keepChecking = false;
						break;
					}
				}
				yield return null;
			}
			bool onLeft = Rand.Bool();
			float y = UnityEngine.Random.Range(CupheadLevelCamera.Current.Bounds.yMin + 50f, CupheadLevelCamera.Current.Bounds.yMax - 50f);
			float x = (!onLeft) ? (CupheadLevelCamera.Current.Bounds.xMax + 50f) : (CupheadLevelCamera.Current.Bounds.xMin - 50f);
			float scale = (float)((!onLeft) ? -1 : 1);
			spawn.transform.position = new Vector3(x, y);
			spawn.Init(new Vector2((!onLeft) ? (-this.velocityX.RandomFloat()) : this.velocityX.RandomFloat(), (float)((!Rand.Bool()) ? (-(float)this.velocityY) : this.velocityY)), scale, UnityEngine.Random.Range(1, 5), this.velocityX);
			yield return CupheadTime.WaitForSeconds(this, this.delay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060032D1 RID: 13009 RVA: 0x001D8254 File Offset: 0x001D6654
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawLine(this.startButterflies.position + new Vector3(0f, 1000f), this.startButterflies.position + new Vector3(0f, -1000f));
		Gizmos.color = Color.yellow;
		if (this.endButterflies != null)
		{
			Gizmos.DrawLine(this.endButterflies.position + new Vector3(1000f, 0f), this.endButterflies.position + new Vector3(-1000f, 0f));
		}
	}

	// Token: 0x04003AF8 RID: 15096
	private const int BUTTERFLIES = 5;

	// Token: 0x04003AF9 RID: 15097
	private const float OFFSET = 50f;

	// Token: 0x04003AFA RID: 15098
	[SerializeField]
	private TreePlatformingLevelButterfly butterflySmall;

	// Token: 0x04003AFB RID: 15099
	[SerializeField]
	private Transform startButterflies;

	// Token: 0x04003AFC RID: 15100
	[SerializeField]
	private Transform endButterflies;

	// Token: 0x04003AFD RID: 15101
	private List<TreePlatformingLevelButterfly> butterflies;

	// Token: 0x04003AFE RID: 15102
	private MinMax delay = new MinMax(1.5f, 3f);

	// Token: 0x04003AFF RID: 15103
	private MinMax velocityX = new MinMax(300f, 600f);

	// Token: 0x04003B00 RID: 15104
	private MinMax velocityY = new MinMax(100f, 200f);

	// Token: 0x04003B01 RID: 15105
	private float initalDelay = 6f;
}
