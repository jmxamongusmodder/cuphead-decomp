using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200064B RID: 1611
public class FlyingCowboyLevelBird : AbstractProjectile
{
	// Token: 0x0600211C RID: 8476 RVA: 0x0013219F File Offset: 0x0013059F
	public void Initialize(Vector3 startPosition, Vector3 endPosition, float bulletLandingPosition, LevelProperties.FlyingCowboy.Bird properties, FlyingCowboyLevelCowboy cowgirl)
	{
		this.bulletLandingPosition = bulletLandingPosition;
		this.properties = properties;
		this.cowgirl = cowgirl;
		base.StartCoroutine(this.move_cr(startPosition, endPosition, properties));
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x0600211D RID: 8477 RVA: 0x001321D8 File Offset: 0x001305D8
	public void InitializeIntro(Vector3 startPosition)
	{
		base.transform.position = startPosition;
		SpriteRenderer component = base.GetComponent<SpriteRenderer>();
		component.sortingLayerName = "Default";
		component.sortingOrder = -120;
		base.animator.Play("Return");
	}

	// Token: 0x0600211E RID: 8478 RVA: 0x0013221B File Offset: 0x0013061B
	public void MoveIntro(Vector3 endPosition, LevelProperties.FlyingCowboy.Bird properties)
	{
		base.StartCoroutine(this.moveIntro_cr(endPosition, properties));
	}

	// Token: 0x0600211F RID: 8479 RVA: 0x0013222C File Offset: 0x0013062C
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002120 RID: 8480 RVA: 0x0013224C File Offset: 0x0013064C
	private void move()
	{
		Vector3 position = base.transform.position;
		position.x += this.properties.speed * CupheadTime.FixedDelta;
		base.transform.position = position;
	}

	// Token: 0x06002121 RID: 8481 RVA: 0x00132290 File Offset: 0x00130690
	private IEnumerator move_cr(Vector3 startPosition, Vector3 endPosition, LevelProperties.FlyingCowboy.Bird properties)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		while (base.transform.position.x < endPosition.x - 60f)
		{
			yield return wait;
			this.move();
		}
		while (base.animator.GetCurrentAnimatorStateInfo(1).IsName("Throw"))
		{
			yield return wait;
			this.move();
		}
		float normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		while (normalizedTime >= 0.18181819f && normalizedTime <= 0.8181818f)
		{
			yield return wait;
			this.move();
			normalizedTime = base.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
		}
		base.animator.Play("Turn");
		float slowdownTime = KinematicUtilities.CalculateTimeToChangeVelocity(properties.speed, 0f, 60f);
		float elapsedTime = 0f;
		while (elapsedTime < slowdownTime)
		{
			yield return wait;
			elapsedTime += CupheadTime.FixedDelta;
			Vector3 position = base.transform.position;
			position.x += Mathf.Lerp(properties.speed, 0f, elapsedTime / slowdownTime) * CupheadTime.FixedDelta;
			base.transform.position = position;
		}
		yield return base.animator.WaitForNormalizedTime(this, 0.75f, "Turn", 0, false, false, true);
		elapsedTime = 0f;
		while (base.transform.position.x > startPosition.x)
		{
			yield return wait;
			elapsedTime += CupheadTime.FixedDelta;
			float speed = Mathf.Lerp(0f, properties.speed, elapsedTime / 0.25f);
			Vector3 position2 = base.transform.position;
			position2.x -= properties.speed * CupheadTime.FixedDelta;
			base.transform.position = position2;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002122 RID: 8482 RVA: 0x001322C0 File Offset: 0x001306C0
	private IEnumerator moveIntro_cr(Vector3 endPosition, LevelProperties.FlyingCowboy.Bird properties)
	{
		WaitForFixedUpdate wait = new WaitForFixedUpdate();
		while (base.transform.position.x > endPosition.x)
		{
			yield return wait;
			Vector3 position = base.transform.position;
			position.x -= properties.speed * CupheadTime.FixedDelta;
			base.transform.position = position;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002123 RID: 8483 RVA: 0x001322EC File Offset: 0x001306EC
	private IEnumerator attack_cr()
	{
		if (this.bulletLandingPosition > -400f)
		{
			yield break;
		}
		while (base.transform.position.x < -385f)
		{
			yield return null;
		}
		if (this.projectileSpawned || base.animator.GetCurrentAnimatorStateInfo(0).IsName("Turn"))
		{
			yield break;
		}
		base.animator.RoundFrame(0);
		base.animator.Play("Throw", 1);
		yield return base.animator.WaitForNormalizedTime(this, 1f, "Throw", 1, true, false, true);
		base.animator.Play("Off", 1);
		this.holdingFeetRenderer.enabled = false;
		this.emptyFeetRenderer.enabled = true;
		yield break;
	}

	// Token: 0x06002124 RID: 8484 RVA: 0x00132308 File Offset: 0x00130708
	private void spawnProjectile()
	{
		if (this.projectileSpawned)
		{
			return;
		}
		this.projectileSpawned = true;
		this.SFX_COWGIRL_COWGIRL_P1_BirdCall();
		Vector3 position = this.projectileSpawnPoint.position;
		float num = KinematicUtilities.CalculateInitialSpeedToReachApex(this.properties.bulletArcHeight, this.properties.bulletGravity);
		float distance = this.bulletLandingPosition - position.x;
		float x = KinematicUtilities.CalculateHorizontalSpeedToTravelDistance(distance, num, position.y - FlyingCowboyLevelBirdProjectile.HighLandingPosition, this.properties.bulletGravity);
		Vector2 initialVelocity = new Vector2(x, num);
		float num2 = Mathf.Atan2(initialVelocity.y, initialVelocity.x) * 57.29578f;
		FlyingCowboyLevelBirdProjectile flyingCowboyLevelBirdProjectile = this.projectilePrefab.Create(this.projectileSpawnPoint.position) as FlyingCowboyLevelBirdProjectile;
		flyingCowboyLevelBirdProjectile.Initialize(initialVelocity, this.properties.bulletGravity, this.properties.shrapnelSecondStageDelay, this.properties.shrapnelSpeed, this.properties.shrapnelSpreadAngle, this.cowgirl);
		flyingCowboyLevelBirdProjectile.shrapnelCount = this.properties.shrapnelCount;
	}

	// Token: 0x06002125 RID: 8485 RVA: 0x00132418 File Offset: 0x00130818
	private void animationEvent_SpawnProjectile()
	{
		this.spawnProjectile();
	}

	// Token: 0x06002126 RID: 8486 RVA: 0x00132420 File Offset: 0x00130820
	private void animationEvent_ShiftLayers()
	{
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.sortingLayerName = "Background";
		}
	}

	// Token: 0x06002127 RID: 8487 RVA: 0x00132457 File Offset: 0x00130857
	private void SFX_COWGIRL_COWGIRL_P1_BirdCall()
	{
		AudioManager.Play("sfx_dlc_cowgirl_p1_birdcall");
		this.emitAudioFromObject.Add("sfx_dlc_cowgirl_p1_birdcall");
	}

	// Token: 0x040029B5 RID: 10677
	[SerializeField]
	private FlyingCowboyLevelBirdProjectile projectilePrefab;

	// Token: 0x040029B6 RID: 10678
	[SerializeField]
	private SpriteRenderer holdingFeetRenderer;

	// Token: 0x040029B7 RID: 10679
	[SerializeField]
	private SpriteRenderer emptyFeetRenderer;

	// Token: 0x040029B8 RID: 10680
	[SerializeField]
	private Transform projectileSpawnPoint;

	// Token: 0x040029B9 RID: 10681
	private LevelProperties.FlyingCowboy.Bird properties;

	// Token: 0x040029BA RID: 10682
	private FlyingCowboyLevelCowboy cowgirl;

	// Token: 0x040029BB RID: 10683
	private float bulletLandingPosition;

	// Token: 0x040029BC RID: 10684
	private bool projectileSpawned;
}
