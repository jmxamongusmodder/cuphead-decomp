using System;
using System.Collections;
using UnityEngine;

// Token: 0x0200093C RID: 2364
public class MapLevelDependentObstacle : AbstractMapLevelDependentEntity
{
	// Token: 0x06003751 RID: 14161 RVA: 0x001FA8B7 File Offset: 0x001F8CB7
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06003752 RID: 14162 RVA: 0x001FA8BF File Offset: 0x001F8CBF
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

	// Token: 0x06003753 RID: 14163 RVA: 0x001FA8FB File Offset: 0x001F8CFB
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

	// Token: 0x06003754 RID: 14164 RVA: 0x001FA937 File Offset: 0x001F8D37
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

	// Token: 0x06003755 RID: 14165 RVA: 0x001FA973 File Offset: 0x001F8D73
	public override void DoTransition()
	{
		base.StartCoroutine(this.transition_cr());
	}

	// Token: 0x06003756 RID: 14166 RVA: 0x001FA982 File Offset: 0x001F8D82
	public void OnChange()
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

	// Token: 0x06003757 RID: 14167 RVA: 0x001FA9C0 File Offset: 0x001F8DC0
	private IEnumerator transition_cr()
	{
		AudioManager.Play("world_level_bridge_building_poof");
		this.poofPrefab.Create(this.poofRoot.position, new Vector3(0.01f, 0.01f, 1f));
		yield return CupheadTime.WaitForSeconds(this, 0.25f);
		SpriteRenderer[] sprites = base.GetComponentsInChildren<SpriteRenderer>(true);
		foreach (SpriteRenderer spriteRenderer in sprites)
		{
			spriteRenderer.material = this.flashMaterial;
		}
		if (!this.seeDisabledOnlyDuringTransition)
		{
			this.ToEnable.SetActive(true);
		}
		if (this.seeEnableOnlyDuringTransition)
		{
			this.ToDisable.SetActive(false);
		}
		yield return CupheadTime.WaitForSeconds(this, 0.04f);
		for (int i = 0; i < 4; i++)
		{
			foreach (SpriteRenderer spriteRenderer2 in sprites)
			{
				spriteRenderer2.color = Color.white;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.04f);
			foreach (SpriteRenderer spriteRenderer3 in sprites)
			{
				spriteRenderer3.color = Color.black;
			}
			yield return CupheadTime.WaitForSeconds(this, 0.04f);
		}
		if (this.seeDisabledOnlyDuringTransition)
		{
			this.ToEnable.SetActive(true);
		}
		if (!this.seeEnableOnlyDuringTransition && this.ToDisable != null)
		{
			this.ToDisable.SetActive(false);
		}
		base.CurrentState = AbstractMapLevelDependentEntity.State.Complete;
		yield break;
	}

	// Token: 0x04003F6B RID: 16235
	[SerializeField]
	private GameObject ToEnable;

	// Token: 0x04003F6C RID: 16236
	[SerializeField]
	private bool seeEnableOnlyDuringTransition;

	// Token: 0x04003F6D RID: 16237
	[SerializeField]
	private GameObject ToDisable;

	// Token: 0x04003F6E RID: 16238
	[SerializeField]
	private bool seeDisabledOnlyDuringTransition;

	// Token: 0x04003F6F RID: 16239
	[SerializeField]
	private Effect poofPrefab;

	// Token: 0x04003F70 RID: 16240
	[SerializeField]
	private Material flashMaterial;

	// Token: 0x04003F71 RID: 16241
	[SerializeField]
	private Transform poofRoot;

	// Token: 0x04003F72 RID: 16242
	[SerializeField]
	private bool DontPlayPoofSFX;
}
