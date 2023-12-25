using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004FE RID: 1278
public class BaronessLevelPeppermint : ParrySwitch
{
	// Token: 0x0600167F RID: 5759 RVA: 0x000CA50B File Offset: 0x000C890B
	protected override void Awake()
	{
		base.Awake();
		this.damageDealer = DamageDealer.NewEnemy();
	}

	// Token: 0x06001680 RID: 5760 RVA: 0x000CA51E File Offset: 0x000C891E
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06001681 RID: 5761 RVA: 0x000CA53C File Offset: 0x000C893C
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06001682 RID: 5762 RVA: 0x000CA554 File Offset: 0x000C8954
	public void Init(Vector2 pos, float speed)
	{
		base.transform.position = pos;
		this.speed = speed;
		AudioManager.Play("level_baroness_candy_roll");
		base.StartCoroutine(this.fade_color_cr());
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06001683 RID: 5763 RVA: 0x000CA594 File Offset: 0x000C8994
	protected virtual IEnumerator fade_color_cr()
	{
		float fadeTime = 0.7f;
		float t = 0f;
		while (t < fadeTime)
		{
			base.GetComponent<SpriteRenderer>().color = new Color(t / fadeTime, t / fadeTime, t / fadeTime, 1f);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
		yield return null;
		yield break;
	}

	// Token: 0x06001684 RID: 5764 RVA: 0x000CA5B0 File Offset: 0x000C89B0
	private IEnumerator move_cr()
	{
		float offsetX = 220f;
		Vector3 pos = base.transform.position;
		for (;;)
		{
			if (base.transform.position.x > -640f - offsetX)
			{
				pos.x = Mathf.MoveTowards(base.transform.position.x, -640f - offsetX, this.speed * CupheadTime.FixedDelta);
			}
			else
			{
				this.Die();
			}
			base.transform.position = pos;
			yield return new WaitForFixedUpdate();
		}
		yield break;
	}

	// Token: 0x06001685 RID: 5765 RVA: 0x000CA5CB File Offset: 0x000C89CB
	private void Die()
	{
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06001686 RID: 5766 RVA: 0x000CA5D8 File Offset: 0x000C89D8
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		player.stats.ParryOneQuarter();
	}

	// Token: 0x06001687 RID: 5767 RVA: 0x000CA5EC File Offset: 0x000C89EC
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		base.IsParryable = false;
		base.StartCoroutine(this.peppermintParryCooldown_cr());
	}

	// Token: 0x06001688 RID: 5768 RVA: 0x000CA60C File Offset: 0x000C8A0C
	private IEnumerator peppermintParryCooldown_cr()
	{
		float t = 0f;
		while (t < this.coolDown)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.IsParryable = true;
		yield return null;
		yield break;
	}

	// Token: 0x04001FDA RID: 8154
	private DamageDealer damageDealer;

	// Token: 0x04001FDB RID: 8155
	private float speed;
}
