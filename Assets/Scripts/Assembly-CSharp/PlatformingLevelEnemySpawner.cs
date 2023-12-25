using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200085D RID: 2141
public abstract class PlatformingLevelEnemySpawner : AbstractPausableComponent
{
	// Token: 0x060031BE RID: 12734 RVA: 0x001D0FD8 File Offset: 0x001CF3D8
	protected virtual void Start()
	{
		this.started = false;
		this.ended = false;
		Vector2 vector = base.transform.position;
		this.startRect = RectUtils.NewFromCenter(this.startTrigger.Position.x + vector.x, this.startTrigger.Position.y + vector.y, this.startTrigger.Size.x, this.startTrigger.Size.y);
		this.stopRect = RectUtils.NewFromCenter(this.stopTrigger.Position.x + vector.x, this.stopTrigger.Position.y + vector.y, this.stopTrigger.Size.x, this.stopTrigger.Size.y);
	}

	// Token: 0x060031BF RID: 12735 RVA: 0x001D10BC File Offset: 0x001CF4BC
	private void Update()
	{
		if (this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.startRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnStartTriggerHit();
		}
		if (this.stopRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerOne).center) || (PlayerManager.GetPlayer(PlayerId.PlayerTwo) != null && this.stopRect.Contains(PlayerManager.GetPlayer(PlayerId.PlayerTwo).center)))
		{
			this.OnStopTriggerHit();
		}
	}

	// Token: 0x060031C0 RID: 12736 RVA: 0x001D1163 File Offset: 0x001CF563
	private void OnStartTriggerHit()
	{
		if (this.started)
		{
			return;
		}
		this.started = true;
		this.StartSpawning();
		base.StartCoroutine(this.loop_cr());
	}

	// Token: 0x060031C1 RID: 12737 RVA: 0x001D118B File Offset: 0x001CF58B
	private void OnStopTriggerHit()
	{
		if (!this.started)
		{
			return;
		}
		if (this.ended)
		{
			return;
		}
		this.ended = true;
		this.EndSpawning();
		this.StopAllCoroutines();
	}

	// Token: 0x060031C2 RID: 12738 RVA: 0x001D11B8 File Offset: 0x001CF5B8
	protected virtual void EndSpawning()
	{
	}

	// Token: 0x060031C3 RID: 12739 RVA: 0x001D11BA File Offset: 0x001CF5BA
	protected virtual void StartSpawning()
	{
	}

	// Token: 0x060031C4 RID: 12740 RVA: 0x001D11BC File Offset: 0x001CF5BC
	protected virtual void Spawn()
	{
	}

	// Token: 0x060031C5 RID: 12741 RVA: 0x001D11C0 File Offset: 0x001CF5C0
	private IEnumerator loop_cr()
	{
		yield return CupheadTime.WaitForSeconds(this, this.initalSpawnDelay.RandomFloat());
		for (;;)
		{
			this.Spawn();
			yield return CupheadTime.WaitForSeconds(this, this.spawnDelay.RandomFloat());
		}
		yield break;
	}

	// Token: 0x060031C6 RID: 12742 RVA: 0x001D11DB File Offset: 0x001CF5DB
	protected override void OnDrawGizmos()
	{
		base.OnDrawGizmos();
		this.DrawGizmos(0.2f);
	}

	// Token: 0x060031C7 RID: 12743 RVA: 0x001D11EE File Offset: 0x001CF5EE
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		this.DrawGizmos(1f);
	}

	// Token: 0x060031C8 RID: 12744 RVA: 0x001D1204 File Offset: 0x001CF604
	private void DrawGizmos(float a)
	{
		Gizmos.color = new Color(0f, 1f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.startTrigger.Position, this.startTrigger.Size);
		Gizmos.color = new Color(1f, 0f, 0f, a);
		Gizmos.DrawWireCube(base.baseTransform.position + this.stopTrigger.Position, this.stopTrigger.Size);
	}

	// Token: 0x04003A1E RID: 14878
	public bool destroyEnemyAfterLeavingScreen = true;

	// Token: 0x04003A1F RID: 14879
	[Header("Spawning Properties")]
	public MinMax spawnDelay = new MinMax(2f, 2f);

	// Token: 0x04003A20 RID: 14880
	public MinMax initalSpawnDelay = new MinMax(0f, 0f);

	// Token: 0x04003A21 RID: 14881
	[Header("Triggers")]
	public PlatformingLevelEnemySpawner.TriggerProperties startTrigger = new PlatformingLevelEnemySpawner.TriggerProperties(new Vector2(-200f, 0f));

	// Token: 0x04003A22 RID: 14882
	public PlatformingLevelEnemySpawner.TriggerProperties stopTrigger = new PlatformingLevelEnemySpawner.TriggerProperties(new Vector2(200f, 0f));

	// Token: 0x04003A23 RID: 14883
	private bool started;

	// Token: 0x04003A24 RID: 14884
	private bool ended;

	// Token: 0x04003A25 RID: 14885
	private Rect startRect;

	// Token: 0x04003A26 RID: 14886
	private Rect stopRect;

	// Token: 0x0200085E RID: 2142
	[Serializable]
	public class TriggerProperties
	{
		// Token: 0x060031C9 RID: 12745 RVA: 0x001D12AF File Offset: 0x001CF6AF
		public TriggerProperties(Vector2 position)
		{
			this.Position = position;
		}

		// Token: 0x04003A27 RID: 14887
		public Vector2 Position = Vector2.zero;

		// Token: 0x04003A28 RID: 14888
		public Vector2 Size = Vector2.one * 100f;
	}
}
