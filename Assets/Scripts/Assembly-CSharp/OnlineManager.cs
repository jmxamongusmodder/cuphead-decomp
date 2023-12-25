using System;

// Token: 0x020009C7 RID: 2503
public class OnlineManager
{
	// Token: 0x170004D0 RID: 1232
	// (get) Token: 0x06003AD6 RID: 15062 RVA: 0x00212527 File Offset: 0x00210927
	public static OnlineManager Instance
	{
		get
		{
			if (OnlineManager.instance == null)
			{
				OnlineManager.instance = new OnlineManager();
			}
			return OnlineManager.instance;
		}
	}

	// Token: 0x170004D1 RID: 1233
	// (get) Token: 0x06003AD7 RID: 15063 RVA: 0x00212542 File Offset: 0x00210942
	// (set) Token: 0x06003AD8 RID: 15064 RVA: 0x0021254A File Offset: 0x0021094A
	public OnlineInterface Interface { get; private set; }

	// Token: 0x06003AD9 RID: 15065 RVA: 0x00212553 File Offset: 0x00210953
	public void Init()
	{
		this.Interface = new OnlineInterfaceSteam();
		this.Interface.Init();
	}

	// Token: 0x0400429F RID: 17055
	private static OnlineManager instance;
}
