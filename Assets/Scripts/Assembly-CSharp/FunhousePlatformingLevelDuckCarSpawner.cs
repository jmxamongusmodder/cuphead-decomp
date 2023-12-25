using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008B4 RID: 2228
public class FunhousePlatformingLevelDuckCarSpawner : PlatformingLevelEnemySpawner
{
	// Token: 0x060033EF RID: 13295 RVA: 0x001E1ED7 File Offset: 0x001E02D7
	protected override void Start()
	{
		base.Start();
		this.duckPinkIndex = UnityEngine.Random.Range(0, this.duckPinkString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x060033F0 RID: 13296 RVA: 0x001E1F03 File Offset: 0x001E0303
	protected override void StartSpawning()
	{
		base.StartSpawning();
		base.StartCoroutine(this.start_spawning_cr());
	}

	// Token: 0x060033F1 RID: 13297 RVA: 0x001E1F18 File Offset: 0x001E0318
	protected override void EndSpawning()
	{
		base.StopCoroutine(this.start_spawning_cr());
		base.EndSpawning();
	}

	// Token: 0x060033F2 RID: 13298 RVA: 0x001E1F2C File Offset: 0x001E032C
	private IEnumerator start_spawning_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.initalSpawnDelay.RandomFloat());
		for (;;)
		{
			if (this.firstTime)
			{
				this.ducksTop = false;
				this.firstTime = false;
			}
			else
			{
				this.ducksTop = !this.ducksTop;
			}
			base.StartCoroutine(this.spawn_ducks_cr());
			base.StartCoroutine(this.spawn_cars_cr());
			this.ducksSpawning = true;
			this.carsSpawning = true;
			while (this.ducksSpawning || this.carsSpawning)
			{
				yield return null;
			}
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelay.RandomFloat());
			yield return null;
		}
		yield break;
	}

	// Token: 0x060033F3 RID: 13299 RVA: 0x001E1F48 File Offset: 0x001E0348
	private IEnumerator spawn_ducks_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.duckDelay);
		float bigDuckSize = this.bigDuckPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
		float smallDuckSize = this.smallDuckPrefab.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
		FunhousePlatformingLevelDuck lastDuck = null;
		Vector2 startPos = Vector2.zero;
		startPos.x = CupheadLevelCamera.Current.Bounds.xMax + bigDuckSize + this.duckSpacing;
		startPos.y = ((!this.ducksTop) ? this.bottomSpawnRoot.position.y : this.topSpawnRoot.position.y);
		FunhousePlatformingLevelDuck bigDuck = this.bigDuckPrefab.Spawn(startPos);
		bigDuck.transform.SetScale(null, new float?((!this.ducksTop) ? bigDuck.transform.localScale.y : (-bigDuck.transform.localScale.y)), null);
		int num = 1;
		while ((float)num < this.duckCount)
		{
			startPos.x = CupheadLevelCamera.Current.Bounds.xMax + bigDuckSize + this.duckSpacing + (smallDuckSize + this.duckSpacing) * (float)num;
			startPos.y = ((!this.ducksTop) ? this.bottomSpawnRoot.position.y : this.topSpawnRoot.position.y);
			if (this.duckPinkString.Split(new char[]
			{
				','
			})[this.duckPinkIndex][0] == 'P')
			{
				FunhousePlatformingLevelDuck funhousePlatformingLevelDuck = this.smallDuckPrefab.Spawn(startPos);
				funhousePlatformingLevelDuck.transform.SetScale(null, new float?((!this.ducksTop) ? funhousePlatformingLevelDuck.transform.localScale.y : (-funhousePlatformingLevelDuck.transform.localScale.y)), null);
				funhousePlatformingLevelDuck.smallFirst = (num == 1);
				if ((float)num == this.duckCount - 1f)
				{
					funhousePlatformingLevelDuck.smallLast = true;
					lastDuck = funhousePlatformingLevelDuck;
				}
				else
				{
					funhousePlatformingLevelDuck.smallLast = false;
				}
			}
			else if (this.duckPinkString.Split(new char[]
			{
				','
			})[this.duckPinkIndex][0] == 'R')
			{
				FunhousePlatformingLevelDuck funhousePlatformingLevelDuck2 = this.smallDuckPinkPrefab.Spawn(startPos);
				funhousePlatformingLevelDuck2.transform.SetScale(null, new float?((!this.ducksTop) ? funhousePlatformingLevelDuck2.transform.localScale.y : (-funhousePlatformingLevelDuck2.transform.localScale.y)), null);
				funhousePlatformingLevelDuck2.smallFirst = (num == 1);
				if ((float)num == this.duckCount - 1f)
				{
					funhousePlatformingLevelDuck2.smallLast = true;
					lastDuck = funhousePlatformingLevelDuck2;
				}
				else
				{
					funhousePlatformingLevelDuck2.smallLast = false;
				}
			}
			this.duckPinkIndex = (this.duckPinkIndex + 1) % this.duckPinkString.Split(new char[]
			{
				','
			}).Length;
			num++;
		}
		while (lastDuck != null)
		{
			if (lastDuck.transform.position.x < CupheadLevelCamera.Current.transform.position.x)
			{
				break;
			}
			yield return null;
		}
		this.ducksSpawning = false;
		yield return null;
		yield break;
	}

	// Token: 0x060033F4 RID: 13300 RVA: 0x001E1F64 File Offset: 0x001E0364
	private IEnumerator spawn_cars_cr()
	{
		this.SpawnHonk((!this.ducksTop) ? this.topSpawnRoot.position.y : this.bottomSpawnRoot.position.y, (float)((!this.ducksTop) ? -1 : 1), (!this.ducksTop) ? -100f : 100f);
		yield return CupheadTime.WaitForSeconds(this, this.carDelay);
		float carSize = this.carPrefabNormal.GetComponentInChildren<SpriteRenderer>().bounds.size.x;
		int index = 0;
		FunhousePlatformingLevelCar lastCar = null;
		Vector2 startPos = Vector2.zero;
		for (int i = 0; i < this.carCount; i++)
		{
			startPos.x = CupheadLevelCamera.Current.Bounds.xMax + (carSize + this.carSpacing * (float)i);
			startPos.y = ((!this.ducksTop) ? this.topSpawnRoot.position.y : this.bottomSpawnRoot.position.y);
			FunhousePlatformingLevelCar funhousePlatformingLevelCar = UnityEngine.Object.Instantiate<FunhousePlatformingLevelCar>(this.carPrefabNormal);
			funhousePlatformingLevelCar.Init(startPos, 180f, this.carSpeed, index, i == 0, i == this.carCount - 1);
			funhousePlatformingLevelCar.transform.SetScale(null, new float?((!this.ducksTop) ? funhousePlatformingLevelCar.transform.localScale.y : (-funhousePlatformingLevelCar.transform.localScale.y)), null);
			if (i == this.carCount - 1)
			{
				lastCar = funhousePlatformingLevelCar;
			}
			index = (index + 1) % 4;
		}
		while (lastCar.transform.position.x > CupheadLevelCamera.Current.transform.position.x)
		{
			yield return null;
		}
		this.carsSpawning = false;
		yield return null;
		yield break;
	}

	// Token: 0x060033F5 RID: 13301 RVA: 0x001E1F80 File Offset: 0x001E0380
	private void SpawnHonk(float rootY, float yScale, float offset)
	{
		AudioManager.Play("funhouse_car_honk_sweet");
		Vector2 v = new Vector2(CupheadLevelCamera.Current.Bounds.xMax, rootY + offset);
		Effect effect = this.honkEffect.Create(v);
		effect.transform.parent = CupheadLevelCamera.Current.transform;
		effect.transform.SetScale(null, new float?(yScale), null);
	}

	// Token: 0x060033F6 RID: 13302 RVA: 0x001E2000 File Offset: 0x001E0400
	protected override void OnDrawGizmos()
	{
		Gizmos.color = new Color(1f, 1f, 0f, 1f);
		Gizmos.DrawWireSphere(this.topSpawnRoot.transform.position, 50f);
		Gizmos.color = new Color(1f, 0f, 1f, 1f);
		Gizmos.DrawWireSphere(this.bottomSpawnRoot.transform.position, 50f);
	}

	// Token: 0x04003C31 RID: 15409
	[SerializeField]
	private Effect honkEffect;

	// Token: 0x04003C32 RID: 15410
	[SerializeField]
	private Transform topSpawnRoot;

	// Token: 0x04003C33 RID: 15411
	[SerializeField]
	private Transform bottomSpawnRoot;

	// Token: 0x04003C34 RID: 15412
	[Header("Cars")]
	[SerializeField]
	private FunhousePlatformingLevelCar carPrefabNormal;

	// Token: 0x04003C35 RID: 15413
	[SerializeField]
	private float carSpeed;

	// Token: 0x04003C36 RID: 15414
	[SerializeField]
	private float carDelay;

	// Token: 0x04003C37 RID: 15415
	[SerializeField]
	private float carSpacing;

	// Token: 0x04003C38 RID: 15416
	[SerializeField]
	private int carCount;

	// Token: 0x04003C39 RID: 15417
	[Header("Ducks")]
	[SerializeField]
	private FunhousePlatformingLevelDuck bigDuckPrefab;

	// Token: 0x04003C3A RID: 15418
	[SerializeField]
	private FunhousePlatformingLevelDuck smallDuckPrefab;

	// Token: 0x04003C3B RID: 15419
	[SerializeField]
	private FunhousePlatformingLevelDuck smallDuckPinkPrefab;

	// Token: 0x04003C3C RID: 15420
	[SerializeField]
	private float duckDelay;

	// Token: 0x04003C3D RID: 15421
	[SerializeField]
	private float duckCount;

	// Token: 0x04003C3E RID: 15422
	[SerializeField]
	private float duckSpacing;

	// Token: 0x04003C3F RID: 15423
	[SerializeField]
	private string duckPinkString;

	// Token: 0x04003C40 RID: 15424
	private int duckPinkIndex;

	// Token: 0x04003C41 RID: 15425
	private bool carsSpawning;

	// Token: 0x04003C42 RID: 15426
	private bool ducksSpawning;

	// Token: 0x04003C43 RID: 15427
	private bool ducksTop;

	// Token: 0x04003C44 RID: 15428
	private bool firstTime = true;
}
