using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008EF RID: 2287
public class MountainPlatformingLevelScale : AbstractPausableComponent
{
	// Token: 0x060035A4 RID: 13732 RVA: 0x001F4074 File Offset: 0x001F2474
	private void Start()
	{
		this.scaleLeftStart = this.ScaleLeft.transform.position;
		this.scaleRightStart = this.ScaleRight.transform.position;
		base.StartCoroutine(this.check_scale_cr());
	}

	// Token: 0x060035A5 RID: 13733 RVA: 0x001F40C4 File Offset: 0x001F24C4
	private IEnumerator check_scale_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		for (;;)
		{
			if (this.ScaleRight.steppedOn)
			{
				if (!this.ScaleLeft.steppedOn)
				{
					if (this.ScaleRight.transform.position.y > this.scaleRightStart.y - this.scaleChangeAmount)
					{
						this.ScaleRight.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown);
					}
					if (this.ScaleLeft.transform.position.y < this.scaleLeftStart.y + this.scaleChangeAmount)
					{
						this.ScaleLeft.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
				else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					if (this.ScaleRight.transform.position.y < this.scaleRightStart.y)
					{
						this.ScaleRight.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown);
					}
					else if (this.ScaleLeft.steppedOn)
					{
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
					}
					if (this.ScaleLeft.transform.position.y > this.scaleLeftStart.y)
					{
						this.ScaleLeft.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
			}
			else
			{
				if (this.ScaleRight.transform.position.y < this.scaleRightStart.y)
				{
					this.ScaleRight.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown);
				}
				else if (!this.ScaleLeft.steppedOn)
				{
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
				}
				if (this.ScaleLeft.transform.position.y > this.scaleLeftStart.y)
				{
					this.ScaleLeft.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
				}
			}
			if (this.ScaleLeft.steppedOn)
			{
				if (!this.ScaleRight.steppedOn)
				{
					if (this.ScaleLeft.transform.position.y > this.scaleLeftStart.y - this.scaleChangeAmount)
					{
						this.ScaleLeft.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.leftDown);
					}
					if (this.ScaleRight.transform.position.y < this.scaleRightStart.y + this.scaleChangeAmount)
					{
						this.ScaleRight.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
				else if (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null)
				{
					if (this.ScaleLeft.transform.position.y < this.scaleLeftStart.y)
					{
						this.ScaleLeft.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown);
					}
					else if (this.ScaleRight.steppedOn)
					{
						this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
					}
					if (this.ScaleRight.transform.position.y > this.scaleRightStart.y)
					{
						this.ScaleRight.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					}
				}
			}
			else
			{
				if (this.ScaleLeft.transform.position.y < this.scaleLeftStart.y)
				{
					this.ScaleLeft.transform.AddPosition(0f, this.scaleSpeed * CupheadTime.FixedDelta, 0f);
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.rightDown);
				}
				else if (!this.ScaleRight.steppedOn)
				{
					this.ChangeState(MountainPlatformingLevelScale.ScaleState.still);
				}
				if (this.ScaleRight.transform.position.y > this.scaleRightStart.y)
				{
					this.ScaleRight.transform.AddPosition(0f, -this.scaleSpeed * CupheadTime.FixedDelta, 0f);
				}
			}
			yield return wait;
		}
		yield break;
	}

	// Token: 0x060035A6 RID: 13734 RVA: 0x001F40E0 File Offset: 0x001F24E0
	private void ChangeState(MountainPlatformingLevelScale.ScaleState state)
	{
		if (this.scaleState != state)
		{
			this.scaleState = state;
			string name = "goingDown";
			string name2 = "goingUp";
			MountainPlatformingLevelScale.ScaleState scaleState = this.scaleState;
			if (scaleState != MountainPlatformingLevelScale.ScaleState.leftDown)
			{
				if (scaleState != MountainPlatformingLevelScale.ScaleState.rightDown)
				{
					if (scaleState == MountainPlatformingLevelScale.ScaleState.still)
					{
						this.ScaleLeft.animator.SetBool(name, false);
						this.ScaleLeft.animator.SetBool(name2, false);
						this.ScaleRight.animator.SetBool(name, false);
						this.ScaleRight.animator.SetBool(name2, false);
						if (this.ScaleLeft.animator.GetCurrentAnimatorStateInfo(0).IsName("Scale_Idle"))
						{
							AudioManager.Stop("castle_scales_tip_up");
							AudioManager.Stop("castle_scales_tip_down");
						}
					}
				}
				else
				{
					this.ScaleLeft.animator.SetBool(name, false);
					this.ScaleLeft.animator.SetBool(name2, true);
					this.ScaleRight.animator.SetBool(name, true);
					this.ScaleRight.animator.SetBool(name2, false);
					this.ScalesUpSound();
					this.ScalesDownSound();
				}
			}
			else
			{
				this.ScaleLeft.animator.SetBool(name, true);
				this.ScaleLeft.animator.SetBool(name2, false);
				this.ScaleRight.animator.SetBool(name, false);
				this.ScaleRight.animator.SetBool(name2, true);
				this.ScalesUpSound();
				this.ScalesDownSound();
			}
		}
	}

	// Token: 0x060035A7 RID: 13735 RVA: 0x001F4262 File Offset: 0x001F2662
	private void ScalesUpSound()
	{
		if (!AudioManager.CheckIfPlaying("castle_scales_tip_up"))
		{
			AudioManager.Play("castle_scales_tip_up");
			this.emitAudioFromObject.Add("castle_scales_tip_up");
		}
	}

	// Token: 0x060035A8 RID: 13736 RVA: 0x001F428D File Offset: 0x001F268D
	private void ScalesDownSound()
	{
		if (!AudioManager.CheckIfPlaying("castle_scales_tip_down"))
		{
			AudioManager.Play("castle_scales_tip_down");
			this.emitAudioFromObject.Add("castle_scales_tip_down");
		}
	}

	// Token: 0x04003DB8 RID: 15800
	[SerializeField]
	private float scaleSpeed;

	// Token: 0x04003DB9 RID: 15801
	[SerializeField]
	private float scaleChangeAmount;

	// Token: 0x04003DBA RID: 15802
	[SerializeField]
	private MountainPlatformingLevelScalePart ScaleLeft;

	// Token: 0x04003DBB RID: 15803
	[SerializeField]
	private MountainPlatformingLevelScalePart ScaleRight;

	// Token: 0x04003DBC RID: 15804
	private Vector2 scaleLeftStart;

	// Token: 0x04003DBD RID: 15805
	private Vector2 scaleRightStart;

	// Token: 0x04003DBE RID: 15806
	public MountainPlatformingLevelScale.ScaleState scaleState;

	// Token: 0x020008F0 RID: 2288
	public enum ScaleState
	{
		// Token: 0x04003DC0 RID: 15808
		rightDown,
		// Token: 0x04003DC1 RID: 15809
		leftDown,
		// Token: 0x04003DC2 RID: 15810
		still
	}
}
