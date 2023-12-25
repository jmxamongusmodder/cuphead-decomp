using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x020006AF RID: 1711
public class FrogsLevelMorphedSlot : AbstractPausableComponent
{
	// Token: 0x170003AE RID: 942
	// (get) Token: 0x06002451 RID: 9297 RVA: 0x001552B0 File Offset: 0x001536B0
	// (set) Token: 0x06002452 RID: 9298 RVA: 0x001552B8 File Offset: 0x001536B8
	public Slots.Mode mode { get; private set; }

	// Token: 0x170003AF RID: 943
	// (get) Token: 0x06002453 RID: 9299 RVA: 0x001552C1 File Offset: 0x001536C1
	// (set) Token: 0x06002454 RID: 9300 RVA: 0x001552C9 File Offset: 0x001536C9
	public FrogsLevelMorphedSlot.State state { get; private set; }

	// Token: 0x170003B0 RID: 944
	// (get) Token: 0x06002455 RID: 9301 RVA: 0x001552D2 File Offset: 0x001536D2
	// (set) Token: 0x06002456 RID: 9302 RVA: 0x001552DA File Offset: 0x001536DA
	public FrogsLevelMorphedSlot.Action action { get; private set; }

	// Token: 0x06002457 RID: 9303 RVA: 0x001552E4 File Offset: 0x001536E4
	protected override void Awake()
	{
		base.Awake();
		this.offsets = new Dictionary<Slots.Mode, float>();
		this.offsets[Slots.Mode.Snake] = 0.4f;
		this.offsets[Slots.Mode.Tiger] = -0.095f;
		this.offsets[Slots.Mode.Bison] = 0.095f;
		this.offsets[Slots.Mode.Oni] = -0.4f;
		this.mat = base.GetComponent<Renderer>().material;
		this.SetTexture(this.textures.Get(FrogsLevelMorphedSlot.State.Normal, 0));
		this.SetOffset(this.offsets[Slots.Mode.Snake]);
	}

	// Token: 0x06002458 RID: 9304 RVA: 0x0015537C File Offset: 0x0015377C
	private void Start()
	{
		base.StartCoroutine(this.animate_cr());
	}

	// Token: 0x06002459 RID: 9305 RVA: 0x0015538B File Offset: 0x0015378B
	private void SetTexture(Texture2D texture)
	{
		this.mat.mainTexture = texture;
	}

	// Token: 0x0600245A RID: 9306 RVA: 0x0015539C File Offset: 0x0015379C
	private void SetOffset(float y)
	{
		Vector2 mainTextureOffset = this.mat.mainTextureOffset;
		mainTextureOffset.y = y;
		this.mat.mainTextureOffset = mainTextureOffset;
	}

	// Token: 0x0600245B RID: 9307 RVA: 0x001553C9 File Offset: 0x001537C9
	public void StartSpin()
	{
		AudioManager.PlayLoop("level_frogs_morphed_spin_loop");
		this.emitAudioFromObject.Add("level_frogs_morphed_spin_loop");
		this.StopAllCoroutines();
		base.StartCoroutine(this.animate_cr());
		base.StartCoroutine(this.spin_cr());
	}

	// Token: 0x0600245C RID: 9308 RVA: 0x00155408 File Offset: 0x00153808
	public void StopSpin(Slots.Mode mode)
	{
		AudioManager.Stop("level_frogs_morphed_spin_loop");
		this.emitAudioFromObject.Add("level_frogs_morphed_spin_loop");
		AudioManager.Play("level_frogs_morphed_spin");
		this.emitAudioFromObject.Add("level_frogs_morphed_spin");
		this.mode = mode;
		this.action = FrogsLevelMorphedSlot.Action.Ending;
	}

	// Token: 0x0600245D RID: 9309 RVA: 0x00155457 File Offset: 0x00153857
	public void Flash()
	{
		base.StartCoroutine(this.flash_cr());
	}

	// Token: 0x0600245E RID: 9310 RVA: 0x00155468 File Offset: 0x00153868
	private IEnumerator animate_cr()
	{
		int frame = 0;
		for (;;)
		{
			yield return CupheadTime.WaitForSeconds(this, 0.06f);
			this.SetTexture(this.textures.Get(this.state, frame));
			frame = (int)Mathf.Repeat((float)(frame + 1), 3f);
		}
		yield break;
	}

	// Token: 0x0600245F RID: 9311 RVA: 0x00155484 File Offset: 0x00153884
	private IEnumerator spin_cr()
	{
		float offset = this.mat.mainTextureOffset.y;
		this.action = FrogsLevelMorphedSlot.Action.Spinning;
		while (this.action == FrogsLevelMorphedSlot.Action.Spinning)
		{
			offset = Mathf.Repeat(offset + 5f * CupheadTime.Delta, 1f);
			this.SetOffset(offset);
			yield return null;
		}
		float t = 0f;
		this.SetOffset(-3f);
		float startOffset = this.mat.mainTextureOffset.y;
		while (t < 1f)
		{
			float val = t / 1f;
			float o = EaseUtils.Ease(EaseUtils.EaseType.easeOutElastic, startOffset, this.offsets[this.mode], val);
			this.SetOffset(o);
			t += CupheadTime.Delta;
			yield return null;
		}
		yield break;
	}

	// Token: 0x06002460 RID: 9312 RVA: 0x001554A0 File Offset: 0x001538A0
	private IEnumerator flash_cr()
	{
		this.state = FrogsLevelMorphedSlot.State.Flashing;
		yield return CupheadTime.WaitForSeconds(this, 0.2f);
		this.state = FrogsLevelMorphedSlot.State.Normal;
		yield break;
	}

	// Token: 0x06002461 RID: 9313 RVA: 0x001554BB File Offset: 0x001538BB
	protected override void OnDestroy()
	{
		base.OnDestroy();
		UnityEngine.Object.Destroy(this.mat);
		this.textures.flashing = null;
		this.textures.normal = null;
	}

	// Token: 0x04002D0C RID: 11532
	private const float STOP_OFFSET = 3f;

	// Token: 0x04002D0D RID: 11533
	private const float STOP_TIME = 1f;

	// Token: 0x04002D0E RID: 11534
	private const float OFFSET_SPEED = 5f;

	// Token: 0x04002D0F RID: 11535
	private const float FLASH_TIME = 0.2f;

	// Token: 0x04002D10 RID: 11536
	[SerializeField]
	private FrogsLevelMorphedSlot.Textures textures;

	// Token: 0x04002D14 RID: 11540
	private Material mat;

	// Token: 0x04002D15 RID: 11541
	private Dictionary<Slots.Mode, float> offsets;

	// Token: 0x020006B0 RID: 1712
	public enum State
	{
		// Token: 0x04002D17 RID: 11543
		Normal,
		// Token: 0x04002D18 RID: 11544
		Flashing
	}

	// Token: 0x020006B1 RID: 1713
	public enum Action
	{
		// Token: 0x04002D1A RID: 11546
		Static,
		// Token: 0x04002D1B RID: 11547
		Spinning,
		// Token: 0x04002D1C RID: 11548
		Ending
	}

	// Token: 0x020006B2 RID: 1714
	[Serializable]
	public class Textures
	{
		// Token: 0x06002463 RID: 9315 RVA: 0x001554EE File Offset: 0x001538EE
		public Texture2D Get(FrogsLevelMorphedSlot.State state, int frame)
		{
			if (state == FrogsLevelMorphedSlot.State.Normal || state != FrogsLevelMorphedSlot.State.Flashing)
			{
				return this.normal[frame];
			}
			return this.flashing[frame];
		}

		// Token: 0x04002D1D RID: 11549
		public Texture2D[] normal;

		// Token: 0x04002D1E RID: 11550
		public Texture2D[] flashing;
	}
}
