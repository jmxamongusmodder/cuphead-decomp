using System;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000837 RID: 2103
public class TutorialShmupLevelParryNext : AbstractCollidableObject
{
	// Token: 0x060030C4 RID: 12484 RVA: 0x001CB264 File Offset: 0x001C9664
	private void Start()
	{
		this.parrySwitch.OnActivate += this.SetNextParry;
		if (this.startAsParry)
		{
			this.image.enabled = true;
			this.parrySwitch.enabled = true;
		}
		else
		{
			this.image.enabled = false;
			this.parrySwitch.enabled = false;
		}
	}

	// Token: 0x060030C5 RID: 12485 RVA: 0x001CB2C8 File Offset: 0x001C96C8
	private void SetNextParry()
	{
		this.nextSphere.parrySwitch.enabled = true;
		this.parrySwitch.enabled = false;
		if (this.lastPlayerController != null)
		{
			this.lastPlayerController.stats.OnParry(1f, true);
			this.lastPlayerController = null;
		}
		this.image.enabled = false;
		this.nextSphere.image.enabled = true;
	}

	// Token: 0x060030C6 RID: 12486 RVA: 0x001CB340 File Offset: 0x001C9740
	protected override void OnCollisionOther(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionOther(hit, phase);
		if (hit.transform && hit.transform.parent)
		{
			this.lastPlayerController = hit.transform.parent.GetComponent<AbstractPlayerController>();
		}
	}

	// Token: 0x04003960 RID: 14688
	[SerializeField]
	private TutorialShmupLevelParryNext nextSphere;

	// Token: 0x04003961 RID: 14689
	[SerializeField]
	private Image image;

	// Token: 0x04003962 RID: 14690
	[SerializeField]
	private bool startAsParry;

	// Token: 0x04003963 RID: 14691
	[SerializeField]
	private ParrySwitch parrySwitch;

	// Token: 0x04003964 RID: 14692
	private AbstractPlayerController lastPlayerController;
}
