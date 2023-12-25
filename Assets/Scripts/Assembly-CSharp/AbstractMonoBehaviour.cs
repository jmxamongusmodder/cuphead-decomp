using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200035C RID: 860
public abstract class AbstractMonoBehaviour : MonoBehaviour
{
	// Token: 0x170001E6 RID: 486
	// (get) Token: 0x06000955 RID: 2389 RVA: 0x000066D1 File Offset: 0x00004AD1
	public Transform baseTransform
	{
		get
		{
			return base.transform;
		}
	}

	// Token: 0x170001E7 RID: 487
	// (get) Token: 0x06000956 RID: 2390 RVA: 0x000066D9 File Offset: 0x00004AD9
	public new Transform transform
	{
		get
		{
			if (!this._transformCached)
			{
				this._transform = this.baseTransform;
				this._transformCached = true;
			}
			return this._transform;
		}
	}

	// Token: 0x170001E8 RID: 488
	// (get) Token: 0x06000957 RID: 2391 RVA: 0x000066FF File Offset: 0x00004AFF
	public RectTransform baseRectTransform
	{
		get
		{
			return base.transform as RectTransform;
		}
	}

	// Token: 0x170001E9 RID: 489
	// (get) Token: 0x06000958 RID: 2392 RVA: 0x0000670C File Offset: 0x00004B0C
	public RectTransform rectTransform
	{
		get
		{
			if (this._rectTransform == null)
			{
				this._rectTransform = this.baseRectTransform;
			}
			return this._rectTransform;
		}
	}

	// Token: 0x170001EA RID: 490
	// (get) Token: 0x06000959 RID: 2393 RVA: 0x00006731 File Offset: 0x00004B31
	public Rigidbody baseRigidbody
	{
		get
		{
			return base.GetComponent<Rigidbody>();
		}
	}

	// Token: 0x170001EB RID: 491
	// (get) Token: 0x0600095A RID: 2394 RVA: 0x00006739 File Offset: 0x00004B39
	public Rigidbody rigidbody
	{
		get
		{
			if (this._rigidbody == null)
			{
				this._rigidbody = this.baseRigidbody;
			}
			return this._rigidbody;
		}
	}

	// Token: 0x170001EC RID: 492
	// (get) Token: 0x0600095B RID: 2395 RVA: 0x0000675E File Offset: 0x00004B5E
	public Rigidbody2D baseRigidbody2D
	{
		get
		{
			return base.GetComponent<Rigidbody2D>();
		}
	}

	// Token: 0x170001ED RID: 493
	// (get) Token: 0x0600095C RID: 2396 RVA: 0x00006766 File Offset: 0x00004B66
	public Rigidbody2D rigidbody2D
	{
		get
		{
			if (this._rigidbody2D == null)
			{
				this._rigidbody2D = this.baseRigidbody2D;
			}
			return this._rigidbody2D;
		}
	}

	// Token: 0x170001EE RID: 494
	// (get) Token: 0x0600095D RID: 2397 RVA: 0x0000678B File Offset: 0x00004B8B
	public Animator baseAnimator
	{
		get
		{
			return base.GetComponent<Animator>();
		}
	}

	// Token: 0x170001EF RID: 495
	// (get) Token: 0x0600095E RID: 2398 RVA: 0x00006793 File Offset: 0x00004B93
	public Animator animator
	{
		get
		{
			if (this._animator == null)
			{
				this._animator = this.baseAnimator;
			}
			return this._animator;
		}
	}

	// Token: 0x0600095F RID: 2399 RVA: 0x000067B8 File Offset: 0x00004BB8
	protected virtual void Awake()
	{
		base.useGUILayout = false;
	}

	// Token: 0x06000960 RID: 2400 RVA: 0x000067C1 File Offset: 0x00004BC1
	protected virtual void Reset()
	{
	}

	// Token: 0x06000961 RID: 2401 RVA: 0x000067C3 File Offset: 0x00004BC3
	protected virtual void OnDrawGizmos()
	{
	}

	// Token: 0x06000962 RID: 2402 RVA: 0x000067C5 File Offset: 0x00004BC5
	protected virtual void OnDrawGizmosSelected()
	{
	}

	// Token: 0x06000963 RID: 2403 RVA: 0x000067C8 File Offset: 0x00004BC8
	public virtual T InstantiatePrefab<T>() where T : MonoBehaviour
	{
		GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(base.gameObject);
		gameObject.name = gameObject.name.Replace("(Clone)", string.Empty);
		return gameObject.GetComponent<T>();
	}

	// Token: 0x06000964 RID: 2404 RVA: 0x00006802 File Offset: 0x00004C02
	public Coroutine FrameDelayedCallback(Action callback, int frames)
	{
		return base.StartCoroutine(this.frameDelayedCallback_cr(callback, frames));
	}

	// Token: 0x06000965 RID: 2405 RVA: 0x00006814 File Offset: 0x00004C14
	public IEnumerator frameDelayedCallback_cr(Action callback, int frames)
	{
		for (int i = 0; i < frames; i++)
		{
			yield return null;
		}
		if (callback != null)
		{
			callback();
		}
		yield break;
	}

	// Token: 0x170001F0 RID: 496
	// (get) Token: 0x06000966 RID: 2406 RVA: 0x00006836 File Offset: 0x00004C36
	protected float LocalDeltaTime
	{
		get
		{
			if (this.ignoreGlobalTime)
			{
				return Time.deltaTime;
			}
			return CupheadTime.Delta[this.timeLayer];
		}
	}

	// Token: 0x06000967 RID: 2407 RVA: 0x00006859 File Offset: 0x00004C59
	public Coroutine TweenValue(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenValue_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000968 RID: 2408 RVA: 0x00006870 File Offset: 0x00004C70
	protected IEnumerator tweenValue_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			if (updateCallback != null)
			{
				updateCallback(EaseUtils.Ease(ease, start, end, val));
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		if (updateCallback != null)
		{
			updateCallback(end);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000969 RID: 2409 RVA: 0x000068B0 File Offset: 0x00004CB0
	public Coroutine TweenScale(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenScale_cr(start, end, time, ease, null));
	}

	// Token: 0x0600096A RID: 2410 RVA: 0x000068C4 File Offset: 0x00004CC4
	public Coroutine TweenScale(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenScale_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x0600096B RID: 2411 RVA: 0x000068DC File Offset: 0x00004CDC
	private IEnumerator tweenScale_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetScale(new float?(start.x), new float?(start.y), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			this.transform.SetScale(new float?(x), new float?(y), null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetScale(new float?(end.x), new float?(end.y), null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600096C RID: 2412 RVA: 0x0000691C File Offset: 0x00004D1C
	public Coroutine TweenPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPosition_cr(start, end, time, ease, null));
	}

	// Token: 0x0600096D RID: 2413 RVA: 0x00006930 File Offset: 0x00004D30
	public Coroutine TweenPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPosition_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x0600096E RID: 2414 RVA: 0x00006948 File Offset: 0x00004D48
	private IEnumerator tweenPosition_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.position = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			this.transform.SetPosition(new float?(x), new float?(y), new float?(0f));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.position = end;
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600096F RID: 2415 RVA: 0x00006988 File Offset: 0x00004D88
	public Coroutine TweenLocalPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPosition_cr(start, end, time, ease, null));
	}

	// Token: 0x06000970 RID: 2416 RVA: 0x0000699C File Offset: 0x00004D9C
	public Coroutine TweenLocalPosition(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPosition_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000971 RID: 2417 RVA: 0x000069B4 File Offset: 0x00004DB4
	private IEnumerator tweenLocalPosition_cr(Vector2 start, Vector2 end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.localPosition = start;
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			float x = EaseUtils.Ease(ease, start.x, end.x, val);
			float y = EaseUtils.Ease(ease, start.y, end.y, val);
			this.transform.SetLocalPosition(new float?(x), new float?(y), new float?(0f));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.localPosition = end;
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000972 RID: 2418 RVA: 0x000069F4 File Offset: 0x00004DF4
	public Coroutine TweenPositionX(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPositionX_cr(start, end, time, ease, null));
	}

	// Token: 0x06000973 RID: 2419 RVA: 0x00006A08 File Offset: 0x00004E08
	public Coroutine TweenPositionX(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPositionX_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000974 RID: 2420 RVA: 0x00006A20 File Offset: 0x00004E20
	private IEnumerator tweenPositionX_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetPosition(new float?(start), null, null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetPosition(new float?(end), null, null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000975 RID: 2421 RVA: 0x00006A60 File Offset: 0x00004E60
	public Coroutine TweenLocalPositionX(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPositionX_cr(start, end, time, ease, null));
	}

	// Token: 0x06000976 RID: 2422 RVA: 0x00006A74 File Offset: 0x00004E74
	public Coroutine TweenLocalPositionX(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPositionX_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000977 RID: 2423 RVA: 0x00006A8C File Offset: 0x00004E8C
	private IEnumerator tweenLocalPositionX_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalPosition(new float?(start), null, null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalPosition(new float?(EaseUtils.Ease(ease, start, end, val)), null, null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalPosition(new float?(end), null, null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000978 RID: 2424 RVA: 0x00006ACC File Offset: 0x00004ECC
	public Coroutine TweenPositionY(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPositionY_cr(start, end, time, ease, null));
	}

	// Token: 0x06000979 RID: 2425 RVA: 0x00006AE0 File Offset: 0x00004EE0
	public Coroutine TweenPositionY(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPositionY_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x0600097A RID: 2426 RVA: 0x00006AF8 File Offset: 0x00004EF8
	private IEnumerator tweenPositionY_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetPosition(null, new float?(end), null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600097B RID: 2427 RVA: 0x00006B38 File Offset: 0x00004F38
	public Coroutine TweenLocalPositionY(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPositionY_cr(start, end, time, ease, null));
	}

	// Token: 0x0600097C RID: 2428 RVA: 0x00006B4C File Offset: 0x00004F4C
	public Coroutine TweenLocalPositionY(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPositionY_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x0600097D RID: 2429 RVA: 0x00006B64 File Offset: 0x00004F64
	private IEnumerator tweenLocalPositionY_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalPosition(null, new float?(start), null);
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalPosition(null, new float?(EaseUtils.Ease(ease, start, end, val)), null);
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalPosition(null, new float?(end), null);
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600097E RID: 2430 RVA: 0x00006BA4 File Offset: 0x00004FA4
	public Coroutine TweenPositionZ(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenPositionZ_cr(start, end, time, ease, null));
	}

	// Token: 0x0600097F RID: 2431 RVA: 0x00006BB8 File Offset: 0x00004FB8
	public Coroutine TweenPositionZ(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenPositionZ_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000980 RID: 2432 RVA: 0x00006BD0 File Offset: 0x00004FD0
	private IEnumerator tweenPositionZ_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetPosition(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetPosition(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetPosition(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000981 RID: 2433 RVA: 0x00006C10 File Offset: 0x00005010
	public Coroutine TweenLocalPositionZ(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalPositionZ_cr(start, end, time, ease, null));
	}

	// Token: 0x06000982 RID: 2434 RVA: 0x00006C24 File Offset: 0x00005024
	public Coroutine TweenLocalPositionZ(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalPositionZ_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000983 RID: 2435 RVA: 0x00006C3C File Offset: 0x0000503C
	private IEnumerator tweenLocalPositionZ_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalPosition(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalPosition(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalPosition(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000984 RID: 2436 RVA: 0x00006C7C File Offset: 0x0000507C
	public Coroutine TweenRotation2D(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenRotation2D_cr(start, end, time, ease, null));
	}

	// Token: 0x06000985 RID: 2437 RVA: 0x00006C90 File Offset: 0x00005090
	public Coroutine TweenRotation2D(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenRotation2D_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000986 RID: 2438 RVA: 0x00006CA8 File Offset: 0x000050A8
	private IEnumerator tweenRotation2D_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetEulerAngles(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetEulerAngles(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x06000987 RID: 2439 RVA: 0x00006CE8 File Offset: 0x000050E8
	public Coroutine TweenLocalRotation2D(float start, float end, float time, EaseUtils.EaseType ease)
	{
		return base.StartCoroutine(this.tweenLocalRotation2D_cr(start, end, time, ease, null));
	}

	// Token: 0x06000988 RID: 2440 RVA: 0x00006CFC File Offset: 0x000050FC
	public Coroutine TweenLocalRotation2D(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback)
	{
		return base.StartCoroutine(this.tweenLocalRotation2D_cr(start, end, time, ease, updateCallback));
	}

	// Token: 0x06000989 RID: 2441 RVA: 0x00006D14 File Offset: 0x00005114
	private IEnumerator tweenLocalRotation2D_cr(float start, float end, float time, EaseUtils.EaseType ease, AbstractMonoBehaviour.TweenUpdateHandler updateCallback = null)
	{
		this.transform.SetLocalEulerAngles(null, null, new float?(start));
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			this.transform.SetLocalEulerAngles(null, null, new float?(EaseUtils.Ease(ease, start, end, val)));
			if (updateCallback != null)
			{
				updateCallback(val);
			}
			t += this.LocalDeltaTime;
			yield return null;
		}
		this.transform.SetLocalEulerAngles(null, null, new float?(end));
		if (updateCallback != null)
		{
			updateCallback(1f);
		}
		yield return null;
		yield break;
	}

	// Token: 0x0600098A RID: 2442 RVA: 0x00006D54 File Offset: 0x00005154
	public new virtual void StopAllCoroutines()
	{
		base.StopAllCoroutines();
	}

	// Token: 0x04001429 RID: 5161
	private Transform _transform;

	// Token: 0x0400142A RID: 5162
	private bool _transformCached;

	// Token: 0x0400142B RID: 5163
	private RectTransform _rectTransform;

	// Token: 0x0400142C RID: 5164
	private Rigidbody _rigidbody;

	// Token: 0x0400142D RID: 5165
	private Rigidbody2D _rigidbody2D;

	// Token: 0x0400142E RID: 5166
	private Animator _animator;

	// Token: 0x0400142F RID: 5167
	protected bool ignoreGlobalTime;

	// Token: 0x04001430 RID: 5168
	protected CupheadTime.Layer timeLayer;

	// Token: 0x0200035D RID: 861
	// (Invoke) Token: 0x0600098C RID: 2444
	public delegate void TweenUpdateHandler(float value);
}
