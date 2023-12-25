using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007F7 RID: 2039
public class SnowCultLevelSnowman : AbstractCollidableObject
{
	// Token: 0x06002ED4 RID: 11988 RVA: 0x001BA130 File Offset: 0x001B8530
	public void Init(Vector3 pos, LevelProperties.SnowCult.Snowman properties, bool goingRight)
	{
		base.transform.position = pos;
		this.yPos = base.transform.position.y;
		this.properties = properties;
		this.goingRight = goingRight;
		if (goingRight)
		{
			base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		}
		else
		{
			base.transform.SetScale(new float?(base.transform.localScale.x), null, null);
		}
	}

	// Token: 0x06002ED5 RID: 11989 RVA: 0x001BA1E8 File Offset: 0x001B85E8
	private void Start()
	{
		this.coll = base.GetComponent<Collider2D>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.Health = this.properties.health;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002ED6 RID: 11990 RVA: 0x001BA24D File Offset: 0x001B864D
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002ED7 RID: 11991 RVA: 0x001BA265 File Offset: 0x001B8665
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002ED8 RID: 11992 RVA: 0x001BA283 File Offset: 0x001B8683
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f && !this.melted)
		{
			this.melted = true;
			this.Melt();
		}
	}

	// Token: 0x06002ED9 RID: 11993 RVA: 0x001BA2C0 File Offset: 0x001B86C0
	private void Melt()
	{
		base.StartCoroutine(this.melt_cr());
	}

	// Token: 0x06002EDA RID: 11994 RVA: 0x001BA2D0 File Offset: 0x001B86D0
	private IEnumerator melt_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Melt");
		yield return base.animator.WaitForAnimationToEnd(this, "Melt", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.timeUntilUnmelt);
		base.animator.SetTrigger("Continue");
		yield return CupheadTime.WaitForSeconds(this, this.properties.unmeltLoopTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Unmelt", false, true);
		this.melted = false;
		this.Health = this.properties.health;
		base.GetComponent<Collider2D>().enabled = true;
		yield return null;
		yield break;
	}

	// Token: 0x06002EDB RID: 11995 RVA: 0x001BA2EC File Offset: 0x001B86EC
	private void Turn()
	{
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
	}

	// Token: 0x06002EDC RID: 11996 RVA: 0x001BA330 File Offset: 0x001B8730
	private IEnumerator move_cr()
	{
		float sizeX = this.coll.bounds.size.x;
		float left = -640f + sizeX / 2f;
		float right = 640f - sizeX / 2f;
		float t = 0f;
		float time = this.properties.runTime;
		EaseUtils.EaseType ease = EaseUtils.EaseType.linear;
		Vector3 endPos = Vector3.zero;
		endPos = ((!this.goingRight) ? new Vector3(left, this.yPos) : new Vector3(right, this.yPos));
		float speed = Vector3.Distance(new Vector3(right, this.yPos), new Vector3(left, this.yPos)) / time;
		while (base.transform.position != endPos)
		{
			while (this.melted)
			{
				yield return null;
			}
			base.transform.position = Vector3.MoveTowards(base.transform.position, endPos, speed * CupheadTime.Delta);
			yield return null;
		}
		float start = 0f;
		float end = 0f;
		base.animator.Play("Turn");
		yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
		base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
		if (this.goingRight)
		{
			start = base.transform.position.x;
			end = left;
		}
		else
		{
			start = base.transform.position.x;
			end = right;
		}
		for (;;)
		{
			t = 0f;
			while (t < time)
			{
				if (!this.melted)
				{
					float value = t / time;
					base.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, value)), null, null);
					t += CupheadTime.Delta;
				}
				yield return null;
			}
			base.animator.Play("Turn");
			yield return base.animator.WaitForAnimationToEnd(this, "Turn", false, true);
			base.transform.SetScale(new float?(-base.transform.localScale.x), null, null);
			this.goingRight = !this.goingRight;
			if (!this.goingRight)
			{
				start = left;
				end = right;
			}
			else
			{
				start = right;
				end = left;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002EDD RID: 11997 RVA: 0x001BA34B File Offset: 0x001B874B
	public void Die()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x04003785 RID: 14213
	private LevelProperties.SnowCult.Snowman properties;

	// Token: 0x04003786 RID: 14214
	private Collider2D coll;

	// Token: 0x04003787 RID: 14215
	private DamageDealer damageDealer;

	// Token: 0x04003788 RID: 14216
	private DamageReceiver damageReceiver;

	// Token: 0x04003789 RID: 14217
	private bool goingRight;

	// Token: 0x0400378A RID: 14218
	private bool melted;

	// Token: 0x0400378B RID: 14219
	private float Health;

	// Token: 0x0400378C RID: 14220
	private float yPos;
}
