using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200076E RID: 1902
public class RetroArcadeWormTongue : AbstractCollidableObject
{
	// Token: 0x170003E6 RID: 998
	// (get) Token: 0x0600295C RID: 10588 RVA: 0x00181D12 File Offset: 0x00180112
	// (set) Token: 0x0600295D RID: 10589 RVA: 0x00181D1A File Offset: 0x0018011A
	public float TileRotationSpeed { get; private set; }

	// Token: 0x0600295E RID: 10590 RVA: 0x00181D23 File Offset: 0x00180123
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		base.GetComponentInChildren<Collider2D>().enabled = false;
	}

	// Token: 0x0600295F RID: 10591 RVA: 0x00181D42 File Offset: 0x00180142
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002960 RID: 10592 RVA: 0x00181D5A File Offset: 0x0018015A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x06002961 RID: 10593 RVA: 0x00181D71 File Offset: 0x00180171
	public void Init(LevelProperties.RetroArcade.Worm properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002962 RID: 10594 RVA: 0x00181D7A File Offset: 0x0018017A
	public void Extend()
	{
		base.StartCoroutine(this.main_cr());
	}

	// Token: 0x06002963 RID: 10595 RVA: 0x00181D89 File Offset: 0x00180189
	public void Retract()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.retract_cr());
	}

	// Token: 0x06002964 RID: 10596 RVA: 0x00181DA0 File Offset: 0x001801A0
	private IEnumerator main_cr()
	{
		float extendTime = 4.45f;
		float t = 0f;
		while (t < extendTime)
		{
			base.transform.SetPosition(new float?(this.parent.transform.position.x), new float?(this.parent.transform.position.y + Mathf.Lerp(-250f, 195f, t / extendTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		base.GetComponentInChildren<Collider2D>().enabled = true;
		for (;;)
		{
			float rotation = this.tongueSpinner.eulerAngles.z;
			this.tongueSpinner.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation + this.properties.tongueRotateSpeed * CupheadTime.FixedDelta * -1f));
			base.transform.SetPosition(new float?(this.parent.transform.position.x), new float?(this.parent.transform.position.y + 195f), null);
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002965 RID: 10597 RVA: 0x00181DBC File Offset: 0x001801BC
	private IEnumerator retract_cr()
	{
		float retractTime = 4.45f;
		base.GetComponentInChildren<Collider2D>().enabled = false;
		float t = 0f;
		while (t < retractTime)
		{
			base.transform.SetPosition(new float?(this.parent.transform.position.x), new float?(this.parent.transform.position.y + Mathf.Lerp(195f, -250f, t / retractTime)), null);
			t += CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x04003260 RID: 12896
	private const float RETRACTED_Y_OFFSET = -250f;

	// Token: 0x04003261 RID: 12897
	private const float EXTENDED_Y_OFFSET = 195f;

	// Token: 0x04003262 RID: 12898
	private const float EXTEND_MOVE_SPEED = 100f;

	// Token: 0x04003263 RID: 12899
	private LevelProperties.RetroArcade.Worm properties;

	// Token: 0x04003264 RID: 12900
	[SerializeField]
	private Transform tongueSpinner;

	// Token: 0x04003265 RID: 12901
	[SerializeField]
	private RetroArcadeWorm parent;

	// Token: 0x04003267 RID: 12903
	private DamageDealer damageDealer;
}
