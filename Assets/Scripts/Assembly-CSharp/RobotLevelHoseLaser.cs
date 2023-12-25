using System;
using UnityEngine;

// Token: 0x02000771 RID: 1905
public class RobotLevelHoseLaser : AbstractCollidableObject
{
	// Token: 0x06002980 RID: 10624 RVA: 0x00183F40 File Offset: 0x00182340
	public RobotLevelHoseLaser Create(Vector3 pos, float angle, RobotLevelRobotBodyPart parent)
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		gameObject.transform.parent = parent.transform;
		gameObject.transform.position = pos;
		gameObject.transform.SetEulerAngles(null, null, new float?(angle));
		base.GetComponent<Collider2D>().enabled = false;
		return gameObject.GetComponent<RobotLevelHoseLaser>();
	}

	// Token: 0x06002981 RID: 10625 RVA: 0x00183FAB File Offset: 0x001823AB
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002982 RID: 10626 RVA: 0x00183FBE File Offset: 0x001823BE
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002983 RID: 10627 RVA: 0x00183FD6 File Offset: 0x001823D6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x04003280 RID: 12928
	private DamageDealer damageDealer;
}
