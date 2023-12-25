using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000761 RID: 1889
public class RetroArcadeTrafficUFO : AbstractCollidableObject
{
	// Token: 0x170003E3 RID: 995
	// (get) Token: 0x06002929 RID: 10537 RVA: 0x00180113 File Offset: 0x0017E513
	// (set) Token: 0x0600292A RID: 10538 RVA: 0x0018011B File Offset: 0x0017E51B
	public bool IsMoving { get; private set; }

	// Token: 0x170003E4 RID: 996
	// (get) Token: 0x0600292B RID: 10539 RVA: 0x00180124 File Offset: 0x0017E524
	// (set) Token: 0x0600292C RID: 10540 RVA: 0x0018012C File Offset: 0x0017E52C
	public bool IsDead { get; private set; }

	// Token: 0x0600292D RID: 10541 RVA: 0x00180135 File Offset: 0x0017E535
	private void Start()
	{
		base.gameObject.SetActive(false);
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x0600292E RID: 10542 RVA: 0x0018014E File Offset: 0x0017E54E
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600292F RID: 10543 RVA: 0x00180166 File Offset: 0x0017E566
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002930 RID: 10544 RVA: 0x00180184 File Offset: 0x0017E584
	public void StartMoving(List<Vector3> positions, float speed, float delay)
	{
		this.IsMoving = true;
		base.StartCoroutine(this.check_pieces_cr());
		base.StartCoroutine(this.move_cr(positions, speed, delay));
	}

	// Token: 0x06002931 RID: 10545 RVA: 0x001801AC File Offset: 0x0017E5AC
	private IEnumerator move_cr(List<Vector3> positions, float speed, float delay)
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (int i = 0; i < positions.Count; i++)
		{
			yield return CupheadTime.WaitForSeconds(this, delay);
			Vector3 dir = (positions[i] - base.transform.position).normalized;
			while (Vector3.Distance(base.transform.position, positions[i]) > 3f)
			{
				base.transform.position += dir * speed * CupheadTime.FixedDelta;
				yield return wait;
			}
			yield return null;
		}
		this.IsMoving = false;
		yield break;
	}

	// Token: 0x06002932 RID: 10546 RVA: 0x001801DC File Offset: 0x0017E5DC
	private IEnumerator check_pieces_cr()
	{
		RetroArcadeTrafficUFOPiece[] pieces = base.GetComponentsInChildren<RetroArcadeTrafficUFOPiece>();
		int countDeadOnes = 0;
		for (;;)
		{
			countDeadOnes = 0;
			for (int i = 0; i < pieces.Length; i++)
			{
				if (pieces[i].IsDead)
				{
					countDeadOnes++;
				}
			}
			if (countDeadOnes >= pieces.Length)
			{
				break;
			}
			yield return null;
		}
		this.IsDead = true;
		yield return null;
		yield break;
	}

	// Token: 0x0400321D RID: 12829
	private const float DIST_TO_SWITCH = 3f;

	// Token: 0x04003220 RID: 12832
	private DamageDealer damageDealer;
}
