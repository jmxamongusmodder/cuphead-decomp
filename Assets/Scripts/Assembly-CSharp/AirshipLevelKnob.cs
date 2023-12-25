using System;
using UnityEngine;

// Token: 0x020004CA RID: 1226
public class AirshipLevelKnob : ParrySwitch
{
	// Token: 0x17000315 RID: 789
	// (get) Token: 0x060014C3 RID: 5315 RVA: 0x000BA3DB File Offset: 0x000B87DB
	// (set) Token: 0x060014C4 RID: 5316 RVA: 0x000BA3E8 File Offset: 0x000B87E8
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

	// Token: 0x060014C5 RID: 5317 RVA: 0x000BA3F8 File Offset: 0x000B87F8
	public static AirshipLevelKnob Create(Transform root)
	{
		GameObject gameObject = new GameObject("Airship_Knob");
		AirshipLevelKnob airshipLevelKnob = gameObject.AddComponent<AirshipLevelKnob>();
		airshipLevelKnob.target = root;
		airshipLevelKnob.tag = "ParrySwitch";
		return airshipLevelKnob;
	}

	// Token: 0x060014C6 RID: 5318 RVA: 0x000BA42C File Offset: 0x000B882C
	protected override void Awake()
	{
		base.Awake();
		CircleCollider2D circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		circleCollider2D.radius = 20f;
		circleCollider2D.isTrigger = true;
	}

	// Token: 0x060014C7 RID: 5319 RVA: 0x000BA45D File Offset: 0x000B885D
	private void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x060014C8 RID: 5320 RVA: 0x000BA465 File Offset: 0x000B8865
	private void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x060014C9 RID: 5321 RVA: 0x000BA46D File Offset: 0x000B886D
	private void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x04001E30 RID: 7728
	private Transform target;
}
