using System;
using UnityEngine;

// Token: 0x0200086D RID: 2157
public class PlatformingLevelPitMoveTrigger : AbstractPausableComponent
{
	// Token: 0x0600322C RID: 12844 RVA: 0x001D47B0 File Offset: 0x001D2BB0
	private void Start()
	{
		Vector2 vector = base.transform.position;
		this.rect = RectUtils.NewFromCenter(this.trigger.Position.x + vector.x, this.trigger.Position.y + vector.y, this.trigger.Size.x, this.trigger.Size.y);
	}

	// Token: 0x0600322D RID: 12845 RVA: 0x001D482C File Offset: 0x001D2C2C
	private void Update()
	{
		if (this.rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.rect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnTriggerHit();
		}
	}

	// Token: 0x0600322E RID: 12846 RVA: 0x001D4886 File Offset: 0x001D2C86
	private void OnTriggerHit()
	{
		LevelPit.Instance.ExtraOffset = this.pitOffset;
	}

	// Token: 0x0600322F RID: 12847 RVA: 0x001D4898 File Offset: 0x001D2C98
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003230 RID: 12848 RVA: 0x001D48AB File Offset: 0x001D2CAB
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06003231 RID: 12849 RVA: 0x001D48C0 File Offset: 0x001D2CC0
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.trigger.Position, this.trigger.Size);
	}

	// Token: 0x04003A85 RID: 14981
	[SerializeField]
	private float pitOffset;

	// Token: 0x04003A86 RID: 14982
	[Header("Triggers")]
	public PlatformingLevelPitMoveTrigger.TriggerProperties trigger = new PlatformingLevelPitMoveTrigger.TriggerProperties(new Vector2(-200f, 0f));

	// Token: 0x04003A87 RID: 14983
	private Rect rect;

	// Token: 0x0200086E RID: 2158
	[Serializable]
	public class TriggerProperties
	{
		// Token: 0x06003232 RID: 12850 RVA: 0x001D491C File Offset: 0x001D2D1C
		public TriggerProperties(Vector2 position)
		{
			this.Position = position;
		}

		// Token: 0x04003A88 RID: 14984
		public Vector2 Position = Vector2.zero;

		// Token: 0x04003A89 RID: 14985
		public Vector2 Size = Vector2.one * 100f;
	}
}
