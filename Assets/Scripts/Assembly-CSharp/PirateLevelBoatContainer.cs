using System;
using UnityEngine;

// Token: 0x0200071E RID: 1822
public class PirateLevelBoatContainer : AbstractPausableComponent
{
	// Token: 0x060027AE RID: 10158 RVA: 0x00173FF6 File Offset: 0x001723F6
	protected override void Awake()
	{
		base.Awake();
		this.startPos = base.transform.position;
		this.endPos = this.startPos + new Vector3(0f, this.targetY, 0f);
	}

	// Token: 0x060027AF RID: 10159 RVA: 0x00174038 File Offset: 0x00172438
	private void Update()
	{
		if (PauseManager.state == PauseManager.State.Paused || this.state == PirateLevelBoatContainer.State.Static)
		{
			return;
		}
		float t = EaseUtils.Ease(EaseUtils.EaseType.easeInOutSine, 0f, 1f, Mathf.PingPong(this.time, 1f));
		base.transform.position = Vector3.Lerp(this.startPos, this.endPos, t);
		this.time += Time.deltaTime / 2f;
	}

	// Token: 0x060027B0 RID: 10160 RVA: 0x001740B4 File Offset: 0x001724B4
	public void EndBobbing()
	{
		base.transform.position = this.startPos;
		this.state = PirateLevelBoatContainer.State.Static;
	}

	// Token: 0x0400306B RID: 12395
	[SerializeField]
	private float targetY;

	// Token: 0x0400306C RID: 12396
	private PirateLevelBoatContainer.State state;

	// Token: 0x0400306D RID: 12397
	private Vector3 startPos;

	// Token: 0x0400306E RID: 12398
	private Vector3 endPos;

	// Token: 0x0400306F RID: 12399
	private float time;

	// Token: 0x0200071F RID: 1823
	public enum State
	{
		// Token: 0x04003071 RID: 12401
		Bobbing,
		// Token: 0x04003072 RID: 12402
		ToStatic,
		// Token: 0x04003073 RID: 12403
		Static
	}
}
