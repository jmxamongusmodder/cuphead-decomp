using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200064C RID: 1612
public class FlyingCowboyLevelBirdProjectile : BasicProjectile
{
	// Token: 0x1700038A RID: 906
	// (get) Token: 0x06002129 RID: 8489 RVA: 0x00132B1C File Offset: 0x00130F1C
	protected override Vector3 Direction
	{
		get
		{
			return this._direction;
		}
	}

	// Token: 0x1700038B RID: 907
	// (get) Token: 0x0600212A RID: 8490 RVA: 0x00132B24 File Offset: 0x00130F24
	// (set) Token: 0x0600212B RID: 8491 RVA: 0x00132B2C File Offset: 0x00130F2C
	public int shrapnelCount { get; set; }

	// Token: 0x0600212C RID: 8492 RVA: 0x00132B35 File Offset: 0x00130F35
	protected override void Start()
	{
		base.Start();
		base.StartCoroutine(this.landingPosition_cr());
		base.StartCoroutine(this.shrapnel_cr());
		base.StartCoroutine(this.shadow_cr());
	}

	// Token: 0x0600212D RID: 8493 RVA: 0x00132B64 File Offset: 0x00130F64
	public void Initialize(Vector2 initialVelocity, float gravity, float shrapnelDelay, float shrapnelSpeed, float shrapnelSpreadAngle, FlyingCowboyLevelCowboy cowgirl)
	{
		this.Speed = initialVelocity.magnitude;
		this._direction = initialVelocity.normalized;
		this.gravity = gravity;
		this.shrapnelDelay = shrapnelDelay;
		this.shrapnelSpeed = shrapnelSpeed;
		this.shrapnelSpreadAngle = shrapnelSpreadAngle;
		this.cowgirl = cowgirl;
		this.landingPosition = FlyingCowboyLevelBirdProjectile.HighLandingPosition;
	}

	// Token: 0x0600212E RID: 8494 RVA: 0x00132BC4 File Offset: 0x00130FC4
	protected override void FixedUpdate()
	{
		Vector3 vector = this.Direction * this.Speed;
		vector.y -= this.gravity * CupheadTime.FixedDelta;
		this._direction = vector.normalized;
		this.Speed = vector.magnitude;
		base.FixedUpdate();
	}

	// Token: 0x0600212F RID: 8495 RVA: 0x00132C20 File Offset: 0x00131020
	private IEnumerator landingPosition_cr()
	{
		while (base.transform.position.y > 0f)
		{
			yield return null;
		}
		if (this.cowgirl.onBottom && this.cowgirl.state == FlyingCowboyLevelCowboy.State.BeamAttack)
		{
			this.landingPosition = FlyingCowboyLevelBirdProjectile.LowLandingPosition;
		}
		yield break;
	}

	// Token: 0x06002130 RID: 8496 RVA: 0x00132C3C File Offset: 0x0013103C
	private IEnumerator shadow_cr()
	{
		while (base.transform.position.y > this.landingPosition + FlyingCowboyLevelBirdProjectile.ShadowTriggerDistance)
		{
			yield return null;
		}
		base.animator.Play("Land", FlyingCowboyLevelBirdProjectile.ShadowLayer);
		base.animator.Update(0f);
		while (!base.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelBirdProjectile.ShadowLayer).IsName("Off"))
		{
			Vector3 position = this.shadowTransform.position;
			position.y = this.landingPosition;
			this.shadowTransform.position = position;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002131 RID: 8497 RVA: 0x00132C58 File Offset: 0x00131058
	private IEnumerator shrapnel_cr()
	{
		while (base.transform.position.y > this.landingPosition)
		{
			yield return null;
		}
		Transform transform = base.transform;
		float? y = new float?(this.landingPosition);
		transform.SetPosition(null, y, null);
		this.move = false;
		float initialAngle = (180f - this.shrapnelSpreadAngle) * 0.5f;
		float angleInterval = this.shrapnelSpreadAngle / (float)(this.shrapnelCount - 1);
		for (int i = 0; i < this.shrapnelCount; i += 2)
		{
			this.shrapnelPrefab.Create(this.spawnPoint.position, initialAngle + angleInterval * (float)i, this.shrapnelSpeed);
		}
		this.SFX_COWGIRL_COWGIRL_P1_DynamiteExp();
		base.animator.Play("Bounce");
		base.animator.Play("A", FlyingCowboyLevelBirdProjectile.ExplosionLayer);
		base.animator.Play("A", FlyingCowboyLevelBirdProjectile.SmokeLayer);
		base.StartCoroutine(this.moveSmoke_cr("A"));
		base.animator.Play("Off", FlyingCowboyLevelBirdProjectile.ShadowLayer);
		yield return CupheadTime.WaitForSeconds(this, this.shrapnelDelay);
		for (int j = 1; j < this.shrapnelCount; j += 2)
		{
			this.shrapnelPrefab.Create(this.spawnPoint.position, initialAngle + angleInterval * (float)j, this.shrapnelSpeed);
		}
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Off");
		base.animator.Play("B", FlyingCowboyLevelBirdProjectile.ExplosionLayer);
		base.animator.Play("B", FlyingCowboyLevelBirdProjectile.SmokeLayer);
		base.StartCoroutine(this.moveSmoke_cr("B"));
		yield break;
	}

	// Token: 0x06002132 RID: 8498 RVA: 0x00132C74 File Offset: 0x00131074
	private IEnumerator moveSmoke_cr(string animationName)
	{
		Vector3 initialPosition = this.smokeTransform.position;
		yield return base.animator.WaitForAnimationToStart(this, animationName, FlyingCowboyLevelBirdProjectile.SmokeLayer, false);
		float speed = 0f;
		while (!base.animator.GetCurrentAnimatorStateInfo(FlyingCowboyLevelBirdProjectile.SmokeLayer).IsName("Off"))
		{
			yield return null;
			speed += CupheadTime.Delta * 1500f;
			Vector3 position = this.smokeTransform.position;
			position.x -= speed * CupheadTime.Delta;
			this.smokeTransform.position = position;
		}
		this.smokeTransform.position = initialPosition;
		yield break;
	}

	// Token: 0x06002133 RID: 8499 RVA: 0x00132C96 File Offset: 0x00131096
	private void animationEvent_ExplosionsFinished()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06002134 RID: 8500 RVA: 0x00132CA3 File Offset: 0x001310A3
	private void SFX_COWGIRL_COWGIRL_P1_DynamiteExp()
	{
		AudioManager.Play("sfx_DLC_Cowgirl_P1_DynamiteExp");
		this.emitAudioFromObject.Add("sfx_DLC_Cowgirl_P1_DynamiteExp");
	}

	// Token: 0x040029BD RID: 10685
	private static readonly int ExplosionLayer = 1;

	// Token: 0x040029BE RID: 10686
	private static readonly int SmokeLayer = 2;

	// Token: 0x040029BF RID: 10687
	private static readonly int ShadowLayer = 3;

	// Token: 0x040029C0 RID: 10688
	private static readonly float ShadowTriggerDistance = 260f;

	// Token: 0x040029C1 RID: 10689
	public static readonly float HighLandingPosition = -300f;

	// Token: 0x040029C2 RID: 10690
	public static readonly float LowLandingPosition = -340f;

	// Token: 0x040029C3 RID: 10691
	private Vector3 _direction;

	// Token: 0x040029C5 RID: 10693
	[SerializeField]
	private Transform shadowTransform;

	// Token: 0x040029C6 RID: 10694
	[SerializeField]
	private Transform spawnPoint;

	// Token: 0x040029C7 RID: 10695
	[SerializeField]
	private Transform smokeTransform;

	// Token: 0x040029C8 RID: 10696
	[SerializeField]
	private BasicProjectile shrapnelPrefab;

	// Token: 0x040029C9 RID: 10697
	private float landingPosition;

	// Token: 0x040029CA RID: 10698
	private float gravity;

	// Token: 0x040029CB RID: 10699
	private float shrapnelDelay;

	// Token: 0x040029CC RID: 10700
	private float shrapnelSpeed;

	// Token: 0x040029CD RID: 10701
	private float shrapnelSpreadAngle;

	// Token: 0x040029CE RID: 10702
	private FlyingCowboyLevelCowboy cowgirl;
}
