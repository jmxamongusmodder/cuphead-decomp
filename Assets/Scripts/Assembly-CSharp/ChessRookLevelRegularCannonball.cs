using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000553 RID: 1363
public class ChessRookLevelRegularCannonball : AbstractProjectile
{
	// Token: 0x0600195C RID: 6492 RVA: 0x000E5E78 File Offset: 0x000E4278
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x0600195D RID: 6493 RVA: 0x000E5E80 File Offset: 0x000E4280
	public ChessRookLevelRegularCannonball Create(Vector3 position, float apexHeight, float targetDistance, LevelProperties.ChessRook.RegularCannonBall properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.apexHeight = apexHeight;
		this.targetDistance = targetDistance;
		this.gravity = properties.cannonGravity;
		this.Move();
		return this;
	}

	// Token: 0x0600195E RID: 6494 RVA: 0x000E5EBC File Offset: 0x000E42BC
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600195F RID: 6495 RVA: 0x000E5EDA File Offset: 0x000E42DA
	private void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001960 RID: 6496 RVA: 0x000E5EE9 File Offset: 0x000E42E9
	protected override void Die()
	{
		this.Recycle<ChessRookLevelRegularCannonball>();
	}

	// Token: 0x06001961 RID: 6497 RVA: 0x000E5EF4 File Offset: 0x000E42F4
	private IEnumerator move_cr()
	{
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_launch");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_launch");
		float endPosY = (float)Level.Current.Ground;
		float x = new Vector3(base.transform.position.x - this.targetDistance, endPosY).x - base.transform.position.x;
		float apexDist = this.apexHeight;
		float toSqrRootForviY = Mathf.Abs(2f * this.gravity * apexDist);
		float viY = Mathf.Sqrt(toSqrRootForviY);
		float timeToApex = Mathf.Abs(viY / this.gravity);
		float toSqrtForTimeToG = Mathf.Abs(2f * (base.transform.position.y + apexDist) / this.gravity);
		float timeToGround = Mathf.Sqrt(toSqrtForTimeToG);
		float viX = x / (timeToApex + timeToGround);
		Vector3 speed = new Vector3(viX, viY);
		bool stillMoving = true;
		while (stillMoving)
		{
			speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
			base.transform.Translate(speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
			if (base.transform.position.y < (float)(Level.Current.Ground + 40))
			{
				stillMoving = false;
				break;
			}
		}
		base.animator.Play("Explode", 1, 0f);
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitground_explode");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitground_explode");
		this.rend.flipX = Rand.Bool();
		this.coll.enabled = false;
		yield break;
	}

	// Token: 0x06001962 RID: 6498 RVA: 0x000E5F10 File Offset: 0x000E4310
	private void LateUpdate()
	{
		this.shadow.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground);
		int num = (int)(Mathf.Abs(base.transform.position.y - (float)Level.Current.Ground) / this.maxShadowDistance * (float)this.shadowSprites.Length);
		this.shadow.enabled = (this.coll.enabled && num >= 0 && num < this.shadowSprites.Length);
		if (this.shadow.enabled)
		{
			this.shadow.sprite = this.shadowSprites[num];
		}
	}

	// Token: 0x04002276 RID: 8822
	private float apexTime;

	// Token: 0x04002277 RID: 8823
	private float apexHeight;

	// Token: 0x04002278 RID: 8824
	private float targetDistance;

	// Token: 0x04002279 RID: 8825
	private float gravity;

	// Token: 0x0400227A RID: 8826
	[SerializeField]
	private Collider2D coll;

	// Token: 0x0400227B RID: 8827
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x0400227C RID: 8828
	[SerializeField]
	private SpriteRenderer shadow;

	// Token: 0x0400227D RID: 8829
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x0400227E RID: 8830
	[SerializeField]
	private float maxShadowDistance = 750f;
}
