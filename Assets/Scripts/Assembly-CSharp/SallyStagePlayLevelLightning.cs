using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007B0 RID: 1968
public class SallyStagePlayLevelLightning : AbstractProjectile
{
	// Token: 0x06002C3D RID: 11325 RVA: 0x001A040C File Offset: 0x0019E80C
	public SallyStagePlayLevelLightning Create(Vector2 pos, float rotation, float speed, bool lightningLast)
	{
		SallyStagePlayLevelLightning sallyStagePlayLevelLightning = base.Create(pos, rotation) as SallyStagePlayLevelLightning;
		sallyStagePlayLevelLightning.speed = speed;
		sallyStagePlayLevelLightning.rotation = rotation;
		sallyStagePlayLevelLightning.lightningLast = lightningLast;
		return sallyStagePlayLevelLightning;
	}

	// Token: 0x06002C3E RID: 11326 RVA: 0x001A0440 File Offset: 0x0019E840
	protected override void Start()
	{
		base.Start();
		this.sprite.SetEulerAngles(null, null, new float?(0f));
		base.animator.Play(UnityEngine.Random.Range(0, 3).ToStringInvariant());
		base.StartCoroutine(this.move_cr());
		AudioManager.PlayLoop("sally_sally_lightning_move_loop");
		this.emitAudioFromObject.Add("sally_sally_lightning_move_loop");
		AudioManager.Play("sally_thunder");
		this.sprite.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
	}

	// Token: 0x06002C3F RID: 11327 RVA: 0x001A04DF File Offset: 0x0019E8DF
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002C40 RID: 11328 RVA: 0x001A04FD File Offset: 0x0019E8FD
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06002C41 RID: 11329 RVA: 0x001A051C File Offset: 0x0019E91C
	protected IEnumerator move_cr()
	{
		this.velocity = base.transform.right;
		for (;;)
		{
			base.transform.position += this.velocity * this.speed * CupheadTime.FixedDelta;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002C42 RID: 11330 RVA: 0x001A0538 File Offset: 0x0019E938
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		if (phase == CollisionPhase.Enter && !this.goingBackUp)
		{
			Vector3 position = base.transform.position;
			Vector3 a = new Vector3(base.transform.position.x, (float)Level.Current.Ceiling, 0f);
			this.goingBackUp = true;
			this.collisionPoint = a - position;
			base.StartCoroutine(this.change_direction_cr(this.collisionPoint));
		}
	}

	// Token: 0x06002C43 RID: 11331 RVA: 0x001A05BC File Offset: 0x0019E9BC
	protected IEnumerator change_direction_cr(Vector3 collisionPoint)
	{
		base.transform.SetEulerAngles(null, null, new float?(-this.rotation));
		this.sprite.SetEulerAngles(null, null, new float?(0f));
		this.velocity = 1f * (-2f * Vector3.Dot(this.velocity, Vector3.Normalize(collisionPoint.normalized)) * Vector3.Normalize(collisionPoint.normalized) + this.velocity);
		yield return new WaitForEndOfFrame();
		AudioManager.Play("sally_thunder_impact");
		while (base.transform.position.y < (float)(Level.Current.Ceiling + 100))
		{
			yield return null;
		}
		if (this.lightningLast)
		{
			AudioManager.Stop("sally_sally_lightning_move_loop");
			AudioManager.Play("sally_thunder_end");
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002C44 RID: 11332 RVA: 0x001A05DE File Offset: 0x0019E9DE
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x040034E5 RID: 13541
	[SerializeField]
	private Transform sprite;

	// Token: 0x040034E6 RID: 13542
	private Vector3 velocity;

	// Token: 0x040034E7 RID: 13543
	private Vector3 collisionPoint;

	// Token: 0x040034E8 RID: 13544
	private float speed;

	// Token: 0x040034E9 RID: 13545
	private float rotation;

	// Token: 0x040034EA RID: 13546
	private bool lightningLast;

	// Token: 0x040034EB RID: 13547
	private bool goingBackUp;
}
