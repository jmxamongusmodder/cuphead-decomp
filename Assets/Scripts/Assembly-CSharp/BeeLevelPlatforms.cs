using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000515 RID: 1301
public class BeeLevelPlatforms : AbstractMonoBehaviour
{
	// Token: 0x06001730 RID: 5936 RVA: 0x000D0230 File Offset: 0x000CE630
	public void Init()
	{
		List<Transform> list = new List<Transform>(this.rows);
		for (int i = 0; i < this.rows.Length; i++)
		{
			Transform transform = UnityEngine.Object.Instantiate<Transform>(this.rows[i]);
			list.Add(transform);
			transform.transform.SetParent(base.transform);
			transform.transform.AddLocalPosition(0f, -230f, 0f);
		}
		this.rows = list.ToArray();
	}

	// Token: 0x06001731 RID: 5937 RVA: 0x000D02B0 File Offset: 0x000CE6B0
	public void Randomize(int missingCount)
	{
		foreach (Transform transform in this.rows)
		{
			List<Transform> list = new List<Transform>(transform.GetChildTransforms());
			foreach (Transform transform2 in list)
			{
				transform2.gameObject.SetActive(true);
			}
			for (int j = 0; j < missingCount; j++)
			{
				if (list.Count <= 1)
				{
					break;
				}
				int num = UnityEngine.Random.Range(0, list.Count);
				if (num == 0 && BeeLevelPlatforms.lastPlatform == 0)
				{
					break;
				}
				if (num == 3 && BeeLevelPlatforms.lastPlatform == 2)
				{
					break;
				}
				list[num].gameObject.SetActive(false);
				BeeLevelPlatforms.lastPlatform = num;
				list.RemoveAt(num);
			}
		}
	}

	// Token: 0x0400206D RID: 8301
	private const float OFFSET = -230f;

	// Token: 0x0400206E RID: 8302
	[SerializeField]
	private Transform[] rows;

	// Token: 0x0400206F RID: 8303
	private static int lastPlatform;
}
