using System;
using UnityEngine;

// Token: 0x02000A48 RID: 2632
public class WeaponChargeChargingEffect : AbstractMonoBehaviour
{
	// Token: 0x06003EBB RID: 16059 RVA: 0x002264CC File Offset: 0x002248CC
	public WeaponChargeChargingEffect Create(Vector2 pos)
	{
		WeaponChargeChargingEffect weaponChargeChargingEffect = this.InstantiatePrefab<WeaponChargeChargingEffect>();
		weaponChargeChargingEffect.transform.position = pos;
		return weaponChargeChargingEffect;
	}
}
