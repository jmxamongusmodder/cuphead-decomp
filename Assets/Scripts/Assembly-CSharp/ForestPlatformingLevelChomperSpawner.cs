using System;
using UnityEngine;

// Token: 0x0200087D RID: 2173
public class ForestPlatformingLevelChomperSpawner : AbstractPausableComponent
{
	// Token: 0x06003274 RID: 12916 RVA: 0x001D628C File Offset: 0x001D468C
	private void Start()
	{
		this.started = false;
		Vector2 vector = base.transform.position;
		this.startRect = RectUtils.NewFromCenter(this.startTrigger.Position.x + vector.x, this.startTrigger.Position.y + vector.y, this.startTrigger.Size.x + UnityEngine.Random.Range(-this.startTrigger.xVariation, this.startTrigger.xVariation), this.startTrigger.Size.y);
	}

	// Token: 0x06003275 RID: 12917 RVA: 0x001D632C File Offset: 0x001D472C
	private void Update()
	{
		if (this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnStartTriggerHit();
		}
	}

	// Token: 0x06003276 RID: 12918 RVA: 0x001D6388 File Offset: 0x001D4788
	private void OnStartTriggerHit()
	{
		if (this.started)
		{
			return;
		}
		this.started = true;
		foreach (ForestPlatformingLevelChomper forestPlatformingLevelChomper in this.chompers)
		{
			if (forestPlatformingLevelChomper != null)
			{
				forestPlatformingLevelChomper.StartAttacking();
			}
		}
	}

	// Token: 0x06003277 RID: 12919 RVA: 0x001D63D9 File Offset: 0x001D47D9
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x06003278 RID: 12920 RVA: 0x001D63EC File Offset: 0x001D47EC
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x06003279 RID: 12921 RVA: 0x001D6400 File Offset: 0x001D4800
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.startTrigger.Position, this.startTrigger.Size);
	}

	// Token: 0x04003AD7 RID: 15063
	[Header("Triggers")]
	public ForestPlatformingLevelChomperSpawner.TriggerProperties startTrigger = new ForestPlatformingLevelChomperSpawner.TriggerProperties(new Vector2(-200f, 0f));

	// Token: 0x04003AD8 RID: 15064
	[Header("Chompers")]
	public ForestPlatformingLevelChomper[] chompers;

	// Token: 0x04003AD9 RID: 15065
	private bool started;

	// Token: 0x04003ADA RID: 15066
	private Rect startRect;

	// Token: 0x0200087E RID: 2174
	[Serializable]
	public class TriggerProperties
	{
		// Token: 0x0600327A RID: 12922 RVA: 0x001D645C File Offset: 0x001D485C
		public TriggerProperties(Vector2 position)
		{
			this.Position = position;
		}

		// Token: 0x04003ADB RID: 15067
		public Vector2 Position = Vector2.zero;

		// Token: 0x04003ADC RID: 15068
		public Vector2 Size = Vector2.one * 100f;

		// Token: 0x04003ADD RID: 15069
		public float xVariation;
	}
}
