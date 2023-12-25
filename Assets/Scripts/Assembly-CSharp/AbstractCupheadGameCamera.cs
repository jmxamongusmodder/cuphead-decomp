using System;
using System.Collections;
using System.Diagnostics;
using UnityEngine;
using UnityStandardAssets.ImageEffects;

// Token: 0x020003D2 RID: 978
public abstract class AbstractCupheadGameCamera : AbstractCupheadCamera
{
	// Token: 0x17000227 RID: 551
	// (get) Token: 0x06000CC0 RID: 3264 RVA: 0x0008953E File Offset: 0x0008793E
	// (set) Token: 0x06000CC1 RID: 3265 RVA: 0x00089546 File Offset: 0x00087946
	public float zoom
	{
		get
		{
			return this._zoom;
		}
		protected set
		{
			this._zoom = value;
			base.camera.orthographicSize = this.OrthographicSize / this._zoom;
		}
	}

	// Token: 0x17000228 RID: 552
	// (get) Token: 0x06000CC2 RID: 3266 RVA: 0x00089567 File Offset: 0x00087967
	// (set) Token: 0x06000CC3 RID: 3267 RVA: 0x0008956F File Offset: 0x0008796F
	public bool isShaking { get; private set; }

	// Token: 0x17000229 RID: 553
	// (get) Token: 0x06000CC4 RID: 3268
	public abstract float OrthographicSize { get; }

	// Token: 0x1700022A RID: 554
	// (get) Token: 0x06000CC5 RID: 3269 RVA: 0x00089578 File Offset: 0x00087978
	public float Width
	{
		get
		{
			return 1.7777778f * this.OrthographicSize * this.zoom;
		}
	}

	// Token: 0x1700022B RID: 555
	// (get) Token: 0x06000CC6 RID: 3270 RVA: 0x0008958D File Offset: 0x0008798D
	public float Height
	{
		get
		{
			return this.OrthographicSize * this.zoom;
		}
	}

	// Token: 0x1700022C RID: 556
	// (get) Token: 0x06000CC7 RID: 3271 RVA: 0x0008959C File Offset: 0x0008799C
	public float Top
	{
		get
		{
			return base.camera.ScreenToWorldPoint(new Vector3(0f, (float)Screen.height, 0f)).y;
		}
	}

	// Token: 0x06000CC8 RID: 3272 RVA: 0x000895D1 File Offset: 0x000879D1
	protected override void Awake()
	{
		base.Awake();
		base.camera.orthographicSize = this.OrthographicSize;
		this._blurEffect = base.GetComponent<BlurOptimized>();
		this._blurEffect.enabled = false;
		base.camera.clearFlags = CameraClearFlags.Color;
	}

	// Token: 0x06000CC9 RID: 3273 RVA: 0x0008960E File Offset: 0x00087A0E
	protected void Move()
	{
		base.transform.position = this._position + this._shakeAdd + this._floatAdd;
	}

	// Token: 0x14000025 RID: 37
	// (add) Token: 0x06000CCA RID: 3274 RVA: 0x00089638 File Offset: 0x00087A38
	// (remove) Token: 0x06000CCB RID: 3275 RVA: 0x00089670 File Offset: 0x00087A70
	[DebuggerBrowsable(DebuggerBrowsableState.Never)]
	public event AbstractCupheadGameCamera.OnShakeHandler OnShakeEvent;

	// Token: 0x06000CCC RID: 3276 RVA: 0x000896A8 File Offset: 0x00087AA8
	public void Shake(float amount, float time, bool bypassEvent = false)
	{
		this.isShaking = true;
		this.ResetShake();
		this.shakeAmount = amount;
		if (!bypassEvent && this.OnShakeEvent != null)
		{
			this.OnShakeEvent(amount, time);
		}
		this.shakeCoroutine = this.falloffShake_cr(amount, time);
		base.StartCoroutine(this.shakeCoroutine);
	}

	// Token: 0x06000CCD RID: 3277 RVA: 0x00089702 File Offset: 0x00087B02
	public void StartShake(float amount)
	{
		this.ResetShake();
		this.shakeAmount = amount;
		this.shakeCoroutine = this.endlessShake_cr(amount);
		base.StartCoroutine(this.shakeCoroutine);
	}

	// Token: 0x06000CCE RID: 3278 RVA: 0x0008972B File Offset: 0x00087B2B
	public void EndShake(float time)
	{
		this.ResetShake();
		if (time > 0f)
		{
			this.shakeCoroutine = this.falloffShake_cr(this.shakeAmount, time);
			base.StartCoroutine(this.shakeCoroutine);
		}
	}

	// Token: 0x06000CCF RID: 3279 RVA: 0x0008975E File Offset: 0x00087B5E
	public void StartSmoothShake(float amount, float period, int octaves)
	{
		this.ResetShake();
		this.shakeCoroutine = this.endlessSmoothShake_cr(amount, period, octaves);
		base.StartCoroutine(this.shakeCoroutine);
	}

	// Token: 0x06000CD0 RID: 3280 RVA: 0x00089782 File Offset: 0x00087B82
	public void ResetShake()
	{
		if (this.shakeCoroutine != null)
		{
			base.StopCoroutine(this.shakeCoroutine);
		}
		this.shakeCoroutine = null;
		this._shakeAdd = Vector3.zero;
	}

	// Token: 0x06000CD1 RID: 3281 RVA: 0x000897B0 File Offset: 0x00087BB0
	private IEnumerator falloffShake_cr(float amount, float time)
	{
		float t = 0f;
		float halfAmount = amount / 2f;
		while (t < time)
		{
			float val = 1f - t / time;
			this.shakeAmount = amount * val;
			float x = UnityEngine.Random.Range(-halfAmount, halfAmount);
			float y = UnityEngine.Random.Range(-halfAmount, halfAmount);
			this._shakeAdd = new Vector3(x * val, y * val, 0f);
			t += CupheadTime.Delta;
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		this.ResetShake();
		this.isShaking = false;
		yield break;
	}

	// Token: 0x06000CD2 RID: 3282 RVA: 0x000897DC File Offset: 0x00087BDC
	private IEnumerator endlessShake_cr(float amount)
	{
		float halfAmount = amount / 2f;
		for (;;)
		{
			float x = UnityEngine.Random.Range(-halfAmount, halfAmount);
			float y = UnityEngine.Random.Range(-halfAmount, halfAmount);
			this._shakeAdd = new Vector3(x, y, 0f);
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000CD3 RID: 3283 RVA: 0x00089800 File Offset: 0x00087C00
	private IEnumerator endlessSmoothShake_cr(float amount, float period, int octaves)
	{
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float x = 0f;
			float y = 0f;
			float scale = 1f;
			for (int i = 0; i < octaves; i++)
			{
				scale = Mathf.Pow(2f, (float)i);
				x += Mathf.PerlinNoise((t + 1000f) * scale / period, 0f) * amount / scale;
				y += Mathf.PerlinNoise((t + 2000f) * scale / period, 0f) * amount / scale;
			}
			this._shakeAdd.x = x;
			this._shakeAdd.y = y;
			this._shakeAdd.z = 0f;
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		yield break;
	}

	// Token: 0x06000CD4 RID: 3284 RVA: 0x00089830 File Offset: 0x00087C30
	public void StartFloat(float amount, float time)
	{
		this.ResetFloat();
		this.floatCoroutine = this.float_cr(amount, time);
		base.StartCoroutine(this.floatCoroutine);
	}

	// Token: 0x06000CD5 RID: 3285 RVA: 0x00089853 File Offset: 0x00087C53
	public void EndFloat()
	{
		this.ResetFloat();
	}

	// Token: 0x06000CD6 RID: 3286 RVA: 0x0008985B File Offset: 0x00087C5B
	public void ResetFloat()
	{
		if (this.floatCoroutine != null)
		{
			base.StopCoroutine(this.floatCoroutine);
		}
		this.floatCoroutine = null;
		this._floatAdd = Vector3.zero;
	}

	// Token: 0x06000CD7 RID: 3287 RVA: 0x00089886 File Offset: 0x00087C86
	public void SetManualFloat(Vector3 value)
	{
		this._floatAdd = value;
	}

	// Token: 0x06000CD8 RID: 3288 RVA: 0x00089890 File Offset: 0x00087C90
	private IEnumerator float_cr(float amount, float time)
	{
		this.floatState = AbstractCupheadGameCamera.FloatState.Float;
		float t = 0f;
		float bottom = 0f;
		for (;;)
		{
			t = 0f;
			while (t < time)
			{
				float val = t / time;
				float y = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, bottom, amount, val);
				this._floatAdd = new Vector3(0f, y, 0f);
				t += CupheadTime.Delta;
				yield return null;
			}
			t = 0f;
			while (t < time)
			{
				float val2 = t / time;
				float y2 = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, amount, bottom, val2);
				this._floatAdd = new Vector3(0f, y2, 0f);
				t += CupheadTime.Delta;
				yield return null;
			}
			if (this.floatState == AbstractCupheadGameCamera.FloatState.Stop)
			{
				this.ResetFloat();
			}
		}
		yield break;
	}

	// Token: 0x06000CD9 RID: 3289 RVA: 0x000898B9 File Offset: 0x00087CB9
	public void Zoom(float newZoom, float time, EaseUtils.EaseType ease)
	{
		this.StopZoom();
		this.zoomCoroutine = this.zoom_cr(newZoom, time, ease);
		base.StartCoroutine(this.zoomCoroutine);
	}

	// Token: 0x06000CDA RID: 3290 RVA: 0x000898DD File Offset: 0x00087CDD
	private void StopZoom()
	{
		if (this.zoomCoroutine != null)
		{
			base.StopCoroutine(this.zoomCoroutine);
		}
		this.zoomCoroutine = null;
	}

	// Token: 0x06000CDB RID: 3291 RVA: 0x00089900 File Offset: 0x00087D00
	private IEnumerator zoom_cr(float newZoom, float time, EaseUtils.EaseType ease)
	{
		float oldZoom = this.zoom;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.zoom = EaseUtils.Ease(ease, oldZoom, newZoom, val);
			t += CupheadTime.Delta;
			yield return null;
			if (PauseManager.state == PauseManager.State.Paused)
			{
				while (PauseManager.state == PauseManager.State.Paused)
				{
					yield return null;
				}
			}
		}
		this.zoom = newZoom;
		yield return null;
		yield break;
	}

	// Token: 0x06000CDC RID: 3292 RVA: 0x00089930 File Offset: 0x00087D30
	public void StartBlur()
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(2f, 0.15f, false);
	}

	// Token: 0x06000CDD RID: 3293 RVA: 0x0008994E File Offset: 0x00087D4E
	public void StartBlur(float time)
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(2f, time, false);
	}

	// Token: 0x06000CDE RID: 3294 RVA: 0x00089968 File Offset: 0x00087D68
	public void StartBlur(float time, float amount)
	{
		this.maxBlur = amount;
		this.StartBlurCoroutine(2f, time, false);
	}

	// Token: 0x06000CDF RID: 3295 RVA: 0x0008997E File Offset: 0x00087D7E
	public void EndBlur()
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(0f, 0.15f, true);
	}

	// Token: 0x06000CE0 RID: 3296 RVA: 0x0008999C File Offset: 0x00087D9C
	public void EndBlur(float time)
	{
		this.maxBlur = 1.2f;
		this.StartBlurCoroutine(0f, time, true);
	}

	// Token: 0x06000CE1 RID: 3297 RVA: 0x000899B6 File Offset: 0x00087DB6
	public void EndBlur(float time, float amount)
	{
		this.maxBlur = amount;
		this.StartBlurCoroutine(0f, time, true);
	}

	// Token: 0x06000CE2 RID: 3298 RVA: 0x000899CC File Offset: 0x00087DCC
	private void StartBlurCoroutine(float amount, float time, bool disableBlurWhenComplete)
	{
		this.StopBlurCoroutine();
		this._blurCoroutine = this.blur_cr(amount, time, disableBlurWhenComplete);
		base.StartCoroutine(this._blurCoroutine);
	}

	// Token: 0x06000CE3 RID: 3299 RVA: 0x000899F0 File Offset: 0x00087DF0
	private void StopBlurCoroutine()
	{
		if (this._blurCoroutine != null)
		{
			base.StopCoroutine(this._blurCoroutine);
		}
		this._blurCoroutine = null;
	}

	// Token: 0x06000CE4 RID: 3300 RVA: 0x00089A10 File Offset: 0x00087E10
	private void UpdateBlur()
	{
		this._blurEffect.blurSize = Mathf.Lerp(0f, this.maxBlur, this._currentBlurAmount);
	}

	// Token: 0x06000CE5 RID: 3301 RVA: 0x00089A34 File Offset: 0x00087E34
	protected IEnumerator blur_cr(float end, float time, bool disableBlurWhenComplete)
	{
		float start = this._currentBlurAmount;
		this._blurEffect.enabled = true;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this._currentBlurAmount = Mathf.Lerp(start, end, val);
			this.UpdateBlur();
			t += Time.deltaTime;
			yield return null;
		}
		this._currentBlurAmount = end;
		this.UpdateBlur();
		yield return null;
		if (disableBlurWhenComplete)
		{
			this._blurEffect.enabled = false;
		}
		yield break;
	}

	// Token: 0x04001643 RID: 5699
	private Vector3 _shakeAdd;

	// Token: 0x04001644 RID: 5700
	private Vector3 _floatAdd;

	// Token: 0x04001645 RID: 5701
	protected Vector3 _position;

	// Token: 0x04001646 RID: 5702
	protected BlurOptimized _blurEffect;

	// Token: 0x04001647 RID: 5703
	private float _zoom = 1f;

	// Token: 0x04001649 RID: 5705
	private IEnumerator shakeCoroutine;

	// Token: 0x0400164A RID: 5706
	private float shakeAmount;

	// Token: 0x0400164C RID: 5708
	private AbstractCupheadGameCamera.FloatState floatState;

	// Token: 0x0400164D RID: 5709
	private IEnumerator floatCoroutine;

	// Token: 0x0400164E RID: 5710
	private IEnumerator zoomCoroutine;

	// Token: 0x0400164F RID: 5711
	private const float BLUR_TIME_START = 0.15f;

	// Token: 0x04001650 RID: 5712
	private const float BLUR_TIME_END = 0.15f;

	// Token: 0x04001651 RID: 5713
	private IEnumerator _blurCoroutine;

	// Token: 0x04001652 RID: 5714
	private float _currentBlurAmount;

	// Token: 0x04001653 RID: 5715
	private float maxBlur = 1.2f;

	// Token: 0x020003D3 RID: 979
	// (Invoke) Token: 0x06000CE7 RID: 3303
	public delegate void OnShakeHandler(float amount, float time);

	// Token: 0x020003D4 RID: 980
	private enum FloatState
	{
		// Token: 0x04001655 RID: 5717
		Stop,
		// Token: 0x04001656 RID: 5718
		Float
	}
}
