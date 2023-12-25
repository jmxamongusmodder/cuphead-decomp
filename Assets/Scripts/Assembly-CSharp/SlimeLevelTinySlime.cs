using System;
using System.Collections;
using UnityEngine;

// Token: 0x020007E0 RID: 2016
public class SlimeLevelTinySlime : AbstractCollidableObject
{
	// Token: 0x06002E1A RID: 11802 RVA: 0x001B2CA4 File Offset: 0x001B10A4
	public void Init(Vector3 pos, LevelProperties.Slime.Tombstone properties, bool goingRight, SlimeLevelTombstone parent)
	{
		base.transform.position = pos;
		this.properties = properties;
		this.goingRight = goingRight;
		this.parent = parent;
		SlimeLevelTombstone slimeLevelTombstone = this.parent;
		slimeLevelTombstone.onDeath = (Action)Delegate.Combine(slimeLevelTombstone.onDeath, new Action(this.Death));
		if (goingRight)
		{
			base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
		}
		else
		{
			base.transform.SetScale(new float?(base.transform.localScale.x), new float?(1f), new float?(1f));
		}
	}

	// Token: 0x06002E1B RID: 11803 RVA: 0x001B2D74 File Offset: 0x001B1174
	private void Start()
	{
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		this.Health = this.properties.tinyHealth;
		base.StartCoroutine(this.move_cr());
	}

	// Token: 0x06002E1C RID: 11804 RVA: 0x001B2DCD File Offset: 0x001B11CD
	private void Update()
	{
		if (this.damageDealer != null)
		{
			this.damageDealer.Update();
		}
	}

	// Token: 0x06002E1D RID: 11805 RVA: 0x001B2DE5 File Offset: 0x001B11E5
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002E1E RID: 11806 RVA: 0x001B2E03 File Offset: 0x001B1203
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.Health -= info.damage;
		if (this.Health < 0f && !this.melted)
		{
			this.Melt();
		}
	}

	// Token: 0x06002E1F RID: 11807 RVA: 0x001B2E39 File Offset: 0x001B1239
	private void Melt()
	{
		this.melted = true;
		base.StartCoroutine(this.melt_cr());
	}

	// Token: 0x06002E20 RID: 11808 RVA: 0x001B2E50 File Offset: 0x001B1250
	private IEnumerator melt_cr()
	{
		base.GetComponent<Collider2D>().enabled = false;
		AudioManager.Stop("level_blobrunner");
		AudioManager.Play("level_frogs_tall_firefly_death");
		base.animator.Play("Melt");
		yield return base.animator.WaitForAnimationToEnd(this, "Melt", false, true);
		yield return CupheadTime.WaitForSeconds(this, this.properties.tinyMeltDelay);
		base.animator.SetTrigger("Continue");
		AudioManager.Play("level_blobrunner_reform");
		this.emitAudioFromObject.Add("level_blobrunner_reform");
		yield return CupheadTime.WaitForSeconds(this, this.properties.tinyTimeUntilUnmelt);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToEnd(this, "Unmelt", false, true);
		this.melted = false;
		this.Health = this.properties.tinyHealth;
		base.GetComponent<Collider2D>().enabled = true;
		yield break;
	}

	// Token: 0x06002E21 RID: 11809 RVA: 0x001B2E6C File Offset: 0x001B126C
	private IEnumerator move_cr()
	{
		float sizeX = this.sprite.bounds.size.x;
		float sizeY = this.sprite.bounds.size.y;
		float left = -640f + sizeX / 2f;
		float right = 640f - sizeX / 2f;
		float down = (float)Level.Current.Ground + sizeY / 3f;
		float t = 0f;
		float time = this.properties.tinyRunTime;
		EaseUtils.EaseType ease = EaseUtils.EaseType.linear;
		float speed = 600f;
		float acceleration = 10f;
		Vector3 endPos = Vector3.zero;
		endPos = ((!this.goingRight) ? new Vector3(left, down) : new Vector3(right, down));
		while (base.transform.position != endPos)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, endPos, speed * CupheadTime.Delta);
			speed += acceleration;
			yield return null;
		}
		float start = 0f;
		float end = 0f;
		this.sprite.sortingLayerName = SpriteLayer.Projectiles.ToString();
		if (this.goingRight)
		{
			start = base.transform.position.x;
			end = right;
		}
		else
		{
			start = base.transform.position.x;
			end = left;
		}
		time = 0f;
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
			base.transform.SetScale(new float?(-base.transform.localScale.x), new float?(1f), new float?(1f));
			this.goingRight = !this.goingRight;
			if (this.goingRight)
			{
				start = left;
				end = right;
			}
			else
			{
				start = right;
				end = left;
			}
			time = this.properties.tinyRunTime;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002E22 RID: 11810 RVA: 0x001B2E87 File Offset: 0x001B1287
	private void Death()
	{
		this.StopAllCoroutines();
		base.GetComponent<Collider2D>().enabled = false;
		base.animator.Play("Melt");
	}

	// Token: 0x06002E23 RID: 11811 RVA: 0x001B2EAB File Offset: 0x001B12AB
	protected override void OnDestroy()
	{
		SlimeLevelTombstone slimeLevelTombstone = this.parent;
		slimeLevelTombstone.onDeath = (Action)Delegate.Remove(slimeLevelTombstone.onDeath, new Action(this.Death));
		base.OnDestroy();
	}

	// Token: 0x04003694 RID: 13972
	[SerializeField]
	private SpriteRenderer sprite;

	// Token: 0x04003695 RID: 13973
	private LevelProperties.Slime.Tombstone properties;

	// Token: 0x04003696 RID: 13974
	private SlimeLevelTombstone parent;

	// Token: 0x04003697 RID: 13975
	private DamageDealer damageDealer;

	// Token: 0x04003698 RID: 13976
	private DamageReceiver damageReceiver;

	// Token: 0x04003699 RID: 13977
	private bool goingRight;

	// Token: 0x0400369A RID: 13978
	private bool melted;

	// Token: 0x0400369B RID: 13979
	private float Health;
}
