using System;
using UnityEngine;

// Token: 0x02000527 RID: 1319
public class ChaliceTutorialLevelParryable : ParrySwitch
{
	// Token: 0x1700032D RID: 813
	// (get) Token: 0x060017BF RID: 6079 RVA: 0x000D5E06 File Offset: 0x000D4206
	// (set) Token: 0x060017C0 RID: 6080 RVA: 0x000D5E0E File Offset: 0x000D420E
	public bool isDeactivated { get; private set; }

	// Token: 0x060017C1 RID: 6081 RVA: 0x000D5E17 File Offset: 0x000D4217
	protected override void Awake()
	{
		base.Awake();
		this.Deactivated();
	}

	// Token: 0x060017C2 RID: 6082 RVA: 0x000D5E25 File Offset: 0x000D4225
	public override void OnParryPostPause(AbstractPlayerController player)
	{
		base.OnParryPostPause(player);
		this.Deactivated();
	}

	// Token: 0x060017C3 RID: 6083 RVA: 0x000D5E34 File Offset: 0x000D4234
	public override void OnParryPrePause(AbstractPlayerController player)
	{
		base.OnParryPrePause(player);
		AudioManager.Play("sfx_parry_pink_shows");
	}

	// Token: 0x060017C4 RID: 6084 RVA: 0x000D5E47 File Offset: 0x000D4247
	public void Deactivated()
	{
		base.GetComponent<Collider2D>().enabled = false;
		this.isDeactivated = true;
	}

	// Token: 0x060017C5 RID: 6085 RVA: 0x000D5E5C File Offset: 0x000D425C
	public void Activated()
	{
		base.GetComponent<Collider2D>().enabled = true;
		this.isDeactivated = false;
	}

	// Token: 0x060017C6 RID: 6086 RVA: 0x000D5E74 File Offset: 0x000D4274
	private void Update()
	{
		this.rend.color = new Color(1f, 1f, 1f, Mathf.Clamp(this.rend.color.a + ((!this.isDeactivated) ? CupheadTime.Delta : (-CupheadTime.Delta)) * 5f, 0f, 1f));
	}

	// Token: 0x040020EB RID: 8427
	private const float FADE_SPEED = 5f;

	// Token: 0x040020EC RID: 8428
	[SerializeField]
	private bool firstOne;

	// Token: 0x040020ED RID: 8429
	[SerializeField]
	private SpriteRenderer rend;
}
