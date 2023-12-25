using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200075A RID: 1882
public class RetroArcadeSnakeManager : LevelProperties.RetroArcade.Entity
{
	// Token: 0x06002908 RID: 10504 RVA: 0x0017E848 File Offset: 0x0017CC48
	public void StartSnake()
	{
		base.StartCoroutine(this.spawn_snake_cr());
	}

	// Token: 0x06002909 RID: 10505 RVA: 0x0017E858 File Offset: 0x0017CC58
	private IEnumerator spawn_snake_cr()
	{
		AbstractPlayerController player = PlayerManager.GetNext();
		this.snakeFull = new RetroArcadeSnakeBodyPart[8];
		RetroArcadeSnakeBodyPart.Direction direction = RetroArcadeSnakeBodyPart.Direction.Down;
		Vector3 startPos = new Vector3(-player.transform.position.x, 200f);
		this.snakeFull[0] = this.bodyPrefab.Create(new Vector2(startPos.x, startPos.y), true, direction, this, this.snakeFull[0], base.properties.CurrentState.snake.moveSpeed);
		for (int i = 1; i < 8; i++)
		{
			this.snakeFull[i] = this.bodyPrefab.Create(new Vector2(startPos.x, startPos.y + (float)i * 60f), i == 0, direction, this, (i != 0) ? this.snakeFull[i - 1] : this.snakeFull[i], base.properties.CurrentState.snake.moveSpeed);
		}
		this.snakeFull[0].GetPartBehind(this.snakeFull[1]);
		yield return null;
		yield break;
	}

	// Token: 0x0600290A RID: 10506 RVA: 0x0017E873 File Offset: 0x0017CC73
	public void EndPhase()
	{
		base.StartCoroutine(this.end_phase_cr());
	}

	// Token: 0x0600290B RID: 10507 RVA: 0x0017E884 File Offset: 0x0017CC84
	private IEnumerator end_phase_cr()
	{
		for (int j = 0; j < this.snakeFull.Length; j++)
		{
			this.snakeFull[j].Die();
		}
		yield return CupheadTime.WaitForSeconds(this, 0.1f);
		for (int i = this.snakeFull.Length - 1; i >= 0; i--)
		{
			UnityEngine.Object.Destroy(this.snakeFull[i].gameObject);
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		base.properties.DealDamageToNextNamedState();
		yield return null;
		yield break;
	}

	// Token: 0x040031F2 RID: 12786
	[SerializeField]
	private RetroArcadeSnakeBodyPart bodyPrefab;

	// Token: 0x040031F3 RID: 12787
	private RetroArcadeSnakeBodyPart[] snakeFull;

	// Token: 0x040031F4 RID: 12788
	private const int BODYPARTS = 8;

	// Token: 0x040031F5 RID: 12789
	private const float SPACING = 60f;

	// Token: 0x040031F6 RID: 12790
	private const float OFFSCREEN_Y = 300f;
}
