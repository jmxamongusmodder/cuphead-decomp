using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200068F RID: 1679
public class FlyingMermaidLevelMerdusaBodyPart : LevelProperties.FlyingMermaid.Entity
{
	// Token: 0x170003A1 RID: 929
	// (get) Token: 0x0600236F RID: 9071 RVA: 0x0014C8AB File Offset: 0x0014ACAB
	// (set) Token: 0x06002370 RID: 9072 RVA: 0x0014C8B3 File Offset: 0x0014ACB3
	public bool IsSinking { get; private set; }

	// Token: 0x06002371 RID: 9073 RVA: 0x0014C8BC File Offset: 0x0014ACBC
	protected override void Awake()
	{
		base.Awake();
		if (this.damagePlayer)
		{
			this.damageDealer = DamageDealer.NewEnemy();
		}
		base.StartCoroutine(this.main_cr());
		base.StartCoroutine(this.check_to_delete_cr());
	}

	// Token: 0x06002372 RID: 9074 RVA: 0x0014C8F4 File Offset: 0x0014ACF4
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002373 RID: 9075 RVA: 0x0014C91D File Offset: 0x0014AD1D
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002374 RID: 9076 RVA: 0x0014C938 File Offset: 0x0014AD38
	private IEnumerator main_cr()
	{
		AudioManager.Play("level_mermaid_merdusa_fallapart_break");
		yield return CupheadTime.WaitForSeconds(this, this.waitTime);
		if (this.stopBobbingAfterWait)
		{
			base.GetComponent<FlyingMermaidLevelFloater>().enabled = false;
		}
		this.IsSinking = true;
		float t = 0f;
		while (t < this.moveTime)
		{
			base.transform.AddPosition(this.velocity.x * CupheadTime.Delta, this.velocity.y * CupheadTime.Delta, 0f);
			base.transform.Rotate(0f, 0f, this.rotationSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002375 RID: 9077 RVA: 0x0014C954 File Offset: 0x0014AD54
	public FlyingMermaidLevelMerdusaBodyPart Create(Vector2 pos)
	{
		FlyingMermaidLevelMerdusaBodyPart flyingMermaidLevelMerdusaBodyPart = UnityEngine.Object.Instantiate<FlyingMermaidLevelMerdusaBodyPart>(this);
		flyingMermaidLevelMerdusaBodyPart.transform.SetPosition(new float?(pos.x), new float?(pos.y), null);
		return flyingMermaidLevelMerdusaBodyPart;
	}

	// Token: 0x06002376 RID: 9078 RVA: 0x0014C998 File Offset: 0x0014AD98
	private IEnumerator check_to_delete_cr()
	{
		while (base.transform.position.x >= -1140f && base.transform.position.x <= 1140f && base.transform.position.y >= -860f && base.transform.position.y <= 1220f)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04002C11 RID: 11281
	[SerializeField]
	private float waitTime;

	// Token: 0x04002C12 RID: 11282
	[SerializeField]
	private Vector2 velocity;

	// Token: 0x04002C13 RID: 11283
	[SerializeField]
	private float moveTime;

	// Token: 0x04002C14 RID: 11284
	[SerializeField]
	private bool stopBobbingAfterWait;

	// Token: 0x04002C15 RID: 11285
	[SerializeField]
	private float rotationSpeed;

	// Token: 0x04002C16 RID: 11286
	[SerializeField]
	private bool damagePlayer;

	// Token: 0x04002C17 RID: 11287
	private DamageDealer damageDealer;
}
