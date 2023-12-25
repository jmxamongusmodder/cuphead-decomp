using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000765 RID: 1893
public class RetroArcadeUFOMole : RetroArcadeEnemy
{
	// Token: 0x06002941 RID: 10561 RVA: 0x00180A04 File Offset: 0x0017EE04
	public RetroArcadeUFOMole Create(LevelProperties.RetroArcade.UFO properties)
	{
		RetroArcadeUFOMole retroArcadeUFOMole = this.InstantiatePrefab<RetroArcadeUFOMole>();
		retroArcadeUFOMole.properties = properties;
		retroArcadeUFOMole.direction = ((!Rand.Bool()) ? RetroArcadeUFOMole.Direction.Right : RetroArcadeUFOMole.Direction.Left);
		retroArcadeUFOMole.hp = properties.hp;
		retroArcadeUFOMole.StartCoroutine(retroArcadeUFOMole.main_cr());
		return retroArcadeUFOMole;
	}

	// Token: 0x06002942 RID: 10562 RVA: 0x00180A50 File Offset: 0x0017EE50
	private IEnumerator main_cr()
	{
		base.transform.SetPosition(new float?(UnityEngine.Random.Range(-200f, 200f)), new float?(-167f), null);
		this.direction = ((!Rand.Bool()) ? RetroArcadeUFOMole.Direction.Right : RetroArcadeUFOMole.Direction.Left);
		SpriteRenderer sprite = base.GetComponent<SpriteRenderer>();
		Collider2D col = base.GetComponent<Collider2D>();
		sprite.sortingOrder = 90;
		col.enabled = false;
		base.MoveY(-66f - base.transform.position.y, this.properties.moleAttackSpeed);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		bool[] leftOfPlayer = new bool[2];
		bool firstCheck = true;
		for (;;)
		{
			bool shouldAttack = false;
			while (!shouldAttack)
			{
				base.transform.AddPosition((float)((this.direction != RetroArcadeUFOMole.Direction.Left) ? 1 : -1) * this.properties.moleSpeed * CupheadTime.FixedDelta, 0f, 0f);
				if ((this.direction == RetroArcadeUFOMole.Direction.Left && base.transform.position.x < -200f) || (this.direction == RetroArcadeUFOMole.Direction.Right && base.transform.position.x > 200f))
				{
					this.direction = ((this.direction != RetroArcadeUFOMole.Direction.Left) ? RetroArcadeUFOMole.Direction.Left : RetroArcadeUFOMole.Direction.Right);
				}
				for (int i = 0; i < 2; i++)
				{
					ArcadePlayerController arcadePlayerController = ((i != 0) ? PlayerManager.GetPlayer(PlayerId.PlayerTwo) : PlayerManager.GetPlayer(PlayerId.PlayerOne)) as ArcadePlayerController;
					if (!(arcadePlayerController == null))
					{
						bool flag = leftOfPlayer[i];
						leftOfPlayer[i] = (arcadePlayerController.center.x < base.transform.position.x);
						if (leftOfPlayer[i] != flag && Mathf.Abs(base.transform.position.x) < 150f && arcadePlayerController.motor.Grounded && !firstCheck)
						{
							shouldAttack = true;
						}
					}
				}
				firstCheck = false;
				yield return new WaitForFixedUpdate();
			}
			yield return CupheadTime.WaitForSeconds(this, this.properties.moleWarningDelay);
			base.MoveY(-167f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			sprite.sortingOrder = 200;
			col.enabled = true;
			base.MoveY(-114f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			base.MoveY(-167f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			sprite.sortingOrder = 90;
			col.enabled = false;
			base.MoveY(-66f - base.transform.position.y, this.properties.moleAttackSpeed);
			while (this.movingY)
			{
				yield return new WaitForFixedUpdate();
			}
			firstCheck = true;
		}
		yield break;
	}

	// Token: 0x06002943 RID: 10563 RVA: 0x00180A6B File Offset: 0x0017EE6B
	public void OnWaveEnd()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.moveOffscreen_cr());
	}

	// Token: 0x06002944 RID: 10564 RVA: 0x00180A80 File Offset: 0x0017EE80
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(-167f - base.transform.position.y, this.properties.moleAttackSpeed);
		while (this.movingY)
		{
			yield return new WaitForFixedUpdate();
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04003234 RID: 12852
	private const float BACKGROUND_Y = -66f;

	// Token: 0x04003235 RID: 12853
	private const float UNDERGROUND_Y = -167f;

	// Token: 0x04003236 RID: 12854
	private const float POPUP_Y = -114f;

	// Token: 0x04003237 RID: 12855
	private const float TURNAROUND_X = 200f;

	// Token: 0x04003238 RID: 12856
	private const float MAX_ATTACK_X = 150f;

	// Token: 0x04003239 RID: 12857
	private const int BACKGROUND_SORT_ORDER = 90;

	// Token: 0x0400323A RID: 12858
	private const int ATTACK_SORT_ORDER = 200;

	// Token: 0x0400323B RID: 12859
	private RetroArcadeUFOMole.Direction direction;

	// Token: 0x0400323C RID: 12860
	private LevelProperties.RetroArcade.UFO properties;

	// Token: 0x02000766 RID: 1894
	private enum Direction
	{
		// Token: 0x0400323E RID: 12862
		Left,
		// Token: 0x0400323F RID: 12863
		Right
	}
}
