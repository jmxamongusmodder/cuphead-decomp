using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008DC RID: 2268
public class MountainPlatformingLevelCyclopsRocks : AbstractPausableComponent
{
	// Token: 0x06003514 RID: 13588 RVA: 0x001EDA57 File Offset: 0x001EBE57
	private void Start()
	{
		this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.UnSpawned;
		base.StartCoroutine(this.start_trigger_cr());
		this.cyclopsAnimator = this.cyclopsBG.GetComponent<Animator>();
	}

	// Token: 0x06003515 RID: 13589 RVA: 0x001EDA80 File Offset: 0x001EBE80
	private IEnumerator start_trigger_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.player = PlayerManager.GetNext();
		while (this.player.transform.position.x < this.onTrigger.transform.position.x)
		{
			yield return null;
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
		}
		this.StartCyclops();
		while (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead)
		{
			if (this.player == null || this.player.IsDead)
			{
				this.player = PlayerManager.GetNext();
			}
			this.playerInTrigger = (this.player.transform.position.x > this.onTrigger.transform.position.x);
			if (this.player.transform.position.x > this.offTrigger.transform.position.x)
			{
				this.cyclopsBG.isDead = true;
				this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead;
				break;
			}
			yield return null;
		}
		yield return null;
		yield break;
	}

	// Token: 0x06003516 RID: 13590 RVA: 0x001EDA9C File Offset: 0x001EBE9C
	private void StartCyclops()
	{
		this.IsIdle = true;
		this.playerInTrigger = true;
		this.cyclopsBG.start = this.cyclopsBG.transform.position;
		this.cyclopsAnimator.SetTrigger("StartCyclops");
		this.cyclopsAnimator.SetBool("isIdle", this.IsIdle);
		this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned;
		base.StartCoroutine(this.walk_and_idle_cr());
		base.StartCoroutine(this.attack_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06003517 RID: 13591 RVA: 0x001EDB28 File Offset: 0x001EBF28
	private IEnumerator turn_cyclops_cr()
	{
		if (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning)
		{
			string ani = this.IsIdle ? "Turn" : "Turn_To_Walk";
			this.cyclopsAnimator.SetTrigger("OnTurn");
			this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning;
			yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, ani, false, true);
			this.facingLeft = (this.cyclopsBG.transform.localScale.x == 1f);
			this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned;
		}
		yield break;
	}

	// Token: 0x06003518 RID: 13592 RVA: 0x001EDB44 File Offset: 0x001EBF44
	private IEnumerator walk_and_idle_cr()
	{
		this.facingLeft = true;
		float t = 0f;
		float timer = 1f;
		while (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead)
		{
			if (this.cyclopsState == MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned)
			{
				if (this.IsIdle)
				{
					if (this.player.transform.position.x < this.cyclopsBG.transform.position.x + this.cyclopsStopOffset && this.player.transform.position.x > this.cyclopsBG.transform.position.x - this.cyclopsStopOffset)
					{
						if (this.player.transform.position.x < this.cyclopsBG.transform.position.x && !this.facingLeft)
						{
							yield return base.StartCoroutine(this.turn_cyclops_cr());
						}
						else if (this.player.transform.position.x > this.cyclopsBG.transform.position.x && this.facingLeft)
						{
							yield return base.StartCoroutine(this.turn_cyclops_cr());
						}
					}
					else
					{
						this.IsIdle = false;
						this.cyclopsAnimator.SetBool("isIdle", this.IsIdle);
						yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Idle_To_Walk", false, true);
					}
				}
				else if (this.player.transform.position.x < this.cyclopsBG.transform.position.x - this.cyclopsStopOffset && !this.facingLeft)
				{
					yield return base.StartCoroutine(this.turn_cyclops_cr());
				}
				else if (this.player.transform.position.x > this.cyclopsBG.transform.position.x + this.cyclopsStopOffset && this.facingLeft)
				{
					yield return base.StartCoroutine(this.turn_cyclops_cr());
				}
				else if (t < timer)
				{
					t += CupheadTime.Delta;
				}
				else
				{
					this.IsIdle = true;
					this.cyclopsAnimator.SetBool("isIdle", this.IsIdle);
					yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Walk_To_Idle", false, true);
					t = 0f;
				}
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003519 RID: 13593 RVA: 0x001EDB60 File Offset: 0x001EBF60
	private IEnumerator move_cr()
	{
		while (this.cyclopsBG != null)
		{
			if (this.cyclopsBG.isWalking)
			{
				this.cyclopsBG.transform.AddPosition(((!this.facingLeft) ? this.walkSpeed : (-this.walkSpeed)) * CupheadTime.Delta, 0f, 0f);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600351A RID: 13594 RVA: 0x001EDB7C File Offset: 0x001EBF7C
	private IEnumerator attack_cr()
	{
		while (this.cyclopsState != MountainPlatformingLevelCyclopsRocks.CyclopsState.Dead)
		{
			yield return CupheadTime.WaitForSeconds(this, this.attackDelayRange.RandomFloat());
			while (!this.playerInTrigger || this.cyclopsState == MountainPlatformingLevelCyclopsRocks.CyclopsState.Turning)
			{
				yield return null;
			}
			this.cyclopsBG.GetPlayer(this.player);
			this.cyclopsAnimator.SetTrigger("OnAttack");
			yield return this.cyclopsAnimator.WaitForAnimationToStart(this, "Attack_Start", false);
			this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Attacking;
			if (this.IsIdle)
			{
				yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Attack_To_Idle", false, true);
			}
			else
			{
				yield return this.cyclopsAnimator.WaitForAnimationToEnd(this, "Attack_To_Walk", false, true);
			}
			if (this.player.transform.position.x < this.cyclopsBG.transform.position.x && !this.facingLeft)
			{
				yield return base.StartCoroutine(this.turn_cyclops_cr());
			}
			else if (this.player.transform.position.x > this.cyclopsBG.transform.position.x && this.facingLeft)
			{
				yield return base.StartCoroutine(this.turn_cyclops_cr());
			}
			else
			{
				this.cyclopsState = MountainPlatformingLevelCyclopsRocks.CyclopsState.Spawned;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.1f);
		}
		this.cyclopsAnimator.SetTrigger("OnAttack");
		yield return null;
		yield break;
	}

	// Token: 0x0600351B RID: 13595 RVA: 0x001EDB98 File Offset: 0x001EBF98
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.offTrigger.transform.position, new Vector3(this.offTrigger.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.onTrigger.transform.position, new Vector3(this.onTrigger.transform.position.x, 5000f, 0f));
		Gizmos.color = new Color(1f, 0f, 1f, 1f);
		if (this.cyclopsBG)
		{
			Gizmos.DrawLine(new Vector3(this.cyclopsBG.transform.position.x + this.cyclopsStopOffset, this.cyclopsBG.transform.position.y), new Vector3(this.cyclopsBG.transform.position.x - this.cyclopsStopOffset, this.cyclopsBG.transform.position.y));
		}
	}

	// Token: 0x04003D3A RID: 15674
	[SerializeField]
	private float walkSpeed;

	// Token: 0x04003D3B RID: 15675
	[SerializeField]
	private MinMax attackDelayRange;

	// Token: 0x04003D3C RID: 15676
	[SerializeField]
	private Transform onTrigger;

	// Token: 0x04003D3D RID: 15677
	[SerializeField]
	private Transform offTrigger;

	// Token: 0x04003D3E RID: 15678
	[SerializeField]
	private MountainPlatformingLevelCyclopsBG cyclopsBG;

	// Token: 0x04003D3F RID: 15679
	[SerializeField]
	private float cyclopsStopOffset;

	// Token: 0x04003D40 RID: 15680
	private AbstractPlayerController player;

	// Token: 0x04003D41 RID: 15681
	private MountainPlatformingLevelCyclopsRocks.CyclopsState cyclopsState;

	// Token: 0x04003D42 RID: 15682
	private bool IsIdle;

	// Token: 0x04003D43 RID: 15683
	private bool playerInTrigger;

	// Token: 0x04003D44 RID: 15684
	private bool facingLeft;

	// Token: 0x04003D45 RID: 15685
	private Animator cyclopsAnimator;

	// Token: 0x020008DD RID: 2269
	private enum CyclopsState
	{
		// Token: 0x04003D47 RID: 15687
		UnSpawned,
		// Token: 0x04003D48 RID: 15688
		Spawned,
		// Token: 0x04003D49 RID: 15689
		Turning,
		// Token: 0x04003D4A RID: 15690
		Attacking,
		// Token: 0x04003D4B RID: 15691
		Dead
	}
}
