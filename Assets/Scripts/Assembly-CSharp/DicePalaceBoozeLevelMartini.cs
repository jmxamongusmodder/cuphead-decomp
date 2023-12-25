using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020005A1 RID: 1441
public class DicePalaceBoozeLevelMartini : DicePalaceBoozeLevelBossBase
{
	// Token: 0x06001BAA RID: 7082 RVA: 0x000FC284 File Offset: 0x000FA684
	protected override void Awake()
	{
		this.olives = new List<DicePalaceBoozeLevelOlive>();
		this.damageDealer = DamageDealer.NewEnemy();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		base.Awake();
	}

	// Token: 0x06001BAB RID: 7083 RVA: 0x000FC2D0 File Offset: 0x000FA6D0
	private void Update()
	{
		this.damageDealer.Update();
	}

	// Token: 0x06001BAC RID: 7084 RVA: 0x000FC2E0 File Offset: 0x000FA6E0
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		float health = this.health;
		this.health -= info.damage;
		if (health > 0f)
		{
			Level.Current.timeline.DealDamage(Mathf.Clamp(health - this.health, 0f, health));
		}
		if (this.health < 0f && !base.isDead)
		{
			this.StartDying();
			this.MartiniDeathSFX();
		}
	}

	// Token: 0x06001BAD RID: 7085 RVA: 0x000FC35C File Offset: 0x000FA75C
	public override void LevelInit(LevelProperties.DicePalaceBooze properties)
	{
		this.activeOlives = 0;
		this.pinkShotIndex = UnityEngine.Random.Range(0, properties.CurrentState.martini.pinkString.Split(new char[]
		{
			','
		}).Length);
		int num = properties.CurrentState.martini.olivePositionStringX.Length;
		int num2 = UnityEngine.Random.Range(0, num);
		int num3 = UnityEngine.Random.Range(0, num);
		for (int i = 0; i < num; i++)
		{
			GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.olive.gameObject, this.spawnPoint.position, Quaternion.identity);
			gameObject.GetComponent<DicePalaceBoozeLevelOlive>().InitOlive(properties, Parser.IntParse(properties.CurrentState.martini.pinkString.Split(new char[]
			{
				','
			})[this.pinkShotIndex]), properties.CurrentState.martini.olivePositionStringY[num3], properties.CurrentState.martini.olivePositionStringX[num2]);
			this.pinkShotIndex++;
			if (this.pinkShotIndex >= properties.CurrentState.martini.pinkString.Split(new char[]
			{
				','
			}).Length)
			{
				this.pinkShotIndex = 0;
			}
			num3++;
			if (num3 >= num)
			{
				num3 = 0;
			}
			num2++;
			if (num2 >= num)
			{
				num2 = 0;
			}
			gameObject.SetActive(false);
			this.olives.Add(gameObject.GetComponent<DicePalaceBoozeLevelOlive>());
		}
		Level.Current.OnIntroEvent += this.OnIntroEnd;
		Level.Current.OnWinEvent += this.HandleDead;
		AudioManager.Play("booze_martini_intro");
		this.emitAudioFromObject.Add("booze_martini_intro");
		base.LevelInit(properties);
		this.health = properties.CurrentState.martini.martiniHP;
	}

	// Token: 0x06001BAE RID: 7086 RVA: 0x000FC52C File Offset: 0x000FA92C
	private void OnIntroEnd()
	{
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06001BAF RID: 7087 RVA: 0x000FC53C File Offset: 0x000FA93C
	private IEnumerator attack_cr()
	{
		this.oliveIndex = 0;
		int counter = 0;
		for (;;)
		{
			counter = 0;
			yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.martini.oliveSpawnDelay - DicePalaceBoozeLevelBossBase.ATTACK_DELAY);
			yield return null;
			while (this.olives[this.oliveIndex].gameObject.activeSelf)
			{
				this.oliveIndex = (this.oliveIndex + 1) % this.olives.Count;
				counter++;
				if (counter >= this.olives.Count)
				{
					this.allActive = true;
					break;
				}
				yield return null;
			}
			if (counter < this.olives.Count)
			{
				this.allActive = false;
			}
			if (!this.allActive)
			{
				base.animator.SetTrigger("OnAttack");
				yield return base.animator.WaitForAnimationToStart(this, "Attack", false);
				AudioManager.Play("booze_martini_attack");
				this.emitAudioFromObject.Add("booze_martini_attack");
				yield return base.animator.WaitForAnimationToEnd(this, "Attack", false, true);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06001BB0 RID: 7088 RVA: 0x000FC558 File Offset: 0x000FA958
	private void ShootOlive()
	{
		this.olives[this.oliveIndex].transform.position = this.spawnPoint.position;
		this.olives[this.oliveIndex].gameObject.SetActive(true);
		this.olives[this.oliveIndex].ResetOlive(Parser.IntParse(base.properties.CurrentState.martini.pinkString.Split(new char[]
		{
			','
		})[this.pinkShotIndex]));
		this.pinkShotIndex++;
		if (this.pinkShotIndex >= base.properties.CurrentState.martini.pinkString.Split(new char[]
		{
			','
		}).Length)
		{
			this.pinkShotIndex = 0;
		}
		this.oliveIndex = (this.oliveIndex + 1) % this.olives.Count;
	}

	// Token: 0x06001BB1 RID: 7089 RVA: 0x000FC64F File Offset: 0x000FAA4F
	private void OnOliveDeath()
	{
		this.activeOlives--;
	}

	// Token: 0x06001BB2 RID: 7090 RVA: 0x000FC65F File Offset: 0x000FAA5F
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
		base.OnCollisionPlayer(hit, phase);
	}

	// Token: 0x06001BB3 RID: 7091 RVA: 0x000FC67D File Offset: 0x000FAA7D
	protected override void OnDestroy()
	{
		this.StopAllCoroutines();
		base.OnDestroy();
		this.olive = null;
	}

	// Token: 0x06001BB4 RID: 7092 RVA: 0x000FC692 File Offset: 0x000FAA92
	private void MartiniDeathSFX()
	{
		AudioManager.Play("martini_death_vox");
		this.emitAudioFromObject.Add("martini_death_vox");
	}

	// Token: 0x040024B9 RID: 9401
	[SerializeField]
	private DicePalaceBoozeLevelOlive olive;

	// Token: 0x040024BA RID: 9402
	[SerializeField]
	private Transform spawnPoint;

	// Token: 0x040024BB RID: 9403
	private bool allActive;

	// Token: 0x040024BC RID: 9404
	private int oliveIndex;

	// Token: 0x040024BD RID: 9405
	private int pinkShotIndex;

	// Token: 0x040024BE RID: 9406
	private int activeOlives;

	// Token: 0x040024BF RID: 9407
	private List<DicePalaceBoozeLevelOlive> olives;

	// Token: 0x040024C0 RID: 9408
	private DamageDealer damageDealer;

	// Token: 0x040024C1 RID: 9409
	private DamageReceiver damageReceiver;
}
