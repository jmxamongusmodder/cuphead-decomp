using System;

// Token: 0x02000987 RID: 2439
public class LevelEquipUI : AbstractEquipUI
{
	// Token: 0x06003906 RID: 14598 RVA: 0x00205F39 File Offset: 0x00204339
	private void Start()
	{
		base.gameObject.SetActive(false);
	}

	// Token: 0x06003907 RID: 14599 RVA: 0x00205F47 File Offset: 0x00204347
	public void Activate()
	{
		base.StartCoroutine(base.pause_cr());
	}

	// Token: 0x06003908 RID: 14600 RVA: 0x00205F56 File Offset: 0x00204356
	protected override void Unpause()
	{
		LevelGameOverGUI.Current.ReactivateOnChangeEquipmentClosed();
		base.StartCoroutine(base.unpause_cr());
	}

	// Token: 0x06003909 RID: 14601 RVA: 0x00205F6F File Offset: 0x0020436F
	protected override void OnPauseSound()
	{
	}

	// Token: 0x0600390A RID: 14602 RVA: 0x00205F71 File Offset: 0x00204371
	protected override void OnUnpauseSound()
	{
	}

	// Token: 0x0600390B RID: 14603 RVA: 0x00205F73 File Offset: 0x00204373
	protected override void PauseGameplay()
	{
	}

	// Token: 0x0600390C RID: 14604 RVA: 0x00205F75 File Offset: 0x00204375
	protected override void UnpauseGameplay()
	{
	}
}
