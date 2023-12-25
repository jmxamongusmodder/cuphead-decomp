using System;
using System.Collections;
using UnityEngine;

// Token: 0x02000792 RID: 1938
public class RumRunnersLevelMine : AbstractProjectile
{
	// Token: 0x170003F3 RID: 1011
	// (get) Token: 0x06002AF1 RID: 10993 RVA: 0x00190BF3 File Offset: 0x0018EFF3
	// (set) Token: 0x06002AF2 RID: 10994 RVA: 0x00190BFB File Offset: 0x0018EFFB
	public int xPos { get; private set; }

	// Token: 0x170003F4 RID: 1012
	// (get) Token: 0x06002AF3 RID: 10995 RVA: 0x00190C04 File Offset: 0x0018F004
	// (set) Token: 0x06002AF4 RID: 10996 RVA: 0x00190C0C File Offset: 0x0018F00C
	public int yPos { get; private set; }

	// Token: 0x170003F5 RID: 1013
	// (get) Token: 0x06002AF5 RID: 10997 RVA: 0x00190C15 File Offset: 0x0018F015
	// (set) Token: 0x06002AF6 RID: 10998 RVA: 0x00190C1D File Offset: 0x0018F01D
	public int endPhaseExplodePriority { get; private set; }

	// Token: 0x170003F6 RID: 1014
	// (get) Token: 0x06002AF7 RID: 10999 RVA: 0x00190C26 File Offset: 0x0018F026
	protected override float DestroyLifetime
	{
		get
		{
			return 0f;
		}
	}

	// Token: 0x06002AF8 RID: 11000 RVA: 0x00190C30 File Offset: 0x0018F030
	public RumRunnersLevelMine Init(Vector3 targetPos, LevelProperties.RumRunners.Mine properties, RumRunnersLevelSpider parent, int xPos, int yPos)
	{
		base.ResetLifetime();
		base.ResetDistance();
		base.transform.position = new Vector3(targetPos.x, 800f, -targetPos.y * 1E-05f);
		this.targetPos = targetPos;
		this.targetPos.z = -targetPos.y * 1E-05f;
		this.xPos = xPos;
		this.yPos = yPos;
		if (this.xPos == 3 && this.yPos == 2)
		{
			this.endPhaseExplodePriority = 0;
		}
		else if (this.xPos == 2)
		{
			this.endPhaseExplodePriority = 1;
		}
		else
		{
			this.endPhaseExplodePriority = 2;
		}
		this.properties = properties;
		base.animator.Play("Drop");
		base.GetComponent<SpriteRenderer>().enabled = true;
		this.parent = parent;
		base.StartCoroutine(this.lifetime_cr());
		this.MoveDown();
		this.webRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.explosionRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.smokeRenderer.transform.eulerAngles = new Vector3(0f, 0f, (float)UnityEngine.Random.Range(0, 360));
		this.webRenderer.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus(), 1f);
		this.explosionRenderer.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus(), 1f);
		this.smokeRenderer.transform.localScale = new Vector3((float)MathUtils.PlusOrMinus(), (float)MathUtils.PlusOrMinus(), 1f);
		if (yPos == 0)
		{
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_high");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_high");
		}
		else if (yPos == 1)
		{
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_mid");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_mid");
		}
		else
		{
			AudioManager.Play("sfx_dlc_rumrun_mine_drop_low");
			this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_drop_low");
		}
		return this;
	}

	// Token: 0x06002AF9 RID: 11001 RVA: 0x00190E7E File Offset: 0x0018F27E
	private void MoveDown()
	{
		base.StartCoroutine(this.move_down_cr());
	}

	// Token: 0x06002AFA RID: 11002 RVA: 0x00190E90 File Offset: 0x0018F290
	private IEnumerator move_down_cr()
	{
		base.transform.position = this.targetPos;
		yield return base.animator.WaitForAnimationToEnd(this, "Drop", false, true);
		base.StartCoroutine(this.check_distance_cr());
		yield break;
	}

	// Token: 0x06002AFB RID: 11003 RVA: 0x00190EAC File Offset: 0x0018F2AC
	private IEnumerator check_distance_cr()
	{
		this.damageDealer.SetDamage(this.properties.mineBossDamage);
		this.damageDealer.SetRate(0f);
		this.checkingPlayers = true;
		while (this.checkingPlayers)
		{
			if (this.parent && this.parent.moving && !this.parent.isSummoning && Vector3.Distance(this.parent.transform.position, base.transform.position) < this.properties.mineDistToExplode)
			{
				base.animator.Play((!this.parent.goingLeft) ? "SwingRight" : "SwingLeft");
			}
			LevelPlayerController player = PlayerManager.GetPlayer(PlayerId.PlayerOne) as LevelPlayerController;
			LevelPlayerController player2 = PlayerManager.GetPlayer(PlayerId.PlayerTwo) as LevelPlayerController;
			float player1Dist = Vector3.Distance(player.center, base.transform.position);
			if (!player.IsDead && player1Dist < this.properties.mineDistToExplode)
			{
				this.checkingPlayers = false;
			}
			if (player2 != null)
			{
				float num = Vector3.Distance(player2.center, base.transform.position);
				if (!player2.IsDead && num < this.properties.mineDistToExplode)
				{
					this.checkingPlayers = false;
				}
			}
			yield return null;
		}
		if (!this.exploding)
		{
			base.StartCoroutine(this.explosion_cr(false));
		}
		yield break;
	}

	// Token: 0x06002AFC RID: 11004 RVA: 0x00190EC8 File Offset: 0x0018F2C8
	private IEnumerator explosion_cr(bool timedOut)
	{
		this.exploding = true;
		base.animator.Play("PreExplode");
		AudioManager.Play("sfx_dlc_rumrun_mine_preexplode");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_preexplode");
		yield return CupheadTime.WaitForSeconds(this, this.properties.mineExplosionWarning * (float)((!timedOut) ? 1 : 2));
		AudioManager.Play("sfx_dlc_rumrun_mine_explode");
		this.emitAudioFromObject.Add("sfx_dlc_rumrun_mine_explode");
		base.animator.Play("Explode");
		yield break;
	}

	// Token: 0x06002AFD RID: 11005 RVA: 0x00190EEA File Offset: 0x0018F2EA
	protected override void OnCollisionEnemy(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002AFE RID: 11006 RVA: 0x00190F08 File Offset: 0x0018F308
	protected override void OnCollisionPlayer(GameObject hit, CollisionPhase phase)
	{
		base.OnCollisionPlayer(hit, phase);
		if (phase != CollisionPhase.Exit)
		{
			this.damageDealer.DealDamage(hit);
		}
	}

	// Token: 0x06002AFF RID: 11007 RVA: 0x00190F26 File Offset: 0x0018F326
	public void SetTimer(float t)
	{
		if (!this.exploding)
		{
			this.timer = t;
		}
	}

	// Token: 0x06002B00 RID: 11008 RVA: 0x00190F3C File Offset: 0x0018F33C
	private IEnumerator lifetime_cr()
	{
		this.timer = this.properties.mineTimer;
		while (this.timer > 0f)
		{
			this.timer -= CupheadTime.Delta;
			yield return null;
		}
		this.checkingPlayers = false;
		if (!this.exploding)
		{
			base.StartCoroutine(this.explosion_cr(true));
		}
		yield break;
	}

	// Token: 0x06002B01 RID: 11009 RVA: 0x00190F57 File Offset: 0x0018F357
	private void Death()
	{
		this.StopAllCoroutines();
		this.Recycle<RumRunnersLevelMine>();
	}

	// Token: 0x040033A6 RID: 13222
	private const float START_HEIGHT = 800f;

	// Token: 0x040033AA RID: 13226
	private LevelProperties.RumRunners.Mine properties;

	// Token: 0x040033AB RID: 13227
	private RumRunnersLevelSpider parent;

	// Token: 0x040033AC RID: 13228
	private Vector3 targetPos;

	// Token: 0x040033AD RID: 13229
	private bool checkingPlayers;

	// Token: 0x040033AE RID: 13230
	private bool exploding;

	// Token: 0x040033AF RID: 13231
	private float timer;

	// Token: 0x040033B0 RID: 13232
	[SerializeField]
	private GameObject webRenderer;

	// Token: 0x040033B1 RID: 13233
	[SerializeField]
	private GameObject explosionRenderer;

	// Token: 0x040033B2 RID: 13234
	[SerializeField]
	private GameObject smokeRenderer;
}
