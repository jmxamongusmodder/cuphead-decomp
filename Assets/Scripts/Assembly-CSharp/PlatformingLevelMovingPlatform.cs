using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000906 RID: 2310
public class PlatformingLevelMovingPlatform : AbstractPausableComponent
{
	// Token: 0x17000464 RID: 1124
	// (get) Token: 0x06003630 RID: 13872 RVA: 0x001F74A4 File Offset: 0x001F58A4
	// (set) Token: 0x06003631 RID: 13873 RVA: 0x001F74AC File Offset: 0x001F58AC
	private protected float[] allValues { protected get; private set; }

	// Token: 0x06003632 RID: 13874 RVA: 0x001F74B8 File Offset: 0x001F58B8
	protected virtual void Start()
	{
		this._offset = base.transform.position;
		base.StartCoroutine(this.pingpong_cr());
		AudioManager.PlayLoop("level_platform_propellor_loop");
		this.emitAudioFromObject.Add("level_platform_propellor_loop");
		base.StartCoroutine(this.check_sound_cr());
	}

	// Token: 0x06003633 RID: 13875 RVA: 0x001F750C File Offset: 0x001F590C
	private IEnumerator check_sound_cr()
	{
		bool inRange = false;
		for (;;)
		{
			if (CupheadLevelCamera.Current.ContainsPoint(base.transform.position, new Vector2(100f, 1000f)))
			{
				if (!inRange)
				{
					AudioManager.PlayLoop("level_platform_propellor_loop");
					this.emitAudioFromObject.Add("level_platform_propellor_loop");
					inRange = true;
				}
			}
			else if (inRange)
			{
				AudioManager.Stop("level_platform_propellor_loop");
				inRange = false;
			}
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003634 RID: 13876 RVA: 0x001F7528 File Offset: 0x001F5928
	private float CalculateTime()
	{
		return this.path.Distance / this.speed;
	}

	// Token: 0x06003635 RID: 13877 RVA: 0x001F754C File Offset: 0x001F594C
	private float CalculateRemainingTime(float t)
	{
		float num = this.CalculateTime();
		return (!this.goingUp) ? (t * num) : ((1f - t) * num);
	}

	// Token: 0x06003636 RID: 13878 RVA: 0x001F757C File Offset: 0x001F597C
	private void MoveCallback(float value)
	{
		base.transform.position = this._offset + this.path.Lerp(value);
	}

	// Token: 0x06003637 RID: 13879 RVA: 0x001F75A0 File Offset: 0x001F59A0
	protected virtual IEnumerator pingpong_cr()
	{
		for (;;)
		{
			if (this.goingUp)
			{
				yield return base.TweenValue(0f, 1f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			}
			else
			{
				yield return base.TweenValue(1f, 0f, this.CalculateTime(), this._easeType, new AbstractMonoBehaviour.TweenUpdateHandler(this.MoveCallback));
			}
			yield return CupheadTime.WaitForSeconds(this, this.loopRepeatDelay);
			this.goingUp = !this.goingUp;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06003638 RID: 13880 RVA: 0x001F75BB File Offset: 0x001F59BB
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003639 RID: 13881 RVA: 0x001F75CE File Offset: 0x001F59CE
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x0600363A RID: 13882 RVA: 0x001F75E4 File Offset: 0x001F59E4
	private void DrawGizmos(float a)
	{
		if (Application.isPlaying)
		{
			this.path.DrawGizmos(a, this._offset);
			return;
		}
		this.path.DrawGizmos(a, base.baseTransform.position);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawSphere(this.path.Lerp(0f) + base.baseTransform.position, 10f);
		Gizmos.DrawWireSphere(this.path.Lerp(0f) + base.baseTransform.position, 11f);
	}

	// Token: 0x04003E2C RID: 15916
	private const float ON_SCREEN_SOUND_PADDING = 100f;

	// Token: 0x04003E2E RID: 15918
	protected int pathIndex;

	// Token: 0x04003E2F RID: 15919
	public float loopRepeatDelay;

	// Token: 0x04003E30 RID: 15920
	public float speed = 100f;

	// Token: 0x04003E31 RID: 15921
	public VectorPath path;

	// Token: 0x04003E32 RID: 15922
	public bool goingUp;

	// Token: 0x04003E33 RID: 15923
	public SpriteRenderer sprite;

	// Token: 0x04003E34 RID: 15924
	private EaseUtils.EaseType _easeType = EaseUtils.EaseType.linear;

	// Token: 0x04003E35 RID: 15925
	protected Vector3 _offset;
}
