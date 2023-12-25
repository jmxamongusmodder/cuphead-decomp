using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200074A RID: 1866
public class RetroArcadeQShipTentacle : RetroArcadeEnemy
{
	// Token: 0x060028AD RID: 10413 RVA: 0x0017BAB0 File Offset: 0x00179EB0
	public RetroArcadeQShipTentacle Create(RetroArcadeQShipTentacle.Direction direction, LevelProperties.RetroArcade.QShip properties)
	{
		RetroArcadeQShipTentacle retroArcadeQShipTentacle = this.InstantiatePrefab<RetroArcadeQShipTentacle>();
		retroArcadeQShipTentacle.transform.position = new Vector2((direction != RetroArcadeQShipTentacle.Direction.Left) ? -400f : 400f, -140f);
		retroArcadeQShipTentacle.properties = properties;
		retroArcadeQShipTentacle.direction = direction;
		retroArcadeQShipTentacle.hp = 1f;
		retroArcadeQShipTentacle.transform.SetScale(new float?((float)((direction != RetroArcadeQShipTentacle.Direction.Right) ? -1 : 1)), new float?(1f), new float?(1f));
		return retroArcadeQShipTentacle;
	}

	// Token: 0x060028AE RID: 10414 RVA: 0x0017BB40 File Offset: 0x00179F40
	protected override void FixedUpdate()
	{
		base.transform.AddPosition((float)((this.direction != RetroArcadeQShipTentacle.Direction.Right) ? -1 : 1) * this.properties.tentacleSpeed * CupheadTime.FixedDelta, 0f, 0f);
		if ((this.direction != RetroArcadeQShipTentacle.Direction.Left) ? (base.transform.position.x > 400f) : (base.transform.position.x < -400f))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x060028AF RID: 10415 RVA: 0x0017BBDC File Offset: 0x00179FDC
	public override void Dead()
	{
		this.StopAllCoroutines();
		foreach (Collider2D collider2D in base.GetComponentsInChildren<Collider2D>())
		{
			collider2D.enabled = false;
		}
		base.IsDead = true;
		foreach (SpriteRenderer spriteRenderer in base.GetComponentsInChildren<SpriteRenderer>())
		{
			spriteRenderer.color = new Color(0f, 0f, 0f, 0.25f);
		}
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x060028B0 RID: 10416 RVA: 0x0017BC70 File Offset: 0x0017A070
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(-250f - base.transform.position.y, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003183 RID: 12675
	private const float SPAWN_X = 400f;

	// Token: 0x04003184 RID: 12676
	private const float SPAWN_Y = -140f;

	// Token: 0x04003185 RID: 12677
	private const float OFFSCREEN_Y = -250f;

	// Token: 0x04003186 RID: 12678
	private const float MOVE_Y_SPEED = 500f;

	// Token: 0x04003187 RID: 12679
	private LevelProperties.RetroArcade.QShip properties;

	// Token: 0x04003188 RID: 12680
	private RetroArcadeQShipTentacle.Direction direction;

	// Token: 0x0200074B RID: 1867
	public enum Direction
	{
		// Token: 0x0400318A RID: 12682
		Left,
		// Token: 0x0400318B RID: 12683
		Right
	}
}
