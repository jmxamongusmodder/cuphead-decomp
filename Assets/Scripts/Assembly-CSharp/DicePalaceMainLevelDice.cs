using System;
using System.Collections;
using UnityEngine;

// Token: 0x020005D2 RID: 1490
public class DicePalaceMainLevelDice : ParrySwitch
{
	// Token: 0x06001D50 RID: 7504 RVA: 0x0010CCB8 File Offset: 0x0010B0B8
	public void Init(Vector2 pos, LevelProperties.DicePalaceMain.Dice properties, Transform pivotPoint)
	{
		base.transform.position = pos;
		this.pivotPoint = pivotPoint;
		this.properties = properties;
		base.GetComponent<Collider2D>().enabled = false;
		this.waitingToRoll = true;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001D51 RID: 7505 RVA: 0x0010CD04 File Offset: 0x0010B104
	public void StartRoll()
	{
		base.animator.SetTrigger("StartRoll");
		this.waitingToRoll = true;
		base.animator.SetBool("Reverse", Rand.Bool());
		base.GetComponent<Collider2D>().enabled = true;
	}

	// Token: 0x06001D52 RID: 7506 RVA: 0x0010CD3E File Offset: 0x0010B13E
	public void RollOne()
	{
		this.roll = DicePalaceMainLevelDice.Roll.One;
		this.PostRoll();
	}

	// Token: 0x06001D53 RID: 7507 RVA: 0x0010CD4D File Offset: 0x0010B14D
	public void RollTwo()
	{
		this.roll = DicePalaceMainLevelDice.Roll.Two;
		this.PostRoll();
	}

	// Token: 0x06001D54 RID: 7508 RVA: 0x0010CD5C File Offset: 0x0010B15C
	public void RollThree()
	{
		this.roll = DicePalaceMainLevelDice.Roll.Three;
		this.PostRoll();
	}

	// Token: 0x06001D55 RID: 7509 RVA: 0x0010CD6B File Offset: 0x0010B16B
	private void PostRoll()
	{
		this.waitingToRoll = false;
		DicePalaceMainLevelGameInfo.TURN_COUNTER++;
	}

	// Token: 0x06001D56 RID: 7510 RVA: 0x0010CD80 File Offset: 0x0010B180
	private IEnumerator move_cr()
	{
		float loopSize = 20f;
		float speed = this.properties.movementSpeed;
		float angle = 0f;
		for (;;)
		{
			Vector3 pivotOffset = Vector3.left * 2f * loopSize;
			angle += speed * CupheadTime.Delta;
			if (angle > 6.2831855f)
			{
				this.reverse = !this.reverse;
				angle -= 6.2831855f;
			}
			if (angle < 0f)
			{
				angle += 6.2831855f;
			}
			float value;
			if (this.reverse)
			{
				base.transform.position = this.pivotPoint.position + pivotOffset;
				value = 1f;
			}
			else
			{
				base.transform.position = this.pivotPoint.position;
				value = -1f;
			}
			Vector3 handleRotationX = new Vector3(Mathf.Cos(angle) * value * loopSize, 0f, 0f);
			Vector3 handleRotationY = new Vector3(0f, Mathf.Sin(angle) * loopSize, 0f);
			base.transform.position += handleRotationX + handleRotationY;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001D57 RID: 7511 RVA: 0x0010CD9B File Offset: 0x0010B19B
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.animator.SetTrigger("Hit");
		base.GetComponent<Collider2D>().enabled = false;
	}

	// Token: 0x04002631 RID: 9777
	public DicePalaceMainLevelDice.Roll roll;

	// Token: 0x04002632 RID: 9778
	public bool waitingToRoll;

	// Token: 0x04002633 RID: 9779
	private LevelProperties.DicePalaceMain.Dice properties;

	// Token: 0x04002634 RID: 9780
	private Transform pivotPoint;

	// Token: 0x04002635 RID: 9781
	private bool reverse;

	// Token: 0x020005D3 RID: 1491
	public enum Roll
	{
		// Token: 0x04002637 RID: 9783
		One,
		// Token: 0x04002638 RID: 9784
		Two,
		// Token: 0x04002639 RID: 9785
		Three
	}
}
