using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000744 RID: 1860
public class RetroArcadeChaserManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x06002885 RID: 10373 RVA: 0x00179B71 File Offset: 0x00177F71
	public void StartChasers()
	{
		this.SetupSpawnPoints();
		base.StartCoroutine(this.chasers_cr());
	}

	// Token: 0x06002886 RID: 10374 RVA: 0x00179B88 File Offset: 0x00177F88
	private void SetupSpawnPoints()
	{
		int num = 8;
		float num2 = (float)(Level.Current.Right / (num - 1) * 2);
		float num3 = 5f;
		int num4 = 0;
		this.spawnPositions = new Vector3[num * 2];
		for (int i = 0; i < num * 2; i++)
		{
			float x = (float)Level.Current.Left + num2 * (float)num4;
			float y = (i >= num) ? ((float)Level.Current.Ground + num3) : ((float)Level.Current.Ceiling - num3);
			this.spawnPositions[i] = new Vector3(x, y);
			num4 = ((i != num - 1) ? (num4 + 1) : 0);
		}
	}

	// Token: 0x06002887 RID: 10375 RVA: 0x00179C44 File Offset: 0x00178044
	private IEnumerator chasers_cr()
	{
		LevelProperties.RetroArcade.Chasers p = base.properties.CurrentState.chasers;
		int mainColorIndex = UnityEngine.Random.Range(0, p.colorString.Length);
		string[] colorString = p.colorString[mainColorIndex].Split(new char[]
		{
			','
		});
		int mainDelayIndex = UnityEngine.Random.Range(0, p.delayString.Length);
		string[] delayString = p.delayString[mainDelayIndex].Split(new char[]
		{
			','
		});
		int delayIndex = UnityEngine.Random.Range(0, delayString.Length);
		int mainOrderIndex = UnityEngine.Random.Range(0, p.orderString.Length);
		string[] orderString = p.orderString[mainOrderIndex].Split(new char[]
		{
			','
		});
		int orderIndex = UnityEngine.Random.Range(0, orderString.Length);
		RetroArcadeChaser chaserSelected = null;
		int spawnIndex = 0;
		float delay = 0f;
		float chaserSpeed = 0f;
		float chaserrotation = 0f;
		float chaserHp = 0f;
		float chaserDuration = 0f;
		this.chasers = new List<RetroArcadeChaser>();
		for (int i = 0; i < colorString.Length; i++)
		{
			AbstractPlayerController player = PlayerManager.GetNext();
			orderString = p.orderString[mainOrderIndex].Split(new char[]
			{
				','
			});
			delayString = p.delayString[mainDelayIndex].Split(new char[]
			{
				','
			});
			if (colorString[i][0] == 'G')
			{
				chaserSelected = this.chaserGreenPrefab;
				chaserSpeed = p.greenSpeed;
				chaserrotation = p.greenRotation;
				chaserHp = p.greenHP;
				chaserDuration = p.greenDuration;
			}
			else if (colorString[i][0] == 'Y')
			{
				chaserSelected = this.chaserYellowPrefab;
				chaserSpeed = p.yellowSpeed;
				chaserrotation = p.yellowRotation;
				chaserHp = p.yellowHP;
				chaserDuration = p.yellowDuration;
			}
			else if (colorString[i][0] == 'O')
			{
				chaserSelected = this.chaserOrangePrefab;
				chaserSpeed = p.orangeSpeed;
				chaserrotation = p.orangeRotation;
				chaserHp = p.orangeHP;
				chaserDuration = p.orangeDuration;
			}
			Parser.IntTryParse(orderString[orderIndex], out spawnIndex);
			Parser.FloatTryParse(delayString[delayIndex], out delay);
			RetroArcadeChaser chaser = chaserSelected.Spawn<RetroArcadeChaser>();
			chaser.Init(this.spawnPositions[spawnIndex], 0f, chaserSpeed, chaserSpeed, chaserrotation, chaserDuration, chaserHp, player, p);
			this.chasers.Add(chaser);
			if (orderIndex < p.orderString.Length - 1)
			{
				orderIndex++;
			}
			else
			{
				mainOrderIndex = (mainOrderIndex + 1) % p.orderString.Length;
				orderIndex = 0;
			}
			if (delayIndex < p.delayString.Length - 1)
			{
				delayIndex++;
			}
			else
			{
				mainDelayIndex = (mainDelayIndex + 1) % p.delayString.Length;
				delayIndex = 0;
			}
			yield return CupheadTime.WaitForSeconds(this, delay);
		}
		bool isDone = false;
		while (!isDone)
		{
			isDone = true;
			foreach (RetroArcadeChaser retroArcadeChaser in this.chasers)
			{
				isDone = retroArcadeChaser.IsDone;
			}
			yield return null;
		}
		foreach (RetroArcadeChaser obj in this.chasers)
		{
			UnityEngine.Object.Destroy(obj);
		}
		base.properties.DealDamageToNextNamedState();
		yield return null;
		yield break;
	}

	// Token: 0x0400315A RID: 12634
	[SerializeField]
	private RetroArcadeChaser chaserGreenPrefab;

	// Token: 0x0400315B RID: 12635
	[SerializeField]
	private RetroArcadeChaser chaserYellowPrefab;

	// Token: 0x0400315C RID: 12636
	[SerializeField]
	private RetroArcadeChaser chaserOrangePrefab;

	// Token: 0x0400315D RID: 12637
	private Vector3[] spawnPositions;

	// Token: 0x0400315E RID: 12638
	private List<RetroArcadeChaser> chasers;
}
