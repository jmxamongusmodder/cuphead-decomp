using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000941 RID: 2369
public class MapLevelMausoleumEntity : AbstractMapLevelDependentEntity
{
	// Token: 0x06003766 RID: 14182 RVA: 0x001FDC7C File Offset: 0x001FC07C
	public override void OnConditionNotMet()
	{
		if (this.ToEnable != null)
		{
			this.ToEnable.SetActive(false);
		}
		if (this.ToDisable != null)
		{
			this.ToDisable.SetActive(true);
		}
	}

	// Token: 0x06003767 RID: 14183 RVA: 0x001FDCB8 File Offset: 0x001FC0B8
	public override void OnConditionMet()
	{
		if (this.ToEnable != null)
		{
			this.ToEnable.SetActive(false);
		}
		if (this.ToDisable != null)
		{
			this.ToDisable.SetActive(true);
		}
	}

	// Token: 0x06003768 RID: 14184 RVA: 0x001FDCF4 File Offset: 0x001FC0F4
	public override void OnConditionAlreadyMet()
	{
		if (this.ToEnable != null)
		{
			this.ToEnable.SetActive(true);
		}
		if (this.ToDisable != null)
		{
			this.ToDisable.SetActive(false);
		}
	}

	// Token: 0x06003769 RID: 14185 RVA: 0x001FDD30 File Offset: 0x001FC130
	protected override bool ValidateSucess()
	{
		return PlayerData.Data.IsUnlocked(PlayerId.PlayerOne, this.superUnlock) && PlayerData.Data.IsUnlocked(PlayerId.PlayerTwo, this.superUnlock);
	}

	// Token: 0x0600376A RID: 14186 RVA: 0x001FDD5C File Offset: 0x001FC15C
	protected override bool ValidateCondition(Levels level)
	{
		return Level.Won && Level.PreviousLevel == level && Level.SuperUnlocked;
	}

	// Token: 0x0600376B RID: 14187 RVA: 0x001FDD84 File Offset: 0x001FC184
	public override void DoTransition()
	{
		base.StartCoroutine(this.transition_cr());
	}

	// Token: 0x0600376C RID: 14188 RVA: 0x001FDD94 File Offset: 0x001FC194
	private IEnumerator transition_cr()
	{
		this.poofPrefab.Create(this.poofRoot.position, new Vector3(0.01f, 0.01f, 1f));
		AudioManager.Play("world_map_mausoleum_destruction");
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		this.ToEnable.SetActive(true);
		this.ToDisable.SetActive(false);
		yield return CupheadTime.WaitForSeconds(this, 0.36f);
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield break;
	}

	// Token: 0x04003F7B RID: 16251
	[SerializeField]
	private GameObject ToEnable;

	// Token: 0x04003F7C RID: 16252
	[SerializeField]
	private GameObject ToDisable;

	// Token: 0x04003F7D RID: 16253
	[SerializeField]
	private Effect poofPrefab;

	// Token: 0x04003F7E RID: 16254
	[SerializeField]
	private Transform poofRoot;

	// Token: 0x04003F7F RID: 16255
	[SerializeField]
	private Super superUnlock;
}
