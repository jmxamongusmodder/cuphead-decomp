using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000745 RID: 1861
public class RetroArcadeMissile : AbstractCollidableObject
{
	// Token: 0x06002889 RID: 10377 RVA: 0x0017A2CC File Offset: 0x001786CC
	public void Init(Vector2 pos, float rotation, LevelProperties.RetroArcade.Missile properties, Vector3 pivot)
	{
		base.transform.position = pos;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		this.properties = properties;
		this.pivotPoint = pivot;
	}

	// Token: 0x0600288A RID: 10378 RVA: 0x0017A31E File Offset: 0x0017871E
	private void Start()
	{
		this.loopXSize = this.properties.loopXSize;
		this.loopYSize = this.properties.loopYSize;
		this.damageDealer = DamageDealer.NewEnemy();
		this.Deactivate();
	}

	// Token: 0x0600288B RID: 10379 RVA: 0x0017A353 File Offset: 0x00178753
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600288C RID: 10380 RVA: 0x0017A36B File Offset: 0x0017876B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600288D RID: 10381 RVA: 0x0017A389 File Offset: 0x00178789
	public void StartCircle(bool onRight, Vector3 pivot)
	{
		this.circleAngle = 0f;
		this.pivotPoint = pivot;
		base.GetComponent<SpriteRenderer>().enabled = true;
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.move_in_circle_cr(onRight));
	}

	// Token: 0x0600288E RID: 10382 RVA: 0x0017A3C4 File Offset: 0x001787C4
	private IEnumerator move_in_circle_cr(bool onRight)
	{
		Vector3 handleRotationX = Vector3.zero;
		float rotateInCir;
		if (onRight)
		{
			rotateInCir = -90f;
		}
		else
		{
			rotateInCir = 90f;
		}
		while (this.circleAngle < 6.108652f)
		{
			this.circleAngle += 5f * CupheadTime.Delta;
			if (onRight)
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.circleAngle) * this.loopXSize, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(Mathf.Sin(this.circleAngle) * this.loopXSize, 0f, 0f);
			}
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.circleAngle) * this.loopYSize, 0f);
			base.transform.position = this.pivotPoint;
			base.transform.position += handleRotationX + handleRotationY;
			Vector3 dir = this.pivotPoint - base.transform.position;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(dir) + rotateInCir));
			yield return null;
		}
		this.Deactivate();
		yield return null;
		yield break;
	}

	// Token: 0x0600288F RID: 10383 RVA: 0x0017A3E6 File Offset: 0x001787E6
	private void Deactivate()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x0400315F RID: 12639
	private LevelProperties.RetroArcade.Missile properties;

	// Token: 0x04003160 RID: 12640
	private float loopYSize;

	// Token: 0x04003161 RID: 12641
	private float loopXSize;

	// Token: 0x04003162 RID: 12642
	private float circleAngle;

	// Token: 0x04003163 RID: 12643
	private Vector3 pivotPoint;

	// Token: 0x04003164 RID: 12644
	private DamageDealer damageDealer;
}
