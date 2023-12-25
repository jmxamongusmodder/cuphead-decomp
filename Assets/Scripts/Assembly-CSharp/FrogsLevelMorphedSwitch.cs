using System;
using UnityEngine;

// Token: 0x020006B3 RID: 1715
public class FrogsLevelMorphedSwitch : ParrySwitch
{
	// Token: 0x170003B1 RID: 945
	// (get) Token: 0x06002465 RID: 9317 RVA: 0x00155881 File Offset: 0x00153C81
	// (set) Token: 0x06002466 RID: 9318 RVA: 0x0015588E File Offset: 0x00153C8E
	public new bool enabled
	{
		get
		{
			return base.GetComponent<Collider2D>().enabled;
		}
		set
		{
			base.GetComponent<Collider2D>().enabled = value;
		}
	}

	// Token: 0x06002467 RID: 9319 RVA: 0x0015589C File Offset: 0x00153C9C
	public static FrogsLevelMorphedSwitch Create(FrogsLevelMorphed parent)
	{
		GameObject gameObject = new GameObject("Frogs_Morphed_Handle");
		FrogsLevelMorphedSwitch frogsLevelMorphedSwitch = gameObject.AddComponent<FrogsLevelMorphedSwitch>();
		frogsLevelMorphedSwitch.target = parent.switchRoot;
		return frogsLevelMorphedSwitch;
	}

	// Token: 0x06002468 RID: 9320 RVA: 0x001558C8 File Offset: 0x00153CC8
	protected override void Awake()
	{
		base.Awake();
		CircleCollider2D circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		circleCollider2D.radius = 50f;
		circleCollider2D.isTrigger = true;
	}

	// Token: 0x06002469 RID: 9321 RVA: 0x001558F9 File Offset: 0x00153CF9
	private void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x0600246A RID: 9322 RVA: 0x00155901 File Offset: 0x00153D01
	private void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x0600246B RID: 9323 RVA: 0x00155909 File Offset: 0x00153D09
	private void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x04002D1F RID: 11551
	private Transform target;
}
