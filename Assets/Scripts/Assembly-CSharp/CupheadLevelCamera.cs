using System;
using System.Collections;
using UnityEngine;

// Token: 0x020003D8 RID: 984
public class CupheadLevelCamera : AbstractCupheadGameCamera
{
	// Token: 0x1700022F RID: 559
	// (get) Token: 0x06000CFC RID: 3324 RVA: 0x0008A852 File Offset: 0x00088C52
	// (set) Token: 0x06000CFD RID: 3325 RVA: 0x0008A859 File Offset: 0x00088C59
	public static CupheadLevelCamera Current { get; private set; }

	// Token: 0x17000230 RID: 560
	// (get) Token: 0x06000CFE RID: 3326 RVA: 0x0008A861 File Offset: 0x00088C61
	// (set) Token: 0x06000CFF RID: 3327 RVA: 0x0008A869 File Offset: 0x00088C69
	public bool cameraLocked { get; private set; }

	// Token: 0x17000231 RID: 561
	// (get) Token: 0x06000D00 RID: 3328 RVA: 0x0008A872 File Offset: 0x00088C72
	// (set) Token: 0x06000D01 RID: 3329 RVA: 0x0008A87A File Offset: 0x00088C7A
	public bool cameraOffset { get; private set; }

	// Token: 0x17000232 RID: 562
	// (get) Token: 0x06000D02 RID: 3330 RVA: 0x0008A883 File Offset: 0x00088C83
	// (set) Token: 0x06000D03 RID: 3331 RVA: 0x0008A88B File Offset: 0x00088C8B
	public bool autoScrolling { get; private set; }

	// Token: 0x17000233 RID: 563
	// (get) Token: 0x06000D04 RID: 3332 RVA: 0x0008A894 File Offset: 0x00088C94
	// (set) Token: 0x06000D05 RID: 3333 RVA: 0x0008A89C File Offset: 0x00088C9C
	public float autoScrollSpeedMultiplier { get; private set; }

	// Token: 0x17000234 RID: 564
	// (get) Token: 0x06000D06 RID: 3334 RVA: 0x0008A8A5 File Offset: 0x00088CA5
	// (set) Token: 0x06000D07 RID: 3335 RVA: 0x0008A8AD File Offset: 0x00088CAD
	public float Left { get; private set; }

	// Token: 0x17000235 RID: 565
	// (get) Token: 0x06000D08 RID: 3336 RVA: 0x0008A8B6 File Offset: 0x00088CB6
	// (set) Token: 0x06000D09 RID: 3337 RVA: 0x0008A8BE File Offset: 0x00088CBE
	public float Right { get; private set; }

	// Token: 0x17000236 RID: 566
	// (get) Token: 0x06000D0A RID: 3338 RVA: 0x0008A8C7 File Offset: 0x00088CC7
	// (set) Token: 0x06000D0B RID: 3339 RVA: 0x0008A8CF File Offset: 0x00088CCF
	public float Bottom { get; private set; }

	// Token: 0x17000237 RID: 567
	// (get) Token: 0x06000D0C RID: 3340 RVA: 0x0008A8D8 File Offset: 0x00088CD8
	// (set) Token: 0x06000D0D RID: 3341 RVA: 0x0008A8E0 File Offset: 0x00088CE0
	public new float Top { get; private set; }

	// Token: 0x17000238 RID: 568
	// (get) Token: 0x06000D0E RID: 3342 RVA: 0x0008A8E9 File Offset: 0x00088CE9
	public override float OrthographicSize
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x06000D0F RID: 3343 RVA: 0x0008A8F0 File Offset: 0x00088CF0
	protected override void Awake()
	{
		base.Awake();
		CupheadLevelCamera.Current = this;
	}

	// Token: 0x06000D10 RID: 3344 RVA: 0x0008A8FE File Offset: 0x00088CFE
	private void Start()
	{
		this.autoScrolling = false;
		this.cameraLocked = false;
		this.cameraOffset = false;
		this.autoScrollSpeedMultiplier = 1f;
		this._position = base.transform.position;
	}

	// Token: 0x06000D11 RID: 3345 RVA: 0x0008A931 File Offset: 0x00088D31
	private void OnDestroy()
	{
		if (CupheadLevelCamera.Current == this)
		{
			CupheadLevelCamera.Current = null;
		}
	}

	// Token: 0x06000D12 RID: 3346 RVA: 0x0008A94C File Offset: 0x00088D4C
	private void Update()
	{
		if (PlayerManager.Count <= 0)
		{
			return;
		}
		this.UpdateBounds();
		Vector3 position = this._position;
		CupheadLevelCamera.Mode mode = this.mode;
		switch (mode)
		{
		case CupheadLevelCamera.Mode.Lerp:
			break;
		case CupheadLevelCamera.Mode.TrapBox:
			this.UpdateModeTrapBox();
			goto IL_A4;
		case CupheadLevelCamera.Mode.Relative:
			this.UpdateModeRelative();
			goto IL_A4;
		case CupheadLevelCamera.Mode.Platforming:
			this.UpdatePlatforming();
			goto IL_A4;
		case CupheadLevelCamera.Mode.Path:
			this.UpdatePath();
			goto IL_A4;
		case CupheadLevelCamera.Mode.RelativeRook:
			this.UpdateModeRelativeRook();
			goto IL_A4;
		case CupheadLevelCamera.Mode.RelativeRumRunners:
			this.UpdateModeRelativeRumRunners();
			goto IL_A4;
		default:
			if (mode == CupheadLevelCamera.Mode.Static)
			{
				goto IL_A4;
			}
			break;
		}
		this.UpdateModeLerp();
		IL_A4:
		Vector3 position2 = this._position;
		if (base.Width * 2f > (float)this.bounds.Width)
		{
			position2.x = Mathf.Lerp(position.x, 0f, CupheadTime.Delta * 10f);
		}
		if (base.Height * 2f > (float)this.bounds.Height)
		{
			position2.y = Mathf.Lerp(position.y, 0f, CupheadTime.Delta * 10f);
		}
		this._position = position2;
		base.Move();
	}

	// Token: 0x06000D13 RID: 3347 RVA: 0x0008AA9B File Offset: 0x00088E9B
	protected override void LateUpdate()
	{
		base.LateUpdate();
		base.Move();
	}

	// Token: 0x06000D14 RID: 3348 RVA: 0x0008AAAC File Offset: 0x00088EAC
	private void UpdateBounds()
	{
		this.Left = ((!this.bounds.leftEnabled) ? float.MinValue : ((float)(-(float)this.bounds.left) + base.Width));
		this.Right = ((!this.bounds.rightEnabled) ? float.MaxValue : ((float)this.bounds.right - base.Width));
		this.Bottom = ((!this.bounds.bottomEnabled) ? float.MinValue : ((float)(-(float)this.bounds.bottom) + base.Height));
		this.Top = ((!this.bounds.topEnabled) ? float.MaxValue : ((float)this.bounds.top - base.Height));
	}

	// Token: 0x06000D15 RID: 3349 RVA: 0x0008AB87 File Offset: 0x00088F87
	public void DisableRightCollider()
	{
		this.rightCollider.gameObject.SetActive(false);
	}

	// Token: 0x06000D16 RID: 3350 RVA: 0x0008AB9A File Offset: 0x00088F9A
	public void MoveRightCollider()
	{
		this.rightCollider.transform.AddPosition(100f, 0f, 0f);
	}

	// Token: 0x06000D17 RID: 3351 RVA: 0x0008ABBC File Offset: 0x00088FBC
	public void Init(Level.Camera properties)
	{
		base.enabled = true;
		this.mode = properties.mode;
		base.zoom = properties.zoom;
		this.moveX = properties.moveX;
		this.moveY = properties.moveY;
		this.stabilizeY = properties.stabilizeY;
		this.stabilizePaddingTop = properties.stabilizePaddingTop;
		this.stabilizePaddingBottom = properties.stabilizePaddingBottom;
		this.bounds = properties.bounds;
		this.path = properties.path;
		this.pathMovesOnlyForward = properties.pathMovesOnlyForward;
		if (properties.mode == CupheadLevelCamera.Mode.Path)
		{
			base.transform.position = this.path.Lerp(0f);
		}
		this.UpdateBounds();
		if (properties.colliders)
		{
			this.collidersRoot = new GameObject("Colliders").transform;
			this.collidersRoot.parent = base.transform;
			this.collidersRoot.ResetLocalTransforms();
			this.SetupCollider(Level.Bounds.Side.Left);
			this.rightCollider = this.SetupCollider(Level.Bounds.Side.Right);
		}
	}

	// Token: 0x06000D18 RID: 3352 RVA: 0x0008ACC8 File Offset: 0x000890C8
	private SpriteRenderer CreateBorderRenderer(Texture2D texture, Transform parent, string name)
	{
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		Vector2 zero3 = Vector2.zero;
		string text = name.ToLower();
		if (text != null)
		{
			if (!(text == "left"))
			{
				if (!(text == "right"))
				{
					if (!(text == "top"))
					{
						if (text == "bottom")
						{
							zero = new Vector2((float)this.bounds.Width + 2000f, 1000f);
							zero2 = new Vector2(this.bounds.Center.x, (float)(-(float)this.bounds.bottom));
							zero3 = new Vector2(0.5f, 1f);
						}
					}
					else
					{
						zero = new Vector2((float)this.bounds.Width + 2000f, 1000f);
						zero2 = new Vector2(this.bounds.Center.x, (float)this.bounds.top);
						zero3 = new Vector2(0.5f, 0f);
					}
				}
				else
				{
					zero = new Vector2(1000f, (float)this.bounds.Height + 2000f);
					zero2 = new Vector2((float)this.bounds.right, this.bounds.Center.y);
					zero3 = new Vector2(0f, 0.5f);
				}
			}
			else
			{
				zero = new Vector2(1000f, (float)this.bounds.Height + 2000f);
				zero2 = new Vector2((float)(-(float)this.bounds.left), this.bounds.Center.y);
				zero3 = new Vector2(1f, 0.5f);
			}
		}
		SpriteRenderer spriteRenderer = new GameObject(name).AddComponent<SpriteRenderer>();
		spriteRenderer.transform.SetParent(parent);
		Sprite sprite = Sprite.Create(texture, new Rect(0f, 0f, 1f, 1f), zero3, 1f);
		spriteRenderer.sprite = sprite;
		spriteRenderer.transform.localScale = zero;
		spriteRenderer.transform.position = zero2;
		spriteRenderer.sortingLayerName = SpriteLayer.Foreground.ToString();
		spriteRenderer.sortingOrder = 10000;
		return spriteRenderer;
	}

	// Token: 0x06000D19 RID: 3353 RVA: 0x0008AF40 File Offset: 0x00089340
	private Texture2D CreateBorderTexture()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixel(0, 0, Color.black);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x06000D1A RID: 3354 RVA: 0x0008AF70 File Offset: 0x00089370
	private Transform SetupCollider(Level.Bounds.Side side)
	{
		string name = string.Empty;
		string tag = string.Empty;
		int layer = 0;
		int num = 0;
		Vector2 zero = Vector2.zero;
		Vector2 vector = new Vector2(base.Bounds.xMin, base.Bounds.yMax);
		Vector2 vector2 = new Vector2(base.Bounds.xMax, base.Bounds.yMin);
		float x = vector.x;
		float x2 = vector2.x;
		float y = vector2.y;
		float y2 = vector.y;
		switch (side)
		{
		case Level.Bounds.Side.Left:
			name = "Level_Wall_Left";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = 90;
			zero = new Vector2(x - 200f, 0f);
			break;
		case Level.Bounds.Side.Right:
			name = "Level_Wall_Right";
			tag = "Wall";
			layer = LayerMask.NameToLayer(Layers.Bounds_Walls.ToString());
			num = -90;
			zero = new Vector2(x2 + 200f, 0f);
			break;
		case Level.Bounds.Side.Top:
			name = "Level_Ceiling";
			tag = "Ceiling";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ceiling.ToString());
			zero = new Vector2(0f, y + 200f);
			break;
		case Level.Bounds.Side.Bottom:
			name = "Level_Ground";
			tag = "Ground";
			layer = LayerMask.NameToLayer(Layers.Bounds_Ground.ToString());
			num = 180;
			zero = new Vector2(0f, y2 - 200f);
			break;
		}
		GameObject gameObject = new GameObject(name);
		gameObject.tag = tag;
		gameObject.layer = layer;
		gameObject.transform.ResetLocalTransforms();
		gameObject.transform.SetPosition(new float?(zero.x), new float?(zero.y), null);
		gameObject.transform.SetEulerAngles(null, null, new float?((float)num));
		gameObject.transform.parent = this.collidersRoot;
		BoxCollider2D boxCollider2D = gameObject.AddComponent<BoxCollider2D>();
		boxCollider2D.isTrigger = true;
		boxCollider2D.size = new Vector2(2000f, 400f);
		return gameObject.transform;
	}

	// Token: 0x06000D1B RID: 3355 RVA: 0x0008B1D4 File Offset: 0x000895D4
	private void UpdateModeLerp()
	{
		Vector3 position = this._position;
		Vector3 vector = PlayerManager.Center;
		if (this.moveX)
		{
			position.x = vector.x;
		}
		if (this.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.Left, this.Right);
		position.y = Mathf.Clamp(position.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, position, CupheadTime.Delta * this.LERP_SPEED);
	}

	// Token: 0x06000D1C RID: 3356 RVA: 0x0008B288 File Offset: 0x00089688
	private void UpdateModeTrapBox()
	{
		Vector3 position = this._position;
		Vector3 vector = PlayerManager.CameraCenter;
		if (this.moveX)
		{
			position.x = vector.x;
		}
		if (this.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.Left, this.Right);
		position.y = Mathf.Clamp(position.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, position, Time.deltaTime * this.LERP_SPEED);
	}

	// Token: 0x06000D1D RID: 3357 RVA: 0x0008B338 File Offset: 0x00089738
	private void UpdateModeRelative()
	{
		Vector2 v = this._position;
		Vector2 vector = new Vector2(0f, 0f);
		vector.x = MathUtils.GetPercentage((float)Level.Current.Left, (float)Level.Current.Right, PlayerManager.Center.x);
		vector.y = MathUtils.GetPercentage((float)Level.Current.Ground, (float)Level.Current.Ceiling, PlayerManager.Center.y);
		if (this.moveX)
		{
			v.x = Mathf.Lerp(this.Left, this.Right, vector.x);
		}
		if (this.moveY)
		{
			v.y = Mathf.Lerp(this.Bottom, this.Top, vector.y);
		}
		v.x = Mathf.Clamp(v.x, this.Left, this.Right);
		v.y = Mathf.Clamp(v.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, v, CupheadTime.Delta * 5f);
	}

	// Token: 0x06000D1E RID: 3358 RVA: 0x0008B47C File Offset: 0x0008987C
	private void UpdateModeRelativeRook()
	{
		Vector2 vector = this._position;
		Vector2 vector2 = new Vector2(0f, 0f);
		vector2.x = MathUtils.GetPercentage((float)Level.Current.Left, (float)Level.Current.Right, PlayerManager.TopPlayerPosition.x);
		vector2.y = PlayerManager.TopPlayerPosition.y;
		if (this.moveX)
		{
			vector.x = Mathf.Lerp(this.Left, this.Right, vector2.x);
		}
		vector.y = Mathf.Lerp(this.Bottom, this.Top, Mathf.InverseLerp(200f, 400f, vector2.y));
		vector.x = Mathf.Clamp(vector.x, this.Left, this.Right);
		vector.y = Mathf.Clamp(vector.y, this.Bottom, this.Top);
		this._position = new Vector3(Mathf.Lerp(this._position.x, vector.x, CupheadTime.Delta * 5f), Mathf.Lerp(this._position.y, vector.y, CupheadTime.Delta * 2.5f));
	}

	// Token: 0x06000D1F RID: 3359 RVA: 0x0008B5D8 File Offset: 0x000899D8
	private void UpdateModeRelativeRumRunners()
	{
		Vector2 vector = this._position;
		Vector2 vector2 = new Vector2(0f, 0f);
		vector2.x = MathUtils.GetPercentage((float)Level.Current.Left, (float)Level.Current.Right, PlayerManager.TopPlayerPosition.x);
		vector2.y = PlayerManager.TopPlayerPosition.y;
		if (this.moveX)
		{
			vector.x = Mathf.Lerp(this.Left, this.Right, vector2.x);
		}
		vector.y = ((vector2.y >= 200f) ? this.Top : this.Bottom);
		vector.x = Mathf.Clamp(vector.x, this.Left, this.Right);
		vector.y = Mathf.Clamp(vector.y, this.Bottom, this.Top);
		this._position = new Vector3(Mathf.Lerp(this._position.x, vector.x, CupheadTime.Delta * 5f), Mathf.Lerp(this._position.y, vector.y, CupheadTime.Delta * 2.5f));
	}

	// Token: 0x06000D20 RID: 3360 RVA: 0x0008B730 File Offset: 0x00089B30
	private void UpdatePlatforming()
	{
		Vector3 position = this._position;
		Vector3 vector = PlayerManager.Center;
		if (this.moveX && position.x < vector.x)
		{
			position.x = vector.x;
		}
		if (this.moveY)
		{
			position.y = vector.y;
		}
		position.x = Mathf.Clamp(position.x, this.Left, this.Right);
		position.y = Mathf.Clamp(position.y, this.Bottom, this.Top);
		this._position = Vector3.Lerp(this._position, position, CupheadTime.Delta * 5f);
	}

	// Token: 0x06000D21 RID: 3361 RVA: 0x0008B7F4 File Offset: 0x00089BF4
	public void LockCamera(bool lockCamera)
	{
		this.cameraLocked = lockCamera;
	}

	// Token: 0x06000D22 RID: 3362 RVA: 0x0008B7FD File Offset: 0x00089BFD
	public void SetAutoScroll(bool isScrolling)
	{
		this.autoScrolling = isScrolling;
	}

	// Token: 0x06000D23 RID: 3363 RVA: 0x0008B806 File Offset: 0x00089C06
	public void OffsetCamera(bool cameraOffset, bool leftOffset)
	{
		this.cameraOffset = cameraOffset;
		this.leftOffset = leftOffset;
	}

	// Token: 0x06000D24 RID: 3364 RVA: 0x0008B816 File Offset: 0x00089C16
	public void SetAutoscrollSpeedMultiplier(float multiplier)
	{
		this.autoScrollSpeedMultiplier = multiplier;
	}

	// Token: 0x06000D25 RID: 3365 RVA: 0x0008B820 File Offset: 0x00089C20
	private void UpdatePath()
	{
		Vector3 position = this._position;
		Vector2 v = PlayerManager.Center;
		if (this.stabilizeY)
		{
			AbstractPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne);
			AbstractPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo);
			Vector2 vector = (!(player == null)) ? player.center : Vector2.zero;
			Vector2 vector2 = (!(player2 == null)) ? player2.center : Vector2.zero;
			if (vector.y > position.y + this.stabilizePaddingTop)
			{
				vector.y -= this.stabilizePaddingTop;
			}
			else if (vector.y < position.y - this.stabilizePaddingBottom)
			{
				vector.y += this.stabilizePaddingBottom;
			}
			else
			{
				vector.y = position.y;
			}
			if (vector2.y > position.y + this.stabilizePaddingTop)
			{
				vector2.y -= this.stabilizePaddingTop;
			}
			else if (vector2.y < position.y - this.stabilizePaddingBottom)
			{
				vector2.y += this.stabilizePaddingBottom;
			}
			else
			{
				vector2.y = position.y;
			}
			if (player != null && !player.IsDead && player2 != null && !player2.IsDead)
			{
				v = (vector + vector2) / 2f;
			}
			else if (player != null && !player.IsDead)
			{
				v = vector;
			}
			else if (player2 != null && !player2.IsDead)
			{
				v = vector2;
			}
		}
		if (this.cameraOffset)
		{
			float num = (!this.leftOffset) ? -500f : 500f;
			this.targetPos = new Vector3(v.x + num, v.y);
		}
		else
		{
			this.targetPos = v;
		}
		Vector3 vector3 = this.path.GetClosestPoint(this._position, this.targetPos, this.moveX, this.moveY);
		float num2 = (vector3 - position).magnitude / CupheadTime.Delta;
		float num3 = Mathf.Max(this._speedLastFrame + 5000f * CupheadTime.Delta, 1000f);
		if (num2 > num3)
		{
			vector3 = position + (vector3 - position).normalized * num3 * CupheadTime.Delta;
		}
		this._speedLastFrame = Mathf.Min(num2, num3);
		if (this.pathMovesOnlyForward)
		{
			float closestNormalizedPoint = this.path.GetClosestNormalizedPoint(this._position, vector3, this.moveX, this.moveY);
			if (closestNormalizedPoint < this._minPathValue)
			{
				return;
			}
		}
		position.x = vector3.x;
		position.y = vector3.y;
		if (!this.cameraLocked)
		{
			if (!this.autoScrolling)
			{
				this._position = Vector3.Lerp(this._position, position, CupheadTime.Delta * 15f);
			}
			else
			{
				Vector3 v2 = new Vector3(base.transform.position.x + 500f, base.transform.position.y);
				Vector3 target = this.path.GetClosestPoint(this._position, v2, this.moveX, this.moveY);
				float num4 = 200f * this.autoScrollSpeedMultiplier;
				this._position = Vector3.MoveTowards(this._position, target, CupheadTime.Delta * num4);
			}
		}
		if (this.pathMovesOnlyForward)
		{
			this._minPathValue = this.path.GetClosestNormalizedPoint(this._position, this._position, this.moveX, this.moveY);
		}
	}

	// Token: 0x06000D26 RID: 3366 RVA: 0x0008BC80 File Offset: 0x0008A080
	public IEnumerator rotate_camera()
	{
		float time = 2f;
		float t = 0f;
		for (;;)
		{
			t += CupheadTime.Delta;
			float phase = Mathf.Sin(t / time);
			base.transform.localRotation = Quaternion.Euler(new Vector3(0f, 0f, phase * 1f));
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000D27 RID: 3367 RVA: 0x0008BC9C File Offset: 0x0008A09C
	public void SetRotation(float amount)
	{
		base.transform.SetEulerAngles(null, null, new float?(amount));
	}

	// Token: 0x06000D28 RID: 3368 RVA: 0x0008BCCC File Offset: 0x0008A0CC
	public IEnumerator change_zoom_cr(float newSize, float time)
	{
		float t = 0f;
		while (t < time)
		{
			float val = t / time;
			base.zoom = Mathf.Lerp(base.zoom, newSize, val);
			t += CupheadTime.Delta;
			yield return null;
		}
		base.zoom = newSize;
		yield return null;
		yield break;
	}

	// Token: 0x06000D29 RID: 3369 RVA: 0x0008BCF8 File Offset: 0x0008A0F8
	public IEnumerator slide_camera_cr(Vector3 slideAmount, float time)
	{
		float t = 0f;
		Vector3 start = this._position;
		while (t < time)
		{
			t += CupheadTime.Delta;
			float val = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, t / time);
			this._position = Vector3.Lerp(start, slideAmount, val);
			yield return null;
		}
		yield break;
	}

	// Token: 0x06000D2A RID: 3370 RVA: 0x0008BD21 File Offset: 0x0008A121
	public void ChangeHorizontalBounds(int left, int right)
	{
		this.bounds.left = left;
		this.bounds.right = right;
	}

	// Token: 0x06000D2B RID: 3371 RVA: 0x0008BD3B File Offset: 0x0008A13B
	public void ChangeVerticalBounds(int top, int bottom)
	{
		this.bounds.top = top;
		this.bounds.bottom = bottom;
	}

	// Token: 0x06000D2C RID: 3372 RVA: 0x0008BD55 File Offset: 0x0008A155
	public void ChangeCameraMode(CupheadLevelCamera.Mode mode)
	{
		this.mode = mode;
	}

	// Token: 0x06000D2D RID: 3373 RVA: 0x0008BD5E File Offset: 0x0008A15E
	public void SetPosition(Vector3 pos)
	{
		this._position = pos;
		base.transform.position = pos;
	}

	// Token: 0x0400166F RID: 5743
	public const string EDITOR_PATH = "Assets/_CUPHEAD/Prefabs/Camera/LevelCamera.prefab";

	// Token: 0x04001670 RID: 5744
	public const float AUTOSCROLL_SPEED = 200f;

	// Token: 0x04001675 RID: 5749
	private bool leftOffset;

	// Token: 0x04001676 RID: 5750
	private const float BOUND_COLLIDER_SIZE = 400f;

	// Token: 0x04001677 RID: 5751
	private const float BORDER_THICKNESS = 1000f;

	// Token: 0x04001678 RID: 5752
	private const float CENTER_SPEED = 10f;

	// Token: 0x04001679 RID: 5753
	private const float AUTOSCROLL_CHECK = 500f;

	// Token: 0x0400167A RID: 5754
	private const float OFFSET_AMOUNT = 500f;

	// Token: 0x0400167B RID: 5755
	private const float THREE_SIXTY = 360f;

	// Token: 0x0400167C RID: 5756
	private bool moveX;

	// Token: 0x0400167D RID: 5757
	private bool moveY;

	// Token: 0x0400167E RID: 5758
	private bool stabilizeY;

	// Token: 0x0400167F RID: 5759
	private float stabilizePaddingTop;

	// Token: 0x04001680 RID: 5760
	private float stabilizePaddingBottom;

	// Token: 0x04001681 RID: 5761
	private Vector3 targetPos;

	// Token: 0x04001682 RID: 5762
	private CupheadLevelCamera.Mode mode;

	// Token: 0x04001683 RID: 5763
	private Level.Bounds bounds;

	// Token: 0x04001684 RID: 5764
	private Transform collidersRoot;

	// Token: 0x04001685 RID: 5765
	private VectorPath path;

	// Token: 0x04001686 RID: 5766
	private bool pathMovesOnlyForward;

	// Token: 0x04001687 RID: 5767
	public bool enablePathScrubbing;

	// Token: 0x04001688 RID: 5768
	[Range(0f, 1f)]
	public float scrub;

	// Token: 0x0400168D RID: 5773
	private Transform leftCollider;

	// Token: 0x0400168E RID: 5774
	private Transform rightCollider;

	// Token: 0x0400168F RID: 5775
	private Transform topCollider;

	// Token: 0x04001690 RID: 5776
	private Transform bottomCollider;

	// Token: 0x04001691 RID: 5777
	[HideInInspector]
	public float LERP_SPEED = 2f;

	// Token: 0x04001692 RID: 5778
	private const float RELATIVE_LERP_SPEED = 5f;

	// Token: 0x04001693 RID: 5779
	private const float ROOK_SCROLL_UP_MIN = 200f;

	// Token: 0x04001694 RID: 5780
	private const float ROOK_SCROLL_UP_MAX = 400f;

	// Token: 0x04001695 RID: 5781
	private const float V_LERP_SLOW_SPEED = 2.5f;

	// Token: 0x04001696 RID: 5782
	private const float RUMRUNNERS_SCROLL_UP_THRESHOLD = 200f;

	// Token: 0x04001697 RID: 5783
	private const float PLATFORMING_LERP_SPEED = 5f;

	// Token: 0x04001698 RID: 5784
	private const float PATH_LERP_SPEED = 15f;

	// Token: 0x04001699 RID: 5785
	private const float PATH_MAX_SPEED_BEFORE_ACCELERATION = 1000f;

	// Token: 0x0400169A RID: 5786
	private const float PATH_ACCELERATION = 5000f;

	// Token: 0x0400169B RID: 5787
	private float _minPathValue = float.MinValue;

	// Token: 0x0400169C RID: 5788
	private float _speedLastFrame;

	// Token: 0x020003D9 RID: 985
	public enum Mode
	{
		// Token: 0x0400169E RID: 5790
		Lerp,
		// Token: 0x0400169F RID: 5791
		TrapBox,
		// Token: 0x040016A0 RID: 5792
		Relative,
		// Token: 0x040016A1 RID: 5793
		Platforming,
		// Token: 0x040016A2 RID: 5794
		Path,
		// Token: 0x040016A3 RID: 5795
		RelativeRook,
		// Token: 0x040016A4 RID: 5796
		RelativeRumRunners,
		// Token: 0x040016A5 RID: 5797
		Static = 10000
	}
}
