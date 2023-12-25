using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200067C RID: 1660
public class FlyingGenieLevelSphinxPiece : AbstractProjectile
{
	// Token: 0x1700039E RID: 926
	// (get) Token: 0x06002301 RID: 8961 RVA: 0x00148923 File Offset: 0x00146D23
	protected override bool DestroyedAfterLeavingScreen
	{
		get
		{
			return true;
		}
	}

	// Token: 0x06002302 RID: 8962 RVA: 0x00148928 File Offset: 0x00146D28
	public void StartMoving(LevelProperties.FlyingGenie.Sphinx properties, AbstractPlayerController player, int maxCounter, bool moveRight, string[] pinkPattern, int pinkIndex)
	{
		this.properties = properties;
		this.player = player;
		base.GetComponent<Collider2D>().enabled = true;
		this.maxCounter = maxCounter;
		this.moveRight = moveRight;
		this.pinkPattern = pinkPattern;
		this.pinkIndex = pinkIndex;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002303 RID: 8963 RVA: 0x0014897C File Offset: 0x00146D7C
	private IEnumerator move_cr()
	{
		base.StartCoroutine(this.spawn_minis_cr());
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.sphinxSplitSpeed * CupheadTime.Delta * (float)((!this.moveRight) ? -1 : 1);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002304 RID: 8964 RVA: 0x00148997 File Offset: 0x00146D97
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002305 RID: 8965 RVA: 0x001489B5 File Offset: 0x00146DB5
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002306 RID: 8966 RVA: 0x001489D4 File Offset: 0x00146DD4
	private IEnumerator spawn_minis_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.properties.miniInitialSpawnDelay);
		int counter = 0;
		while (base.transform.position.y < 720f && base.transform.position.y > -360f)
		{
			if (counter >= this.maxCounter)
			{
				break;
			}
			FlyingGenieLevelMiniCat p = this.miniCat.Create(base.transform.position, 0f, this.player, this.properties);
			p.SetParryable(this.pinkPattern[this.pinkIndex][0] == 'P');
			this.pinkIndex = (this.pinkIndex + 1) % this.pinkPattern.Length;
			counter++;
			yield return CupheadTime.WaitForSeconds(this, this.properties.miniSpawnDelay);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06002307 RID: 8967 RVA: 0x001489EF File Offset: 0x00146DEF
	protected override void RandomizeVariant()
	{
	}

	// Token: 0x06002308 RID: 8968 RVA: 0x001489F1 File Offset: 0x00146DF1
	protected override void SetTrigger(string trigger)
	{
	}

	// Token: 0x04002B9C RID: 11164
	[SerializeField]
	private FlyingGenieLevelMiniCat miniCat;

	// Token: 0x04002B9D RID: 11165
	private LevelProperties.FlyingGenie.Sphinx properties;

	// Token: 0x04002B9E RID: 11166
	private AbstractPlayerController player;

	// Token: 0x04002B9F RID: 11167
	private bool moveRight;

	// Token: 0x04002BA0 RID: 11168
	private int maxCounter;

	// Token: 0x04002BA1 RID: 11169
	private string[] pinkPattern;

	// Token: 0x04002BA2 RID: 11170
	private int pinkIndex;
}
