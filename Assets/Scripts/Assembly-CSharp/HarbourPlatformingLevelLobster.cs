using System;
using System.Collections;
using UnityEngine;

// Token: 0x020008CD RID: 2253
public class HarbourPlatformingLevelLobster : PlatformingLevelShootingEnemy
{
	// Token: 0x060034A4 RID: 13476 RVA: 0x001E8D80 File Offset: 0x001E7180
	protected override void Start()
	{
		base.Start();
		this.startPositionY = base.transform.position.y;
		base.StartCoroutine(this.start_trigger_cr());
		this.previousY = base.transform.position.y;
		this.exploder = base.GetComponent<LevelBossDeathExploder>();
	}

	// Token: 0x060034A5 RID: 13477 RVA: 0x001E8DE0 File Offset: 0x001E71E0
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		Gizmos.color = new Color(0f, 0f, 1f, 1f);
		Gizmos.DrawLine(this.offTrigger.transform.position, new Vector3(this.offTrigger.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.onTrigger.transform.position, new Vector3(this.onTrigger.transform.position.x, 5000f, 0f));
		Gizmos.color = new Color(0f, 1f, 0f, 0.5f);
		Gizmos.DrawLine(this.leftBoundary.transform.position, new Vector3(this.leftBoundary.transform.position.x, 5000f, 0f));
		Gizmos.DrawLine(this.rightBoundary.transform.position, new Vector3(this.rightBoundary.transform.position.x, 5000f, 0f));
	}

	// Token: 0x060034A6 RID: 13478 RVA: 0x001E8F20 File Offset: 0x001E7320
	private IEnumerator attack_cr()
	{
		base.animator.SetTrigger("OnAttackStart");
		yield return base.animator.WaitForAnimationToStart(this, "Warning_Loop", false);
		yield return CupheadTime.WaitForSeconds(this, base.Properties.lobsterWarningTime);
		base.animator.SetTrigger("Attack");
		this.AttackSFX();
		yield return null;
		yield return base.animator.WaitForAnimationToStart(this, "Attack_Trans_Idle", false);
		yield break;
	}

	// Token: 0x060034A7 RID: 13479 RVA: 0x001E8F3C File Offset: 0x001E733C
	private IEnumerator start_trigger_cr()
	{
		while (this._target == null)
		{
			yield return null;
		}
		this.dist = this._target.transform.position.x - this.onTrigger.transform.position.x;
		while (this.dist < 0f)
		{
			this.dist = this._target.transform.position.x - this.onTrigger.transform.position.x;
			yield return null;
		}
		this.mainCoroutine = base.StartCoroutine(this.main_cr());
		this.dist = this._target.transform.position.x - this.offTrigger.transform.position.x;
		while (this.dist < 0f)
		{
			this.dist = this._target.transform.position.x - this.offTrigger.transform.position.x;
			yield return null;
		}
		this.isGone = true;
		yield return null;
		yield break;
	}

	// Token: 0x060034A8 RID: 13480 RVA: 0x001E8F58 File Offset: 0x001E7358
	private IEnumerator main_cr()
	{
		while (!this.isGone)
		{
			this.direction = ((this.direction != PlatformingLevelShootingEnemy.Direction.Right) ? PlatformingLevelShootingEnemy.Direction.Right : PlatformingLevelShootingEnemy.Direction.Left);
			base.transform.localScale = new Vector3((float)((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? 1 : -1), 1f, 1f);
			base.transform.SetPosition(new float?((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (CupheadLevelCamera.Current.Bounds.xMin - 350f) : (CupheadLevelCamera.Current.Bounds.xMax + 350f)), new float?(base.Properties.lobsterY), null);
			if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x > this.rightBoundary.position.x) : (base.transform.position.x < this.leftBoundary.position.x))
			{
				base.transform.SetPosition(null, new float?(-5000f), null);
				yield return CupheadTime.WaitForSeconds(this, base.Properties.lobsterOffscreenTime);
			}
			else
			{
				if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x < this.leftBoundary.position.x) : (base.transform.position.x > this.rightBoundary.position.x))
				{
					base.transform.SetPosition(new float?((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? this.leftBoundary.position.x : this.rightBoundary.position.x), null, null);
					yield return base.StartCoroutine(this.pop_up_cr());
				}
				else
				{
					base.animator.Play("Idle");
					this.IdleSFX();
					this.poppedUp = true;
				}
				while ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMax + -250f) : (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - -250f))
				{
					base.transform.AddPosition(base.Properties.lobsterSpeed * CupheadTime.Delta * (float)((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? 1 : -1), 0f, 0f);
					if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x > this.rightBoundary.position.x) : (base.transform.position.x < this.leftBoundary.position.x))
					{
						this.Popdown(false);
						yield break;
					}
					yield return null;
				}
				yield return base.StartCoroutine(this.attack_cr());
				while ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x < CupheadLevelCamera.Current.Bounds.xMax + 350f) : (base.transform.position.x > CupheadLevelCamera.Current.Bounds.xMin - 350f))
				{
					base.transform.AddPosition(base.Properties.lobsterSpeed * CupheadTime.Delta * (float)((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? 1 : -1), 0f, 0f);
					if ((this.direction != PlatformingLevelShootingEnemy.Direction.Left) ? (base.transform.position.x > this.rightBoundary.position.x) : (base.transform.position.x < this.leftBoundary.position.x))
					{
						this.Popdown(false);
						yield break;
					}
					yield return null;
				}
				base.transform.SetPosition(null, new float?(-5000f), null);
				yield return CupheadTime.WaitForSeconds(this, base.Properties.lobsterOffscreenTime);
			}
		}
		UnityEngine.Object.Destroy(this.main.gameObject);
		yield break;
		yield break;
	}

	// Token: 0x060034A9 RID: 13481 RVA: 0x001E8F73 File Offset: 0x001E7373
	private void Popup()
	{
		base.StartCoroutine(this.pop_up_cr());
	}

	// Token: 0x060034AA RID: 13482 RVA: 0x001E8F82 File Offset: 0x001E7382
	private void Popdown(bool dead)
	{
		AudioManager.Stop("harbour_lobster_idle");
		base.StartCoroutine(this.pop_down_cr(dead));
	}

	// Token: 0x060034AB RID: 13483 RVA: 0x001E8F9C File Offset: 0x001E739C
	private IEnumerator pop_up_cr()
	{
		base.animator.SetTrigger("OnEmerge");
		this.EmergeSFX();
		float t = 0f;
		float time = 0.6f;
		float endY = base.Properties.lobsterY;
		Vector2 end = new Vector2(base.transform.position.x, endY);
		while (t < time)
		{
			Vector2 start = base.transform.position;
			end = new Vector2(base.transform.position.x, endY);
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, end, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.transform.position = end;
		this.poppedUp = true;
		yield break;
	}

	// Token: 0x060034AC RID: 13484 RVA: 0x001E8FB8 File Offset: 0x001E73B8
	private IEnumerator pop_down_cr(bool dead)
	{
		if (!this.poppedUp)
		{
			yield break;
		}
		this.poppedUp = false;
		if (dead && this.mainCoroutine != null)
		{
			base.StopCoroutine(this.mainCoroutine);
		}
		if (this.isGone)
		{
			base.transform.parent = null;
			base.animator.SetTrigger("OnTuck");
		}
		else
		{
			base.animator.Play("Tuck");
			this.SinkSFX();
		}
		if (dead)
		{
			this.exploder.StartExplosion();
			yield return CupheadTime.WaitForSeconds(this, 1f);
			this.exploder.StopExplosions();
		}
		float t = 0f;
		float time = 1.5f;
		Vector2 start = base.transform.position;
		Vector2 end = new Vector2(base.transform.position.x, this.startPositionY);
		float splashDepth = -1200f;
		while (t < time)
		{
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInSine, 0f, 1f, t / time);
			base.transform.position = Vector2.Lerp(start, end, val);
			if (base.transform.position.y <= splashDepth && this.previousY > splashDepth)
			{
				GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(this.splashPrefab, new Vector3(base.transform.position.x, splashDepth, base.transform.position.z), Quaternion.identity);
				gameObject.transform.SetParent(null);
				this.delay_destroy_cr(gameObject, 10f);
			}
			t += CupheadTime.Delta;
			this.previousY = base.transform.position.y;
			yield return null;
		}
		base.transform.position = end;
		if (this.isGone)
		{
			UnityEngine.Object.Destroy(this.main.gameObject);
		}
		else
		{
			base.StartCoroutine(this.delay_cr(dead));
		}
		yield return null;
		yield break;
	}

	// Token: 0x060034AD RID: 13485 RVA: 0x001E8FDC File Offset: 0x001E73DC
	private IEnumerator delay_destroy_cr(GameObject o, float t)
	{
		yield return CupheadTime.WaitForSeconds(this, t);
		UnityEngine.Object.Destroy(o);
		yield break;
	}

	// Token: 0x060034AE RID: 13486 RVA: 0x001E9005 File Offset: 0x001E7405
	protected override void Die()
	{
		this.Popdown(true);
	}

	// Token: 0x060034AF RID: 13487 RVA: 0x001E9010 File Offset: 0x001E7410
	private IEnumerator delay_cr(bool dead)
	{
		yield return CupheadTime.WaitForSeconds(this, (!dead) ? base.Properties.lobsterOffscreenTime : base.Properties.lobsterTuckTime);
		if (this.isGone)
		{
			UnityEngine.Object.Destroy(this.main.gameObject);
			yield break;
		}
		base.Health = base.Properties.Health;
		this.mainCoroutine = base.StartCoroutine(this.main_cr());
		yield break;
	}

	// Token: 0x060034B0 RID: 13488 RVA: 0x001E9032 File Offset: 0x001E7432
	private void EmergeSFX()
	{
		AudioManager.Play("harbour_lobster_emerge");
		this.emitAudioFromObject.Add("harbour_lobster_emerge");
	}

	// Token: 0x060034B1 RID: 13489 RVA: 0x001E904E File Offset: 0x001E744E
	private void SinkSFX()
	{
		AudioManager.Stop("harbour_lobster_idle");
		AudioManager.Play("harbour_lobster_sink");
		this.emitAudioFromObject.Add("harbour_lobster_sink");
	}

	// Token: 0x060034B2 RID: 13490 RVA: 0x001E9074 File Offset: 0x001E7474
	private void AttackSFX()
	{
		AudioManager.Play("harbour_lobster_attack");
		this.emitAudioFromObject.Add("harbour_lobster_attack");
	}

	// Token: 0x060034B3 RID: 13491 RVA: 0x001E9090 File Offset: 0x001E7490
	private void IdleSFX()
	{
		AudioManager.PlayLoop("harbour_lobster_idle");
		this.emitAudioFromObject.Add("harbour_lobster_idle");
	}

	// Token: 0x060034B4 RID: 13492 RVA: 0x001E90AC File Offset: 0x001E74AC
	protected override void OnDestroy()
	{
		base.OnDestroy();
		this.splashPrefab = null;
	}

	// Token: 0x04003CD0 RID: 15568
	[SerializeField]
	private Transform main;

	// Token: 0x04003CD1 RID: 15569
	[SerializeField]
	private Transform onTrigger;

	// Token: 0x04003CD2 RID: 15570
	[SerializeField]
	private Transform offTrigger;

	// Token: 0x04003CD3 RID: 15571
	[SerializeField]
	private Transform leftBoundary;

	// Token: 0x04003CD4 RID: 15572
	[SerializeField]
	private Transform rightBoundary;

	// Token: 0x04003CD5 RID: 15573
	[SerializeField]
	private LevelBossDeathExploder exploder;

	// Token: 0x04003CD6 RID: 15574
	[SerializeField]
	private GameObject splashPrefab;

	// Token: 0x04003CD7 RID: 15575
	[SerializeField]
	private Transform splashTransform;

	// Token: 0x04003CD8 RID: 15576
	private bool poppedUp;

	// Token: 0x04003CD9 RID: 15577
	private bool isGone;

	// Token: 0x04003CDA RID: 15578
	private float dist = 1000f;

	// Token: 0x04003CDB RID: 15579
	private float startPositionY;

	// Token: 0x04003CDC RID: 15580
	private const float OffScreenPadding = 350f;

	// Token: 0x04003CDD RID: 15581
	private const float attackPadding = -250f;

	// Token: 0x04003CDE RID: 15582
	private Coroutine mainCoroutine;

	// Token: 0x04003CDF RID: 15583
	private float previousY;

	// Token: 0x04003CE0 RID: 15584
	private PlatformingLevelShootingEnemy.Direction direction = PlatformingLevelShootingEnemy.Direction.Right;
}
