using System;
using UnityEngine;

// Token: 0x02000832 RID: 2098
public class TutorialLevelDamagableBox : AbstractCollidableObject
{
	// Token: 0x060030AB RID: 12459 RVA: 0x001CA1BE File Offset: 0x001C85BE
	private void Start()
	{
		base.GetComponent<DamageReceiver>().OnDamageTaken += this.OnDamageTaken;
	}

	// Token: 0x060030AC RID: 12460 RVA: 0x001CA1D7 File Offset: 0x001C85D7
	protected override void OnDrawGizmosSelected()
	{
		base.OnDrawGizmosSelected();
		Gizmos.color = Color.red;
		Gizmos.DrawWireSphere(this.explosionPosition + base.transform.position, 10f);
	}

	// Token: 0x060030AD RID: 12461 RVA: 0x001CA20C File Offset: 0x001C860C
	private void OnDamageTaken(DamageDealer.DamageInfo info)
	{
		this.boxHealth -= info.damage;
		if (this.boxHealth <= 0f)
		{
			this.toEnable.SetActive(true);
			this.toDisable.SetActive(false);
			this.explosionPrefab.Create(this.explosionPosition + base.transform.position);
			AudioManager.Play("sfx_object_explode");
			this.emitAudioFromObject.Add("sfx_object_explode");
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	// Token: 0x0400394D RID: 14669
	[SerializeField]
	private float boxHealth = 20f;

	// Token: 0x0400394E RID: 14670
	[SerializeField]
	private GameObject toDisable;

	// Token: 0x0400394F RID: 14671
	[SerializeField]
	private GameObject toEnable;

	// Token: 0x04003950 RID: 14672
	[SerializeField]
	private PlatformingLevelGenericExplosion explosionPrefab;

	// Token: 0x04003951 RID: 14673
	[SerializeField]
	private Vector3 explosionPosition;
}
