using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;

// Token: 0x02000490 RID: 1168
public class LevelHUDWeapon : AbstractMonoBehaviour
{
	// Token: 0x0600125E RID: 4702 RVA: 0x000A9F38 File Offset: 0x000A8338
	public LevelHUDWeapon Create(Transform parent, Weapon weapon)
	{
		LevelHUDWeapon levelHUDWeapon = this.InstantiatePrefab<LevelHUDWeapon>();
		levelHUDWeapon.transform.SetParent(parent, false);
		levelHUDWeapon.SetIcon(weapon);
		return levelHUDWeapon;
	}

	// Token: 0x14000030 RID: 48
	// (add) Token: 0x0600125F RID: 4703 RVA: 0x000A9F64 File Offset: 0x000A8364
	// (remove) Token: 0x06001260 RID: 4704 RVA: 0x000A9F98 File Offset: 0x000A8398
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	private static event Action OnAwakeEvent;

	// Token: 0x06001261 RID: 4705 RVA: 0x000A9FCC File Offset: 0x000A83CC
	protected override void Awake()
	{
		base.Awake();
		this.startY = -80f;
		this.endY = -10f;
		base.transform.ResetLocalTransforms();
		base.transform.SetLocalPosition(new float?(0f), new float?(this.startY), new float?(0f));
		this.inCoroutine = base.StartCoroutine(this.go_cr());
		if (LevelHUDWeapon.OnAwakeEvent != null)
		{
			LevelHUDWeapon.OnAwakeEvent();
		}
		LevelHUDWeapon.OnAwakeEvent = null;
		LevelHUDWeapon.OnAwakeEvent += this.Out;
	}

	// Token: 0x06001262 RID: 4706 RVA: 0x000AA067 File Offset: 0x000A8467
	private void OnDestroy()
	{
		LevelHUDWeapon.OnAwakeEvent -= this.Out;
	}

	// Token: 0x06001263 RID: 4707 RVA: 0x000AA07A File Offset: 0x000A847A
	private void Out()
	{
		if (!this.ending)
		{
			if (this.inCoroutine != null)
			{
				base.StopCoroutine(this.inCoroutine);
			}
			base.StartCoroutine(this.out_cr());
		}
	}

	// Token: 0x06001264 RID: 4708 RVA: 0x000AA0AB File Offset: 0x000A84AB
	private void SetIcon(Weapon weapon)
	{
		base.animator.Play(weapon.ToString());
	}

	// Token: 0x06001265 RID: 4709 RVA: 0x000AA0C8 File Offset: 0x000A84C8
	private IEnumerator go_cr()
	{
		yield return base.TweenLocalPositionY(this.startY, this.endY, 0.2f, EaseUtils.EaseType.easeOutSine);
		yield return CupheadTime.WaitForSeconds(this, 2f);
		this.ending = true;
		yield return base.TweenLocalPositionY(this.endY, this.startY, 0.2f, EaseUtils.EaseType.easeInSine);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06001266 RID: 4710 RVA: 0x000AA0E4 File Offset: 0x000A84E4
	private IEnumerator out_cr()
	{
		this.ending = true;
		yield return base.TweenLocalPositionY(this.endY, this.startY, 0.05f, EaseUtils.EaseType.easeInSine);
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04001BCC RID: 7116
	public const float TIME_IN = 0.2f;

	// Token: 0x04001BCD RID: 7117
	public const float TIME_DELAY = 2f;

	// Token: 0x04001BCE RID: 7118
	public const float TIME_OUT = 0.2f;

	// Token: 0x04001BCF RID: 7119
	public const float TIME_OUT_FAST = 0.05f;

	// Token: 0x04001BD0 RID: 7120
	private bool ending;

	// Token: 0x04001BD1 RID: 7121
	private Coroutine inCoroutine;

	// Token: 0x04001BD2 RID: 7122
	private float startY;

	// Token: 0x04001BD3 RID: 7123
	private float endY;
}
