using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E0 RID: 2272
public class MountainPlatformingLevelDragonSpawner : AbstractPausableComponent
{
	// Token: 0x0600352C RID: 13612 RVA: 0x001EF154 File Offset: 0x001ED554
	private void Start()
	{
		base.StartCoroutine(this.spawn_cr());
		this.spawnIndex = UnityEngine.Random.Range(0, this.spawnString.Split(new char[]
		{
			','
		}).Length);
	}

	// Token: 0x0600352D RID: 13613 RVA: 0x001EF188 File Offset: 0x001ED588
	private IEnumerator spawn_cr()
	{
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(0f, 1000f)))
			{
				if ((this.isElevator && MountainPlatformingLevelElevatorHandler.elevatorIsMoving) || !this.isElevator)
				{
					MountainPlatformingLevelDragon dragonPrefab = null;
					int scale = 1;
					int spawnPoint = 1;
					Parser.IntTryParse(this.spawnString.Split(new char[]
					{
						','
					})[this.spawnIndex], out spawnPoint);
					Vector3 startPos = new Vector3(this.spawnPoints[spawnPoint - 1].position.x, this.spawnPoints[spawnPoint - 1].position.y + 500f);
					if (spawnPoint != 1)
					{
						if (spawnPoint != 2)
						{
							if (spawnPoint == 3)
							{
								dragonPrefab = this.dragonSidePrefab;
								scale = -1;
							}
						}
						else
						{
							dragonPrefab = this.dragonMiddlePrefab;
						}
					}
					else
					{
						dragonPrefab = this.dragonSidePrefab;
					}
					MountainPlatformingLevelDragon dragon = UnityEngine.Object.Instantiate<MountainPlatformingLevelDragon>(dragonPrefab);
					dragon.Init(startPos, this.spawnPoints[spawnPoint - 1].position);
					dragon.transform.SetScale(new float?((float)scale), null, null);
					this.spawnIndex = (this.spawnIndex + 1) % this.spawnString.Split(new char[]
					{
						','
					}).Length;
					yield return CupheadTime.WaitForSeconds(this, this.spawnDelay);
				}
				yield return null;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600352E RID: 13614 RVA: 0x001EF1A3 File Offset: 0x001ED5A3
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x0600352F RID: 13615 RVA: 0x001EF1B6 File Offset: 0x001ED5B6
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06003530 RID: 13616 RVA: 0x001EF1CC File Offset: 0x001ED5CC
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(1f, 0f, 1f, a);
		foreach (Transform transform in this.spawnPoints)
		{
			Gizmos.DrawWireSphere(transform.position, 30f);
		}
	}

	// Token: 0x04003D53 RID: 15699
	[SerializeField]
	private bool isElevator;

	// Token: 0x04003D54 RID: 15700
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x04003D55 RID: 15701
	[SerializeField]
	private MountainPlatformingLevelDragon dragonMiddlePrefab;

	// Token: 0x04003D56 RID: 15702
	[SerializeField]
	private MountainPlatformingLevelDragon dragonSidePrefab;

	// Token: 0x04003D57 RID: 15703
	[SerializeField]
	private string spawnString;

	// Token: 0x04003D58 RID: 15704
	[SerializeField]
	private float spawnDelay;

	// Token: 0x04003D59 RID: 15705
	private int spawnIndex;
}
