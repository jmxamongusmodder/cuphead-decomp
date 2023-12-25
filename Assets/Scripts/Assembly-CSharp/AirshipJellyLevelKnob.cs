using System;
using UnityEngine;

// Token: 0x020004D7 RID: 1239
public class AirshipJellyLevelKnob : ParrySwitch
{
	// Token: 0x17000318 RID: 792
	// (get) Token: 0x06001527 RID: 5415 RVA: 0x000BDEE7 File Offset: 0x000BC2E7
	// (set) Token: 0x06001528 RID: 5416 RVA: 0x000BDEF4 File Offset: 0x000BC2F4
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

	// Token: 0x06001529 RID: 5417 RVA: 0x000BDF04 File Offset: 0x000BC304
	public static AirshipJellyLevelKnob Create(AirshipJellyLevelJelly jelly)
	{
		GameObject gameObject = new GameObject("Airship_Jelly_Knob");
		AirshipJellyLevelKnob airshipJellyLevelKnob = gameObject.AddComponent<AirshipJellyLevelKnob>();
		airshipJellyLevelKnob.target = jelly.knobRoot;
		airshipJellyLevelKnob.tag = "ParrySwitch";
		return airshipJellyLevelKnob;
	}

	// Token: 0x0600152A RID: 5418 RVA: 0x000BDF3C File Offset: 0x000BC33C
	protected override void Awake()
	{
		base.Awake();
		CircleCollider2D circleCollider2D = base.gameObject.AddComponent<CircleCollider2D>();
		circleCollider2D.radius = 20f;
		circleCollider2D.isTrigger = true;
	}

	// Token: 0x0600152B RID: 5419 RVA: 0x000BDF6D File Offset: 0x000BC36D
	private void Update()
	{
		this.UpdateLocation();
	}

	// Token: 0x0600152C RID: 5420 RVA: 0x000BDF75 File Offset: 0x000BC375
	private void LateUpdate()
	{
		this.UpdateLocation();
	}

	// Token: 0x0600152D RID: 5421 RVA: 0x000BDF7D File Offset: 0x000BC37D
	private void UpdateLocation()
	{
		if (this.target != null)
		{
			base.transform.position = this.target.position;
		}
	}

	// Token: 0x04001E8C RID: 7820
	private Transform target;
}
