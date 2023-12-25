using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x0200081C RID: 2076
public class TrainLevelLollipopGhoul : LevelProperties.Train.Entity
{
	// Token: 0x17000420 RID: 1056
	// (get) Token: 0x0600302A RID: 12330 RVA: 0x001C6E56 File Offset: 0x001C5256
	// (set) Token: 0x0600302B RID: 12331 RVA: 0x001C6E5E File Offset: 0x001C525E
	public TrainLevelLollipopGhoul.State state { get; private set; }

	// Token: 0x14000053 RID: 83
	// (add) Token: 0x0600302C RID: 12332 RVA: 0x001C6E68 File Offset: 0x001C5268
	// (remove) Token: 0x0600302D RID: 12333 RVA: 0x001C6EA0 File Offset: 0x001C52A0
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event TrainLevelLollipopGhoul.OnDamageTakenHandler OnDamageTakenEvent;

	// Token: 0x14000054 RID: 84
	// (add) Token: 0x0600302E RID: 12334 RVA: 0x001C6ED8 File Offset: 0x001C52D8
	// (remove) Token: 0x0600302F RID: 12335 RVA: 0x001C6F10 File Offset: 0x001C5310
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event Action OnDeathEvent;

	// Token: 0x06003030 RID: 12336 RVA: 0x001C6F46 File Offset: 0x001C5346
	protected override void Awake()
	{
		base.Awake();
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x06003031 RID: 12337 RVA: 0x001C6F74 File Offset: 0x001C5374
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.health <= 0f)
		{
			return;
		}
		if (this.OnDamageTakenEvent != null)
		{
			this.OnDamageTakenEvent(info.damage);
		}
		this.health -= info.damage;
		if (this.health <= 0f)
		{
			this.Die();
		}
	}

	// Token: 0x06003032 RID: 12338 RVA: 0x001C6FD7 File Offset: 0x001C53D7
	public override void LevelInit(LevelProperties.Train properties)
	{
		base.LevelInit(properties);
		this.health = properties.CurrentState.lollipopGhouls.health;
	}

	// Token: 0x06003033 RID: 12339 RVA: 0x001C6FF6 File Offset: 0x001C53F6
	private void Die()
	{
		if (this.OnDeathEvent != null)
		{
			this.OnDeathEvent();
		}
		this.OnDeathEvent = null;
		this.StopAllCoroutines();
		base.StartCoroutine(this.die_cr());
	}

	// Token: 0x06003034 RID: 12340 RVA: 0x001C7028 File Offset: 0x001C5428
	private void DeathAnimComplete()
	{
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003035 RID: 12341 RVA: 0x001C703B File Offset: 0x001C543B
	public void AnimateIn()
	{
		base.animator.Play("Intro");
		this.state = TrainLevelLollipopGhoul.State.Ready;
	}

	// Token: 0x06003036 RID: 12342 RVA: 0x001C7054 File Offset: 0x001C5454
	public void Attack()
	{
		this.state = TrainLevelLollipopGhoul.State.Attacking;
		base.StartCoroutine(this.attack_cr());
	}

	// Token: 0x06003037 RID: 12343 RVA: 0x001C706C File Offset: 0x001C546C
	private void StartLightning()
	{
		if (this.currentLightning != null)
		{
			UnityEngine.Object.Destroy(this.currentLightning);
		}
		this.currentLightning = UnityEngine.Object.Instantiate<TrainLevelLollipopGhoulLightning>(this.lightningPrefab);
		this.currentLightning.transform.SetParent(this.lightningRoot);
		this.currentLightning.transform.ResetLocalTransforms();
	}

	// Token: 0x06003038 RID: 12344 RVA: 0x001C70CC File Offset: 0x001C54CC
	private void EndLightning()
	{
		if (this.currentLightning == null)
		{
			return;
		}
		this.currentLightning.End();
		this.currentLightning = null;
	}

	// Token: 0x06003039 RID: 12345 RVA: 0x001C70F4 File Offset: 0x001C54F4
	private IEnumerator attack_cr()
	{
		yield return null;
		base.animator.ResetTrigger("Continue");
		base.animator.SetTrigger("OnAttack");
		yield return base.animator.WaitForAnimationToStart(this, "Attack_Charge", false);
		AudioManager.Play("train_lollipop_ghoul_attack_start");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_attack_start");
		yield return CupheadTime.WaitForSeconds(this, base.properties.CurrentState.lollipopGhouls.warningTime);
		base.animator.SetTrigger("Continue");
		yield return base.animator.WaitForAnimationToStart(this, "Attack_Loop", false);
		AudioManager.PlayLoop("train_lollipop_ghoul_attack_loop");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_attack_loop");
		this.StartLightning();
		yield return base.StartCoroutine(this.head_cr());
		this.EndLightning();
		AudioManager.Stop("train_lollipop_ghoul_attack_loop");
		AudioManager.Play("train_lollipop_ghoul_attack_end");
		yield return null;
		base.animator.SetTrigger("Continue");
		this.state = TrainLevelLollipopGhoul.State.Ready;
		yield break;
	}

	// Token: 0x0600303A RID: 12346 RVA: 0x001C7110 File Offset: 0x001C5510
	private IEnumerator head_cr()
	{
		float t = 0f;
		float time = base.properties.CurrentState.lollipopGhouls.moveTime;
		EaseUtils.EaseType ease = EaseUtils.EaseType.easeInOutSine;
		Vector3 start = Vector3.zero;
		Vector3 end = new Vector3(base.properties.CurrentState.lollipopGhouls.moveDistance, 0f, 0f);
		this.head.localPosition = start;
		while (t < time)
		{
			float val = EaseUtils.Ease(ease, 0f, 1f, t / time);
			this.head.localPosition = Vector3.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.head.localPosition = end;
		t = 0f;
		while (t < time)
		{
			float val2 = EaseUtils.Ease(ease, 0f, 1f, t / time);
			this.head.localPosition = Vector3.Lerp(end, start, val2);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.head.localPosition = start;
		yield break;
	}

	// Token: 0x0600303B RID: 12347 RVA: 0x001C712C File Offset: 0x001C552C
	private IEnumerator die_cr()
	{
		AudioManager.Stop("train_lollipop_ghoul_attack_loop");
		AudioManager.Play("train_lollipop_ghoul_die");
		this.emitAudioFromObject.Add("train_lollipop_ghoul_die");
		this.state = TrainLevelLollipopGhoul.State.Dead;
		yield return CupheadTime.WaitForSeconds(this, 0.3f);
		if (this.currentLightning != null)
		{
			this.EndLightning();
		}
		base.animator.Play("Die");
		yield break;
	}

	// Token: 0x0600303C RID: 12348 RVA: 0x001C7147 File Offset: 0x001C5547
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.lightningPrefab = null;
	}

	// Token: 0x040038F3 RID: 14579
	[SerializeField]
	private Transform head;

	// Token: 0x040038F4 RID: 14580
	[SerializeField]
	private Transform lightningRoot;

	// Token: 0x040038F5 RID: 14581
	[Space(10f)]
	[SerializeField]
	private TrainLevelLollipopGhoulLightning lightningPrefab;

	// Token: 0x040038F7 RID: 14583
	private float health;

	// Token: 0x040038F8 RID: 14584
	private DamageReceiver damageReceiver;

	// Token: 0x040038F9 RID: 14585
	private TrainLevelLollipopGhoulLightning currentLightning;

	// Token: 0x0200081D RID: 2077
	public enum State
	{
		// Token: 0x040038FD RID: 14589
		Init,
		// Token: 0x040038FE RID: 14590
		Ready,
		// Token: 0x040038FF RID: 14591
		Attacking,
		// Token: 0x04003900 RID: 14592
		Dead
	}

	// Token: 0x0200081E RID: 2078
	// (Invoke) Token: 0x0600303E RID: 12350
	public delegate void OnDamageTakenHandler(float damage);
}
