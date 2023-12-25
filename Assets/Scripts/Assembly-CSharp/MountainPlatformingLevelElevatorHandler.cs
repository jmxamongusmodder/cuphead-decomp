using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E1 RID: 2273
public class MountainPlatformingLevelElevatorHandler : AbstractPausableComponent
{
	// Token: 0x17000450 RID: 1104
	// (get) Token: 0x06003532 RID: 13618 RVA: 0x001EF50F File Offset: 0x001ED90F
	// (set) Token: 0x06003533 RID: 13619 RVA: 0x001EF516 File Offset: 0x001ED916
	public static bool elevatorIsMoving { get; private set; }

	// Token: 0x06003534 RID: 13620 RVA: 0x001EF520 File Offset: 0x001ED920
	private void Start()
	{
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = false;
		base.StartCoroutine(this.wait_cr());
		this.cameraLockRoutine = base.StartCoroutine(this.lock_camera_cr());
		this.invisibleWall.SetActive(false);
		foreach (PlatformingLevelParallax platformingLevelParallax in this.bottomBackground.GetComponentsInChildren<PlatformingLevelParallax>())
		{
			platformingLevelParallax.enabled = false;
		}
	}

	// Token: 0x06003535 RID: 13621 RVA: 0x001EF58C File Offset: 0x001ED98C
	private IEnumerator lock_camera_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		for (;;)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			CupheadLevelCamera.Current.LockCamera(CupheadLevelCamera.Current.transform.position.x > this.triggerPoint.transform.position.x);
			if (player2 != null)
			{
				if (!player2.IsDead && !player.IsDead)
				{
					if (player.transform.position.x < this.triggerPoint.transform.position.x && player2.transform.position.x < this.triggerPoint.transform.position.x)
					{
						CupheadLevelCamera.Current.LockCamera(false);
					}
				}
				else if (player2.IsDead)
				{
					if (player.transform.position.x < this.triggerPoint.transform.position.x)
					{
						CupheadLevelCamera.Current.LockCamera(false);
					}
				}
				else if (player.IsDead && player2.transform.position.x < this.triggerPoint.transform.position.x)
				{
					CupheadLevelCamera.Current.LockCamera(false);
				}
			}
			else if (player.transform.position.x < this.triggerPoint.transform.position.x)
			{
				CupheadLevelCamera.Current.LockCamera(false);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003536 RID: 13622 RVA: 0x001EF5A8 File Offset: 0x001ED9A8
	private IEnumerator wait_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 1f);
		AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
		AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
		for (;;)
		{
			player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			if (player2 != null)
			{
				if (!player2.IsDead && !player.IsDead)
				{
					if (player.transform.position.x > this.triggerPoint.transform.position.x && player2.transform.position.x > this.triggerPoint2.transform.position.x)
					{
						break;
					}
					if (player.transform.position.x > this.triggerPoint2.transform.position.x && player2.transform.position.x > this.triggerPoint.transform.position.x)
					{
						break;
					}
				}
				else if (player2.IsDead)
				{
					if (player.transform.position.x > this.triggerPoint.transform.position.x)
					{
						break;
					}
				}
				else if (player.IsDead && player2.transform.position.x > this.triggerPoint.transform.position.x)
				{
					break;
				}
			}
			else if (player.transform.position.x > this.triggerPoint.transform.position.x)
			{
				break;
			}
			yield return null;
		}
		base.StopCoroutine(this.cameraLockRoutine);
		CupheadLevelCamera.Current.LockCamera(true);
		this.invisibleWall.SetActive(true);
		AudioManager.Play("castle_lift_start");
		CupheadLevelCamera.Current.Shake(10f, 1f, false);
		yield return CupheadTime.WaitForSeconds(this, 0.9f);
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x06003537 RID: 13623 RVA: 0x001EF5C4 File Offset: 0x001ED9C4
	private IEnumerator move_cr()
	{
		YieldInstruction wait = new WaitForFixedUpdate();
		float t = 0f;
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = true;
		Vector3 startPos = this.scrollingObject.transform.position;
		Vector3 pos = this.scrollingObject.transform.position;
		Vector3 dir = this.pointA.transform.position - this.scrollingObject.transform.position;
		Vector3 middle = this.scrollingObject.transform.position + dir.normalized * (Vector3.Distance(this.scrollingObject.transform.position, this.pointA.transform.position) / 2f);
		foreach (ScrollingBackgroundElevator scrollingBackgroundElevator in this.midgroundSprites)
		{
			scrollingBackgroundElevator.SetUp(MathUtils.AngleToDirection(-45f), this.speed);
		}
		this.backgroundSprite.SetUp(MathUtils.AngleToDirection(-45f), this.speed - this.speed / 4f);
		this.foregroundSprite.SetUp(MathUtils.AngleToDirection(-45f), this.speed + this.speed / 4f);
		this.cloudSprite.SetUp(MathUtils.AngleToDirection(-45f), this.speed + this.speed / 4f);
		foreach (PlatformingLevelParallax platformingLevelParallax in this.topBackground.GetComponentsInChildren<PlatformingLevelParallax>())
		{
			platformingLevelParallax.enabled = false;
		}
		this.bottomBackground.parent = this.scrollingObject.transform;
		this.mudmanSpawner.SpawnMudmen();
		AudioManager.PlayLoop("castle_lift_loop");
		while (this.scrollingObject.transform.position.y < middle.y)
		{
			pos -= MathUtils.AngleToDirection(-45f) * this.speed * CupheadTime.FixedDelta;
			this.scrollingObject.transform.position = pos;
			yield return wait;
		}
		while (t < this.time)
		{
			t += CupheadTime.FixedDelta;
			yield return null;
		}
		Vector3 midPos = this.scrollingObject.transform.position;
		float endTime = Vector3.Distance(startPos, this.pointA.position) / this.speed;
		t = 0f;
		foreach (ScrollingBackgroundElevator scrollingBackgroundElevator2 in this.midgroundSprites)
		{
			scrollingBackgroundElevator2.EaseoutSpeed(endTime);
		}
		this.cloudSprite.EaseoutSpeed(endTime);
		this.foregroundSprite.EaseoutSpeed(endTime);
		this.backgroundSprite.EaseoutSpeed(endTime);
		base.StartCoroutine(this.easeTime(endTime));
		this.cloudSprite.ending = true;
		while (this.easeingTime < endTime)
		{
			pos -= MathUtils.AngleToDirection(-45f) * this.speed * CupheadTime.FixedDelta;
			this.scrollingObject.transform.position = pos;
			yield return wait;
		}
		AudioManager.Stop("castle_lift_loop");
		AudioManager.Play("castle_lift_end");
		CupheadLevelCamera.Current.Shake(10f, 0.5f, false);
		foreach (PlatformingLevelParallax platformingLevelParallax2 in this.bottomBackground.GetComponentsInChildren<PlatformingLevelParallax>())
		{
			platformingLevelParallax2.enabled = true;
			platformingLevelParallax2.UpdateBasePosition();
		}
		foreach (ScrollingBackgroundElevator scrollingBackgroundElevator3 in this.midgroundSprites)
		{
			scrollingBackgroundElevator3.ending = true;
		}
		this.backgroundSprite.ending = true;
		MountainPlatformingLevelElevatorHandler.elevatorIsMoving = false;
		if (PlayerManager.GetFirst().transform.position.x < CupheadLevelCamera.Current.transform.position.x)
		{
			CupheadLevelCamera.Current.OffsetCamera(true, true);
		}
		else
		{
			CupheadLevelCamera.Current.OffsetCamera(true, false);
		}
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		CupheadLevelCamera.Current.OffsetCamera(false, false);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		CupheadLevelCamera.Current.LockCamera(false);
		yield return null;
		yield break;
	}

	// Token: 0x06003538 RID: 13624 RVA: 0x001EF5E0 File Offset: 0x001ED9E0
	private IEnumerator easeTime(float time)
	{
		float startSpeed = this.speed;
		this.easeingTime = 0f;
		while (this.easeingTime < time)
		{
			this.easeingTime += CupheadTime.Delta;
			this.speed = Mathf.Lerp(startSpeed, 0f, this.easeingTime / time);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003539 RID: 13625 RVA: 0x001EF604 File Offset: 0x001EDA04
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.triggerPoint.transform.position, new Vector3(this.triggerPoint.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.triggerPoint2.transform.position, new Vector3(this.triggerPoint2.transform.position.x, 5000f, 0f));
		Gizmos.color = new Color(0f, 1f, 0f, 1f);
		Gizmos.DrawWireSphere(this.pointA.transform.position, 100f);
		Gizmos.DrawLine(this.pointA.transform.position, this.scrollingObject.transform.position);
	}

	// Token: 0x04003D5B RID: 15707
	[SerializeField]
	private MountainPlatformingLevelMudmanSpawner mudmanSpawner;

	// Token: 0x04003D5C RID: 15708
	[SerializeField]
	private Transform topBackground;

	// Token: 0x04003D5D RID: 15709
	[SerializeField]
	private Transform bottomBackground;

	// Token: 0x04003D5E RID: 15710
	[SerializeField]
	private float speed;

	// Token: 0x04003D5F RID: 15711
	[SerializeField]
	private GameObject scrollingObject;

	// Token: 0x04003D60 RID: 15712
	[SerializeField]
	private Transform triggerPoint;

	// Token: 0x04003D61 RID: 15713
	[SerializeField]
	private Transform triggerPoint2;

	// Token: 0x04003D62 RID: 15714
	[SerializeField]
	private Transform pointA;

	// Token: 0x04003D63 RID: 15715
	[SerializeField]
	private GameObject invisibleWall;

	// Token: 0x04003D64 RID: 15716
	[SerializeField]
	private ScrollingBackgroundElevator cloudSprite;

	// Token: 0x04003D65 RID: 15717
	[SerializeField]
	private ScrollingBackgroundElevator foregroundSprite;

	// Token: 0x04003D66 RID: 15718
	[SerializeField]
	private ScrollingBackgroundElevator backgroundSprite;

	// Token: 0x04003D67 RID: 15719
	[SerializeField]
	private ScrollingBackgroundElevator[] midgroundSprites;

	// Token: 0x04003D68 RID: 15720
	[SerializeField]
	private float time;

	// Token: 0x04003D69 RID: 15721
	private float easeingTime;

	// Token: 0x04003D6A RID: 15722
	private Coroutine cameraLockRoutine;
}
