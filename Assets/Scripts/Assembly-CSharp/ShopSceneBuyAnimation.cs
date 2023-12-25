using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000B04 RID: 2820
public class ShopSceneBuyAnimation : MonoBehaviour
{
	// Token: 0x06004469 RID: 17513 RVA: 0x00243364 File Offset: 0x00241764
	private void Start()
	{
		this.rightIndex = new List<int>();
		this.indexes = new List<int>
		{
			0,
			1,
			2,
			3,
			4,
			5
		};
		int index = UnityEngine.Random.Range(0, 6);
		this.rightIndex.Add(this.indexes[index]);
		this.indexes.RemoveAt(index);
		int index2 = UnityEngine.Random.Range(0, 5);
		this.rightIndex.Add(this.indexes[index2]);
		this.indexes.RemoveAt(index2);
		int index3 = UnityEngine.Random.Range(0, 4);
		this.rightIndex.Add(this.indexes[index3]);
		this.indexes.RemoveAt(index3);
		for (int i = 0; i < this.rightIndex.Count; i++)
		{
			this.coins[this.rightIndex[i]].gameObject.SetActive(true);
		}
	}

	// Token: 0x0600446A RID: 17514 RVA: 0x00243474 File Offset: 0x00241874
	private void OnDestroy()
	{
		for (int i = 0; i < this.coins.Length; i++)
		{
			this.coins[i] = null;
		}
	}

	// Token: 0x04004A03 RID: 18947
	public GameObject[] coins;

	// Token: 0x04004A04 RID: 18948
	private List<int> indexes;

	// Token: 0x04004A05 RID: 18949
	private List<int> rightIndex;
}
