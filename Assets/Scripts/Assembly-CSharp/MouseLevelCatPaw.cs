using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006E8 RID: 1768
public class MouseLevelCatPaw : AbstractCollidableObject
{
	// Token: 0x170003C3 RID: 963
	// (get) Token: 0x060025DC RID: 9692 RVA: 0x00162DB9 File Offset: 0x001611B9
	// (set) Token: 0x060025DD RID: 9693 RVA: 0x00162DC1 File Offset: 0x001611C1
	public MouseLevelCatPaw.State state { get; private set; }

	// Token: 0x060025DE RID: 9694 RVA: 0x00162DCA File Offset: 0x001611CA
	protected override void Awake()
	{
		base.Awake();
		this.initialPos = base.transform.localPosition;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060025DF RID: 9695 RVA: 0x00162DF3 File Offset: 0x001611F3
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x060025E0 RID: 9696 RVA: 0x00162E0B File Offset: 0x0016120B
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060025E1 RID: 9697 RVA: 0x00162E29 File Offset: 0x00161229
	public void Attack(LevelProperties.Mouse.Claw properties)
	{
		this.properties = properties;
		if (this.state == MouseLevelCatPaw.State.Idle)
		{
			this.state = MouseLevelCatPaw.State.Attack;
			base.StartCoroutine(this.attack_cr());
		}
	}

	// Token: 0x060025E2 RID: 9698 RVA: 0x00162E54 File Offset: 0x00161254
	private IEnumerator attack_cr()
	{
		float totalMoveTime = 0.584f;
		float startX = this.initialPos.x;
		float endX = this.initialPos.x + totalMoveTime * this.properties.moveSpeed;
		int hitAnim = Animator.StringToHash(base.animator.GetLayerName(0) + ".Attack_Hit");
		float previousAnimationsTime = 0f;
		for (int i = 0; i < 3; i++)
		{
			base.animator.SetTrigger("Attack");
			float animationTime = (i != 0) ? 0.167f : 0.25f;
			bool hitGround = false;
			while (!hitGround)
			{
				yield return new WaitForEndOfFrame();
				AnimatorStateInfo animState = base.animator.GetCurrentAnimatorStateInfo(0);
				if (animState.fullPathHash == hitAnim)
				{
					hitGround = true;
					previousAnimationsTime += animationTime;
					float moveProgress = previousAnimationsTime / totalMoveTime;
					base.transform.SetLocalPosition(new float?(Mathf.Lerp(startX, endX, moveProgress)), null, null);
					CupheadLevelCamera.Current.Shake(15f, 1f, false);
					yield return CupheadTime.WaitForSeconds(this, this.properties.holdGroundTime);
				}
				else
				{
					float num = animState.normalizedTime * animationTime;
					float t2 = (previousAnimationsTime + num) / totalMoveTime;
					base.transform.SetLocalPosition(new float?(Mathf.Lerp(startX, endX, t2)), null, null);
				}
			}
		}
		base.animator.SetTrigger("Leave");
		base.StartCoroutine(this.timedAudioCatMeow_cr());
		float moveStartX = base.transform.localPosition.x;
		float moveEndX = this.initialPos.x;
		float leaveTime = Mathf.Abs(moveEndX - moveStartX) / this.properties.leaveSpeed;
		float t = 0f;
		while (t < leaveTime)
		{
			t += CupheadTime.Delta;
			base.transform.SetLocalPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInSine, moveStartX, moveEndX, t / leaveTime)), null, null);
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(moveEndX), null, null);
		this.state = MouseLevelCatPaw.State.Idle;
		yield return null;
		yield break;
	}

	// Token: 0x060025E3 RID: 9699 RVA: 0x00162E70 File Offset: 0x00161270
	private IEnumerator timedAudioCatMeow_cr()
	{
		yield return new WaitForSeconds(1f);
		AudioManager.Play("level_mouse_cat_claw_end");
		yield break;
	}

	// Token: 0x060025E4 RID: 9700 RVA: 0x00162E84 File Offset: 0x00161284
	private void SoundCatPawAttack()
	{
		AudioManager.Play("level_mouse_cat_paw_attack");
		this.emitAudioFromObject.Add("level_mouse_cat_paw_attack");
	}

	// Token: 0x060025E5 RID: 9701 RVA: 0x00162EA0 File Offset: 0x001612A0
	private void SoundCatMeowVoice()
	{
		AudioManager.Play("level_mouse_cat_meow_voice");
		this.emitAudioFromObject.Add("level_mouse_cat_meow_voice");
	}

	// Token: 0x04002E69 RID: 11881
	private LevelProperties.Mouse.Claw properties;

	// Token: 0x04002E6A RID: 11882
	private Vector2 initialPos;

	// Token: 0x04002E6B RID: 11883
	private DamageDealer damageDealer;

	// Token: 0x020006E9 RID: 1769
	public enum State
	{
		// Token: 0x04002E6D RID: 11885
		Idle,
		// Token: 0x04002E6E RID: 11886
		Attack
	}
}
