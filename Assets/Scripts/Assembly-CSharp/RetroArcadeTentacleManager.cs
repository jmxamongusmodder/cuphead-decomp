using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200075C RID: 1884
public class RetroArcadeTentacleManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x06002913 RID: 10515 RVA: 0x0017EF9C File Offset: 0x0017D39C
	public void StartTentacle()
	{
		this.SetupYSpawnpoints();
		base.StartCoroutine(this.spawn_tentacles_cr());
	}

	// Token: 0x06002914 RID: 10516 RVA: 0x0017EFB4 File Offset: 0x0017D3B4
	private void SetupYSpawnpoints()
	{
		this.YSpawnPoints = new float[8];
		float num = (float)Level.Current.Ground - this.bottom.position.y;
		this.offset = ((float)Level.Current.Height - num) / 8f;
		for (int i = 0; i < 8; i++)
		{
			float num2 = this.offset * (float)i;
			this.YSpawnPoints[i] = num2;
		}
	}

	// Token: 0x06002915 RID: 10517 RVA: 0x0017F02C File Offset: 0x0017D42C
	private IEnumerator spawn_tentacles_cr()
	{
		this.octopusHead.SetActive(true);
		LevelProperties.RetroArcade.Tentacle p = base.properties.CurrentState.tentacle;
		int mainTargetIndex = UnityEngine.Random.Range(0, p.targetString.Length);
		string[] targetString = p.targetString[mainTargetIndex].Split(new char[]
		{
			','
		});
		int targetIndex = UnityEngine.Random.Range(0, targetString.Length);
		int counter = 0;
		int positionIndex = 0;
		bool spawningLeft = Rand.Bool();
		RetroArcadeTentacle tentacle = null;
		RetroArcadeTentacle lastLeftTentacle = null;
		RetroArcadeTentacle lastRightTentacle = null;
		this.tentacles = new RetroArcadeTentacle[p.tentacleCount];
		while (counter < p.tentacleCount)
		{
			targetString = p.targetString[mainTargetIndex].Split(new char[]
			{
				','
			});
			Parser.IntTryParse(targetString[targetIndex], out positionIndex);
			Vector3 spawnPoint = new Vector3((!spawningLeft) ? 320f : -320f, -500f);
			if (spawningLeft)
			{
				while (lastLeftTentacle != null)
				{
					if (lastLeftTentacle.transform.position.x >= -240f)
					{
						break;
					}
					yield return null;
				}
			}
			else
			{
				while (lastRightTentacle != null)
				{
					if (lastRightTentacle.transform.position.x <= 240f)
					{
						break;
					}
					yield return null;
				}
			}
			tentacle = this.tentaclePrefab.Spawn<RetroArcadeTentacle>();
			tentacle.Init(spawnPoint, this.YSpawnPoints[positionIndex], spawningLeft, p.risingSpeed, p.moveSpeed);
			this.tentacles[counter] = tentacle;
			if (spawningLeft)
			{
				lastLeftTentacle = tentacle;
			}
			else
			{
				lastRightTentacle = tentacle;
			}
			if (targetIndex < targetString.Length - 1)
			{
				targetIndex++;
			}
			else
			{
				mainTargetIndex = (mainTargetIndex + 1) % p.targetString.Length;
				targetIndex = 0;
			}
			spawningLeft = !spawningLeft;
			counter++;
			yield return null;
		}
		int countDeadOnes = 0;
		for (;;)
		{
			countDeadOnes = 0;
			for (int i = 0; i < this.tentacles.Length; i++)
			{
				if (this.tentacles[i] == null)
				{
					countDeadOnes++;
				}
			}
			if (countDeadOnes >= this.tentacles.Length)
			{
				break;
			}
			yield return null;
		}
		this.octopusHead.SetActive(false);
		base.properties.DealDamageToNextNamedState();
		yield return null;
		yield break;
	}

	// Token: 0x040031FF RID: 12799
	private const float LEFT_SIDE_SPAWN = -320f;

	// Token: 0x04003200 RID: 12800
	private const float RIGHT_SIDE_SPAWN = 320f;

	// Token: 0x04003201 RID: 12801
	private const int SPACES_COUNT = 8;

	// Token: 0x04003202 RID: 12802
	private const float SPAWN_OFFSET = 240f;

	// Token: 0x04003203 RID: 12803
	[SerializeField]
	private GameObject octopusHead;

	// Token: 0x04003204 RID: 12804
	[SerializeField]
	private RetroArcadeTentacle tentaclePrefab;

	// Token: 0x04003205 RID: 12805
	[SerializeField]
	private Transform bottom;

	// Token: 0x04003206 RID: 12806
	private RetroArcadeTentacle[] tentacles;

	// Token: 0x04003207 RID: 12807
	private float[] YSpawnPoints;

	// Token: 0x04003208 RID: 12808
	private float offset;
}
