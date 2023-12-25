using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200067B RID: 1659
public class FlyingGenieLevelSphinx : AbstractProjectile
{
	// Token: 0x060022F9 RID: 8953 RVA: 0x001483B1 File Offset: 0x001467B1
	public void Init(Vector3 startPos, LevelProperties.FlyingGenie.Sphinx properties, AbstractPlayerController player, string[] pinkPattern, int pinkIndex)
	{
		base.transform.position = startPos;
		this.properties = properties;
		this.player = player;
		this.pinkPattern = pinkPattern;
		this.pinkIndex = pinkIndex;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060022FA RID: 8954 RVA: 0x001483EA File Offset: 0x001467EA
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x060022FB RID: 8955 RVA: 0x00148413 File Offset: 0x00146813
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060022FC RID: 8956 RVA: 0x00148434 File Offset: 0x00146834
	private IEnumerator move_cr()
	{
		float startPos = (base.transform.position + Vector3.up * this.outOfChestY).y;
		while (base.transform.position.y < startPos)
		{
			base.transform.AddPosition(0f, this.outOfChestY * this.outOfChestSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.sphinxRenderer.sortingLayerName = "Projectiles";
		this.sphinxRenderer.sortingOrder = 2;
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 targetPos = this.player.transform.position;
		while (base.transform.position != targetPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, targetPos, this.properties.sphinxSpeed * CupheadTime.Delta);
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, this.properties.splitDelay);
		base.animator.SetTrigger("Split");
		yield break;
	}

	// Token: 0x060022FD RID: 8957 RVA: 0x0014844F File Offset: 0x0014684F
	public void Split()
	{
		base.StartCoroutine(this.split_cr());
		AudioManager.Play("genie_scarab_release");
		this.emitAudioFromObject.Add("genie_scarab_release");
	}

	// Token: 0x060022FE RID: 8958 RVA: 0x00148478 File Offset: 0x00146878
	private IEnumerator split_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		int counter = (int)Mathf.Round(this.properties.sphinxSpawnNum / 2f);
		bool moveRight = false;
		for (int i = 0; i < this.sphinxPieces.Length; i++)
		{
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			int pink = (this.pinkIndex + i * 2) % this.pinkPattern.Length;
			this.sphinxPieces[i].StartMoving(this.properties, this.player, counter, moveRight, this.pinkPattern, pink);
			moveRight = !moveRight;
			yield return null;
		}
		this.Die();
		while (base.transform.childCount > 1)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x060022FF RID: 8959 RVA: 0x00148493 File Offset: 0x00146893
	protected override void Die()
	{
		base.GetComponent<SpriteRenderer>().enabled = false;
		base.Die();
	}

	// Token: 0x04002B91 RID: 11153
	private const string SplitParameterName = "Split";

	// Token: 0x04002B92 RID: 11154
	private const string ProjectilesLayer = "Projectiles";

	// Token: 0x04002B93 RID: 11155
	[SerializeField]
	private SpriteRenderer sphinxRenderer;

	// Token: 0x04002B94 RID: 11156
	[SerializeField]
	private float outOfChestY;

	// Token: 0x04002B95 RID: 11157
	[SerializeField]
	private float outOfChestSpeed;

	// Token: 0x04002B96 RID: 11158
	public FlyingGenieLevelSphinxPiece[] sphinxPieces;

	// Token: 0x04002B97 RID: 11159
	private AbstractPlayerController player;

	// Token: 0x04002B98 RID: 11160
	private LevelProperties.FlyingGenie.Sphinx properties;

	// Token: 0x04002B99 RID: 11161
	private bool moving;

	// Token: 0x04002B9A RID: 11162
	private string[] pinkPattern;

	// Token: 0x04002B9B RID: 11163
	private int pinkIndex;
}
