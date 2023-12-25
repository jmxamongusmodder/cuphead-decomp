using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007F0 RID: 2032
public class SnowCultLevelPlatform : LevelPlatform
{
	// Token: 0x06002EA4 RID: 11940 RVA: 0x001B81CE File Offset: 0x001B65CE
	public void SetID(int value)
	{
		base.animator.SetInteger("ID", value % 5);
	}

	// Token: 0x06002EA5 RID: 11941 RVA: 0x001B81E4 File Offset: 0x001B65E4
	public void StartRotate(float angle, Vector3 pivotPoint, float loopSizeX, float loopSizeY, float speed, float pathOffset, bool isClockwise)
	{
		this.angle = angle;
		this.pivotPos = pivotPoint;
		this.speed = speed;
		this.loopSizeX = loopSizeX;
		this.loopSizeY = loopSizeY;
		this.isClockwise = isClockwise;
		base.StartCoroutine(this.move_rotating_platforms_cr());
		base.StartCoroutine(this.sheen_cr());
	}

	// Token: 0x06002EA6 RID: 11942 RVA: 0x001B8238 File Offset: 0x001B6638
	private IEnumerator move_rotating_platforms_cr()
	{
		Vector3 handleRotationX = Vector3.zero;
		if (this.angle == 0f || this.angle == 180f)
		{
			base.transform.parent.position = this.pivotPos + MathUtils.AngleToDirection(this.angle) * this.loopSizeX;
		}
		else
		{
			base.transform.parent.position = this.pivotPos + MathUtils.AngleToDirection(this.angle) * this.loopSizeY;
		}
		this.angle *= 0.017453292f;
		for (;;)
		{
			this.angle += this.speed * CupheadTime.FixedDelta;
			if (this.isClockwise)
			{
				handleRotationX = new Vector3(Mathf.Sin(this.angle) * this.loopSizeX, 0f, 0f);
			}
			else
			{
				handleRotationX = new Vector3(-Mathf.Sin(this.angle) * this.loopSizeX, 0f, 0f);
			}
			Vector3 handleRotationY = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSizeY, 0f);
			base.transform.parent.position = this.pivotPos;
			base.transform.parent.position += handleRotationX + handleRotationY;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06002EA7 RID: 11943 RVA: 0x001B8254 File Offset: 0x001B6654
	private IEnumerator sheen_cr()
	{
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, UnityEngine.Random.Range(this.sheenTimeMin, this.sheenTimeMax));
			base.animator.SetTrigger("Sheen");
			while (!base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Sheen"))
			{
				yield return null;
			}
			while (base.animator.GetCurrentAnimatorStateInfo(0).IsTag("Sheen"))
			{
				yield return null;
			}
		}
		yield break;
	}

	// Token: 0x06002EA8 RID: 11944 RVA: 0x001B8270 File Offset: 0x001B6670
	private void FixedUpdate()
	{
		this.player1 = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		this.player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		if (this.player1 != null)
		{
			this.p1IsColliding = (this.player1.transform.parent == base.transform);
			this.player1.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p1IsColliding = false;
		}
		if (this.player2 != null)
		{
			this.p2IsColliding = (this.player2.transform.parent == base.transform);
			this.player2.transform.SetEulerAngles(null, null, new float?(0f));
		}
		else
		{
			this.p2IsColliding = false;
		}
		base.transform.localPosition = Vector3.Lerp(base.transform.localPosition, new Vector3(0f, (!this.p1IsColliding && !this.p2IsColliding) ? 0f : this.downDist), this.bounceSpeed * CupheadTime.FixedDelta);
	}

	// Token: 0x0400373E RID: 14142
	[SerializeField]
	private float downDist = -30f;

	// Token: 0x0400373F RID: 14143
	[SerializeField]
	private float bounceSpeed = 20f;

	// Token: 0x04003740 RID: 14144
	private Vector3 pivotPos;

	// Token: 0x04003741 RID: 14145
	private bool isClockwise;

	// Token: 0x04003742 RID: 14146
	private float angle;

	// Token: 0x04003743 RID: 14147
	private float speed;

	// Token: 0x04003744 RID: 14148
	private float loopSizeX;

	// Token: 0x04003745 RID: 14149
	private float loopSizeY;

	// Token: 0x04003746 RID: 14150
	private AbstractPlayerController player1;

	// Token: 0x04003747 RID: 14151
	private AbstractPlayerController player2;

	// Token: 0x04003748 RID: 14152
	private bool p1IsColliding;

	// Token: 0x04003749 RID: 14153
	private bool p2IsColliding;

	// Token: 0x0400374A RID: 14154
	[SerializeField]
	private float sheenTimeMin = 1f;

	// Token: 0x0400374B RID: 14155
	[SerializeField]
	private float sheenTimeMax = 5f;
}
