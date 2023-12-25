using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200060E RID: 1550
public class FlowerLevelPlatform : LevelPlatform
{
	// Token: 0x06001F43 RID: 8003 RVA: 0x0011F294 File Offset: 0x0011D694
	private void Start()
	{
		this.YPositionDown = this.YPositionUp - 30f;
		this.YFall = this.YPositionUp - 35f;
		if (this.shadow != null)
		{
			this.shadow.parent = null;
			Vector3 position = this.shadow.position;
			position.y = (float)Level.Current.Ground;
			this.shadow.position = position;
		}
		this.startPos = base.transform.position;
		this.startPos.y = this.YPositionUp;
		this.endPos = base.transform.position;
		this.endPos.y = this.YPositionDown;
		if (this.state == FlowerLevelPlatform.State.Down)
		{
			base.transform.SetPosition(null, new float?(this.YPositionUp), null);
			this.StartDown();
		}
		else
		{
			base.transform.SetPosition(null, new float?(this.YPositionDown), null);
			this.StartUp();
		}
	}

	// Token: 0x06001F44 RID: 8004 RVA: 0x0011F3BD File Offset: 0x0011D7BD
	public void StartDown()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.down_cr());
	}

	// Token: 0x06001F45 RID: 8005 RVA: 0x0011F3D2 File Offset: 0x0011D7D2
	public void StartUp()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.up_cr());
	}

	// Token: 0x06001F46 RID: 8006 RVA: 0x0011F3E7 File Offset: 0x0011D7E7
	public override void AddChild(Transform player)
	{
		base.AddChild(player);
		this.StopAllCoroutines();
		base.StartCoroutine(this.fall_cr());
	}

	// Token: 0x06001F47 RID: 8007 RVA: 0x0011F403 File Offset: 0x0011D803
	public override void OnPlayerExit(Transform player)
	{
		base.OnPlayerExit(player);
		this.StartUp();
	}

	// Token: 0x06001F48 RID: 8008 RVA: 0x0011F414 File Offset: 0x0011D814
	private IEnumerator down_cr()
	{
		yield return new WaitForSeconds(0f);
		yield return base.StartCoroutine(this.goTo_cr(this.YPositionUp, this.YPositionDown, 3f, EaseUtils.EaseType.easeInOutSine));
		this.StartUp();
		yield break;
	}

	// Token: 0x06001F49 RID: 8009 RVA: 0x0011F430 File Offset: 0x0011D830
	private IEnumerator up_cr()
	{
		yield return new WaitForSeconds(0f);
		yield return base.StartCoroutine(this.goTo_cr(this.YPositionDown, this.YPositionUp, 3f, EaseUtils.EaseType.easeInOutSine));
		this.StartDown();
		yield break;
	}

	// Token: 0x06001F4A RID: 8010 RVA: 0x0011F44C File Offset: 0x0011D84C
	private IEnumerator fall_cr()
	{
		float time = (1f - base.transform.position.y / this.YPositionDown) * 0.13f;
		yield return base.StartCoroutine(this.goTo_cr(base.transform.position.y, this.YFall, time, EaseUtils.EaseType.easeOutSine));
		yield return base.StartCoroutine(this.goTo_cr(this.YFall, this.YPositionDown, 0.12f, EaseUtils.EaseType.easeInOutSine));
		yield break;
	}

	// Token: 0x06001F4B RID: 8011 RVA: 0x0011F468 File Offset: 0x0011D868
	private IEnumerator goTo_cr(float start, float end, float time, EaseUtils.EaseType ease)
	{
		float t = 0f;
		base.transform.SetPosition(null, new float?(start), null);
		while (t < time)
		{
			float val = t / time;
			base.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			t += Time.deltaTime;
			yield return base.StartCoroutine(base.WaitForPause_CR());
		}
		base.transform.SetPosition(null, new float?(end), null);
		yield break;
	}

	// Token: 0x040027D9 RID: 10201
	public float YPositionUp;

	// Token: 0x040027DA RID: 10202
	public const float TIME = 3f;

	// Token: 0x040027DB RID: 10203
	public const float FALL_TIME = 0.13f;

	// Token: 0x040027DC RID: 10204
	public const float FALL_BOUNCE_TIME = 0.12f;

	// Token: 0x040027DD RID: 10205
	public const float DELAY = 0f;

	// Token: 0x040027DE RID: 10206
	public const EaseUtils.EaseType FLOAT_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x040027DF RID: 10207
	public const EaseUtils.EaseType FALL_EASE = EaseUtils.EaseType.easeOutSine;

	// Token: 0x040027E0 RID: 10208
	public const EaseUtils.EaseType FALL_BOUNCE_EASE = EaseUtils.EaseType.easeInOutSine;

	// Token: 0x040027E1 RID: 10209
	[SerializeField]
	private FlowerLevelPlatform.State state;

	// Token: 0x040027E2 RID: 10210
	[SerializeField]
	private Transform shadow;

	// Token: 0x040027E3 RID: 10211
	private Vector3 startPos;

	// Token: 0x040027E4 RID: 10212
	private Vector3 endPos;

	// Token: 0x040027E5 RID: 10213
	private float YPositionDown;

	// Token: 0x040027E6 RID: 10214
	private float YFall;

	// Token: 0x0200060F RID: 1551
	public enum State
	{
		// Token: 0x040027E8 RID: 10216
		Up,
		// Token: 0x040027E9 RID: 10217
		Down,
		// Token: 0x040027EA RID: 10218
		PlayerOn
	}
}
