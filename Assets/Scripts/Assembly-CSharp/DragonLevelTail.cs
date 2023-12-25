using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000601 RID: 1537
public class DragonLevelTail : LevelProperties.Dragon.Entity
{
	// Token: 0x17000378 RID: 888
	// (get) Token: 0x06001E8F RID: 7823 RVA: 0x00119986 File Offset: 0x00117D86
	// (set) Token: 0x06001E90 RID: 7824 RVA: 0x0011998E File Offset: 0x00117D8E
	public DragonLevelTail.State state { get; private set; }

	// Token: 0x06001E91 RID: 7825 RVA: 0x00119998 File Offset: 0x00117D98
	protected override void Awake()
	{
		base.Awake();
		base.RegisterCollisionChild(this.childCollider);
		base.transform.SetPosition(null, new float?(-1210f), null);
		this.damageDealer = new DamageDealer(1f, 0.1f, true, false, false);
	}

	// Token: 0x06001E92 RID: 7826 RVA: 0x001199F6 File Offset: 0x00117DF6
	public override void LevelInit(LevelProperties.Dragon properties)
	{
		base.LevelInit(properties);
	}

	// Token: 0x06001E93 RID: 7827 RVA: 0x001199FF File Offset: 0x00117DFF
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001E94 RID: 7828 RVA: 0x00119A17 File Offset: 0x00117E17
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001E95 RID: 7829 RVA: 0x00119A40 File Offset: 0x00117E40
	public void TailStart(float warningTime, float inTime, float holdTime, float outTime)
	{
		base.StartCoroutine(this.go_cr(warningTime, inTime, holdTime, outTime));
	}

	// Token: 0x06001E96 RID: 7830 RVA: 0x00119A54 File Offset: 0x00117E54
	private IEnumerator go_cr(float warningTime, float inTime, float holdTime, float outTime)
	{
		this.state = DragonLevelTail.State.Tail;
		base.transform.SetPosition(new float?(PlayerManager.GetNext().transform.position.x), null, null);
		AudioManager.Play("level_dragon_left_dragon_tail_appear");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_tail_appear");
		yield return base.TweenPositionY(-1210f, -1045f, 0.3f, EaseUtils.EaseType.easeOutSine);
		yield return CupheadTime.WaitForSeconds(this, warningTime);
		AudioManager.Play("level_dragon_left_dragon_tail_attack");
		this.emitAudioFromObject.Add("level_dragon_left_dragon_tail_attack");
		yield return base.TweenPositionY(-1045f, -465f, inTime, EaseUtils.EaseType.easeInSine);
		CupheadLevelCamera.Current.Shake(20f, 0.4f, false);
		yield return CupheadTime.WaitForSeconds(this, holdTime);
		yield return base.TweenPositionY(-465f, -1210f, outTime, EaseUtils.EaseType.easeInSine);
		this.state = DragonLevelTail.State.Idle;
		yield break;
	}

	// Token: 0x04002766 RID: 10086
	public const float OUT_Y = -1210f;

	// Token: 0x04002767 RID: 10087
	public const float IN_Y = -465f;

	// Token: 0x04002768 RID: 10088
	public const float START_Y = -1045f;

	// Token: 0x04002769 RID: 10089
	public const float START_TIME = 0.3f;

	// Token: 0x0400276B RID: 10091
	[SerializeField]
	private CollisionChild childCollider;

	// Token: 0x0400276C RID: 10092
	private DamageDealer damageDealer;

	// Token: 0x02000602 RID: 1538
	public enum State
	{
		// Token: 0x0400276E RID: 10094
		Idle,
		// Token: 0x0400276F RID: 10095
		Tail
	}
}
