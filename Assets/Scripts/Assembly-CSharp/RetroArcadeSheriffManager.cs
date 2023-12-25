using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000757 RID: 1879
public class RetroArcadeSheriffManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x060028EF RID: 10479 RVA: 0x0017D4BE File Offset: 0x0017B8BE
	public void StartSheriff()
	{
		this.SetupSpawnPositions();
		base.StartCoroutine(this.sheriff_cr());
	}

	// Token: 0x060028F0 RID: 10480 RVA: 0x0017D4D4 File Offset: 0x0017B8D4
	private void SetupSpawnPositions()
	{
		int num = 6;
		int num2 = 4;
		float f = this.bottom.position.y - (float)Level.Current.Ground;
		float num3 = ((float)Level.Current.Right - 20f) / (float)(num - 1) * 2f;
		float num4 = ((float)Level.Current.Ceiling - Mathf.Abs(f) - 20f) / (float)(num2 - 1) * 2f;
		int num5 = 0;
		this.spawnPositions = new List<Vector3>();
		for (int i = 0; i < num * 2; i++)
		{
			float x = (float)Level.Current.Left + 20f + num3 * (float)num5;
			float y = (i >= num) ? ((float)Level.Current.Ground + 20f) : ((float)Level.Current.Ceiling - 20f);
			this.spawnPositions.Add(new Vector3(x, y));
			num5 = ((i != num - 1) ? (num5 + 1) : 0);
		}
		num5 = 1;
		for (int j = 1; j < num2 * 2 - 1; j++)
		{
			float x2 = (j >= num2) ? ((float)Level.Current.Left + 20f) : ((float)Level.Current.Right - 20f);
			float y2 = (float)Level.Current.Ground + 20f + num4 * (float)num5;
			this.spawnPositions.Add(new Vector3(x2, y2));
			if (j == num2 - 2)
			{
				num5 = 1;
				j = num2 - 1 + 1;
			}
			else
			{
				num5++;
			}
		}
	}

	// Token: 0x060028F1 RID: 10481 RVA: 0x0017D684 File Offset: 0x0017BA84
	private IEnumerator sheriff_cr()
	{
		LevelProperties.RetroArcade.Sheriff p = base.properties.CurrentState.sheriff;
		int delayMainIndex = UnityEngine.Random.Range(0, p.delayString.Length);
		string[] delayString = p.delayString[delayMainIndex].Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
		int colorMainIndex = UnityEngine.Random.Range(0, p.colorString.Length);
		string[] colorString = p.colorString[colorMainIndex].Split(new char[]
		{
			','
		});
		int colorIndex = UnityEngine.Random.Range(0, colorString.Length);
		RetroArcadeSheriff sheriffChosen = null;
		bool direction = false;
		this.sheriffs = new List<RetroArcadeSheriff>();
		for (int i = 0; i < this.spawnPositions.Count; i++)
		{
			colorString = p.colorString[colorMainIndex].Split(new char[]
			{
				','
			});
			if (colorString[colorIndex][0] == 'G')
			{
				sheriffChosen = this.sheriffGreenPrefab;
			}
			else if (colorString[colorIndex][0] == 'Y')
			{
				sheriffChosen = this.sheriffYellowPrefab;
			}
			else if (colorString[colorIndex][0] == 'O')
			{
				sheriffChosen = this.sheriffOrangePrefab;
			}
			RetroArcadeSheriff item = sheriffChosen.Create(this.spawnPositions[i], p.moveSpeed, direction, 20f, p);
			this.sheriffs.Add(item);
			if (colorIndex < colorString.Length - 1)
			{
				colorIndex++;
			}
			else
			{
				colorMainIndex = (colorMainIndex + 1) % p.colorString.Length;
				colorIndex = 0;
			}
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		foreach (RetroArcadeSheriff retroArcadeSheriff in this.sheriffs)
		{
			retroArcadeSheriff.StartMoving();
		}
		base.StartCoroutine(this.check_if_dead_cr());
		float delay = 0f;
		Parser.FloatTryParse(delayString[delayIndex], out delay);
		yield return CupheadTime.WaitForSeconds(this, delay - this.delaySubstract);
		for (;;)
		{
			int chosen = UnityEngine.Random.Range(0, this.sheriffs.Count);
			int countDeadOnes = 0;
			for (int j = 0; j < this.sheriffs.Count; j++)
			{
				if (this.sheriffs[j].IsDead)
				{
					countDeadOnes++;
				}
			}
			if (countDeadOnes >= this.sheriffs.Count)
			{
				break;
			}
			while (this.sheriffs[chosen].IsDead)
			{
				chosen = UnityEngine.Random.Range(0, this.sheriffs.Count);
				yield return null;
			}
			AbstractPlayerController player = PlayerManager.GetNext();
			this.sheriffs[chosen].Shoot(player);
			if (delayIndex < delayString.Length - 1)
			{
				delayIndex++;
			}
			else
			{
				delayMainIndex = (delayMainIndex + 1) % p.delayString.Length;
				delayIndex = 0;
			}
			yield return null;
			Parser.FloatTryParse(delayString[delayIndex], out delay);
			yield return CupheadTime.WaitForSeconds(this, delay - this.delaySubstract);
		}
		this.EndPhase();
		yield return null;
		yield break;
	}

	// Token: 0x060028F2 RID: 10482 RVA: 0x0017D6A0 File Offset: 0x0017BAA0
	private IEnumerator check_if_dead_cr()
	{
		bool[] killedOff = new bool[this.sheriffs.Count];
		for (;;)
		{
			for (int i = 0; i < this.sheriffs.Count; i++)
			{
				if (this.sheriffs[i].IsDead && !killedOff[i])
				{
					killedOff[i] = true;
					this.HandleDeathChange();
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060028F3 RID: 10483 RVA: 0x0017D6BC File Offset: 0x0017BABC
	private void HandleDeathChange()
	{
		for (int i = 0; i < this.sheriffs.Count; i++)
		{
			this.sheriffs[i].speed += base.properties.CurrentState.sheriff.moveSpeedAddition;
		}
		this.delaySubstract += base.properties.CurrentState.sheriff.delayMinus;
	}

	// Token: 0x060028F4 RID: 10484 RVA: 0x0017D734 File Offset: 0x0017BB34
	private void EndPhase()
	{
		this.StopAllCoroutines();
		foreach (RetroArcadeSheriff retroArcadeSheriff in this.sheriffs)
		{
			UnityEngine.Object.Destroy(retroArcadeSheriff.gameObject);
		}
		base.properties.DealDamageToNextNamedState();
	}

	// Token: 0x040031D5 RID: 12757
	[SerializeField]
	private Transform bottom;

	// Token: 0x040031D6 RID: 12758
	[SerializeField]
	private RetroArcadeSheriff sheriffGreenPrefab;

	// Token: 0x040031D7 RID: 12759
	[SerializeField]
	private RetroArcadeSheriff sheriffYellowPrefab;

	// Token: 0x040031D8 RID: 12760
	[SerializeField]
	private RetroArcadeSheriff sheriffOrangePrefab;

	// Token: 0x040031D9 RID: 12761
	private List<RetroArcadeSheriff> sheriffs;

	// Token: 0x040031DA RID: 12762
	private List<Vector3> spawnPositions;

	// Token: 0x040031DB RID: 12763
	private const float offset = 20f;

	// Token: 0x040031DC RID: 12764
	private float delaySubstract;
}
