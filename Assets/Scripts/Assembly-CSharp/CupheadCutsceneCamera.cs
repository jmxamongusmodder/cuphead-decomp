using System;
using UnityEngine;

// Token: 0x020003D6 RID: 982
public class CupheadCutsceneCamera : AbstractCupheadGameCamera
{
	// Token: 0x1700022D RID: 557
	// (get) Token: 0x06000CF1 RID: 3313 RVA: 0x0008A519 File Offset: 0x00088919
	// (set) Token: 0x06000CF2 RID: 3314 RVA: 0x0008A520 File Offset: 0x00088920
	public static CupheadCutsceneCamera Current { get; private set; }

	// Token: 0x1700022E RID: 558
	// (get) Token: 0x06000CF3 RID: 3315 RVA: 0x0008A528 File Offset: 0x00088928
	public override float OrthographicSize
	{
		get
		{
			return 360f;
		}
	}

	// Token: 0x06000CF4 RID: 3316 RVA: 0x0008A52F File Offset: 0x0008892F
	protected override void Awake()
	{
		base.Awake();
		CupheadCutsceneCamera.Current = this;
	}

	// Token: 0x06000CF5 RID: 3317 RVA: 0x0008A53D File Offset: 0x0008893D
	private void OnDestroy()
	{
		if (CupheadCutsceneCamera.Current == this)
		{
			CupheadCutsceneCamera.Current = null;
		}
	}

	// Token: 0x06000CF6 RID: 3318 RVA: 0x0008A555 File Offset: 0x00088955
	protected override void LateUpdate()
	{
		base.LateUpdate();
		base.Move();
	}

	// Token: 0x06000CF7 RID: 3319 RVA: 0x0008A563 File Offset: 0x00088963
	public void SetPosition(Vector3 newPos)
	{
		this._position = newPos;
	}

	// Token: 0x06000CF8 RID: 3320 RVA: 0x0008A56C File Offset: 0x0008896C
	public void Init()
	{
		base.enabled = true;
		Texture2D texture = this.CreateBorderTexture();
		if (!this.noBars)
		{
			Transform transform = new GameObject("Border").transform;
			this.CreateBorderRenderer(texture, transform, "Left");
			this.CreateBorderRenderer(texture, transform, "Right");
			this.CreateBorderRenderer(texture, transform, "Top");
			this.CreateBorderRenderer(texture, transform, "Bottom");
		}
		if (!this.noShake)
		{
			if (this.minimalShake)
			{
				base.StartSmoothShake(3f, 1.5f, 6);
			}
			else
			{
				base.StartSmoothShake(4f, 0.75f, 8);
			}
		}
	}

	// Token: 0x06000CF9 RID: 3321 RVA: 0x0008A618 File Offset: 0x00088A18
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
							zero = new Vector2(3280f, 1000f);
							zero2 = new Vector2(0f, -360f);
							zero3 = new Vector2(0.5f, 1f);
						}
					}
					else
					{
						zero = new Vector2(3280f, 1000f);
						zero2 = new Vector2(0f, 360f);
						zero3 = new Vector2(0.5f, 0f);
					}
				}
				else
				{
					zero = new Vector2(1000f, 2720f);
					zero2 = new Vector2(640f, 0f);
					zero3 = new Vector2(0f, 0.5f);
				}
			}
			else
			{
				zero = new Vector2(1000f, 2720f);
				zero2 = new Vector2(-640f, 0f);
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

	// Token: 0x06000CFA RID: 3322 RVA: 0x0008A804 File Offset: 0x00088C04
	private Texture2D CreateBorderTexture()
	{
		Texture2D texture2D = new Texture2D(1, 1);
		texture2D.filterMode = FilterMode.Point;
		texture2D.SetPixel(0, 0, Color.black);
		texture2D.Apply();
		return texture2D;
	}

	// Token: 0x0400165E RID: 5726
	public const string EDITOR_PATH = "Assets/_CUPHEAD/Prefabs/Camera/CutsceneCamera.prefab";

	// Token: 0x0400165F RID: 5727
	private const float BOUND_COLLIDER_SIZE = 400f;

	// Token: 0x04001660 RID: 5728
	private const float BORDER_THICKNESS = 1000f;

	// Token: 0x04001661 RID: 5729
	public const float LEFT = -640f;

	// Token: 0x04001662 RID: 5730
	public const float RIGHT = 640f;

	// Token: 0x04001663 RID: 5731
	public const float BOTTOM = -360f;

	// Token: 0x04001664 RID: 5732
	public const float TOP = 360f;

	// Token: 0x04001665 RID: 5733
	public bool noShake;

	// Token: 0x04001666 RID: 5734
	public bool minimalShake;

	// Token: 0x04001667 RID: 5735
	public bool noBars;

	// Token: 0x020003D7 RID: 983
	public enum Mode
	{
		// Token: 0x04001669 RID: 5737
		Lerp,
		// Token: 0x0400166A RID: 5738
		TrapBox,
		// Token: 0x0400166B RID: 5739
		Relative,
		// Token: 0x0400166C RID: 5740
		Platforming,
		// Token: 0x0400166D RID: 5741
		Static = 10000
	}
}
