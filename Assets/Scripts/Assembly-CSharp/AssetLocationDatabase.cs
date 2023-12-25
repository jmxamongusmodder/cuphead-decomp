using System;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020003A9 RID: 937
public class AssetLocationDatabase : ScriptableObject
{
	// Token: 0x06000B92 RID: 2962 RVA: 0x00083FCC File Offset: 0x000823CC
	public void SetDLCAssets(string[] dlcAssets)
	{
		this.dlcAssetNames = dlcAssets;
	}

	// Token: 0x1700020B RID: 523
	// (get) Token: 0x06000B93 RID: 2963 RVA: 0x00083FD5 File Offset: 0x000823D5
	public HashSet<string> dlcAssets
	{
		get
		{
			if (this._dlcAssets == null)
			{
				this._dlcAssets = new HashSet<string>(this.dlcAssetNames);
			}
			return this._dlcAssets;
		}
	}

	// Token: 0x04001520 RID: 5408
	[SerializeField]
	private string[] dlcAssetNames;

	// Token: 0x04001521 RID: 5409
	private HashSet<string> _dlcAssets;

	// Token: 0x020003AA RID: 938
	public enum AssetType
	{
		// Token: 0x04001523 RID: 5411
		Base,
		// Token: 0x04001524 RID: 5412
		DLC
	}
}
