using System;
using UnityEngine;

// Token: 0x02000441 RID: 1089
public class CupheadEventSystem : AbstractMonoBehaviour
{
	// Token: 0x06001000 RID: 4096 RVA: 0x0009E599 File Offset: 0x0009C999
	public static void Init()
	{
		if (CupheadEventSystem._instance != null)
		{
			return;
		}
		CupheadEventSystem._instance = (UnityEngine.Object.Instantiate(Resources.Load("EventSystems/CupheadEventSystem")) as GameObject).GetComponent<CupheadEventSystem>();
	}

	// Token: 0x06001001 RID: 4097 RVA: 0x0009E5CC File Offset: 0x0009C9CC
	protected override void Awake()
	{
		base.Awake();
		if (CupheadEventSystem._instance == null)
		{
			CupheadEventSystem._instance = this;
			base.gameObject.name = base.gameObject.name.Replace("(Clone)", string.Empty);
			return;
		}
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x0400199C RID: 6556
	private const string PATH = "EventSystems/CupheadEventSystem";

	// Token: 0x0400199D RID: 6557
	private static CupheadEventSystem _instance;
}
