using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000A50 RID: 2640
public class PlayerSuperChaliceBounce : AbstractPlayerSuper
{
	// Token: 0x06003EE5 RID: 16101 RVA: 0x00226F34 File Offset: 0x00225334
	protected override void Start()
	{
		base.Start();
	}

	// Token: 0x06003EE6 RID: 16102 RVA: 0x00226F3C File Offset: 0x0022533C
	protected override void StartSuper()
	{
		base.StartSuper();
		base.StartCoroutine(this.super_cr());
	}

	// Token: 0x06003EE7 RID: 16103 RVA: 0x00226F54 File Offset: 0x00225354
	private IEnumerator super_cr()
	{
		float duration = this.DURATION;
		this.timer = duration;
		this.Fire();
		if (this.LAUNCHED_VERSION)
		{
			yield return new WaitForEndOfFrame();
			this.player.animationController.EnableSpriteRenderer();
		}
		while (this.timer > 0f && !this.interrupted)
		{
			this.timer -= CupheadTime.FixedDelta;
			yield return null;
		}
		if (!this.LAUNCHED_VERSION)
		{
			this.EndSuper(true);
			this.player.transform.position = this.ball.transform.position;
		}
		this.CleanUp();
		yield break;
	}

	// Token: 0x06003EE8 RID: 16104 RVA: 0x00226F6F File Offset: 0x0022536F
	public void CleanUp()
	{
		UnityEngine.Object.Destroy(this.ball.gameObject);
		UnityEngine.Object.Destroy(base.gameObject);
	}

	// Token: 0x06003EE9 RID: 16105 RVA: 0x00226F8C File Offset: 0x0022538C
	protected override void Fire()
	{
		PauseManager.Unpause();
		if (!this.LAUNCHED_VERSION)
		{
			this.player.PauseAll();
			AnimationHelper component = base.GetComponent<AnimationHelper>();
			component.IgnoreGlobal = false;
		}
		else
		{
			this.EndSuper(true);
			this.player.stats.OnSuperEnd();
		}
		this.ball = (this.ball.Create(base.transform.position + Vector3.up * 100f) as PlayerSuperChaliceBounceBall);
		this.ball.player = this.player;
		this.ball.PlayerId = this.player.id;
		this.ball.velocity.x = (float)(this.player.motor.MoveDirection.x * 500);
		this.ball.Damage = this.DAMAGE;
		this.ball.DamageRate = this.DAMAGE_RATE;
		this.ball.super = this;
	}

	// Token: 0x040045DF RID: 17887
	[NonSerialized]
	public bool LAUNCHED_VERSION = WeaponProperties.LevelSuperChaliceBounce.launchedVersion;

	// Token: 0x040045E0 RID: 17888
	private float DAMAGE = WeaponProperties.LevelSuperChaliceBounce.damage;

	// Token: 0x040045E1 RID: 17889
	private float DAMAGE_RATE = WeaponProperties.LevelSuperChaliceBounce.damageRate;

	// Token: 0x040045E2 RID: 17890
	private float DURATION = WeaponProperties.LevelSuperChaliceBounce.duration;

	// Token: 0x040045E3 RID: 17891
	[SerializeField]
	private PlayerSuperChaliceBounceBall ball;

	// Token: 0x040045E4 RID: 17892
	public float timer;
}
