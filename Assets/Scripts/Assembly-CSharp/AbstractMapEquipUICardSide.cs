using System;
using UnityEngine;

// Token: 0x02000986 RID: 2438
public class AbstractMapEquipUICardSide : AbstractMonoBehaviour
{
	// Token: 0x170004A3 RID: 1187
	// (get) Token: 0x06003900 RID: 14592 RVA: 0x00205EE8 File Offset: 0x002042E8
	// (set) Token: 0x06003901 RID: 14593 RVA: 0x00205EF0 File Offset: 0x002042F0
	private protected PlayerId playerID { protected get; private set; }

	// Token: 0x06003902 RID: 14594 RVA: 0x00205EF9 File Offset: 0x002042F9
	protected override void Awake()
	{
		base.Awake();
		this.canvasGroup = base.GetComponent<CanvasGroup>();
	}

	// Token: 0x06003903 RID: 14595 RVA: 0x00205F0D File Offset: 0x0020430D
	public virtual void Init(PlayerId playerID)
	{
		this.playerID = playerID;
	}

	// Token: 0x06003904 RID: 14596 RVA: 0x00205F16 File Offset: 0x00204316
	public void SetActive(bool active)
	{
		this.canvasGroup.alpha = (float)((!active) ? 0 : 1);
	}

	// Token: 0x04004091 RID: 16529
	protected CanvasGroup canvasGroup;
}
