using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000704 RID: 1796
public class OldManLevelGoose : AbstractProjectile
{
	// Token: 0x0600269D RID: 9885 RVA: 0x001698F4 File Offset: 0x00167CF4
	public virtual OldManLevelGoose Init(Vector2 pos, float speed, LevelProperties.OldMan.GooseAttack properties, bool hasCollision, string sortingLayer, int sortingOrder, float whiten)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = pos;
		this.properties = properties;
		this.speed = speed;
		this.coll.enabled = hasCollision;
		this.rend.sortingLayerName = sortingLayer;
		this.rend.color = new Color(whiten, whiten, whiten);
		if (sortingLayer == "Foreground")
		{
			this.rend.material = this.altMaterial;
			base.gameObject.layer = 31;
		}
		this.rend.sortingOrder = sortingOrder;
		this.anim.Play((UnityEngine.Random.Range(0, 8) % 6).ToString());
		this.Move();
		return this;
	}

	// Token: 0x0600269E RID: 9886 RVA: 0x001699C1 File Offset: 0x00167DC1
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600269F RID: 9887 RVA: 0x001699DF File Offset: 0x00167DDF
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060026A0 RID: 9888 RVA: 0x001699FD File Offset: 0x00167DFD
	private void Move()
	{
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x060026A1 RID: 9889 RVA: 0x00169A0C File Offset: 0x00167E0C
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		while (base.transform.position.x > (float)Level.Current.Left - 1000f)
		{
			base.transform.position += Vector3.left * this.speed * CupheadTime.FixedDelta;
			yield return wait;
		}
		this.Recycle<OldManLevelGoose>();
		yield return null;
		yield break;
	}

	// Token: 0x04002F56 RID: 12118
	private const float OFFSET = 1000f;

	// Token: 0x04002F57 RID: 12119
	private LevelProperties.OldMan.GooseAttack properties;

	// Token: 0x04002F58 RID: 12120
	private float speed;

	// Token: 0x04002F59 RID: 12121
	[SerializeField]
	private BoxCollider2D coll;

	// Token: 0x04002F5A RID: 12122
	[SerializeField]
	private Animator anim;

	// Token: 0x04002F5B RID: 12123
	[SerializeField]
	private SpriteRenderer rend;

	// Token: 0x04002F5C RID: 12124
	[SerializeField]
	private Material altMaterial;
}
