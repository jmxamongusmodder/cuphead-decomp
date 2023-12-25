using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000507 RID: 1287
public class BatLevelHomingSoul : AbstractCollidableObject
{
	// Token: 0x060016D0 RID: 5840 RVA: 0x000CD160 File Offset: 0x000CB560
	public void Init(Vector2 pos, AbstractPlayerController player, LevelProperties.Bat.WolfSoul properties)
	{
		this.aim = new GameObject("Aim").transform;
		this.aim.SetParent(base.transform);
		this.aim.ResetLocalTransforms();
		this.properties = properties;
		this.player = player;
		this.durationString = properties.floatUpDuration.GetRandom<string>().Split(new char[]
		{
			','
		});
	}

	// Token: 0x060016D1 RID: 5841 RVA: 0x000CD1CD File Offset: 0x000CB5CD
	protected override void Awake()
	{
		base.Awake();
		base.GetComponent<Collider2D>().enabled = false;
		this.isHoming = true;
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x060016D2 RID: 5842 RVA: 0x000CD1F4 File Offset: 0x000CB5F4
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
		if (this.aim == null || this.player == null)
		{
			return;
		}
		if (this.isHoming)
		{
			float f = Vector3.Distance(base.transform.position, this.player.transform.position);
			base.transform.position -= base.transform.right * this.properties.homingSpeed * CupheadTime.Delta;
			this.aim.LookAt2D(2f * base.transform.position - this.player.center);
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, this.aim.rotation, this.properties.homingRotation * CupheadTime.Delta);
			if (Mathf.Abs(f) < this.maxDist && this.isHoming)
			{
				base.StartCoroutine(this.attack_cr());
				this.isHoming = false;
			}
		}
	}

	// Token: 0x060016D3 RID: 5843 RVA: 0x000CD342 File Offset: 0x000CB742
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		this.damageDealer.DealDamage(hit);
	}

	// Token: 0x060016D4 RID: 5844 RVA: 0x000CD35C File Offset: 0x000CB75C
	private IEnumerator attack_cr()
	{
		base.animator.SetTrigger("Warning");
		yield return CupheadTime.WaitForSeconds(this, this.properties.floatWarningDuration);
		base.animator.SetTrigger("Attack");
		base.GetComponent<Collider2D>().enabled = true;
		yield return CupheadTime.WaitForSeconds(this, this.properties.attackDuration);
		base.animator.SetTrigger("End");
		base.GetComponent<Collider2D>().enabled = false;
		base.StartCoroutine(this.float_up_cr());
		yield return null;
		yield break;
	}

	// Token: 0x060016D5 RID: 5845 RVA: 0x000CD378 File Offset: 0x000CB778
	private IEnumerator float_up_cr()
	{
		float t = 0f;
		float duration = 0f;
		Parser.FloatTryParse(this.durationString[this.durationIndex], out duration);
		this.player = PlayerManager.GetNext();
		while (t < duration)
		{
			base.transform.AddPosition(0f, this.properties.floatSpeed * CupheadTime.Delta, 0f);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.isHoming = true;
		this.durationIndex %= this.durationString.Length;
		yield return null;
		yield break;
	}

	// Token: 0x0400201B RID: 8219
	private LevelProperties.Bat.WolfSoul properties;

	// Token: 0x0400201C RID: 8220
	private AbstractPlayerController player;

	// Token: 0x0400201D RID: 8221
	private DamageDealer damageDealer;

	// Token: 0x0400201E RID: 8222
	private int durationIndex;

	// Token: 0x0400201F RID: 8223
	private Transform aim;

	// Token: 0x04002020 RID: 8224
	private Transform targetPos;

	// Token: 0x04002021 RID: 8225
	private float maxDist = 100f;

	// Token: 0x04002022 RID: 8226
	private bool isHoming;

	// Token: 0x04002023 RID: 8227
	private string[] durationString;
}
