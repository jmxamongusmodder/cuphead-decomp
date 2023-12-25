using System;
using UnityEngine;

// Token: 0x02000816 RID: 2070
public class TrainLevelEngineBossTail : ParrySwitch
{
	// Token: 0x1700041D RID: 1053
	// (get) Token: 0x06002FFE RID: 12286 RVA: 0x001C61C5 File Offset: 0x001C45C5
	// (set) Token: 0x06002FFF RID: 12287 RVA: 0x001C61E9 File Offset: 0x001C45E9
	public bool tailEnabled
	{
		get
		{
			return !(this.circleCollider == null) && this.circleCollider.enabled;
		}
		set
		{
			if (this.circleCollider != null)
			{
				this.circleCollider.enabled = value;
			}
		}
	}

	// Token: 0x06003000 RID: 12288 RVA: 0x001C6208 File Offset: 0x001C4608
	public static TrainLevelEngineBossTail Create(Transform target)
	{
		GameObject gameObject = new GameObject("Engine_Boss_Tail");
		TrainLevelEngineBossTail trainLevelEngineBossTail = gameObject.AddComponent<TrainLevelEngineBossTail>();
		trainLevelEngineBossTail.target = target;
		trainLevelEngineBossTail.tag = "ParrySwitch";
		return trainLevelEngineBossTail;
	}

	// Token: 0x06003001 RID: 12289 RVA: 0x001C623A File Offset: 0x001C463A
	protected override void Awake()
	{
		base.Awake();
		this.circleCollider = base.gameObject.AddComponent<CircleCollider2D>();
		this.circleCollider.radius = 40f;
		this.circleCollider.isTrigger = true;
	}

	// Token: 0x06003002 RID: 12290 RVA: 0x001C626F File Offset: 0x001C466F
	private void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x06003003 RID: 12291 RVA: 0x001C6277 File Offset: 0x001C4677
	private void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x06003004 RID: 12292 RVA: 0x001C627F File Offset: 0x001C467F
	private void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x040038DB RID: 14555
	private CircleCollider2D circleCollider;

	// Token: 0x040038DC RID: 14556
	private Transform target;
}
