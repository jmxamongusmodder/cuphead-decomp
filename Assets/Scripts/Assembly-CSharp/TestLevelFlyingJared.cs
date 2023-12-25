using System;
using System.Collections;
using UnityEngine;

// Token: 0x020004AC RID: 1196
public class TestLevelFlyingJared : LevelProperties.Test.Entity
{
	// Token: 0x0600137E RID: 4990 RVA: 0x000ABA00 File Offset: 0x000A9E00
	public override void LevelInit(LevelProperties.Test properties)
	{
		base.LevelInit(properties);
		this.damageReceiver = base.GetComponent<DamageReceiver>();
		this.damageReceiver.OnDamageTaken += this.OnDamageTaken;
		Level.Current.OnLevelStartEvent += this.OnLevelStart;
		AudioManager.PlayLoop("test_sound");
		this.emitAudioFromObject.Add("test_sound");
	}

	// Token: 0x0600137F RID: 4991 RVA: 0x000ABA67 File Offset: 0x000A9E67
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		base.properties.DealDamage(info.damage);
		base.GetComponent<AudioWarble>().HandleWarble();
	}

	// Token: 0x06001380 RID: 4992 RVA: 0x000ABA85 File Offset: 0x000A9E85
	private void OnLevelStart()
	{
		base.StartCoroutine(this.moveX_cr());
		base.StartCoroutine(this.moveY_cr());
		base.StartCoroutine(this.scale_cr());
	}

	// Token: 0x06001381 RID: 4993 RVA: 0x000ABAB0 File Offset: 0x000A9EB0
	private float GetHealthTimeX()
	{
		float i = 1f - base.properties.CurrentHealth / base.properties.TotalHealth;
		return base.properties.CurrentState.moving.timeX.GetFloatAt(i);
	}

	// Token: 0x06001382 RID: 4994 RVA: 0x000ABAF8 File Offset: 0x000A9EF8
	private float GetHealthTimeY()
	{
		float i = 1f - base.properties.CurrentHealth / base.properties.TotalHealth;
		return base.properties.CurrentState.moving.timeY.GetFloatAt(i);
	}

	// Token: 0x06001383 RID: 4995 RVA: 0x000ABB40 File Offset: 0x000A9F40
	private float GetHealthTimeScale()
	{
		float i = 1f - base.properties.CurrentHealth / base.properties.TotalHealth;
		return base.properties.CurrentState.moving.timeScale.GetFloatAt(i);
	}

	// Token: 0x06001384 RID: 4996 RVA: 0x000ABB88 File Offset: 0x000A9F88
	private IEnumerator moveX_cr()
	{
		float start = base.transform.position.x;
		float end = -start;
		for (;;)
		{
			this.childSprite.transform.SetScale(new float?(1f), null, null);
			yield return base.TweenLocalPositionX(start, end, this.GetHealthTimeX(), EaseUtils.EaseType.easeInOutSine);
			this.childSprite.transform.SetScale(new float?(-1f), null, null);
			yield return base.TweenLocalPositionX(end, start, this.GetHealthTimeX(), EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06001385 RID: 4997 RVA: 0x000ABBA4 File Offset: 0x000A9FA4
	private IEnumerator moveY_cr()
	{
		float start = base.transform.position.y;
		float end = start - 100f;
		for (;;)
		{
			yield return base.TweenLocalPositionY(start, end, this.GetHealthTimeY(), EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenLocalPositionY(end, start, this.GetHealthTimeY(), EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x06001386 RID: 4998 RVA: 0x000ABBC0 File Offset: 0x000A9FC0
	private IEnumerator scale_cr()
	{
		Vector3 start = new Vector3(1f, 1f, 1f);
		Vector3 end = new Vector3(2f, 2f, 2f);
		for (;;)
		{
			yield return base.TweenScale(start, end, this.GetHealthTimeScale(), EaseUtils.EaseType.easeInOutSine);
			yield return base.TweenScale(end, start, this.GetHealthTimeScale(), EaseUtils.EaseType.easeInOutSine);
		}
		yield break;
	}

	// Token: 0x04001C94 RID: 7316
	[SerializeField]
	private Transform childSprite;

	// Token: 0x04001C95 RID: 7317
	private DamageReceiver damageReceiver;

	// Token: 0x04001C96 RID: 7318
	[SerializeField]
	private AudioSource audioClip;
}
