using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200067D RID: 1661
public class FlyingGenieLevelSword : AbstractProjectile
{
	// Token: 0x0600230A RID: 8970 RVA: 0x00148D10 File Offset: 0x00147110
	public void Init(Vector3 startPos, Vector3 endPos, LevelProperties.FlyingGenie.Swords properties, AbstractPlayerController player)
	{
		this.startPos = startPos;
		base.transform.position = startPos;
		this.properties = properties;
		this.endPos = endPos;
		this.player = player;
		base.StartCoroutine(this.move_to_pos_cr());
		AudioManager.Play("genie_chest_sword_spawn");
		this.emitAudioFromObject.Add("genie_chest_sword_spawn");
	}

	// Token: 0x0600230B RID: 8971 RVA: 0x00148D6D File Offset: 0x0014716D
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (this.damageDealer != null && phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x0600230C RID: 8972 RVA: 0x00148D96 File Offset: 0x00147196
	protected override void Update()
	{
		base.Update();
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x0600230D RID: 8973 RVA: 0x00148DB4 File Offset: 0x001471B4
	private IEnumerator move_to_pos_cr()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, 90f);
		while (base.transform.position.y < (this.startPos + Vector3.up * this.outOfChestY).y)
		{
			base.transform.AddPosition(0f, this.outOfChestY * this.outOfChestSpeed * CupheadTime.Delta, 0f);
			yield return null;
		}
		this.swordRenderer.sortingLayerName = "Projectiles";
		this.swordRenderer.sortingOrder = 2;
		base.transform.eulerAngles = new Vector3(0f, 0f, MathUtils.DirectionToAngle(this.endPos - base.transform.position));
		while (base.transform.position != this.endPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, this.endPos, this.properties.swordSpeed * CupheadTime.Delta);
			yield return null;
		}
		float t = 0f;
		while (t < this.properties.attackDelay)
		{
			base.transform.Rotate(Vector3.forward, this.swordRotationSpeed * CupheadTime.Delta);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.StartCoroutine(this.move_continue_cr());
		AudioManager.Play("genie_chest_sword_attack");
		this.emitAudioFromObject.Add("genie_chest_sword_attack");
		yield return null;
		yield break;
	}

	// Token: 0x0600230E RID: 8974 RVA: 0x00148DD0 File Offset: 0x001471D0
	private IEnumerator move_continue_cr()
	{
		base.transform.eulerAngles = new Vector3(0f, 0f, 35f);
		base.animator.SetTrigger("Spin");
		yield return CupheadTime.WaitForSeconds(this, this.fastSpinTime);
		base.animator.SetTrigger("Attack");
		if (this.player == null || this.player.IsDead)
		{
			this.player = PlayerManager.GetNext();
		}
		Vector3 direction = this.player.transform.position - base.transform.position;
		base.transform.SetEulerAngles(null, null, new float?(MathUtils.DirectionToAngle(direction)));
		for (;;)
		{
			base.transform.position += base.transform.right * this.properties.swordSpeed * CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x0600230F RID: 8975 RVA: 0x00148DEB File Offset: 0x001471EB
	public override void SetParryable(bool parryable)
	{
		base.SetParryable(parryable);
		base.animator.SetFloat("Pink", (!parryable) ? 0f : 1f);
	}

	// Token: 0x04002BA3 RID: 11171
	private const string PinkParameterName = "Pink";

	// Token: 0x04002BA4 RID: 11172
	private const string SpinParameterName = "Spin";

	// Token: 0x04002BA5 RID: 11173
	private const string AttackParameterName = "Attack";

	// Token: 0x04002BA6 RID: 11174
	private const string ProjectilesLayer = "Projectiles";

	// Token: 0x04002BA7 RID: 11175
	private const float spinRotationOffset = 35f;

	// Token: 0x04002BA8 RID: 11176
	[SerializeField]
	private float outOfChestY;

	// Token: 0x04002BA9 RID: 11177
	[SerializeField]
	private float outOfChestSpeed;

	// Token: 0x04002BAA RID: 11178
	[SerializeField]
	private float swordRotationSpeed;

	// Token: 0x04002BAB RID: 11179
	[SerializeField]
	private float fastSpinTime;

	// Token: 0x04002BAC RID: 11180
	[SerializeField]
	private SpriteRenderer swordRenderer;

	// Token: 0x04002BAD RID: 11181
	private LevelProperties.FlyingGenie.Swords properties;

	// Token: 0x04002BAE RID: 11182
	private AbstractPlayerController player;

	// Token: 0x04002BAF RID: 11183
	private Vector3 endPos;

	// Token: 0x04002BB0 RID: 11184
	private Vector3 startPos;
}
