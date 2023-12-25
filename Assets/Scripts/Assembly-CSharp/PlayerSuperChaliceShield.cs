using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A56 RID: 2646
public class PlayerSuperChaliceShield : AbstractPlayerSuper
{
	// Token: 0x06003F10 RID: 16144 RVA: 0x002289D9 File Offset: 0x00226DD9
	protected override void Awake()
	{
		base.Awake();
		base.tag = "Untagged";
	}

	// Token: 0x06003F11 RID: 16145 RVA: 0x002289EC File Offset: 0x00226DEC
	protected override void StartSuper()
	{
		this.player.weaponManager.OnSuperStart -= this.player.motor.StartSuper;
		if (this.player.motor.Grounded)
		{
			this.player.weaponManager.OnSuperEnd -= this.player.motor.OnSuperEnd;
		}
		base.StartSuper();
		AudioManager.Play("player_super_chalice_shield");
		base.StartCoroutine(this.super_cr());
		Level.ScoringData.superMeterUsed += 5;
	}

	// Token: 0x06003F12 RID: 16146 RVA: 0x00228A8C File Offset: 0x00226E8C
	private void CreateHeart()
	{
		this.shieldHeart = UnityEngine.Object.Instantiate<GameObject>(this.shieldHeartPrefab);
		this.shieldHeart.transform.position = this.shieldHeartSpawnPos.position;
		this.shieldHeartScript = this.shieldHeart.GetComponent<PlayerSuperChaliceShieldHeart>();
		this.shieldHeartScript.player = this.player.transform;
		this.player.stats.SetChaliceShield(true);
		this.player.damageReceiver.Invulnerable(0.1f);
	}

	// Token: 0x06003F13 RID: 16147 RVA: 0x00228B12 File Offset: 0x00226F12
	private void LetPlayerMove()
	{
		this.Fire();
		this.EndSuper(true);
	}

	// Token: 0x06003F14 RID: 16148 RVA: 0x00228B24 File Offset: 0x00226F24
	private IEnumerator super_cr()
	{
		if (!this.player.motor.Grounded)
		{
			base.animator.Play("SuperAir");
		}
		while (this.player && !this.player.stats.ChaliceShieldOn)
		{
			yield return null;
		}
		while (this.player && this.player.stats.ChaliceShieldOn)
		{
			yield return null;
		}
		this.shieldHeartScript.Destroy();
		if (this.player)
		{
			this.player.damageReceiver.OnRevive(Vector3.zero);
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x04004626 RID: 17958
	[SerializeField]
	private Vector3 shadowOffset;

	// Token: 0x04004627 RID: 17959
	[SerializeField]
	private GameObject shieldHeartPrefab;

	// Token: 0x04004628 RID: 17960
	private GameObject shieldHeart;

	// Token: 0x04004629 RID: 17961
	[SerializeField]
	private Transform shieldHeartSpawnPos;

	// Token: 0x0400462A RID: 17962
	private PlayerSuperChaliceShieldHeart shieldHeartScript;
}
