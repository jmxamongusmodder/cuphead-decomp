using System;
using UnityEngine;

// Token: 0x02000372 RID: 882
public class GameObjectHelper
{
	// Token: 0x06000A29 RID: 2601 RVA: 0x0007E3D4 File Offset: 0x0007C7D4
	public GameObjectHelper(string name)
	{
		this._gameObject = new GameObject("[Helper] " + name);
		this.events = this._gameObject.AddComponent<GameObjectHelperGO>();
	}

	// Token: 0x170001F3 RID: 499
	// (get) Token: 0x06000A2A RID: 2602 RVA: 0x0007E403 File Offset: 0x0007C803
	// (set) Token: 0x06000A2B RID: 2603 RVA: 0x0007E40B File Offset: 0x0007C80B
	public GameObjectHelperGO events { get; private set; }

	// Token: 0x06000A2C RID: 2604 RVA: 0x0007E414 File Offset: 0x0007C814
	public void Destroy()
	{
		UnityEngine.Object.Destroy(this._gameObject);
	}

	// Token: 0x06000A2D RID: 2605 RVA: 0x0007E421 File Offset: 0x0007C821
	public void DontDestroyOnLoad()
	{
		UnityEngine.Object.DontDestroyOnLoad(this._gameObject);
	}

	// Token: 0x0400145E RID: 5214
	private GameObject _gameObject;
}
