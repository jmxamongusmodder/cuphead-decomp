using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000551 RID: 1361
public class ChessRookLevelPinkCannonBall : AbstractProjectile
{
	// Token: 0x17000342 RID: 834
	// (get) Token: 0x06001946 RID: 6470 RVA: 0x000E53E5 File Offset: 0x000E37E5
	public override float ParryMeterMultiplier
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x17000343 RID: 835
	// (get) Token: 0x06001947 RID: 6471 RVA: 0x000E53EC File Offset: 0x000E37EC
	// (set) Token: 0x06001948 RID: 6472 RVA: 0x000E53F4 File Offset: 0x000E37F4
	public bool finishedOriginalArc { get; private set; }

	// Token: 0x06001949 RID: 6473 RVA: 0x000E53FD File Offset: 0x000E37FD
	public override void OnLevelEnd()
	{
	}

	// Token: 0x0600194A RID: 6474 RVA: 0x000E53FF File Offset: 0x000E37FF
	protected override void Start()
	{
		base.Start();
		this.coll = base.GetComponent<Collider2D>();
	}

	// Token: 0x0600194B RID: 6475 RVA: 0x000E5414 File Offset: 0x000E3814
	public ChessRookLevelPinkCannonBall Create(Vector3 position, float apexHeight, float targetDistance, LevelProperties.ChessRook.PinkCannonBall properties)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = position;
		this.properties = properties;
		this.apexHeight = apexHeight;
		this.targetDistance = targetDistance;
		this.gravity = properties.pinkGravity;
		this.Move();
		return this;
	}

	// Token: 0x0600194C RID: 6476 RVA: 0x000E5463 File Offset: 0x000E3863
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600194D RID: 6477 RVA: 0x000E5481 File Offset: 0x000E3881
	private void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x0600194E RID: 6478 RVA: 0x000E5490 File Offset: 0x000E3890
	private IEnumerator move_cr()
	{
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_launch");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_launch");
		float endPosY = (float)Level.Current.Ground;
		this.newRoot = new Vector3(base.transform.position.x - this.targetDistance, endPosY);
		float x = this.newRoot.x - base.transform.position.x;
		float apexDist = this.apexHeight;
		float toSqrRootForviY = Mathf.Abs(2f * this.gravity * apexDist);
		float viY = Mathf.Sqrt(toSqrRootForviY);
		float timeToApex = Mathf.Abs(viY / this.gravity);
		float toSqrtForTimeToG = Mathf.Abs(2f * (base.transform.position.y + apexDist) / this.gravity);
		float timeToGround = Mathf.Sqrt(toSqrtForTimeToG);
		float viX = x / (timeToApex + timeToGround);
		bool stillMoving = true;
		if (this.finishedOriginalArc && !this.playerOnBottom)
		{
			viY = this.properties.velocityAfterSlam;
			this.gravity = this.properties.gravityAfterSlam;
			yield return null;
		}
		Vector3 speed = new Vector3(viX, viY);
		while (stillMoving)
		{
			speed += new Vector3(0f, this.gravity * CupheadTime.FixedDelta);
			base.transform.Translate(speed * CupheadTime.FixedDelta);
			yield return new WaitForFixedUpdate();
			if (this.gotParried)
			{
				stillMoving = false;
				break;
			}
			if (base.transform.position.y < (float)(Level.Current.Ground + 120))
			{
				this.Sink(speed.x);
			}
			if (base.transform.position.y < (float)(Level.Current.Ground + 40))
			{
				this.coll.enabled = false;
			}
			if (base.transform.position.y < (float)(Level.Current.Ground - 40))
			{
				this.Die();
			}
		}
		if (this.gotParried)
		{
			this.Bounce();
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600194F RID: 6479 RVA: 0x000E54AC File Offset: 0x000E38AC
	private void Sink(float speedX)
	{
		if (this.sinking)
		{
			return;
		}
		this.sinking = true;
		this.parryColl.enabled = false;
		this.sinkFX.Create(new Vector3(base.transform.position.x + speedX / 9f, (float)(Level.Current.Ground - 40)));
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitground_explode");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitground_explode");
	}

	// Token: 0x06001950 RID: 6480 RVA: 0x000E552C File Offset: 0x000E392C
	public void Explosion()
	{
		this.StopAllCoroutines();
		this.parryColl.enabled = false;
		this.coll.enabled = false;
		this.rotatingExplosion.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.topExplosion.flipX = false;
		base.animator.Play("Explode");
		AudioManager.Play("sfx_dlc_kog_rook_ghosthead_hitsrook");
		this.emitAudioFromObject.Add("sfx_dlc_kog_rook_ghosthead_hitsrook");
	}

	// Token: 0x06001951 RID: 6481 RVA: 0x000E55B8 File Offset: 0x000E39B8
	protected override void Die()
	{
		this.Recycle<ChessRookLevelPinkCannonBall>();
	}

	// Token: 0x06001952 RID: 6482 RVA: 0x000E55C0 File Offset: 0x000E39C0
	private void Bounce()
	{
		this.gravity = this.properties.pinkReactionGravity;
		this.apexHeight = this.properties.bounceUpApexHeight + this.heightAddition;
		this.targetDistance = ((!this.playerOnLeft) ? (this.properties.bounceUpTargetDist + this.distAddition) : (-this.properties.bounceUpTargetDist - this.distAddition));
		if (!this.finishedOriginalArc)
		{
			this.finishedOriginalArc = true;
		}
		this.gotParried = false;
		this.Move();
	}

	// Token: 0x06001953 RID: 6483 RVA: 0x000E5650 File Offset: 0x000E3A50
	public void GotParried(AbstractPlayerController player)
	{
		this.playerOnLeft = (player.transform.position.x < base.transform.position.x);
		this.playerOnBottom = true;
		Vector3 v = player.center - base.transform.position;
		float num = MathUtils.DirectionToAngle(v);
		if (num < 0f)
		{
			num = 360f + num;
		}
		if (num >= 180f - this.properties.goodQuadrantClemencyLeft && num <= 270f + this.properties.goodQuadrantClemencyBottom)
		{
			base.animator.SetTrigger("Parried");
			base.GetComponent<SpriteRenderer>().sortingOrder = 1;
			float num2 = Mathf.InverseLerp(270f + this.properties.goodQuadrantClemencyBottom, 180f, num);
			this.distAddition = this.properties.distanceAddition.GetFloatAt(num2);
			this.heightAddition = this.properties.heightAddition.GetFloatAt(1f - num2);
		}
		else if (num > 270f + this.properties.goodQuadrantClemencyBottom)
		{
			float num3 = Mathf.InverseLerp(270f + this.properties.goodQuadrantClemencyBottom, 360f, num);
			this.distAddition = this.properties.distanceAdditionBack.GetFloatAt(num3);
			this.heightAddition = this.properties.heightAdditionBack.GetFloatAt(1f - num3);
		}
		else
		{
			if (this.playerOnLeft)
			{
				base.animator.SetTrigger("Parried");
			}
			float i = Mathf.InverseLerp(180f, 0f, num);
			this.distAddition = ((!this.playerOnLeft) ? this.properties.distanceAdditionBack.GetFloatAt(i) : this.properties.distanceAddition.GetFloatAt(i));
			this.heightAddition = 0f;
			base.GetComponent<SpriteRenderer>().sortingOrder = 1;
			this.playerOnBottom = false;
		}
		this.gotParried = true;
		base.StartCoroutine(this.collider_off_cr());
	}

	// Token: 0x06001954 RID: 6484 RVA: 0x000E5874 File Offset: 0x000E3C74
	private IEnumerator collider_off_cr()
	{
		this.parryColl.enabled = false;
		this.coll.enabled = false;
		yield return CupheadTime.WaitForSeconds(this, this.properties.bounceCollisionOffTimer);
		this.parryColl.enabled = true;
		this.coll.enabled = true;
		yield break;
	}

	// Token: 0x06001955 RID: 6485 RVA: 0x000E5890 File Offset: 0x000E3C90
	private void LateUpdate()
	{
		this.shadow.transform.position = new Vector3(base.transform.position.x, (float)Level.Current.Ground);
		int num = (int)(Mathf.Abs(base.transform.position.y - (float)Level.Current.Ground) / this.maxShadowDistance * (float)this.shadowSprites.Length);
		this.shadow.enabled = (this.coll.enabled && num >= 0 && num < this.shadowSprites.Length);
		if (this.shadow.enabled)
		{
			this.shadow.sprite = this.shadowSprites[num];
		}
		if (Level.Current.Ending)
		{
			this.coll.enabled = false;
			this.parryColl.enabled = false;
		}
	}

	// Token: 0x04002261 RID: 8801
	private LevelProperties.ChessRook.PinkCannonBall properties;

	// Token: 0x04002262 RID: 8802
	private Collider2D coll;

	// Token: 0x04002263 RID: 8803
	private Vector3 newRoot;

	// Token: 0x04002264 RID: 8804
	private float apexHeight;

	// Token: 0x04002265 RID: 8805
	private float targetDistance;

	// Token: 0x04002266 RID: 8806
	private float gravity;

	// Token: 0x04002267 RID: 8807
	private float distAddition;

	// Token: 0x04002268 RID: 8808
	private float heightAddition;

	// Token: 0x04002269 RID: 8809
	private bool gotParried;

	// Token: 0x0400226A RID: 8810
	private bool playerOnLeft;

	// Token: 0x0400226B RID: 8811
	private bool playerOnBottom;

	// Token: 0x0400226C RID: 8812
	[SerializeField]
	private Collider2D parryColl;

	// Token: 0x0400226D RID: 8813
	[SerializeField]
	private SpriteRenderer shadow;

	// Token: 0x0400226E RID: 8814
	[SerializeField]
	private Sprite[] shadowSprites;

	// Token: 0x0400226F RID: 8815
	[SerializeField]
	private SpriteRenderer topExplosion;

	// Token: 0x04002270 RID: 8816
	[SerializeField]
	private SpriteRenderer rotatingExplosion;

	// Token: 0x04002271 RID: 8817
	[SerializeField]
	private SpriteRenderer bigExplosion;

	// Token: 0x04002272 RID: 8818
	[SerializeField]
	private Effect sinkFX;

	// Token: 0x04002273 RID: 8819
	private bool sinking;

	// Token: 0x04002274 RID: 8820
	[SerializeField]
	private float maxShadowDistance = 750f;
}
