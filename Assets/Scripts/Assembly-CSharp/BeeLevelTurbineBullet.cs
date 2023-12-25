using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000525 RID: 1317
public class BeeLevelTurbineBullet : AbstractProjectile
{
	// Token: 0x060017B0 RID: 6064 RVA: 0x000D55BC File Offset: 0x000D39BC
	public BeeLevelTurbineBullet Create(Vector2 pos, float rotation, bool onRight, LevelProperties.Bee.TurbineBlasters properties)
	{
		BeeLevelTurbineBullet beeLevelTurbineBullet = base.Create() as BeeLevelTurbineBullet;
		beeLevelTurbineBullet.properties = properties;
		beeLevelTurbineBullet.transform.position = pos;
		beeLevelTurbineBullet.onRight = onRight;
		beeLevelTurbineBullet.direction = MathUtils.AngleToDirection(rotation);
		beeLevelTurbineBullet.velocity = properties.bulletSpeed;
		beeLevelTurbineBullet.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(rotation));
		beeLevelTurbineBullet.sprite.flipX = onRight;
		return beeLevelTurbineBullet;
	}

	// Token: 0x060017B1 RID: 6065 RVA: 0x000D5645 File Offset: 0x000D3A45
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.trail_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060017B2 RID: 6066 RVA: 0x000D5667 File Offset: 0x000D3A67
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060017B3 RID: 6067 RVA: 0x000D5685 File Offset: 0x000D3A85
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060017B4 RID: 6068 RVA: 0x000D56A4 File Offset: 0x000D3AA4
	private IEnumerator move_cr()
	{
		while (base.transform.position.y < 360f - this.loopSizeY)
		{
			base.transform.position += this.direction * this.velocity * CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.move_in_circle_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060017B5 RID: 6069 RVA: 0x000D56C0 File Offset: 0x000D3AC0
	private IEnumerator move_in_circle_cr()
	{
		this.pivotPoint = base.transform.position + Vector3.right * ((!this.onRight) ? this.loopSizeX : (-this.loopSizeX));
		Vector3 handleRotationX = Vector3.zero;
		float offset = 100f;
		this.circleAngle -= 1.5707964f;
		float endPos;
		float endVelocity;
		float rotateInCir;
		if (this.onRight)
		{
			endPos = -640f - offset;
			endVelocity = -this.velocity;
			rotateInCir = -90f;
		}
		else
		{
			endPos = 640f + offset;
			endVelocity = this.velocity;
			rotateInCir = 90f;
		}
		while (this.circleAngle < 6.108652f)
		{
			this.circleAngle += this.properties.bulletCircleTime * CupheadTime.Delta;
			if (this.onRight)
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.circleAngle) * this.loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(Mathf.Sin(this.circleAngle) * this.loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.circleAngle) * this.loopSizeY, 0f);
			base.transform.position = this.pivotPoint;
			base.transform.position += handleRotationX + handleRotationY;
			Vector3 dir = this.pivotPoint - base.transform.position;
			base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(MathUtils.DirectionToAngle(dir) + rotateInCir));
			yield return null;
		}
		while (base.transform.position.x != endPos)
		{
			base.transform.AddPosition(endVelocity * CupheadTime.Delta, 0f, 0f);
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x060017B6 RID: 6070 RVA: 0x000D56DC File Offset: 0x000D3ADC
	private IEnumerator trail_cr()
	{
		for (;;)
		{
			this.trailPrefab.Create(base.transform.position);
			yield return CupheadTime.WaitForSeconds(this, 0.25f);
		}
		yield break;
	}

	// Token: 0x060017B7 RID: 6071 RVA: 0x000D56F7 File Offset: 0x000D3AF7
	protected override void Die()
	{
		base.Die();
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
	}

	// Token: 0x040020DD RID: 8413
	[SerializeField]
	private Effect trailPrefab;

	// Token: 0x040020DE RID: 8414
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x040020DF RID: 8415
	private LevelProperties.Bee.TurbineBlasters properties;

	// Token: 0x040020E0 RID: 8416
	private float velocity;

	// Token: 0x040020E1 RID: 8417
	private float circleAngle;

	// Token: 0x040020E2 RID: 8418
	private float loopSizeY = 200f;

	// Token: 0x040020E3 RID: 8419
	private float loopSizeX = 500f;

	// Token: 0x040020E4 RID: 8420
	private bool onRight;

	// Token: 0x040020E5 RID: 8421
	private Vector3 direction;

	// Token: 0x040020E6 RID: 8422
	private Vector3 pivotPoint;
}
