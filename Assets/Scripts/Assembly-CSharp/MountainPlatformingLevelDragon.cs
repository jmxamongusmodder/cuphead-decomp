using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008DE RID: 2270
public class MountainPlatformingLevelDragon : PlatformingLevelShootingEnemy
{
	// Token: 0x0600351D RID: 13597 RVA: 0x001EEA04 File Offset: 0x001ECE04
	protected override void Start()
	{
		base.Start();
		AudioManager.Play("castle_dragon_spawn");
		this.emitAudioFromObject.Add("castle_dragon_spawn");
	}

	// Token: 0x0600351E RID: 13598 RVA: 0x001EEA26 File Offset: 0x001ECE26
	public void Init(Vector3 startPos, Vector3 endPos)
	{
		this.startPos = startPos;
		base.transform.position = startPos;
		this.endPos = endPos;
		base.StartCoroutine(this.move_to_pos_cr());
		base.StartCoroutine(this.check_cr());
	}

	// Token: 0x0600351F RID: 13599 RVA: 0x001EEA5C File Offset: 0x001ECE5C
	private IEnumerator move_to_pos_cr()
	{
		float t = 0f;
		float time = base.Properties.dragonTimeIn;
		Vector2 start = base.transform.position;
		this._target = PlayerManager.GetNext();
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.endPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.StartShoot();
		yield return null;
		yield break;
	}

	// Token: 0x06003520 RID: 13600 RVA: 0x001EEA78 File Offset: 0x001ECE78
	private IEnumerator check_cr()
	{
		while (MountainPlatformingLevelElevatorHandler.elevatorIsMoving)
		{
			yield return null;
		}
		this.Die();
		yield break;
	}

	// Token: 0x06003521 RID: 13601 RVA: 0x001EEA93 File Offset: 0x001ECE93
	protected override void Shoot()
	{
		base.Shoot();
		AudioManager.Play("castle_dragon_attack");
		this.emitAudioFromObject.Add("castle_dragon_attack");
		base.StartCoroutine(this.leave_cr());
	}

	// Token: 0x06003522 RID: 13602 RVA: 0x001EEAC4 File Offset: 0x001ECEC4
	protected override void SpawnShootEffect()
	{
		if (base.transform.localScale.x < 0f)
		{
			this._effectRoot.localEulerAngles = new Vector3(0f, 0f, this._effectRoot.localEulerAngles.z - 180f);
		}
		if (this._shootEffect != null)
		{
			Effect effect = this._shootEffect.Create(this._effectRoot.position);
			effect.transform.rotation = this._effectRoot.rotation;
		}
	}

	// Token: 0x06003523 RID: 13603 RVA: 0x001EEB60 File Offset: 0x001ECF60
	private IEnumerator leave_cr()
	{
		float t = 0f;
		float time = base.Properties.dragonTimeOut;
		base.transform.position = this.endPos;
		Vector2 start = base.transform.position;
		yield return CupheadTime.WaitForSeconds(this, base.Properties.dragonLeaveDelay);
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, this.startPos, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.Die();
		yield return null;
		yield break;
	}

	// Token: 0x06003524 RID: 13604 RVA: 0x001EEB7B File Offset: 0x001ECF7B
	protected override void Die()
	{
		AudioManager.Play("castle_dragon_death");
		this.emitAudioFromObject.Add("castle_dragon_death");
		base.Die();
	}

	// Token: 0x06003525 RID: 13605 RVA: 0x001EEB9D File Offset: 0x001ECF9D
	private void ActivateTail()
	{
		base.animator.Play("Tail", 1);
	}

	// Token: 0x04003D4C RID: 15692
	private Vector3 endPos;

	// Token: 0x04003D4D RID: 15693
	private Vector3 startPos;
}
