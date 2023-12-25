using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000508 RID: 1288
public class BatLevelLightning : AbstractCollidableObject
{
	// Token: 0x060016D7 RID: 5847 RVA: 0x000CD684 File Offset: 0x000CBA84
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
		this.collisionChild = this.lightning.GetComponent<CollisionChild>();
		this.collisionChild.OnPlayerCollision += this.OnCollisionPlayer;
		this.lightning.SetActive(false);
	}

	// Token: 0x060016D8 RID: 5848 RVA: 0x000CD6D7 File Offset: 0x000CBAD7
	public void Init(LevelProperties.Bat.BatLightning properties, Vector2 startPos)
	{
		this.properties = properties;
		base.transform.position = startPos;
		base.StartCoroutine(this.lightning_cr());
	}

	// Token: 0x060016D9 RID: 5849 RVA: 0x000CD700 File Offset: 0x000CBB00
	private IEnumerator lightning_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.cloudWarning);
		this.lightning.SetActive(true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.lightningOnDuration);
		this.Die();
		yield break;
	}

	// Token: 0x060016DA RID: 5850 RVA: 0x000CD71B File Offset: 0x000CBB1B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060016DB RID: 5851 RVA: 0x000CD732 File Offset: 0x000CBB32
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04002024 RID: 8228
	[SerializeField]
	private GameObject lightning;

	// Token: 0x04002025 RID: 8229
	private CollisionChild collisionChild;

	// Token: 0x04002026 RID: 8230
	private LevelProperties.Bat.BatLightning properties;

	// Token: 0x04002027 RID: 8231
	private DamageDealer damageDealer;
}
