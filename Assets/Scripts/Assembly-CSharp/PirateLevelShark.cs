using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000725 RID: 1829
public class PirateLevelShark : LevelProperties.Pirate.Entity
{
	// Token: 0x170003D1 RID: 977
	// (get) Token: 0x060027D2 RID: 10194 RVA: 0x00174B29 File Offset: 0x00172F29
	// (set) Token: 0x060027D3 RID: 10195 RVA: 0x00174B31 File Offset: 0x00172F31
	public PirateLevelShark.State state { get; private set; }

	// Token: 0x060027D4 RID: 10196 RVA: 0x00174B3C File Offset: 0x00172F3C
	protected override void Awake()
	{
		base.Awake();
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
		this.shark.SetActive(false);
		this.shark.transform.SetLocalPosition(new float?(-950f), null, null);
		this.splash.SetActive(false);
	}

	// Token: 0x060027D5 RID: 10197 RVA: 0x00174BAC File Offset: 0x00172FAC
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		if (this.state != PirateLevelShark.State.Exit && this.state != PirateLevelShark.State.Exit_Shot)
		{
			return;
		}
		if (this.shotCoroutine != null)
		{
			base.StopCoroutine(this.shotCoroutine);
		}
		this.shotCoroutine = this.shot_cr();
		base.StartCoroutine(this.shotCoroutine);
	}

	// Token: 0x060027D6 RID: 10198 RVA: 0x00174C02 File Offset: 0x00173002
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x060027D7 RID: 10199 RVA: 0x00174C20 File Offset: 0x00173020
	public override void LevelInitWithGroup(AbstractLevelPropertyGroup propertyGroup)
	{
		base.LevelInitWithGroup(propertyGroup);
		this.sharkProperties = (propertyGroup as LevelProperties.Pirate.Shark);
		base.StartCoroutine(this.shark_cr());
		base.StartCoroutine(this.collider_cr());
		this.state = PirateLevelShark.State.Swim;
		this.damageDealer = new DamageDealer(1f, 1f);
		this.damageDealer.SetDirection(DamageDealer.Direction.Right, base.transform);
		Vector3 position = base.transform.position;
		position.x = this.sharkProperties.x;
		base.transform.position = position;
	}

	// Token: 0x060027D8 RID: 10200 RVA: 0x00174CB2 File Offset: 0x001730B2
	private void OnBiteAnimComplete()
	{
		this.state = PirateLevelShark.State.Exit;
		base.StartCoroutine(this.exit_cr());
	}

	// Token: 0x060027D9 RID: 10201 RVA: 0x00174CC8 File Offset: 0x001730C8
	private void OnBiteAudio()
	{
	}

	// Token: 0x060027DA RID: 10202 RVA: 0x00174CCA File Offset: 0x001730CA
	private void OnBiteShake()
	{
		CupheadLevelCamera.Current.Shake(12f, 0.5f, false);
	}

	// Token: 0x060027DB RID: 10203 RVA: 0x00174CE1 File Offset: 0x001730E1
	private void Splash()
	{
		this.splash.SetActive(true);
		base.animator.Play("Splash", 3);
	}

	// Token: 0x060027DC RID: 10204 RVA: 0x00174D00 File Offset: 0x00173100
	private void End()
	{
		AudioManager.Stop("level_pirate_shark_exit_normal_loop");
		this.state = PirateLevelShark.State.Complete;
		this.StopAllCoroutines();
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x060027DD RID: 10205 RVA: 0x00174D24 File Offset: 0x00173124
	private IEnumerator shot_cr()
	{
		this.state = PirateLevelShark.State.Exit_Shot;
		base.animator.SetLayerWeight(1, 1f);
		float t = 0f;
		while (t < 1f)
		{
			t += CupheadTime.Delta;
			yield return null;
		}
		base.animator.SetLayerWeight(1, 0f);
		this.state = PirateLevelShark.State.Exit;
		yield break;
	}

	// Token: 0x060027DE RID: 10206 RVA: 0x00174D40 File Offset: 0x00173140
	private IEnumerator shark_cr()
	{
		AudioManager.Play("level_pirate_shark_warning");
		yield return base.StartCoroutine(this.fin_cr());
		this.shark.SetActive(true);
		this.state = PirateLevelShark.State.Attack;
		base.animator.Play("Attack");
		AudioManager.Play("levels_pirate_shark_attack");
		this.emitAudioFromObject.Add("levels_pirate_shark_attack");
		yield break;
	}

	// Token: 0x060027DF RID: 10207 RVA: 0x00174D5C File Offset: 0x0017315C
	private IEnumerator exit_cr()
	{
		base.animator.Play("Exit");
		AudioManager.PlayLoop("level_pirate_shark_exit_normal_loop");
		this.emitAudioFromObject.Add("level_pirate_shark_exit_normal_loop");
		base.animator.Play("Exit", 1);
		for (;;)
		{
			if (this.shark.transform.position.x < -950f)
			{
				this.End();
			}
			float speed = ((this.state != PirateLevelShark.State.Exit) ? this.sharkProperties.shotExitSpeed : this.sharkProperties.exitSpeed) * CupheadTime.Delta;
			base.transform.AddPosition(-speed, 0f, 0f);
			yield return null;
		}
		yield break;
	}

	// Token: 0x060027E0 RID: 10208 RVA: 0x00174D78 File Offset: 0x00173178
	private IEnumerator fin_cr()
	{
		float t = 0f;
		float time = this.sharkProperties.finTime;
		int startX = 640;
		int endX = -740;
		while (t < time)
		{
			float val = t / time;
			float x = Mathf.Lerp((float)startX, (float)endX, val);
			this.fin.transform.SetPosition(new float?(x), null, null);
			t += CupheadTime.Delta;
			yield return null;
		}
		this.fin.transform.SetPosition(new float?((float)endX), null, null);
		yield return CupheadTime.WaitForSeconds(this, this.sharkProperties.attackDelay);
		yield break;
	}

	// Token: 0x060027E1 RID: 10209 RVA: 0x00174D94 File Offset: 0x00173194
	private IEnumerator collider_cr()
	{
		BoxCollider2D collider = base.GetComponent<Collider2D>() as BoxCollider2D;
		BoxCollider2D childCollider = this.shark.GetComponent<Collider2D>() as BoxCollider2D;
		for (;;)
		{
			collider.offset = this.shark.transform.localPosition + childCollider.offset;
			collider.size = childCollider.size;
			yield return null;
		}
		yield break;
	}

	// Token: 0x04003092 RID: 12434
	public const float SHOT_DELAY = 1f;

	// Token: 0x04003093 RID: 12435
	public const float START_X = -950f;

	// Token: 0x04003095 RID: 12437
	[SerializeField]
	private GameObject fin;

	// Token: 0x04003096 RID: 12438
	[SerializeField]
	private GameObject shark;

	// Token: 0x04003097 RID: 12439
	[SerializeField]
	private GameObject splash;

	// Token: 0x04003098 RID: 12440
	private LevelProperties.Pirate.Shark sharkProperties;

	// Token: 0x04003099 RID: 12441
	private DamageDealer damageDealer;

	// Token: 0x0400309A RID: 12442
	private IEnumerator shotCoroutine;

	// Token: 0x02000726 RID: 1830
	public enum State
	{
		// Token: 0x0400309C RID: 12444
		Init,
		// Token: 0x0400309D RID: 12445
		Swim,
		// Token: 0x0400309E RID: 12446
		Attack,
		// Token: 0x0400309F RID: 12447
		Exit,
		// Token: 0x040030A0 RID: 12448
		Exit_Shot,
		// Token: 0x040030A1 RID: 12449
		Complete
	}
}
