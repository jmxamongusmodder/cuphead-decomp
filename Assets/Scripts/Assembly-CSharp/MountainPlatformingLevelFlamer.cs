using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008E4 RID: 2276
public class MountainPlatformingLevelFlamer : AbstractPlatformingLevelEnemy
{
	// Token: 0x0600354A RID: 13642 RVA: 0x001F0BEB File Offset: 0x001EEFEB
	protected override void OnStart()
	{
		this.isDead = false;
		this.angle = 3.1415927f;
		base.transform.position = this.startPos;
		base.StartCoroutine(this.check_dist_cr());
	}

	// Token: 0x0600354B RID: 13643 RVA: 0x001F0C20 File Offset: 0x001EF020
	protected override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
		this.pivotPoint = new GameObject("pivotPoint");
		this.pivotPoint.transform.position = new Vector3(base.transform.position.x, base.transform.position.y + 200f);
		this.angle = 3.1415927f;
		base.StartCoroutine(this.check_dist_cr());
	}

	// Token: 0x0600354C RID: 13644 RVA: 0x001F0CB0 File Offset: 0x001EF0B0
	private void PathMovement()
	{
		this.angle += this.speed * CupheadTime.FixedDelta;
		Vector3 a = new Vector3(-Mathf.Sin(this.angle) * this.loopSize, 0f, 0f);
		Vector3 b = new Vector3(0f, Mathf.Cos(this.angle) * this.loopSize, 0f);
		base.transform.position = this.pivotPoint.transform.position;
		base.transform.position += a + b;
	}

	// Token: 0x0600354D RID: 13645 RVA: 0x001F0D55 File Offset: 0x001EF155
	private void MovePivot()
	{
		this.pivotPoint.transform.AddPosition(this.moveSpeed * CupheadTime.FixedDelta, 0f, 0f);
	}

	// Token: 0x0600354E RID: 13646 RVA: 0x001F0D80 File Offset: 0x001EF180
	private IEnumerator check_dist_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.player = PlayerManager.GetNext();
		bool movingLeft = base.transform.position.x > this.player.transform.position.x;
		this.moveSpeed = ((!movingLeft) ? base.Properties.flamerXSpeed.RandomFloat() : (-base.Properties.flamerXSpeed.RandomFloat()));
		AudioManager.PlayLoop("castle_flamer_loop");
		this.emitAudioFromObject.Add("castle_flamer_loop");
		base.animator.SetTrigger("OnFlame");
		yield return base.animator.WaitForAnimationToEnd(this, "Flame_Appear", false, true);
		base.GetComponent<Collider2D>().enabled = true;
		base.StartCoroutine(this.move_cr());
		yield break;
	}

	// Token: 0x0600354F RID: 13647 RVA: 0x001F0D9C File Offset: 0x001EF19C
	private IEnumerator move_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.startDelayRange.RandomFloat());
		YieldInstruction wait = new WaitForFixedUpdate();
		base.StartCoroutine(this.accelerate_speed_cr());
		while (!this.isDead)
		{
			this.PathMovement();
			this.MovePivot();
			yield return wait;
		}
		yield break;
	}

	// Token: 0x06003550 RID: 13648 RVA: 0x001F0DB8 File Offset: 0x001EF1B8
	private IEnumerator accelerate_speed_cr()
	{
		float incrementBy = 1f;
		while (this.speed < base.Properties.flamerCirSpeed && !this.isDead)
		{
			this.speed += incrementBy * CupheadTime.Delta;
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003551 RID: 13649 RVA: 0x001F0DD3 File Offset: 0x001EF1D3
	private void PlayFace()
	{
		base.animator.Play("Flame_Face", 1);
	}

	// Token: 0x06003552 RID: 13650 RVA: 0x001F0DE6 File Offset: 0x001EF1E6
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.DrawWireSphere(base.transform.position, 100f);
	}

	// Token: 0x06003553 RID: 13651 RVA: 0x001F0E03 File Offset: 0x001EF203
	protected override void Die()
	{
		this.Deactivate();
	}

	// Token: 0x06003554 RID: 13652 RVA: 0x001F0E0C File Offset: 0x001EF20C
	private void Deactivate()
	{
		this.isDead = true;
		AudioManager.Stop("castle_flamer_loop");
		this.speed = 0f;
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Flame_Appear_Loop", 0);
		base.animator.Play("Off", 1);
		base.StartCoroutine(this.activate_cr());
	}

	// Token: 0x06003555 RID: 13653 RVA: 0x001F0E70 File Offset: 0x001EF270
	private IEnumerator activate_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.respawnRange.RandomFloat());
		this.OnStart();
		yield break;
	}

	// Token: 0x04003D70 RID: 15728
	[SerializeField]
	private float loopSize;

	// Token: 0x04003D71 RID: 15729
	[SerializeField]
	private MinMax startDelayRange;

	// Token: 0x04003D72 RID: 15730
	[SerializeField]
	private MinMax respawnRange;

	// Token: 0x04003D73 RID: 15731
	private GameObject pivotPoint;

	// Token: 0x04003D74 RID: 15732
	private float angle;

	// Token: 0x04003D75 RID: 15733
	private float speed;

	// Token: 0x04003D76 RID: 15734
	private float moveSpeed;

	// Token: 0x04003D77 RID: 15735
	private AbstractPlayerController player;

	// Token: 0x04003D78 RID: 15736
	private Vector3 startPos;

	// Token: 0x04003D79 RID: 15737
	private bool isDead;
}
