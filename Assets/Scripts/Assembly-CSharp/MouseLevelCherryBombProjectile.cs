using System;
using UnityEngine;

// Token: 0x020006EB RID: 1771
public class MouseLevelCherryBombProjectile : AbstractProjectile
{
	// Token: 0x060025EF RID: 9711 RVA: 0x00163580 File Offset: 0x00161980
	public MouseLevelCherryBombProjectile Create(Vector2 pos, Vector2 velocity, float gravity, float childSpeed)
	{
		MouseLevelCherryBombProjectile mouseLevelCherryBombProjectile = this.InstantiatePrefab<MouseLevelCherryBombProjectile>();
		mouseLevelCherryBombProjectile.transform.position = pos;
		mouseLevelCherryBombProjectile.velocity = velocity;
		mouseLevelCherryBombProjectile.gravity = gravity;
		mouseLevelCherryBombProjectile.childSpeed = childSpeed;
		mouseLevelCherryBombProjectile.state = MouseLevelCherryBombProjectile.State.Moving;
		return mouseLevelCherryBombProjectile;
	}

	// Token: 0x060025F0 RID: 9712 RVA: 0x001635C4 File Offset: 0x001619C4
	protected override void FixedUpdate()
	{
		base.FixedUpdate();
		if (this.state == MouseLevelCherryBombProjectile.State.Moving)
		{
			if (base.transform.position.y < (float)Level.Current.Ground + 60f)
			{
				this.state = MouseLevelCherryBombProjectile.State.Dead;
				base.animator.SetTrigger("OnExplode");
				return;
			}
			base.transform.AddPosition(this.velocity.x * CupheadTime.FixedDelta, this.velocity.y * CupheadTime.FixedDelta, 0f);
			this.velocity.y = this.velocity.y - this.gravity * CupheadTime.FixedDelta;
		}
	}

	// Token: 0x060025F1 RID: 9713 RVA: 0x00163674 File Offset: 0x00161A74
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060025F2 RID: 9714 RVA: 0x0016369D File Offset: 0x00161A9D
	private void Explode()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060025F3 RID: 9715 RVA: 0x001636AC File Offset: 0x00161AAC
	private void SpawnChildren()
	{
		this.cloud.Create(new Vector3(base.transform.position.x + 20f, base.transform.position.y + 200f), new Vector3(0.52f, 0.52f, 0.52f));
		BasicProjectile basicProjectile = this.childProjectile.Create(base.transform.position - new Vector3(0f, 40f, 0f), 0f, new Vector2(0.6f, 0.6f), -this.childSpeed);
		basicProjectile.GetComponent<Animator>().SetBool("isRight", false);
		BasicProjectile basicProjectile2 = this.childProjectile.Create(base.transform.position - new Vector3(0f, 40f, 0f), 0f, new Vector2(-0.6f, -0.6f), this.childSpeed);
		basicProjectile2.GetComponent<Animator>().SetBool("isRight", true);
	}

	// Token: 0x060025F4 RID: 9716 RVA: 0x001637D1 File Offset: 0x00161BD1
	protected override void Die()
	{
		this.Explode();
		base.Die();
	}

	// Token: 0x060025F5 RID: 9717 RVA: 0x001637DF File Offset: 0x00161BDF
	private void SoundAnimCherryBomExp()
	{
		AudioManager.Play("level_mouse_cannon_bomb_explode");
		this.emitAudioFromObject.Add("level_mouse_cannon_bomb_explode");
	}

	// Token: 0x060025F6 RID: 9718 RVA: 0x001637FB File Offset: 0x00161BFB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.cloud = null;
		this.childProjectile = null;
	}

	// Token: 0x04002E78 RID: 11896
	private const float ChildProjectileScale = 0.6f;

	// Token: 0x04002E79 RID: 11897
	[SerializeField]
	private Effect cloud;

	// Token: 0x04002E7A RID: 11898
	[SerializeField]
	private BasicProjectile childProjectile;

	// Token: 0x04002E7B RID: 11899
	private MouseLevelCherryBombProjectile.State state;

	// Token: 0x04002E7C RID: 11900
	private Vector2 velocity;

	// Token: 0x04002E7D RID: 11901
	private float gravity;

	// Token: 0x04002E7E RID: 11902
	private float childSpeed;

	// Token: 0x020006EC RID: 1772
	public enum State
	{
		// Token: 0x04002E80 RID: 11904
		Init,
		// Token: 0x04002E81 RID: 11905
		Moving,
		// Token: 0x04002E82 RID: 11906
		Dead
	}
}
