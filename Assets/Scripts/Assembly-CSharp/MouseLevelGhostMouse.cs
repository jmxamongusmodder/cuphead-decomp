using System;
using System.Collections;
using UnityEngine;

// Token: 0x020006EF RID: 1775
public class MouseLevelGhostMouse : AbstractCollidableObject
{
	// Token: 0x170003C7 RID: 967
	// (get) Token: 0x06002605 RID: 9733 RVA: 0x00163B73 File Offset: 0x00161F73
	// (set) Token: 0x06002606 RID: 9734 RVA: 0x00163B7B File Offset: 0x00161F7B
	public MouseLevelGhostMouse.State state { get; private set; }

	// Token: 0x06002607 RID: 9735 RVA: 0x00163B84 File Offset: 0x00161F84
	protected override void Awake()
	{
		base.Awake();
		this.basePos = base.transform.localPosition;
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06002608 RID: 9736 RVA: 0x00163BB9 File Offset: 0x00161FB9
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.hp -= info.damage;
		if (this.hp < 0f && this.state != MouseLevelGhostMouse.State.Dying)
		{
			this.Die();
		}
	}

	// Token: 0x06002609 RID: 9737 RVA: 0x00163BF0 File Offset: 0x00161FF0
	public void Spawn(LevelProperties.Mouse properties)
	{
		this.properties = properties;
		if (this.state == MouseLevelGhostMouse.State.Unspawned)
		{
			this.StopAllCoroutines();
			this.state = MouseLevelGhostMouse.State.Intro;
			base.animator.ResetTrigger("AttackBlue");
			base.animator.ResetTrigger("AttackPink");
			base.animator.ResetTrigger("Continue");
			base.StartCoroutine(this.spawn_cr());
		}
	}

	// Token: 0x0600260A RID: 9738 RVA: 0x00163C5C File Offset: 0x0016205C
	private IEnumerator spawn_cr()
	{
		float spawnOffset = 150f * base.transform.localScale.x;
		float yPos = this.basePos.y + UnityEngine.Random.Range(-35f, 35f);
		Vector2 start = new Vector2(this.basePos.x * 0.125f + spawnOffset, yPos);
		this.hp = this.properties.CurrentState.ghostMouse.hp;
		base.transform.SetEulerAngles(new float?(0f), new float?(0f), new float?(0f));
		base.animator.SetTrigger("Spawn");
		float t = 0f;
		while (t < 1.083f)
		{
			base.transform.SetLocalPosition(new float?(EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, start.x, this.basePos.x, t / 1.083f)), new float?(yPos), null);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.SetLocalPosition(new float?(this.basePos.x), new float?(yPos), null);
		yield return base.animator.WaitForAnimationToStart(this, "Idle_A", false);
		this.state = MouseLevelGhostMouse.State.Idle;
		yield break;
	}

	// Token: 0x0600260B RID: 9739 RVA: 0x00163C77 File Offset: 0x00162077
	public void Attack(bool pink)
	{
		this.state = MouseLevelGhostMouse.State.Attack;
		base.StartCoroutine(this.attack_cr(pink));
	}

	// Token: 0x0600260C RID: 9740 RVA: 0x00163C90 File Offset: 0x00162090
	private IEnumerator attack_cr(bool pink)
	{
		base.animator.SetTrigger((!pink) ? "AttackBlue" : "AttackPink");
		yield return base.animator.WaitForAnimationToStart(this, (!pink) ? "Attack_Blue_Loop" : "Attack_Pink_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, this.properties.CurrentState.ghostMouse.attackAnticipation);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Idle_B", false);
		this.state = MouseLevelGhostMouse.State.Idle;
		yield break;
	}

	// Token: 0x0600260D RID: 9741 RVA: 0x00163CB4 File Offset: 0x001620B4
	private void FireBlue()
	{
		this.blueBallPrefab.Create(this.projectileRoot.position, this.properties.CurrentState.ghostMouse.ballSpeed, this.properties.CurrentState.ghostMouse.splitSpeed);
	}

	// Token: 0x0600260E RID: 9742 RVA: 0x00163D08 File Offset: 0x00162108
	private void FirePink()
	{
		this.pinkBallPrefab.Create(this.projectileRoot.position, this.properties.CurrentState.ghostMouse.ballSpeed, this.properties.CurrentState.ghostMouse.splitSpeed);
	}

	// Token: 0x0600260F RID: 9743 RVA: 0x00163D5B File Offset: 0x0016215B
	public void Die()
	{
		if (this.state == MouseLevelGhostMouse.State.Unspawned || this.state == MouseLevelGhostMouse.State.Dying)
		{
			return;
		}
		this.state = MouseLevelGhostMouse.State.Dying;
		this.StopAllCoroutines();
		base.StartCoroutine(this.death_cr());
	}

	// Token: 0x06002610 RID: 9744 RVA: 0x00163D90 File Offset: 0x00162190
	private IEnumerator death_cr()
	{
		while (this.state == MouseLevelGhostMouse.State.Intro)
		{
			yield return null;
		}
		base.animator.SetTrigger("Die");
		base.transform.Rotate(0f, 0f, (float)UnityEngine.Random.Range(-16, 16));
		yield return base.animator.WaitForAnimationToEnd(this, "Death", false, true);
		this.state = MouseLevelGhostMouse.State.Unspawned;
		yield break;
	}

	// Token: 0x06002611 RID: 9745 RVA: 0x00163DAB File Offset: 0x001621AB
	private void SoundMouseGhostWail()
	{
		AudioManager.Play("level_mouse_ghost_mouse_wail");
		this.emitAudioFromObject.Add("level_mouse_ghost_mouse_wail");
	}

	// Token: 0x06002612 RID: 9746 RVA: 0x00163DC7 File Offset: 0x001621C7
	private void SoundMouseGhostLaugh()
	{
		AudioManager.Play("level_mouse_ghost_mouse_laugh");
		this.emitAudioFromObject.Add("level_mouse_ghost_mouse_laugh");
	}

	// Token: 0x06002613 RID: 9747 RVA: 0x00163DE3 File Offset: 0x001621E3
	private void SoundMouseGhostAttack()
	{
		AudioManager.Play("level_mouse_ghost_attack");
		this.emitAudioFromObject.Add("level_mouse_ghost_attack");
	}

	// Token: 0x06002614 RID: 9748 RVA: 0x00163DFF File Offset: 0x001621FF
	private void SoundMouseGhostDeath()
	{
		AudioManager.Play("level_mouse_ghost_death");
		this.emitAudioFromObject.Add("level_mouse_ghost_death");
	}

	// Token: 0x06002615 RID: 9749 RVA: 0x00163E1B File Offset: 0x0016221B
	private void SoundMouseGhostDeathStart()
	{
		AudioManager.Play("level_mouse_ghost_death_start");
		this.emitAudioFromObject.Add("level_mouse_ghost_death_start");
	}

	// Token: 0x06002616 RID: 9750 RVA: 0x00163E37 File Offset: 0x00162237
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.blueBallPrefab = null;
		this.pinkBallPrefab = null;
	}

	// Token: 0x04002E89 RID: 11913
	private const float heightVariation = 35f;

	// Token: 0x04002E8A RID: 11914
	private const float spawnXRatio = 0.125f;

	// Token: 0x04002E8C RID: 11916
	private Vector2 basePos;

	// Token: 0x04002E8D RID: 11917
	private LevelProperties.Mouse properties;

	// Token: 0x04002E8E RID: 11918
	private float hp;

	// Token: 0x04002E8F RID: 11919
	[SerializeField]
	private MouseLevelGhostMouseBall blueBallPrefab;

	// Token: 0x04002E90 RID: 11920
	[SerializeField]
	private MouseLevelGhostMouseBall pinkBallPrefab;

	// Token: 0x04002E91 RID: 11921
	[SerializeField]
	private Transform projectileRoot;

	// Token: 0x020006F0 RID: 1776
	public enum State
	{
		// Token: 0x04002E93 RID: 11923
		Unspawned,
		// Token: 0x04002E94 RID: 11924
		Intro,
		// Token: 0x04002E95 RID: 11925
		Idle,
		// Token: 0x04002E96 RID: 11926
		Attack,
		// Token: 0x04002E97 RID: 11927
		Dying
	}
}
