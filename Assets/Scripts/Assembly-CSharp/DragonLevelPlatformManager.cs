using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005FA RID: 1530
public class DragonLevelPlatformManager : AbstractPausableComponent
{
	// Token: 0x06001E71 RID: 7793 RVA: 0x00118BA4 File Offset: 0x00116FA4
	public void Init(LevelProperties.Dragon.Clouds properties)
	{
		this.properties = properties;
		this.toggleDelay = true;
		this.platforms = new List<DragonLevelCloudPlatform>();
		for (int i = 0; i < this.maxPlatforms; i++)
		{
			DragonLevelCloudPlatform dragonLevelCloudPlatform = UnityEngine.Object.Instantiate<DragonLevelCloudPlatform>(this.platformPrefab);
			dragonLevelCloudPlatform.gameObject.SetActive(false);
			dragonLevelCloudPlatform.transform.parent = base.transform;
			this.platforms.Add(dragonLevelCloudPlatform);
		}
		base.StartCoroutine(this.spawn_platforms());
		base.StartCoroutine(this.run_delay_cr());
	}

	// Token: 0x06001E72 RID: 7794 RVA: 0x00118C30 File Offset: 0x00117030
	public void UpdateProperties(LevelProperties.Dragon.Clouds properties)
	{
		this.toggleDelay = true;
		this.properties = properties;
		foreach (DragonLevelCloudPlatform dragonLevelCloudPlatform in this.platforms)
		{
			dragonLevelCloudPlatform.GetProperties(properties, false);
		}
		base.StartCoroutine(this.run_delay_cr());
	}

	// Token: 0x06001E73 RID: 7795 RVA: 0x00118CA8 File Offset: 0x001170A8
	public void DestroyObjectPool(DragonLevelCloudPlatform obj)
	{
		this.platforms.Add(obj);
		obj.gameObject.SetActive(false);
	}

	// Token: 0x06001E74 RID: 7796 RVA: 0x00118CC4 File Offset: 0x001170C4
	private IEnumerator run_delay_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.cloudSpeed / 200f);
		this.toggleDelay = false;
		yield break;
	}

	// Token: 0x06001E75 RID: 7797 RVA: 0x00118CE0 File Offset: 0x001170E0
	private IEnumerator spawn_platforms()
	{
		List<string> positions = new List<string>(this.properties.cloudPositions);
		int mainIndex = UnityEngine.Random.Range(0, positions.Count);
		string[] positionString = positions[mainIndex].Split(new char[]
		{
			','
		});
		int positionIndex = UnityEngine.Random.Range(0, positionString.Length);
		int platformIndex = 0;
		float platformWidth = this.platformPrefab.GetComponent<Renderer>().bounds.size.x / 2f;
		float waitTime = 0f;
		float position = 0f;
		for (;;)
		{
			while (this.toggleDelay)
			{
				yield return null;
			}
			positionString = positions[mainIndex].Split(new char[]
			{
				','
			});
			this.startPosition = ((!this.properties.movingRight) ? (640f + platformWidth) : (-640f - platformWidth));
			if (positionString[positionIndex][0] == 'D')
			{
				Parser.FloatTryParse(positionString[positionIndex].Substring(1), out waitTime);
			}
			else
			{
				string[] array = positionString[positionIndex].Split(new char[]
				{
					'-'
				});
				foreach (string s in array)
				{
					Parser.FloatTryParse(s, out position);
					this.platforms[platformIndex].transform.position = new Vector3(this.startPosition, 360f - position, 0f);
					this.platforms[platformIndex].gameObject.SetActive(true);
					this.platforms[platformIndex].GetProperties(this, this.properties);
					platformIndex = (platformIndex + 1) % this.platforms.Count;
				}
				waitTime = this.properties.cloudDelay;
			}
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			if (positionIndex < positionString.Length - 1)
			{
				positionIndex++;
			}
			else if (positions.Count > 1)
			{
				positions.Remove(positions[mainIndex]);
				positionIndex = 0;
				mainIndex = UnityEngine.Random.Range(0, positions.Count);
			}
			else
			{
				positionIndex = 0;
				mainIndex = 0;
				positions = new List<string>(this.properties.cloudPositions);
			}
		}
		yield break;
	}

	// Token: 0x06001E76 RID: 7798 RVA: 0x00118CFB File Offset: 0x001170FB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.platformPrefab = null;
	}

	// Token: 0x04002749 RID: 10057
	private const float LEFT_X = -1280f;

	// Token: 0x0400274A RID: 10058
	private const float RIGHT_X = 1280f;

	// Token: 0x0400274B RID: 10059
	[SerializeField]
	public List<DragonLevelCloudPlatform> platforms;

	// Token: 0x0400274C RID: 10060
	[SerializeField]
	private DragonLevelCloudPlatform platformPrefab;

	// Token: 0x0400274D RID: 10061
	private LevelProperties.Dragon.Clouds properties;

	// Token: 0x0400274E RID: 10062
	private int maxPlatforms = 20;

	// Token: 0x0400274F RID: 10063
	private float startPosition;

	// Token: 0x04002750 RID: 10064
	private bool toggleDelay;
}
