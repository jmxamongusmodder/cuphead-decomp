using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006ED RID: 1773
public class MouseLevelFallingObject : AbstractProjectile
{
	// Token: 0x060025F8 RID: 9720 RVA: 0x0016381C File Offset: 0x00161C1C
	public MouseLevelFallingObject Create(float xPos, LevelProperties.Mouse.Claw properties)
	{
		Vector2 vector = new Vector2(-600f, 50f);
		MouseLevelFallingObject mouseLevelFallingObject = this.InstantiatePrefab<MouseLevelFallingObject>();
		mouseLevelFallingObject.GetComponent<Animator>().SetInteger("Pick", UnityEngine.Random.Range(0, 3));
		mouseLevelFallingObject.speed = properties.objectStartingFallSpeed;
		mouseLevelFallingObject.gravity = properties.objectGravity;
		mouseLevelFallingObject.transform.SetPosition(new float?(xPos + vector.x), new float?((float)Level.Current.Ceiling + vector.y), null);
		mouseLevelFallingObject.StartCoroutine(mouseLevelFallingObject.move_cr());
		return mouseLevelFallingObject;
	}

	// Token: 0x060025F9 RID: 9721 RVA: 0x001638B8 File Offset: 0x00161CB8
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060025FA RID: 9722 RVA: 0x001638D6 File Offset: 0x00161CD6
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060025FB RID: 9723 RVA: 0x001638F4 File Offset: 0x00161CF4
	private IEnumerator move_cr()
	{
		while (base.transform.position.y > (float)Level.Current.Ground)
		{
			this.speed += this.gravity * CupheadTime.FixedDelta;
			base.transform.AddPosition(0f, -this.speed * CupheadTime.FixedDelta, 0f);
			yield return new WaitForFixedUpdate();
		}
		this.explosionSmall.Create(base.transform.position);
		base.animator.SetTrigger("Death");
		AudioManager.Play("level_mouse_debris_smash");
		this.emitAudioFromObject.Add("level_mouse_debris_smash");
		yield break;
	}

	// Token: 0x060025FC RID: 9724 RVA: 0x0016390F File Offset: 0x00161D0F
	private void DestroyWood()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060025FD RID: 9725 RVA: 0x0016391C File Offset: 0x00161D1C
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.explosionSmall = null;
	}

	// Token: 0x04002E83 RID: 11907
	[SerializeField]
	private Effect explosionSmall;

	// Token: 0x04002E84 RID: 11908
	private float gravity;

	// Token: 0x04002E85 RID: 11909
	private float speed;
}
