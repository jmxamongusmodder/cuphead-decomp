using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008D8 RID: 2264
public class MountainPlatformingLevelCyclops : PlatformingLevelAutoscrollObject
{
	// Token: 0x060034EF RID: 13551 RVA: 0x001EC852 File Offset: 0x001EAC52
	protected override void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.checkToLock = true;
		this.lockDistance = -1300f;
		this.IsEnabled(false);
		base.StartCoroutine(this.start_scrolling_cr());
		base.Start();
	}

	// Token: 0x060034F0 RID: 13552 RVA: 0x001EC88B File Offset: 0x001EAC8B
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060034F1 RID: 13553 RVA: 0x001EC8A9 File Offset: 0x001EACA9
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060034F2 RID: 13554 RVA: 0x001EC8C7 File Offset: 0x001EACC7
	private void IsEnabled(bool isenabled)
	{
		base.GetComponent<Collider2D>().enabled = isenabled;
		base.GetComponent<SpriteRenderer>().enabled = isenabled;
	}

	// Token: 0x060034F3 RID: 13555 RVA: 0x001EC8E1 File Offset: 0x001EACE1
	protected override void StartAutoscroll()
	{
		base.StartAutoscroll();
		CupheadLevelCamera.Current.OffsetCamera(true, false);
	}

	// Token: 0x060034F4 RID: 13556 RVA: 0x001EC8F8 File Offset: 0x001EACF8
	private IEnumerator start_scrolling_cr()
	{
		while (!this.isLocked)
		{
			yield return null;
		}
		this.cyclopsMoving = true;
		base.StartCoroutine(this.start_moving_cr());
		this.IsEnabled(true);
		PlatformingLevel level = (PlatformingLevel)Level.Current;
		level.useAltQuote = true;
		while (base.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 650f)
		{
			yield return null;
		}
		this.autoscrollMoving = true;
		base.StartCoroutine(this.check_to_move_forward_cr());
		this.StartAutoscroll();
		yield break;
	}

	// Token: 0x060034F5 RID: 13557 RVA: 0x001EC914 File Offset: 0x001EAD14
	private IEnumerator start_moving_cr()
	{
		base.animator.Play("Run");
		while (this.cyclopsMoving)
		{
			base.transform.position += Vector3.right * (200f * this.autoScrollMultiplier) * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034F6 RID: 13558 RVA: 0x001EC930 File Offset: 0x001EAD30
	private IEnumerator check_to_move_forward_cr()
	{
		while (this.autoscrollMoving)
		{
			float dist = PlayerManager.Center.x - base.transform.position.x;
			if (base.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 1600f)
			{
				base.transform.position = new Vector3(CupheadLevelCamera.Current.transform.position.x - 1600f, base.transform.position.y);
			}
			if (dist > 1300f)
			{
				if (CupheadLevelCamera.Current.autoScrolling)
				{
					CupheadLevelCamera.Current.SetAutoScroll(false);
				}
			}
			else if (!CupheadLevelCamera.Current.autoScrolling)
			{
				CupheadLevelCamera.Current.SetAutoScroll(true);
				CupheadLevelCamera.Current.SetAutoscrollSpeedMultiplier(this.autoScrollMultiplier);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x060034F7 RID: 13559 RVA: 0x001EC94B File Offset: 0x001EAD4B
	protected override void StartEndingAutoscroll()
	{
		base.StartEndingAutoscroll();
		this.autoscrollMoving = false;
	}

	// Token: 0x060034F8 RID: 13560 RVA: 0x001EC95A File Offset: 0x001EAD5A
	protected override void EndAutoscroll()
	{
		base.EndAutoscroll();
		base.StartCoroutine(this.fall_cr());
	}

	// Token: 0x060034F9 RID: 13561 RVA: 0x001EC970 File Offset: 0x001EAD70
	private IEnumerator fall_cr()
	{
		while (base.transform.position.x < CupheadLevelCamera.Current.transform.position.x - 1300f)
		{
			yield return null;
		}
		CupheadLevelCamera.Current.LockCamera(true);
		this.cyclopsMoving = false;
		base.GetComponent<SpriteRenderer>().sortingLayerName = SpriteLayer.Map.ToString();
		base.animator.SetTrigger("OnFall");
		yield return base.animator.WaitForAnimationToEnd(this, "Fall", false, true);
		CupheadLevelCamera.Current.LockCamera(false);
		CupheadLevelCamera.Current.OffsetCamera(false, false);
		yield return CupheadTime.WaitForSeconds(this, 0.5f);
		UnityEngine.Object.Destroy(base.gameObject);
		yield return null;
		yield break;
	}

	// Token: 0x060034FA RID: 13562 RVA: 0x001EC98B File Offset: 0x001EAD8B
	public void CameraShake()
	{
		CupheadLevelCamera.Current.Shake(10f, 0.5f, false);
	}

	// Token: 0x060034FB RID: 13563 RVA: 0x001EC9A2 File Offset: 0x001EADA2
	private void SoundCyclopsFall()
	{
		AudioManager.Play("castle_giant_rock_chase_death");
	}

	// Token: 0x060034FC RID: 13564 RVA: 0x001EC9AE File Offset: 0x001EADAE
	private void SoundCyclopsFootstep()
	{
		AudioManager.Play("castle_giant_rock_chase_footstep");
		this.emitAudioFromObject.Add("castle_giant_rock_chase_footstep");
	}

	// Token: 0x04003D20 RID: 15648
	[SerializeField]
	private float autoScrollMultiplier;

	// Token: 0x04003D21 RID: 15649
	private const float MAX_AUTOSCROLL_DIST = 1300f;

	// Token: 0x04003D22 RID: 15650
	private const float MAX_CYCLOPS_DIST = 1600f;

	// Token: 0x04003D23 RID: 15651
	private bool autoscrollMoving;

	// Token: 0x04003D24 RID: 15652
	private bool cyclopsMoving;

	// Token: 0x04003D25 RID: 15653
	private DamageDealer damageDealer;
}
