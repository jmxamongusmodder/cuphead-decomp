using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200070E RID: 1806
public class OldManLevelSockPuppet : AbstractCollidableObject
{
	// Token: 0x06002708 RID: 9992 RVA: 0x0016DD7D File Offset: 0x0016C17D
	private void Start()
	{
		this.rootPosition = base.transform.position;
		this.startPosition = base.transform.position;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06002709 RID: 9993 RVA: 0x0016DDAC File Offset: 0x0016C1AC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.WORKAROUND_NullifyFields();
	}

	// Token: 0x0600270A RID: 9994 RVA: 0x0016DDBC File Offset: 0x0016C1BC
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		base.transform.position = this.rootPosition + Mathf.Sin(this.wobbleTimer * 3f) * this.wobbleX * Vector3.right + Mathf.Sin(this.wobbleTimer * 2f) * this.wobbleY * Vector3.up;
		this.wobbleTimer += CupheadTime.Delta;
	}

	// Token: 0x0600270B RID: 9995 RVA: 0x0016DE55 File Offset: 0x0016C255
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x0600270C RID: 9996 RVA: 0x0016DE73 File Offset: 0x0016C273
	private void AniEvent_IncCmonCount()
	{
		base.animator.SetInteger("CmonCount", base.animator.GetInteger("CmonCount") + 1);
	}

	// Token: 0x0600270D RID: 9997 RVA: 0x0016DE97 File Offset: 0x0016C297
	private void AniEvent_ResetCmonCount()
	{
		base.animator.SetInteger("CmonCount", 0);
	}

	// Token: 0x0600270E RID: 9998 RVA: 0x0016DEAA File Offset: 0x0016C2AA
	public Vector3 throwPosition()
	{
		return this.throwPos.position;
	}

	// Token: 0x0600270F RID: 9999 RVA: 0x0016DEB7 File Offset: 0x0016C2B7
	public Vector3 catchPosition()
	{
		return this.catchPos.position;
	}

	// Token: 0x06002710 RID: 10000 RVA: 0x0016DEC4 File Offset: 0x0016C2C4
	private float EaseOvershoot(float start, float end, float value, float overshoot)
	{
		float num = Mathf.Lerp(start, end, value);
		return num + Mathf.Sin(value * 3.1415927f) * ((end - start) * overshoot);
	}

	// Token: 0x06002711 RID: 10001 RVA: 0x0016DEF1 File Offset: 0x0016C2F1
	public void MoveToPos(float endYPos, float distanceToCover)
	{
		if (distanceToCover == 0f && !this.entering)
		{
			this.ready = true;
			return;
		}
		this.ready = false;
		base.StartCoroutine(this.move_to_pos_cr(endYPos, distanceToCover));
	}

	// Token: 0x06002712 RID: 10002 RVA: 0x0016DF27 File Offset: 0x0016C327
	private float InverseLerpUnclamped(float a, float b, float value)
	{
		return (value - a) / (b - a);
	}

	// Token: 0x06002713 RID: 10003 RVA: 0x0016DF30 File Offset: 0x0016C330
	private IEnumerator move_to_pos_cr(float endYPos, float distanceToCover)
	{
		float t = 0f;
		float startYPos = this.rootPosition.y;
		float time = (distanceToCover != 1f) ? this.moveTimeLong : this.moveTimeShort;
		if (this.entering)
		{
			time = 0.5f;
			if (this.isLeft)
			{
				this.SFX_OMM_P2_PuppetLeftRaiseUp();
				this.SFX_OMM_P2_PuppetLeftRaiseUpVocal();
			}
			else
			{
				this.SFX_OMM_P2_PuppetRightRaiseUp();
				this.SFX_OMM_P2_PuppetRightRaiseUpVocal();
			}
		}
		YieldInstruction wait = new WaitForFixedUpdate();
		bool startEndAnimation = false;
		bool movingUp = endYPos > this.rootPosition.y;
		string moveBool = (!movingUp) ? ((distanceToCover != 1f) ? "MovingDown" : "MovingDownShort") : "MovingUp";
		base.animator.SetBool(moveBool, true);
		string startAnimation = (!movingUp) ? ((distanceToCover != 1f) ? "Move_Down_Start" : "Move_Down_Short_Start") : "Move_Up_Start";
		if (!this.dead)
		{
			if (movingUp)
			{
				yield return base.animator.WaitForAnimationToStart(this, startAnimation, false);
			}
			else
			{
				yield return base.animator.WaitForAnimationToStart(this, startAnimation, false);
			}
		}
		WaitForFrameTimePersistent wait24fps = new WaitForFrameTimePersistent(0.041666668f, false);
		while (t < time)
		{
			t += CupheadTime.FixedDelta;
			this.rootPosition = new Vector3(this.startPosition.x + this.armBowingXModifier * Mathf.Sin(this.InverseLerpUnclamped(startYPos, endYPos, this.rootPosition.y) * 3.1415927f), this.EaseOvershoot(startYPos, endYPos, t / time, this.moveOvershoot), base.transform.position.z);
			if (t / time >= 0.35f && !startEndAnimation)
			{
				base.animator.SetBool(moveBool, false);
				startEndAnimation = true;
			}
			if (t / time >= 0.6f && this.entering)
			{
				this.entering = false;
				this.colliders = base.GetComponentsInChildren<Collider2D>();
				foreach (Collider2D collider2D in this.colliders)
				{
					collider2D.enabled = true;
				}
			}
			yield return wait24fps;
		}
		this.rootPosition.x = this.startPosition.x;
		this.armsHolding.GetComponent<SpriteRenderer>().sortingLayerName = "Enemies";
		this.arms.transform.GetChild(0).GetComponent<SpriteRenderer>().sortingOrder = 10;
		int target = Animator.StringToHash(base.animator.GetLayerName(0) + "." + ((!movingUp) ? ((distanceToCover != 1f) ? "Move_Down_End" : "Move_Down_Short_End") : "Move_Up_End"));
		while (base.animator.GetCurrentAnimatorStateInfo(0).fullPathHash == target)
		{
			yield return null;
		}
		base.animator.SetBool("TauntA", !base.animator.GetBool("TauntA"));
		this.ready = true;
		yield break;
	}

	// Token: 0x06002714 RID: 10004 RVA: 0x0016DF59 File Offset: 0x0016C359
	public void StopTaunt()
	{
		base.animator.SetInteger("CmonCount", 5);
	}

	// Token: 0x06002715 RID: 10005 RVA: 0x0016DF6C File Offset: 0x0016C36C
	private void AniEvent_Catch()
	{
		this.main.CatchBall();
	}

	// Token: 0x06002716 RID: 10006 RVA: 0x0016DF79 File Offset: 0x0016C379
	public void AnIEvent_HoldingBall()
	{
		this.arms.SetActive(false);
		this.armsHolding.SetActive(true);
	}

	// Token: 0x06002717 RID: 10007 RVA: 0x0016DF93 File Offset: 0x0016C393
	public void AnIEvent_NotHoldingBall()
	{
		this.arms.SetActive(true);
		this.armsHolding.SetActive(false);
	}

	// Token: 0x06002718 RID: 10008 RVA: 0x0016DFB0 File Offset: 0x0016C3B0
	public void Die()
	{
		this.dead = true;
		foreach (Collider2D collider2D in this.colliders)
		{
			collider2D.enabled = false;
		}
		base.animator.Play("Death");
		base.GetComponent<LevelBossDeathExploder>().StartExplosion();
		if (this.rootPosition.y > 200f)
		{
			this.StopAllCoroutines();
			base.StartCoroutine(this.move_to_pos_cr(180f, 1f));
		}
	}

	// Token: 0x06002719 RID: 10009 RVA: 0x0016E037 File Offset: 0x0016C437
	private void AnimationEvent_SFX_OMM_P2_PuppetRightCatch()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_catch");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_catch");
	}

	// Token: 0x0600271A RID: 10010 RVA: 0x0016E053 File Offset: 0x0016C453
	private void SFX_OMM_P2_PuppetRightRaiseUp()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_raiseup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_raiseup");
	}

	// Token: 0x0600271B RID: 10011 RVA: 0x0016E06F File Offset: 0x0016C46F
	private void SFX_OMM_P2_PuppetRightRaiseUpVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_raiseup_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_raiseup_vocal");
	}

	// Token: 0x0600271C RID: 10012 RVA: 0x0016E08B File Offset: 0x0016C48B
	private void AnimationEvent_SFX_OMM_P2_PuppetRightThrow()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_throw");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_throw");
	}

	// Token: 0x0600271D RID: 10013 RVA: 0x0016E0A7 File Offset: 0x0016C4A7
	private void AnimationEvent_SFX_OMM_P2_PuppetRightThrowVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_right_throw_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_right_throw_vocal");
	}

	// Token: 0x0600271E RID: 10014 RVA: 0x0016E0C3 File Offset: 0x0016C4C3
	private void AnimationEvent_SFX_OMM_P2_PuppetLeftCatch()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_catch");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_catch");
	}

	// Token: 0x0600271F RID: 10015 RVA: 0x0016E0DF File Offset: 0x0016C4DF
	private void SFX_OMM_P2_PuppetLeftRaiseUpVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_raiseup");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_raiseup");
	}

	// Token: 0x06002720 RID: 10016 RVA: 0x0016E0FB File Offset: 0x0016C4FB
	private void SFX_OMM_P2_PuppetLeftRaiseUp()
	{
		base.StartCoroutine(this.SFX_OMM_P2_PuppetLeftRaiseUpVocal_cr());
	}

	// Token: 0x06002721 RID: 10017 RVA: 0x0016E10C File Offset: 0x0016C50C
	private IEnumerator SFX_OMM_P2_PuppetLeftRaiseUpVocal_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, 0f);
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_raiseup_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_raiseup_vocal");
		yield break;
	}

	// Token: 0x06002722 RID: 10018 RVA: 0x0016E127 File Offset: 0x0016C527
	private void AnimationEvent_SFX_OMM_P2_PuppetLeftThrow()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_throw");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_throw");
	}

	// Token: 0x06002723 RID: 10019 RVA: 0x0016E143 File Offset: 0x0016C543
	private void AnimationEvent_SFX_OMM_P2_PuppetLeftThrowVocal()
	{
		AudioManager.Play("sfx_dlc_omm_p2_puppet_left_throw_vocal");
		this.emitAudioFromObject.Add("sfx_dlc_omm_p2_puppet_left_throw_vocal");
	}

	// Token: 0x06002724 RID: 10020 RVA: 0x0016E15F File Offset: 0x0016C55F
	private void WORKAROUND_NullifyFields()
	{
		this.arms = null;
		this.armsHolding = null;
		this.throwPos = null;
		this.catchPos = null;
		this.damageDealer = null;
		this.main = null;
		this.colliders = null;
	}

	// Token: 0x04002FBD RID: 12221
	private const string MOVING_UP = "MovingUp";

	// Token: 0x04002FBE RID: 12222
	private const string MOVING_DOWN = "MovingDown";

	// Token: 0x04002FBF RID: 12223
	private const string MOVING_DOWN_SHORT = "MovingDownShort";

	// Token: 0x04002FC0 RID: 12224
	private const string MOVE_UP_START = "Move_Up_Start";

	// Token: 0x04002FC1 RID: 12225
	private const string MOVE_DOWN_START = "Move_Down_Start";

	// Token: 0x04002FC2 RID: 12226
	private const string MOVE_DOWN_SHORT_START = "Move_Down_Short_Start";

	// Token: 0x04002FC3 RID: 12227
	private const string MOVE_UP_END = "Move_Up_End";

	// Token: 0x04002FC4 RID: 12228
	private const string MOVE_DOWN_END = "Move_Down_End";

	// Token: 0x04002FC5 RID: 12229
	private const string MOVE_DOWN_SHORT_END = "Move_Down_Short_End";

	// Token: 0x04002FC6 RID: 12230
	[SerializeField]
	private GameObject arms;

	// Token: 0x04002FC7 RID: 12231
	[SerializeField]
	private GameObject armsHolding;

	// Token: 0x04002FC8 RID: 12232
	[SerializeField]
	private Transform throwPos;

	// Token: 0x04002FC9 RID: 12233
	[SerializeField]
	private Transform catchPos;

	// Token: 0x04002FCA RID: 12234
	public bool ready;

	// Token: 0x04002FCB RID: 12235
	private DamageDealer damageDealer;

	// Token: 0x04002FCC RID: 12236
	[SerializeField]
	private float armBowingXModifier;

	// Token: 0x04002FCD RID: 12237
	[SerializeField]
	private float wobbleX = 5f;

	// Token: 0x04002FCE RID: 12238
	[SerializeField]
	private float wobbleY = 5f;

	// Token: 0x04002FCF RID: 12239
	public Vector3 rootPosition;

	// Token: 0x04002FD0 RID: 12240
	public Vector3 startPosition;

	// Token: 0x04002FD1 RID: 12241
	[SerializeField]
	private float wobbleTimer;

	// Token: 0x04002FD2 RID: 12242
	[SerializeField]
	private float moveTimeShort = 0.375f;

	// Token: 0x04002FD3 RID: 12243
	[SerializeField]
	private float moveTimeLong = 0.5f;

	// Token: 0x04002FD4 RID: 12244
	[SerializeField]
	private float moveOvershoot = 0.5f;

	// Token: 0x04002FD5 RID: 12245
	[SerializeField]
	private bool isLeft;

	// Token: 0x04002FD6 RID: 12246
	private bool entering = true;

	// Token: 0x04002FD7 RID: 12247
	private bool dead;

	// Token: 0x04002FD8 RID: 12248
	[SerializeField]
	private OldManLevelSockPuppetHandler main;

	// Token: 0x04002FD9 RID: 12249
	private Collider2D[] colliders;
}
