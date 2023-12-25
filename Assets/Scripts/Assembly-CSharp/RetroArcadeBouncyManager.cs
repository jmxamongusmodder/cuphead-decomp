using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x0200073C RID: 1852
public class RetroArcadeBouncyManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x06002864 RID: 10340 RVA: 0x001787FD File Offset: 0x00176BFD
	public void StartBouncy()
	{
		base.StartCoroutine(this.spawn_balls_cr());
	}

	// Token: 0x06002865 RID: 10341 RVA: 0x0017880C File Offset: 0x00176C0C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = Color.black;
		foreach (Transform transform in this.spawnPoints)
		{
			Gizmos.DrawWireSphere(transform.position, 50f);
		}
	}

	// Token: 0x06002866 RID: 10342 RVA: 0x00178858 File Offset: 0x00176C58
	private IEnumerator spawn_balls_cr()
	{
		LevelProperties.RetroArcade.Bouncy p = base.properties.CurrentState.bouncy;
		int counter = 0;
		List<RetroArcadeBouncyBallHolder> holders = new List<RetroArcadeBouncyBallHolder>();
		int typeMainIndex = UnityEngine.Random.Range(0, p.typeString.Length);
		string[] typeString = p.typeString[typeMainIndex].Split(new char[]
		{
			','
		});
		int typeIndex = UnityEngine.Random.Range(0, typeString.Length);
		string[] ballTypes = new string[3];
		while (counter < p.waveCount)
		{
			typeString = p.typeString[typeMainIndex].Split(new char[]
			{
				','
			});
			int posIndex = UnityEngine.Random.Range(0, this.spawnPoints.Length);
			for (int i = 0; i < 3; i++)
			{
				ballTypes[i] = typeString[typeIndex];
				if (typeIndex < typeString.Length - 1)
				{
					typeIndex++;
				}
				else
				{
					typeMainIndex = (typeMainIndex + 1) % p.typeString.Length;
					typeIndex = 0;
				}
			}
			RetroArcadeBouncyBallHolder holder = this.ballHolder.Create(this, p, this.spawnPoints[posIndex].position, ballTypes);
			holders.Add(holder);
			counter++;
			yield return CupheadTime.WaitForSeconds(this, p.spawnRange.RandomFloat());
		}
		bool allDead = true;
		for (;;)
		{
			allDead = true;
			for (int j = 0; j < holders.Count; j++)
			{
				if (!holders[j].IsDead)
				{
					allDead = false;
				}
			}
			if (allDead)
			{
				break;
			}
			yield return null;
		}
		base.properties.DealDamageToNextNamedState();
		foreach (RetroArcadeBouncyBallHolder retroArcadeBouncyBallHolder in holders)
		{
			retroArcadeBouncyBallHolder.DestroyBallsHeld();
			UnityEngine.Object.Destroy(retroArcadeBouncyBallHolder.gameObject);
		}
		yield return null;
		yield break;
	}

	// Token: 0x04003123 RID: 12579
	[SerializeField]
	private RetroArcadeBouncyBallHolder ballHolder;

	// Token: 0x04003124 RID: 12580
	[SerializeField]
	private Transform[] spawnPoints;

	// Token: 0x04003125 RID: 12581
	private const int BALLCOUNT = 3;
}
