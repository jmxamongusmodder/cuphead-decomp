using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200077A RID: 1914
public class RobotLevelBlockade : AbstractCollidableObject
{
	// Token: 0x060029ED RID: 10733 RVA: 0x00188704 File Offset: 0x00186B04
	public RobotLevelBlockade Create(Vector3 origin, int dir)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		gameObject.transform.position = origin + Vector3.up * (float)dir * 300f;
		this.rootSegment = gameObject.GetComponent<RobotLevelBlockade>();
		return this.rootSegment;
	}

	// Token: 0x060029EE RID: 10734 RVA: 0x00188756 File Offset: 0x00186B56
	public void InitBlockade(int dir, int xSpeed, int ySpeed)
	{
		this.xSpeed = xSpeed;
		this.ySpeed = ySpeed * -dir;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060029EF RID: 10735 RVA: 0x00188776 File Offset: 0x00186B76
	protected override void Awake()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x060029F0 RID: 10736 RVA: 0x00188789 File Offset: 0x00186B89
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060029F1 RID: 10737 RVA: 0x001887A1 File Offset: 0x00186BA1
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060029F2 RID: 10738 RVA: 0x001887C0 File Offset: 0x00186BC0
	private IEnumerator move_cr()
	{
		for (;;)
		{
			base.transform.position += (Vector3.left * (float)this.xSpeed + Vector3.up * (float)this.ySpeed) * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x040032CF RID: 13007
	private const float heightOffset = 300f;

	// Token: 0x040032D0 RID: 13008
	private DamageDealer damageDealer;

	// Token: 0x040032D1 RID: 13009
	private RobotLevelBlockade rootSegment;

	// Token: 0x040032D2 RID: 13010
	private int xSpeed;

	// Token: 0x040032D3 RID: 13011
	private int ySpeed;
}
