using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200076B RID: 1899
public class RetroArcadeWormRocket : RetroArcadeEnemy
{
	// Token: 0x06002954 RID: 10580 RVA: 0x00181980 File Offset: 0x0017FD80
	public RetroArcadeWormRocket Create(RetroArcadeWormRocket.Direction direction, LevelProperties.RetroArcade.Worm properties)
	{
		RetroArcadeWormRocket retroArcadeWormRocket = this.InstantiatePrefab<RetroArcadeWormRocket>();
		retroArcadeWormRocket.transform.position = new Vector2((direction != RetroArcadeWormRocket.Direction.Left) ? -330f : 330f, 330f);
		retroArcadeWormRocket.properties = properties;
		retroArcadeWormRocket.direction = direction;
		retroArcadeWormRocket.hp = 1f;
		retroArcadeWormRocket.StartCoroutine(retroArcadeWormRocket.main_cr());
		return retroArcadeWormRocket;
	}

	// Token: 0x06002955 RID: 10581 RVA: 0x001819EA File Offset: 0x0017FDEA
	protected override void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.brokenPiecePrefab.Create(this.brokenPieceRoot.position, -90f, this.properties.rocketBrokenPieceSpeed);
	}

	// Token: 0x06002956 RID: 10582 RVA: 0x00181A18 File Offset: 0x0017FE18
	private IEnumerator main_cr()
	{
		base.MoveY(-60f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		while ((this.direction == RetroArcadeWormRocket.Direction.Left && base.transform.position.x > -330f) || (this.direction == RetroArcadeWormRocket.Direction.Right && base.transform.position.x < 330f))
		{
			base.transform.AddPosition((float)((this.direction != RetroArcadeWormRocket.Direction.Right) ? -1 : 1) * this.properties.rocketSpeed * CupheadTime.FixedDelta, 0f, 0f);
			yield return new WaitForFixedUpdate();
		}
		base.MoveY(60f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003255 RID: 12885
	private const float SPAWN_X = 330f;

	// Token: 0x04003256 RID: 12886
	private const float OFFSCREEN_Y = 330f;

	// Token: 0x04003257 RID: 12887
	private const float BASE_Y = 270f;

	// Token: 0x04003258 RID: 12888
	private const float MOVE_Y_SPEED = 500f;

	// Token: 0x04003259 RID: 12889
	private RetroArcadeWormRocket.Direction direction;

	// Token: 0x0400325A RID: 12890
	private LevelProperties.RetroArcade.Worm properties;

	// Token: 0x0400325B RID: 12891
	[SerializeField]
	private BasicProjectile brokenPiecePrefab;

	// Token: 0x0400325C RID: 12892
	[SerializeField]
	private Transform brokenPieceRoot;

	// Token: 0x0200076C RID: 1900
	public enum Direction
	{
		// Token: 0x0400325E RID: 12894
		Left,
		// Token: 0x0400325F RID: 12895
		Right
	}
}
