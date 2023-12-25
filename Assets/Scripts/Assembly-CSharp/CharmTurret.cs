using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A47 RID: 2631
public class CharmTurret : AbstractCollidableObject
{
	// Token: 0x06003EB7 RID: 16055 RVA: 0x002261DC File Offset: 0x002245DC
	public void Init(GameObject rootObject, float circleSpeed, float projectileSpeed, float delay)
	{
		base.transform.position = rootObject.transform.position;
		this.rootObject = rootObject;
		this.circleSpeed = circleSpeed;
		this.projectileSpeed = projectileSpeed;
		this.delay = delay;
		base.StartCoroutine(this.move_cr());
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06003EB8 RID: 16056 RVA: 0x00226238 File Offset: 0x00224638
	private IEnumerator move_cr()
	{
		for (;;)
		{
			this.angle += this.circleSpeed * CupheadTime.FixedDelta;
			Vector3 handleRotationX = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
			base.transform.position = this.rootObject.transform.position;
			base.transform.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003EB9 RID: 16057 RVA: 0x00226254 File Offset: 0x00224654
	private IEnumerator shoot_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, this.delay);
			this.projectile.Create(base.transform.position, 0f, this.projectileSpeed);
			yield return null;
		}
		yield break;
	}

	// Token: 0x040045C4 RID: 17860
	[SerializeField]
	private BasicProjectile projectile;

	// Token: 0x040045C5 RID: 17861
	private float circleSpeed;

	// Token: 0x040045C6 RID: 17862
	private float projectileSpeed;

	// Token: 0x040045C7 RID: 17863
	private float delay;

	// Token: 0x040045C8 RID: 17864
	private float angle;

	// Token: 0x040045C9 RID: 17865
	private float loopSize = 200f;

	// Token: 0x040045CA RID: 17866
	private GameObject rootObject;
}
