using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004CC RID: 1228
public class AirshipClamLevelBarnacleParryable : ParrySwitch
{
	// Token: 0x060014D3 RID: 5331 RVA: 0x000BA6D7 File Offset: 0x000B8AD7
	protected override void Awake()
	{
		this.parried = false;
		this.damageDealer = DamageDealer.NewEnemy();
		base.Awake();
	}

	// Token: 0x060014D4 RID: 5332 RVA: 0x000BA6F1 File Offset: 0x000B8AF1
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x060014D5 RID: 5333 RVA: 0x000BA700 File Offset: 0x000B8B00
	public void InitBarnacle(int dir, LevelProperties.AirshipClam properties)
	{
		this.onPlayerCollisionDeath = true;
		this.properties = properties;
		base.GetComponent<SpriteRenderer>().sprite = this.parraybleBarnacleSprite;
		this.direction = dir;
		this.circleCollider = base.GetComponent<CircleCollider2D>();
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060014D6 RID: 5334 RVA: 0x000BA74C File Offset: 0x000B8B4C
	private IEnumerator move_cr()
	{
		this.velocity = new Vector3(this.properties.CurrentState.barnacles.initialArcMovementX * (float)this.direction, this.properties.CurrentState.barnacles.initialArcMovementY, 0f);
		for (;;)
		{
			base.transform.position += this.velocity * CupheadTime.Delta;
			if (!this.parried)
			{
				this.velocity.y = this.velocity.y + this.properties.CurrentState.barnacles.initialGravity;
			}
			else
			{
				this.velocity.y = this.velocity.y + this.properties.CurrentState.barnacles.parryGravity;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060014D7 RID: 5335 RVA: 0x000BA767 File Offset: 0x000B8B67
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.onPlayerCollisionDeath)
		{
			this.damageDealer.DealDamage(hit);
			base.OnCollisionPlayer(hit, phase);
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060014D8 RID: 5336 RVA: 0x000BA794 File Offset: 0x000B8B94
	protected override void OnCollisionWalls(GameObject hit, CollisionPhase phase)
	{
		this.velocity.x = 0f;
		base.OnCollisionWalls(hit, phase);
	}

	// Token: 0x060014D9 RID: 5337 RVA: 0x000BA7AE File Offset: 0x000B8BAE
	protected override void OnCollisionGround(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionGround(hit, phase);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060014DA RID: 5338 RVA: 0x000BA7C4 File Offset: 0x000B8BC4
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		this.direction = ((base.transform.position.x <= player.center.x) ? -1 : 1);
		this.parried = true;
		this.velocity.y = this.properties.CurrentState.barnacles.parryArcMovementY;
		this.velocity.x = this.properties.CurrentState.barnacles.parryArcMovementX * (float)this.direction;
		base.OnParryPostPause(player);
		this.circleCollider.enabled = true;
		base.StartCoroutine(this.damageTypes_cr());
	}

	// Token: 0x060014DB RID: 5339 RVA: 0x000BA874 File Offset: 0x000B8C74
	private IEnumerator damageTypes_cr()
	{
		this.damageDealer.SetDamageFlags(false, false, false);
		this.onPlayerCollisionDeath = false;
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.damageDealer.SetDamageFlags(true, false, false);
		this.onPlayerCollisionDeath = true;
		yield break;
	}

	// Token: 0x060014DC RID: 5340 RVA: 0x000BA88F File Offset: 0x000B8C8F
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
	}

	// Token: 0x04001E34 RID: 7732
	public bool parried;

	// Token: 0x04001E35 RID: 7733
	private bool onPlayerCollisionDeath;

	// Token: 0x04001E36 RID: 7734
	private int direction;

	// Token: 0x04001E37 RID: 7735
	private Vector3 velocity;

	// Token: 0x04001E38 RID: 7736
	private LevelProperties.AirshipClam properties;

	// Token: 0x04001E39 RID: 7737
	private CircleCollider2D circleCollider;

	// Token: 0x04001E3A RID: 7738
	private DamageDealer damageDealer;

	// Token: 0x04001E3B RID: 7739
	[SerializeField]
	private Sprite parraybleBarnacleSprite;
}
