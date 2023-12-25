using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008C5 RID: 2245
public class HarbourPlatformingLevelClam : PlatformingLevelShootingEnemy
{
	// Token: 0x0600346F RID: 13423 RVA: 0x001E7146 File Offset: 0x001E5546
	protected override void Start()
	{
		base.Start();
		this.startPos = base.transform.position;
	}

	// Token: 0x06003470 RID: 13424 RVA: 0x001E715F File Offset: 0x001E555F
	protected override void StartShoot()
	{
		if (this.counter < base.Properties.ClamShotCount)
		{
			this.counter++;
			base.StartShoot();
			this.AttackSFX();
		}
	}

	// Token: 0x06003471 RID: 13425 RVA: 0x001E7194 File Offset: 0x001E5594
	protected override void Update()
	{
		if (!this.startClam && this.octopus != null && this.octopus.Started())
		{
			this.Popup();
			this.startClam = true;
		}
		if (this._target != null)
		{
			if (!this.startClam && this.octopus == null)
			{
				this.dist = this._target.transform.position.x - this.onTrigger.transform.position.x;
				if (this.dist > 0f && !this.startClam)
				{
					this.Popup();
					this.startClam = true;
				}
			}
			else
			{
				this.dist = this._target.transform.position.x - this.offTrigger.transform.position.x;
				if (this.dist > 0f && !this.endClam)
				{
					this.startClam = false;
					this.endClam = true;
				}
			}
		}
	}

	// Token: 0x06003472 RID: 13426 RVA: 0x001E72CB File Offset: 0x001E56CB
	private void Popup()
	{
		base.animator.SetTrigger("OnPopup");
		base.transform.parent = CupheadLevelCamera.Current.transform;
		base.StartCoroutine(this.pop_up_cr());
	}

	// Token: 0x06003473 RID: 13427 RVA: 0x001E7300 File Offset: 0x001E5700
	private IEnumerator pop_up_cr()
	{
		for (;;)
		{
			if (!this.isDead)
			{
				EaseUtils.EaseType ease = EaseUtils.EaseType.easeInOutSine;
				float t = 0f;
				float time = base.Properties.ClamTimeSpeedUp;
				float startY = this.startPos.y;
				float endY = base.Properties.ClamMaxPointRange.RandomFloat();
				base.transform.SetPosition(new float?(CupheadLevelCamera.Current.Bounds.xMin + this.offset), null, null);
				this.Show();
				while (t < time)
				{
					float val = t / time;
					base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, startY, endY, val)), null);
					t += CupheadTime.Delta;
					yield return null;
				}
				base.animator.SetTrigger("OnSlowdown");
				base.transform.SetPosition(null, new float?(endY), null);
				t = 0f;
				time = base.Properties.ClamTimeSpeedDown;
				yield return null;
				while (t < time)
				{
					float val2 = t / time;
					base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, endY, startY, val2)), null);
					t += CupheadTime.Delta;
					yield return null;
				}
				this.Hide();
				base.transform.SetPosition(null, new float?(startY), null);
			}
			yield return CupheadTime.WaitForSeconds(this, base.Properties.ProjectileDelay.RandomFloat());
			if (!this.endClam)
			{
				this.isDead = false;
			}
			else
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003474 RID: 13428 RVA: 0x001E731C File Offset: 0x001E571C
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.offTrigger.transform.position, new Vector3(this.offTrigger.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.onTrigger.transform.position, new Vector3(this.onTrigger.transform.position.x, 5000f, 0f));
	}

	// Token: 0x06003475 RID: 13429 RVA: 0x001E73C8 File Offset: 0x001E57C8
	protected override void Die()
	{
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<DamageReceiver>().enabled = false;
		base.animator.Play("Off");
		this.DeathParts();
		this.isDead = true;
		this.StopAllCoroutines();
		base.StartCoroutine(this.fall_cr());
		base.Explode();
	}

	// Token: 0x06003476 RID: 13430 RVA: 0x001E7424 File Offset: 0x001E5824
	private IEnumerator fall_cr()
	{
		Vector2 velocity = new Vector2(0f, 200f);
		float accumulatedGravity = 0f;
		float speed = 400f;
		while (base.transform.position.y > CupheadLevelCamera.Current.Bounds.yMin - 100f)
		{
			base.transform.position += (velocity + new Vector2(-speed, accumulatedGravity)) * Time.fixedDeltaTime;
			accumulatedGravity += -100f;
			yield return null;
		}
		yield return CupheadTime.WaitForSeconds(this, base.Properties.ClamDespawnDelayRange.RandomFloat());
		base.transform.SetPosition(new float?(CupheadLevelCamera.Current.Bounds.xMin + this.offset), new float?(this.startPos.y), null);
		this.Hide();
		yield return null;
		yield break;
	}

	// Token: 0x06003477 RID: 13431 RVA: 0x001E7440 File Offset: 0x001E5840
	private void Hide()
	{
		this.counter = 0;
		base.animator.Play("Off");
		base.GetComponent<Collider2D>().enabled = false;
		base.GetComponent<DamageReceiver>().enabled = false;
		this.OnStart();
		if (this.isDead)
		{
			this.Popup();
		}
	}

	// Token: 0x06003478 RID: 13432 RVA: 0x001E7493 File Offset: 0x001E5893
	private void Show()
	{
		base.animator.SetTrigger("OnPopup");
		base.GetComponent<Collider2D>().enabled = true;
		base.GetComponent<DamageReceiver>().enabled = true;
	}

	// Token: 0x06003479 RID: 13433 RVA: 0x001E74C0 File Offset: 0x001E58C0
	private void DeathParts()
	{
		foreach (SpriteDeathParts spriteDeathParts in this.deathParts)
		{
			spriteDeathParts.CreatePart(base.transform.position);
		}
	}

	// Token: 0x0600347A RID: 13434 RVA: 0x001E74FE File Offset: 0x001E58FE
	private void AttackSFX()
	{
		AudioManager.Play("harbour_clam_attack");
		this.emitAudioFromObject.Add("harbour_clam_attack");
	}

	// Token: 0x0600347B RID: 13435 RVA: 0x001E751A File Offset: 0x001E591A
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.octopus = null;
		this.deathParts = null;
	}

	// Token: 0x04003C98 RID: 15512
	private const float GRAVITY = -100f;

	// Token: 0x04003C99 RID: 15513
	[SerializeField]
	private SpriteDeathParts[] deathParts;

	// Token: 0x04003C9A RID: 15514
	[SerializeField]
	private Transform main;

	// Token: 0x04003C9B RID: 15515
	[SerializeField]
	private Transform onTrigger;

	// Token: 0x04003C9C RID: 15516
	[SerializeField]
	private Transform offTrigger;

	// Token: 0x04003C9D RID: 15517
	[SerializeField]
	private HarbourPlatformingLevelOctopus octopus;

	// Token: 0x04003C9E RID: 15518
	private bool startClam;

	// Token: 0x04003C9F RID: 15519
	private bool endClam;

	// Token: 0x04003CA0 RID: 15520
	private bool isDead;

	// Token: 0x04003CA1 RID: 15521
	private float dist = -1000f;

	// Token: 0x04003CA2 RID: 15522
	private float offset = 100f;

	// Token: 0x04003CA3 RID: 15523
	private int counter;

	// Token: 0x04003CA4 RID: 15524
	private Vector3 startPos;
}
