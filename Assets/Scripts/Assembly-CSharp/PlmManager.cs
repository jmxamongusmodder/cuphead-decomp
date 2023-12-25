using System;

// Token: 0x02000AE2 RID: 2786
public class PlmManager
{
	// Token: 0x170005F2 RID: 1522
	// (get) Token: 0x06004335 RID: 17205 RVA: 0x0023FCE7 File Offset: 0x0023E0E7
	public static PlmManager Instance
	{
		get
		{
			if (PlmManager.instance == null)
			{
				PlmManager.instance = new PlmManager();
			}
			return PlmManager.instance;
		}
	}

	// Token: 0x170005F3 RID: 1523
	// (get) Token: 0x06004336 RID: 17206 RVA: 0x0023FD02 File Offset: 0x0023E102
	// (set) Token: 0x06004337 RID: 17207 RVA: 0x0023FD0A File Offset: 0x0023E10A
	public PlmInterface Interface { get; private set; }

	// Token: 0x06004338 RID: 17208 RVA: 0x0023FD14 File Offset: 0x0023E114
	public void Init()
	{
		this.Interface = new DummyPlmInterface();
		this.Interface.Init();
		this.Interface.OnSuspend += this.OnSuspend;
		this.Interface.OnResume += this.OnResume;
		this.Interface.OnConstrained += this.OnConstrained;
		this.Interface.OnUnconstrained += this.OnUnconstrained;
	}

	// Token: 0x06004339 RID: 17209 RVA: 0x0023FD93 File Offset: 0x0023E193
	private void OnSuspend()
	{
	}

	// Token: 0x0600433A RID: 17210 RVA: 0x0023FD95 File Offset: 0x0023E195
	private void OnResume()
	{
	}

	// Token: 0x0600433B RID: 17211 RVA: 0x0023FD97 File Offset: 0x0023E197
	private void OnConstrained()
	{
	}

	// Token: 0x0600433C RID: 17212 RVA: 0x0023FD99 File Offset: 0x0023E199
	private void OnUnconstrained()
	{
	}

	// Token: 0x04004929 RID: 18729
	private static PlmManager instance;
}
