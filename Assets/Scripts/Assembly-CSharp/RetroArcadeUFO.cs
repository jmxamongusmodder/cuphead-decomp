using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Token: 0x02000763 RID: 1891
public class RetroArcadeUFO : RetroArcadeEnemy
{
	// Token: 0x06002935 RID: 10549 RVA: 0x001804E6 File Offset: 0x0017E8E6
	public void LevelInit(LevelProperties.RetroArcade properties)
	{
		this.properties = properties;
	}

	// Token: 0x06002936 RID: 10550 RVA: 0x001804F0 File Offset: 0x0017E8F0
	public void StartUFO()
	{
		base.gameObject.SetActive(true);
		this.p = this.properties.CurrentState.uFO;
		base.transform.SetPosition(new float?(0f), new float?(500f), null);
		base.MoveY(-200f, 500f);
		this.alien = this.alienPrefab.Create(this, this.p);
		this.mole = this.molePrefab.Create(this.p);
		this.turrets = new List<RetroArcadeUFOTurret>();
		for (int i = 0; i < this.p.turretCount; i++)
		{
			RetroArcadeUFOTurret item = this.turretPrefab.Create(this, this.p, (float)i / (float)this.p.turretCount);
			this.turrets.Add(item);
		}
		base.StartCoroutine(this.shoot_cr());
	}

	// Token: 0x06002937 RID: 10551 RVA: 0x001805E8 File Offset: 0x0017E9E8
	private IEnumerator shoot_cr()
	{
		for (;;)
		{
			float waitTime = this.p.shotRate.min * Mathf.Pow(this.p.shotRate.max / this.p.shotRate.min, 1f - this.alien.NormalizedHpRemaining);
			yield return CupheadTime.WaitForSeconds(this, waitTime);
			foreach (RetroArcadeUFOTurret retroArcadeUFOTurret in this.turrets)
			{
				retroArcadeUFOTurret.Shoot();
			}
		}
		yield break;
	}

	// Token: 0x06002938 RID: 10552 RVA: 0x00180604 File Offset: 0x0017EA04
	private IEnumerator moveOffscreen_cr()
	{
		base.MoveY(200f, 500f);
		while (this.movingY)
		{
			yield return null;
		}
		UnityEngine.Object.Destroy(base.gameObject);
		yield break;
	}

	// Token: 0x06002939 RID: 10553 RVA: 0x0018061F File Offset: 0x0017EA1F
	public void OnAlienDie()
	{
		this.StopAllCoroutines();
		base.StartCoroutine(this.moveOffscreen_cr());
		this.properties.DealDamageToNextNamedState();
		this.mole.OnWaveEnd();
	}

	// Token: 0x04003221 RID: 12833
	private const float OFFSCREEN_Y = 500f;

	// Token: 0x04003222 RID: 12834
	private const float ONSCREEN_Y = 300f;

	// Token: 0x04003223 RID: 12835
	private const float MOVE_Y_SPEED = 500f;

	// Token: 0x04003224 RID: 12836
	public const float WIDTH = 600f;

	// Token: 0x04003225 RID: 12837
	public const float HEIGHT = 300f;

	// Token: 0x04003226 RID: 12838
	public const float INNER_WIDTH = 500f;

	// Token: 0x04003227 RID: 12839
	public const float INNER_HEIGHT = 150f;

	// Token: 0x04003228 RID: 12840
	public const float INNER_TURNAROUND_X = 220f;

	// Token: 0x04003229 RID: 12841
	private LevelProperties.RetroArcade properties;

	// Token: 0x0400322A RID: 12842
	private LevelProperties.RetroArcade.UFO p;

	// Token: 0x0400322B RID: 12843
	[SerializeField]
	private RetroArcadeUFOTurret turretPrefab;

	// Token: 0x0400322C RID: 12844
	[SerializeField]
	private RetroArcadeUFOAlien alienPrefab;

	// Token: 0x0400322D RID: 12845
	[SerializeField]
	private RetroArcadeUFOMole molePrefab;

	// Token: 0x0400322E RID: 12846
	private RetroArcadeUFOAlien alien;

	// Token: 0x0400322F RID: 12847
	private List<RetroArcadeUFOTurret> turrets;

	// Token: 0x04003230 RID: 12848
	private RetroArcadeUFOMole mole;
}
