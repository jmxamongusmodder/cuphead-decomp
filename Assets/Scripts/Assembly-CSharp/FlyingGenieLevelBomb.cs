using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000663 RID: 1635
public class FlyingGenieLevelBomb : AbstractProjectile
{
	// Token: 0x0600220C RID: 8716 RVA: 0x0013D29C File Offset: 0x0013B69C
	public FlyingGenieLevelBomb Create(Vector2 pos, Vector3 targetPos, LevelProperties.FlyingGenie.Bomb properties)
	{
		FlyingGenieLevelBomb flyingGenieLevelBomb = base.Create() as FlyingGenieLevelBomb;
		flyingGenieLevelBomb.transform.position = pos;
		flyingGenieLevelBomb.properties = properties;
		flyingGenieLevelBomb.targetPos = targetPos;
		return flyingGenieLevelBomb;
	}

	// Token: 0x0600220D RID: 8717 RVA: 0x0013D2D8 File Offset: 0x0013B6D8
	protected override void Awake()
	{
		base.Awake();
		foreach (GameObject gameObject in this.explosionBeams)
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = false;
			gameObject.GetComponent<Collider2D>().enabled = false;
			gameObject.GetComponent<CollisionChild>().OnPlayerCollision += this.OnCollisionPlayer;
		}
	}

	// Token: 0x0600220E RID: 8718 RVA: 0x0013D33A File Offset: 0x0013B73A
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600220F RID: 8719 RVA: 0x0013D358 File Offset: 0x0013B758
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002210 RID: 8720 RVA: 0x0013D378 File Offset: 0x0013B778
	protected override void Start()
	{
		base.Start();
		this.readyToDetonate = false;
		foreach (GameObject gameObject in this.explosionBeams)
		{
			if (this.bombType == FlyingGenieLevelBomb.BombType.Regular)
			{
				gameObject.transform.SetScale(new float?(this.properties.bombRegularSize), new float?(this.properties.bombRegularSize), null);
			}
			else if (this.bombType == FlyingGenieLevelBomb.BombType.Diagonal)
			{
				gameObject.transform.SetScale(new float?(this.properties.bombDiagonalSize), new float?(this.properties.bombDiagonalSize), null);
			}
			else if (this.bombType == FlyingGenieLevelBomb.BombType.PlusSized)
			{
				gameObject.transform.SetScale(new float?(this.properties.bombPlusSize), new float?(this.properties.bombPlusSize), null);
			}
		}
		base.StartCoroutine(this.start_cr());
	}

	// Token: 0x06002211 RID: 8721 RVA: 0x0013D488 File Offset: 0x0013B888
	private IEnumerator start_cr()
	{
		while (base.transform.position != this.targetPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.targetPos, this.properties.bombSpeed * CupheadTime.Delta);
			yield return null;
		}
		this.readyToDetonate = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002212 RID: 8722 RVA: 0x0013D4A3 File Offset: 0x0013B8A3
	public void Explode()
	{
		base.StartCoroutine(this.explode_cr());
	}

	// Token: 0x06002213 RID: 8723 RVA: 0x0013D4B4 File Offset: 0x0013B8B4
	private IEnumerator explode_cr()
	{
		foreach (GameObject gameObject in this.explosionBeams)
		{
			gameObject.GetComponent<SpriteRenderer>().enabled = true;
			gameObject.GetComponent<Collider2D>().enabled = true;
		}
		yield return CupheadTime.WaitForSeconds(this, 1f);
		base.GetComponent<SpriteRenderer>().enabled = false;
		foreach (GameObject gameObject2 in this.explosionBeams)
		{
			gameObject2.gameObject.SetActive(false);
		}
		this.readyToDetonate = false;
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06002214 RID: 8724 RVA: 0x0013D4CF File Offset: 0x0013B8CF
	protected override void Die()
	{
		this.StopAllCoroutines();
		base.Die();
	}

	// Token: 0x04002ABA RID: 10938
	public FlyingGenieLevelBomb.BombType bombType;

	// Token: 0x04002ABB RID: 10939
	public bool readyToDetonate;

	// Token: 0x04002ABC RID: 10940
	[SerializeField]
	private GameObject[] explosionBeams;

	// Token: 0x04002ABD RID: 10941
	private LevelProperties.FlyingGenie.Bomb properties;

	// Token: 0x04002ABE RID: 10942
	private Vector3 targetPos;

	// Token: 0x02000664 RID: 1636
	public enum BombType
	{
		// Token: 0x04002AC0 RID: 10944
		Regular,
		// Token: 0x04002AC1 RID: 10945
		Diagonal,
		// Token: 0x04002AC2 RID: 10946
		PlusSized
	}
}
