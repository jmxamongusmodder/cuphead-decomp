using System;
using UnityEngine;

// Token: 0x02000834 RID: 2100
public class TutorialLevelParryNext : AbstractCollidableObject
{
	// Token: 0x060030B3 RID: 12467 RVA: 0x001CA4C0 File Offset: 0x001C88C0
	private void Start()
	{
		this.parrySwitch.OnActivate += this.SetNextParry;
		if (this.startAsParry)
		{
			this.spriteRenderer.sprite = this.parrySprite;
			this.spriteRenderer.sharedMaterial = this.parryMaterial;
			this.parrySwitch.enabled = true;
		}
		else
		{
			this.spriteRenderer.sprite = this.normalSprite;
			this.spriteRenderer.sharedMaterial = this.normalMaterial;
			this.parrySwitch.enabled = false;
		}
	}

	// Token: 0x060030B4 RID: 12468 RVA: 0x001CA550 File Offset: 0x001C8950
	private void SetNextParry()
	{
		this.nextSphere.parrySwitch.enabled = true;
		this.parrySwitch.enabled = false;
		if (this.lastPlayerController != null)
		{
			this.lastPlayerController.stats.OnParry(1f, true);
			this.lastPlayerController = null;
		}
		this.spriteRenderer.sprite = this.normalSprite;
		this.spriteRenderer.sharedMaterial = this.normalMaterial;
		this.nextSphere.spriteRenderer.sprite = this.nextSphere.parrySprite;
		this.nextSphere.spriteRenderer.sharedMaterial = this.parryMaterial;
	}

	// Token: 0x060030B5 RID: 12469 RVA: 0x001CA5FC File Offset: 0x001C89FC
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.transform && hit.transform.parent)
		{
			this.lastPlayerController = hit.transform.parent.GetComponent<AbstractPlayerController>();
		}
	}

	// Token: 0x04003954 RID: 14676
	[SerializeField]
	private TutorialLevelParryNext nextSphere;

	// Token: 0x04003955 RID: 14677
	[SerializeField]
	private SpriteRenderer spriteRenderer;

	// Token: 0x04003956 RID: 14678
	[SerializeField]
	private Sprite normalSprite;

	// Token: 0x04003957 RID: 14679
	[SerializeField]
	private Sprite parrySprite;

	// Token: 0x04003958 RID: 14680
	[SerializeField]
	private Material normalMaterial;

	// Token: 0x04003959 RID: 14681
	[SerializeField]
	private Material parryMaterial;

	// Token: 0x0400395A RID: 14682
	[SerializeField]
	private bool startAsParry;

	// Token: 0x0400395B RID: 14683
	[SerializeField]
	private ParrySwitch parrySwitch;

	// Token: 0x0400395C RID: 14684
	private AbstractPlayerController lastPlayerController;
}
